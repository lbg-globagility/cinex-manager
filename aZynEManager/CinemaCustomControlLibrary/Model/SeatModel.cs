using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CinemaCustomControlLibrary.Model
{
    public class SeatModel : INotifyPropertyChanged
    {
        private int intKey; //-negative means newly created
        private int intCinemaKey;
        private string strColumnName;
        private string strRowName;
        private int intX;
        private int intY;
        private int intHeight;
        private int intWidth;
        private int intType; //1 -seat, 2-screen

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

        public int CinemaKey
        {
            get { return intCinemaKey; }
            set
            {
                if (value != intCinemaKey)
                {
                    intCinemaKey = value;
                    NotifyPropertyChanged("CinemaKey");
                }
            }
        }

        public int X
        {
            get { return intX; }
            set
            {
                int _intX = value;
                if (_intX < 0)
                    _intX = 0;

                if (_intX != intX)
                {
                    intX = _intX;
                    NotifyPropertyChanged("X");
                }
            }
        }

        public int Y
        {
            get { return intY; }
            set
            {
                int _intY = value;
                if (_intY < 0)
                    _intY = 0;

                if (_intY != intY)
                {
                    intY = _intY;
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

        public string ColumnName
        {
            get { return strColumnName; }
            set
            {
                if (value != strColumnName)
                {
                    strColumnName = value;
                    NotifyPropertyChanged("ColumnName");
                }
            }
        }

        public string RowName
        {
            get { return strRowName; }
            set
            {
                if (value != strRowName)
                {
                    strRowName = value;
                    NotifyPropertyChanged("RowName");
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
