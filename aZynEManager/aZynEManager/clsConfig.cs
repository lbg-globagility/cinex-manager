using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace aZynEManager
{
    [DefaultPropertyAttribute("Cinema")]
    public class clsConfig
    {
        private string _cinemaname = String.Empty;
        private string _cinemaaddr = String.Empty;
        private string _cinemaaddr2 = String.Empty;
        private string _reportsub1 = String.Empty;
        private string _reportsub2 = String.Empty;
        private string _reportsub3 = String.Empty;
        private int _moviedefaultshare = 0;
        private DateTime _movielistcutoffdate = new DateTime();
        private int _moviedefaultintermission = 0;
        private string _cinemaaccreditation = String.Empty;
        private string _cinemaserialno = String.Empty;

        private string strTin = string.Empty;
        private string strPn = string.Empty;
        private string strPrinter = "CITIZEN";
        private string strAccreditation = String.Empty;

        private bool blnIsRemoveReservedSeat = true;

        //ADDED NEW CLASS AT CONFIG_TABLE
        private DateTime _collectionstartdate = new DateTime();

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Name of the the cinema"),
        ]
        public string CinemaName
        {
            get { return _cinemaname; }
            set { _cinemaname = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Company Name/Location of the the cinema")]
        public string CinemaAddress
        {
            get { return _cinemaaddr; }
            set { _cinemaaddr = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Address/Location of the the cinema")]
        public string CinemaAddress2
        {
            get { return _cinemaaddr2; }
            set { _cinemaaddr2 = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Accreditation No of the Company")]
        public string CinemaAccreditationNo
        {
            get { return _cinemaaccreditation; }
            set { _cinemaaccreditation = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Serial No of the Company")]
        public string CinemaSerialNo
        {
            get { return _cinemaserialno; }
            set { _cinemaserialno = value; }
        }

        [CategoryAttribute("Report"),
        DescriptionAttribute("Sub header that all reports needed")]
        public string ReportSubHeader1
        {
            get { return _reportsub1; }
            set { _reportsub1 = value; }
        }

        [CategoryAttribute("Report"),
        DescriptionAttribute("Sub header that all reports needed")]
        public string ReportSubHeader2
        {
            get { return _reportsub2; }
            set { _reportsub2 = value; }
        }

        [CategoryAttribute("Report"),
        DescriptionAttribute("Sub header that all reports needed")]
        public string ReportSubHeader3
        {
            get { return _reportsub3; }
            set { _reportsub3 = value; }
        }

        [CategoryAttribute("Movies"),
        DescriptionAttribute("Producer's default share per movie.")]
        public int MovieDefaultShare
        {
            get { return _moviedefaultshare; }
            set { _moviedefaultshare = value; }
        }

        [CategoryAttribute("Movies"),
        DescriptionAttribute("Cut-Off date for the movie database.")]
        public DateTime MovieListCutOffDate
        {
            get { return _movielistcutoffdate; }
            set { _movielistcutoffdate = value; }
        }

        [CategoryAttribute("Movies"),
        DescriptionAttribute("Intermission time for each film showing.")]
        public int MovieIntermissionTime
        {
            get { return _moviedefaultintermission; }
            set { _moviedefaultintermission = value; }
        }

        [CategoryAttribute("Movies"),
       DescriptionAttribute("Set system collection start date.")]
        public DateTime SystemCollectionStartDate
        {
            get { return _collectionstartdate; }
            set { _collectionstartdate = value; }
        }

        [CategoryAttribute("Ticket"),
        DescriptionAttribute("TIN")]
        public string TIN
        {
            get { return strTin; }
            set { strTin = value; }
        }

        [CategoryAttribute("Ticket"),
        DescriptionAttribute("PN")]
        public string PN
        {
            get { return strPn; }
            set { strPn = value; }
        }

        [CategoryAttribute("Ticket"),
        DescriptionAttribute("Printer")]
        public string Printer
        {
            get { return strPrinter; }
            set { strPrinter = value; }
        }

        [CategoryAttribute("Ticket"),
        DescriptionAttribute("Remove Reserved Seats on Unreserve command.")]
        public bool IsRemoveReservedSeat
        {
            get { return blnIsRemoveReservedSeat; }
            set { blnIsRemoveReservedSeat = value; }
        }
    }
}
