using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Paradiso
{
    public class MovieCalendarTimeCommand
    {
        private static RoutedUICommand retrieveMovieCalendarTime;

        public static RoutedUICommand RetrieveMovieCalendarTime
        {
            get { return retrieveMovieCalendarTime; }
        }

        static MovieCalendarTimeCommand()
        {
            retrieveMovieCalendarTime = new RoutedUICommand("RetrieveMovieCalendarTime",
                "RetrieveMovieCalendarTime", typeof(MovieCalendarTimeCommand));
        }
    }
}
