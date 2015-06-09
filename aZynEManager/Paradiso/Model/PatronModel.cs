using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class PatronModel : INotifyPropertyChanged
    {
        private int intKey;
        private int intPatronKey;
        private string strCode;
        private string strName;
        private decimal decPrice;
        private decimal decBasePrice;
        private decimal decOrdinancePrice;
        private decimal decSurchargePrice;
        private int intSeatColor;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string Code
        {
            get { return strCode; }
            set
            {
                if (value != strCode)
                {
                    strCode = value;
                    NotifyPropertyChanged("Code");
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

        public decimal BasePrice
        {
            get { return decBasePrice; }
            set
            {
                if (value != decBasePrice)
                {
                    decBasePrice = value;
                    NotifyPropertyChanged("BasePrice");
                }
            }
        }

        public decimal OrdinancePrice
        {
            get { return decOrdinancePrice; }
            set
            {
                if (value != decOrdinancePrice)
                {
                    decOrdinancePrice = value;
                    NotifyPropertyChanged("OrdinancePrice");
                }
            }
        }

        public decimal SurchargePrice
        {
            get { return decSurchargePrice; }
            set
            {
                if (value != decSurchargePrice)
                {
                    decSurchargePrice = value;
                    NotifyPropertyChanged("SurchargePrice");
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

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is PatronModel)
            {
                PatronModel p = (PatronModel)obj;
                if (p.Key == this.Key)
                    return true;
            }

            return false;
            //return base.Equals(obj);
        }
    }
}
