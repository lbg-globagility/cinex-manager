using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Paradiso.Model
{
    public class MovieScheduleModel : INotifyPropertyChanged
    {
        private int intKey;
        private int intNumber;
        private DateTime dtCurrent;

        private ObservableCollection<MovieScheduleListModel> movieScheduleListItems = new ObservableCollection<MovieScheduleListModel>();
        private MovieScheduleListModel selectedMovieScheduleListItem;

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

        public MovieScheduleListModel SelectedMovieScheduleListItem
        {
            get { return selectedMovieScheduleListItem; }
            set
            {
                if (value != selectedMovieScheduleListItem)
                {
                    selectedMovieScheduleListItem = value;
                    NotifyPropertyChanged("SelectedMovieScheduleListItem");
                }
            }
        }

        public ObservableCollection<MovieScheduleListModel> MovieScheduleListItems
        {
            get { return movieScheduleListItems; }
            set
            {
                if (value != movieScheduleListItems)
                {
                    movieScheduleListItems = value;
                    NotifyPropertyChanged("MovieScheduleListItems");
                }
            }
        }

        public int Number
        {
            get { return intNumber; }

            set
            {
                if (value != intNumber)
                {
                    intNumber = value;
                    NotifyPropertyChanged("Number");
                }
            }
        }

        public DateTime CurrentDate
        {
            get { return dtCurrent; }
            set
            {
                if (value != dtCurrent)
                {
                    dtCurrent = value;
                    NotifyPropertyChanged("Current Date");
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
