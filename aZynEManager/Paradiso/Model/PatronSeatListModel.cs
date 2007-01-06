using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Paradiso.Model
{
    public class PatronSeatListModel : INotifyPropertyChanged
    {
        public ObservableCollection<PatronSeatModel> PatronSeats { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PatronSeatListModel()
        {
            PatronSeats = new ObservableCollection<PatronSeatModel>();

            PatronSeats.CollectionChanged += (sender, e) => NotifyPropertyChanged("Total");
        }
        

        public decimal Total
        {
            get
            {
                return PatronSeats.Sum(x => x.Price);
            }
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
