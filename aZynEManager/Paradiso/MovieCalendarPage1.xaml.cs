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
using System.Threading;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MovieCalendarPage1.xaml
    /// </summary>
    public partial class MovieCalendarPage1 : Page
    {
        private ObservableCollection<MovieScheduleItem> movieScheduleItems;
        private int intColumnCount = 1;
        private int intMaxRowCount = 4;

        public MovieCalendarPage1()
        {
            InitializeComponent();

            movieScheduleItems = new ObservableCollection<MovieScheduleItem>();

            //DateTime dtScreenDate = new DateTime(2006, 12, 1);
            DateTime dtScreenDate = new DateTime(2007, 1, 6);
            ParadisoObjectManager.GetInstance().ScreeningDate = dtScreenDate;

            this.LoadMovieSchedules();

            this.DataContext = this;
        }

        public int ColumnCount
        {
            get { return intColumnCount; }
        }

        public ObservableCollection<MovieScheduleItem> MovieSchedules
        {
            get { return movieScheduleItems; }
        }

        private void LoadMovieSchedules()
        {
            DateTime dtNow = this.CurrentDate;
            DateTime dtScreenDate = this.ScreenDate;

            movieScheduleItems.Clear();

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                //get all cinemas
                var _cinemas = (from ms in context.movies_schedule
                                where ms.movie_date == dtScreenDate
                                orderby ms.cinema.in_order
                                select new
                                {
                                    cinema_key = ms.cinema_id,
                                    number = ms.cinema.in_order,
                                    capacity = ms.cinema.capacity
                                }).Distinct().ToList();
                foreach (var _cinema in _cinemas)
                {
                    var movieScheduleItem = new MovieScheduleItem()
                    {
                        Key = _cinema.cinema_key,
                        CurrentDate = dtNow,
                        Number = Convert.ToInt32(_cinema.number),
                    };

                    int capacity = (int)_cinema.capacity;

                    var _movie_schedule_lists = (from msl in context.movies_schedule_list 
                                                 join ms in context.movies_schedule on msl.movies_schedule_id equals ms.id
                                                    where ms.cinema_id == _cinema.cinema_key && ms.movie_date == dtScreenDate 
                                                    orderby msl.start_time
                                                    select new 
                                                    {
                                                        mslkey = msl.id,
                                                        moviekey = msl.movies_schedule.movie_id,
                                                        moviename = msl.movies_schedule.movie.title,
                                                        duration = msl.movies_schedule.movie.duration,
                                                        rating = msl.movies_schedule.movie.mtrcb.name,
                                                        starttime = msl.start_time,
                                                        endtime = msl.end_time
                                                    }).ToList();

                    movieScheduleItem.MovieScheduleListItems.Clear();
                    movieScheduleItem.SelectedMovieScheduleListItem = null;
                    foreach (var _movie_schedule_list in _movie_schedule_lists)
                    {
                        movieScheduleItem.MovieScheduleListItems.Add(new MovieScheduleListItem()
                        {
                            CinemaKey = _cinema.cinema_key,
                            Key = _movie_schedule_list.mslkey,
                            MovieKey = _movie_schedule_list.moviekey,
                            MovieName = _movie_schedule_list.moviename.ToUpper(),
                            Rating = _movie_schedule_list.rating,
                            StartTime = _movie_schedule_list.starttime,
                            EndTime = _movie_schedule_list.endtime,
                            RunningTimeInSeconds = _movie_schedule_list.duration
                        });

                        //set remaining and booked

                        var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                       where mslrs.movies_schedule_list_id == _movie_schedule_list.mslkey
                                       select mslrs.cinema_seat_id).Count();

                        //get booked
                        var bookings = (from mcths in context.movies_schedule_list_house_seat
                                        where mcths.movies_schedule_list_id == _movie_schedule_list.mslkey
                                        select mcths.cinema_seat_id).Count();

                        movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count-1].Booked = (int)bookings;
                        movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].Available = 
                         (int) (capacity - patrons - bookings);

                        if (movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].Available == 0)
                        {
                            movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].IsEnabled = false;
                        }
                        else
                        {
                            if (dtNow < _movie_schedule_list.starttime)
                            {
                                movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].IsEnabled = true;
                                if (movieScheduleItem.SelectedMovieScheduleListItem == null)
                                    movieScheduleItem.SelectedMovieScheduleListItem = movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1];
                            }
                            else if (dtNow < _movie_schedule_list.endtime) //allow already running
                            {
                                movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].IsEnabled = true;
                            }
                            else
                            {
                                movieScheduleItem.MovieScheduleListItems[movieScheduleItem.MovieScheduleListItems.Count - 1].IsEnabled = false;
                            }
                        }
                    }

                    movieScheduleItems.Add(movieScheduleItem);
                }

            }

            intColumnCount = 1;

            if (movieScheduleItems.Count > intMaxRowCount)
                intColumnCount = 2;

        }

        public DateTime CurrentDate
        {
            get
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;
                dtNow = new DateTime(dtScreenDate.Year, dtScreenDate.Month, dtScreenDate.Day, dtNow.Hour, dtNow.Minute, dtNow.Second);
                return dtNow;
            }
        }

        public DateTime ScreenDate
        {
            get
            {
                DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;
                return dtScreenDate;
            }
        }

        private void SetNextMovieScheduleListItem(MovieScheduleItem movieScheduleItem)
        {

            DateTime dtNow = this.CurrentDate;
            if (movieScheduleItem.MovieScheduleListItems.Count > 0)
            {
                int intMovieScheduleListItemIndex = -1;
                int intCount = movieScheduleItem.MovieScheduleListItems.Count;
                for (int i = 0; i <  intCount; i++)
                {
                    MovieScheduleListItem msli = movieScheduleItem.MovieScheduleListItems[i];
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
            double left = (MainCanvas.ActualWidth - MovieCalendarListBox.ActualWidth) / 2;
            if (left < 0)
                left = 0;

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
            if (dataContext != null && dataContext is MovieScheduleListItem)
            {
                MovieScheduleListItem msli = (MovieScheduleListItem)dataContext;
                NavigationService.GetNavigationService(this).Navigate(new SeatingPage1(msli, new List<int>()));
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is MovieScheduleListItem)
            {
                MovieScheduleListItem msli = (MovieScheduleListItem)dataContext;
                //lookup cinemakey
                foreach (MovieScheduleItem msi in movieScheduleItems)
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
            if (dataContext != null && dataContext is MovieScheduleListItem)
            {
                MovieScheduleListItem msli = (MovieScheduleListItem)dataContext;
                //lookup cinemakey
                foreach (MovieScheduleItem msi in movieScheduleItems)
                {
                    if (msli.CinemaKey == msi.Key)
                    {
                        this.SetNextMovieScheduleListItem(msi);
                    }
                }
            }
        }
    }
}
