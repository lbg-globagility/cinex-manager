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
    public class TicketListModel : INotifyPropertyChanged, IDisposable
    {
        public MyObservableCollection<TicketModel> Tickets { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TicketListModel()
        {
            Tickets = new MyObservableCollection<TicketModel>();

            Tickets.CollectionChanged += new NotifyCollectionChangedEventHandler(Tickets_CollectionChanged);
            Tickets.ChildElementPropertyChanged += new MyObservableCollection<TicketModel>.ChildElementPropertyChangedEventHandler(Tickets_ChildElementPropertyChanged);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Tickets.Clear();
            Tickets.CollectionChanged -= Tickets_CollectionChanged;
            Tickets.ChildElementPropertyChanged -= Tickets_ChildElementPropertyChanged;
        }

        #endregion

        void Tickets_ChildElementPropertyChanged(MyObservableCollection<TicketModel>.ChildElementPropertyChangedEventArgs e)
        {
            this.UpdateCountTotal();
        }

        void Tickets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                return Tickets.Where(x => x.IsSelected == true).Count();
            }
        }

        public decimal Total
        {
            get
            {
                return Tickets.Where(x => x.IsSelected == true).Sum(x => x.PatronPrice);
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
