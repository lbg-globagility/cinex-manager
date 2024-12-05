using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class CinemaModel : INotifyPropertyChanged
    {
        private int intId;
        private string strName = string.Empty;

        public CinemaModel()
        {
        }

        public CinemaModel(CinemaModel cinemaModel)
        {
            Id = cinemaModel.Id;
            Name = cinemaModel.Name;
        }

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
