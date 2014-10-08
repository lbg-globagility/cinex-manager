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
using System.Collections.ObjectModel;
using Paradiso.Model;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MovieCalendarPage1.xaml
    /// </summary>
    public partial class MovieCalendarPage : Page
    {
        private ObservableCollection<MovieScheduleModel> movieScheduleItems;
        private int intColumnCount = 1;
        private int intMaxRowCount = 4;
        DispatcherTimer timer;

        public MovieCalendarPage()
        {
            InitializeComponent();

            movieScheduleItems = new ObservableCollection<MovieScheduleModel>();

            //DateTime dtScreenDate = new DateTime(2006, 12, 1);
            //DateTime dtScreenDate = new DateTime(2007, 1, 6);
            //ParadisoObjectManager.GetInstance().ScreeningDate = dtScreenDate;

            this.LoadMovieSchedules();

            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 * Constants.MovieScheduleUiInterval);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();
            if (paradisoObjectManager.HasRights("REPRINT") || paradisoObjectManager.HasRights("VOID"))
                TicketPanel.Visibility = Visibility.Visible;
            else
                TicketPanel.Visibility = Visibility.Hidden;
        }

        public int ColumnCount
        {
            get { return intColumnCount; }
        }

        public ObservableCollection<MovieScheduleModel> MovieSchedules
        {
            get { return movieScheduleItems; }
        }

        //will not attempt to clear but update
        private void UpdateMovieSchedules(bool blnIsClear)
        {
            DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
            DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;

            List<int> lstKeys = new List<int>();

            if (blnIsClear)
                movieScheduleItems.Clear();

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                //get all cinemas
                var _cinemas = (from ms in context.movies_schedule
                                where ms.movie_date == dtScreenDate
                                select new
                                {
                                    key = ms.id, //comment
                                    cinema_key = ms.cinema_id,
                                    number = ms.cinema.in_order,
                                    capacity = ms.cinema.capacity
                                }).Distinct().ToList();
                foreach (var _cinema in _cinemas)
                {
                    var movieScheduleItem = new MovieScheduleModel()
                    {
                        //Key = _cinema.cinema_key,
                        Key = _cinema.key,
                        CurrentDate = dtNow,
                        Number = Convert.ToInt32(_cinema.number),
                    };

                    int capacity = (int)_cinema.capacity;

                    var _movie_schedule_lists = (from msl in context.movies_schedule_list
                                                 where msl.movies_schedule_id == _cinema.key
                                                 //join ms in context.movies_schedule on msl.movies_schedule_id equals ms.id
                                                 //   where ms.cinema_id == _cinema.cinema_key && ms.movie_date == dtScreenDate 
                                                 orderby msl.start_time
                                                 select new
                                                 {
                                                     mslkey = msl.id,
                                                     moviekey = msl.movies_schedule.movie_id,
                                                     moviename = msl.movies_schedule.movie.title,
                                                     duration = msl.movies_schedule.movie.duration,
                                                     rating = msl.movies_schedule.movie.mtrcb.name,
                                                     starttime = msl.start_time,
                                                     endtime = msl.end_time,
                                                     seattype = msl.seat_type,
                                                     laytime = msl.laytime
                                                 }).ToList();

                    movieScheduleItem.MovieScheduleListItems.Clear();
                    movieScheduleItem.SelectedMovieScheduleListItem = null;
                    foreach (var _movie_schedule_list in _movie_schedule_lists)
                    {

                        var _movie_schedule_list_item = new MovieScheduleListModel()
                        {
                            //CinemaKey = _cinema.cinema_key,
                            CinemaKey = _cinema.key,
                            Key = _movie_schedule_list.mslkey,
                            MovieKey = _movie_schedule_list.moviekey,
                            MovieName = _movie_schedule_list.moviename.ToUpper(),
                            Rating = _movie_schedule_list.rating,
                            StartTime = _movie_schedule_list.starttime,
                            EndTime = _movie_schedule_list.endtime,
                            RunningTimeInSeconds = _movie_schedule_list.duration,
                            SeatType = _movie_schedule_list.seattype
                        };

                        /**
                         reserved 
                         -can be set either by web or teller selection
                         -add timer on seat
                         -update periodically
                         */

                        var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                       where mslrs.movies_schedule_list_id == _movie_schedule_list.mslkey
                                       select mslrs.cinema_seat_id).Count();

                        //reserved
                        var reserved = 0;
                        if (_movie_schedule_list_item.SeatType == 1) //reserved
                        {
                            reserved = (from mcths in context.movies_schedule_list_house_seat_view
                                        where mcths.movies_schedule_list_id == _movie_schedule_list.mslkey
                                        select mcths.cinema_seat_id).Count();
                        }
                        else
                        {
                            reserved = (from mcths in context.movies_schedule_list_house_seat_free_view
                                        where mcths.movies_schedule_list_id == _movie_schedule_list.mslkey
                                        select mcths.cinema_seat_id).Count();
                        }

                        _movie_schedule_list_item.Booked = (int)reserved;
                        _movie_schedule_list_item.Available = (int)(capacity - patrons - reserved);


                        var price = (from mslp in context.movies_schedule_list_patron
                                     where mslp.movies_schedule_list_id == _movie_schedule_list.mslkey && mslp.is_default == 1
                                     select mslp.price).FirstOrDefault();
                        if (price != null)
                            _movie_schedule_list_item.Price =  (decimal) price;


                        if (_movie_schedule_list_item.Available == 0 && _movie_schedule_list_item.SeatType != 3) //except unlimited seating
                        {
                            _movie_schedule_list_item.IsEnabled = false;
                        }
                        else
                        {
                            if (dtNow < _movie_schedule_list.starttime)
                            {
                                _movie_schedule_list_item.IsEnabled = true;
                                if (movieScheduleItem.SelectedMovieScheduleListItem == null)
                                    movieScheduleItem.SelectedMovieScheduleListItem = _movie_schedule_list_item;
                            }
                            else if (dtNow < _movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime)) //_movie_schedule_list.endtime ) //allow already running
                            {
                                _movie_schedule_list_item.IsEnabled = true;
                            }
                            else
                            {
                                _movie_schedule_list_item.IsEnabled = false;
                            }
                        }

                        movieScheduleItem.MovieScheduleListItems.Add(_movie_schedule_list_item);

                    }

                    if (blnIsClear)
                    {
                        movieScheduleItems.Add(movieScheduleItem);
                    }
                    else
                    {

                        int intCount = movieScheduleItems.Count;
                        int intIndex = -1;
                        for (int i = 0; i < intCount; i++)
                        {
                            if (movieScheduleItems[i].Key == movieScheduleItem.Key)
                            {
                                intIndex = i;
                                break;
                            }
                        }

                        if (intIndex == -1)
                            movieScheduleItems.Add(movieScheduleItem);
                        else
                        {
                            //determines if currently selected item is valid
                            if (movieScheduleItems[intIndex].SelectedMovieScheduleListItem != null)
                            {
                                if (movieScheduleItem.MovieScheduleListItems.Count > 0)
                                {
                                    foreach (MovieScheduleListModel _msli in movieScheduleItem.MovieScheduleListItems)
                                    {
                                        if (_msli.Key == movieScheduleItems[intIndex].SelectedMovieScheduleListItem.Key)
                                        {
                                            if (_msli.IsEnabled)
                                                movieScheduleItem.SelectedMovieScheduleListItem = _msli;
                                            break;
                                        }
                                    }
                                }
                            }

                            movieScheduleItems[intIndex] = movieScheduleItem;
                            lstKeys.Add(movieScheduleItem.Key);
                        }
                    }

                }

            }

            if (!blnIsClear)
            {
                //remove unwanted 
                int intCount1 = movieScheduleItems.Count - 1;
                for (int i = intCount1; i >= 0; i--)
                {
                    if (movieScheduleItems[i] != null && lstKeys.IndexOf(movieScheduleItems[i].Key) == -1)
                    {
                        movieScheduleItems.RemoveAt(i);
                    }
                }
            }

            for (int k = movieScheduleItems.Count; k < 8; k++)
            {
                movieScheduleItems.Add( new MovieScheduleModel());
            }

            intColumnCount = 1;

            if (movieScheduleItems.Count > intMaxRowCount)
                intColumnCount = 2;

        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateMovieSchedules(false);
        }

        private void LoadMovieSchedules()
        {
            this.UpdateMovieSchedules(true);
        }


        private void SetNextMovieScheduleListItem(MovieScheduleModel movieScheduleItem)
        {

            DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
            if (movieScheduleItem.MovieScheduleListItems.Count > 0)
            {
                int intMovieScheduleListItemIndex = -1;
                int intCount = movieScheduleItem.MovieScheduleListItems.Count;
                for (int i = 0; i <  intCount; i++)
                {
                    MovieScheduleListModel msli = movieScheduleItem.MovieScheduleListItems[i];
                    if (dtNow < msli.StartTime)
                    {
                        intMovieScheduleListItemIndex = i;
                        break; 
                    }
                }

                if (intMovieScheduleListItemIndex == -1)
                    movieScheduleItem.SelectedMovieScheduleListItem = null;
                else
                    movieScheduleItem.SelectedMovieScheduleListItem = movieScheduleItem.MovieScheduleListItems[intMovieScheduleListItemIndex];
            }
            else
            {
                movieScheduleItem.SelectedMovieScheduleListItem = null;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            MovieCalendarListBox.Width = MainCanvas.ActualWidth;

            /*
            double left = (MainCanvas.ActualWidth - MovieCalendarListBox.ActualWidth) / 2;
            if (left < 0)
                left = 0;
            */
            double left = 0;

            Canvas.SetLeft(MovieCalendarListBox, left);

            double top = (MainCanvas.ActualHeight - MovieCalendarListBox.ActualHeight) / 4;
            if (top < 0)
                top = 0;
            Canvas.SetTop(MovieCalendarListBox, top);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is MovieScheduleListModel)
            {
                MovieScheduleListModel msli = (MovieScheduleListModel)dataContext;
                timer.Stop();
                NavigationService.GetNavigationService(this).Navigate(new SeatingPage(msli));

                /*
                if (msli.SeatType == 1) //reserved seating
                    NavigationService.GetNavigationService(this).Navigate(new SeatingPage(msli));
                else
                    NavigationService.GetNavigationService(this).Navigate(new FreeSeatingPage(msli.Key));
                */

                /*
                if (msli.SeatType == 1) //reserved seating
                    NavigationService.GetNavigationService(this).Navigate(new ReservedSeatingPage(msli.Key));
                else
                    NavigationService.GetNavigationService(this).Navigate(new FreeSeatingPage(msli.Key));
                //NavigationService.GetNavigationService(this).Navigate(new ReservedSeatingPage(msli.Key));
                */
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is MovieScheduleListModel)
            {
                MovieScheduleListModel msli = (MovieScheduleListModel)dataContext;

                //lookup cinemakey
                foreach (MovieScheduleModel msi in movieScheduleItems)
                {
                    if (msli.CinemaKey == msi.Key)
                    {
                        msi.SelectedMovieScheduleListItem = msli;
                        break;
                    }
                }
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is MovieScheduleListModel)
            {
                MovieScheduleListModel msli = (MovieScheduleListModel)dataContext;
                //lookup cinemakey
                foreach (MovieScheduleModel msi in movieScheduleItems)
                {
                    if (msi != null && msli.CinemaKey == msi.Key)
                    {
                        this.SetNextMovieScheduleListItem(msi);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            NavigationService.GetNavigationService(this).Navigate(new TicketPrintPage());
        }
    }
}
