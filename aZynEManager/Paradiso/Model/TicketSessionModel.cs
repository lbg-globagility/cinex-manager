using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class TicketSessionModel : INotifyPropertyChanged
    {
        private string strSessionId = string.Empty;
        private string strTerminal = string.Empty;
        private string strUser = string.Empty;
        private DateTime dtTicketDateTime;

        public int Id
        {
            get { return intId; }
            set 
            {
                if (value != intId)
                {
                    intId = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        public string SessionId
        {
            get { return strSessionId; }
            set
            {
                if (value != strSessionId)
                {
                    strSessionId = value;
                    NotifyPropertyChanged("SessionId");
                }
            }
        }

        public string Terminal
        {
            get { return strTerminal; }
            set 
            { 
                if (value != strTerminal)
                {
                    strTerminal = value;
                    NotifyPropertyChanged("Terminal");
                }
            }
        }

        public string User
        {
            get { return strUser; }
            set 
            {
                if (value != strUser)
                {
                    strUser = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        public DateTime TicketDateTime
        {
            get { return dtTicketDateTime; }
            set
            {
                if (value != dtTicketDateTime)
                {
                    dtTicketDateTime = value;
                    NotifyPropertyChanged("TicketDateTime");
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
