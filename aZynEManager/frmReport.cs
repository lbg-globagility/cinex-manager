using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using System.Xml;
using System.Data.Odbc;

namespace aZynEManager
{
    public partial class frmReport : Form
    {
        frmMain m_frmM;
        clscommon m_clscom;
        public string xmlfile = String.Empty;
        string expFileNm = String.Empty;
        public int _intCinemaID = -1;
        public DateTime _dtStart = new DateTime();
        public DateTime _dtEnd = new DateTime();
       
        public string _strDistributor = String.Empty;
        public static ArrayList queryList = new ArrayList();
        //melvin 2014 10-17
        public string strCinema = null;
        public string account = null;
        public string rp05distributor = null;
        public static string rp08cinema = null;
        public static string rp08movie = null;
        public string module = null;

        //RMB 11-10-2014 added valiables start
        public int _intMovieID = -1;
        public DateTime _dtMovieDate = new DateTime();

        //RMB 11.11.2014 added new variables
        public double _gttoday = 0;
        public double _gtyesterday = 0;
        public string m_companynm = String.Empty;
        public clsConfig m_clsconfig;
        public int _posid = -1;
        public string _posterminal = String.Empty;
        public int _poschangefund = 0;
        public int _intWithUtil = -1;//added 5-31-2022
        public int _intWithQtyBreakdown = -1;//added 5-31-2022

        public frmReport()
        {
            InitializeComponent();
        }

        //RMB 11.11.2014
        public string setDistCode
        {
            set { _strDistributor = value; }
        }

        // RMB 11-10-2014 start - added 
        public int setMovieID
        {
            set { _intMovieID = value; }
        } 
        // RMB 11-10-2014 end 

        //JMBC 10152014

        public DateTime setDate
        {
            set { _dtStart = value; }
        }
        public DateTime setEndDate
        {
            set { _dtEnd = value; }
        }
        //melvin 2014 17 10
        public string setCinema
        {
            set { strCinema = value; }
        }
                
        public int setPOS//added 10.3.2018
        {
            set { _posid = value; }
        }

        public string setTerminal//added 10.3.2018
        {
            set { _posterminal = value; }
        }

        public int setChangeValue//added 10.3.2018
        {
            set { _poschangefund = value; }
        }

        public int setWithUtilValue//added 5.31.2022
        {
            set { _intWithUtil = value; }
        }

        public int setWithQtyBreakdownValue//added 5.31.2022
        {
            set { _intWithQtyBreakdown = value; }
        }

        public void frmInit(frmMain frm, clscommon cls, string reportcode)
        {
            m_frmM = frm;
            m_clscom = cls;
            m_clsconfig = m_clscom.m_clscon;
            //MessageBox.Show(module + "-" + account);
            //return;
            frmInitDbase(reportcode);
          
        }

