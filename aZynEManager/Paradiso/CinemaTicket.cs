using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Paradiso
{
    public class CinemaTicket : INotifyPropertyChanged
    {
        private int intKey;
        private int intMovieTimeKey;
        private int intPatronKey;
        private int intQuantity;
        private int intMinQuantity = 1;
        private int intMaxQuantity = 1;
        private decimal decPrice;
        private ObservableCollection<CinemaPatron> lstPatrons;


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
        
        public int MovieTimeKey 
        {
            get { return intMovieTimeKey; }
            set
            {
                if (value != MovieTimeKey)
                {
                    intMovieTimeKey = value;
                    NotifyPropertyChanged("MovieTimeKey");
                }
            }
        }
        
        public int PatronKey 
        {
            get { return intPatronKey; }
            set
            {
                if (value != PatronKey)
                {
                    intPatronKey = value;
                    NotifyPropertyChanged("PatronKey");
                }
            }
        }

        public int MinQuantity
        {
            get { return intMinQuantity; }
            set
            {
                if (value != MinQuantity)
                {
                    intMinQuantity = value;
                    NotifyPropertyChanged("MinQuantity");
                }
            }
        }

        public int MaxQuantity
        {
            get { return intMaxQuantity; }
            set
            {
                if (value != MaxQuantity)
                {
                    intMaxQuantity = value;
                    NotifyPropertyChanged("MaxQuantity");
                }
            }
        }

        public int Quantity 
        { 
            get { return intQuantity; }
            set
            {
                if (value != Quantity)
                {
                    intQuantity = value;
                    NotifyPropertyChanged("Quantity");
                }
            }
        }

        public decimal Price 
        {
            get { return decPrice; }
            set
            {
                if (value != Price)
                {
                    decPrice = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }
        
        public ObservableCollection<CinemaPatron> Patrons 
        {
            get { return lstPatrons; }
            set
            {
                ObservableCollection<CinemaPatron> _patrons = (ObservableCollection<CinemaPatron>)value;
                bool blnHasChanges = false;

                if (_patrons.Count != Patrons.Count)
                {
                    blnHasChanges = true;
                }
                else
                {
                    int intPatronCount = _patrons.Count;
                    for (int i = 0; i < intPatronCount; i++)
                    {
                        if (_patrons[i].Key != Patrons[i].Key || _patrons[i].PatronKey != Patrons[i].PatronKey || _patrons[i].PatronName != _patrons[i].PatronName ||
                            _patrons[i].Price != Patrons[i].Price)
                        {
                            blnHasChanges = true;
                            break;
                        }
                    }
                }


                if (blnHasChanges)
                {
                    Patrons.Clear();
                    foreach (CinemaPatron patron in _patrons)
                    {
                        Patrons.Add(new CinemaPatron() { Key = patron.Key, PatronKey = patron.PatronKey, PatronName = patron.PatronName, Price = patron.Price });
                    }
                    NotifyPropertyChanged("Patrons");
                }


            }
        }


        public CinemaTicket(CinemaTicket _cinemaTicket)
        {
            Key = _cinemaTicket.Key;
            MovieTimeKey = _cinemaTicket.MovieTimeKey;
            lstPatrons = new ObservableCollection<CinemaPatron>();
            Patrons = _cinemaTicket.Patrons;
            PatronKey = _cinemaTicket.PatronKey;
            Quantity = _cinemaTicket.Quantity;
            MinQuantity = _cinemaTicket.MinQuantity;
            MaxQuantity = _cinemaTicket.MaxQuantity;
            Price = _cinemaTicket.Price;
        }

        public CinemaTicket(int key, int movieTimeKey, int patronKey, int quantity, int minQuantity, int maxQuantity, decimal price, ObservableCollection<CinemaPatron> patrons)
        {
            Key = key;
            MovieTimeKey = movieTimeKey;
            Quantity = quantity;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            Price = price;
            lstPatrons = new ObservableCollection<CinemaPatron>();
            Patrons = patrons;
            PatronKey = patronKey;
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
