using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CinemaCustomControlLibrary.Model;
using CinemaCustomControlLibrary.Helpers;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace CinemaCustomControlLibrary.ViewModel
{
    class SeatsViewModel : ViewModelBase
    {
        private int _cinemaKey;
        private string _cinemaName;
        private int _capacity;

        public MyObservableCollection<SeatModel> Seats { get; set; }
        public SeatModel _selectedSeat;

        public SeatsViewModel()
        {
            Seats = new MyObservableCollection<SeatModel>();
            
            Seats.CollectionChanged += new NotifyCollectionChangedEventHandler(Seats_CollectionChanged);
            Seats.ChildElementPropertyChanged += new MyObservableCollection<SeatModel>.ChildElementPropertyChangedEventHandler(Seats_ChildElementPropertyChanged);
        }

        public SeatModel SelectedSeat
        {
            get { return _selectedSeat; }
            set
            {
                if (_selectedSeat != value)
                {
                    _selectedSeat = value;
                    RaisePropertyChanged("SelectedSeat");
                }
            }
        }

        public int CinemaKey
        {
            get { return _cinemaKey; }
            set
            {
                if (_cinemaKey != value)
                {
                    _cinemaKey = value;
                    RaisePropertyChanged("CinemaKey");
                }
            }
        }

        public string CinemaName
        {
            get { return _cinemaName; }
            set
            {
                if (_cinemaName != value)
                {
                    _cinemaName = value;
                    RaisePropertyChanged("CinemaName");
                }
            }
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    RaisePropertyChanged("Capacity");
                }
            }
        }

        public string MaxColumnName
        {
            get
            {
                string strMaxColumnName = Seats.Max(d => d.ColumnName);
                if (strMaxColumnName == string.Empty)
                    strMaxColumnName = "A";
                return strMaxColumnName;
            }
        }
        public string NextRowName
        {
            get
            {
                string strMaxRowName = this.MaxRowName;
                int intNextRowName = 0;
                int.TryParse(strMaxRowName, out intNextRowName);
                intNextRowName++;
                return string.Format("{0}", intNextRowName);
            }
        }


        public string MaxRowName
        {
            get
            {
                int intMaxRowName = 0;
                try
                {
                    var rowNames = Seats.Where(d => d.ColumnName == this.MaxColumnName).Select(d => d.RowName).ToList();

                    int temp = 0;
                    intMaxRowName = rowNames.Select(n => int.TryParse(n, out temp) ? temp : 0).Max();
                }
                catch { }
                return string.Format("{0}", intMaxRowName);
            }
        }

        public int ScreenCount
        {
            get
            {
                return new ObservableCollection<SeatModel>(Seats.Where(d => d.Type == 2)).Count();
            }
        }

        public int AvailableScreenCount
        {
            get
            {
                int intAvailableScreenCount = 1;
                intAvailableScreenCount -= this.ScreenCount;
                if (intAvailableScreenCount < 0)
                    intAvailableScreenCount = 0;
                return intAvailableScreenCount;
            }
        }

        public int SeatCount
        {
            get
            {
                return new ObservableCollection<SeatModel>(Seats.Where(d => d.Type == 1)).Count();
            }
        }

        public int AvailableSeatCount
        {
            get
            {
                int intAvailableSeatCount = Capacity;
                intAvailableSeatCount -= this.SeatCount;
                if (intAvailableSeatCount < 0)
                    intAvailableSeatCount = 0;
                return intAvailableSeatCount;
            }
        }

        private void UpdateProperties()
        {
            this.RaisePropertyChanged("ScreenCount");
            this.RaisePropertyChanged("AvailableScreenCount");
            this.RaisePropertyChanged("SeatCount");
            this.RaisePropertyChanged("AvailableSeatCount");
        }

        void Seats_ChildElementPropertyChanged(MyObservableCollection<SeatModel>.ChildElementPropertyChangedEventArgs e)
        {
            this.UpdateProperties();
        }

        void Seats_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateProperties();
        }




    }
}
