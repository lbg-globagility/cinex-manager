using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;

namespace Paradiso.Model
{
    public class PatronSeatModel : INotifyPropertyChanged
    {
        private int intKey;
        private int intPatronKey;
        private string strPatronName;
        private int intSeatKey;
        private int intSeatColor;
        private string strSeatName;
        private decimal decPrice;
        private DateTime dtReservedDate;
        
        private string strRemainingTime;
        DispatcherTimer timer = new DispatcherTimer();

        public PatronSeatModel()
        {
        }

        public PatronSeatModel(int key, int seatKey, string seatName, int patronKey, string patronName, decimal price, DateTime reservedDate, int seatColor)
        {
            Key = key;
            SeatKey = seatKey;
            SeatName = seatName;
            PatronKey = patronKey;
            PatronName = patronName;
            Price = price;
            ReservedDate = reservedDate;
            RemainingTime = RemainingTimeValue;
            SeatColor = seatColor;

            timer.Interval = TimeSpan.FromMilliseconds(1000 * Paradiso.Constants.PatronSeatUiInterval);

            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }


        public string RemainingTime
        {
            get { return strRemainingTime; }

            private set
            {
                strRemainingTime = value;
                NotifyPropertyChanged("RemainingTime");
            }
        }

        private string RemainingTimeValue
        {
            get
            {
                DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;

                TimeSpan span = dtReservedDate.AddMinutes(10) - dtNow;
                if (span.TotalMinutes > 10 || span.Seconds < 0 || span.Minutes < 0)
                    return RemainingTime;

                return string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                RemainingTime = RemainingTimeValue;
            }
            catch { }
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

        public int PatronKey
        {
            get { return intPatronKey; }
            set
            {
                if (value != intPatronKey)
                {
                    intPatronKey = value;
                    NotifyPropertyChanged("PatronKey");
                }
            }
        }

        public int SeatKey
        {
            get { return intSeatKey; }
            set
            {
                if (value != intSeatKey)
                {
                    intSeatKey = value;
                    NotifyPropertyChanged("SeatKey");
                }
            }
        }

        public int SeatColor
        {
            get { return intSeatColor; }
            set
            {
                if (value != intSeatColor)
                {
                    intSeatColor = value;
                    NotifyPropertyChanged("SeatColor");
                }
            }
        }

        public string SeatName
        {
            get { return strSeatName; }
            set
            {
                if (value != strSeatName)
                {
                    strSeatName = value;
                    NotifyPropertyChanged("SeatName");
                }
            }
        }

        public string PatronName
        {
            get { return strPatronName; }
            set
            {
                if (value != strPatronName)
                {
                    strPatronName = value;
                    NotifyPropertyChanged("PatronName");
                }
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

        public DateTime ReservedDate
        {
            get { return dtReservedDate; }
            set
            {
                if (value != dtReservedDate)
                {
                    dtReservedDate = value;
                    NotifyPropertyChanged("ReservedDate");
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
