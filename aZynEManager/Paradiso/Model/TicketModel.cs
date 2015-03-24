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
        private string strHeader3 = "HEADER 3";

        private string strAccreditationNumber = "XXX-XXXXXXXXX-XXXXXX-XXXXX";
        //private string strPermitNumber = "XXXX-XXX-XXXXXX-XXX";
        private string strServerSerialNumber = "XXXXXXXXXXX-X";
        private string strPOSNumber = "XXXXXXXX";

        private string strMIN = string.Empty;
        private string strTIN = "XXX-XXX-XXX-XXX";
        private string strPN = "XXXX-XXX-XXXXX-XXX";

        private int intCinemaNumber = 0;
        private string strMovieCode = string.Empty;
        private string strRating = string.Empty;
        private DateTime dtStartTime;
        private string strPatronCode = string.Empty;
        private decimal decPatronPrice = 0m;
        private decimal decBasePrice = 0m;

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

        private bool blnIsHandicapped = false;

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
            BasePrice = 0m;
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
            strHeader3 = ParadisoObjectManager.GetInstance().Subheader1;
            strAccreditationNumber = ParadisoObjectManager.GetInstance().GetConfigValue("ACCREDITATION", string.Empty); //"XXX-XXXXXXXXX-XXXXXX-XXXXX");
            //strPermitNumber = ParadisoObjectManager.GetInstance().GetConfigValue("PERMIT", "XXXX-XXX-XXXXXX-XXX");
            strServerSerialNumber = ParadisoObjectManager.GetInstance().GetConfigValue("SERVER SERIAL",  string.Empty); //"XXXXXXXXXXX-X");
            strPOSNumber = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("POS_NO_{0}", Environment.MachineName), string.Empty);

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

        private void UpdateSeatTypeName()
        {
            if (SeatType == 1)
            {
                StringBuilder _strSeatName = new StringBuilder();
                if (SeatName != string.Empty)
                    _strSeatName.Append(SeatName);
                if (blnIsHandicapped)
                {
                    if (_strSeatName.Length > 0)
                        _strSeatName.Append(" - ");
                    _strSeatName.Append("Disabled");
                }
                if (_strSeatName.Length > 0)
                {
                    _strSeatName.Insert(0, " (");
                    _strSeatName.Append(")");
                }

                bool blnIsTicketFormatB = false;
                if (ParadisoObjectManager.GetInstance().GetConfigValue("TICKET_FORMAT", "A") == "B")
                    blnIsTicketFormatB = true;
                if (ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("TICKET_FORMAT_{0}", Environment.MachineName), "A") == "B")
                    blnIsTicketFormatB = true;

                if (blnIsTicketFormatB)
                    SeatTypeName = "RESERVED SEATING"; 
                else
                    SeatTypeName = string.Format("RESERVED SEATING{0}", _strSeatName.ToString());

            }
            else if (SeatType == 2)
                SeatTypeName = "FREE SEATING (GUARANTEED)";
            else if (SeatType == 3)
                SeatTypeName = "FREE SEATING (UNLIMITED)";
            else
                SeatTypeName = string.Empty;

        }

        public int SeatType
        {
            get { return intSeatType; }
            set
            {
                if (intSeatType != value)
                {
                    intSeatType = value;
                    this.UpdateSeatTypeName();
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
                    NotifyPropertyChanged("OrdinancePrice");
                }
            }
        }

        public decimal BasePrice
        {
            get { return decBasePrice; }
            set
            {
                if (decBasePrice != value)
                {
                    decBasePrice = value;
                    NotifyPropertyChanged("BasePrice");
                    NotifyPropertyChanged("OrdinancePrice");
                }
            }
        }

        public decimal OrdinancePrice
        {
            get
            {
                decimal decOrdinancePrice = this.PatronPrice - this.BasePrice;
                if (decOrdinancePrice < 0m)
                    decOrdinancePrice = 0m;
                return decOrdinancePrice;
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

        public string Header3
        {
            get { return strHeader3; }
            set
            {
                if (value != strHeader3)
                {
                    strHeader3 = value;
                    NotifyPropertyChanged("Header3");
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
                    UpdateSeatTypeName();
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

        public bool IsHandicapped
        {
            get { return blnIsHandicapped; }
            set
            {
                if (blnIsHandicapped != value)
                {
                    blnIsHandicapped = value;
                    UpdateSeatTypeName();
                    NotifyPropertyChanged("IsHandicapped");
                }
            }
        }

        public string AccreditationNumber
        {
            get { return strAccreditationNumber; }
            set
            {
                if (value != strAccreditationNumber)
                {
                    strAccreditationNumber = value;
                    NotifyPropertyChanged("AccreditationNumber");
                }
            }
        }

        /*
        public string PermitNumber
        {
            get { return strPermitNumber; }
            set
            {
                if (value != strPermitNumber)
                {
                    strPermitNumber = value;
                    NotifyPropertyChanged("PermitNumber");
                }
            }
        }
        */

        public string ServerSerialNumber
        {
            get { return strServerSerialNumber; }
            set
            {
                if (value != strServerSerialNumber)
                {
                    strServerSerialNumber = value;
                    NotifyPropertyChanged("ServerSerialNumber");
                }
            }
        }

        public string POSNumber
        {
            get { return strPOSNumber; }
            set
            {
                if (value != strPOSNumber)
                {
                    strPOSNumber = value;
                    NotifyPropertyChanged("POSNumber");
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
