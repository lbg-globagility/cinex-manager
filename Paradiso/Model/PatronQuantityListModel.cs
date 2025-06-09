using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Paradiso.Helpers;

namespace Paradiso.Model
{
    public class PatronQuantityListModel : INotifyPropertyChanged, IDisposable
    {
        public MyObservableCollection<PatronQuantityModel> Patrons { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PatronQuantityListModel()
        {
            Patrons = new MyObservableCollection<PatronQuantityModel>();
            
            Patrons.CollectionChanged += new NotifyCollectionChangedEventHandler(Patrons_CollectionChanged);
            Patrons.ChildElementPropertyChanged += new MyObservableCollection<PatronQuantityModel>.ChildElementPropertyChangedEventHandler(Patrons_ChildElementPropertyChanged);
        }

        void Patrons_ChildElementPropertyChanged(MyObservableCollection<PatronQuantityModel>.ChildElementPropertyChangedEventArgs e)
        {
            this.UpdateCountTotal();
        }

        void Patrons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateCountTotal();
        }

        public void UpdateCountTotal()
        {
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("Total");
        }

        public int Count
        {
            get
            {
                return Patrons.Sum(x => x.Quantity);
            }
        }

        public decimal Total
        {
            get
            {
                return Patrons.Sum(x => x.TotalAmount);
            }
        }


        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Patrons.Clear();
            Patrons.CollectionChanged -= Patrons_CollectionChanged;
            Patrons.ChildElementPropertyChanged -= Patrons_ChildElementPropertyChanged;
        }

        #endregion
    }
}
