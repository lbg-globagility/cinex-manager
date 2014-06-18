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
        
        private int intTime1 = 0;
        private DateTime dtTime1;
        private int intTime2 = 0;
        private DateTime dtTime2;
        private int intTime3 = 0;
        private DateTime dtTime3;
        private int intTime4 = 0;
        private DateTime dtTime4;
        private int intTime5 = 0;
        private DateTime dtTime5;

        private string strRating = string.Empty;
        private string strRunningTime = string.Empty;
        private int intAvailable = 0;
        private int intBooked = 0;

        private DateTime dtCurrent;

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

        public bool IsVisible
        {
            get { return (intKey != 0); }
        }

        public bool IsTime1Enabled
        {
            get
            {
                if (Available == 0 || Time1 < dtCurrent)
                    return false;
                return true;
            }
        }

        public bool IsTime2Enabled
        {
            get
            {
                if (Available == 0 || Time2 < dtCurrent)
                    return false;
                return true;
            }
        }

        public bool IsTime3Enabled
        {
            get
            {
                if (Available == 0 || Time3 < dtCurrent)
                    return false;
                return true;
            }
        }

        public bool IsTime4Enabled
        {
            get
            {
                if (Available == 0 || Time4 < dtCurrent)
                    return false;
                return true;
            }
        }

        public bool IsTime5Enabled
        {
            get
            {
                if (Available == 0 || Time5 < dtCurrent)
                    return false;
                return true;
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

        public DateTime CurrentDate
        {
            get { return dtCurrent; }
            set
            {
                if (value != dtCurrent)
                {
                    dtCurrent = value;
                    NotifyPropertyChanged("Current Date");
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

        public int TimeKey1
        {
            get { return intTime1; }

            set
            {
                if (value != intTime1)
                {
                    intTime1 = value;
                    NotifyPropertyChanged("TimeKey1");
                }
            }
        }

        public int TimeKey2
        {
            get { return intTime2; }

            set
            {
                if (value != intTime2)
                {
                    intTime2 = value;
                    NotifyPropertyChanged("TimeKey2");
                }
            }
        }

        public int TimeKey3
        {
            get { return intTime3; }

            set
            {
                if (value != intTime3)
                {
                    intTime3 = value;
                    NotifyPropertyChanged("TimeKey3");
                }
            }
        }

        public int TimeKey4
        {
            get { return intTime4; }

            set
            {
                if (value != intTime4)
                {
                    intTime4 = value;
                    NotifyPropertyChanged("TimeKey4");
                }
            }
        }

        public int TimeKey5
        {
            get { return intTime5; }

            set
            {
                if (value != intTime5)
                {
                    intTime5 = value;
                    NotifyPropertyChanged("TimeKey5");
                }
            }
        }

        public DateTime Time1
        {
            get { return dtTime1; }

            set
            {
                if (value != dtTime1)
                {
                    dtTime1 = value;
                    NotifyPropertyChanged("Time1");
                }
            }
        }

        public DateTime Time2
        {
            get { return dtTime2; }

            set
            {
                if (value != dtTime2)
                {
                    dtTime2 = value;
                    NotifyPropertyChanged("Time2");
                }
            }
        }

        public DateTime Time3
        {
            get { return dtTime3; }

            set
            {
                if (value != dtTime3)
                {
                    dtTime3 = value;
                    NotifyPropertyChanged("Time3");
                }
            }
        }

        public DateTime Time4
        {
            get { return dtTime4; }

            set
            {
                if (value != dtTime4)
                {
                    dtTime4 = value;
                    NotifyPropertyChanged("Time4");
                }
            }
        }

        public DateTime Time5
        {
            get { return dtTime5; }

            set
            {
                if (value != dtTime5)
                {
                    dtTime5 = value;
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

        public int LeastTimeKey
        {
            get
            {
                if (IsTime1Enabled)
                    return TimeKey1;
                else if (IsTime2Enabled)
                    return TimeKey2;
                else if (IsTime3Enabled)
                    return TimeKey3;
                else if (IsTime4Enabled)
                    return TimeKey4;
                else if (IsTime5Enabled)
                    return TimeKey5;
                else
                    return TimeKey1; //return 1st entry if all disabled
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
