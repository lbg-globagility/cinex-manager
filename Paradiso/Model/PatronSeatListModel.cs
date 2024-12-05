using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Paradiso.Model
{
    public class PatronSeatListModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<PatronSeatModel> PatronSeats { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PatronSeatListModel()
        {
            PatronSeats = new ObservableCollection<PatronSeatModel>();

            PatronSeats.CollectionChanged += (sender, e) => NotifyPropertyChanged("Total");
        }


        public void Dispose()
        {
            int intCount = PatronSeats.Count;
            if (intCount > 0)
            {
                for (int i = 0; i < intCount; i++)
                {
                    PatronSeats[i].Dispose();
                }
            }
            PatronSeats.Clear();
            try
            {
                PatronSeats.CollectionChanged -= (sender, e) => NotifyPropertyChanged("Total");
            }
            catch { }
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
