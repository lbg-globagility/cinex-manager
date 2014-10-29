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
        private string _reportsub1 = String.Empty;
        private string _reportsub2 = String.Empty;
        private string _reportsub3 = String.Empty;
        private int _moviedefaultshare = 0;
        private DateTime _movielistcutoffdate = new DateTime();
        private int _moviedefaultintermission = 0;

        private string strTin = string.Empty;
        private string strPn = string.Empty;

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Name of the the cinema"),
        ]
        public string CinemaName
        {
            get { return _cinemaname; }
            set { _cinemaname = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("Address/Location of the the cinema")]
        public string CinemaAddress
        {
            get { return _cinemaaddr; }
            set { _cinemaaddr = value; }
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

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("TIN")]
        public string TIN
        {
            get { return strTin; }
            set { strTin = value; }
        }

        [CategoryAttribute("Cinema"),
        DescriptionAttribute("PN")]
        public string PN
        {
            get { return strPn; }
            set { strPn = value; }
        }
    }
}
