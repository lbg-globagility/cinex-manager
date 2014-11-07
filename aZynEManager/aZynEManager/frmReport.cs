using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
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
        string xmlfile = String.Empty;
        string expFileNm = String.Empty;
        public int _intCinemaID = -1;
        public DateTime _dtStart = new DateTime();
        public DateTime _dtEnd = new DateTime();
       
        public string _strDistributor = String.Empty;
        public static ArrayList queryList = new ArrayList();
        //melvin 2014 10-17
        public string strCinema = null;
        public string rp01Account = null;
        public string rp05distributor = null;

        public frmReport()
        {
            InitializeComponent();
        }


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
        private void frmReport_Load(object sender, EventArgs e)
        {
        }

        public void frmInit(frmMain frm, clscommon cls, string reportcode)
        {
            m_frmM = frm;
            m_clscom = cls;
            
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
                        sqry.Append("SELECT f.name, e.title, COUNT(cinema_seat_id) quantity, SUM(price) sales, g.system_value, h.name report_name, d.movie_date ");
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
                        sqry.Append("select @patron:= a.name as PATRON, @cinema:= c.name ");
                        sqry.Append("CINEMA, b.price PRICE, @c:= (select count(f.cinema_seat_id) ");
                        sqry.Append("from patrons a inner join cinema_patron b on a.id=b.patron_id ");
                        sqry.Append("inner join  cinema c on b.cinema_id = c.id ");
                        sqry.Append("inner join movies_schedule d on d.cinema_id = c.id ");
                        sqry.Append("inner join movies_schedule_list e on ");
                        sqry.Append("e.movies_schedule_id = d.id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id where d.movie_date=");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtStart));
                        sqry.Append(" and a.name=@patron ");
                        sqry.Append("and c.name = @cinema) as QTY, (@c*b.price) as ");
                        sqry.Append("`TOTALSALES` from patrons a inner join cinema_patron ");
                        sqry.Append("b on a.id=b.patron_id inner join  cinema c on ");
                        sqry.Append("b.cinema_id = c.id inner join movies_schedule d ");
                        sqry.Append("on d.cinema_id = c.id inner join movies_schedule_list ");
                        sqry.Append("e on e.movies_schedule_id = d.id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id inner join ");
                        sqry.Append("ticket g on e.id= g.movies_schedule_list_id ");
                        sqry.Append("where d.movie_date=");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtStart));
                        sqry.Append(String.Format(" and g.terminal='{0}' ",rp01Account));
                        sqry.Append(" group by a.name, c.name order by c.name;");
                        break;
                    case "RP02":
                        sqry.Append("SELECT e.title, e.no_of_days, ");
                        sqry.Append("f.no_of_screenings, total_seats_taken, ");
                        sqry.Append("total_available_seats,  total_ticket_sales,");
                        sqry.Append("(total_seats_taken/total_available_seats) * 100 util ");
                        sqry.Append("FROM ( SELECT a.id, code, title, rating_id, COUNT");
                        sqry.Append("(movie_date) no_of_days FROM movies a, movies_schedule b ");
                        sqry.Append("WHERE a.id = b.movie_id  AND b.movie_date BETWEEN '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' AND '" );
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) +"' GROUP BY a.id ) e, ");
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
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) +"' GROUP BY movie_id ) ");
                        sqry.Append("g,(SELECT movie_id, SUM(available_seats) total_available_seats ");
                        sqry.Append("FROM (SELECT movie_id, n.capacity * COUNT(l.id) ");
                        sqry.Append("  available_seats FROM movies_schedule_list l, ");
                        sqry.Append("movies_schedule m, cinema n WHERE l.movies_schedule_id");
                        sqry.Append("=m.id AND m.cinema_id = n.id AND m.movie_date BETWEEN  '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' AND '");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtEnd) + "' GROUP BY movie_id, ");
                        sqry.Append("cinema_id) o GROUP BY movie_id) k WHERE e.id = f.movie_id AND ");
                        sqry.Append("e.id = g.movie_id AND e.id = k.movie_id;");
                        break;
                    case "RP05":
                        sqry.Append("select a.code as movie_code, a.title, ");
                        sqry.Append("count(e.cinema_seat_id) as QTY, ");
                        sqry.Append("(e.price* count(e.cinema_seat_id)) as Sales, ");
                        sqry.Append("b.movie_date, c.code as dist_code from movies a ");
                        sqry.Append("inner join movies_schedule b on a.id= b.movie_id ");
                        sqry.Append("inner join distributor c on a.dist_id= c.id ");
                        sqry.Append("inner join movies_schedule_list d on b.id= ");
                        sqry.Append("d.movies_schedule_id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat e ");
                        sqry.Append("on d.id = e.movies_schedule_list_id where b.movie_date = ");
                        sqry.Append(string.Format("'{0:yyyy/MM/dd}' ",_dtStart));
                        sqry.Append(" and c.code='"+rp05distributor+"' and  e.status=1 ");
                        sqry.Append("GROUP BY a.code;");
                        break;
                    case "RP06":
                        
                        sqry.Append("SELECT MIN( CONCAT(a.title,'\\n', ");
                        sqry.Append("DATE_FORMAT(d.start_time,'%h:%i %p'), \" id:\",cast(d.id as char(5))) )");
                        sqry.Append(" col1, CONCAT(a.title,'\\n', DATE_FORMAT((select MIN(d.start_time) ");
                        sqry.Append("from movies a, movies_schedule b, ");
                        sqry.Append("cinema c, movies_schedule_list d  ");
                        sqry.Append("where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("  d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and " + "c.name='");
                        sqry.Append(strCinema + "' and  start_time > (SELECT MIN(d.start_time) ");
                        sqry.Append("FROM movies a, movies_schedule b,");
                        sqry.Append("cinema c, movies_schedule_list d where a.id=b.movie_id and ");
                        sqry.Append("b.cinema_id = c.id and d.movies_schedule_id = b.id and ");
                        sqry.Append("b.movie_date ='"+String.Format("{0:yyyy/MM/dd}", _dtStart) +"' ");
                        sqry.Append("and c.name='"+strCinema+"')),'%h:%i %p'), \" id:\", ");
                        sqry.Append("cast((SELECT MIN(d.id) FROM movies a, movies_schedule b, cinema c,");
                        sqry.Append("movies_schedule_list d where a.id=b.movie_id and b.cinema_id = c.id ");
                        sqry.Append("and d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "' and d.start_time> ");
                        sqry.Append("(select Min(d.start_time) FROM movies a, movies_schedule b,");
                        sqry.Append("cinema c, movies_schedule_list d where a.id=b.movie_id and ");
                        sqry.Append("b.cinema_id = c.id and d.movies_schedule_id = b.id and ");
                        sqry.Append("b.movie_date ='" + String.Format("{0:yyyy/MM/dd}", _dtStart));
                        sqry.Append("' and c.name='" + strCinema + "'))as char(5)))  col2,");
                        sqry.Append(" CONCAT(a.title,'\\n', DATE_FORMAT(");
                        sqry.Append("(select Max(d.start_time) from movies a, ");
                        sqry.Append("movies_schedule b, cinema c, movies_schedule_list d ");
                        sqry.Append("where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "' and start_time < ");
                        sqry.Append("(SELECT Max(d.start_time) FROM movies a, movies_schedule b, ");
                        sqry.Append("cinema c, movies_schedule_list d where a.id=b.movie_id and ");
                        sqry.Append("b.cinema_id = c.id and  d.movies_schedule_id = b.id and ");
                        sqry.Append("b.movie_date ='" + String.Format("{0:yyyy/MM/dd}", _dtStart));
                        sqry.Append("' and c.name='" + strCinema + "') ),'%h:%i %p'), \" id:\",");
                        sqry.Append("cast((SELECT MAX(d.id) FROM movies a, movies_schedule b, cinema c,");
                        sqry.Append("movies_schedule_list d where a.id=b.movie_id and b.cinema_id = c.id ");
                        sqry.Append("and d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "' and d.start_time< (select Max(d.start_time)");
                        sqry.Append("FROM movies a, movies_schedule b, cinema c, movies_schedule_list d ");
                        sqry.Append("where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "'))as char(5)))  col3, ");
                        sqry.Append(" MAX( CONCAT(a.title,'\\n', ");
                        sqry.Append("DATE_FORMAT(d.start_time,'%h:%i %p'),\" id:\",");
                        sqry.Append("cast(d.id as char(5))) ) col4 ");
                        sqry.Append(" FROM movies a, movies_schedule b, cinema c, movies_schedule_list d ");
                        sqry.Append(" where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "';");

                        MySqlConnection myconn = new MySqlConnection();
                        myconn.ConnectionString = m_frmM._connection;

                        if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                        queryList.Add(reader["col1"]);
                        queryList.Add(reader["col2"]);
                        queryList.Add(reader["col4"]);
                        queryList.Add(reader["col3"]);
                        }
                        sqry = new StringBuilder();
                        sqry.Append("SELECT MIN( CONCAT(a.title,'\\n', ");
                        sqry.Append("DATE_FORMAT(d.start_time,'%h:%i %p')))");
                        sqry.Append(" col1, CONCAT(a.title,'\\n', DATE_FORMAT((select MIN(d.start_time) ");
                        sqry.Append("from movies a, movies_schedule b, ");
                        sqry.Append("cinema c, movies_schedule_list d  ");
                        sqry.Append("where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("  d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and " + "c.name='");
                        sqry.Append(strCinema + "' and  start_time > (SELECT MIN(d.start_time) ");
                        sqry.Append("FROM movies a, movies_schedule b,");
                        sqry.Append("cinema c, movies_schedule_list d where a.id=b.movie_id and ");
                        sqry.Append("b.cinema_id = c.id and d.movies_schedule_id = b.id and ");
                        sqry.Append("b.movie_date ='"+String.Format("{0:yyyy/MM/dd}", _dtStart) +"' ");
                        sqry.Append("and c.name='"+strCinema+"')),'%h:%i %p'))  col2,");//colum2
                        sqry.Append(" CONCAT(a.title,'\\n', DATE_FORMAT(");
                        sqry.Append("(select Max(d.start_time) from movies a, ");
                        sqry.Append("movies_schedule b, cinema c, movies_schedule_list d ");
                        sqry.Append("where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "' and start_time < ");
                        sqry.Append("(SELECT Max(d.start_time) FROM movies a, movies_schedule b, ");
                        sqry.Append("cinema c, movies_schedule_list d where a.id=b.movie_id and ");
                        sqry.Append("b.cinema_id = c.id and  d.movies_schedule_id = b.id and ");
                        sqry.Append("b.movie_date ='" + String.Format("{0:yyyy/MM/dd}", _dtStart));
                        sqry.Append("' and c.name='" + strCinema + "') ),'%h:%i %p'))  col3, ");
                        sqry.Append(" MAX( CONCAT(a.title,'\\n', ");
                        sqry.Append("DATE_FORMAT(d.start_time,'%h:%i %p'))) col4 ");
                        sqry.Append(" FROM movies a, movies_schedule b, cinema c, movies_schedule_list d ");
                        sqry.Append(" where a.id=b.movie_id and b.cinema_id = c.id and ");
                        sqry.Append("d.movies_schedule_id = b.id and b.movie_date ='");
                        sqry.Append(String.Format("{0:yyyy/MM/dd}", _dtStart) + "' and ");
                        sqry.Append("c.name='" + strCinema + "';");
                        break;
                    case "RP12":
                        //2014/10/31
                        sqry.Append("select a.code, @patron:= a.name as PATRON, ");
                        sqry.Append("@cinema:= c.name CINEMA, b.price PRICE, @c:= ");
                        sqry.Append("(select count(f.cinema_seat_id) from patrons a ");
                        sqry.Append("inner join cinema_patron b on a.id=b.patron_id ");
                        sqry.Append("inner join  cinema c on b.cinema_id = c.id ");
                        sqry.Append("inner join movies_schedule d on d.cinema_id = c.id ");
                        sqry.Append("inner join movies_schedule_list e on ");
                        sqry.Append("e.movies_schedule_id = d.id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id where ");
                        sqry.Append(String.Format("d.movie_date='{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append(" and a.name=@patron and c.name = @cinema)");
                        sqry.Append(" as QTY, (@c*b.price) as `TOTALSALES`,");
                        sqry.Append(" d.movie_date , h.start_date, h.title, ");
                        sqry.Append("i.userid, I.fname, i.lname, i.mname ");
                        sqry.Append("from patrons a inner join cinema_patron ");
                        sqry.Append("b on a.id=b.patron_id inner join  cinema c on ");
                        sqry.Append("b.cinema_id = c.id inner join movies_schedule d ");
                        sqry.Append("on d.cinema_id = c.id inner join movies_schedule_list ");
                        sqry.Append("e on e.movies_schedule_id = d.id inner join ");
                        sqry.Append("movies_schedule_list_reserved_seat f on ");
                        sqry.Append("f.movies_schedule_list_id= e.id inner join ");
                        sqry.Append("ticket g on e.id= g.movies_schedule_list_id ");
                        sqry.Append("inner join movies h on h.id= d.movie_id ");
                        sqry.Append("inner join users i on i.id= g.user_id where ");
                        sqry.Append(String.Format("d.movie_date='{0:yyyy/MM/dd}' ", _dtStart));
                        sqry.Append("group by a.name, c.name order by c.name;");
                        break;
                    case "AUDIT":
                        sqry.Append("select a.id, a.tr_date, b.userid,");
                        sqry.Append("a.tr_details, c.module_desc, c.module_group,");
                        sqry.Append("a.computer_name, concat(b.fname , ' ',b.lname)");
                        sqry.Append("username from a_trail a left join users b ");
                        sqry.Append("on a.user_id = b.id left join system_module c ");
                        sqry.Append("on trim(a.module_code) = trim(c.id) where tr_date between ");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtStart) + " and ");
                        sqry.Append(String.Format("'{0:yyyy/MM/dd}'", _dtEnd));
                        sqry.Append(" order by tr_date desc");
                        //MessageBox.Show(_dtStart.ToShortDateString() + "to:" + _dtEnd.ToShortDateString());
                        break;
                }

                xmlfile = GetXmlString(Path.GetDirectoryName(Application.ExecutablePath) + @"\reports\" + reportcode + ".xml", sqry.ToString(), m_frmM._odbcconnection, _intCinemaID.ToString(), reportcode, _dtStart, _dtEnd,rp01Account);
               // MessageBox.Show(sqry.ToString());
                rdlViewer1.SourceRdl = xmlfile;
                rdlViewer1.Rebuild();
               // MessageBox.Show(xmlfile.ToString());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return;
            }
        }

        static string GetXmlString(string strFile, string sQry, string sConnString, string cine, string code, DateTime _dtStart, DateTime _dtEnd,string rp01)
        {
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
                            if (code == "RP06")
                            {
                                if (flag == 0)
                                {
                                    sQry = "Select name as cinema_name from "+  
                                        "cinema where id=" + cine + " limit 1;";
                                }
                                else if(flag==1)
                                {
                                    string[] result = new string[]{};
                                    ArrayList id = new ArrayList();
                                    string[] stringSeparators = new string[] { "id:" };
                                    for (int x = 0; x < queryList.Count; x++)
                                    {
                                        result = queryList[x].ToString().Split(
                                            stringSeparators, 
                                            StringSplitOptions.None
                                            );
                                        id.Add(result[1]);
                                        result = null;
                                    }
                                sQry = "SELECT  @myVal := g.code as patron_code, g.name, a.price, " +
                                "@myQty1:=(select count(a.cinema_seat_id) as count " +
                                "FROM movies_schedule_list_reserved_seat a, " +
                                "movies_schedule_list_patron b," +
                                "movies_schedule_list c, patrons g WHERE "+
                                "a.movies_schedule_list_id " +
                                "= c.id and  a.patron_id = b.id and b.patron_id=g.id and " +
                                "c.id=" + id[0].ToString() + " and g.code = @myVal) as qty1, " +
                                "(@myQty1*a.price) as Sales1, " +
                                "@myQty2:=(select count(a.cinema_seat_id) as count " +
                                "FROM movies_schedule_list_reserved_seat a, " +
                                "movies_schedule_list_patron b,movies_schedule_list c, patrons " +
                                "g WHERE a.movies_schedule_list_id = c.id and  a.patron_id = b.id " +
                                "and b.patron_id=g.id and c.id=" + id[1].ToString() +
                                " and g.code = @myVal) as qty2, (@myQty2*a.price) as Sales2," +
                                " @myQty3:=(select count(a.cinema_seat_id) as count " +
                                "FROM movies_schedule_list_reserved_seat a, " +
                                "movies_schedule_list_patron b, movies_schedule_list c, patrons g " +
                                "WHERE a.movies_schedule_list_id = c.id and  a.patron_id = b.id " +
                                "and b.patron_id=g.id and c.id=" + id[2].ToString() +
                                " and g.code = @myVal) as qty3, (@myQty3*a.price) as Sales3, " +
                                "@myQty4:=(select count(a.cinema_seat_id) as count " +
                                "FROM movies_schedule_list_reserved_seat a, " +
                                "movies_schedule_list_patron b," +
                                "movies_schedule_list c, patrons g WHERE " +
                                "a.movies_schedule_list_id = c.id and  a.patron_id = b.id and " +
                                "b.patron_id=g.id and c.id=" + id[3].ToString() +
                                " and g.code = @myVal) as qty4,(@myQty4*a.price) as Sales4," +
                                "count(a.cinema_seat_id) as `TotalQuality`," +
                                "(count(a.cinema_seat_id)* a.price) as `TotalPrice`" +
                                "FROM movies_schedule_list_reserved_seat a, ticket b, " +
                                "movies_schedule_list c, movies_schedule d, movies e, cinema f, " +
                                "patrons g, movies_schedule_list_patron h " +
                                "WHERE a.ticket_id = b.id and g.id= h.patron_id and a.patron_id " +
                                "= h.id and a.status = 1 AND a.movies_schedule_list_id = c.id AND " +
                                "c.movies_schedule_id = d.id AND d.movie_date = '2006/12/07' " +
                                " AND d.movie_id = e.id AND d.cinema_id = f.id and " +
                                "f.name='Cinema 1' GROUP BY g.code;";
                                 }
                            }
                            else if (code == "RP02")
                            {
                                sQry = "SELECT Sum(e.no_of_days) as`days`," +
                               "sum( f.no_of_screenings) as `screening`, " +
                               "sum(total_seats_taken)as `seat_taken`, " +
                               "sum(total_available_seats) `available_seat`," +
                               "sum( total_ticket_sales)as `ticket_sales`," +
                               "sum((total_seats_taken/total_available_seats) * 100) `util`" +
                               "FROM (SELECT a.id, code, title, rating_id, " +
                               "COUNT(movie_date) no_of_days FROM movies a, movies_schedule " +
                               "b WHERE a.id = b.movie_id AND b.movie_date BETWEEN " +
                               "'" + String.Format("{0:yyyy/MM/dd}", _dtStart) +"' AND '"+
                                    String.Format("{0:yyyy/MM/dd}", _dtEnd) + 
                                    "' GROUP BY a.id ) e," +
                               "(SELECT movie_id, COUNT(c.id) no_of_screenings FROM " +
                               "movies_schedule_list c, movies_schedule d WHERE " +
                               "c.movies_schedule_id =d.id AND d.movie_date BETWEEN " +
                               "'"+ String.Format("{0:yyyy/MM/dd}", _dtStart) +"' AND '"+
                                    String.Format("{0:yyyy/MM/dd}", _dtEnd) + 
                                    "' GROUP BY movie_id ) f," +
                               "(SELECT j.movie_id, COUNT(h.cinema_seat_id) " +
                               "total_seats_taken, SUM(h.price) total_ticket_sales FROM " +
                               "movies_schedule_list_reserved_seat h, movies_schedule_list " +
                               "i, movies_schedule j WHERE h.movies_schedule_list_id = i.id" +
                               " AND i.movies_schedule_id = j.id AND j.movie_date BETWEEN  " +
                               "'"+  String.Format("{0:yyyy/MM/dd}", _dtStart) +"' AND '"+
                                    String.Format("{0:yyyy/MM/dd}", _dtEnd) + 
                                    "'   GROUP BY movie_id) g," +
                               "(SELECT movie_id, SUM(available_seats) total_available_seats" +
                               " FROM (SELECT movie_id, n.capacity * COUNT(l.id) " +
                               "available_seats FROM movies_schedule_list l, movies_schedule" +
                               " m, cinema n WHERE l.movies_schedule_id =m.id AND " +
                               "m.cinema_id = n.id AND m.movie_date BETWEEN  '"+
                                String.Format("{0:yyyy/MM/dd}", _dtStart) +"' AND '"+
                                String.Format("{0:yyyy/MM/dd}", _dtEnd) +
                                "' GROUP BY movie_id, cinema_id) o GROUP BY " +
                               "movie_id) k WHERE e.id = f.movie_id AND e.id = g.movie_id" +
                               " AND e.id = k.movie_id;";
                            }

                            else if (code == "AUDIT")
                            {
                                sQry = " Select '" + _dtStart.ToShortDateString() + "' as date_from, '" + _dtEnd.AddDays(-1).ToShortDateString() + "' as date_to;";
                            }
                            else if (code == "RP01")
                            {
                                if (flag == 0)
                                {
                                    sQry = String.Format("select '{0:yyyy/MM/dd}' as date_sel", _dtStart);
                                }
                                else if (flag == 1)
                                {
                                    sQry = "select userid, concat(fname,' ',lname) as name " +
                                        "from users where userid='"+rp01+"'";
                                }
                                else if (flag == 2)
                                {
                                    sQry = "select sum(a.a) as total from (select @patron:= " +
                                        "a.name as PATRON, @cinema:= c.name CINEMA, b.price PRICE," +
                                        "@c:= (select count(f.cinema_seat_id)" +
                                        "from patrons a inner join cinema_patron b on " +
                                        "a.id=b.patron_id inner join  cinema c on b.cinema_id " +
                                        "= c.id inner join movies_schedule d on d.cinema_id = " +
                                        "c.id inner join movies_schedule_list e on " +
                                        "e.movies_schedule_id = d.id inner join " +
                                        "movies_schedule_list_reserved_seat f on " +
                                        "f.movies_schedule_list_id= e.id " +
                                        String.Format(" where d.movie_date='{0:yyyy/MM/dd}'", _dtStart) +
                                        " and a.name=@patron and c.name = @cinema) as QTY," +
                                        "(@c*b.price) as a from patrons a inner join " +
                                        "cinema_patron b on a.id=b.patron_id " +
                                        "inner join  cinema c on b.cinema_id = c.id " +
                                        "inner join movies_schedule d on d.cinema_id = c.id " +
                                        "inner join movies_schedule_list e on e.movies_schedule_id = " +
                                        "d.id inner join movies_schedule_list_reserved_seat f on " +
                                        "f.movies_schedule_list_id= e.id " +
                                        "inner join ticket g on e.id= g.movies_schedule_list_id " +
                                        String.Format("where d.movie_date='{0:yyyy/MM/dd}'", _dtStart) +
                                        "and g.terminal='" + rp01 + "' group by a.name, c.name " +
                                        "order by c.name) a;";
                                }
                                else if (flag == 3)
                                {
                                    sQry = " select count(f.cinema_seat_id) as totalQTY " +
                                        "from patrons a inner join cinema_patron b on " +
                                        "a.id=b.patron_id inner join  cinema c on " +
                                        "b.cinema_id = c.id inner join movies_schedule d" +
                                        "on d.cinema_id = c.id inner join movies_schedule_list " +
                                        "e on e.movies_schedule_id = d.id inner join " +
                                        "movies_schedule_list_reserved_seat f on " +
                                        "f.movies_schedule_list_id= e.id where d.movie_date=" +
                                     String.Format("'{0:yyyy/MM/dd}';", _dtStart);
                                }
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
            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|" +
                "XML files (*.xml)|*.xml|" +
                "HTML files (*.html)|*.html|" +
                "CSV files (*.csv)|*.csv|" +
                "RTF files (*.rtf)|*.rtf|" +
                "TIF files (*.tif)|*.tif|" +
                "MHT files (*.mht)|*.mht";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.InitialDirectory = Application.ExecutablePath;
            saveFileDialog1.FileName = (String)"*.pdf";
            String ext = (String)"";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int i = saveFileDialog1.FileName.LastIndexOf(".");
                if (i < 1)
                {
                    MessageBox.Show("Please specify what file type to be used.", this.Text);
                    return;
                }
                else
                    ext = saveFileDialog1.FileName.Substring(i + 1).ToLower();
                rdlViewer1.SaveAs(saveFileDialog1.FileName, ext);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = rdlViewer1.SourceFile == null ? "untitled" : rdlViewer1.SourceFile;
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
