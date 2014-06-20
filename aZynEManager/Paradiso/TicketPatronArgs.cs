using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class TicketPatronArgs:EventArgs
    {
        private int m_intPrevPatronKey;
        private CinemaTicket cinemaTicket;

        internal TicketPatronArgs(CinemaTicket _cinemaTicket, int intPrevPatronKey)
        {
            cinemaTicket = new CinemaTicket(_cinemaTicket);
            m_intPrevPatronKey = intPrevPatronKey;
        }

        public CinemaTicket Ticket { get { return cinemaTicket; } }
        public int PrevPatronKey { get { return m_intPrevPatronKey; } }
    }
}
