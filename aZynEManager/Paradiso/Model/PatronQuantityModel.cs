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
