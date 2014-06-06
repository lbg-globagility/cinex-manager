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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MovieCalendarItemControl> movieCalendars;

        public MainWindow()
        {
            InitializeComponent();

            MovieCalendar.Children.Clear();
            movieCalendars = new List<MovieCalendarItemControl>();
            /*
            for (int i = 0; i < 10; i++)
            {
                movieCalendars.Add(new MovieCalendarItemControl());
                MovieCalendar.Children.Add(movieCalendars[movieCalendars.Count - 1]);
            }
            */
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dtScreenDate = new DateTime(2009, 1, 21);

            using (var context = new paradisoEntities())
            {
                var moviecalendars = (from mc in context.movie_calendar
                                      where mc.screening_start_date >= dtScreenDate && mc.screening_end_date <= dtScreenDate
                                      orderby mc.cinema.code
                                      select new { mc.cinema.code, mc.movie_database.name, mtrcb_name = mc.movie_database.mtrcb_ratings.code,
                                      mc.movie_database.running_time}).ToList();
                foreach (var moviecalendar in moviecalendars)
                {
                    TimeSpan ts = new TimeSpan(0, 0, moviecalendar.running_time);
                        movieCalendars.Add(new MovieCalendarItemControl());
                        movieCalendars[movieCalendars.Count - 1].MovieCalendar.Number = Convert.ToInt32(moviecalendar.code);
                        movieCalendars[movieCalendars.Count - 1].MovieCalendar.Name = moviecalendar.name;
                        movieCalendars[movieCalendars.Count - 1].MovieCalendar.Rating = string.Format("({0})", moviecalendar.mtrcb_name);
                        movieCalendars[movieCalendars.Count - 1].MovieCalendar.RunningTime = string.Format("{0}:{1:00}", (int) ts.TotalMinutes, ts.Seconds);

                        MovieCalendar.Children.Add(movieCalendars[movieCalendars.Count - 1]);
                }
            }
        }
    }
}
