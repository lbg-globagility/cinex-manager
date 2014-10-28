using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class ReservedSeatModel : INotifyPropertyChanged
    {
        private int intKey;
        private string strName;

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

        public override bool Equals(object obj)
        {
            if (obj is ReservedSeatModel)
            {
                ReservedSeatModel rs = obj as ReservedSeatModel;
                return (rs.Key == this.Key) ;
            }
            return false;
        }

        public override string ToString()
        {
            if (this.Name != null)
                return this.Name;
            return string.Empty;
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
