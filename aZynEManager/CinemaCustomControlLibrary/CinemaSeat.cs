using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaCustomControlLibrary
{
    public class CinemaSeat:CinemaItem
    {
        public enum SeatType
        {
            NormalTakenSeatType,
            NormalAvailableSeatType,
            NormalLockedSeatType,
            NormalReservedSeatType,
            HandicappedTakenSeatType,
            HandicappedAvailableSeatType,
            HandicappedLockedSeatType,
            HandicappedReservedSeatType,
        }

        public enum ActionType
        {
            NoActionType,
            TakenActionType,
            ReservedActionType,
        }

        public CinemaSeat()
        {
            this.Key = 0;
            this.Name = string.Empty;

            Type = SeatType.NormalTakenSeatType;
            Action = ActionType.NoActionType;

            IsResizable = false;
        }

        public CinemaSeat(CinemaSeat seat)
        {
            this.Key = seat.Key;
            this.IsResizable = seat.IsResizable;

            this.Name = seat.Name;
            this.CX = seat.CX;
            this.CY = seat.CY;
            this.A = seat.A;

            this.X1 = seat.X1;
            this.Y1 = seat.Y1;
            this.X2 = seat.X2;
            this.Y2 = seat.Y2;
            

            this.Type = seat.Type;
            this.Action = seat.Action;
        }


        public SeatType Type { get; set; }
        public ActionType Action { get; set; }

        public string Name { get; set; }
        

        public string ColumnName
        {
            get
            {
                string strColumnName = string.Empty;
                if (Name != null && Name.Length > 0 && Char.IsLetter(Name[0]))
                    strColumnName = Name[0].ToString();
                return strColumnName;
            }
        }

        public string RowName
        {
            get
            {
                string strRowName = string.Empty;
                if (Name != null && Name.Length > 1 && Char.IsLetter(Name[0]))
                    strRowName = Name.Substring(1);

                return strRowName;
            }
        }
    }
}
