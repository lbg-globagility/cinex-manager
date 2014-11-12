using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso.Model
{
    public class PatronQuantityModel: PatronModel
    {
        private int intQuantity = 0;
        private decimal decTotalAmount = 0;
        private int intMaxQuantity = 999;

        public PatronModel _patron;

        public int Quantity
        {
            get { return intQuantity; }
            set
            {
                if (value != intQuantity)
                {
                    intQuantity = value;
                    TotalAmount = Quantity * Price;
                    NotifyPropertyChanged("Quantity");
                }
            }
        }

        public PatronModel Patron
        {
            get { return _patron; }
            set
            {
                if (value != _patron)
                {
                    _patron = value;
                    NotifyPropertyChanged("Patron");
                }
            }
        }

        public int MaxQuantity
        {
            get { return intMaxQuantity; }
            set
            {
                if (value != intMaxQuantity)
                {
                    intMaxQuantity = value;
                    NotifyPropertyChanged("MaxQuantity");
                }
            }
        }

        public decimal TotalAmount
        {
            get { return decTotalAmount; }
            set
            {
                if (value != decTotalAmount)
                {
                    decTotalAmount = value;
                    NotifyPropertyChanged("TotalAmount");
                }
            }
        }
    }
}
