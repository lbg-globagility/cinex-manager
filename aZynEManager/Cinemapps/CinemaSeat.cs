using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinemapps
{
    public class CinemaSeat
    {
        public enum SeatType
        {
            NormalSeatType,
            HandicappedSeatType,
        }

        public enum ActionType
        {
            OpenActionType,
            TakenActionType,
            ReservedActionType,
        }

        public SeatType Type { get; set; }
        public ActionType Action { get; set; }

        public int Key { get; set; }
        public string RowName { get; set; }
        
        public double CX { get; set; } //center x
        public double CY { get; set; } //center y
        public double A { get; set; } //angle

        //internal computations 
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
}
