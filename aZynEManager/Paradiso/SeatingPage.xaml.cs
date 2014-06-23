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
using Cinemapps;
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
            //using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParidisoModel")))
            using (var context = new paradisoEntities())
            {
                var movietimes = (from mct in context.movie_calendar_times
                                  where mct.key == this.MovieTimeKey
                                  select new
                                  {
                                      mct.key,
                                      movie_key = mct.movie_calendar.movie_database.key,
                                      movie_name = mct.movie_calendar.movie_database.name,
                                      cinema_key = mct.movie_calendar.cinema_key,
                                      mct.start_time,
                                      mct.end_time
                                  }).ToList();
                if (movietimes.Count > 0)
                {
                    MovieName.Text = movietimes[0].movie_name;
                    ScreenTime.Text = string.Format("{0:HH:mm} - {1:HH:mm}", movietimes[0].start_time, movietimes[0].end_time);
                    var maxprice = (from mctp in context.movie_calendar_time_patrons
                                    where mctp.movie_calendar_time_key == this.MovieTimeKey
                                    select mctp.price).Max();
                    Price.Text = string.Format("Php {0:#,##0}", maxprice);

                    var CinemaKey = movietimes[0].cinema_key;

                    var capacity = (from c in context.cinemas
                                    where c.key == CinemaKey
                                    select c.capacity).FirstOrDefault();

                    //get paid
                    var patrons = (from mctp in context.movie_calendar_time_patrons
                                   where mctp.movie_calendar_time_key == MovieTimeKey
                                   select mctp.key).Count();
                    //get booked
                    var bookings = (from mcths in context.movie_calendar_time_house_seats
                                    where mcths.movie_calendar_time_key == MovieTimeKey
                                    select mcths.cinema_seat_key).Count();

                    SeatSelected.Text = string.Format("{0}", SelectedCinemaSeats.Count);

                    SeatBooked.Text = string.Format("{0}", (int)bookings);

                    SeatAvailable.Text = string.Format("{0}", capacity - patrons - bookings - SelectedCinemaSeats.Count);


                    //load cinema seating and resize to fit

                    var seats = (from s in context.cinema_seats
                                 where s.cinema_key == CinemaKey && s.object_type == 2
                                 select s).ToList();
                    foreach (var seat in seats)
                    {
                        CinemaScreens.Add(new CinemaScreen()
                        {
                            Key = seat.key,
                            X1 = seat.p1x,
                            Y1 = seat.p1y,
                            X2 = seat.p3x,
                            Y2 = seat.p3y
                        });

                    }


                    double dblCX = 0.0;
                    double dblCY = 0.0;
                    double dblWidth = 24;
                    double dblHeight = 24;

                    var screens = (from s in context.cinema_seats
                                   where s.cinema_key == CinemaKey && s.object_type == 1
                                   select s).ToList();
                    foreach (var screen in screens)
                    {
                        dblCX = screen.p1x + ((screen.p2x - screen.p1x) / 2);
                        dblCY = screen.p1y + ((screen.p2y - screen.p1y) / 2);

                        //is handicapped
                        var isdisabled = screen.handicapped;

                        CinemaSeat.SeatType _seatType = CinemaSeat.SeatType.NormalAvailableSeatType;
                        if (isdisabled)
                            _seatType = CinemaSeat.SeatType.HandicappedAvailableSeatType;

                        //checks if taken
                        var houseseats = (from mcths in context.movie_calendar_time_house_seats
                                          where mcths.movie_calendar_time_key == MovieTimeKey && mcths.cinema_seat_key == screen.key
                                          select mcths.cinema_seat_key).ToList();
                        if (houseseats.Count > 0)
                        {
                            if (isdisabled)
                                _seatType = CinemaSeat.SeatType.HandicappedTakenSeatType;
                            else
                                _seatType = CinemaSeat.SeatType.NormalTakenSeatType;
                        }
                        else
                        {
                            //get booked
                            var reservedseats = (from mcths in context.movie_calendar_time_house_seats
                                                 where mcths.movie_calendar_time_key == MovieTimeKey && mcths.cinema_seat_key == screen.key
                                                 select mcths.cinema_seat_key).ToList();
                            if (reservedseats.Count > 0)
                            {
                                if (isdisabled)
                                    _seatType = CinemaSeat.SeatType.HandicappedReservedSeatType;
                                else
                                    _seatType = CinemaSeat.SeatType.NormalReservedSeatType;
                            }
                        }

                        if (SelectedCinemaSeats.IndexOf(screen.key) != -1)
                        {
                            if (isdisabled)
                                _seatType = CinemaSeat.SeatType.HandicappedLockedSeatType;
                            else
                                _seatType = CinemaSeat.SeatType.NormalLockedSeatType;
                        }

                        CinemaSeats.Add(new CinemaSeat()
                        {
                            Key = screen.key,
                            Name = string.Format("{0}{1}", screen.row, screen.column),
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
                            Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/screen.png", UriKind.Relative)),
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


            //using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParidisoModel")))
            using (var context = new paradisoEntities())
            {
                var cinemaKey = (from mct in context.movie_calendar_times
                                 where mct.key == this.MovieTimeKey
                                 select mct.movie_calendar.cinema_key
                                 ).First();

                var capacity = (from c in context.cinemas
                                where c.key == cinemaKey
                                select c.capacity).First();
                //get paid
                var patrons = (from mctp in context.movie_calendar_time_patrons
                               where mctp.movie_calendar_time_key == MovieTimeKey
                               select mctp.key).Count();
                //get booked
                var bookings = (from mcths in context.movie_calendar_time_house_seats
                                where mcths.movie_calendar_time_key == MovieTimeKey
                                select mcths.cinema_seat_key).Count();

                SeatSelected.Text = string.Format("{0}", SelectedCinemaSeats.Count);
                
                SeatBooked.Text = string.Format("{0}", (int)bookings);

                SeatAvailable.Text = string.Format("{0}", capacity - patrons - bookings - SelectedCinemaSeats.Count);
            }

            
        }
    }
}
