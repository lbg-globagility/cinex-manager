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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for FreeSeatingPage.xaml
    /// </summary>
    public partial class FreeSeatingPage : Page
    {
        public int Key { get; set; }
        public MovieScheduleListModel MovieSchedule { get; set; }
        //public ObservableCollection<PatronQuantityModel> Patrons { get; set; }
        public PatronQuantityListModel PatronQuantities { get; set; }

        //just a list 

        DispatcherTimer timer;
        private bool blnIsUpdating = false;

        public FreeSeatingPage(int intKey)
        {
            InitializeComponent();

            Key = intKey;

            MovieSchedule = new MovieScheduleListModel();

            //2 limited
            //3 unlimited

            this.UpdateMovieSchedule();

            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 * Constants.ReservedSeatingUiInterval);
            timer.Tick += new EventHandler(timer_Tick);
            
            timer.Start();

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
                //checks if movie schedule exists
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

                    if (NavigationService != null)
                        NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

                //checks if movie already expired
                DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
                if (_movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime) < dtNow)
                {
                    blnIsUpdating = false;
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Movie Schedule is already expired.";
                    messageWindow.ShowDialog();

                    if (NavigationService != null)
                        NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

                string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                {
                    MovieSchedule.Key = _movie_schedule_list.mslkey;
                    MovieSchedule.CinemaKey = _movie_schedule_list.cinemakey;
                    MovieSchedule.MovieKey = _movie_schedule_list.moviekey;
                    MovieSchedule.MovieName = _movie_schedule_list.moviename;
                    MovieSchedule.RunningTimeInSeconds = _movie_schedule_list.duration;
                    MovieSchedule.Rating = _movie_schedule_list.rating;
                    MovieSchedule.StartTime = _movie_schedule_list.starttime;
                    MovieSchedule.EndTime = _movie_schedule_list.endtime;
                    MovieSchedule.SeatType = _movie_schedule_list.seattype;
                    MovieSchedule.LayTime = _movie_schedule_list.laytime;
                }

                //taken seats
                var takenseats = (from mslrs in context.movies_schedule_list_reserved_seat
                                  where mslrs.movies_schedule_list_id == this.Key
                                  select mslrs.cinema_seat_id).Count();


                //reserved seats from other sessions
                var reservedseats = (from mcths in context.movies_schedule_list_house_seat_free_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id != strSessionId
                                     select mcths.cinema_seat_id).Count();

                //selected seats 
                var selectedseats = (from mcths in context.movies_schedule_list_house_seat_free_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id == strSessionId
                                     select new
                                     {
                                         mcths.id,
                                         mcths.cinema_seat_id,
                                         mcths.movies_schedule_list_patron_id,
                                         mcths.reserved_date
                                     }).ToList();

                MovieSchedule.Selected = selectedseats.Count;
                MovieSchedule.Booked = (int) reservedseats;
                MovieSchedule.Available = (int)(_movie_schedule_list.capacity - takenseats - reservedseats - selectedseats.Count);

                /*
                var price = (from mslp in context.movies_schedule_list_patron
                             where mslp.movies_schedule_list_id == this.Key && mslp.is_default == 1
                             select mslp.price).FirstOrDefault();
                
                if (price != null)
                    MovieSchedule.Price = (decimal)price;
                */


                if (MovieSchedule.Available == 0 && MovieSchedule.SeatType != 3) //except unlimited seating
                {
                    blnIsUpdating = false;
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "No more available seats.";
                    messageWindow.ShowDialog();

                    if (NavigationService != null)
                        NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

            }
            
            if (PatronQuantities == null)
            {
                PatronQuantities = new PatronQuantityListModel();
                this.LoadPatronQuantities();
            }

            blnIsUpdating = false;

        }

        private void LoadPatronQuantities()
        {
            //load all patrons
            PatronQuantities.Patrons.Clear();

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                //Patrons = new ObservableCollection<PatronQuantityModel>();
                //Patrons.Add(null);
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
                        PatronQuantities.Patrons.Add(new PatronQuantityModel()
                        {
                            Key = _patron.key,
                            PatronKey = _patron.patronkey,
                            Code = _patron.patroncode,
                            Name = _patron.patronname,
                            Price = (decimal)_patron.price,
                            Quantity = 0,
                            MaxQuantity = MovieSchedule.Available
                            //MaxQuantity = capacity
                        });
                    }
                }
            }

            //PatronQuantities.Patrons[0].Quantity = 1;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void ClearSelection()
        {
            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var selectedseats = (from mslhsv in context.movies_schedule_list_house_seat_free_view
                                     where mslhsv.movies_schedule_list_id == MovieSchedule.Key && mslhsv.session_id == strSessionId
                                     select mslhsv.id).ToList();
                if (selectedseats != null)
                {
                    foreach (var sid in selectedseats)
                    {
                        var selectedseat = (from mslhs in context.movies_schedule_list_house_seat
                                            where mslhs.id == sid
                                            select mslhs).FirstOrDefault();
                        if (selectedseat != null)
                        {
                            context.movies_schedule_list_house_seat.DeleteObject(selectedseat);
                        }
                    }

                    context.SaveChanges();
                }

            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelection();
            this.LoadPatronQuantities();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelection();
            ParadisoObjectManager.GetInstance().SetNewSessionId();
            if (NavigationService != null)
                NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {

            timer.Stop();
            
            if (PatronQuantities.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No patron has been selected.";
                messageWindow.ShowDialog();
                timer.Start();
                return;
            }

            this.UpdateMovieSchedule();

            if (MovieSchedule.SeatType == 2) //limited 
            {
                if (PatronQuantities.Count > MovieSchedule.Available)
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Selected patron(s) exceeds available.";
                    messageWindow.ShowDialog();
                    timer.Start();
                    return;
                }
            }

            this.ClearSelection();

            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var screenKey = (from cs in context.cinema_seat where cs.cinema_id == MovieSchedule.CinemaKey && cs.object_type == 2 select cs.id).SingleOrDefault();
                if (screenKey == null)
                    screenKey = 0;

                foreach (var patronQuantity in PatronQuantities.Patrons)
                {
                    for (int i = 0; i < patronQuantity.Quantity; i++)
                    {

                        movies_schedule_list_house_seat mslhs = new movies_schedule_list_house_seat()
                        {
                            movies_schedule_list_id = MovieSchedule.Key,
                            cinema_seat_id = screenKey,
                            reserved_date = ParadisoObjectManager.GetInstance().CurrentDate,
                            session_id = strSessionId,
                            movies_schedule_list_patron_id = patronQuantity.Key
                        };
                        context.movies_schedule_list_house_seat.AddObject(mslhs);
                    }
                }
                context.SaveChanges();
            }

            //call payment
            if (NavigationService != null) 
                NavigationService.Navigate(new Uri("TenderAmountPage.xaml", UriKind.Relative));
        }

    }
}
