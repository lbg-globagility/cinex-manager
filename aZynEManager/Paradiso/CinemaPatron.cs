using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class CinemaPatron
    {
        public int Key { get; set; }
        public int PatronKey { get; set; }
        public string PatronName { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return PatronName;
        }
    }
}
