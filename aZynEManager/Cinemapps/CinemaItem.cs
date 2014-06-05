using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinemapps
{
    public class CinemaItem
    {
        public int Key { get; set; }
        public double CX { get; set; } //center x
        public double CY { get; set; } //center y
        public double A { get; set; } //angle

        //internal computations 
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public bool IsResizable { get; set; }

        public CinemaItem()
        {
            IsResizable = true;
        }

        public bool IsEqual(CinemaItem item)
        {
            if (item.X1 == this.X1 && item.Y1 == this.Y1 && item.X2 == this.X2 && item.Y2 == this.Y2)
                return true;
            return false;
        }

        public double Width
        {
            get
            {
                return X2 - X1;
            }
        }

        public double Height
        {
            get
            {
                return Y2 - Y1;
            }
        }
       

    }
}
