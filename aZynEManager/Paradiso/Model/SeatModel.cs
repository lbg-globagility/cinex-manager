using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class SeatModel : INotifyPropertyChanged
    {
        private int intKey;
        private string strName;
        private int intX;
        private int intY;
        private int intHeight;
        private int intWidth;
        private int intType; //1 -seat, 2-screen

        private int intSeatType; //1-available, 2-selected, 3-taken

        public event PropertyChangedEventHandler PropertyChanged;

        public int Key
        {
            get { return intKey; }
            set
            {
                if (value != intKey)
                {
                    intKey = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }

        public int X
        {
            get { return intX; }
            set
            {
                if (value != intX)
                {
                    intX = value;
                    NotifyPropertyChanged("X");
                }
            }
        }

        public int Y
        {
            get { return intY; }
            set
            {
                if (value != intY)
                {
                    intY = value;
                    NotifyPropertyChanged("Y");
                }
            }
        }

        public int Width
        {
            get { return intWidth; }
            set
            {
                if (value != intWidth)
                {
                    intWidth = value;
                    NotifyPropertyChanged("Width");
                }
            }
        }

        public int Height
        {
            get { return intHeight; }
            set
            {
                if (value != intHeight)
                {
                    intHeight = value;
                    NotifyPropertyChanged("Height");
                }
            }
        }

        public int Type
        {
            get { return intType; }
            set
            {
                if (value != intType)
                {
                    intType = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        public int SeatType
        {
            get { return intSeatType; }
            set
            {
                if (value != intSeatType)
                {
                    intSeatType = value;
                    NotifyPropertyChanged("SeatType");
                }
            }
        }

        public string Name
        {
            get { return strName; }
            set
            {
                if (value != strName)
                {
                    strName = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
