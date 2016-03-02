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
using System.Data.SqlClient;
using System.Data.Entity;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MovieCalendarPage1.xaml
    /// </summary>
    public partial class MovieCalendarPage : Page, IDisposable
    {
        private ObservableCollection<MovieScheduleModel> movieScheduleItems;
        private int intColumnCount = 1;
        private int intMaxRowCount = 2;
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
            if (paradisoObjectManager.HasRights("REPRINT") || paradisoObjectManager.HasRights("VOID")
                || paradisoObjectManager.HasRights("SETTINGS"))
                TicketPanel.Visibility = Visibility.Visible;
            else
                TicketPanel.Visibility = Visibility.Hidden;

            if (paradisoObjectManager.HasRights("RESERVE"))
                Reserved.Visibility = Visibility.Visible;
            else
                Reserved.Visibility = Visibility.Hidden;
            Unreserved.Visibility = Visibility.Hidden;

            if (paradisoObjectManager.HasRights("ENDTELLERSESSION"))
                Teller.Visibility = Visibility.Visible;
            else
                Teller.Visibility = Visibility.Collapsed;

            if (paradisoObjectManager.HasRights("SETTINGS"))
                Settings.Visibility = Visibility.Visible;
            else
                Settings.Visibility = Visibility.Collapsed;

            Version.Text = ParadisoObjectManager.GetInstance().Version;
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
            try
            {
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
                                                     where msl.movies_schedule_id == _cinema.key && msl.status == 1
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
                                                         ratingdescription = msl.movies_schedule.movie.mtrcb.remarks,
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
                                RatingDescription = _movie_schedule_list.ratingdescription,
                                StartTime = _movie_schedule_list.starttime,
                                EndTime = _movie_schedule_list.endtime,
                                RunningTimeInSeconds = _movie_schedule_list.duration,
                                SeatType = _movie_schedule_list.seattype
                            };
                            if (_movie_schedule_list_item.EndTime < _movie_schedule_list_item.StartTime)
                                _movie_schedule_list_item.EndTime = _movie_schedule_list_item.EndTime.AddDays(1);


                            /**
                             reserved 
                             -can be set either by web or teller selection
                             -add timer on seat
                             -update periodically
                             */



                            var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                           where mslrs.movies_schedule_list_id == _movie_schedule_list.mslkey &&  mslrs.status != 2
                                           select mslrs.cinema_seat_id).Count();

                            int tmpreservedseats = 0;
                            if (_movie_schedule_list_item.SeatType == 1)
                            {
                                tmpreservedseats = (from mcths in context.movies_schedule_list_house_seat
                                                    where mcths.movies_schedule_list_id == _movie_schedule_list.mslkey 
                                                    && (mcths.notes == "RESERVED" || mcths.notes.StartsWith("RESERVED "))
                                                    select mcths.cinema_seat_id).Count();
                            }

                            int disabledseats = 0;
                            disabledseats = (from cs in context.cinema_seat where cs.cinema_id == _cinema.cinema_key && cs.is_disabled == 1 select cs.id).Count();

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

                            //new definitions
                            //reserved - used in reserving online or manul so notes with reserved
                            //tempreserved - different session 
                            //booked - already purchased  and is not voided
                            
                            _movie_schedule_list_item.Booked = (int) patrons;
                            _movie_schedule_list_item.Reserved = (int) (tmpreservedseats + disabledseats);
                            _movie_schedule_list_item.Available = (int)(capacity - patrons - reserved - disabledseats);
                            if (_movie_schedule_list_item.Available < 0)
                                _movie_schedule_list_item.Available = 0;
                            
                            var _patrons = context.ExecuteStoreQuery<Result1>("CALL retrieve_movies_schedule_list_patron_mslid_default({0}, {1});", _movie_schedule_list.mslkey, ParadisoObjectManager.GetInstance().ScreeningDate).ToList();
                            if (_patrons != null && _patrons.Count > 0)
                                _movie_schedule_list_item.Price = (decimal) _patrons[0].price;

                            if (_movie_schedule_list_item.Available <= 0 
                                && _movie_schedule_list_item.Reserved == 0 
                                && _movie_schedule_list_item.SeatType != 3) //except unlimited seating
                            {
                                _movie_schedule_list_item.IsEnabled = false;
                                _movie_schedule_list_item.HasStarted = true;
                            }
                            else
                            {
                                int _laytime = _movie_schedule_list.laytime;
                                if (_laytime == 0)
                                    _laytime = 30;
                                if (dtNow < _movie_schedule_list.starttime)
                                {
                                    _movie_schedule_list_item.IsEnabled = true;
                                    _movie_schedule_list_item.HasStarted = false;
                                    if (movieScheduleItem.SelectedMovieScheduleListItem == null)
                                        movieScheduleItem.SelectedMovieScheduleListItem = _movie_schedule_list_item;
                                }
                                else if (dtNow < _movie_schedule_list.starttime.AddMinutes(_laytime)) //_movie_schedule_list.endtime ) //allow already running
                                {
                                    _movie_schedule_list_item.IsEnabled = true;
                                    _movie_schedule_list_item.HasStarted = true;
                                }
                                else
                                {
                                    _movie_schedule_list_item.IsEnabled = false;
                                    _movie_schedule_list_item.HasStarted = true;
                                }
                            }

                            _movie_schedule_list_item.IsEllapsed = !_movie_schedule_list_item.IsEnabled;

                            if (ParadisoObjectManager.GetInstance().HasRights("PRIORDATE") && !_movie_schedule_list_item.IsEnabled)
                            {
                                _movie_schedule_list_item.IsEnabled = true;
                            }
                            _movie_schedule_list_item.Index = movieScheduleItem.MovieScheduleListItems.Count;

                            movieScheduleItem.MovieScheduleListItems.Add(_movie_schedule_list_item);

                        }

                        if (blnIsClear)
                        {

                            //checks if date 
                            if (movieScheduleItem.MovieScheduleListItems.Count > 0)
                            {
                                if (dtScreenDate < dtNow && movieScheduleItem.SelectedMovieScheduleListItem == null && ParadisoObjectManager.GetInstance().HasRights("PRIORDATE"))
                                {
                                    movieScheduleItem.SelectedMovieScheduleListItem = movieScheduleItem.MovieScheduleListItems[0];
                                }

                                movieScheduleItems.Add(movieScheduleItem);
                            }
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
                            {
                                movieScheduleItems.Add(movieScheduleItem);
                                lstKeys.Add(movieScheduleItem.Key);
                            }
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

                                //checks if date 
                                if (movieScheduleItem.MovieScheduleListItems.Count > 0)
                                {
                                    if (dtScreenDate < dtNow && movieScheduleItem.SelectedMovieScheduleListItem == null && ParadisoObjectManager.GetInstance().HasRights("PRIORDATE")
                                        )
                                    {
                                        movieScheduleItem.SelectedMovieScheduleListItem = movieScheduleItem.MovieScheduleListItems[0];
                                    }

                                    movieScheduleItems[intIndex] = movieScheduleItem;
                                    lstKeys.Add(movieScheduleItem.Key);
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ParadisoObjectManager.GetInstance().MessageBox(ex);
            }

            if (!blnIsClear)
            {
                //remove unwanted 
                int intCount1 = movieScheduleItems.Count - 1;
                for (int i = intCount1; i >= 0; i--)
                {
                    if (movieScheduleItems[i] != null && 
                        (lstKeys.IndexOf(movieScheduleItems[i].Key) == -1 || movieScheduleItems[i].MovieScheduleListItems.Count == 0))
                    {
                        movieScheduleItems.RemoveAt(i);
                    }
                }
            }

            /*
            if (movieScheduleItems.Count == 2)
                intMaxRowCount = 1;
            else if (movieScheduleItems.Count == 4)
                intMaxRowCount = 2;
            */
            //get maximum row count
            intMaxRowCount = movieScheduleItems.Count / 2;
            if (movieScheduleItems.Count % 2 != 0)
                intMaxRowCount++;


            //enable this code to fill up empty slot
            /*
            for (int k = movieScheduleItems.Count; k < intMaxRowCount*2; k++)
            {
                movieScheduleItems.Add( new MovieScheduleModel());
            }
            */

            intColumnCount = 1;

            if (movieScheduleItems.Count > intMaxRowCount)
                intColumnCount = 2;

            if (movieScheduleItems.Count > 0)
                MovieCalendarListBox.Visibility = System.Windows.Visibility.Visible;
            else
                MovieCalendarListBox.Visibility = System.Windows.Visibility.Hidden;
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

                //checks if date
                DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;
                if (dtScreenDate < dtNow && movieScheduleItem.SelectedMovieScheduleListItem == null && ParadisoObjectManager.GetInstance().HasRights("PRIORDATE"))
                {
                    movieScheduleItem.SelectedMovieScheduleListItem = movieScheduleItem.MovieScheduleListItems[0];
                }

            }
            else
            {
                movieScheduleItem.SelectedMovieScheduleListItem = null;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //MovieScheduleListItemsListView
            /*
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            */

            //double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            //double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (MainCanvas.ActualHeight < MovieCalendarListBox.ActualHeight ||
                MainCanvas.ActualWidth < MovieCalendarListBox.ActualWidth)
            {
                MainCanvas.Height = MovieCalendarListBox.ActualHeight;
                MainCanvas.Width = MovieCalendarListBox.ActualWidth;
            }

            //checks for terminated session
            if (ParadisoObjectManager.GetInstance().RunOnce)
            {
                ParadisoObjectManager.GetInstance().RunOnce = false;

                bool blnHasTerminatedSession = false;

                DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
                string strCurrentSessionId = ParadisoObjectManager.GetInstance().SessionId;
                try
                {
                    //MessageBox.Show(strCurrentSessionId);
                    using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                    {
                        var _mslhs = (from mslsh in context.movies_schedule_list_house_seat
                                      where mslsh.reserved_date.Value.Year == dtNow.Year &&
                                      mslsh.reserved_date.Value.Month == dtNow.Month &&
                                      mslsh.reserved_date.Value.Day == dtNow.Day
                                      && mslsh.notes == null && mslsh.full_name == null
                                      select new { id = mslsh.id, session_id = mslsh.session_id, reserved_date = mslsh.reserved_date }).ToList();
                        foreach (var m in _mslhs)
                        {
                            TimeSpan ts = dtNow - (DateTime) m.reserved_date;
                            if (ts.TotalMinutes <= 10)
                            {

                                if (m.session_id.EndsWith(string.Format("-{0}", ParadisoObjectManager.GetInstance().UserId)))
                                {
                                    blnHasTerminatedSession = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch //(Exception ex)
                {
                }

                if (blnHasTerminatedSession)
                {
                    if (ParadisoObjectManager.GetInstance().HasRights("VOID"))
                    {
                        MessageBox.Show("You have pending transactions for the day. Select the transactions to void.");
                        //open 
                        this.Dispose();

                        bool blnIsTicketFormatB = false;
                        if (ParadisoObjectManager.GetInstance().GetConfigValue("TICKET_FORMAT", "A") == "B")
                            blnIsTicketFormatB = true;
                        if (ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), "A") == "B")
                            blnIsTicketFormatB = true;

                        ParadisoObjectManager.GetInstance().RunOnce = true;
                        if (blnIsTicketFormatB)
                            NavigationService.GetNavigationService(this).Navigate(new TicketPrintPage2());
                        else
                            NavigationService.GetNavigationService(this).Navigate(new TicketPrintPage());

                    }
                    else
                    {
                        MessageBox.Show("You have pending transactions for the day. Please Call your administrator to cancel it.");
                    }
                }
            }
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
            //Button button = sender as Button;
            TextBlock button = sender as TextBlock;

            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is MovieScheduleListModel)
            {
                MovieScheduleListModel msli = (MovieScheduleListModel)dataContext;

                this.StopTimer();

                if (ParadisoObjectManager.GetInstance().IsReservedMode)
                {
                    //verify if msli is valid
                    if (msli.SeatType != 1)
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "Reservation can only be done on reserved seating.";
                        messageWindow.ShowDialog();
                        ParadisoObjectManager.GetInstance().CurrentMovieSchedule = null;

                        return;
                    }
                    else if (msli.IsEllapsed)
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "Reservation cannot be done on ellapsed screen times.";
                        messageWindow.ShowDialog();
                        ParadisoObjectManager.GetInstance().CurrentMovieSchedule = null;
                        return;
                    }

                    this.Dispose();

                    ParadisoObjectManager.GetInstance().CurrentMovieSchedule = new MovieScheduleListModel(msli);
                    NavigationService.GetNavigationService(this).Navigate(new ReservedSeatingPage(msli));
                }
                else //if (msli.SeatType == 1)
                {
                    this.Dispose();

                    ParadisoObjectManager.GetInstance().CurrentMovieSchedule = new MovieScheduleListModel(msli);
                    NavigationService.GetNavigationService(this).Navigate(new SeatingPage(msli));
                }
                /*
                else
                {
                    NavigationService.GetNavigationService(this).Navigate(new FreeSeatingPage(msli));
                }
                */
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            //Button button = sender as Button;
            TextBlock button = sender as TextBlock;
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
            //Button button = sender as Button;
            TextBlock button = sender as TextBlock;
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
            this.Dispose();

            bool blnIsTicketFormatB = false;
            if (ParadisoObjectManager.GetInstance().GetConfigValue("TICKET_FORMAT", "A") == "B")
                blnIsTicketFormatB = true;
            if (ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), "A") == "B")
                blnIsTicketFormatB = true;

            if (blnIsTicketFormatB)
                NavigationService.GetNavigationService(this).Navigate(new TicketPrintPage2());
            else
                NavigationService.GetNavigationService(this).Navigate(new TicketPrintPage());
        }

        private void Reserved_Click(object sender, RoutedEventArgs e)
        {
            ParadisoObjectManager.GetInstance().IsReservedMode = true;
            Reserved.Visibility = System.Windows.Visibility.Hidden;
            Unreserved.Visibility = System.Windows.Visibility.Visible;
        }

        private void Unreserved_Click(object sender, RoutedEventArgs e)
        {
            ParadisoObjectManager.GetInstance().IsReservedMode = false;
            Unreserved.Visibility = System.Windows.Visibility.Hidden;
            Reserved.Visibility = System.Windows.Visibility.Visible;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
            //checks grant
            
            NavigationService.GetNavigationService(this).Navigate(new SettingPage());
        }
        
        private void Teller_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();

            NavigationService.GetNavigationService(this).Navigate(new EndTellerSessionPage());
        }

        private void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= timer_Tick;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.StopTimer();
            if (movieScheduleItems.Count > 0)
            {
                foreach (var msi in movieScheduleItems)
                    msi.Dispose();
                movieScheduleItems.Clear();
            }
        }

        #endregion
    }
}
