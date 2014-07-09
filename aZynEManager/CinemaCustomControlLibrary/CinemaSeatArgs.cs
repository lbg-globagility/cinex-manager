using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaCustomControlLibrary
{
    public class CinemaSeatArgs:EventArgs
    {
        private int m_intSeatKey;
        private CinemaSeat.SeatType seatType;

        internal CinemaSeatArgs(int intSeatKey, CinemaSeat.SeatType _seatType)
        {
            m_intSeatKey = intSeatKey;
            seatType = _seatType;
        }

        public int SeatKey { get { return m_intSeatKey; } }
        public CinemaSeat.SeatType SeatType { get { return seatType; } }
    }
}
