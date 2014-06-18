using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso
{
    public class MovieCalendarTimeArgs:EventArgs
    {
        private int m_intMovieKey;
        private int m_intMovieTimeKey;

        internal MovieCalendarTimeArgs(int intMovieKey, int intMovieTimeKey)
        {
            m_intMovieKey = intMovieKey;
            m_intMovieTimeKey = intMovieTimeKey;
        }

        public int MovieKey { get { return m_intMovieKey; } }
        public int MovieTimeKey { get { return m_intMovieTimeKey; } }
    }
}
