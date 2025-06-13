﻿using System;
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

        private string strSupplierName = string.Empty;
        private string strSupplierAddress = string.Empty;
        private string strSupplierTIN = string.Empty;

        private string strAccreditationNumber = "XXX-XXXXXXXXX-XXXXXX-XXXXX";
        private string strDateIssued = string.Empty;
        private string strValidDate = string.Empty;

        //private string strPermitNumber = string.Empty; //"XXXX-XXX-XXXXXX-XXX";

        private string strServerSerialNumber = "XXXXXXXXXXX-X";
        private string strPOSNumber = "XXXXXXXX";

        private string strMIN = string.Empty;
        private string strTIN = "XXX-XXX-XXX-XXX";
        private string strPN = string.Empty; //"XXXX-XXX-XXXXX-XXX";

        private int intCinemaNumber = 0;
        private string strMovieCode = string.Empty;
        private string strRating = string.Empty;
        private DateTime dtStartTime;
        private string strPatronCode = string.Empty;
        private string strPatronName = string.Empty;
        private decimal decPatronPrice = 0m;
        private decimal decBasePrice = 0m;

        private decimal decOrdinancePrice = 0m;
        private decimal decFoodSurchargePrice = 0m;
        private decimal decNonFoodSurchargePrice = 0m;
        private decimal decSurchargePrice = 0m;
        private bool blnIsPremium = false;

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
        private string strGroupName = string.Empty;
        private string strSectionName = string.Empty;
        private bool blnIsSelected = false;

        private bool blnIsHandicapped = false;

        private int intPaymentMode = 0;
        private decimal decCash = 0;
        private decimal decCC = 0;
        private decimal decGC = 0;

        private string strBuyerLastName = string.Empty;
        private string strBuyerFirstName = string.Empty;
        private string strBuyerMiddleInitial = string.Empty;

        private string strBuyerAddress = string.Empty;
        private string strBuyerMunicipality = string.Empty;
        private string strBuyerProvince = string.Empty;
        private string strBuyerTIN = string.Empty;
        private string strBuyerIDNum = string.Empty;
        private string strOfficialReceiptCaption;
        private string strHeader4;

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
            PatronName = string.Empty;
            PatronPrice = 0m;
            BasePrice = 0m;
            OrdinancePrice = 0m;
            FoodSurchargePrice = 0m;
            NonFoodSurchargePrice = 0m;
            SurchargePrice = 0m;
            IsPremium = false;
            SeatType = 0;
            Code = string.Empty;
            CulturalTax = 0;
            AmusementTax = 0;
            VatTax = 0;
            CurrentTime = dtNow;
            SessionName = string.Empty;
            SerialNumber = string.Empty;
            TIN = ParadisoObjectManager.GetInstance().TIN;
            PN = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("PN_{0}", Environment.MachineName), string.Empty); 
            MIN = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("MIN_{0}", Environment.MachineName), string.Empty);
            strHeader1 = ParadisoObjectManager.GetInstance().Header;
            strHeader2 = ParadisoObjectManager.GetInstance().Subheader;
            strHeader3 = ParadisoObjectManager.GetInstance().Subheader1;
            var fsdfsd = strHeader3.Split('\n');
            if ((fsdfsd?.Count() ?? 0) > 1)
            {
                strHeader3 = fsdfsd.FirstOrDefault();
                strHeader4 = fsdfsd.LastOrDefault();
            }

            strSupplierName = ParadisoObjectManager.GetInstance().GetConfigValue("SUPPLIER NAME", string.Empty);
            strSupplierAddress = ParadisoObjectManager.GetInstance().GetConfigValue("SUPPLIER ADDRESS", string.Empty);
            strSupplierTIN = ParadisoObjectManager.GetInstance().GetConfigValue("SUPPLIER TIN", string.Empty); 
            strAccreditationNumber = ParadisoObjectManager.GetInstance().GetConfigValue("ACCREDITATION", string.Empty); //"XXX-XXXXXXXXX-XXXXXX-XXXXX");
            strDateIssued = ParadisoObjectManager.GetInstance().GetConfigValue("DATE ISSUED", string.Empty);
            strValidDate = ParadisoObjectManager.GetInstance().GetConfigValue("VALID DATE", string.Empty);
            strServerSerialNumber = ParadisoObjectManager.GetInstance().GetConfigValue("SERVER SERIAL",  string.Empty); //"XXXXXXXXXXX-X");
            strPOSNumber = ParadisoObjectManager.GetInstance().GetConfigValue(string.Format("POS_NO_{0}", Environment.MachineName), string.Empty);

            BuyerLastName = string.Empty;
            BuyerFirstName = string.Empty;
            BuyerMiddleInitial = string.Empty;
            BuyerAddress = string.Empty;
            BuyerMunicipality = string.Empty;
            BuyerProvince = string.Empty;
            BuyerTIN = string.Empty;
            BuyerIDNum = string.Empty;

            IsVoid = false;

            strOfficialReceiptCaption = SettingPage.OFFICIAL_RECEIPT_CAPTION;
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
                    //NotifyPropertyChanged("OrdinancePrice");
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
                    //NotifyPropertyChanged("OrdinancePrice");
                }
            }
        }

        public decimal OrdinancePrice
        {
            get { return decOrdinancePrice; }
            set
            {
                if (decOrdinancePrice != value)
                {
                    decOrdinancePrice = value;
                    NotifyPropertyChanged("OrdinancePrice");
                }
            }
        }

        public decimal FoodSurchargePrice
        {
            get { return decFoodSurchargePrice; }
            set
            {
                if (decFoodSurchargePrice != value)
                {
                    decFoodSurchargePrice = value;
                    NotifyPropertyChanged("FoodSurchargePrice");
                }
            }
        }

        public decimal NonFoodSurchargePrice
        {
            get { return decNonFoodSurchargePrice; }
            set
            {
                if (decNonFoodSurchargePrice != value)
                {
                    decNonFoodSurchargePrice = value;
                    NotifyPropertyChanged("NonFoodSurchargePrice");
                }
            }
        }

        public decimal SurchargePrice
        {
            get { return decSurchargePrice; }
            set
            {
                if (decSurchargePrice != value)
                {
                    decSurchargePrice = value;
                    NotifyPropertyChanged("SurchargePrice");
                }
            }
        }

        public bool IsPremium
        {
            get { return blnIsPremium; }
            set
            {
                if (blnIsPremium != value)
                {
                    blnIsPremium = value;
                    NotifyPropertyChanged("IsPremium");
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

        public string PatronName
        {
            get { return strPatronName; }
            set
            {
                if (strPatronName != value)
                {
                    strPatronName = value;
                    NotifyPropertyChanged("PatronName");

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

        public string Header4
        {
            get { return strHeader4; }
            set
            {
                if (value != strHeader4)
                {
                    strHeader4 = value;
                    NotifyPropertyChanged("Header4");
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

        public string GroupName
        {
            get { return strGroupName; }
            set
            {
                if (value != strGroupName)
                {
                    strGroupName = value;
                    NotifyPropertyChanged("GroupName");
                }
            }
        }

        public string SectionName
        {
            get { return strSectionName; }
            set
            {
                if (value != strSectionName)
                {
                    strSectionName = value;
                    NotifyPropertyChanged("SectionName");
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

        public string SupplierName
        {
            get { return strSupplierName; }
            set
            {
                if (value != strSupplierName)
                {
                    strSupplierName = value;
                    NotifyPropertyChanged("SupplierName");
                }
            }
        }

        public string SupplierAddress
        {
            get { return strSupplierAddress; }
            set
            {
                if (value != strSupplierAddress)
                {
                    strSupplierAddress = value;
                    NotifyPropertyChanged("SupplierAddress");
                }
            }
        }

        public string SupplierTIN
        {
            get { return strSupplierTIN; }
            set
            {
                if (value != strSupplierTIN)
                {
                    strSupplierTIN = value;
                    NotifyPropertyChanged("SupplierTIN");
                }
            }
        }

        public string Supplier
        {
            get
            {
                StringBuilder strSupplier = new StringBuilder();
                if (SupplierName != string.Empty)
                    strSupplier.Append(SupplierName);

                //do not display if no supplier name is set

                if (SupplierAddress != string.Empty && strSupplier.Length > 0)
                    strSupplier.Append(string.Format(", {0}", SupplierAddress));
                /*
                if (SupplierTIN != string.Empty && strSupplier.Length > 0)
                    strSupplier.Append(string.Format(", TIN No. {0}", SupplierTIN));
                */
                return strSupplier.ToString();
            }
        }

        public string Accreditation
        {
            get
            {
                StringBuilder strAccreditation = new StringBuilder();
                if (AccreditationNumber != string.Empty)
                    strAccreditation.Append(AccreditationNumber);
                //if (strAccreditation.Length > 0 && DateIssued != string.Empty && ValidDate != string.Empty)
                //    strAccreditation.Append(string.Format(" ({0}-{1})", DateIssued, ValidDate));
                //else
                if (strAccreditation.Length > 0 && DateIssued != string.Empty && ValidDate == string.Empty) strAccreditation.Append(string.Format(" ({0})", DateIssued));

                return strAccreditation.ToString();
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

        public string DateIssued
        {
            get { return strDateIssued; }
            set
            {
                if (value != strDateIssued)
                {
                    strDateIssued = value;
                    NotifyPropertyChanged("DateIssued");
                }
            }
        }

        public string ValidDate
        {
            get { return strValidDate; }
            set
            {
                if (value != strValidDate)
                {
                    strValidDate = value;
                    NotifyPropertyChanged("ValidDate");
                }
            }
        }

        public string BuyerLastName
        {
            get { return strBuyerLastName; }
            set
            {
                if (value != strBuyerLastName)
                {
                    strBuyerLastName = value;
                    NotifyPropertyChanged("BuyerLastName");
                }
            }
        }

        public string BuyerFirstName
        {
            get { return strBuyerFirstName; }
            set
            {
                if (value != strBuyerFirstName)
                {
                    strBuyerFirstName = value;
                    NotifyPropertyChanged("BuyerFirstName");
                }
            }
        }

        public string BuyerMiddleInitial
        {
            get { return strBuyerMiddleInitial; }
            set
            {
                if (value != strBuyerMiddleInitial)
                {
                    strBuyerMiddleInitial = value;
                    NotifyPropertyChanged("BuyerMiddleInitial");
                }
            }
        }

        public string BuyerAddress
        {
            get { return strBuyerAddress; }
            set
            {
                if (value != strBuyerAddress)
                {
                    strBuyerAddress = value;
                    NotifyPropertyChanged("BuyerAddress");
                }
            }
        }

        public string BuyerMunicipality
        {
            get { return strBuyerMunicipality; }
            set
            {
                if (value != strBuyerMunicipality)
                {
                    strBuyerMunicipality = value;
                    NotifyPropertyChanged("BuyerMunicipality");
                }
            }
        }

        public string BuyerProvince
        {
            get { return strBuyerProvince; }
            set
            {
                if (value != strBuyerProvince)
                {
                    strBuyerProvince = value;
                    NotifyPropertyChanged("BuyerProvince");
                }
            }
        }

        public string BuyerName
        {
            get
            {
                StringBuilder strBuyerNameAddress = new StringBuilder();
                if (BuyerFirstName != null && BuyerFirstName != string.Empty)
                    strBuyerNameAddress.Append(BuyerFirstName);
                if (BuyerMiddleInitial != null && BuyerMiddleInitial != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerMiddleInitial);
                    if (!BuyerMiddleInitial.EndsWith("."))
                        strBuyerNameAddress.Append(".");
                }

                if (BuyerLastName != null && BuyerLastName != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerLastName);
                }

                return strBuyerNameAddress.ToString();
            }
        }

        public string BuyerFullAddress
        {
            get
            {
                StringBuilder strBuyerNameAddress = new StringBuilder();
                if (BuyerAddress != null && BuyerAddress != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerAddress);
                }
                if (BuyerMunicipality != null && BuyerMunicipality != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerMunicipality);
                }
                if (BuyerProvince != null && BuyerProvince != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerProvince);
                }

                return strBuyerNameAddress.ToString();
            }
        }

        public string BuyerNameAddress
        {
            get
            {
                StringBuilder strBuyerNameAddress = new StringBuilder();
                if (BuyerFirstName != null && BuyerFirstName != string.Empty)
                    strBuyerNameAddress.Append(BuyerFirstName);
                if (BuyerMiddleInitial != null && BuyerMiddleInitial != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerMiddleInitial);
                    if (!BuyerMiddleInitial.EndsWith("."))
                        strBuyerNameAddress.Append(".");
                }

                if (BuyerLastName != null && BuyerLastName != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerLastName);
                }

                //address
                if (strBuyerNameAddress.Length > 0)
                    strBuyerNameAddress.Append(",");

                if (BuyerAddress != null && BuyerAddress != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerAddress);
                }
                if (BuyerMunicipality != null && BuyerMunicipality != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerMunicipality);
                }
                if (BuyerProvince != null && BuyerProvince != string.Empty)
                {
                    if (strBuyerNameAddress.Length > 0)
                        strBuyerNameAddress.Append(" ");
                    strBuyerNameAddress.Append(BuyerProvince);
                }

                return strBuyerNameAddress.ToString();
            }
        }

        public string BuyerTIN
        {
            get { return strBuyerTIN; }
            set
            {
                if (value != strBuyerTIN)
                {
                    strBuyerTIN = value;
                    NotifyPropertyChanged("BuyerTIN");
                }
            }
        }

        public string BuyerIDNum
        {
            get { return strBuyerIDNum; }
            set
            {
                if (value != strBuyerIDNum)
                {
                    strBuyerIDNum = value;
                    NotifyPropertyChanged("BuyerIDNum");
                }
            }
        }

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

        public int PaymentMode
        {
            get { return intPaymentMode; }
            set
            {
                if (value != intPaymentMode)
                {
                    intPaymentMode = value;
                    NotifyPropertyChanged("PaymentMode");
                }
            }
        }

        public decimal Cash
        {
            get { return decCash; }
            set
            {
                if (value != decCash)
                {
                    decCash = value;
                    NotifyPropertyChanged("Cash");
                }
            }
        }

        public decimal CreditCard
        {
            get { return decCC; }
            set
            {
                if (value != decCC)
                {
                    decCC = value;
                    NotifyPropertyChanged("CreditCard");
                }
            }
        }

        public decimal GiftCertificate
        {
            get { return decGC; }
            set
            {
                if (value != decGC)
                {
                    decGC = value;
                    NotifyPropertyChanged("GiftCertificate");
                }
            }
        }

        public bool IsSC
        {
            get
            {
                return PatronName.IndexOf("SC ") != -1;
            }
        }

        public bool IsPWD
        {
            get
            {
                return PatronName.IndexOf("PWD ") != -1;
            }
        }

        public decimal DiscountRate
        {
            get
            {
                decimal decDiscountRate = 0;
                if (IsSC || IsPWD)
                {
                    int idx0 = PatronName.LastIndexOf("(");
                    int idx1 = PatronName.IndexOf("%)");
                    if (idx0 != -1 && idx1 != -1)
                    {
                        decimal.TryParse(PatronName.Substring(idx0+1, idx1 - idx0 - 1), out decDiscountRate);
                        decDiscountRate /= 100;
                    }
                }

                return decDiscountRate;
            }
        }

        public decimal FullPrice
        {
            get
            {
                return BasePrice / (1 - DiscountRate);
            }
        }

        public decimal Discount
        {
            get
            {
                return FullPrice * DiscountRate;
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

        public string OfficialReceiptCaption
        {
            get { return strOfficialReceiptCaption; }
            set
            {
                strOfficialReceiptCaption = string.IsNullOrEmpty(value) ?
                    SettingPage.SALES_INVOICE_CAPTION_BIR_COMPLIANCE :
                    value;
            }
        }
    }
}
