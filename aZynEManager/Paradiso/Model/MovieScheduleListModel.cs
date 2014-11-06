using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class MovieScheduleListModel : INotifyPropertyChanged
    {

        private int intKey; //to be passed 
        private int intCinemaKey; //to be referenced
        private DateTime dtStartTime;
        private DateTime dtEndTime;

        private int intMovieKey; //to be passed as well
        private string strMovieName = string.Empty;

        private string strRunningTime;
        private int intAvailable = 0;
        private int intBooked = 0;
        private int intSelected = 0;
        private string strRating = string.Empty;
        private string strRatingDescription = string.Empty;

        private bool blnIsEnabled = false;
        private bool blnIsEllapsed = false;

        private decimal decPrice = 0;
        private int intLayTime = 0; //in minutes
        private int intSeatType = 3; //1=reserved , 2=free seating (limited), 3=free seating (unlimited)

        private int intIndex = 0;


        public MovieScheduleListModel()
        {
        }

        public MovieScheduleListModel(MovieScheduleListModel movieScheduleListModel)
        {
            Key = movieScheduleListModel.Key;
            CinemaKey = movieScheduleListModel.CinemaKey;
            StartTime = movieScheduleListModel.StartTime;
            EndTime = movieScheduleListModel.EndTime;
            MovieKey = movieScheduleListModel.MovieKey;
            MovieName = movieScheduleListModel.MovieName;
            RunningTime = movieScheduleListModel.RunningTime;
            Available = movieScheduleListModel.Available;
            Booked = movieScheduleListModel.Booked;
            Selected = movieScheduleListModel.Selected;
            Rating = movieScheduleListModel.Rating;
            RatingDescription = movieScheduleListModel.RatingDescription;
            IsEnabled = movieScheduleListModel.IsEnabled;
            IsEllapsed = movieScheduleListModel.IsEllapsed;
            Price = movieScheduleListModel.Price;
            LayTime = movieScheduleListModel.LayTime;
            SeatType = movieScheduleListModel.SeatType;
            Index = movieScheduleListModel.Index;
        }

        public int CinemaKey
        {
            get { return intCinemaKey; }

            set
            {
                if (value != intCinemaKey)
                {
                    intCinemaKey = value;
                    NotifyPropertyChanged("CinemaKey");
                }
            }
        }

        public int Key
        {
            get { return intKey; }

            set
            {
                if (value != intKey)
                {
                    intKey = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }

        public int Index
        {
            get { return intIndex; }

            set
            {
                if (value != intIndex)
                {
                    intIndex = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

        public DateTime StartTime
        {
            get { return dtStartTime; }

            set
            {
                if (value != dtStartTime)
                {
                    dtStartTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        public DateTime EndTime
        {
            get { return dtEndTime; }

            set
            {
                if (value != dtEndTime)
                {
                    dtEndTime = value;
                    NotifyPropertyChanged("EndTime");
                }
            }
        }

        public int MovieKey
        {
            get { return intMovieKey; }

            set
            {
                if (value != intMovieKey)
                {
                    intMovieKey = value;
                    NotifyPropertyChanged("MovieKey");
                }
            }
        }

        public string MovieName
        {
            get { return strMovieName; }

            set
            {
                if (value != strMovieName)
                {
                    strMovieName = value;
                    NotifyPropertyChanged("MovieName");
                }
            }
        }

        public int Available
        {
            get { return intAvailable; }

            set
            {
                if (value != intAvailable)
                {
                    intAvailable = value;
                    NotifyPropertyChanged("Available");
                }
            }
        }

        public int Selected
        {
            get { return intSelected; }

            set
            {
                if (value != intSelected)
                {
                    intSelected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        public int Booked
        {
            get { return intBooked; }

            set
            {
                if (value != intBooked)
                {
                    intBooked = value;
                    NotifyPropertyChanged("Booked");
                }
            }
        }

        public string Rating
        {
            get { return strRating; }

            set
            {
                if (value != strRating)
                {
                    strRating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }

        public string RatingDescription
        {
            get { return strRatingDescription; }

            set
            {
                if (value != strRatingDescription)
                {
                    strRatingDescription = value;
                    NotifyPropertyChanged("RatingDescription");
                }
            }
        }

        public int RunningTimeInSeconds
        {
            set
            {
                int intSeconds = value;
                TimeSpan ts = new TimeSpan(0, 0, intSeconds);
                RunningTime = string.Format("{0}:{1:00}", (int) ts.TotalMinutes, ts.Seconds);
            }
        }

        public decimal Price
        {
            get { return decPrice; }
            set
            {
                if (value != decPrice)
                {
                    decPrice = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public int LayTime
        {
            get { return intLayTime; }
            set
            {
                if (value != intLayTime)
                {
                    intLayTime = value;
                    NotifyPropertyChanged("LayTime");
                }
            }
        }

        public int SeatType
        {
            get { return intSeatType; }
            set
            {
                if (value != intSeatType)
                {
                    intSeatType = value;
                    NotifyPropertyChanged("SeatType");
                }
            }
        }

        public string RunningTime
        {
            get { return strRunningTime; }

            set
            {
                if (value != strRunningTime)
                {
                    strRunningTime = value;
                    NotifyPropertyChanged("RunningTime");
                }
            }
        }

        public bool IsEnabled
        {
            get { return blnIsEnabled; }
            set
            {
                if (value != blnIsEnabled)
                {
                    blnIsEnabled = value;
                    NotifyPropertyChanged("IsEnabled");
                }
            }
        }

        public bool IsEllapsed
        {
            get { return blnIsEllapsed; }
            set
            {
                if (value != blnIsEllapsed)
                {
                    blnIsEllapsed = value;
                    NotifyPropertyChanged("IsEllapsed");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
