using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class CinemaTicket
    {
        /*
        private int intKey;
        private int intMovieTimeKey;
        private int intPatronKey;
        private int intQuantity;
        private decimal decPrice;
        */

        public int Key { get; set; }
        public int MovieTimeKey { get; set; }
        public int PatronKey { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<CinemaPatron> Patrons { get; set; }

        public CinemaTicket(int key, int movieTimeKey, int patronKey, int quantity, decimal price, List<CinemaPatron> patrons)
        {
            Key = key;
            MovieTimeKey = movieTimeKey;
            PatronKey = patronKey;
            Quantity = quantity;
            Price = price;

            Patrons = new List<CinemaPatron>();
            if (patrons.Count > 0)
            {
                foreach (CinemaPatron patron in patrons)
                {
                    Patrons.Add(new CinemaPatron() { Key = patron.Key, PatronKey = patron.PatronKey, PatronName = patron.PatronName, Price = patron.Price });
                }
            }
        }
    }
}
