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

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
