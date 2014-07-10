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
using System.Threading;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MovieCalendarPage.xaml
    /// </summary>
    public partial class MovieCalendarPage : Page
    {
        private ObservableCollection<MovieCalendarItemControl> movieCalendars;
        private int m_intCalendarCount = 10;
        

        public MovieCalendarPage()
        {
            InitializeComponent();

            movieCalendars = new ObservableCollection<MovieCalendarItemControl>();

            DateTime dtScreenDate = new DateTime(2006, 12, 1);
            ParadisoObjectManager.GetInstance().ScreeningDate = dtScreenDate;

        }

        public ObservableCollection<MovieCalendarItemControl> MovieCalendars
        {
            get { return movieCalendars; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dtScreenDate = ParadisoObjectManager.GetInstance().ScreeningDate;
            dtNow = new DateTime(dtScreenDate.Year, dtScreenDate.Month, dtScreenDate.Day, dtNow.Hour, dtNow.Minute, dtNow.Second);            

            int i = 0;
            int intColumn = 0;
            int intRow = 0;

            int intCinemaKey = 0;
            int intCinemaCapacity = 0;

            MovieCalendar.Children.Clear();
            movieCalendars.Clear();

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var moviecalendars = (from mc in context.movies_schedule
                                      where mc.movie_date == dtScreenDate 
                                      orderby mc.cinema.in_order
                                      select new
                                      {
                                          key = mc.id,
                                          code = mc.cinema.in_order,
                                          name = mc.movie.title,
                                          mtrcb_name = mc.movie.mtrcb.name,
                                          running_time = mc.movie.duration,
                                          cinema_key = mc.cinema_id,
                                          capacity = mc.cinema.capacity,

                                      }).ToList();
                foreach (var moviecalendar in moviecalendars)
                {

                    intColumn = i / (m_intCalendarCount / 2);
                    intRow = i % (m_intCalendarCount / 2);

                    movieCalendars.Add(new MovieCalendarItemControl());

                    TimeSpan ts = new TimeSpan(0, 0, moviecalendar.running_time);
                    movieCalendars[i].MovieCalendar.Key = moviecalendar.key;
                    movieCalendars[i].MovieCalendar.Number = Convert.ToInt32(moviecalendar.code);
                    movieCalendars[i].MovieCalendar.Name = moviecalendar.name;
                    movieCalendars[i].MovieCalendar.Rating = string.Format("({0})", moviecalendar.mtrcb_name);
                    movieCalendars[i].MovieCalendar.RunningTime = string.Format("{0}:{1:00}", (int)ts.TotalMinutes, ts.Seconds);
                    movieCalendars[i].MovieCalendar.CurrentDate = dtNow;
                    intCinemaKey = moviecalendar.cinema_key;
                    intCinemaCapacity = (int) moviecalendar.capacity;

                    //get screening time
                    var movietimes = (from mct in context.movies_schedule_list
                                      where mct.movies_schedule_id == moviecalendar.key
                                      orderby mct.start_time, mct.end_time
                                      select new { key = mct.id, mct.start_time, mct.end_time }).ToList();
                    int intIndex = 0;
                    int intTimeKey = 0;
                    foreach (var movietime in movietimes)
                    {
                        if (intIndex == 0)
                        {
                            movieCalendars[i].MovieCalendar.TimeKey1 = movietime.key;
                            movieCalendars[i].MovieCalendar.Time1 = movietime.start_time;
                        }
                        else if (intIndex == 1)
                        {
                            movieCalendars[i].MovieCalendar.TimeKey2 = movietime.key;
                            movieCalendars[i].MovieCalendar.Time2 = movietime.start_time;
                        }
                        else if (intIndex == 2)
                        {
                            movieCalendars[i].MovieCalendar.TimeKey3 = movietime.key;
                            movieCalendars[i].MovieCalendar.Time3 = movietime.start_time;
                        }
                        else if (intIndex == 3)
                        {
                            movieCalendars[i].MovieCalendar.TimeKey4 = movietime.key;
                            movieCalendars[i].MovieCalendar.Time4 = movietime.start_time;
                        }
                        else if (intIndex == 4)
                        {
                            movieCalendars[i].MovieCalendar.TimeKey5 = movietime.key;
                            movieCalendars[i].MovieCalendar.Time5 = movietime.start_time;
                        }
                        intIndex++;
                    }
                    
                    intTimeKey = movieCalendars[i].MovieCalendar.LeastTimeKey;

                    //get paid
                   var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.movies_schedule_list_id == intTimeKey
                                   select mslrs.cinema_seat_id).Count();
                    
                    //get booked
                    var bookings = (from mcths in context.movies_schedule_list_house_seat
                                    where mcths.movies_schedule_list_id == intTimeKey
                                    select mcths.cinema_seat_id).Count();

                    movieCalendars[i].MovieCalendar.Booked = (int)bookings;
                    
                    movieCalendars[i].MovieCalendar.Available = intCinemaCapacity - patrons - bookings;

                    movieCalendars[movieCalendars.Count - 1].MovieCalendarTimeClicked += new EventHandler(MovieCalendarPage_MovieCalendarTimeClicked);

                    MovieCalendar.Children.Add(movieCalendars[movieCalendars.Count - 1]);

                    MovieCalendar.Children[MovieCalendar.Children.Count - 1].SetValue(Grid.RowProperty, intRow);
                    MovieCalendar.Children[MovieCalendar.Children.Count - 1].SetValue(Grid.ColumnProperty, intColumn);
                    i++;
                }
            }


            for (int j = i; j < m_intCalendarCount; j++)
            {
                movieCalendars.Add(new MovieCalendarItemControl());
                movieCalendars[movieCalendars.Count - 1].MovieCalendar.CurrentDate = dtNow;
                MovieCalendar.Children.Add(movieCalendars[movieCalendars.Count - 1]);
                //MovieCalendar.Children.Add(new MovieCalendarItemControl());

                intColumn = j / (m_intCalendarCount / 2);
                intRow = j % (m_intCalendarCount / 2);

                MovieCalendar.Children[MovieCalendar.Children.Count - 1].SetValue(Grid.RowProperty, intRow);
                MovieCalendar.Children[MovieCalendar.Children.Count - 1].SetValue(Grid.ColumnProperty, intColumn);
            }

            //Thread thread = new Thread(UpdateMovieCalendar);
            //thread.Start();
        }

        /*
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //databinding example
            MovieCalendarItemControl control = (MovieCalendarItemControl)MovieCalendar.Children[0];
            control.MovieCalendar.Name = "RASHOMON";

        }
        */

        private void MovieCalendarPage_MovieCalendarTimeClicked(Object sender, EventArgs e)
        {
            MovieCalendarTimeArgs mcte = (MovieCalendarTimeArgs)e;
            NavigationService.GetNavigationService(this).Navigate(new SeatingPage(mcte.MovieKey, mcte.MovieTimeKey, new List<int>()));
        }
    }
}
