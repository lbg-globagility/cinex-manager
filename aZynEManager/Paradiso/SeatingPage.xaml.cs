using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CinemaCustomControlLibrary;
using CommonLibrary;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SeatingPage.xaml
    /// </summary>
    public partial class SeatingPage : Page
    {

        public int MovieKey { get; set; }
        public int MovieTimeKey { get; set; }

        public List<CinemaSeat> CinemaSeats { get; set; }
        public List<CinemaScreen> CinemaScreens { get; set; }

        public List<int> SelectedCinemaSeats { get; set; }

        public SeatingPage(int intMovieKey, int intMovieTimeKey, List<int> selectedCinemaSeats)
        {
            InitializeComponent();

            MovieKey = intMovieKey;
            MovieTimeKey = intMovieTimeKey;

            //update 
            CinemaScreens = new List<CinemaScreen>();
            CinemaSeats = new List<CinemaSeat>();
            SelectedCinemaSeats = new List<int>();

            if (selectedCinemaSeats != null)
            {
                for (int i = 0; i < selectedCinemaSeats.Count; i++)
                    SelectedCinemaSeats.Add(selectedCinemaSeats[i]);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
            messageYesNoWindow.MessageText.Text = "Are you sure you want to cancel?";
            messageYesNoWindow.ShowDialog();
            if (!messageYesNoWindow.IsYes)
            //if (  .Show("Are you sure you want to cancel?", string.Empty, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }

        private void LoadValues()
        {
            //no databinding yet
            SeatSelected.Text = "0";
            SeatBooked.Text = "0";
            SeatAvailable.Text = "0";
            SeatCanvas.Children.Clear();

            //load values
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var movietimes = (from mct in context.movies_schedule_list
                                  where mct.id == this.MovieTimeKey
                                  select new
                                  {
                                      key = mct.id,
                                      movie_key = mct.movies_schedule.movie.id,
                                      movie_name = mct.movies_schedule.movie.title,
                                      cinema_key = mct.movies_schedule.cinema_id,
                                      mct.start_time,
                                      mct.end_time
                                  }).ToList();
                if (movietimes.Count > 0)
                {
                    MovieName.Text = movietimes[0].movie_name;
                    ScreenTime.Text = string.Format("{0:HH:mm} - {1:HH:mm}", movietimes[0].start_time, movietimes[0].end_time);
                    var maxprice = (from mctp in context.movies_schedule_list_patron
                                    where mctp.movies_schedule_list_id == this.MovieTimeKey
                                    select mctp.price).Max();
                    Price.Text = string.Format("Php {0:#,##0}", maxprice);

                    var CinemaKey = movietimes[0].cinema_key;

                    var capacity = (from c in context.cinemas
                                    where c.id == CinemaKey
                                    select c.capacity).FirstOrDefault();

                    //get paid
                    var patrons = (from mctp in context.movies_schedule_list_patron
                                   where mctp.movies_schedule_list_id == MovieTimeKey
                                   select mctp.id).Count();
                    //get booked
                    var bookings = (from mcths in context.movies_schedule_list_house_seat
                                    where mcths.movies_schedule_list_id == MovieTimeKey
                                    select mcths.cinema_seat_id).Count();

                    SeatSelected.Text = string.Format("{0}", SelectedCinemaSeats.Count);

                    SeatBooked.Text = string.Format("{0}", (int)bookings);

                    SeatAvailable.Text = string.Format("{0}", capacity - patrons - bookings - SelectedCinemaSeats.Count);


                    //load cinema seating and resize to fit

                    var seats = (from s in context.cinema_seat
                                 where s.cinema_id == CinemaKey && s.object_type == 2
                                 select s).ToList();
                    foreach (var seat in seats)
                    {
                        CinemaScreens.Add(new CinemaScreen()
                        {
                            Key = seat.id,
                            X1 = (int) seat.x1,
                            Y1 = (int) seat.y1,
                            X2 = (int) seat.x2,
                            Y2 = (int) seat.y2
                        });

                    }


                    double dblCX = 0.0;
                    double dblCY = 0.0;
                    double dblWidth = 24;
                    double dblHeight = 24;

                    var screens = (from s in context.cinema_seat
                                   where s.cinema_id == CinemaKey && s.object_type == 1
                                   select s).ToList();
                    foreach (var screen in screens)
                    {
                        dblCX = (int) screen.x1 + (( (int) screen.x2 - (int) screen.x1) / 2);
                        dblCY = (int) screen.y1 + (( (int) screen.y2 - (int) screen.y1) / 2);

                        //is handicapped
                        var isdisabled = (sbyte) screen.is_handicapped;

                        CinemaSeat.SeatType _seatType = CinemaSeat.SeatType.NormalAvailableSeatType;
                        if (isdisabled == 1)
                            _seatType = CinemaSeat.SeatType.HandicappedAvailableSeatType;

                        //checks if taken
                        var houseseats = (from mcths in context.movies_schedule_list_house_seat
                                          where mcths.movies_schedule_list_id == MovieTimeKey && mcths.cinema_seat_id == screen.id
                                          select mcths.cinema_seat_id).ToList();
                        if (houseseats.Count > 0)
                        {
                            if (isdisabled == 1)
                                _seatType = CinemaSeat.SeatType.HandicappedTakenSeatType;
                            else
                                _seatType = CinemaSeat.SeatType.NormalTakenSeatType;
                        }
                        else
                        {
                            //get booked
                            var reservedseats = (from mcths in context.movies_schedule_list_reserved_seat
                                                 where mcths.movies_schedule_list_id == MovieTimeKey && mcths.cinema_seat_id == screen.id
                                                 select mcths.cinema_seat_id).ToList();
                            if (reservedseats.Count > 0)
                            {
                                if (isdisabled == 1)
                                    _seatType = CinemaSeat.SeatType.HandicappedReservedSeatType;
                                else
                                    _seatType = CinemaSeat.SeatType.NormalReservedSeatType;
                            }
                        }

                        if (SelectedCinemaSeats.IndexOf(screen.id) != -1)
                        {
                            if (isdisabled == 1)
                                _seatType = CinemaSeat.SeatType.HandicappedLockedSeatType;
                            else
                                _seatType = CinemaSeat.SeatType.NormalLockedSeatType;
                        }

                        CinemaSeats.Add(new CinemaSeat()
                        {
                            Key = screen.id,
                            Name = string.Format("{0}{1}", screen.row_name, screen.col_name),
                            CX = dblCX,
                            CY = dblCY,
                            X1 = dblCX - (dblWidth / 2),
                            Y1 = dblCY - (dblHeight / 2),
                            X2 = dblCX + (dblWidth / 2),
                            Y2 = dblCY + (dblHeight / 2),
                            Type = _seatType,
                            Action = CinemaSeat.ActionType.TakenActionType
                        });
                    }


                    SeatCanvas.Children.Clear();

                    //grid lines

                    double dblMaxWidth = SeatCanvas.ActualWidth;
                    double dblMaxHeight = SeatCanvas.ActualHeight;

                    //screen
                    if (CinemaScreens.Count > 0)
                    {
                        ContentControl contentControl = new ContentControl()
                        {
                            Width = CinemaScreens[0].Width,
                            Height = CinemaScreens[0].Height,
                        };


                        Image image = new Image()
                        {
                            Width = CinemaScreens[0].Width,
                            Height = CinemaScreens[0].Height,
                            Source = new BitmapImage(new Uri(@"/CinemaCustomControlLibrary;component/Images/screen.png", UriKind.Relative)),
                            Stretch = Stretch.Fill,
                            IsHitTestVisible = true,
                        };

                        contentControl.Content = image;

                        SeatCanvas.Children.Add(contentControl);
                        Canvas.SetLeft(contentControl, CinemaScreens[0].X1);
                        Canvas.SetTop(contentControl, CinemaScreens[0].Y1);
                    }

                    //seats
                    if (CinemaSeats.Count > 0)
                    {


                        foreach (CinemaSeat cinemaSeat in CinemaSeats)
                        {
                            ContentControl contentControl = new ContentControl()
                            {
                                Width = dblWidth,
                                Height = dblHeight,
                                IsHitTestVisible = true,
                            };

                            //Style style = this.FindResource("DesignerItemStyle") as Style;
                            //contentControl.Style = style;

                            SeatControl seatControl = new SeatControl(cinemaSeat);
                            seatControl.SeatControlClicked += new EventHandler(SeatControl_SeatControlClicked);

                            contentControl.Content = seatControl;

                            SeatCanvas.Children.Add(contentControl);
                            Canvas.SetLeft(contentControl, cinemaSeat.X1);
                            Canvas.SetTop(contentControl, cinemaSeat.Y1);
                        }
                    }

                }

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadValues();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCinemaSeats.Count > 0)
            {
                MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
                messageYesNoWindow.MessageText.Text = "Are you sure you want to clear the selection?";
                messageYesNoWindow.ShowDialog();
                if (!messageYesNoWindow.IsYes)
                //if (MessageBox.Show("Are you sure you want to clear the selection?", string.Empty, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
            }

            SelectedCinemaSeats.Clear();
            this.LoadValues();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCinemaSeats.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You must select a seat to continue.";
                messageWindow.ShowDialog();
                return;
            }
            NavigationService.GetNavigationService(this).Navigate(new TicketingPage(this.MovieKey, this.MovieTimeKey, this.SelectedCinemaSeats));
            
        }


        private void SeatControl_SeatControlClicked(Object sender, EventArgs e)
        {
            CinemaSeatArgs csa = (CinemaSeatArgs)e;
            
            //update labels
            if (csa.SeatType == CinemaSeat.SeatType.NormalLockedSeatType ||
                csa.SeatType == CinemaSeat.SeatType.HandicappedLockedSeatType) //add seats
            {
                if (!SelectedCinemaSeats.Contains(csa.SeatKey))
                    SelectedCinemaSeats.Add(csa.SeatKey);
            }
            else if (csa.SeatType == CinemaSeat.SeatType.NormalAvailableSeatType ||
                csa.SeatType == CinemaSeat.SeatType.HandicappedAvailableSeatType) //remove seats 
            {
                if (SelectedCinemaSeats.Contains(csa.SeatKey))
                    SelectedCinemaSeats.Remove(csa.SeatKey);
            }


            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var cinemaKey = (from mct in context.movies_schedule_list
                                 where mct.id == this.MovieTimeKey
                                 select mct.movies_schedule.cinema_id
                                 ).First();

                var capacity = (from c in context.cinemas
                                where c.id == cinemaKey
                                select c.capacity).First();
                //get paid
                var patrons = (from mctp in context.movies_schedule_list_patron
                               where mctp.movies_schedule_list_id == MovieTimeKey
                               select mctp.id).Count();
                //get booked
                var bookings = (from mcths in context.movies_schedule_list_house_seat
                                where mcths.movies_schedule_list_id == MovieTimeKey
                                select mcths.cinema_seat_id).Count();

                SeatSelected.Text = string.Format("{0}", SelectedCinemaSeats.Count);
                
                SeatBooked.Text = string.Format("{0}", (int)bookings);

                SeatAvailable.Text = string.Format("{0}", capacity - patrons - bookings - SelectedCinemaSeats.Count);
            }

            
        }
    }
}