        public void frmInitDbase(string reportcode)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                StringBuilder sqry = new StringBuilder();
                switch (reportcode)
                {
                    case "RP03":
                        sqry.Append("SELECT f.name, e.title, COUNT(cinema_seat_id) quantity, SUM(a.base_price) sales, g.system_value, h.name report_name, d.movie_date ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, ticket b, movies_schedule_list c, movies_schedule d, movies e, cinema f, config_table g, report h ");
                        sqry.Append("WHERE a.ticket_id = b.id ");
                        sqry.Append("AND a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = c.id ");
                        sqry.Append("AND c.movies_schedule_id = d.id ");
                        sqry.Append("AND d.movie_id = e.id ");
                        sqry.Append("AND d.cinema_id = f.id ");
                        sqry.Append(String.Format("AND g.system_code = '{0}' ", "001"));
                        sqry.Append(String.Format("AND h.code = '{0}' ", reportcode));
                        sqry.Append(String.Format("AND d.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append("GROUP BY f.id, e.id ");
                        sqry.Append("ORDER BY f.in_order");
                       
                        break;
                        
                    case "RP01":
                        //rmb REMARKED FOR THE NEW REPORT
                        /*
                        //RMB added to the report to limit the qty with > o values only
                        sqry.Append("select * from (");
                        //melvin 11/6/2014
                        sqry.Append("select @user_id:= g.user_id user, a.code, @patron:= ");
                        sqry.Append("a.name as PATRON,  @cinema:= c.name CINEMA, ");
                        sqry.Append("b.price PRICE, @c:= (select count(a.cinema_seat_id)");
                        sqry.Append("from ticket z inner join  movies_schedule_list_reserved_seat ");
                        sqry.Append("a on z.id=a.ticket_id inner join movies_schedule_list b on ");
                        sqry.Append("a.movies_schedule_list_id= b.id inner join movies_schedule ");
                        sqry.Append("c on b.movies_schedule_id=c.id inner join cinema d ");
                        sqry.Append("on d.id= c.cinema_id inner join movies_schedule_list_patron");
                        sqry.Append(" e on e.id = a.patron_id inner join patrons f on f.id = ");
                        // remarked 11.21.2014 update on report
                        //sqry.Append("e.patron_id where  a.status = 1 and f.name=@patron and ");
                        //sqry.Append("d.name=@cinema and g.user_id=@user_id AND c.movie_date = ");
                        sqry.Append("e.patron_id inner join users g on z.user_id = g.id where a.status = 1 and f.name=@patron and ");
                        sqry.Append("d.name=@cinema and g.id=@user_id AND c.movie_date = ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}'", _dtStart));
                        sqry.Append(") as QTY, (@c*b.price) as `TOTALSALES`, d.movie_date, ");
                        sqry.Append("i.userid, h.system_value from patrons a inner join ");
                        sqry.Append("cinema_patron b on a.id=b.patron_id inner join cinema");
                        sqry.Append(" c on b.cinema_id = c.id inner join movies_schedule d");
                        sqry.Append(" on d.cinema_id = c.id inner join movies_schedule_list ");
                        sqry.Append("e on e.movies_schedule_id = d.id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat f on f.movies_schedule_list_id= ");
                        sqry.Append("e.id inner join ticket g on e.id= g.movies_schedule_list_id ");
                        sqry.Append("inner join config_table h inner join users i on ");
                        sqry.Append("i.id= g.user_id where d.movie_date=");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' and i.userid=",_dtStart));
                        sqry.Append(string.Format("'{0}' and  h.system_code='001' ",account));
                        sqry.Append("and f.status=1 group by a.name, c.name order by c.name");  
                        // RMB 11.21.2014 added sql query for the > 0 values visible only
                        sqry.Append(") table1 where QTY > 0");*/


                        //3.26.2015 for update sa opis
                        /*sqry.Append("select d.id user, f.code, f.name PATRON, i.name CINEMA, b.base_price PRICE,  ");
                        sqry.Append("count(b.cinema_seat_id) QTY, sum(b.base_price) TOTALSALES, sum(b.ordinance_price) TOTALORDINANCE, ");
                        sqry.Append("sum(b.surcharge_price) TOTALSURCHARGE, sum(b.base_price) + sum(b.ordinance_price) + sum(b.surcharge_price) GRANDTOTAL,h.movie_date,  ");
                        sqry.Append("d.userid, j.system_value, k.name reportname ");
                        sqry.Append("from movies_schedule_list_reserved_seat b, ticket c, ");
                        sqry.Append("users d, movies_schedule_list_patron e, patrons f, movies_schedule_list g, ");
                        sqry.Append("movies_schedule h, cinema i, config_table j, report k ");
                        sqry.Append("where b.movies_schedule_list_id in ");
                        sqry.Append("(select a.id from movies_schedule_list a ");
                        sqry.Append(String.Format("where a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ",_dtStart.AddDays(1)));
                        sqry.Append("and a.status = 1) ");
                        sqry.Append("and b.ticket_id = c.id ");
                        sqry.Append("and b.movies_schedule_list_id = g.id ");
                        sqry.Append("and c.user_id = d.id ");
                        sqry.Append(String.Format("and d.userid = '{0}' ",account)); 
                        sqry.Append("and d.system_code = 2 ");
                        sqry.Append("and d.status = 1 ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and b.patron_id = e.id ");
                        sqry.Append("and e.patron_id = f.id ");
                        sqry.Append("and g.movies_schedule_id = h.id ");
                        sqry.Append("and h.cinema_id = i.id ");
                        sqry.Append("and j.system_code = '001' ");
                        sqry.Append("and k.id = 1 ");
                        sqry.Append("group by h.cinema_id, e.patron_id ");
                        sqry.Append("order by h.cinema_id, f.code asc");*/
                        //1.26.2016
                        sqry.Append("select d.id user, f.code, f.name PATRON, i.name CINEMA, b.base_price PRICE, ");
                        sqry.Append("count(b.cinema_seat_id) QTY, sum(b.base_price) TOTALSALES, sum(b.ordinance_price) TOTALORDINANCE, ");
                        sqry.Append("sum(b.surcharge_price) TOTALSURCHARGE, sum(b.base_price) + sum(b.ordinance_price) + sum(b.surcharge_price) GRANDTOTAL, ");
                        sqry.Append("h.movie_date, c.ticket_datetime, ");
                        sqry.Append("d.userid, j.system_value, k.name reportname, ");
                        sqry.Append("(case when table1.patron_id is null then 0 else sum(table1.amount_val) end) FOOD ");//added 8.1.2018
                        sqry.Append("from movies_schedule_list_reserved_seat b, ticket c, ");
                        sqry.Append("users d, patrons f, movies_schedule_list g, ");
                        sqry.Append("movies_schedule h, cinema i, config_table j, report k, ");
                        sqry.Append("movies_schedule_list_patron e ");
                        sqry.Append("left join(SELECT l.id, l.patron_id, m.amount_val FROM patrons_surcharge l, surcharge_tbl m ");//added 8.1.2018
                        sqry.Append("where l.surcharge_id = m.id and m.details like '%FOOD%') table1 on table1.patron_id = e.patron_id ");//added 8.1.2018
                        sqry.Append("where b.movies_schedule_list_id = c.movies_schedule_list_id ");
                        sqry.Append(String.Format("and DATE(c.ticket_datetime) = '{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append("and DATE(g.start_time) >= DATE(c.ticket_datetime) ");
                        sqry.Append("and c.status = 1 ");
                        sqry.Append("and b.ticket_id = c.id ");
                        sqry.Append("and b.movies_schedule_list_id = g.id ");
                        sqry.Append("and c.user_id = d.id ");
                        sqry.Append(String.Format("and d.userid = '{0}' ",account));
                        sqry.Append("and d.system_code = 2 ");
                        sqry.Append("and d.status = 1 ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and b.patron_id = e.id ");
                        sqry.Append("and e.patron_id = f.id ");
                        sqry.Append("and g.movies_schedule_id = h.id ");
                        sqry.Append("and h.cinema_id = i.id ");
                        sqry.Append("and j.system_code = '001' ");
                        sqry.Append("and k.id = 1 ");
                        sqry.Append("group by DATE(g.start_time),h.cinema_id, e.patron_id ");
                        sqry.Append("order by h.cinema_id, f.code asc");
                        break;
                    case "RP02":
                        //RMB 11.28.2014 REMARKED
                        /*//melvin 11/7/2014
                        sqry.Append("SELECT e.title, e.no_of_days, ");
                        sqry.Append("f.no_of_screenings, total_seats_taken, ");
                        sqry.Append("total_available_seats,  total_ticket_sales,");
                        sqry.Append("(total_seats_taken/total_available_seats) * 100 util ");
                        sqry.Append("FROM ( SELECT a.id, code, title, rating_id, COUNT");
                        sqry.Append("(movie_date) no_of_days FROM movies a, movies_schedule b ");
                        sqry.Append("WHERE a.id = b.movie_id  AND b.movie_date BETWEEN ");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtStart) + " AND " );
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtEnd) +" GROUP BY a.id ) e, ");
                        sqry.Append("(SELECT movie_id, COUNT(c.id) no_of_screenings FROM ");
                        sqry.Append(" movies_schedule_list c, movies_schedule d WHERE ");
                        sqry.Append("c.movies_schedule_id =d.id AND  d.movie_date BETWEEN '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' AND '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) +"' GROUP BY movie_id ) f, ");
                        sqry.Append("(SELECT j.movie_id, COUNT(h.cinema_seat_id) ");
                        sqry.Append("total_seats_taken, SUM(h.price) total_ticket_sales ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat h, ");
                        sqry.Append("movies_schedule_list i, movies_schedule j ");
                        sqry.Append("WHERE h.movies_schedule_list_id = i.id AND ");
                        sqry.Append(" i.movies_schedule_id = j.id AND  j.movie_date BETWEEN  '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' AND '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) +"' and h.status=1 GROUP BY movie_id ) ");
                        sqry.Append("g,(SELECT movie_id, SUM(available_seats) total_available_seats ");
                        sqry.Append("FROM (SELECT movie_id, n.capacity * COUNT(l.id) ");
                        sqry.Append("  available_seats FROM movies_schedule_list l, ");
                        sqry.Append("movies_schedule m, cinema n WHERE l.movies_schedule_id");
                        sqry.Append("=m.id AND m.cinema_id = n.id AND m.movie_date BETWEEN  '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' AND '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) + "' GROUP BY movie_id, ");
                        sqry.Append("cinema_id) o GROUP BY movie_id) k WHERE e.id = f.movie_id AND ");
                        sqry.Append("e.id = g.movie_id AND e.id = k.movie_id;");
                        _dtEnd.AddDays(-1);*/
                        //sqry.Append(String.Format("set @startdate:= '{0:yyyy/MM/dd}'; ",));
                        //sqry.Append(String.Format("set @enddate:= '{0:yyyy/MM/dd}'; ", ));
                        //sqry.Append("select moviecode, movietitle, dayshown, sum(screen) screen, sum(seattaken) seattaken,  ");//remarked 3.9.2016
                        //sqry.Append("select moviecode, movietitle, sum(dayshown) as dayshown, sum(screen) screen, sum(seattaken) seattaken,  ");//remarked 7.5.2017 for days shown
                        sqry.Append("select moviecode, movietitle, dayshown, sum(screen) screen, sum(seattaken) seattaken,  ");
                        sqry.Append("sum(seattotal) seattotal, sum(totalprice) totalprice, ((sum(seattaken)/sum(seattotal)) * 100) util, ");
                        sqry.Append("system_value, reportname ");
                        //sqry.Append(String.Format("from (select e.code moviecode, e.title movietitle, (DATEDIFF('{0:yyyy/MM/dd}','{1:yyyy/MM/dd}') + 1) dayshown, count(distinct(c.id)) screen, ", _dtEnd, _dtStart));//remarked 3.9.2016
                        sqry.Append(String.Format("from (select e.code moviecode, e.title movietitle, count(distinct(d.movie_date)) as dayshown, count(distinct(c.id)) screen, ", _dtEnd, _dtStart));
                        sqry.Append("count(b.cinema_seat_id) seattaken, f.capacity * count(distinct(c.id)) seattotal, ");
                        sqry.Append("sum(b.base_price) totalprice, j.system_value, k.name reportname ");
                        sqry.Append("from movies_schedule_list_reserved_seat b, movies_schedule_list c, ");
                        sqry.Append("movies_schedule d, movies e, cinema f, config_table j, report k ");
                        sqry.Append("where b.movies_schedule_list_id in ");
                        sqry.Append("(select a.id from movies_schedule_list a ");
                        sqry.Append(String.Format("where a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ",_dtEnd.AddDays(1)));
                        sqry.Append("and a.status = 1) ");
                        sqry.Append("and b.movies_schedule_list_id = c.id ");
                        sqry.Append("and c.movies_schedule_id = d.id ");
                        sqry.Append("and d.movie_id = e.id ");
                        sqry.Append("and d.cinema_id = f.id ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and j.system_code = '001' ");
                        sqry.Append("and k.id = 2 ");
                        sqry.Append("group by e.code,e.title ");//added 1.30.2017 ,e.title //removed 10.12.2017 ,d.cinema_id
                        sqry.Append("order by e.title asc) tbl1  ");
                        sqry.Append("group by moviecode,movietitle ");//added 1.30.2017 ,movietitle
                        sqry.Append("order by movietitle");
                        break;
                    case "RP04":
                        //melvin 11/11/2014 
                        //RMB remarked 11.25.2014
                        /*sqry.Append("select @nam:= a.name as pname, @cine:= c.name as cname, @user:= ");
                        sqry.Append("j.userid as user, b.price PRICE, @c:= (SELECT ");
                        sqry.Append("COUNT(cinema_seat_id) quantity ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, ");
                        sqry.Append("ticket b, movies_schedule_list c, movies_schedule d, ");
                        sqry.Append("movies e, cinema f, ticket g, users h ");
                        sqry.Append("WHERE a.ticket_id = b.id AND a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = c.id AND ");
                        sqry.Append("c.movies_schedule_id = d.id AND d.movie_id ");
                        sqry.Append("= e.id AND d.cinema_id = f.id and g.id=a.ticket_id and "); 
                        sqry.Append("h.id= g.user_id and h.userid=@user AND  ");
                        sqry.Append(string.Format("d.movie_date ='{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(" ORDER BY f.in_order ) ");
                        sqry.Append("as QTY, (@c*b.price) as `TOTALSALES`,");
                        sqry.Append("d.movie_date, h.system_value, j.fname, j.mname, ");
                        sqry.Append("j.lname, k.start_date  from patrons a inner join");
                        sqry.Append(" cinema_patron b on a.id=b.patron_id inner join ");
                        sqry.Append("cinema c on b.cinema_id = c.id inner join ");
                        sqry.Append("movies_schedule d on d.cinema_id = c.id inner ");
                        sqry.Append("join movies_schedule_list e on  e.movies_schedule_id = ");
                        sqry.Append("d.id inner join movies_schedule_list_reserved_seat f on ");
                        sqry.Append(" f.movies_schedule_list_id= e.id inner join ticket g on ");
                        sqry.Append(" e.id= g.movies_schedule_list_id inner join users j  on ");
                        sqry.Append(" j.id = g.user_id  inner join config_table h inner join ");
                        sqry.Append("movies k on d.movie_id = k.id where d.movie_date=");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' and ",_dtStart));
                        sqry.Append("h.system_code='001' and f.status=1 group by j.userid ");*/


                        sqry.Append("select e.userid as usercode, concat_ws(' ',e.fname, e.mname, e.lname) username, ");
                        sqry.Append("count(c.ticket_id) as qty, sum(c.base_price) as totalsales, f.start_time as movie_date, ");
                        sqry.Append("g.system_value, h.name report_name ");
                        sqry.Append("from movies_schedule_list_reserved_seat c, ticket d, users e, movies_schedule_list f, ");
                        sqry.Append("config_table g, report h ");
                        sqry.Append("where  c.movies_schedule_list_id in (select a.id from movies_schedule_list a, movies_schedule b ");
                        sqry.Append(String.Format("where a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", _dtStart.AddDays(1)));
                        sqry.Append("and a.movies_schedule_id = b.id) ");
                        sqry.Append("and d.id = c.ticket_id ");
                        sqry.Append("and c.status = 1 ");
                        sqry.Append("and d.user_id = e.id ");
                        sqry.Append("and c.movies_schedule_list_id = f.id ");
                        sqry.Append("and g.system_code = '001' ");
                        sqry.Append("and h.id = 6 ");
                        sqry.Append("group by d.user_id");
                        break;

                    case "RP05":
                        //melvin 11/10/2014
                        //RMB 12.11.2014
                        /*sqry.Append("select a.code as movie_code, a.title, ");
                        sqry.Append("count(e.cinema_seat_id) as QTY, ");
                        sqry.Append("(e.price* count(e.cinema_seat_id)) as Sales, ");
                        sqry.Append("b.movie_date, c.code as dist_code, f.system_value from movies a ");
                        sqry.Append("inner join movies_schedule b on a.id= b.movie_id ");
                        sqry.Append("inner join distributor c on a.dist_id= c.id ");
                        sqry.Append("inner join movies_schedule_list d on b.id= ");
                        sqry.Append("d.movies_schedule_id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat e ");
                        sqry.Append("on d.id = e.movies_schedule_list_id  inner join config_table f");
                        sqry.Append(" where b.movie_date = ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append(" and c.code='"+rp05distributor+"' and  e.status=1 and f.system_code='001'");
                        sqry.Append("GROUP BY a.code;");*/

                        sqry.Append("select d.code as movie_code, d.title, count(a.cinema_seat_id) as qty, ");
                        sqry.Append("sum(a.base_price) as Sales, c.movie_date, e.name dist_name, ");
                        sqry.Append("e.code as dist_code, g.system_value, h.name report_name ");
                        sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                        sqry.Append("movies_schedule c, movies d, distributor e, config_table g, report h ");
                        sqry.Append("where a.movies_schedule_list_id = b.id ");
                        sqry.Append("and b.movies_schedule_id = c.id ");
                        sqry.Append("and c.movie_id = d.id ");
                        sqry.Append("and d.dist_id = e.id ");
                        sqry.Append(String.Format("and c.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and e.code = '{0}' ", _strDistributor));
                        sqry.Append("and a.status = 1 ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and g.system_code = '001' ");
                        sqry.Append("and h.id = 5 ");
                        sqry.Append("group by e.code, d.code ");
                        sqry.Append("order by d.title asc");
                        break;

                    case "RP06":
                        //RMB 11.20.2014 remarked start
                        /*sqry.Append("SELECT c.start_time, e.title FROM ");
                        sqry.Append("movies_schedule_list_reserved_seat a, ticket b, ");
                        sqry.Append("movies_schedule_list c, movies_schedule d, movies e, ");
                        sqry.Append("cinema f WHERE a.ticket_id = b.id and a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id ");
                        sqry.Append(string.Format("AND d.movie_date = '{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append("AND d.movie_id = e.id AND d.cinema_id = f.id and ");
                        sqry.Append(string.Format("f.name= '{0}' GROUP BY",strCinema));
                        sqry.Append(" c.start_time, f.id, e.id ORDER BY f.in_order;");
                        MySqlConnection myconn = new MySqlConnection();
                        myconn.ConnectionString = m_frmM._connection;

                        if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                        queryList.Add(reader["title"]+"\\n"+reader["start_time"]);
                        }
                        string query = "CREATE TABLE tmp_storage ( ";
                        for (int x = 0; x < queryList.Count; x++)
                        {

                            query += "`"+queryList[x].ToString() + "` text";
                            if (x < queryList.Count - 1)
                            {
                                query += ",";
                            }
                        }
                        query += " )";
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();*/
                        //RMB 11.20.2014 remarked end

                        //for the new report// prepare the query to fill the tmp_dailysummary table
                        m_clscom.refreshTable(m_frmM, "tmp_dailysummary", m_frmM._connection);
                        StringBuilder strqry1 = new StringBuilder();
                        strqry1.Append("set @list_id = 0; ");
                        strqry1.Append("set @rank = 0; ");
                        strqry1.Append(String.Format("insert into {0} (", "tmp_dailysummary"));
                        strqry1.Append("select t1.*, ");
                        strqry1.Append("@rank := if(@list_id = t1.movies_schedule_list_id, @rank ,@rank + 1) as rank, ");
                        strqry1.Append("@list_id := t1.movies_schedule_list_id as list_id ");
                        strqry1.Append("from (");
                        strqry1.Append("select 0, j.name cinema_name, h.title, f.start_time, f.end_time, e.name patron_name, ");
                        strqry1.Append("c.base_price price, count(c.ticket_id), sum(c.base_price), min(c.or_number), max(c.or_number), ");
                        strqry1.Append(String.Format("'{0}', c.movies_schedule_list_id ", m_frmM.m_usercode));
                        strqry1.Append("from movies_schedule_list_reserved_seat c, movies_schedule_list_patron d, ");
                        strqry1.Append("patrons e, movies_schedule_list f, movies_schedule g, movies h, cinema j ");
                        strqry1.Append("where c.movies_schedule_list_id in (select a.id from movies_schedule_list a, movies_schedule b ");
                        strqry1.Append("where a.movies_schedule_id = b.id ");
                        strqry1.Append(String.Format("and a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                        strqry1.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", _dtStart.AddDays(1)));
                        strqry1.Append(String.Format("and b.cinema_id = {0} order by a.start_time asc) ", _intCinemaID));
                        strqry1.Append("and c.patron_id = d.id ");
                        strqry1.Append("and d.patron_id = e.id ");
                        strqry1.Append("and c.movies_schedule_list_id = f.id ");
                        strqry1.Append("and f.movies_schedule_id = g.id ");
                        strqry1.Append("and g.movie_id = h.id ");
                        strqry1.Append("and g.cinema_id = j.id ");
                        strqry1.Append("and c.status = 1 ");
                        strqry1.Append("group by c.movies_schedule_list_id, c.patron_id order by f.start_time asc)as t1 order by t1.start_time asc);");
                        m_clscom.populateTableDaily(m_frmM, "tmp_dailysummary", m_frmM._connection, strqry1.ToString());
                        ////set the query for the report
                        sqry.Append("select a.patron_nm, a.price, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 1) qty1, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 1) sales1, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 1) movie1, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 1) time1, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 2) qty2, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 2) sales2, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 2) movie2, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 2) time2, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 3) qty3, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 3) sales3, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 3) movie3, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 3) time3, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 4) qty4, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 4) sales4, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 4) movie4, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 4) time4, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 5) qty5, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 5) sales5, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 5) movie5, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 5) time5, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 6) qty6, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 6) sales6, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 6) movie6, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 6) time6, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 7) qty7, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 7) sales7, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 7) movie7, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 7) time7, ");
                        sqry.Append(String.Format("(select unit from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 8) qty8, ");
                        sqry.Append(String.Format("(select total_price from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and patron_nm = a.patron_nm and screening_id = 8) sales8, ");
                        sqry.Append(String.Format("(select distinct(title) from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 8) movie8, ");
                        sqry.Append("(select distinct(concat_ws(' - ',DATE_FORMAT(start_time,'%l:%i %p'),DATE_FORMAT(end_time,'%l:%i %p'))) ");
                        sqry.Append(String.Format("from tmp_dailysummary where userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("and screening_id = 8) time8, ");
                        sqry.Append(String.Format("(select min(min_or) from tmp_dailysummary where userid = '{0}') min_or, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select max(max_or) from tmp_dailysummary where userid = '{0}') max_or, ", m_frmM.m_usercode));
                        sqry.Append("(select system_value from config_table where system_code = '001') system_value, ");
                        sqry.Append("(select h.name from report h where h.id = '6') report_name, ");
                        sqry.Append(String.Format("(select distinct(start_time) from tmp_dailysummary where userid = '{0}' and start_time is not null limit 1) movie_date, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select sum(unit) from tmp_dailysummary where userid = '{0}') seat_taken, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select capacity * (select max(screening_id) from tmp_dailysummary where userid = '{0}') from cinema where name = ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select distinct(cinema_name) from tmp_dailysummary where userid = '{0}')) seat_capacity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select distinct(cinema_name) from tmp_dailysummary where userid = '{0}') cinema_nm ", m_frmM.m_usercode));
                        sqry.Append("from tmp_dailysummary a ");
                        sqry.Append(String.Format("where a.userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("group by a.patron_nm, a.price");
                        break;

                    case "RP12":
                        // RMB 11.28.2014 remarked
                        /*//melvin 
                        sqry.Append("select @user:= i.userid as userid, a.code, ");
                        sqry.Append("@patron:= a.name as PATRON, @cinema:=c.name");
                        sqry.Append(" CINEMA, b.price PRICE, @c:= (select ");
                        sqry.Append("count(a.cinema_seat_id)from movies_schedule_list_reserved_seat");
                        sqry.Append(" a inner join movies_schedule_list b on ");
                        sqry.Append("a.movies_schedule_list_id= b.id inner join ");
                        sqry.Append("movies_schedule c on b.movies_schedule_id=c.id ");
                        sqry.Append("inner join cinema d on d.id= c.cinema_id inner ");
                        sqry.Append("join movies_schedule_list_patron e on e.id = ");
                        sqry.Append("a.patron_id inner join patrons f on f.id = ");
                        sqry.Append("e.patron_id inner join ticket g on g.id= a.ticket_id ");
                        sqry.Append("inner join users h on g.user_id= h.id where ");
                        sqry.Append("a.status = 1 and f.name=@patron and d.name=@cinema ");
                        sqry.Append("and h.userid=@user AND c.movie_date = ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}')",_dtStart));
                        sqry.Append("as QTY, (@c*b.price) as `TOTALSALES`, ");
                        sqry.Append("d.movie_date , h.start_date, h.title, i.fname,");
                        sqry.Append("i.lname, i.mname, j.system_value from patrons a ");
                        sqry.Append("inner join cinema_patron b on a.id=b.patron_id ");
                        sqry.Append("inner join cinema c on b.cinema_id = c.id inner join");
                        sqry.Append(" movies_schedule d on d.cinema_id = c.id inner join ");
                        sqry.Append("movies_schedule_list e on e.movies_schedule_id = d.id");
                        sqry.Append(" inner join movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id inner join ticket g ");
                        sqry.Append("on e.id= g.movies_schedule_list_id inner join movies");
                        sqry.Append(" h on h.id= d.movie_id inner join users i on i.id = ");
                        sqry.Append("g.user_id inner join config_table j where ");
                        sqry.Append(string.Format("d.movie_date='{0:yyyy/MM/dd}'",_dtStart));
                        sqry.Append(" and j.system_code='001' and f.status=1 ");
                        sqry.Append("group by g.user_id, a.name,c.name order by c.name; ");*/

                        sqry.Append("select * from ");
                        sqry.Append("(select d.id user, f.code, f.name PATRON, i.name CINEMA, b.base_price PRICE, ");
                        sqry.Append("count(b.cinema_seat_id) QTY, sum(b.base_price) TOTALSALES, h.movie_date, ");
                        sqry.Append("d.userid, concat_ws(' ', d.fname, d.mname, d.lname) usernm, j.system_value, k.name reportname ");
                        sqry.Append("from movies_schedule_list_reserved_seat b, ticket c, ");
                        sqry.Append("users d, movies_schedule_list_patron e, patrons f, movies_schedule_list g, ");
                        sqry.Append("movies_schedule h, cinema i, config_table j, report k ");
                        sqry.Append("where b.movies_schedule_list_id in ");
                        sqry.Append("(select a.id from movies_schedule_list a ");
                        sqry.Append(String.Format("where a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", _dtStart.AddDays(1)));
                        sqry.Append("and a.status = 1) ");
                        sqry.Append("and b.ticket_id = c.id ");
                        sqry.Append("and b.movies_schedule_list_id = g.id ");
                        sqry.Append("and c.user_id = d.id ");
                        sqry.Append("and d.system_code = 2 ");
                        sqry.Append("and d.status = 1 ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and b.patron_id = e.id ");
                        sqry.Append("and e.patron_id = f.id ");
                        sqry.Append("and g.movies_schedule_id = h.id ");
                        sqry.Append("and h.cinema_id = i.id ");
                        sqry.Append("and j.system_code = '001' ");
                        sqry.Append("and k.id = 12 ");
                        sqry.Append("group by c.user_id, e.patron_id, h.cinema_id ");
                        sqry.Append("order by c.user_id, f.code, h.cinema_id asc) tbl ");
                        sqry.Append("order by userid, code asc ");
                        break;

                    case "RP13":
                        //RMB 11.28.2014 remarked
                    /*// melvin 11/11/2014
                        sqry.Append("select @user_id:= g.user_id, a.code, @patron:= ");
                        sqry.Append("a.name as PATRON, @cinema:= c.name CINEMA, b.price ");
                        sqry.Append("PRICE, @c:= ((select count(a.cinema_seat_id) ");
                        sqry.Append("from ticket z inner join  movies_schedule_list_reserved_seat ");
                        sqry.Append("a on z.id=a.ticket_id inner join movies_schedule_list b on ");
                        sqry.Append("a.movies_schedule_list_id= b.id inner join movies_schedule c ");
                        sqry.Append("on b.movies_schedule_id=c.id inner join cinema d on d.id= ");
                        sqry.Append("c.cinema_id inner join movies_schedule_list_patron e on e.id ");
                        sqry.Append("= a.patron_id inner join patrons f on f.id = e.patron_id ");
                        sqry.Append("where  a.status = 1 and f.name=@patron and d.name=@cinema ");
                        sqry.Append("and z.user_id=@user_id AND c.movie_date = ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}'", _dtStart));
                        sqry.Append(")) as QTY, (@c*b.price) as `TOTALSALES`, ");
                        sqry.Append("d.movie_date , h.start_date,h.title, i.userid, ");
                        sqry.Append("i.fname, i.lname, i.mname, j.system_value ");
                        sqry.Append("from patrons a inner join cinema_patron b on ");
                        sqry.Append("a.id=b.patron_id inner join cinema c on b.cinema_id ");
                        sqry.Append("= c.id inner join movies_schedule d on d.cinema_id = ");
                        sqry.Append("c.id inner join movies_schedule_list e on e.movies_schedule_id = d.id ");
                        sqry.Append("inner join movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id inner join  ticket ");
                        sqry.Append("g on e.id= g.movies_schedule_list_id inner join movies h ");
                        sqry.Append("on h.id= d.movie_id inner join users i on i.id= ");
                        sqry.Append("g.user_id inner join config_table j where ");
                        sqry.Append("j.system_code='001' and f.status=1 and d.movie_date=");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append("group by  g.user_id, a.name, c.name order by c.name;");*/
                        sqry.Append("select * from ");
                        sqry.Append("(select f.code patroncode, f.name patronname,i.id cinema_id, i.name cinemaname, b.base_price price, ");
                        sqry.Append("count(b.cinema_seat_id) qty, sum(b.base_price) totalsales, h.movie_date, ");
                        sqry.Append("d.userid, concat_ws(' ', d.fname, d.mname, d.lname) usernm, ");
                        sqry.Append("j.system_value, k.name reportname, l.title ");
                        sqry.Append("from movies_schedule_list_reserved_seat b, ticket c, ");
                        sqry.Append("users d, movies_schedule_list_patron e, patrons f, movies_schedule_list g, ");
                        sqry.Append("movies_schedule h, cinema i, config_table j, report k, movies l ");
                        sqry.Append("where b.movies_schedule_list_id in ");
                        sqry.Append("(select a.id from movies_schedule_list a ");
                        sqry.Append(String.Format("where a.start_time >= '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", _dtStart.AddDays(1)));
                        sqry.Append("and a.status = 1) ");
                        sqry.Append("and b.ticket_id = c.id ");
                        sqry.Append("and b.movies_schedule_list_id = g.id ");
                        sqry.Append("and c.user_id = d.id ");
                        sqry.Append("and d.system_code = 2 ");
                        sqry.Append("and d.status = 1 ");
                        sqry.Append("and b.status = 1 ");
                        sqry.Append("and b.patron_id = e.id ");
                        sqry.Append("and e.patron_id = f.id ");
                        sqry.Append("and g.movies_schedule_id = h.id ");
                        sqry.Append("and h.cinema_id = i.id ");
                        sqry.Append("and h.movie_id = l.id ");
                        sqry.Append("and j.system_code = '001' ");
                        sqry.Append("and k.id = 13 ");
                        sqry.Append("group by h.cinema_id, h.movie_id, c.user_id, e.patron_id ");
                        sqry.Append("order by h.cinema_id, c.user_id, f.code asc) tbl ");
                        sqry.Append("order by cinemaname, usernm, patroncode asc ");
                        break;
                    case "RP15":
                        DateTime endDate = _dtEnd.AddDays(-1);
                        sqry.Append(string.Format("select h.system_value, '{0:yyyy/MM/dd}'",_dtStart));
                        sqry.Append(string.Format( "as dtFrom , '{0:yyyy/MM/dd}' as dtTo, ",endDate));
                        sqry.Append("f.name `cinema`, e.code, e.name `patron`, a.base_price price, ");
                        sqry.Append("c.movie_date, count(a.cinema_seat_id) as `qty`, d.title ");
                        sqry.Append("from movies_schedule_list_reserved_seat a, ");
                        sqry.Append("movies_schedule_list b, movies_schedule c, movies d,");
                        sqry.Append("patrons e, cinema f, movies_schedule_list_patron g,");
                        sqry.Append("config_table h where a.movies_schedule_list_id= b.id ");
                        sqry.Append("and b.movies_schedule_id = c.id and c.movie_id = d.id ");
                        sqry.Append("and e.id= g.patron_id and g.id = a.patron_id ");
                        sqry.Append("and f.id=c.cinema_id and a.status=2 and ");
                        sqry.Append("h.system_code='001' and c.movie_date between ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'",_dtStart,_dtEnd));
                        sqry.Append(" group by d.title, f.name;");
                        break;
                    case "RP17":
                        //melvin 11/13/2014 
                        //RMB remarked 12.02.2014
                        /*string endDt = _dtEnd.AddDays(-1).ToShortDateString();
                        sqry.Append("SELECT '"+_dtStart.ToShortDateString()+"' as dt1,");
                        sqry.Append("'"+endDt+"' as dt2, z.system_value, k.id, k.name,");
                        sqry.Append("sum(total_available_seats) total_available_seats,");
                        sqry.Append("sum(total_seats_taken) total_seats_taken, ");
                        sqry.Append("sum(total_ticket_sales) total_ticket_sales, ");
                        sqry.Append("sum((total_seats_taken/total_available_seats) * 100) util ");
                        sqry.Append("FROM (SELECT a.id, code, title, rating_id, ");
                        sqry.Append("COUNT(movie_date) no_of_days FROM movies a,");
                        sqry.Append("movies_schedule b WHERE a.id = b.movie_id  ");
                        sqry.Append(string.Format("AND b.movie_date BETWEEN '{0:yyyy/MM/dd}'",_dtStart));
                        sqry.Append(string.Format("AND '{0:yyyy/MM/dd}' GROUP BY a.id ) e, ",_dtEnd));
                        sqry.Append("(SELECT movie_id, COUNT(c.id) no_of_screenings FROM  ");
                        sqry.Append("movies_schedule_list c, movies_schedule d WHERE ");
                        sqry.Append("c.movies_schedule_id =d.id AND  d.movie_date ");
                        sqry.Append(string.Format("BETWEEN '{0:yyyy/MM/dd}' AND ",_dtStart));
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' GROUP BY movie_id ) f, ",_dtEnd));
                        sqry.Append("(SELECT j.movie_id, COUNT(h.cinema_seat_id) ");
                        sqry.Append("total_seats_taken, SUM(h.price) total_ticket_sales ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat h, movies_schedule_list ");
                        sqry.Append("i,movies_schedule j WHERE h.movies_schedule_list_id = ");
                        sqry.Append("i.id AND  i.movies_schedule_id = j.id AND ");
                        sqry.Append(string.Format("j.movie_date BETWEEN  '{0:yyyy/MM/dd}'",_dtStart));
                        sqry.Append(string.Format(" AND '{0:yyyy/MM/dd}' and ",_dtEnd));
                        sqry.Append("h.status=1 GROUP BY movie_id ) g,(SELECT movie_id, ");
                        sqry.Append("name, id, SUM(available_seats) total_available_seats ");
                        sqry.Append("FROM (SELECT movie_id, n.capacity * COUNT(l.id) ");
                        sqry.Append("available_seats , n.name, n.id FROM movies_schedule_list ");
                        sqry.Append("l, movies_schedule m, cinema n WHERE l.movies_schedule_id=m.id ");
                        sqry.Append("AND m.cinema_id = n.id AND m.movie_date BETWEEN  ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' AND '{1:yyyy/MM/dd}'",_dtStart,_dtEnd));
                        sqry.Append(" GROUP BY movie_id, cinema_id) o GROUP BY  movie_id) k ");
                        sqry.Append(", config_table z WHERE e.id = f.movie_id AND e.id = ");
                        sqry.Append("g.movie_id AND e.id = k.movie_id and z.system_code='001' ");
                        sqry.Append(" group by k.name; ");*/
                        sqry.Append("select cinemacode, cinemaname, sum(seattaken), sum(seattotal), sum(totalprice), ((sum(seattaken)/sum(seattotal)) * 100) util, ");
                        sqry.Append("system_value, reportname from (select f.id cinemacode, f.name cinemaname, ");
                        sqry.Append("count(b.cinema_seat_id) seattaken, f.capacity * count(distinct(c.id)) seattotal, ");
                        sqry.Append("sum(b.base_price) totalprice, j.system_value, k.name reportname ");
                        sqry.Append("from movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, movies e, ");
                        sqry.Append("cinema f, config_table j, report k ");
                        sqry.Append("where b.movies_schedule_list_id in ");
                        sqry.Append(String.Format("(select a.id from movies_schedule_list a where a.start_time > '{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' and a.status = 1)  ", _dtEnd.AddDays(1)));
                        sqry.Append("and b.movies_schedule_list_id = c.id and c.movies_schedule_id = d.id ");
                        sqry.Append("and d.movie_id = e.id and d.cinema_id = f.id and b.status = 1 ");
                        sqry.Append("and j.system_code = '001' and k.id = 17 ");
                        sqry.Append("group by d.cinema_id,e.code ");
                        sqry.Append("order by d.cinema_id,e.title asc) tbl1 ");
                        sqry.Append("group by cinemacode ");
                        sqry.Append("order by cinemacode");

                        break;
                    case "AUDIT":
                        sqry.Append("SELECT a.id, a.tr_date, b.userid, d.system_value, ");
                        sqry.Append("a.tr_details, c.module_desc, c.module_group,");
                        sqry.Append("a.computer_name, concat(b.fname , ' ',b.lname)");
                        sqry.Append("username FROM a_trail a left join users b ");
                        sqry.Append("on a.user_id = b.id left join system_module c ");
                        sqry.Append("on trim(a.module_code) = trim(c.id) inner ");
                        sqry.Append("join config_table d where tr_date BETWEEN ");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtStart) + " AND ");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}' ", _dtEnd));
                        sqry.Append(" and d.system_code='001'");
                        if(!(module==""))
                        {
                            sqry.Append(string.Format("AND c.module_group like '%{0}%' ", module));
                        }
                        if(!(account==""))
                        {
                            sqry.Append(string.Format("AND b.userid like '%{0}%'", account));
                        }

                        sqry.Append(" order by tr_date desc");
                        //MessageBox.Show(_dtStart.ToShortDateString() + "to:" + _dtEnd.ToShortDateString());
                        break;

                        //RMB 11-10-2014 added new report start
                    case "RP08":
                        //RMB 11.21.2014 working for specific screening time with the maximum seats taken
                        /*sqry.Append("SELECT c.name PATRON, IFNULL(COUNT(cinema_seat_id), 0) QTY, a.price PRICE, IFNULL(SUM(a.price), 0) SALES, ");
                        sqry.Append("d.start_time START_TIME, d.end_time END_TIME, f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME, h.TITLE ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, ");
                        sqry.Append("movies_schedule_list d, cinema e, config_table f, report g, movies h ");
                        sqry.Append("where a.movies_schedule_list_id = (select id from (select id, max(totalticket) from ( ");
                        sqry.Append("select a.id, count(c.cinema_seat_id) totalticket, sum(c.price) totalprice, a.start_time, a.end_time from movies_schedule_list a, movies_schedule b, movies_schedule_list_reserved_seat c ");
                        sqry.Append("where a.movies_schedule_id = b.id and a.status = 1 ");
                        sqry.Append(String.Format("and b.cinema_id = {0} ", _intCinemaID));
                        sqry.Append(String.Format("and b.movie_id = {0} ", _intMovieID));
                        sqry.Append(String.Format("and movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append("and a.id = c.movies_schedule_list_id ");
                        sqry.Append("group by c.movies_schedule_list_id ) table1) table2) ");
                        sqry.Append("and a.patron_id = b.id ");
                        sqry.Append("and b.patron_id = c.id ");
                        sqry.Append("and a.movies_schedule_list_id = d.id ");
                        sqry.Append("and f.system_code = '001' ");
                        sqry.Append("and g.id = 8 ");
                        sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                        sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                        sqry.Append("group by a.patron_id");*/
                        /*ver2 edited 2.5.2016*//*sqry.Append("SELECT c.name PATRON, IFNULL(COUNT(cinema_seat_id), 0) QTY, a.base_price PRICE, IFNULL(SUM(a.base_price), 0) SALES, ");
                        sqry.Append("min(d.start_time) START_TIME, max(d.end_time) END_TIME, f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME, h.TITLE ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, ");
                        sqry.Append("movies_schedule_list d, cinema e, config_table f, report g, movies h ");
                        sqry.Append("where a.movies_schedule_list_id in (select a.id from movies_schedule_list a, movies_schedule b, movies_schedule_list_reserved_seat c ");
                        sqry.Append("where a.movies_schedule_id = b.id and a.status = 1 ");
                        sqry.Append(String.Format("and b.cinema_id = {0} ", _intCinemaID));
                        sqry.Append(String.Format("and b.movie_id = {0} ", _intMovieID));
                        sqry.Append(String.Format("and movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append("and a.id = c.movies_schedule_list_id ");
                        sqry.Append("group by c.movies_schedule_list_id) ");
                        sqry.Append("and a.patron_id = b.id ");
                        sqry.Append("and b.patron_id = c.id ");
                        sqry.Append("and a.movies_schedule_list_id = d.id ");
                        sqry.Append("and a.status = 1 ");
                        sqry.Append("and f.system_code = '001' ");
                        sqry.Append("and g.id = 8 ");
                        sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                        sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                        sqry.Append("group by c.name");*/
                        
                        if (_intWithUtil == 1)//added 5-31-2022
                        {
                            sqry.Append("SELECT table1.*, table2.SEATTAKEN, table2.SEATTOTAL, table2.UTIL FROM ");
                            sqry.Append("(SELECT c.name PATRON, IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE, ");//changed price to base_price 2.26.2016
                            sqry.Append("IFNULL(SUM(a.base_price), 0) SALES, min(d.start_time) START_TIME, max(d.end_time) END_TIME, ");//changed price to base_price 2.26.2016
                            sqry.Append("f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME, h.TITLE, e.id cinemaid, h.id movieid ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, ");
                            sqry.Append("movies_schedule_list d, cinema e, config_table f, report g, movies h, movies_schedule i ");
                            sqry.Append("where d.movies_schedule_id = i.id ");
                            sqry.Append("and d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 8 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID)); 
                            sqry.Append("group by c.name) table1 ");
                            sqry.Append("LEFT JOIN ");
                            sqry.Append("(SELECT tbl1.cinemacode, tbl1.movieid, SUM(tbl1.seattaken) SEATTAKEN, SUM(tbl1.seattotal) SEATTOTAL, ((SUM(tbl1.seattaken) / SUM(tbl1.seattotal)) *100) UTIL ");
                            sqry.Append("FROM(SELECT f.id cinemacode, e.id movieid, COUNT(b.cinema_seat_id) seattaken, f.capacity * COUNT(DISTINCT(c.id)) seattotal ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, movies e, cinema f ");
                            sqry.Append("WHERE b.movies_schedule_list_id IN ");
                            sqry.Append(String.Format("(SELECT a.id FROM movies_schedule_list a WHERE a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append(String.Format("AND a.start_time < '{0:yyyy/MM/dd}' AND a.status = 1) ", _dtStart.AddDays(1)));
                            sqry.Append("AND b.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id ");
                            sqry.Append("AND d.movie_id = e.id AND d.cinema_id = f.id AND b.status = 1 ");
                            sqry.Append("GROUP BY d.cinema_id, e.id) tbl1 ");
                            sqry.Append("GROUP BY tbl1.cinemacode, tbl1.movieid ");
                            sqry.Append(String.Format("HAVING tbl1.cinemacode = {0} ", _intCinemaID));
                            sqry.Append(String.Format("AND tbl1.movieid = {0}) table2 ", _intMovieID));
                            sqry.Append("ON table1.cinemaid = table2.cinemacode AND table1.movieid = table2.movieid");

                            /*sqry.Append("SELECT table1.*, table2.SEATTAKEN, table2.SEATTOTAL, table2.UTIL FROM ");
                            sqry.Append("(SELECT c.name PATRON, IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE, ");//changed price to base_price 2.26.2016
                            sqry.Append("IFNULL(SUM(a.base_price), 0) SALES, min(d.start_time) START_TIME, max(d.end_time) END_TIME, ");//changed price to base_price 2.26.2016
                            sqry.Append("f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME, h.TITLE, e.id cinemaid, h.id movieid ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, ");
                            sqry.Append("movies_schedule_list d, cinema e, config_table f, report g, movies h, movies_schedule i, ticket j ");
                            sqry.Append("where d.movies_schedule_id = i.id ");
                            sqry.Append("and d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("and a.ticket_id = j.id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 8 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                            sqry.Append("and date(j.ticket_datetime) = date(d.start_time) ");
                            sqry.Append("group by c.name) table1 ");
                            sqry.Append("LEFT JOIN ");
                            sqry.Append("(SELECT tbl1.cinemacode, tbl1.movieid, SUM(tbl1.seattaken) SEATTAKEN, SUM(tbl1.seattotal) SEATTOTAL, ((SUM(tbl1.seattaken) / SUM(tbl1.seattotal)) *100) UTIL ");
                            sqry.Append("FROM(SELECT f.id cinemacode, e.id movieid, COUNT(b.cinema_seat_id) seattaken, f.capacity * COUNT(DISTINCT(c.id)) seattotal ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, movies e, cinema f ");
                            //sqry.Append("FROM movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, movies e, cinema f, ticket j ");
                            sqry.Append(String.Format("(SELECT a.id FROM movies_schedule_list a WHERE a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append(String.Format("AND a.start_time < '{0:yyyy/MM/dd}' AND a.status = 1) ", _dtStart.AddDays(1)));
                            //sqry.Append("AND b.ticket_id = j.id ");
                            sqry.Append("AND b.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id ");
                            sqry.Append("AND d.movie_id = e.id AND d.cinema_id = f.id AND b.status = 1 ");
                            //sqry.Append("AND DATE(j.ticket_datetime) = DATE(c.start_time) ");
                            sqry.Append("GROUP BY d.cinema_id, e.id) tbl1 ");
                            sqry.Append("GROUP BY tbl1.cinemacode, tbl1.movieid ");
                            sqry.Append(String.Format("HAVING tbl1.cinemacode = {0} ", _intCinemaID));
                            sqry.Append(String.Format("AND tbl1.movieid = {0}) table2 ", _intMovieID));
                            sqry.Append("ON table1.cinemaid = table2.cinemacode AND table1.movieid = table2.movieid");*/
                        }
                        else if (_intWithUtil == 0)
                        {
                            sqry.Append("SELECT c.name PATRON, IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE, ");//changed price to base_price 2.26.2016
                            sqry.Append("IFNULL(SUM(a.base_price), 0) SALES, min(d.start_time) START_TIME, max(d.end_time) END_TIME, ");//changed price to base_price 2.26.2016
                            sqry.Append("f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME, h.TITLE ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, ");
                            sqry.Append("movies_schedule_list  d, cinema e, config_table f, report g, movies h, movies_schedule i ");
                            sqry.Append("where d.movies_schedule_id = i.id ");
                            sqry.Append("and d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 8 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                            sqry.Append("group by c.name");

                            /*sqry.Append("SELECT c.name PATRON,IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE,");
                            sqry.Append("IFNULL(SUM(a.base_price), 0) SALES, MIN(d.start_time) START_TIME, MAX(d.end_time) END_TIME,");
                            sqry.Append("f.system_value SYSTEM_VAL,g.name REPORT_NAME, e.name CINEMA_NAME,h.TITLE ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c,");
                            sqry.Append("movies_schedule_list d,cinema e,config_table f, report g, movies h, movies_schedule i, ticket j ");
                            sqry.Append("where d.movies_schedule_id = i.id ");
                            sqry.Append("and d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("and a.ticket_id = j.id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 8 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                            sqry.Append("and date(j.ticket_datetime) = date(d.start_time) ");
                            sqry.Append("group by c.name");*/
                        }
                        break;
                    case "RP09":
                        sqry.Append("select e.id cinema_id, e.name cinema_name,  d.code patron_code, d.name patron_name, a.base_price price, ");
                        sqry.Append("COUNT(a.cinema_seat_id) qty, SUM(a.base_price) sales, g.system_value report_header, h.name report_title ");
                        sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                        sqry.Append("movies_schedule c, patrons d, cinema e, movies_schedule_list_patron f, ");
                        sqry.Append("config_table g, report h ");
                        sqry.Append("where a.movies_schedule_list_id in (select distinct(bb.id) id from movies_schedule_list bb ");
                        //RMB remarked 12.11.2014                    
                        //if(_dtStart == _dtEnd)
                        //    sqry.Append(String.Format("where bb.start_time >= '{0:yyyy/MM/dd}' and bb.start_time <= '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtEnd.AddDays(1)));
                        //else
                        //  sqry.Append(String.Format("where bb.start_time between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtEnd));
                        sqry.Append(String.Format("where bb.start_time >= '{0:yyyy/MM/dd}' and bb.start_time <= '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtEnd.AddDays(1)));
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append("AND b.movies_schedule_id = c.id ");
                        sqry.Append("AND a.patron_id = f.id ");
                        sqry.Append("AND f.patron_id = d.id ");
                        sqry.Append("AND c.cinema_id = e.id ");
                        sqry.Append("AND a.status = 1 ");
                        sqry.Append("and g.system_code = '001' ");
                        sqry.Append("and h.id = 9 ");
                        sqry.Append("GROUP BY c.cinema_id, f.patron_id, a.base_price ");
                        sqry.Append("ORDER BY e.in_order, d.name, a.base_price");
                        break;

                        //RMB 11-10-2014 added new report end
                        //RMB 11.11.2014 added new report start
                    case "RP10":
                        sqry.Append($"SELECT\r\nGROUP_CONCAT(s.code) `SurchargeCodes`,\r\nGROUP_CONCAT(CAST(s.amount_val AS CHAR)) `SurchargeValues`,\r\n\r\nGROUP_CONCAT(CONCAT_WS(': ', s.code, CAST(s.amount_val AS CHAR)) SEPARATOR '\\n') `SurchargeText`, ii.*\r\nFROM (");
                        sqry.Append("select e.id cinema_id,e.name cinema_name, i.title, d.code patron_code, d.name patron_name, a.base_price price, ");
                        sqry.Append("COUNT(a.cinema_seat_id) qty, SUM(a.base_price) sales, g.system_value report_header, h.name report_title ");
                        sqry.Append(", e.in_order, a.id `PrimaryKey`, f.patron_id `PatronId`");
                        sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                        sqry.Append("movies_schedule c, patrons d, cinema e, movies_schedule_list_patron f, ");
                        sqry.Append("config_table g, report h, movies i ");
                        sqry.Append("where a.movies_schedule_list_id in (select distinct(bb.id) id from movies_schedule_list bb ");
                        //RMB 12.11.2014 remarked
                        //if (_dtStart == _dtEnd)
                        //    sqry.Append(String.Format("where bb.start_time >= '{0:yyyy/MM/dd}' and bb.start_time <= '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtStart.AddDays(1)));
                        //else
                        //    sqry.Append(String.Format("where bb.start_time between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtEnd));
                        sqry.Append(String.Format("where bb.start_time >= '{0:yyyy/MM/dd}' and bb.start_time <= '{1:yyyy/MM/dd}' and bb.status = 1) ", _dtStart, _dtEnd.AddDays(1)));
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append("AND b.movies_schedule_id = c.id ");
                        sqry.Append("AND a.patron_id = f.id ");
                        sqry.Append("AND f.patron_id = d.id ");
                        sqry.Append("AND c.cinema_id = e.id ");
                        sqry.Append("AND c.movie_id = i.id ");
                        sqry.Append("AND a.status = 1 ");
                        sqry.Append("AND g.system_code = '001' ");
                        sqry.Append("AND h.id = 10 ");
                        sqry.Append("GROUP BY c.cinema_id, i.id, f.patron_id, a.base_price ");
                        sqry.Append("ORDER BY e.in_order, i.title, d.name, a.base_price");
                        sqry.Append($") ii\r\n\r\nLEFT JOIN patrons_surcharge ps ON ps.patron_id=ii.`PatronId`\r\nLEFT JOIN surcharge_tbl s ON s.id=ps.surcharge_id AND s.id IN (1, 3, 7)\r\n\r\nGROUP BY ii.`PrimaryKey`\r\nORDER BY ii.in_order, ii.title, ii.patron_name, ii.price;");
                        break;
                    case "RP16":
                        sqry.Append("SELECT f.id, f.name, e.code, e.title, COUNT(cinema_seat_id) quantity, SUM(base_price) sales, g.system_value, h.name report_name, d.movie_date ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, ticket b, ");
                        sqry.Append("movies_schedule_list c, movies_schedule d, movies e, cinema f, config_table g, report h ");
                        sqry.Append("WHERE a.ticket_id = b.id ");
                        sqry.Append("AND a.status = 1 AND a.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id AND a.status = 1 ");
                        sqry.Append(String.Format("AND d.movie_date = '{0:yyyy/MM/dd}' AND d.movie_id = e.id AND d.cinema_id = f.id ", _dtStart));
                        sqry.Append("AND g.system_code = '001' ");
                        sqry.Append("AND h.id = 16 ");
                        sqry.Append("GROUP BY f.id, e.id ORDER BY f.in_order");
                        break;

                    case "RP11":
                        m_clscom.refreshTable(m_frmM,"tmp_gross",m_frmM._connection);
                        m_clscom.populateTable(m_frmM, "tmp_gross", m_frmM._connection,_dtStart,_dtEnd);

                        string strqry = "select * from cinema order by in_order asc";
                        DataTable dtcinema = m_clscom.setDataTable(strqry,m_frmM._connection);

                        sqry.Append("SELECT tg.report_date Report_Date ");
                        string temp = "a";
                        int cntr = 0;
                        string newtemp = ", ( ";
                        foreach (DataRow row in dtcinema.Rows)
                        {
                            cntr += 1;
                            sqry.Append("," + temp + ".gross Cinema" + row["in_order"].ToString()); // 1.6.2016 cntr.ToString());
                            if(cntr == 1)
                                newtemp += temp + ".gross";
                            else if(cntr == dtcinema.Rows.Count)
                                newtemp += " + " + temp + ".gross ) Total_Gross";
                            else
                                newtemp += " + " + temp + ".gross ";
                            temp += "a";
                        }
                        sqry.Append(newtemp);
                        //a.gross Cinema1, aa.gross Cinema2, aaa.gross Cinema3, aaaa.gross Cinema4, (a.gross + aa.gross + aaa.gross + aaaa.gross) Total_Gross,
                        sqry.Append(", g.system_value, h.name report_name ");
                        sqry.Append("FROM tmp_gross tg, ");

                        temp = "a";
                        cntr = 0;
                        foreach (DataRow row in dtcinema.Rows)
                        {
                            cntr += 1;
                            //sqry.Append(String.Format("(SELECT report_date,gross from tmp_gross where cinema_id = {0} and userid = '{1}') {2}, ",cntr,m_frmM.m_usercode,temp));
                            sqry.Append(String.Format("(SELECT report_date,gross from tmp_gross where cinema_id = {0} and userid = '{1}') {2}, ", row["id"].ToString(), m_frmM.m_usercode, temp));
                            temp += "a";
                        }
                        //sqry.Append("(SELECT report_date,gross from tmp_gross where cinema_id = 1 and userid = 'LILOY') a, ");
                        sqry.Append("config_table g, report h ");
                        sqry.Append("WHERE ");

                        temp = "a";
                        cntr = 0;
                        foreach (DataRow row in dtcinema.Rows)
                        {
                            cntr += 1;
                            if(cntr== 1)
                                sqry.Append(String.Format("{0}.report_date = tg.report_date ",temp));
                            else
                                sqry.Append(String.Format("AND {0}.report_date = tg.report_date ",temp));
                            temp += "a";
                        }
                        //sqry.Append("a.report_date = tg.report_date ");
                        //sqry.Append("AND aa.report_date = tg.report_date ");
                        //sqry.Append("AND aaa.report_date = tg.report_date ");
                        //sqry.Append("AND aaaa.report_date = tg.report_date ");

                        sqry.Append("AND g.system_code = '001' ");
                        sqry.Append("AND h.id = 11 ");
                        sqry.Append("GROUP BY tg.report_date ");
                        sqry.Append("ORDER BY tg.report_date");
                        break;
                    case "RP07":
                        m_clscom.refreshTable(m_frmM, "tmp_screening", m_frmM._connection);
                        m_clscom.populateScreening(m_frmM, "tmp_screening", m_frmM._connection,_dtStart);

                        sqry.Append("SELECT c.code, c.title, ");
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 1 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening1_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 1 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening1_amount, ", m_frmM.m_usercode));
                        //added 4.30.2019 request percentage fectival for rp07
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 1 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening1_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 2 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening2_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 2 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening2_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 2 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening2_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 3 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening3_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 3 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening3_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 3 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening3_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 4 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening4_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 4 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening4_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 4 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening4_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 5 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening5_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 5 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening5_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 5 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening5_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 6 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening6_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 6 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening6_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 6 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening6_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 7 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening7_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 7 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening7_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 7 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening7_perc, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select qty from tmp_screening where screening_id = 8 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening8_quantity, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("(select amount from tmp_screening where screening_id = 8 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) screening8_amount, ", m_frmM.m_usercode));
                        sqry.Append(String.Format("round((((select qty from tmp_screening where screening_id = 8 and userid = '{0}' and movie_id = b.movie_id and cinema_id = b.cinema_id) / (select capacity from cinema where id = b.cinema_id)) *100),2) as screening8_perc, ", m_frmM.m_usercode));
                        sqry.Append("g.system_value, h.name report_name, b.movie_date ");
                        sqry.Append("from tmp_screening b, movies c, config_table g, report h ");
                        sqry.Append("where b.movie_id = c.id ");
                        sqry.Append("and g.system_code = '001' ");
                        sqry.Append("and h.id = 7 ");
                        sqry.Append(String.Format("and b.userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("group by b.cinema_id, b.movie_id ");
                        sqry.Append("order by b.cinema_id asc");
                        break;
                    case "RP21":
                        sqry = new StringBuilder();
                        sqry.Append("select h.system_value, g.name, d.title, d.code, c.movie_date, ");
                        sqry.Append("sum(i.amount_val) ord_amt, sum(a.ordinance_price) amount ");//changed sum(a.price - a.base_price) to sum(a.ordinance_price) 1.7.2016
                        sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                        sqry.Append("movies_schedule c, movies d, patrons e, movies_schedule_list_patron f, report g, config_table h, ");
                        sqry.Append("ordinance_tbl i, patrons_ordinance j ");
                        sqry.Append("where a.movies_schedule_list_id= b.id  ");
                        sqry.Append("and b.movies_schedule_id = c.id ");
                        sqry.Append("and c.movie_id = d.id ");
                        sqry.Append("and a.patron_id = f.id ");
                        sqry.Append("and f.patron_id = e.id ");
                        sqry.Append("and e.id = j.patron_id ");
                        sqry.Append("and j.ordinance_id = i.id ");
                        sqry.Append("and i.in_pesovalue = 1 ");
                        sqry.Append("and a.status = 1 ");
                        sqry.Append("and h.system_code='001' ");
                        sqry.Append("and g.id = 21 ");
                        sqry.Append(String.Format("and c.movie_date >= '{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append(String.Format("and c.movie_date < '{0:yyyy/MM/dd}' ", _dtEnd));
                        sqry.Append("group by d.title,c.movie_date ");
                        sqry.Append("order by d.title,c.movie_date asc");

                        break;
                    case "RP22":
                        DateTime firstDayOfMonth = new DateTime(_dtStart.Year, _dtStart.Month, 1);
                        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                        m_clscom.refreshTable(m_frmM,"tmp_bir_msr",m_frmM._connection);
                        m_clscom.populateBIRmsr(m_frmM, "tmp_bir_msr", m_frmM._connection, firstDayOfMonth, lastDayOfMonth);
                        _dtStart = firstDayOfMonth;
                        _dtEnd = lastDayOfMonth.AddDays(1);

                        sqry = new StringBuilder();
                        sqry.Append(String.Format("select * from tmp_bir_msr where userid = '{0}' order by or_date asc", m_frmM.m_usercode));
                        break;
                    case "RP14":
                        /* this version need to fill the movies distributor_share table*/
                        /*version 2 table based reference for film share */
                        /*m_clscom.refreshTable(m_frmM, "tmp_accounting", m_frmM._connection);
                        MySqlConnection myconn = new MySqlConnection(m_frmM._connection);
                        StringBuilder smdqry = new StringBuilder();
                        smdqry.Append(String.Format("select distinct(movie_id) movieid from movies_schedule where cinema_id = {0} and ",_intCinemaID));
                        smdqry.Append(String.Format("movie_date between '{0:yyyy-MM-dd}' and '{1:yyyy-MM-dd}'", _dtStart, _dtEnd));
                        DataTable testdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);
                        if (testdt.Rows.Count > 0)
                        {
                            DateTime tempdate = _dtStart;
                            foreach (DataRow testdr in testdt.Rows)
                            {
                                
                                _intMovieID = -1;
                                int intout = 0;
                                if (int.TryParse(testdr["movieid"].ToString(), out intout))
                                    _intMovieID = intout;

                                if (_intMovieID > -1)
                                {
                                    myconn = new MySqlConnection(m_frmM._connection);
                                    smdqry = new StringBuilder();
                                    smdqry.Append(String.Format("select start_date from movies where id = {0} and status = 1 order by start_date asc", _intMovieID));
                                    DataTable dt1 = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                    int vercntr = 0;
                                    DateTime firstmoviedate = new DateTime();
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr1 in dt1.Rows)
                                        {
                                            DateTime dateout = new DateTime();
                                            if (DateTime.TryParse(dr1["start_date"].ToString(), out dateout))
                                            {
                                                firstmoviedate = dateout;
                                                vercntr += 1;
                                            }
                                        }
                                    }
                                    if (vercntr == 1)
                                    {
                                        DateTime stDate = new DateTime();
                                        DateTime edDate = new DateTime();
                                        DateTime effdate = new DateTime();
                                        DateTime effenddate = new DateTime();

                                        myconn = new MySqlConnection(m_frmM._connection);
                                        do
                                        {
                                            smdqry = new StringBuilder();
                                            smdqry.Append(String.Format("select * from movies_distributor_share where week_id = {0}", vercntr.ToString()));
                                            DataTable dtweek1 = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                            if(vercntr == 1)
                                            {
                                                effdate = firstmoviedate;
                                            }
                                            else
                                                effdate = tempdate;
                                            double shareperc = 0;
                                            int intid = 0;
                                            if (dtweek1.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr2 in dtweek1.Rows)
                                                {
                                                    //daycnt = Convert.ToInt32(dr2["day_count"].ToString());
                                                    double dblout = 0;
                                                    if (double.TryParse(dr2["share_perc"].ToString(), out dblout))
                                                        shareperc = dblout;
                                                    if (int.TryParse(dr2["day_count"].ToString(), out intout))
                                                        effenddate = effdate.AddDays(intout - 1);
                                                    if (int.TryParse(dr2["id"].ToString(), out intout))
                                                        intid = intout;
                                                }
                                            }
                                            if (effdate <= _dtStart && effenddate > _dtStart || effdate <= _dtEnd)
                                            {

                                                if (effenddate < tempdate)
                                                {
                                                    if (vercntr == 1)
                                                        tempdate = firstmoviedate;
                                                }

                                                stDate = tempdate;//start date range with _dtStart
                                                if (effenddate < _dtEnd)
                                                    edDate = effenddate; //use effenddate
                                                else if (effenddate >= _dtEnd)
                                                    edDate = _dtEnd;//use _enddate
                                                tempdate = effenddate.AddDays(1);

                                                m_clscom.populateAccountingTbl2(m_frmM.m_usercode, "tmp_accounting", m_frmM._connection, stDate, edDate, _intMovieID, effdate, shareperc, intid, _intCinemaID);
                                            }
                                            else
                                                tempdate = effenddate.AddDays(1);
                                            vercntr += 1;
                                        } while (effenddate < _dtEnd);


                                    }
                                    
                                }
                            }

                            sqry = new StringBuilder();
                            sqry.Append("SELECT a.*, g.system_value, h.name report_name ");
                            sqry.Append("FROM tmp_accounting a, config_table g, report h ");
                            sqry.Append("WHERE g.system_code = '001' ");
                            sqry.Append(String.Format("AND h.id = 14 AND a.userid = '{0}'", m_frmM.m_usercode));
                        }

                        break; */
                        /* version 1 working with update 2.2.2016*/
                        /* this version need to fill the movies distributor table*/
                        m_clscom.refreshTable(m_frmM, "tmp_accounting", m_frmM._connection);

                        //query for the movie id based on the cinema id and dates covered
                        MySqlConnection myconn = new MySqlConnection(m_frmM._connection);
                        StringBuilder smdqry = new StringBuilder();
                        smdqry.Append(String.Format("select distinct(movie_id) movieid from movies_schedule where cinema_id = {0} and ",_intCinemaID));
                        smdqry.Append(String.Format("movie_date between '{0:yyyy-MM-dd}' and '{1:yyyy-MM-dd}'", _dtStart, _dtEnd));
                        DataTable testdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);
                        if (testdt.Rows.Count > 0)
                        {
                            foreach (DataRow testdr in testdt.Rows)
                            {
                                _intMovieID = -1;
                                int intout = 0;
                                if (int.TryParse(testdr["movieid"].ToString(), out intout))
                                    _intMovieID = intout;

                                if (_intMovieID > -1)
                                {
                                    //query on the date if what movie_distributor value to use
                                    myconn = new MySqlConnection(m_frmM._connection);
                                    smdqry = new StringBuilder();
                                    smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                    smdqry.Append(String.Format("effective_date = '{0:yyyy-MM-dd}'", _dtStart));
                                    DataTable dt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                    if (dt.Rows.Count == 0)
                                    {
                                        smdqry = new StringBuilder();
                                        smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                        smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' order by effective_date desc limit 1", _dtStart));
                                        dt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);
                                        if (dt.Rows.Count > 0)
                                        {
                                            DateTime effdate = new DateTime();
                                            int addday = 0;
                                            foreach (DataRow dr in dt.Rows)
                                            {
                                                DateTime dateout = new DateTime();
                                                if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                                    effdate = dateout;
                                                addday = Convert.ToInt32(dr["day_count"].ToString());
                                            }
                                            if (effdate.AddDays(addday - 1) < _dtStart)
                                            {
                                                dt = new DataTable();
                                            }

                                            if (_dtEnd > effdate.AddDays(addday - 1))
                                            {
                                                smdqry = new StringBuilder();
                                                smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                                //smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));//remarked2.22.2016
                                                smdqry.Append(String.Format("effective_date <= '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));
                                                DataTable newdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                                dt.Merge(newdt);

                                            }
                                        }
                                        else if (dt.Rows.Count == 0)
                                        {
                                            smdqry = new StringBuilder();
                                            smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                            smdqry.Append(String.Format("effective_date > '{0:yyyy-MM-dd}' order by effective_date limit 1", _dtStart));
                                            dt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);
                                            if (dt.Rows.Count > 0)
                                            {
                                                DateTime effdate = new DateTime();
                                                int addday = 0;
                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    DateTime dateout = new DateTime();
                                                    if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                                        effdate = dateout;
                                                    addday = Convert.ToInt32(dr["day_count"].ToString());
                                                }
                                                if (_dtEnd >= effdate.AddDays(addday - 1))
                                                {
                                                    smdqry = new StringBuilder();
                                                    smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                                    //smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));//remarked 2.22.2016
                                                    smdqry.Append(String.Format("effective_date <= '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));
                                                    DataTable newdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                                    dt.Merge(newdt);
                                                }
                                            }
                                        }
                                    }
                                    else if (dt.Rows.Count > 0)
                                    {
                                        DateTime effdate = new DateTime();
                                        int addday = 0;
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            DateTime dateout = new DateTime();
                                            if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                                effdate = dateout;
                                            addday = Convert.ToInt32(dr["day_count"].ToString());
                                        }
                                        if (effdate.AddDays(addday - 1) < _dtStart)
                                        {
                                            dt = new DataTable();
                                        }

                                        if (_dtEnd > effdate.AddDays(addday - 1))
                                        {
                                            smdqry = new StringBuilder();
                                            smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                            //smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));//remarked 2.22.2016
                                            smdqry.Append(String.Format("effective_date <= '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));
                                            DataTable newdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                            dt.Merge(newdt);

                                        }
                                    }

                                    if (dt.Rows.Count > 0)
                                    {
                                        DataView dv = dt.AsDataView();
                                        dv.Sort = "effective_date asc";
                                        dt = dv.ToTable();

                                        DateTime tempdate = _dtStart;
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            DateTime stDate = new DateTime();
                                            DateTime edDate = new DateTime();
                                            DateTime effdate = new DateTime();
                                            DateTime effenddate = new DateTime();
                                            DateTime dateout = new DateTime();
                                            double shareperc = 0;
                                            if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                                effdate = dateout;

                                            intout = 0;
                                            //int intdays = 0;
                                            if (int.TryParse(dr["day_count"].ToString(), out intout))
                                                effenddate = effdate.AddDays(intout - 1);

                                            double dblout = 0;
                                            if (double.TryParse(dr["share_perc"].ToString(), out dblout))
                                                shareperc = dblout;

                                            int intid = 0;
                                            if (int.TryParse(dr["id"].ToString(), out intout))
                                                intid = intout;

                                            if (effdate < tempdate)
                                            {
                                                //start date range with effdate
                                                stDate = tempdate;
                                                if (effenddate < _dtEnd)
                                                {
                                                    edDate = effenddate; //use effenddate
                                                    tempdate = effenddate.AddDays(1);
                                                }
                                                else if (effenddate >= _dtEnd)
                                                {
                                                    edDate = _dtEnd;//use _enddate
                                                }
                                            }
                                            else
                                            {
                                                if (effdate >= tempdate)
                                                {
                                                    stDate = tempdate;//start date range with _dtStart
                                                    if (effenddate < _dtEnd)
                                                    {
                                                        edDate = effenddate; //use effenddate
                                                        tempdate = effenddate.AddDays(1);
                                                    }
                                                    else if (effenddate >= _dtEnd)
                                                    {
                                                        edDate = _dtEnd;//use _enddate
                                                    }
                                                }
                                            }

                                            m_clscom.populateAccountingTbl2(m_frmM.m_usercode, "tmp_accounting", m_frmM._connection, stDate, edDate, _intMovieID, effdate, shareperc, intid, _intCinemaID);
                                        }
                                    }
                                }

                                sqry = new StringBuilder();
                                sqry.Append("SELECT a.*, g.system_value, h.name report_name ");
                                sqry.Append("FROM tmp_accounting a, config_table g, report h ");
                                sqry.Append("WHERE g.system_code = '001' ");
                                sqry.Append(String.Format("AND h.id = 14 AND a.userid = '{0}'", m_frmM.m_usercode));
                            }
                        }
                        break;
                    /*case "RP14": //first version by movie changes 6.26.2015
                        m_clscom.refreshTable(m_frmM, "tmp_accounting", m_frmM._connection);

                        //query on the date if what movie_distributir value to use
                        MySqlConnection myconn = new MySqlConnection(m_frmM._connection);
                        StringBuilder smdqry = new StringBuilder();
                        smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ",_intMovieID));
                        smdqry.Append(String.Format("effective_date = '{0:yyyy-MM-dd}'", _dtStart));
                        DataTable dt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                        if(dt.Rows.Count == 0)
                        {
                            smdqry = new StringBuilder();
                            smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ",_intMovieID));
                            smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' order by effective_date desc limit 1",_dtStart));
                            dt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);
                            if (dt.Rows.Count > 0)
                            {
                                DateTime effdate = new DateTime();
                                int addday = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    DateTime dateout = new DateTime();
                                    if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                        effdate = dateout;
                                    addday = Convert.ToInt32(dr["day_count"].ToString());
                                }
                                if (effdate.AddDays(addday - 1) < _dtStart)
                                {
                                    dt = new DataTable();
                                }

                                if (_dtEnd > effdate.AddDays(addday - 1))
                                {
                                    smdqry = new StringBuilder();
                                    smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                    smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));
                                    DataTable newdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                    dt.Merge(newdt);
                                    
                                }
                            }
                        }
                        else if (dt.Rows.Count > 0)
                        {
                            DateTime effdate = new DateTime();
                            int addday = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                DateTime dateout = new DateTime();
                                if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                    effdate = dateout;
                                addday = Convert.ToInt32(dr["day_count"].ToString());
                            }
                            if (effdate.AddDays(addday - 1) < _dtStart)
                            {
                                dt = new DataTable();
                            }

                            if (_dtEnd > effdate.AddDays(addday - 1))
                            {
                                smdqry = new StringBuilder();
                                smdqry.Append(String.Format("select * from movies_distributor where movie_id = {0} and ", _intMovieID));
                                smdqry.Append(String.Format("effective_date < '{0:yyyy-MM-dd}' and effective_date > '{1:yyyy-MM-dd}'", _dtEnd, effdate));
                                DataTable newdt = m_clscom.setDataTable(smdqry.ToString(), m_frmM._connection);

                                dt.Merge(newdt);

                            }
                        }

                        if(dt.Rows.Count > 0)
                        {
                            DataView dv = dt.AsDataView();
                            dv.Sort = "effective_date asc";
                            dt = dv.ToTable();

                            DateTime tempdate = _dtStart;
                            foreach (DataRow dr in dt.Rows)
                            {
                                DateTime stDate = new DateTime();
                                DateTime edDate = new DateTime();
                                DateTime effdate = new DateTime();
                                DateTime effenddate = new DateTime();
                                DateTime dateout = new DateTime();
                                double shareperc = 0;
                                if (DateTime.TryParse(dr["effective_date"].ToString(), out dateout))
                                    effdate = dateout;

                                int intout = 0;
                                int intdays = 0;
                                if (int.TryParse(dr["day_count"].ToString(), out intout))
                                    effenddate = effdate.AddDays(intout - 1);

                                double dblout = 0;
                                if(double.TryParse(dr["share_perc"].ToString(), out dblout))
                                    shareperc = dblout;

                                int intid = 0;
                                if (int.TryParse(dr["id"].ToString(), out intout))
                                    intid = intout;

                                if (effdate < tempdate)
                                {
                                    //start date range with effdate
                                    stDate = tempdate;
                                    if (effenddate < _dtEnd)
                                    {
                                        edDate = effenddate; //use effenddate
                                        tempdate = effenddate.AddDays(1);
                                    }
                                    else if (effenddate >= _dtEnd)
                                    {
                                        edDate = _dtEnd;//use _enddate
                                    }
                                }
                                else
                                {
                                    if (effdate >= tempdate)
                                    {
                                        stDate = tempdate;//start date range with _dtStart
                                        if (effenddate < _dtEnd)
                                        {
                                            edDate = effenddate; //use effenddate
                                            tempdate = effenddate.AddDays(1);
                                        }
                                        else if (effenddate >= _dtEnd)
                                        {
                                            edDate = _dtEnd;//use _enddate
                                        }
                                    }
                                }
                                //MessageBox.Show("startdate: " + String.Format("{0:d-MMM-yyyy}", stDate) + "   enddate: " + String.Format("{0:d-MMM-yyyy}", edDate));
                                m_clscom.populateAccountingTbl(m_frmM.m_usercode, "tmp_accounting", m_frmM._connection, stDate, edDate, _intMovieID, effdate, shareperc, intid);
                            }
                        }

                        sqry = new StringBuilder();
                        sqry.Append("SELECT a.*, g.system_value, h.name report_name ");
                        sqry.Append("FROM tmp_accounting a, config_table g, report h ");
                        sqry.Append("WHERE g.system_code = '001' ");
                        sqry.Append(String.Format("AND h.id = 14 AND a.userid = '{0}'", m_frmM.m_usercode));
                        break;*/
                    case "RP23":
                        string stablenm = String.Empty;
                        stablenm = "tmp_msr_transaction";
                        firstDayOfMonth = new DateTime(_dtStart.Year, _dtStart.Month, 1);
                        lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                        m_clscom.refreshTable(m_frmM, stablenm, m_frmM._connection);
                        m_clscom.populatemsrtransaction(m_frmM, stablenm, m_frmM._connection, firstDayOfMonth, lastDayOfMonth);
                        _dtStart = firstDayOfMonth;
                        _dtEnd = lastDayOfMonth;
                        
                        sqry = new StringBuilder();
                        sqry.Append("SELECT a.*, g.system_value, h.name report_name ");
                        sqry.Append(String.Format("FROM {0} a, config_table g, report h ", stablenm));
                        sqry.Append(String.Format("WHERE userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("AND g.system_code = '001'");
                        sqry.Append("AND h.id = 23 ");
                        sqry.Append("ORDER BY report_date ASC");

                        //sqry.Append(String.Format("SELECT * FROM {0} ", stablenm));
                        //sqry.Append(String.Format("WHERE userid = '{0}' ", m_frmM.m_usercode));
                        //sqry.Append("ORDER BY report_date ASC");

                        break;
                    case "RP20"://added 1.4.2016
                        sqry = new StringBuilder();
                        if (_intWithQtyBreakdown == 0)
                        {
                            sqry.Append("select h.system_value, g.name, d.title, d.code, c.movie_date, ");
                            sqry.Append("sum(i.amount_val) sur_amt, sum(a.surcharge_price) amount ");//changed sum(a.price - a.base_price) to sum(a.surcharge_price) 1.7.2016
                            sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                            sqry.Append("movies_schedule c, movies d, patrons e, movies_schedule_list_patron f, report g, config_table h, ");
                            sqry.Append("surcharge_tbl i, patrons_surcharge j ");
                            sqry.Append("where a.movies_schedule_list_id= b.id  ");
                            sqry.Append("and b.movies_schedule_id = c.id ");
                            sqry.Append("and c.movie_id = d.id ");
                            sqry.Append("and a.patron_id = f.id ");
                            sqry.Append("and f.patron_id = e.id ");
                            sqry.Append("and e.id = j.patron_id ");
                            sqry.Append("and j.surcharge_id = i.id ");
                            sqry.Append("and i.in_pesovalue = 1 ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and h.system_code='001' ");
                            sqry.Append("and g.id = 20 ");
                            sqry.Append(String.Format("and c.movie_date >= '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append(String.Format("and c.movie_date < '{0:yyyy/MM/dd}' ", _dtEnd));
                            sqry.Append("group by d.title,c.movie_date ");
                            sqry.Append("order by d.title,c.movie_date asc");
                        }else if (_intWithQtyBreakdown == 1)
                        {
                            sqry.Append("select h.system_value, g.name, d.title, d.code, c.movie_date,");
                            sqry.Append("i.details surcharge, a.surcharge_price price, count(i.amount_val) quantity, SUM(a.surcharge_price) amount ");//changed sum(a.price - a.base_price) to sum(a.surcharge_price) 1.7.2016
                            sqry.Append("from movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                            sqry.Append("movies_schedule c, movies d, patrons e, movies_schedule_list_patron f, report g, config_table h, ");
                            sqry.Append("surcharge_tbl i, patrons_surcharge j ");
                            sqry.Append("where a.movies_schedule_list_id= b.id  ");
                            sqry.Append("and b.movies_schedule_id = c.id ");
                            sqry.Append("and c.movie_id = d.id ");
                            sqry.Append("and a.patron_id = f.id ");
                            sqry.Append("and f.patron_id = e.id ");
                            sqry.Append("and e.id = j.patron_id ");
                            sqry.Append("and j.surcharge_id = i.id ");
                            sqry.Append("and i.in_pesovalue = 1 ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and h.system_code='001' ");
                            sqry.Append("and g.id = 20 ");
                            sqry.Append(String.Format("and c.movie_date >= '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append(String.Format("and c.movie_date < '{0:yyyy/MM/dd}' ", _dtEnd));
                            sqry.Append("group by d.title,c.movie_date,a.surcharge_price ");
                            sqry.Append("order by d.title,c.movie_date asc");
                        }
                        break;
                    case "RP00":
                        sqry = new StringBuilder();
                        sqry.Append("SELECT a.id, a.code, a.name, a.description, b.system_value ");
                        sqry.Append("FROM report a, config_table b ");
                        sqry.Append("WHERE b.system_code = '001' ");
                        sqry.Append("ORDER BY a.code; ");
                        break;
                    case "RP19":
                        sqry = new StringBuilder();
                        sqry.Append("SELECT table1.voidamount, table2.totalcnt, table2.totalgross, c.system_value, d.name, table2.movie_date ");
                        sqry.Append("from (SELECT IFNULL(SUM(a.price), 0) AS voidamount ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, ticket d ");
                        sqry.Append("WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id ");
                        sqry.Append("AND a.ticket_id = d.id ");
                        sqry.AppendFormat("AND c.movie_date = '{0:yyyy/MM/dd}' AND a.status = 0)table1, ", _dtStart);
                        sqry.Append("(SELECT IFNULL(COUNT(a.id), 0) AS totalcnt, IFNULL(SUM(a.price), 0) AS totalgross, c.movie_date ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c, ticket e ");
                        sqry.Append("WHERE a.movies_schedule_list_id = b.id AND b.movies_schedule_id = c.id  AND a.ticket_id = e.id ");
                        sqry.AppendFormat("AND c.movie_date = '{0:yyyy/MM/dd}' AND a.status = 1) table2, config_table c, report d ",_dtStart);
                        sqry.Append("WHERE c.system_code = '001' ");
                        sqry.Append("and d.id = 19");
                        break;
                    case "RP24":
                        //added 4.21.2016
                        stablenm = String.Empty;
                        stablenm = "tmp_pos";
                        m_clscom.refreshTable(m_frmM, stablenm, m_frmM._connection);
                        m_clscom.populatePOSTable(m_frmM, stablenm, m_frmM._connection,_dtStart,_dtEnd);

                        sqry = new StringBuilder();
                        sqry.Append("SELECT a.*, g.system_value, h.name report_name ");
                        sqry.Append(String.Format("FROM {0} a, config_table g, report h ", stablenm));
                        sqry.Append(String.Format("WHERE userid = '{0}' ", m_frmM.m_usercode));
                        sqry.Append("AND g.system_code = '001' ");
                        sqry.Append("AND h.id = 24 ");
                        sqry.Append("ORDER BY id ASC");
                        break;
                    case "RP25":
                        sqry = new StringBuilder();
                        sqry.Append("SELECT ");
                        sqry.Append("MAX(CASE WHEN system_desc = 'CINEMA NAME' THEN system_value END) cinema_name, ");
                        sqry.Append("MAX(CASE WHEN system_desc = 'CINEMA ADDRESS' THEN system_value END) cinema_addr, ");
                        sqry.Append("MAX(CASE WHEN system_desc = 'CINEMA ADDRESS2' THEN system_value END) cinema_addr2, ");
                        sqry.Append("MAX(CASE WHEN system_desc = 'TIN' THEN system_value END) tin, ");
                        sqry.AppendFormat("MAX(CASE WHEN system_desc = 'MIN_{0}' THEN system_value END) ticket_min, ", _posterminal);
                        sqry.AppendFormat("MAX(CASE WHEN system_desc = 'POS_NO_{0}' THEN system_value END) ticket_posno, ", _posterminal);
                        sqry.Append("MAX(CASE WHEN system_desc = 'ACCREDITATION' THEN system_value END) accreditation, ");
                        sqry.AppendFormat("MAX(CASE WHEN system_desc = 'PN_{0}' THEN system_value END) ticket_ptu ",_posterminal);
                        sqry.Append("FROM config_table");

                        StringBuilder sqry1 = new StringBuilder();
                        sqry1.Append("SELECT COUNT(a.id) z_cnt, SUM(a.price) price, SUM(a.base_price) base_price, ");
                        sqry1.Append("SUM(a.amusement_tax_amount) amusement_tax_amount, SUM(a.cultural_tax_amount) cultural_tax_amount, ");
                        sqry1.Append("SUM(a.vat_amount) vat_amount,MIN(a.or_number) min_or_number,MAX(or_number) max_or_number, ");
                        sqry1.Append("SUM(a.surcharge_price) surcharge_price,b.terminal,b.ticket_datetime, ");
                        sqry1.Append("COUNT(DISTINCT(b.session_id))session_cnt,b.status,d.code, ");
                        sqry1.Append("SUM(CASE WHEN d.name LIKE 'SC -%' THEN 1 ELSE 0 END) sc_cnt, ");
                        sqry1.Append("(MAX(CASE WHEN d.name = 'REG' THEN a.price END) * 0.2) * SUM(CASE WHEN d.name LIKE 'SC -%' THEN 1 ELSE 0 END) sc_sum, ");
                        sqry1.Append("SUM(CASE WHEN d.name LIKE 'PWD%' THEN 1 ELSE 0 END) pwd_cnt, ");
                        sqry1.Append("(MAX(CASE WHEN d.name = 'REG' THEN a.price END) * 0.2) * SUM(CASE WHEN d.name LIKE 'PWD%' THEN 1 ELSE 0 END) pwd_sum, ");
                        sqry1.Append("concat(e.lname, ', ', e.fname) username ");
                        sqry1.Append("FROM movies_schedule_list_reserved_seat a, ticket b,  ");
                        sqry1.Append("movies_schedule_list_patron c, patrons d, users e ");
                        sqry1.Append("WHERE a.ticket_id = b.id ");
                        sqry1.Append("AND a.patron_id = c.id ");
                        sqry1.Append("AND c.patron_id = d.id ");
                        sqry1.Append("AND b.user_id = e.id ");
                        sqry1.Append("AND b.status = 1 ");
                        sqry1.AppendFormat("AND b.ticket_datetime BETWEEN '{0:yyyy-MM-dd}' AND '{1:yyyy-MM-dd}' ", _dtStart,_dtStart.AddDays(1));
                        sqry1.AppendFormat("AND b.terminal = '{0}'",_posterminal);

                        StringBuilder sqry2 = new StringBuilder();
                        sqry2.Append("select count(a.id),sum(price) init_balance ");
                        sqry2.Append("from movies_schedule_list_reserved_seat a, ticket b ");
                        sqry2.Append("where a.ticket_id = b.id ");
                        sqry2.AppendFormat("and b.terminal = '{0}' ",_posterminal);
                        sqry2.AppendFormat("and b.ticket_datetime < '{0:yyyy-MM-dd}' ", _dtStart);
                        sqry2.Append("and a.status = 1 ");
                        sqry2.Append("and b.status = 1");

                        StringBuilder sqry3 = new StringBuilder();
                        sqry3.Append("select count(a.id),sum(price) end_balance,COUNT(DISTINCT(b.session_id))session_cnt ");
                        sqry3.Append("from movies_schedule_list_reserved_seat a, ticket b ");
                        sqry3.Append("where a.ticket_id = b.id ");
                        sqry3.AppendFormat("and b.terminal = '{0}' ",_posterminal);
                        sqry3.AppendFormat("and b.ticket_datetime < '{0:yyyy-MM-dd}' ", _dtStart.AddDays(1));
                        sqry3.Append("and a.status = 1 ");
                        sqry3.Append("and b.status = 1");

                        StringBuilder sqry4 = new StringBuilder();
                        sqry4.Append("select count(a.id)ticket_cnt,COUNT(DISTINCT(b.session_id))session_cnt, ");
                        sqry4.Append("if (sum(a.price) is null,0,sum(a.price)) sum_price ");
                        sqry4.Append("from movies_schedule_list_reserved_seat a, ticket b ");
                        sqry4.Append("where a.ticket_id = b.id ");
                        sqry4.AppendFormat("and b.terminal = '{0}' ",_posterminal);
                        sqry4.AppendFormat("and b.ticket_datetime >= '{0:yyyy-MM-dd}' ", _dtStart);
                        sqry4.AppendFormat("and b.ticket_datetime < '{0:yyyy-MM-dd}' ", _dtStart.AddDays(1));
                        sqry4.Append("and a.status = 2 ");
                        sqry4.Append("and b.status = 2");

                        xmlfile = GetXmlStringPOS(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + ".xml", 
                            sqry.ToString(), sqry1.ToString(),sqry2.ToString(), sqry3.ToString(), sqry4.ToString(),
                            m_frmM._odbcconnection, _dtStart, m_clsconfig, String.Format("{0:0.00}", Convert.ToDouble(_poschangefund)), m_frmM.m_usernm);
                        break;
                    case "RP072":
                        sqry = new StringBuilder();
                        sqry.Append("SELECT t1.movie_date, t1.cinema_name, t1.movie_id, t1.title,g.system_value, h.name report_name,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 1 THEN t1.qtty ELSE 0 END) AS screening1_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 1 THEN t1.qtty * t1.amt ELSE 0 END) AS screening1_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 1 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 1 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening1_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 2 THEN t1.qtty ELSE 0 END) AS screening2_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 2 THEN t1.qtty * t1.amt ELSE 0 END) AS screening2_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 2 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 2 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening2_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 3 THEN t1.qtty ELSE 0 END) AS screening3_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 3 THEN t1.qtty * t1.amt ELSE 0 END) AS screening3_amount,");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 3 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 3 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening3_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 4 THEN t1.qtty ELSE 0 END) AS screening4_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 4 THEN t1.qtty * t1.amt ELSE 0 END) AS screening4_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 4 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 4 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening4_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 5 THEN t1.qtty ELSE 0 END) AS screening5_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 5 THEN t1.qtty * t1.amt ELSE 0 END) AS screening5_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 5 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 5 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening5_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 6 THEN t1.qtty ELSE 0 END) AS screening6_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 6 THEN t1.qtty * t1.amt ELSE 0 END) AS screening6_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 6 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 6 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening6_perc,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 7 THEN t1.qtty ELSE 0 END) AS screening7_quantity,");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 7 THEN t1.qtty * t1.amt ELSE 0 END) AS screening7_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 7 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 7 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening7_perc, ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 8 THEN t1.qtty ELSE 0 END) AS screening8_quantity, ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 8 THEN t1.qtty * t1.amt ELSE 0 END) AS screening8_amount, ");
                        sqry.Append("ROUND((MAX(CASE WHEN t1.screening_id = 8 THEN t1.qtty ELSE 0 END) / ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 8 THEN t1.capacity ELSE 1 END)) * 100, 2) AS screening8_perc, ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 1 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 2 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 3 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 4 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 5 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 6 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 7 THEN t1.qtty ELSE 0 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 8 THEN t1.qtty ELSE 0 END) AS total_qtty, ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 1 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 2 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 3 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 4 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 5 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 6 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 7 THEN t1.capacity ELSE 1 END) + ");
                        sqry.Append("MAX(CASE WHEN t1.screening_id = 8 THEN t1.capacity ELSE 1 END) AS total_capacity, ");
                        sqry.Append("t1.in_order, t1.amt ");
                        sqry.Append("FROM (SELECT ");
                        sqry.Append("@row_number:= CASE ");
                        sqry.Append("WHEN @movie_no = d.movie_id AND @cinema_no = cinema_id THEN @row_number + 1 ");
                        sqry.Append("ELSE 1 ");
                        sqry.Append("END AS screening_id, ");
                        sqry.Append("@cinema_no:=f.id AS cinema_id, ");
                        sqry.Append("f.name cinema_name, ");
                        sqry.Append("@movie_no:= d.movie_id AS movie_id, ");
                        sqry.Append("g.code AS movie_code, ");
                        sqry.Append("g.title, ");
                        sqry.Append("COUNT(b.ticket_id) qtty, ");
                        sqry.Append("MAX(b.base_price) amt, f.capacity, f.in_order, c.start_time, c.end_time, d.movie_date ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, cinema f, ");
                        sqry.Append("(SELECT @movie_no:= 0, @cinema_no:=0, @row_number:= 0) as t, movies g ");
                        sqry.Append("WHERE b.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id AND d.cinema_id = f.id ");
                        sqry.AppendFormat("AND b.status = 1 AND c.start_time > '{0:yyyy-MM-dd}' AND c.start_time < '{1:yyyy-MM-dd}' ", _dtStart, _dtEnd.AddDays(1));
                        sqry.Append("AND c.status = 1 AND g.id = d.movie_id ");
                        sqry.Append("GROUP BY d.cinema_id , d.movie_id, b.movies_schedule_list_id ");
                        sqry.Append("ORDER BY c.movies_schedule_id , c.start_time) t1, ");
                        sqry.Append("config_table g, report h ");
                        sqry.Append("WHERE g.system_code = '001' ");
                        sqry.Append("AND h.id = 7 ");
                        sqry.AppendFormat("AND t1.movie_id = {0} ", _intMovieID);
                        sqry.Append("GROUP BY t1.movie_date,t1.cinema_id, t1.movie_id ");
                        sqry.Append("ORDER BY t1.in_order,t1.movie_date");


                        xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + ".xml",
                            sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd, m_clsconfig);
                        break;
                    case "RP26":
                        sqry = new StringBuilder();
                        sqry.Append("SELECT date(c.start_time) as movie_date, sum(a.price) as sales, count(a.id) as taken_seats, ");
                        sqry.Append("table1.total_seats, round((count(a.id) / table1.total_seats) * 100, 2) as occupancy, j.system_value, k.name as report_name ");
                        sqry.Append("FROM movies_schedule_list_reserved_seat a, ticket b, movies_schedule_list c, ");
                        sqry.Append("(SELECT f.movie_date,sum(g.capacity) as total_seats ");
                        sqry.Append("FROM movies_schedule f, cinema g, movies_schedule_list h ");
                        sqry.Append("WHERE f.cinema_id = g.id ");
                        sqry.Append("AND f.id = h.movies_schedule_id ");
                        sqry.AppendFormat("AND f.movie_date between '{0:yyyy-MM-dd}' AND '{1:yyyy-MM-dd}' ", _dtStart, _dtEnd);
                        sqry.Append("AND h.status = 1 ");
                        sqry.Append("GROUP BY f.movie_date, date(h.start_time) ");
                        sqry.Append(")table1, config_table j, report k ");
                        sqry.Append("WHERE a.ticket_id = b.id ");
                        sqry.Append("AND a.movies_schedule_list_id = c.id ");
                        sqry.AppendFormat("AND c.start_time >= '{0:yyyy-MM-dd}' ",_dtStart);
                        sqry.AppendFormat("AND c.start_time < '{0:yyyy-MM-dd}' ",_dtEnd.AddDays(1));
                        sqry.Append("AND date(c.start_time) = date(table1.movie_date) ");
                        sqry.Append("AND c.status = 1 ");
                        sqry.Append("AND b.status = 1 ");
                        sqry.Append("AND a.status = 1 ");
                        sqry.Append("AND j.system_code = '001' ");
                        sqry.Append("AND k.id = '26' ");
                        sqry.Append("GROUP BY date(c.start_time)");
                        break;

                    case "RP27":
                        sqry = new StringBuilder();
                        if (_intWithUtil == 1)
                        {
                            sqry.Append("SELECT table1.PATRON,table1.QTY,table1.PRICE, table1.SALES, table1.START_TIME, table1.END_TIME,");
                            sqry.Append("table1.SYSTEM_VAL,table1.REPORT_NAME,table1.CINEMA_NAME,table1.TITLE,table1.moveiedatetime,");
                            sqry.Append("table1.cinemaid,table1.movieid,table2.SEATTAKEN, table2.SEATTOTAL, table2.UTIL FROM ");
                            sqry.Append("(SELECT c.name PATRON,IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE,IFNULL(SUM(a.base_price), 0) SALES,");
                            sqry.Append("MIN(d.start_time) START_TIME, MAX(d.end_time) END_TIME, f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME,");
                            sqry.Append("h.TITLE,Date(j.ticket_datetime) moveiedatetime, e.id cinemaid, h.id movieid ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, movies_schedule_list d,");
                            sqry.Append("cinema e, config_table f, report g, movies h, movies_schedule i, ticket j ");
                            sqry.Append("WHERE d.movies_schedule_id = i.id AND d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("AND a.ticket_id = j.id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 27 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                            sqry.Append("AND date(j.ticket_datetime) < date(d.start_time) ");
                            sqry.Append("GROUP BY date(j.ticket_datetime), c.name) table1 ");
                            sqry.Append("LEFT JOIN ");
                            sqry.Append("(SELECT tbl1.cinemacode, tbl1.movieid, SUM(tbl1.seattaken) SEATTAKEN, SUM(tbl1.seattotal) SEATTOTAL, ((SUM(tbl1.seattaken) / SUM(tbl1.seattotal)) *100) UTIL ");
                            sqry.Append("FROM(SELECT f.id cinemacode, e.id movieid, COUNT(b.cinema_seat_id) seattaken, f.capacity * COUNT(DISTINCT(c.id)) seattotal ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat b, movies_schedule_list c, movies_schedule d, movies e, cinema f ");
                            sqry.Append("WHERE b.movies_schedule_list_id IN ");
                            sqry.Append(String.Format("(SELECT a.id FROM movies_schedule_list a WHERE a.start_time > '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append(String.Format("AND a.start_time < '{0:yyyy/MM/dd}' AND a.status = 1) ", _dtStart.AddDays(1)));
                            sqry.Append("AND b.movies_schedule_list_id = c.id AND c.movies_schedule_id = d.id ");
                            sqry.Append("AND d.movie_id = e.id AND d.cinema_id = f.id AND b.status = 1 ");
                            sqry.Append("GROUP BY d.cinema_id, e.id) tbl1 ");
                            sqry.Append("GROUP BY tbl1.cinemacode, tbl1.movieid ");
                            sqry.Append(String.Format("HAVING tbl1.cinemacode = {0} ", _intCinemaID));
                            sqry.Append(String.Format("AND tbl1.movieid = {0}) table2 ", _intMovieID));
                            sqry.Append("ON table1.cinemaid = table2.cinemacode AND table1.movieid = table2.movieid");
                        }
                        else if (_intWithUtil == 0)
                        {
                            sqry.Append("SELECT c.name PATRON,IFNULL(COUNT(a.cinema_seat_id), 0) QTY, a.base_price PRICE,IFNULL(SUM(a.base_price), 0) SALES,");
                            sqry.Append("MIN(d.start_time) START_TIME, MAX(d.end_time) END_TIME, f.system_value SYSTEM_VAL, g.name REPORT_NAME, e.name CINEMA_NAME,");
                            sqry.Append("h.TITLE,Date(j.ticket_datetime) ");
                            sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list_patron b, patrons c, movies_schedule_list d,");
                            sqry.Append("cinema e, config_table f, report g, movies h, movies_schedule i, ticket j ");
                            sqry.Append("WHERE d.movies_schedule_id = i.id AND d.status = 1 ");
                            sqry.Append(String.Format("and i.cinema_id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and i.movie_id = {0} ", _intMovieID));
                            sqry.Append(String.Format("and i.movie_date = '{0:yyyy/MM/dd}' ", _dtStart));
                            sqry.Append("and d.id = a.movies_schedule_list_id ");
                            sqry.Append("AND a.ticket_id = j.id ");
                            sqry.Append("and a.patron_id = b.id ");
                            sqry.Append("and b.patron_id = c.id ");
                            sqry.Append("and f.system_code = '001' ");
                            sqry.Append("and a.status = 1 ");
                            sqry.Append("and g.id = 27 ");
                            sqry.Append(String.Format("and e.id = {0} ", _intCinemaID));
                            sqry.Append(String.Format("and h.id = {0} ", _intMovieID));
                            sqry.Append("AND date(j.ticket_datetime) < date(d.start_time) ");
                            sqry.Append("GROUP BY date(j.ticket_datetime), c.name");
                        }
                        break;
                }
                if ((reportcode != "RP25") && (reportcode != "RP072"))
                {
                    Console.WriteLine(Application.ExecutablePath);
                    xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + ".xml", sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd, m_clsconfig);
                    if((reportcode == "RP08") && (_intWithUtil == 1))
                        xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + "-2.xml", sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd, m_clsconfig);
                    else if ((reportcode == "RP20") && (_intWithQtyBreakdown == 1))
                        xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + "-2.xml", sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd, m_clsconfig);
                    else if ((reportcode == "RP27") && (_intWithUtil == 1))
                        xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + "-2.xml", sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd, m_clsconfig);
                }
                //MessageBox.Show(sqry.ToString());

                rdlViewer1.SourceRdl = xmlfile;
                rdlViewer1.Rebuild();
                //MessageBox.Show(xmlfile.ToString());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return;
            }
        }

        static string GetXmlString(string strFile, string sQry, string sConnString, string cine, string code, DateTime _dtStart, DateTime _dtEnd, clsConfig clscon)
        {
            clscommon clscom = new clscommon();
            double gttoday = 0;
            double gtyesterday = 0;
            DataTable m_dtconfig = new DataTable();
            string companyname = String.Empty;
            string companyaddress = String.Empty;
            string tin = String.Empty;
            string accreditation = String.Empty;
            string permitno = String.Empty;
            string serialno = String.Empty;

            if (code == "RP16")
            {
                gttoday = clscom.calculateTotalCollection(sConnString, _dtStart);
                gtyesterday = clscom.calculateTotalCollection(sConnString, _dtStart.AddDays(-1));
            }
            else if(code == "RP22")
            {
                companyname = clscon.CinemaName + " (" + clscon.CinemaAddress + ")";
                companyaddress = clscon.CinemaAddress2;
                tin = clscon.TIN;
                accreditation = clscon.CinemaAccreditationNo;
                permitno = clscon.PN;
                serialno = clscon.CinemaSerialNo;
                _dtEnd = _dtEnd.AddDays(-1);
            }

            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(strFile))
            {
                try
                {
                    xmlDoc.Load(strFile);
                }
                catch (XmlException e)
                {
                    Console.WriteLine(e.Message);
                }
                XmlNode nodes = null;
                XmlNode node = xmlDoc.DocumentElement;
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    
                    int flag = 0;
                    foreach (XmlNode node2 in node1.ChildNodes)
                    {

                        
                        if (node2.Name == "DataSet")//CommandText
                        {
                            
                            foreach (XmlNode node3 in node2.ChildNodes)
                            {
                                if (node3.Name == "Query")
                                {
                                    foreach (XmlNode node4 in node3.ChildNodes)
                                    {
                                        if (node4.Name == "CommandText")
                                            node4.InnerText = sQry;

                                    }
                                }
                            }
                            //rmb 11.28.2014 REMARKED
                            /* if (code == "RP02")
                            {
                               if (flag == 0)
                                {
                                    //sQry = String.Format("select '{0:yyyy/MM/dd}' as date1 , '{0:yyyy/MM/dd}' "+
                                    //     "as date2, system_value from config_table where system_code='001'", _dtStart, _dtEnd);
                                    sQry = "select '" + _dtStart.ToShortDateString() + "' as date1, '"+
                                         _dtEnd.ToShortDateString() + "' as date2," +
                                        "system_value from config_table where system_code='001'";
                                }
                                
                            }

                            else*/ if (code == "AUDIT")
                            {
                                sQry = " Select '" + _dtStart.ToShortDateString() + "' as date_from, '" + _dtEnd.AddDays(-1).ToShortDateString() + "' as date_to;";
                            }
                            
                            flag++;
                        }



                        if (node2.Name == "DataSource")//CommandText
                        {
                            foreach (XmlNode node3 in node2.ChildNodes)
                            {
                                if (node3.Name == "ConnectionProperties")
                                {
                                    foreach (XmlNode node4 in node3.ChildNodes)
                                    {
                                        if (node4.Name == "ConnectString")
                                            node4.InnerText = sConnString;
                                    }
                                }
                            }
                        }

             
                    }
                    //RMB 11.10.2014 added start for insertion of query from date to date (txtfrdatetodate)
                    if (node1.Name == "Body")
                    {
                        foreach (XmlNode node2 in node1.ChildNodes)
                        {
                            if (node2.Name == "ReportItems")
                            {
                                foreach (XmlNode node3 in node2.ChildNodes)
                                {
                                    if (node3.Name == "Table")
                                    {
                                        foreach (XmlNode node4 in node3.ChildNodes)
                                        {
                                            if (node4.Name == "Header")
                                            {
                                                foreach (XmlNode node5 in node4.ChildNodes)
                                                {
                                                    if (node5.Name == "TableRows")
                                                    {
                                                        foreach (XmlNode node6 in node5.ChildNodes)
                                                        {
                                                            if (node6.Name == "TableRow")
                                                            {
                                                                foreach (XmlNode node7 in node6.ChildNodes)
                                                                {
                                                                    if (node7.Name == "TableCells")
                                                                    {
                                                                        foreach (XmlNode node8 in node7.ChildNodes)
                                                                                                 {
                                                                            if (node8.Name == "TableCell")
                                                                                                     {
                                                                                foreach (XmlNode node9 in node8.ChildNodes)
                                                                                {
                                                                                    if (node9.Name == "ReportItems")
                                                                                    {
                                                                                        foreach (XmlNode node10 in node9.ChildNodes)
                                                                                        {
                                                                                            if (node10.FirstChild.InnerText == "txtfrdatetodate")
                                                                                                node10.FirstChild.InnerText = "From: " + String.Format("{0:MMM dd, yyyy}", _dtStart) + " to " + String.Format("{0:MMM dd, yyyy}", _dtEnd);//_dtEnd.AddDays(-1));
                                                                                            else if (node10.FirstChild.InnerText == "txtcompany")
                                                                                                node10.FirstChild.InnerText = companyname;
                                                                                            else if (node10.FirstChild.InnerText == "txtaddress")
                                                                                                node10.FirstChild.InnerText = companyaddress;
                                                                                            else if (node10.FirstChild.InnerText == "txttin")
                                                                                                node10.FirstChild.InnerText = "TIN No.: " + tin.ToString();
                                                                                            else if (node10.FirstChild.InnerText == "txtaccreditation")
                                                                                                node10.FirstChild.InnerText = "Accreditation No.: " + accreditation.ToString();
                                                                                            else if (node10.FirstChild.InnerText == "txtpermit")
                                                                                                node10.FirstChild.InnerText = "Permit No.: " + permitno.ToString();
                                                                                            else if (node10.FirstChild.InnerText == "txtserial")
                                                                                                node10.FirstChild.InnerText = "Server Serial No.: " + serialno.ToString();
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (node4.Name == "Footer")
                                            {
                                                foreach (XmlNode node5 in node4.ChildNodes)
                                                {
                                                    if (node5.Name == "TableRows")
                                                    {
                                                        foreach (XmlNode node6 in node5.ChildNodes)
                                                        {
                                                            if (node6.Name == "TableRow")
                                                            {
                                                                foreach (XmlNode node7 in node6.ChildNodes)
                                                                {
                                                                    if (node7.Name == "TableCells")
                                                                    {
                                                                        foreach (XmlNode node8 in node7.ChildNodes)
                                                                        {
                                                                            if (node8.Name == "TableCell")
                                                                            {
                                                                                foreach (XmlNode node9 in node8.ChildNodes)
                                                                                {
                                                                                    if (node9.Name == "ReportItems")
                                                                                    {
                                                                                        foreach (XmlNode node10 in node9.ChildNodes)
                                                                                        {
                                                                                            if (node10.FirstChild.InnerText == "txtgrandtotaltoday")
                                                                                                node10.FirstChild.InnerText = String.Format("{0:#,###,###,###,##0.00}", gttoday);
                                                                                            else if (node10.FirstChild.InnerText == "txtgrandtotalyesterday")
                                                                                                node10.FirstChild.InnerText = String.Format("{0:#,###,###,###,##0.00}", gtyesterday);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //RMB 11.10.2014 added end
                }
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            else
                return String.Empty;
        }

        static string GetXmlStringPOS(string strFile, string sQry, string sQry1, string sQry2, string sQry3, string sQry4,
            string sConnString, DateTime _dtStart, clsConfig clscon, string chngefund, string struser)
        {
            clscommon clscom = new clscommon();
            double gttoday = 0;
            double gtyesterday = 0;
            DataTable m_dtconfig = new DataTable();
            string companyname = String.Empty;
            string companyaddress = String.Empty;
            string tin = String.Empty;
            string accreditation = String.Empty;
            string permitno = String.Empty;
            string serialno = String.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(strFile))
            {
                try
                {
                    xmlDoc.Load(strFile);
                }
                catch (XmlException e)
                {
                    Console.WriteLine(e.Message);
                }

                //XmlDocument doc = new XmlDocument();
                //doc.Load(strFile);
                /*XmlElement root = xmlDoc.DocumentElement;
                MessageBox.Show((root.GetElementsByTagName("DataSet")).Count.ToString());
                MessageBox.Show((root.GetElementsByTagName("DataSet/@Name")).Count.ToString());
                MessageBox.Show((root.GetElementsByTagName("DataSet[@Name='Data']")).Count.ToString());
                MessageBox.Show((root.SelectNodes("//DataSet")).Count.ToString());
                MessageBox.Show((root.SelectNodes("//DataSet[@Name='Data']")).Count.ToString());
                XmlNodeList elemList = root.GetElementsByTagName("DataSet");
                for (int i = 0; i < elemList.Count; i++)
                {
                    MessageBox.Show(elemList[i].InnerXml);
                }*/


                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                //nsmgr.AddNamespace("Name", "samples");
                //var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
                // MessageBox.Show((xmlDoc.SelectNodes("//a:Report/a:DataSets/a:DataSet/@Name", nsmgr).Count.ToString()));
                //MessageBox.Show((xmlDoc.SelectNodes("//a:Report/a:DataSets/a:DataSet/@Name='Data']", nsmgr).Count.ToString()));
                //XmlNodeList nodes = xmlDoc.SelectNodes("//a:Report/a:DataSets/a:DataSet/@Name", nsmgr);
                XmlNodeList nodes = xmlDoc.SelectNodes("//a:Report/a:DataSets/a:DataSet/a:Query/a:CommandText", nsmgr);
                int cntr = 0;
                foreach (XmlNode query in nodes)
                {
                    //MessageBox.Show(isbn.InnerText);
                    switch (cntr)
                    {
                        case 0:
                            query.InnerText = sQry;
                            break;
                        case 1:
                            query.InnerText = sQry1;
                            break;
                        case 2:
                            query.InnerText = sQry2;
                            break;
                        case 3:
                            query.InnerText = sQry3;
                            break;
                        case 4:
                            query.InnerText = sQry4;
                            break;
                    }
                    cntr += 1;
                }

                XmlNodeList nodes1 = xmlDoc.SelectNodes("//a:Report/a:DataSources/a:DataSource/a:ConnectionProperties/a:ConnectString", nsmgr);
                foreach (XmlNode cnnstring in nodes1)
                {
                    cnnstring.InnerText = sConnString;
                }

                XmlNodeList nodes2 = xmlDoc.SelectNodes("//a:Report/a:Body/a:ReportItems/a:List/a:ReportItems/a:Textbox[@Name='txtchangefund']/a:Value", nsmgr);
                foreach (XmlNode str in nodes2)
                { 
                    str.InnerText = chngefund.ToString();
                }

                XmlNodeList nodes3 = xmlDoc.SelectNodes("//a:Report/a:Body/a:ReportItems/a:List/a:ReportItems/a:Textbox[@Name='txtuserid']/a:Value", nsmgr);
                foreach (XmlNode str in nodes3)
                {
                    str.InnerText = struser;
                }

                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            else
                return String.Empty;
        }


        private void cmbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sView = cmbZoom.Text;
            switch (sView)
            {
                case "Actual Size":
                    rdlViewer1.Zoom = 1;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "Fit Page":
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitPage;
                    break;
                case "Fit Width":
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
                    break;
                case "800%":
                    rdlViewer1.Zoom = 8;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "400%":
                    rdlViewer1.Zoom = 4;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "200%":
                    rdlViewer1.Zoom = 2;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "150%":
                    rdlViewer1.Zoom = 1.5f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "125%":
                    rdlViewer1.Zoom = 1.25f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "100%":
                    rdlViewer1.Zoom = 1;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "75%":
                    rdlViewer1.Zoom = 0.75f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "50%":
                    rdlViewer1.Zoom = 0.5f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
                case "25%":
                    rdlViewer1.Zoom = 0.25f;
                    rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
                    break;
            }
            rdlViewer1.Update();
        }

        private void btnSaveas_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter =
                "PDF files (*.pdf)|*.pdf|" +
                "XML files (*.xml)|*.xml|" +
                "HTML files (*.html)|*.html|" +
                "CSV files (*.csv)|*.csv|" +
                "RTF files (*.rtf)|*.rtf|" +
                "TIF files (*.tif)|*.tif|" +
                "XLS files (*.xls)|*.xls|" +
                "Excel files (*.xlsx)|*.xlsx|" +
                "MHT files (*.mht)|*.mht";
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = Application.ExecutablePath;
            sfd.FileName = expFileNm;

            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;
            //m_frmM.m_loadpath = Path.GetDirectoryName(sfd.FileName);
            // save the report in a rendered format 
            string ext = null;
            int i = sfd.FileName.LastIndexOf('.');
            if (i < 1)
                ext = "";
            else
                ext = sfd.FileName.Substring(i + 1).ToLower();

            try
            {
                switch (ext)
                {
                    case "pdf":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.PDF);
                        break;
                    case "xml":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.XML);
                        break;
                    case "html":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.HTML);
                        break;
                    case "htm":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.HTML);
                        break;
                    case "csv":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.CSV);
                        break;
                    case "rtf":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.RTF);
                        break;
                    case "mht":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.MHTML);
                        break;
                    case "mhtml":
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.MHTML);
                        break;
                    case "xls":
                        rdlViewer1.SourceRdl = xmlfile;
                        rdlViewer1.Rebuild();
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.Excel);
                        break;
                    case "xlsx":
                        rdlViewer1.SourceRdl = xmlfile;
                        rdlViewer1.Rebuild();
                        rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.Excel);
                        break;
                    case "tif":
                        DialogResult dr = MessageBox.Show(this, "Do you want to save colors in TIF file?", "Export", MessageBoxButtons.YesNoCancel);
                        if (dr == DialogResult.No)
                            rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.TIFBW);
                        else if (dr == System.Windows.Forms.DialogResult.Yes)
                            rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.TIF);
                        break;
                    case "tiff":
                        DialogResult dr1 = MessageBox.Show(this, "Do you want to save colors in TIF file?", "Export", MessageBoxButtons.YesNoCancel);
                        if (dr1 == DialogResult.No)
                            rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.TIFBW);
                        else if (dr1 == System.Windows.Forms.DialogResult.Yes)
                            rdlViewer1.SaveAs(sfd.FileName, fyiReporting.RDL.OutputPresentationType.TIF);
                        break;
                    default:
                        MessageBox.Show(this,
                            string.Format("{0} is not a valid file type.  File extension must be PDF, XML, HTML, CSV, MHT, RTF, TIF, XLSX.", sfd.FileName), "Save As Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    ex.Message, "Save As Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = rdlViewer1.SourceFile == null ? "untitled" : rdlViewer1.SourceFile.ToString();//edited 3.14.2016
            pd.PrinterSettings.FromPage = 1;
            pd.PrinterSettings.ToPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MaximumPage = rdlViewer1.PageCount;
            pd.PrinterSettings.MinimumPage = 1;
            if (rdlViewer1.PageWidth > rdlViewer1.PageHeight)
                pd.DefaultPageSettings.Landscape = true;
            else
                pd.DefaultPageSettings.Landscape = false;

            PrintDialog dlg = new PrintDialog();
            dlg.Document = pd;
            dlg.AllowSelection = true;
            dlg.AllowSomePages = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
                    {
                        pd.PrinterSettings.FromPage = rdlViewer1.PageCurrent;
                    }
                    rdlViewer1.Print(pd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Print error: " + ex.Message);
                }
            }
        }




        //melvin 10-24-2014
        private void frmReport_Resize(object sender, EventArgs e)
        {
                rdlViewer1.Width = this.Width - 100;
                rdlViewer1.Height = this.Height - 100;
                rdlViewer1.Left = 50;
                rdlViewer1.Top = 70;
        }

        private void rdlViewer1_Click(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


        //public DataTable GetResult()
        //{

        //}

        //public void PreviewReport()
        //{
        //    int intRowCount = 0;
        //    int intQuantity = 0;
        //    int intTotalQuantity = 0;
        //    double dblSales = 0.0;
        //    double dblTotalSales = 0.0;

        //    using (MySqlConnection connection = new MySqlConnection(CommonLibrary.CommonUtility.ConnectionString))
        //    {
        //        using (MySqlCommand command = new MySqlCommand("reports_summary_daily_cinema_sales", connection))
        //        {
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            connection.Open();

        //            command.Parameters.AddWithValue("?_start_date", this.StartDate);
        //            MySqlDataReader reader = command.ExecuteReader();

        //            Hashtable hshParameters = new Hashtable();

        //            //get elements
        //            if (reader.Read())
        //            {
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    hshParameters.Add(reader.GetName(i), reader.GetString(i));
        //                }
        //            }

        //            intRowCount = 6;

        //            if (reader.NextResult())
        //            {
        //                while (reader.Read())
        //                {
        //                    intRowCount++;

        //                    intQuantity = reader.GetInt32(4);
        //                    intTotalQuantity += intQuantity;
        //                    dblSales = reader.GetDouble(5);
        //                    dblTotalSales += dblSales;
        //                }
        //            }
        //        }
        //    }


        //    intRowCount++;
        //    if (intRowCount == 7)
        //        throw new Exception("No records found.");

        //    try
        //    {
        //        //System.Diagnostics.Process.Start();
        //    }
        //    catch (Exception) { }
        //}
    }
}
