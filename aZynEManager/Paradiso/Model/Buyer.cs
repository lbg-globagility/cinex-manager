using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class Buyer : INotifyPropertyChanged
    {
        private string strName;
        private string strAddress;
        private string strTIN;
        private string strIDNum;

        private bool blnIsSCPWD = false;

        private bool blnIsCancelled = true;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string Address
        {
            get { return strAddress; }
            set
            {
                if (value != strAddress)
                {
                    strAddress = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        public string TIN
        {
            get { return strTIN; }
            set
            {
                if (value != strTIN)
                {
                    strTIN = value;
                    NotifyPropertyChanged("TIN");
                }
            }
        }

        public string IDNum
        {
            get { return strIDNum; }
            set
            {
                if (value != strIDNum)
                {
                    strIDNum = value;
                    NotifyPropertyChanged("IDNum");
                }
            }
        }

        public bool IsSCPWD
        {
            get { return blnIsSCPWD; }
            set
            {
                if (value != blnIsSCPWD)
                {
                    blnIsSCPWD = value;
                    NotifyPropertyChanged("IsSCPWD");
                }
            }
        }

        public bool IsCancelled
        {
            get { return blnIsCancelled; }
            set
            {
                if (value != blnIsCancelled)
                {
                    blnIsCancelled = value;
                    NotifyPropertyChanged("IsCancelled");
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
