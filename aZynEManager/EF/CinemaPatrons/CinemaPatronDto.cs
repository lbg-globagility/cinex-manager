using Cinex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aZynEManager.EF.CinemaPatrons
{
    public class CinemaPatronDto
    {
        public Patron Patron { get; }
        public CinemaPatron CinemaPatron { get; }
        public CinemaPatronDefault DefaultPatron { get; }
        public string Code { get; }
        public string Name { get; }
        public decimal OfficialUnitPrice { get; }

        public CinemaPatronDto(Patron patron,
            CinemaPatron cinemaPatron,
            CinemaPatronDefault defaultPatron)
        {
            Patron = patron;
            CinemaPatron = cinemaPatron;
            DefaultPatron = defaultPatron;

            Code = patron.Code;
            Name = patron.Name;
            OfficialUnitPrice = patron.OfficialUnitPrice;

            IsAccomodated = (cinemaPatron?.Id ?? 0) > 0;
            IsDefault = (defaultPatron?.Id ?? 0) > 0;
        }

        public bool IsAccomodated { get; set; }
        public bool IsDefault { get; set; }
    }
}
