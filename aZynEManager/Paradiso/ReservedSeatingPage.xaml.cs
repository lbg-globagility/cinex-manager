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
using Paradiso.Model;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using CinemaCustomControlLibrary;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SeatingPage1.xaml
    /// </summary>
    public partial class ReservedSeatingPage : Page
    {
        public int Key { get; set; }
        public MovieScheduleListModel MovieSchedule { get; set; }
        
        public ObservableCollection<PatronModel> Patrons { get; set; }
        public ObservableCollection<SeatModel> Seats { get; set; }

        DispatcherTimer timer;
        private bool blnIsUpdating = false;

        public ReservedSeatingPage(int intKey) 
        {
            InitializeComponent();

            Key = intKey;
            Seats = new ObservableCollection<SeatModel>();
 
            this.UpdateMovieSchedule();
            
            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 * Constants.ReservedSeatingUiInterval);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MovieCalendarPage1.xaml", UriKind.Relative));
        }


        void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateMovieSchedule();
        }

        private void UpdateMovieSchedule()
        {
            blnIsUpdating = true;
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var _movie_schedule_list = (from msl in context.movies_schedule_list
                                                where msl.id == this.Key
                                                select new
                                                {
                                                    mslkey = msl.id,
                                                    cinemakey = msl.movies_schedule.cinema_id,
                                                    moviekey = msl.movies_schedule.movie_id,
                                                    moviename = msl.movies_schedule.movie.title,
                                                    duration = msl.movies_schedule.movie.duration,
                                                    rating = msl.movies_schedule.movie.mtrcb.name,
                                                    starttime = msl.start_time,
                                                    endtime = msl.end_time,
                                                    seattype = msl.seat_type,
                                                    laytime = msl.laytime,
                                                    capacity = msl.movies_schedule.cinema.capacity
                                                }).FirstOrDefault();
                if (_movie_schedule_list == null)
                {
                    blnIsUpdating = false;
                    //prompt error message
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Movie Schedule is invalid.";
                    messageWindow.ShowDialog();

                    NavigationService.Navigate(new Uri("MovieCalendarPage1.xaml", UriKind.Relative));
                    return;
                }
                else
                {
                    MovieScheduleListModel _movie_schedule_list_item = new MovieScheduleListModel()
                    {
                        Key = _movie_schedule_list.mslkey,
                        CinemaKey = _movie_schedule_list.cinemakey,
                        MovieKey = _movie_schedule_list.moviekey,
                        MovieName = _movie_schedule_list.moviename,
                        RunningTimeInSeconds = _movie_schedule_list.duration,
                        Rating = _movie_schedule_list.rating,
                        StartTime = _movie_schedule_list.starttime,
                        EndTime = _movie_schedule_list.endtime,
                        SeatType = _movie_schedule_list.seattype,
                        LayTime = _movie_schedule_list.laytime
                    };

                    //load 
                    var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.movies_schedule_list_id == this.Key
                                   select mslrs.cinema_seat_id).Count();

                    //reserved
                    var reserved = (from mcths in context.movies_schedule_list_house_seat_view
                                    where mcths.movies_schedule_list_id == this.Key
                                    select mcths.cinema_seat_id).Count();

                    _movie_schedule_list_item.Booked = (int)reserved;
                    _movie_schedule_list_item.Available = (int)(_movie_schedule_list.capacity - patrons - reserved);

                    var price = (from mslp in context.movies_schedule_list_patron
                                 where mslp.movies_schedule_list_id == this.Key && mslp.is_default == 1
                                 select mslp.price).FirstOrDefault();
                    if (price != null)
                        _movie_schedule_list_item.Price = (decimal)price;


                    if (_movie_schedule_list_item.Available == 0 && _movie_schedule_list_item.SeatType != 3) //except unlimited seating
                    {
                        _movie_schedule_list_item.IsEnabled = false;
                    }
                    else
                    {
                        DateTime dtNow = ParadisoObjectManager.GetInstance().ActualCurrentDate;

                        if (dtNow < _movie_schedule_list.starttime)
                        {
                            _movie_schedule_list_item.IsEnabled = true;
                        }
                        else if (dtNow < _movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime))
                        {
                            _movie_schedule_list_item.IsEnabled = true;
                        }
                        else
                        {
                            _movie_schedule_list_item.IsEnabled = false;
                        }
                    }

                    MovieSchedule = _movie_schedule_list_item;
                }

                //load all patrons
                Patrons = new ObservableCollection<PatronModel>();
                Patrons.Add(null);
                var _patrons = (from mslp in context.movies_schedule_list_patron
                                where mslp.movies_schedule_list_id == this.Key
                                select new
                                {
                                    key = mslp.id,
                                    patronkey = mslp.patron_id,
                                    patroncode = mslp.patron.code,
                                    patronname = mslp.patron.name,
                                    price = mslp.price
                                }).ToList();
                if (_patrons != null)
                {
                    foreach (var _patron in _patrons)
                    {
                        Patrons.Add(new PatronModel()
                        {
                            Key = _patron.key,
                            PatronKey = _patron.patronkey,
                            Code = _patron.patroncode,
                            Name = _patron.patronname,
                            Price = (decimal) _patron.price
                        });
                    }
                }

                //TODO add scrollbar and set maximum height and width, get maximum width to center 
                Seats.Clear();

                //taken seats
                var takenseats = (from mslrs in context.movies_schedule_list_reserved_seat
                                  where mslrs.movies_schedule_list_id == this.Key
                                  select mslrs.cinema_seat_id).ToList();

                //selected seats
                var selectedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                     where mcths.movies_schedule_list_id == this.Key
                                     select mcths.cinema_seat_id).ToList();


                var seats = (from s in context.cinema_seat
                                where s.cinema_id == MovieSchedule.CinemaKey
                                select s).ToList();
                foreach (var seat in seats)
                {
                    SeatModel seatModel = new SeatModel()
                    {
                        Key = seat.id,
                        Name = string.Format("{0}{1}", seat.row_name, seat.col_name),
                        X = (int)seat.x1,
                        Y = (int)seat.y1,
                        Width = (int)seat.x2 - (int) seat.x1,
                        Height = (int)seat.y2 - (int)seat.y1,
                        Type = (int) seat.object_type,
                        SeatType = 1
                    };

                    //get seat type
                    if (takenseats.Count > 0)
                    {
                        if (takenseats.IndexOf(seat.id) != -1)
                            seatModel.SeatType = 3;
                    }
                    if (selectedseats.Count > 0 && seatModel.SeatType == 1)
                    {
                        if (selectedseats.IndexOf(seat.id) != -1)
                            seatModel.SeatType = 2;
                    }
                    
                    //TODO add timer for reserved (least priority)


                    Seats.Add(seatModel);
                }
                
            }
            blnIsUpdating = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SeatIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (blnIsUpdating)
                return;
            timer.Stop();

            bool blnHasChanges = false;
            try
            {
                SeatModel seatModel = (SeatModel)((Canvas)sender).DataContext;
                if (seatModel.SeatType == 1 || seatModel.SeatType == 2) //available
                {
                    if (seatModel.SeatType == 2) //get patron
                    {
                    }

                    PatronWindow patronWindow = new PatronWindow(seatModel, Patrons, null);
                    patronWindow.Owner = Window.GetWindow(this);
                    patronWindow.ShowDialog();
                }
            }
            catch { }

            if (blnHasChanges)
                this.UpdateMovieSchedule();
            timer.Start();
        }
    }
}
