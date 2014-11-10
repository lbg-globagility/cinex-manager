using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class TicketModel : INotifyPropertyChanged
    {
        private int intId = 0;
        private string strORNumber = string.Empty;

        private string strHeader1 = "COMMERCENTER";
        private string strHeader2 = "MUNTINLUPA CITY";

        private string strMIN = string.Empty;
        private string strTIN = "XXX-XXX-XXX-XXX";
        private string strPN = "XXXX-XXX-XXXXX-XXX";

        private int intCinemaNumber = 0;
        private string strMovieCode = string.Empty;
        private string strRating = string.Empty;
        private DateTime dtStartTime;
        private string strPatronCode = string.Empty;
        private decimal decPatronPrice = 0m;

        private int intSeatType = 0;
        private string strSeatType = string.Empty;

        private string strCode = string.Empty;
        private decimal decCulturalTax = 0;
        private decimal decAmusementTax = 0;
        private decimal decVatTax = 0;
        private string strTerminalName = string.Empty;
        private string strTellerCode = string.Empty;
        private DateTime dtCurrentTime;
        private string strSessionName = string.Empty;
        private string strSerialNumber = string.Empty;

        private bool blnIsVoid = false;

        private string strPatronDescription = string.Empty;
        private string strSeatName = string.Empty;
        private bool blnIsSelected = false;

        public TicketModel()
        {
            this.Clear();
        }

        public void Clear()
        {
            Id = 0;
            DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
            CinemaNumber = 0;
            MovieCode = string.Empty;
            Rating = string.Empty;
            StartTime = dtNow;
            PatronCode = string.Empty;
            PatronPrice = 0m;
            SeatType = 0;
            Code = string.Empty;
            CulturalTax = 0;
            AmusementTax = 0;
            VatTax = 0;
            CurrentTime = dtNow;
            SessionName = string.Empty;
            SerialNumber = string.Empty;
            TIN = ParadisoObjectManager.GetInstance().TIN;
            PN = ParadisoObjectManager.GetInstance().PN;
            strHeader1 = ParadisoObjectManager.GetInstance().Header;
            strHeader2 = ParadisoObjectManager.GetInstance().Subheader;
            IsVoid = false;
        }

        public int Id
        {
            get { return intId; }
            set
            {
                if (value != intId)
                {
                    intId = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        public bool IsVoid
        {
            get { return blnIsVoid; }
            set
            {
                if (blnIsVoid != value)
                {
                    blnIsVoid = value;
                    NotifyPropertyChanged("IsVoid");
                }
            }
        }

        public string SerialNumber
        {
            get { return strSerialNumber; }
            set
            {
                if (strSerialNumber != value)
                {
                    strSerialNumber = value;
                    NotifyPropertyChanged("SerialNumber");
                }
            }
        }

        public string SessionName
        {
            get { return strSessionName; }
            set
            {
                if (strSessionName != value)
                {
                    strSessionName = value;
                    NotifyPropertyChanged("SessionName");
                }
            }
        }

        public string TerminalName
        {
            get { return strTerminalName; }
            set
            {
                if (strTerminalName != value)
                {
                    strTerminalName = value;
                    NotifyPropertyChanged("TerminalName");
                }
            }
        }

        public string TellerCode
        {
            get { return strTellerCode; }
            set
            {
                if (strTellerCode != value)
                {
                    strTellerCode = value;
                    NotifyPropertyChanged("TellerCode");
                }
            }
        }

        public decimal VatTax
        {
            get { return decVatTax; }
            set
            {
                if (decVatTax != value)
                {
                    decVatTax = value;
                    NotifyPropertyChanged("VatTax");
                }
            }
        }

        public decimal CulturalTax
        {
            get { return decCulturalTax; }
            set
            {
                if (decCulturalTax != value)
                {
                    decCulturalTax = value;
                    NotifyPropertyChanged("CulturalTax");
                }
            }

        }

        public decimal AmusementTax
        {
            get { return decAmusementTax; }
            set
            {
                if (decAmusementTax != value)
                {
                    decAmusementTax = value;
                    NotifyPropertyChanged("AmusementTax");
                }
            }
        }

        public int CinemaNumber
        {
            get { return intCinemaNumber; }
            set
            {
                if (intCinemaNumber != value)
                {
                    intCinemaNumber = value;
                    NotifyPropertyChanged("CinemaNumber");
                }
            }
           
        }

        public string Code
        {
            get { return strCode; }
            set
            {
                if (strCode != value)
                {
                    strCode = value;
                    NotifyPropertyChanged("Code");
                }
            }
        }

        //so that i won't use converters
        public string SeatTypeName
        {
            get { return strSeatType; }
            set
            {
                if (strSeatType != value)
                {
                    strSeatType = value;
                    NotifyPropertyChanged("SeatTypeName");
                }
            }
        }


        public int SeatType
        {
            get { return intSeatType; }
            set
            {
                if (intSeatType != value)
                {
                    intSeatType = value;
                    if (intSeatType == 1)
                        SeatTypeName = "GUARANTEED SEATING";
                    else if (intSeatType == 2)
                        SeatTypeName = "FREE SEATING (LIMITED)";
                    else if (intSeatType == 3)
                        SeatTypeName = "FREE SEATING (UNLIMITED)";
                    else
                        SeatTypeName = string.Empty;

                    NotifyPropertyChanged("SeatType");
                }
            }
        }

        public decimal PatronPrice
        {
            get { return decPatronPrice; }
            set
            {
                if (decPatronPrice != value)
                {
                    decPatronPrice = value;
                    NotifyPropertyChanged("PatronPrice");
                }
            }
        }

        public string PatronCode
        {
            get { return strPatronCode; }
            set
            {
                if (strPatronCode != value)
                {
                    strPatronCode = value;
                    NotifyPropertyChanged("PatronCode");

                }
            }
        }

        public DateTime StartTime
        {
            get { return dtStartTime; }
            set
            {
                if (dtStartTime != value)
                {
                    dtStartTime = value;
                    NotifyPropertyChanged("StartTime");
                }
            }
        }

        public DateTime CurrentTime
        {
            get { return dtCurrentTime; }
            set
            {
                if (dtCurrentTime != value)
                {
                    dtCurrentTime = value;
                    NotifyPropertyChanged("CurrentTime");
                }
            }
        }

        public string Rating
        {
            get { return strRating; }
            set
            {
                if (strRating != value)
                {
                    strRating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }

        public string MovieCode
        {
            get { return strMovieCode; }
            set
            {
                if (value != strMovieCode)
                {
                    strMovieCode = value;
                    NotifyPropertyChanged("MovieCode");
                }
            }
        }

        public string PN
        {
            get { return strPN; }
            set
            {
                if (value != strPN)
                {
                    strPN = value;
                    NotifyPropertyChanged("PN");
                }
            }
        }

        public string TIN
        {
            get { return strTIN; }
            set
            {
                if (value != strTIN)
                {
                    strTIN = value;
                    NotifyPropertyChanged("TIN");
                }
            }
        }

        public string ORNumber
        {
            get { return strORNumber; }
            set
            {
                if (value != strORNumber)
                {
                    strORNumber = value;
                    NotifyPropertyChanged("ORNumber");
                }
            }
        }

        public string Header1
        {
            get { return strHeader1; }
            set
            {
                if (value != strHeader1)
                {
                    strHeader1 = value;
                    NotifyPropertyChanged("Header1");
                }
            }
        }

        public string Header2
        {
            get { return strHeader2; }
            set
            {
                if (value != strHeader2)
                {
                    strHeader2 = value;
                    NotifyPropertyChanged("Header2");
                }
            }
        }

        public string MIN
        {
            get { return strMIN; }
            set
            {
                if (value != strMIN)
                {
                    strMIN = value;
                    NotifyPropertyChanged("MIN");
                }
            }
        }

        public string PatronDescription
        {
            get { return strPatronDescription; }
            set
            {
                if (value != strPatronDescription)
                {
                    strPatronDescription = value;
                    NotifyPropertyChanged("PatronDescription");
                }
            }
        }

        public string SeatName
        {
            get { return strSeatName; }
            set
            {
                if (value != strSeatName)
                {
                    strSeatName = value;
                    NotifyPropertyChanged("SeatName");
                }
            }
        }

        public bool IsSelected
        {
            get { return blnIsSelected; }
            set
            {
                if (value != blnIsSelected)
                {
                    blnIsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
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
