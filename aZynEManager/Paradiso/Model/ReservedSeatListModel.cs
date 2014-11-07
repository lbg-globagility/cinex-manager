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
    public class ReservedSeatListModel : INotifyPropertyChanged
    {
        private string strName;
        private string strNotes;

        public MyObservableCollection<ReservedSeatModel>  ReservedSeats { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReservedSeatListModel()
        {
            ReservedSeats = new MyObservableCollection<ReservedSeatModel>();

            ReservedSeats.CollectionChanged += new NotifyCollectionChangedEventHandler(ReservedSeats_CollectionChanged);
            ReservedSeats.ChildElementPropertyChanged += new MyObservableCollection<ReservedSeatModel>.ChildElementPropertyChangedEventHandler(ReservedSeats_ChildElementPropertyChanged);
        }

        void ReservedSeats_ChildElementPropertyChanged(MyObservableCollection<ReservedSeatModel>.ChildElementPropertyChangedEventArgs e)
        {
            //this.UpdateCountTotal();
        }

        void ReservedSeats_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //this.UpdateCountTotal();
        }

        public string Name
        {
            get { return strName; }
            set
            {
                if (value != strName)
                {
                    strName = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public string Notes
        {
            get 
            {
                if (strNotes == "RESERVED")
                    return string.Empty;
                else if (strNotes.StartsWith("RESERVED "))
                    return strNotes.Substring(9).Trim();
                else
                    return strNotes; 
            }
            set
            {
                if (value != strNotes)
                {
                    strNotes = value;
                    this.NotifyPropertyChanged("Notes");
                }
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
