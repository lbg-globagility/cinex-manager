using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso
{
    public class MovieCalendarItem : INotifyPropertyChanged
    {
        private int intKey = 0;
        private int intNumber = 0;
        private string strName = string.Empty;
        
        private string strTime1 = string.Empty;
        private string strTime2 = string.Empty;
        private string strTime3 = string.Empty;
        private string strTime4 = string.Empty;
        private string strTime5 = string.Empty;

        private string strRating = string.Empty;
        private string strRunningTime = string.Empty;
        private int intAvailable = 0;
        private int intBooked = 0;

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

        public int Number
        {
            get { return intNumber; }

            set
            {
                if (value != intNumber)
                {
                    intNumber = value;
                    NotifyPropertyChanged("Number");
                }
            }
        }

        public string Name
        {
            get { return strName; }

            set
            {
                if (value != strName)
                {
                    strName = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public string Time1
        {
            get { return strTime1; }

            set
            {
                if (value != strTime1)
                {
                    strTime1 = value;
                    NotifyPropertyChanged("Time1");
                }
            }
        }

        public string Time2
        {
            get { return strTime2; }

            set
            {
                if (value != strTime2)
                {
                    strTime1 = value;
                    NotifyPropertyChanged("Time2");
                }
            }
        }

        public string Time3
        {
            get { return strTime3; }

            set
            {
                if (value != strTime3)
                {
                    strTime3 = value;
                    NotifyPropertyChanged("Time3");
                }
            }
        }

        public string Time4
        {
            get { return strTime4; }

            set
            {
                if (value != strTime4)
                {
                    strTime4 = value;
                    NotifyPropertyChanged("Time4");
                }
            }
        }

        public string Time5
        {
            get { return strTime5; }

            set
            {
                if (value != strTime5)
                {
                    strTime5 = value;
                    NotifyPropertyChanged("Time5");
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
