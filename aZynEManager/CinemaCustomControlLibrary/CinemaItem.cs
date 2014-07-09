using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CinemaCustomControlLibrary
{
    public class CinemaItem : INotifyPropertyChanged
    {
        private int m_intKey;
        private double m_dblCX;
        private double m_dblCY;
        private double m_dblA;
        private double m_dblX1;
        private double m_dblY1;
        private double m_dblX2;
        private double m_dblY2;
        private bool m_blnIsResizable;

        public int Key {
            get { return m_intKey; }
            set
            {
                if (value != m_intKey)
                {
                    m_intKey = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }
        
        public double CX {
            get { return m_dblCX; }
            set
            {
                if (value != m_dblCX)
                {
                    m_dblCX = value;
                    NotifyPropertyChanged("CX");
                }
            }
        } 
        
        //center x
        
        public double CY {
            get { return m_dblCY; }
            set
            {
                if (value != m_dblCY)
                {
                    m_dblCY = value;
                    NotifyPropertyChanged("CY");
                }
            }
        } 
        //center y
        
        public double A {
            get { return m_dblA; }
            set
            {
                if (value != m_dblA)
                {
                    m_dblA = value;
                    NotifyPropertyChanged("A");
                }
            }
        } //angle

        //internal computations 
        public double X1 {
            get { return m_dblX1; }
            set
            {
                if (value != m_dblX1)
                {
                    m_dblX1 = value;
                    NotifyPropertyChanged("X1");
                }
            }
        }

        public double Y1
        {
            get { return m_dblY1; }
            set
            {
                if (value != m_dblY1)
                {
                    m_dblY1 = value;
                    NotifyPropertyChanged("Y1");
                }
            }
        }

        public double X2
        {
            get { return m_dblX2; }
            set
            {
                if (value != m_dblX2)
                {
                    m_dblX2 = value;
                    NotifyPropertyChanged("X2");
                }
            }
        }

        public double Y2
        {
            get { return m_dblY2; }
            set
            {
                if (value != m_dblY2)
                {
                    m_dblY2 = value;
                    NotifyPropertyChanged("Y2");
                }
            }
        }

        public bool IsResizable {
            get { return m_blnIsResizable; }
            set
            {
                if (value != m_blnIsResizable)
                {
                    m_blnIsResizable = value;
                    NotifyPropertyChanged("IsResizable");
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
