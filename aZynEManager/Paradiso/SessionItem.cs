using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso
{
    public class SessionItem: INotifyPropertyChanged
    {
        private DateTime dtScreeningDate;
        private DateTime dtCurrentDate;
        private string strUserName = string.Empty;

        public DateTime CurrentDate
        {
            get { return dtCurrentDate; }

            set
            {
                if (value != dtCurrentDate)
                {
                    dtCurrentDate = value;
                    NotifyPropertyChanged("CurrentDate");
                }
            }
        }

        public DateTime ScreeningDate
        {
            get { return dtScreeningDate; }

            set
            {
                if (value != dtScreeningDate)
                {
                    dtScreeningDate = value;
                    NotifyPropertyChanged("ScreeningDate");
                }
            }
        }

        public string UserName
        {
            get { return strUserName; }

            set
            {
                if (value != strUserName)
                {
                    strUserName = value;
                    NotifyPropertyChanged("UserName");
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
