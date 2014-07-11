using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaCustomControlLibrary
{
    public class CinemaScreen:CinemaItem
    {
        public CinemaScreen()
        {
        }

        public CinemaScreen(CinemaScreen cinemaScreen)
        {
            Key = cinemaScreen.Key;
            CX = cinemaScreen.CX;
            CY = cinemaScreen.CY;
            A = cinemaScreen.A;
            X1 = cinemaScreen.X1;
            Y1 = cinemaScreen.Y1;
            X2 = cinemaScreen.X2;
            Y2 = cinemaScreen.Y2;
            IsResizable = cinemaScreen.IsResizable;
        }

    }
}
