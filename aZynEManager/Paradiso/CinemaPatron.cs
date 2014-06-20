using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso
{
    public class CinemaPatron : INotifyPropertyChanged
    {
        private int m_intKey;
        private int m_intPatronKey;
        private string m_strPatronName;
        private decimal m_decPrice;


        public CinemaPatron()
        {
        }

        public CinemaPatron(CinemaPatron patron)
        {
            if (patron != null)
            {
                Key = patron.Key;
                PatronKey = patron.PatronKey;
                PatronName = patron.PatronName;
                Price = patron.Price;
            }
        }

        public int Key
        {
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

        public int PatronKey 
        {
            get { return m_intPatronKey; }
            set
            {
                if (value != m_intPatronKey)
                {
                    m_intPatronKey = value;
                    NotifyPropertyChanged("PatronKey");
                }
            }
        }
        public string PatronName 
        {
            get { return m_strPatronName; }
            set
            {
                if (value != m_strPatronName)
                {
                    m_strPatronName = value;
                    NotifyPropertyChanged("PatronName");
                }
            }
        }
        public decimal Price 
        {
            get { return m_decPrice; }
            set
            {
                if (value != m_decPrice)
                {
                    m_decPrice = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public override string ToString()
        {
            return PatronName;
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
