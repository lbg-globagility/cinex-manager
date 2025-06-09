using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class ReservedSeatingDetails : INotifyPropertyChanged
    {
        private string strName;
        private string strNotes;
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

        public string Notes
        {
            get { return strNotes; }
            set
            {
                if (value != strNotes)
                {
                    strNotes = value;
                    NotifyPropertyChanged("Notes");
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
