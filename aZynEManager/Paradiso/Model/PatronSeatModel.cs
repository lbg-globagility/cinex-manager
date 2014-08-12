using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class PatronSeatModel : INotifyPropertyChanged
    {
        private int intKey;
        private string strPatronName;
        private int intSeatKey;
        private string strSeatName;
        private decimal decPrice;
        private DateTime dtReservedDate;

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
