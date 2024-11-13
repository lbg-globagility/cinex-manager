using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data.Odbc;
using System.Collections;

namespace aZynEManager
{
    public class clscommon
    {
        public clsConfig m_clscon = new clsConfig();
        public double _gttoday = 0;
        public double _gtyesterday = 0;
        public frmMain _frmM = null;//RMB 11.12.2014 added new variable
        public bool _withculturaltax = false;

        public DataTable setDataTable(string sqry, string conn)
        {
            DataTable dt = new DataTable();
       
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = conn;
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                if (myconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sqry, myconn);
                    
                    da.Fill(dt);
                    myconn.Close();
                }
            

            return dt;
        }

        public DataSet getDataSet(string strqry, string conn)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = conn;
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();

                if (myconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(strqry, myconn);
                    da.Fill(ds);
                    myconn.Close();
                }
            }
            catch
            {
            }
            return ds;
        }

        public bool verifyUserRights(int intUserID, string sModuleCode, string sConnString)
        {
            try
            {
                bool ans = false;
                int intCnt = 0;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select count(*) from user_rights a, system_module b where ");
                sQuery.Append(String.Format("a.user_id = {0} and a.module_id = b.id ", intUserID));
                sQuery.Append(String.Format("and b.module_code = '{0}'", sModuleCode));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                intCnt = Convert.ToInt32(cmd.ExecuteScalar());
                if (intCnt > 0)
                    ans = true;
                cmd.Dispose();
                if(myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return ans;
            }
            catch
            {
                return false;
            }
        }

        public int getModuleID(string sModuleCode, string sConnString)
        {
            int intID = -1;
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select a.id from system_module a where ");
                sQuery.Append(String.Format("a.module_code = '{0}'", sModuleCode));
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        intID = Convert.ToInt32(rd[0].ToString());
                    }
                }
                cmd.Dispose();
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return intID;
            }
            catch
            {
                return intID;
            }
        }

        /// <summary>
        /// audit trail for the a_trail table
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModule"></param>
        /// <param name="sAffTable"></param>
        /// <param name="sComputer"></param>
        /// <param name="sDetails"></param>
        /// <param name="sConnString"></param>
        public void AddATrail(int intUserID, String sModule, String sAffTable, String sComputer, String sDetails, String sConnString)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            MySqlConnection myconn = new MySqlConnection(sConnString);
            try
            {
                StringBuilder sqry = new StringBuilder();
                sqry.Append("insert into a_trail values(0,");
                sqry.Append(String.Format("{0},", intUserID));
                sqry.Append(String.Format("'{0}',", String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now)));
                sqry.Append(String.Format("{0},", getModuleID(sModule, sConnString)));
                sqry.Append(String.Format("'{0}',", sAffTable));
                sqry.Append(String.Format("'{0}',", sComputer));
                sqry.Append(String.Format("'{0}'", sDetails));
                sqry.Append(")");
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show(err.Message, " AUDIT TRAIL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        public Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        public static int Get0BGR(Color rgbColor)
        {
            // Return a zero-alpha 24-bit BGR color integer
            return (0 << 24) + (rgbColor.B << 16) + (rgbColor.G << 8) + rgbColor.R;
        }

        public static Color From0BGR(int bgrColor)
        {
            // Get the color bytes
            var bytes = BitConverter.GetBytes(bgrColor);

            // Return the color from the byte array
            return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
        }

        public void initConfig(frmMain frmM)
        {
            //RMB 11.12.2014 added new variable
            _frmM = frmM;

            ////setup for the system configuration
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select * from config_table ");
            sbqry.Append("order by system_code asc");

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable dt = setDataTable(sbqry.ToString(), frmM._connection);

                if (dt.Rows.Count > 0)
                {
                    //cinema name
                    DataTable newdt = new DataTable();
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '001'");
                    var foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaName = newdt.Rows[0]["system_value"].ToString();
                    }
                    //cinema address company name
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '002'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaAddress = newdt.Rows[0]["system_value"].ToString();
                    }

                    //cinema address
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '016'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaAddress2 = newdt.Rows[0]["system_value"].ToString();
                    }

                    //cinema address
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '019'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaSerialNo = newdt.Rows[0]["system_value"].ToString();
                    }

                    //accreditation no
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '018'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaAccreditationNo = newdt.Rows[0]["system_value"].ToString();
                    }

                    //report subheader 1
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '003'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.ReportSubHeader1 = newdt.Rows[0]["system_value"].ToString();
                    }
                    //report subheader 2
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '004'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.ReportSubHeader2 = newdt.Rows[0]["system_value"].ToString();
                    }
                    //report subheader 3
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '005'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.ReportSubHeader3 = newdt.Rows[0]["system_value"].ToString();
                    }
                    //default movie share
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '006'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        int intshare = 0;
                        newdt = foundRows.CopyToDataTable();
                        int intout = -1;
                        if (int.TryParse(newdt.Rows[0]["system_value"].ToString(), out intout))
                            intshare = intout;

                        m_clscon.MovieDefaultShare = intshare;
                    }
                    //default movie share
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '007'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        DateTime datein = new DateTime();
                        newdt = foundRows.CopyToDataTable();
                        DateTime dateout = new DateTime();
                        if (DateTime.TryParse(newdt.Rows[0]["system_value"].ToString(), out dateout))
                            datein = dateout;

                        m_clscon.MovieListCutOffDate = datein;
                    }
                    //intermissiontime before movie
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '008'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        int inttime = 0;
                        newdt = foundRows.CopyToDataTable();
                        int intout = -1;
                        if (int.TryParse(newdt.Rows[0]["system_value"].ToString(), out intout))
                            inttime = intout;

                        m_clscon.MovieIntermissionTime = inttime;
                    }

                    sbqry.Clear();
                    sbqry.Append("[system_code] = '009'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.TIN = newdt.Rows[0]["system_value"].ToString();
                    }

                    sbqry.Clear();
                    sbqry.Append("[system_code] = '010'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.PN = newdt.Rows[0]["system_value"].ToString();
                    }

                    sbqry.Clear();
                    sbqry.Append("[system_code] = '011'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.Printer = newdt.Rows[0]["system_value"].ToString();
                    }
                    
                    sbqry.Clear();
                    sbqry.Append("[system_code] = '013'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        DateTime datein = new DateTime();
                        newdt = foundRows.CopyToDataTable();
                        DateTime dateout = new DateTime();
                        if (DateTime.TryParse(newdt.Rows[0]["system_value"].ToString(), out dateout))
                            datein = dateout;

                        m_clscon.SystemCollectionStartDate = datein;
                    }
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(err.Message);
            }
        }

        //added new function 11.12.2014
        public double calculateTotalCollection(String sConnString, DateTime dt)
        {
            double gt = 0;
            try
            {
                initConfig(new frmMain());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DateTime startdate = m_clscon.SystemCollectionStartDate;
                StringBuilder sqry = new StringBuilder();
                sqry.Append("SELECT sum(a.price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b ");
                sqry.Append("WHERE a.status = 1 ");
                sqry.Append("AND a.movies_schedule_list_id = b.id ");
                sqry.Append(String.Format("AND b.start_time > '{0:yyyy-MM-dd}' ", m_clscon.SystemCollectionStartDate));
                sqry.Append(String.Format("AND b.start_time < '{0:yyyy-MM-dd}' ", dt.AddDays(1)));
                sqry.Append("AND b.status = 1");

                OdbcConnection conn = new OdbcConnection(sConnString);
                if(conn.State == ConnectionState.Closed)
                    conn.Open();
                using (OdbcCommand cmd = new OdbcCommand(sqry.ToString(), conn))
                {
                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gt = Convert.ToDouble(reader[0].ToString());
                        }
                    }
                }

                if (conn.State == ConnectionState.Open)
                    conn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return gt;
            }
            catch
            {
                return gt;
            }
        }

        //added 11.12.2014 added new table to database (total_gross_coll)
        public void refreshTable(frmMain frm, String tbl, string sConnString)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append(String.Format("select count(*) from {0} where ", tbl));
                sQuery.Append(String.Format("userid = '{0}'", frm.m_usercode));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                int intCnt = Convert.ToInt32(cmd.ExecuteScalar());
                if (intCnt > 0)
                {
                    cmd.Dispose();
                    sQuery = new StringBuilder();
                    sQuery.Append(String.Format("delete from {0} where ", tbl));
                    sQuery.Append(String.Format("userid = '{0}'", frm.m_usercode));
                    cmd = new MySqlCommand(sQuery.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //public void populateOrdinanceSummary(frmMain frm, String tbl, string sConnString, DateTime startdate, DateTime enddate)
        //{
        //    try
        //    {
        //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        //        MySqlConnection myconn = new MySqlConnection(sConnString);
                
        //        StringBuilder sQuery = new StringBuilder();

        //        DateTime dt = startdate;
        //        do
        //        {
        //            sQuery = new StringBuilder();
        //            sQuery.Append("");

        //            dt = dt.AddDays(1);
        //        }
        //        while (startdate > enddate);



        //        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        //    }
        //    catch
        //    {
        //    }
        //}

        public void populateTable(frmMain frm, String tbl, string sConnString, DateTime startdate, DateTime enddate)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                String finalsqry = String.Empty;
                finalsqry = String.Format("INSERT INTO {0} VALUES(0,", tbl);
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if(DateTime.TryParse(rd[0].ToString(),out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                foreach(DateTime recdate in datecoll){//for each date in arraylist
                    finalsqry = String.Empty;
                    finalsqry = String.Format("INSERT INTO {0} VALUES(0,", tbl);
                    finalsqry = finalsqry + String.Format("'{0:yyyy/MM/dd}',", recdate); //insert date

                    StringBuilder sbqry = new StringBuilder();
                    sbqry.Append("SELECT * FROM cinema ORDER BY in_order ASC");
                    DataTable dtable = setDataTable(sbqry.ToString(), sConnString);
                    if (dtable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtable.Rows)
                        {
                            string finalquery2  = String.Empty;
                            int cinemaid = Convert.ToInt32(row["id"].ToString());
                                //finalsqry.Append(String.Format("{0},", cinemaid)); //insert cinema_id

                            //create query for the total_gross per date
                            StringBuilder newqry = new StringBuilder();
                            newqry.Append("SELECT sum(price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c ");
                            newqry.Append("WHERE a.movies_schedule_list_id in (SELECT aa.id FROM movies_schedule_list aa ");
                            newqry.Append(String.Format("WHERE aa.start_time > '{0:yyyy/MM/dd}' ", recdate));
                            newqry.Append(String.Format("AND aa.start_time < '{0:yyyy/MM/dd}') ", recdate.AddDays(1)));
                            newqry.Append("AND a.movies_schedule_list_id = b.id ");
                            newqry.Append("AND b.movies_schedule_id = c.id ");
                            newqry.Append("AND a.status = 1 ");
                            newqry.Append(String.Format("AND c.cinema_id = {0} ",cinemaid));
                            newqry.Append("GROUP BY c.movie_date, c.cinema_id ");
                            newqry.Append("ORDER BY c.movie_date ASC");

                            MySqlCommand cmd1 = new MySqlCommand(newqry.ToString(), myconn);
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlDataReader rd1 = cmd1.ExecuteReader();
                            double dbltotal = 0; // //1.7.2016 changes int to double
                            if (rd1.HasRows)
                            {
                                while (rd1.Read())
                                {
                                    double dblout = 0; //1.7.2016 changes int to double
                                    if (double.TryParse(rd1[0].ToString(), out dblout))
                                        dbltotal = dblout;
                                }
                            }
                            rd1.Dispose();
                            cmd1.Dispose();

                            finalquery2 = String.Format("{0},{1},'{2}')", cinemaid, dbltotal, frm.m_usercode); //insert total//1.7.2016 changes int to double

                            //insert final query
                            MySqlCommand cmd2 = new MySqlCommand(finalsqry + finalquery2, myconn);
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd2.ExecuteNonQuery();
                            cmd2.Dispose();
                            finalquery2 = "";
                        }
                    }
                    else
                        finalsqry = String.Empty;
                }
                

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch{
            }
        }

        double getsurchargeval(string sConnString, DateTime startdate, DateTime enddate, string patronid)
        {
            double ord_val = 0;
            MySqlConnection myconn = new MySqlConnection(sConnString);
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select a.* from ordinance_tbl a, patrons_ordinance b ");
                sQuery.Append(String.Format("where '{0:yyyy/MM/dd}' between a.effective_date and a.end_date ",startdate));
                sQuery.Append(String.Format("and a.id = b.ordinance_id and b.patron_id = {0}", patronid));
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        if (!DBNull.Value.Equals(rd["in_pesovalue"]))
                        {
                            if (rd["in_pesovalue"].ToString() == "1")
                            {
                                double dblout = 0;
                                if (double.TryParse(rd["amount_val"].ToString(), out dblout))
                                    ord_val += dblout;
                            }
                        }
                        
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                Cursor.Current = Cursors.Default;

                return ord_val;
            }
            catch
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                Cursor.Current = Cursors.Default;

                return ord_val;
            }
            
        }

        double ordinancevalue(string sConnString, DateTime startdate, DateTime enddate, string patronid, string movieid, string cinemaid)
        {
            double ord_val = 0;
            MySqlConnection myconn = new MySqlConnection(sConnString);
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if (DateTime.TryParse(rd[0].ToString(), out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                StringBuilder finalsqry = new StringBuilder();
                foreach (DateTime recdate in datecoll)
                {
                    finalsqry = new StringBuilder();
                    finalsqry.Append("select (aa.cnt * IF(g.patron_id IS NULL, 0, IF(g.in_pesovalue, g.amount_val, aa.cnt * g.amount_val))) ordinance_val from ");
                    finalsqry.Append("(select count(d.ticket_id) as cnt, c.patron_id ");
                    finalsqry.Append("from movies_schedule a, movies_schedule_list b, ");
                    finalsqry.Append("movies_schedule_list_patron c, movies_schedule_list_reserved_seat d, cinema h ");// added cinema table 6.26.2015
                    finalsqry.Append(String.Format("where a.movie_date = '{0:yyyy-MM-dd}' ",recdate));
                    finalsqry.Append(String.Format("and a.movie_id = {0} ", movieid));
                    finalsqry.Append("and a.id = b.movies_schedule_id ");
                    finalsqry.Append("and b.id = c.movies_schedule_list_id ");
                    finalsqry.Append("and d.patron_id = c.id ");
                    finalsqry.Append("and d.status = 1 ");
                    finalsqry.Append("and b.status = 1 ");
                    finalsqry.Append("and a.cinema_id = h.id ");// added 6.26.2015
                    finalsqry.Append(String.Format("and a.cinema_id = {0} ", cinemaid)); // added 6.26.2015
                    finalsqry.Append(String.Format("and c.patron_id = {0}) aa left outer join ",patronid));
                    finalsqry.Append("((SELECT e.patron_id, f.amount_val, f.in_pesovalue FROM patrons_ordinance e, ordinance_tbl f WHERE e.ordinance_id = f.id ");
                    finalsqry.Append(String.Format("AND ((f.with_enddate = 0 && '{0:yyyy-MM-dd}' >= f.effective_date) || (f.with_enddate = 1 && '{0:yyyy-MM-dd}' >= f.effective_date && '{0:yyyy-MM-dd}' <= f.end_date)))) g ", recdate));
                    finalsqry.Append("ON aa.patron_id = g.patron_id ");

                    cmd = new MySqlCommand(finalsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlDataReader rd1 = cmd.ExecuteReader();
                    if (rd1.HasRows)
                    {
                        while (rd1.Read())
                        {
                            double dblout = 0;
                            if (double.TryParse(rd1[0].ToString(), out dblout))
                                ord_val += dblout;
                        }
                    }
                    rd1.Dispose();
                    cmd.Dispose();
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                Cursor.Current = Cursors.Default;

                return ord_val;
            }
            catch
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                Cursor.Current = Cursors.Default;

                return ord_val;
            }
        }

        public void populateBIRmsr(frmMain frm, String tbl, string sConnString, DateTime startdate, DateTime enddate)
        {
            
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if (DateTime.TryParse(rd[0].ToString(), out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                String finalsqry = String.Empty;
                int cntr = 0;
                foreach (DateTime recdate in datecoll)
                {
                    //for each date in arraylist
                    finalsqry = String.Empty;
                    finalsqry = String.Format("INSERT INTO {0} VALUES(0,", tbl);
                   

                    //accumulated or
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select min(b.or_number), max(b.or_number),count(b.or_number) "); 
                    sqry.Append("from movies_schedule_list_reserved_seat b ");
                    sqry.Append("where b.movies_schedule_list_id ");
                    sqry.Append("in (select a.id from movies_schedule_list a ");
                    sqry.Append(String.Format("where a.start_time >= '{0:yyyy/MM/dd}' ", recdate));
                    sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", recdate.AddDays(1)));
                    sqry.Append("and a.status = 1) and b.status = 1");

                    MySqlCommand cmd1 = new MySqlCommand(sqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlDataReader rd1 = cmd1.ExecuteReader();
                    string smin = "";
                    string smax = "";
                    int inttotalcnt = 0;
                    if (rd1.HasRows)
                    {
                        while (rd1.Read())
                        {
                            if (!DBNull.Value.Equals(rd1[0]))
                                smin = rd1[0].ToString();
                            if (!DBNull.Value.Equals(rd1[1]))
                                smax = rd1[1].ToString();
                            int outcnt = 0;
                            if (int.TryParse(rd1[2].ToString(), out outcnt))
                                inttotalcnt = outcnt;
                        }
                    }
                    rd1.Close();
                    rd1.Dispose();
                    cmd1.Dispose();

                    if (inttotalcnt > 0)
                    {
                        cntr += 1;
                        finalsqry = finalsqry + String.Format("'{0}',", cntr.ToString()); //insert cntr
                    }
                    else
                        finalsqry = finalsqry + String.Format("'{0}',", ""); //insert cntr

                    finalsqry = finalsqry + String.Format("'{0:yyyy/MM/dd}',", recdate); //insert date
                    finalsqry = finalsqry + String.Format("'{0}',", smin); //insert OR BEGIN
                    finalsqry = finalsqry + String.Format("'{0}',", smax); //insert OR END
                    finalsqry = finalsqry + String.Format("{0},", inttotalcnt); //insert OR count

                    //accumulated sales
                    double dblbeg = 0, dblend = 0, dbltotal = 0;

                    if (inttotalcnt > 0)
                    {
                        //yesterday
                        sqry = new StringBuilder();
                        sqry.Append("SELECT sum(a.base_price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b ");//change price to base_price 1.7.2016 
                        sqry.Append("WHERE a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}' ", m_clscon.SystemCollectionStartDate));
                        sqry.Append(String.Format("AND b.start_time < '{0:yyyy-MM-dd}' ", recdate)); // less than date today = yesterday
                        sqry.Append("AND b.status = 1");

                        try
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            using (MySqlCommand cmd3 = new MySqlCommand(sqry.ToString(), myconn))
                            {
                                using (MySqlDataReader reader3 = cmd3.ExecuteReader())
                                {
                                    while (reader3.Read())
                                    {
                                        if (!DBNull.Value.Equals(reader3[0]))
                                            dblbeg = Convert.ToDouble(reader3[0].ToString());
                                    }
                                    reader3.Close();
                                }
                                cmd3.Dispose();
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }

                        //today
                        sqry = new StringBuilder();
                        sqry.Append("SELECT sum(a.base_price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b ");//change price to base_price 1.7.2016 
                        sqry.Append("WHERE a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}' ", m_clscon.SystemCollectionStartDate));
                        sqry.Append(String.Format("AND b.start_time < '{0:yyyy-MM-dd}' ", recdate.AddDays(1)));//less than tomorrow = today
                        sqry.Append("AND b.status = 1");

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        try
                        {
                            using (MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn))
                            {
                                using (MySqlDataReader reader4 = cmd4.ExecuteReader())
                                {
                                    while (reader4.Read())
                                    {
                                        dblend = Convert.ToDouble(reader4[0].ToString());
                                    }
                                    reader4.Close();
                                }
                                cmd4.Dispose();
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }

                    if ((dblend > 0) && (dblend > 0))
                        dbltotal = dblend - dblbeg;

                    finalsqry = finalsqry + String.Format("{0},", dblbeg); //insert sales BEGIN
                    finalsqry = finalsqry + String.Format("{0},", dblend); //insert sales END
                    finalsqry = finalsqry + String.Format("{0},", dbltotal); //insert sales total

                    //vat sales
                    finalsqry = finalsqry + String.Format("{0},", "0"); //insert vatable sales
                    finalsqry = finalsqry + String.Format("{0},", dbltotal); //insert vat-exempt sales
                    finalsqry = finalsqry + String.Format("{0},", "0"); //insert zero rated sales

                    /*//vat 12% //remarked 6.23.2015 for cinema is vat exmpt cause double taxation
                    double vat = 0,netsales = 0;
                    if (dbltotal > 0) 
                        netsales = dbltotal / 1.12;

                    if (netsales > 0)
                        vat = dbltotal - netsales;*/
                    

                    finalsqry = finalsqry + String.Format("{0},", "0"); //insert vat 

                    //discount
                    finalsqry = finalsqry + String.Format("{0},", "0"); //discount SC
                    finalsqry = finalsqry + String.Format("{0},", "0"); //discount PWD
                    finalsqry = finalsqry + String.Format("{0},", "0"); //discount Employee

                    //netsales
                    finalsqry = finalsqry + String.Format("{0},", dbltotal); //NET SALES

                    //userid
                    finalsqry = finalsqry + String.Format("'{0}')", frm.m_usercode); //userid
                    MySqlCommand cmd2 = new MySqlCommand(finalsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                }


                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {
            }
        }

        public void populatemsrtransaction(frmMain frm, string tbl, string sConnString, DateTime startdate, DateTime enddate)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            MySqlConnection myconn = new MySqlConnection(frm._connection);
            try
            {
            
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if (DateTime.TryParse(rd[0].ToString(), out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Close();
                rd.Dispose();
                cmd.Dispose();

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                StringBuilder finalsqry = new StringBuilder();
                int cntr1 = 0;
                foreach (DateTime recdate in datecoll)
                {
                    finalsqry = new StringBuilder();
                    finalsqry.Append(String.Format("INSERT INTO {0} values(0, '{1:yyyy-MM-dd}', ", tbl, recdate));
                    cntr1 += 1;
                    StringBuilder newsqry = new StringBuilder();
                    /*newsqry.Append("select table1.total_cnt, sum(table1.cash_amount) total_cash, ");
                    newsqry.Append("sum(table1.gift_certificate_amount) total_gc, sum(table1.credit_amount) total_cc ");
                    newsqry.Append("from (select count(a.session_id) total_cnt, a.cash_amount, ");
                    newsqry.Append("a.gift_certificate_amount, a.credit_amount ");
                    newsqry.Append("from session a, ticket b, movies_schedule_list_reserved_seat c, movies_schedule_list d, movies_schedule e ");
                    newsqry.Append("where a.session_id = b.session_id ");
                    newsqry.Append("and b.id = c.ticket_id ");
                    newsqry.Append("and c.status = 1 ");
                    newsqry.Append("and c.movies_schedule_list_id = d.id ");
                    newsqry.Append("and d.status = 1 ");
                    newsqry.Append("and d.movies_schedule_id = e.id ");
                    newsqry.Append(String.Format("and e.movie_date = '{0:yyyy-MM-dd}' ", recdate));
                    newsqry.Append("group by a.session_id) table1 ");*/

                    newsqry.Append("select a.payment_mode, count(a.session_id) total_cnt, sum(c.base_price) cash_amount, a.gift_certificate_amount, ");
                    newsqry.Append("(sum(c.base_price) - a.gift_certificate_amount - cash_amount) credit_amount ");
                    newsqry.Append("from session a, ticket b, movies_schedule_list_reserved_seat c, ");
                    newsqry.Append("movies_schedule_list d, movies_schedule e where a.session_id = b.session_id and b.id = c.ticket_id ");
                    newsqry.Append("and c.status = 1 and c.movies_schedule_list_id = d.id and d.status = 1 ");
                    newsqry.Append(String.Format("and d.movies_schedule_id = e.id and e.movie_date = '{0:yyyy-MM-dd}' ",recdate));
                    newsqry.Append("group by a.session_id");

                    MySqlCommand cmd1 = new MySqlCommand(newsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlDataReader rd1 = cmd1.ExecuteReader();
                    if (rd1.HasRows)
                    {
                        int totalcnt = 0;
                        double totalcash = 0;
                        double totalcc = 0;
                        double totalgc = 0;
                        while (rd1.Read())
                        {
                            int inttransaction = 0;
                            int outtransaction = 0;
                            if (!DBNull.Value.Equals(rd1[0]))
                            {
                                if (int.TryParse(rd1[0].ToString(), out outtransaction))
                                    inttransaction = outtransaction;
                            }

                        
                            int intout = 0;
                            if (!DBNull.Value.Equals(rd1[1]))
                            {
                                if (int.TryParse(rd1[1].ToString(), out intout))
                                    totalcnt += intout;
                            }

                        
                            double dblout = 0;
                            if (inttransaction == 1)
                            {
                                if (!DBNull.Value.Equals(rd1[2]))
                                {
                                    if (double.TryParse(rd1[2].ToString(), out dblout))
                                        totalcash += dblout;
                                }
                            }

                            if(inttransaction == 3)
                            {
                                dblout = 0;
                                if (!DBNull.Value.Equals(rd1[2]))
                                {
                                    if (double.TryParse(rd1[2].ToString(), out dblout))
                                        totalcash += dblout;
                                }
                                dblout = 0;
                                if (!DBNull.Value.Equals(rd1[3]))
                                {
                                    if (double.TryParse(rd1[3].ToString(), out dblout))
                                    {
                                        totalgc += dblout;
                                        totalcash -= dblout;
                                    }
                                }
                            }

                            if (inttransaction == 4)
                            {
                                dblout = 0;
                                if (!DBNull.Value.Equals(rd1[4]))
                                {
                                    if (double.TryParse(rd1[4].ToString(), out dblout))
                                        totalcc = dblout;
                                }
                            }

                            if (inttransaction == 2)
                            {
                                dblout = 0;
                                if (!DBNull.Value.Equals(rd1[3]))
                                {
                                    if (double.TryParse(rd1[3].ToString(), out dblout))
                                        totalgc += dblout;
                                }
                                dblout = 0;
                                if (!DBNull.Value.Equals(rd1[4]))
                                {
                                    if (double.TryParse(rd1[4].ToString(), out dblout))
                                        totalgc += dblout;
                                }
                            }
                        }
                        finalsqry.Append(String.Format("{0}, {1}, {2}, {3},", totalcnt, totalcash, totalgc, totalcc));
                        rd1.Close();
                        rd1.Dispose();
                        cmd1.Dispose();
                    }
                    else// added 1.5.2016
                        finalsqry.Append(String.Format("{0}, {1}, {2}, {3},", 0, 0, 0, 0)); // added 1.5.2016

                    rd1.Close();//added 1.5.2016
                    rd1.Dispose();//added 1.5.2016
                    cmd1.Dispose();//added 1.5.2016

                    finalsqry.Append(String.Format("'{0}')",frm.m_usercode));
                    MySqlCommand cmd2 = new MySqlCommand(finalsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                }
            }
            catch(Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show(err.Message);
            }
        }

        public void populateBIRmsr2(frmMain frm, string tbl, string sConnString, DateTime startdate, DateTime enddate)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if (DateTime.TryParse(rd[0].ToString(), out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                string finalsqry = String.Empty;
                int cntr = 0;
                foreach (DateTime recdate in datecoll)
                {
                    //check for the config collection start date

                    //for each date in arraylist
                    finalsqry = String.Empty;
                    finalsqry = String.Format("INSERT INTO {0} VALUES(0,", tbl);

                    //accumulated or
                    StringBuilder sqry = new StringBuilder();
                    //sqry.Append("select min(b.or_number), max(b.or_number),count(b.or_number) ");
                    //sqry.Append("from movies_schedule_list_reserved_seat b ");
                    //sqry.Append("where b.movies_schedule_list_id ");
                    //sqry.Append("in (select a.id from movies_schedule_list a ");
                    //sqry.Append(String.Format("where a.start_time >= '{0:yyyy/MM/dd}' ", recdate));
                    //sqry.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", recdate.AddDays(1)));
                    //sqry.Append("and a.status = 1) and b.status = 1");

                    sqry.Append("SELECT min(a.or_number), max(a.or_number),count(a.or_number) ");
                    sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, movies_schedule c ");
                    sqry.Append(String.Format("WHERE c.movie_date = '{0:yyyy/MM/dd}' ",recdate));
                    sqry.Append("AND b.movies_schedule_id = c.id ");
                    sqry.Append("AND a.movies_schedule_list_id = b.id ");
                    sqry.Append(String.Format("AND b.start_time >= '{0:yyyy/MM/dd}' ",m_clscon.SystemCollectionStartDate));
                    sqry.Append("AND a.status = 1 ");
                    sqry.Append("AND b.status = 1");

                    MySqlCommand cmd1 = new MySqlCommand(sqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlDataReader rd1 = cmd1.ExecuteReader();
                    string smin = "";
                    string smax = "";
                    int inttotalcnt = 0;
                    if (rd1.HasRows)
                    {
                        while (rd1.Read())
                        {
                            if (!DBNull.Value.Equals(rd1[0]))
                                smin = rd1[0].ToString();
                            if (!DBNull.Value.Equals(rd1[1]))
                                smax = rd1[1].ToString();
                            int outcnt = 0;
                            if (!DBNull.Value.Equals(rd1[2]))
                            {
                                if (int.TryParse(rd1[2].ToString(), out outcnt))
                                inttotalcnt = outcnt;
                            }
                        }
                    }
                    rd1.Close();
                    rd1.Dispose();
                    cmd1.Dispose();

                    if (inttotalcnt > 0)
                    {
                        cntr += 1;
                        finalsqry = finalsqry + String.Format("'{0}',", cntr.ToString()); //insert cntr
                    }
                    else
                        finalsqry = finalsqry + String.Format("'{0}',", ""); //insert cntr

                    finalsqry = finalsqry + String.Format("'{0:yyyy/MM/dd}',", recdate); //insert date
                    finalsqry = finalsqry + String.Format("'{0}',", smin); //insert OR BEGIN
                    finalsqry = finalsqry + String.Format("'{0}',", smax); //insert OR END
                    finalsqry = finalsqry + String.Format("{0},", inttotalcnt); //insert OR count

                    //accumulated sales
                    double dblbeg = 0, dblend = 0, dbltotal = 0, totaldiscount = 0;

                    if (inttotalcnt > 0)
                    {
                        //yesterday
                        sqry = new StringBuilder();
                        sqry.Append("SELECT sum(a.base_price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b ");
                        sqry.Append("WHERE a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}' ", m_clscon.SystemCollectionStartDate));
                        sqry.Append(String.Format("AND b.start_time < '{0:yyyy-MM-dd}' ", recdate)); // less than date today = yesterday
                        sqry.Append("AND b.status = 1");

                        try
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            using (MySqlCommand cmd3 = new MySqlCommand(sqry.ToString(), myconn))
                            {
                                using (MySqlDataReader reader3 = cmd3.ExecuteReader())
                                {
                                    while (reader3.Read())
                                    {
                                        if (!DBNull.Value.Equals(reader3[0]))
                                            dblbeg = Convert.ToDouble(reader3[0].ToString());
                                    }
                                    reader3.Close();
                                }
                                cmd3.Dispose();
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }

                        //today
                        sqry = new StringBuilder();
                        sqry.Append("SELECT sum(a.base_price) total FROM movies_schedule_list_reserved_seat a, movies_schedule_list b ");
                        sqry.Append("WHERE a.status = 1 ");
                        sqry.Append("AND a.movies_schedule_list_id = b.id ");
                        sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}' ", m_clscon.SystemCollectionStartDate));
                        sqry.Append(String.Format("AND b.start_time < '{0:yyyy-MM-dd}' ", recdate.AddDays(1)));//less than tomorrow = today
                        sqry.Append("AND b.status = 1");

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        try
                        {
                            using (MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn))
                            {
                                using (MySqlDataReader reader4 = cmd4.ExecuteReader())
                                {
                                    while (reader4.Read())
                                    {
                                        if (!DBNull.Value.Equals(reader4[0]))
                                            dblend = Convert.ToDouble(reader4[0].ToString());
                                    }
                                    reader4.Close();
                                }
                                cmd4.Dispose();
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }

                    if ((dblend > 0) && (dblend > 0))
                        dbltotal = dblend - dblbeg;

                    finalsqry = finalsqry + String.Format("{0},", dblbeg); //insert sales BEGIN
                    finalsqry = finalsqry + String.Format("{0},", dblend); //insert sales END
                    finalsqry = finalsqry + String.Format("{0},", dbltotal); //insert sales total

                    //5.26.2015 compute for vatable sales and vat exempt sales
                    //vat exmpt sales computation: sellingprice / 1.12 = vat_exempt_sales; discount = vat_exempt_sales * .20
                    sqry = new StringBuilder();
                    sqry.Append("SELECT sum(a.base_price) total, sum(f.price) total_base, ");
                    sqry.Append("sum(f.price / 1.12) total_vat_exempt, ");
                    sqry.Append("sum((f.price / 1.12) - (f.price - a.price)) total_discount, count(a.ticket_id) total_cnt ");
                    sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                    sqry.Append("movies_schedule_list_patron c, patrons d, movies_schedule e, ticket_prices f ");
                    sqry.Append("WHERE a.status = 1 AND a.movies_schedule_list_id = b.id ");
                    sqry.Append("AND b.status = 1 AND a.patron_id = c.id AND c.patron_id = d.id AND d.code LIKE '%SC%' ");
                    sqry.Append(String.Format("AND b.movies_schedule_id = e.id AND movie_date = '{0:yyyy-MM-dd}' ", recdate));
                    sqry.Append("AND d.base_price = f.id ");
                    sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}'", m_clscon.SystemCollectionStartDate));
                    //sqry.Append("SELECT sum(a.price) total, count(a.ticket) totalcnt,  FROM movies_schedule_list_reserved_seat a, movies_schedule_list b,  ");
                    //sqry.Append("movies_schedule_list_patron c, patrons d, movies_schedule e ");
                    //sqry.Append("WHERE a.status = 1 AND a.movies_schedule_list_id = b.id  ");
                    //sqry.Append("AND b.status = 1 AND a.patron_id = c.id AND c.patron_id = d.id AND d.code LIKE '%SC%' ");
                    //sqry.Append(String.Format("AND b.movies_schedule_id = e.id AND movie_date = '{0:yyyy-MM-dd}'", recdate));

                    double vatablesales = 0;
                    double vatexemptsales = 0;
                    double sctotalsales = 0;
                    double scdicount = 0;
                    int sccount = 0;
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    try
                    {
                        using (MySqlCommand cmd5 = new MySqlCommand(sqry.ToString(), myconn))
                        {
                            using (MySqlDataReader reader5 = cmd5.ExecuteReader())
                            {
                                double outsales = 0;
                                double outsctotal = 0;
                                double outscdiscount = 0;
                                int outsccount = 0;
                                while (reader5.Read())
                                {
                                    if (!DBNull.Value.Equals(reader5["total_vat_exempt"]))
                                    {
                                        if (double.TryParse(reader5["total_vat_exempt"].ToString(), out outsales))
                                            vatexemptsales = outsales;
                                    }
                                    if (!DBNull.Value.Equals(reader5["total"]))
                                    {
                                        if (double.TryParse(reader5["total"].ToString(), out outsctotal))
                                            sctotalsales = outsctotal;
                                    }
                                    if (!DBNull.Value.Equals(reader5["total_discount"]))
                                    {
                                        if (double.TryParse(reader5["total_discount"].ToString(), out outscdiscount))
                                            scdicount = outscdiscount;
                                    }
                                    if (!DBNull.Value.Equals(reader5["total_cnt"]))
                                    {
                                        if (int.TryParse(reader5["total_cnt"].ToString(), out outsccount))
                                            sccount = outsccount;
                                    }
                                }
                                reader5.Close();
                            }
                            cmd5.Dispose();
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }

                    totaldiscount += scdicount;

                    vatablesales = dbltotal - sctotalsales;

                    //vat sales
                    finalsqry = finalsqry + String.Format("{0},", vatablesales); //insert vatable sales
                    finalsqry = finalsqry + String.Format("{0},", vatexemptsales); //insert vat-exempt sales
                    finalsqry = finalsqry + String.Format("{0},", "0"); //insert zero rated sales

                    //vat 12% 
                    double vat = 0, netsales = 0, tempnetsales = 0, twelvepercent = 0;

                    if (vatexemptsales > 0)
                        tempnetsales += vatexemptsales;

                    if (dbltotal > 0)
                        tempnetsales += vatablesales / 1.12;

                    if (vatablesales > 0)
                        twelvepercent = (vatablesales / 1.12);

                    if (vatablesales > 0)
                        vat = vatablesales - twelvepercent;

                    finalsqry = finalsqry + String.Format("{0},", vat); //insert vat

                    //5.28.2015 discount for PWD
                    sqry = new StringBuilder();
                    sqry.Append("SELECT sum(a.base_price) total, sum(f.price) total_base, ");
                    sqry.Append("sum(f.price * 0.20) total_discount, count(a.ticket_id) total_cnt ");
                    sqry.Append("FROM movies_schedule_list_reserved_seat a, movies_schedule_list b, ");
                    sqry.Append("movies_schedule_list_patron c, patrons d, movies_schedule e, ticket_prices f ");
                    sqry.Append("WHERE a.status = 1 AND a.movies_schedule_list_id = b.id ");
                    sqry.Append("AND b.status = 1 AND a.patron_id = c.id AND c.patron_id = d.id AND d.code LIKE '%PWD%' ");
                    sqry.Append(String.Format("AND b.movies_schedule_id = e.id AND movie_date = '{0:yyyy-MM-dd}' ", recdate));
                    sqry.Append("AND d.base_price = f.id ");
                    sqry.Append(String.Format("AND b.start_time >= '{0:yyyy-MM-dd}'", m_clscon.SystemCollectionStartDate));

                    double pwddicount = 0;
                    int pwdcount = 0;
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    try
                    {
                        using (MySqlCommand cmd6 = new MySqlCommand(sqry.ToString(), myconn))
                        {
                            using (MySqlDataReader reader6 = cmd6.ExecuteReader())
                            {
                                double outpwddiscount = 0;
                                int outpwdcount = 0;
                                while (reader6.Read())
                                {
                                    if (!DBNull.Value.Equals(reader6["total_discount"]))
                                    {
                                        if (double.TryParse(reader6["total_discount"].ToString(), out outpwddiscount))
                                            pwddicount = outpwddiscount;
                                    }
                                    if (!DBNull.Value.Equals(reader6["total_cnt"]))
                                    {
                                        if (int.TryParse(reader6["total_cnt"].ToString(), out outpwdcount))
                                            pwdcount = outpwdcount;
                                    }
                                }
                                reader6.Close();
                            }
                            cmd6.Dispose();
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }

                    totaldiscount += pwddicount;

                    //discount
                    finalsqry = finalsqry + String.Format("{0},", scdicount); //discount SC
                    finalsqry = finalsqry + String.Format("{0},", pwddicount); //discount PWD
                    finalsqry = finalsqry + String.Format("{0},", "0"); //discount Employee/others

                    //netsales
                    if (tempnetsales > 0)
                        netsales = tempnetsales - totaldiscount;
                    else
                        netsales = totaldiscount;
                    finalsqry = finalsqry + String.Format("{0},", netsales); //NET SALES

                    //userid
                    finalsqry = finalsqry + String.Format("'{0}')", frm.m_usercode); //userid
                    MySqlCommand cmd2 = new MySqlCommand(finalsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {
            }
        }
        public void populateTableDaily(frmMain frm, String tbl, string sConnString, string sQuery)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            MySqlConnection myconn = new MySqlConnection(sConnString + "Allow User Variables=True;");
            try
            {
                MySqlCommand cmd2 = new MySqlCommand(sQuery, myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        public void populateScreening(frmMain frm, String tbl, string sConnString, DateTime startdate)
        {
            try
            {
                DataTable dtsched = new DataTable();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select a.id, a.movies_schedule_id from movies_schedule_list a ");
                sQuery.Append(String.Format("where a.start_time > '{0:yyyy/MM/dd}' ", startdate));
                sQuery.Append(String.Format("and a.start_time < '{0:yyyy/MM/dd}' ", startdate.AddDays(1)));
                sQuery.Append("and a.status = 1 ");
                sQuery.Append("order by a.movies_schedule_id, a.start_time");
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                if (myconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sQuery.ToString(), myconn);
                    da.Fill(dtsched);
                    myconn.Close();
                }

                if (dtsched.Rows.Count > 0)
                {
                    int schedidcntr = 0;
                    int movieschedid = -1;
                    int schedid = -1;
                    foreach (DataRow dr in dtsched.Rows)
                    {
                        schedid = Convert.ToInt32(dr[0].ToString());
                        if (schedidcntr == 0)
                            movieschedid = Convert.ToInt32(dr[1].ToString());
                        else
                        {
                            if (movieschedid != Convert.ToInt32(dr[1].ToString()))
                            {
                                movieschedid = Convert.ToInt32(dr[1].ToString());
                                schedidcntr = 0;
                            }
                        }
                        schedidcntr += 1; //screening counter

                        int movieid = -1;
                        int cinemaid = -1;
                        int qtty = 0;
                        double amt = 0;
                        sQuery = new StringBuilder();
                        sQuery.Append("select f.id cinema_id, d.movie_id,count(b.ticket_id) qtty,sum(b.base_price) amt from movies_schedule_list_reserved_seat b, ");//1.7.2016 changed price to base_price
                        sQuery.Append("movies_schedule_list c, movies_schedule d, cinema f ");
                        sQuery.Append(String.Format("where b.movies_schedule_list_id = {0} ", schedid));
                        sQuery.Append("and b.movies_schedule_list_id = c.id ");
                        sQuery.Append("and c.movies_schedule_id = d.id ");
                        sQuery.Append("and d.cinema_id = f.id ");
                        sQuery.Append("and b.status = 1");
                        MySqlCommand cmd1 = new MySqlCommand(sQuery.ToString(), myconn);
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlDataReader rd1 = cmd1.ExecuteReader();
                        int inttotal = 0;
                        if (rd1.HasRows)
                        {
                            while (rd1.Read())
                            {
                                cinemaid = Convert.ToInt32(rd1["cinema_id"].ToString());
                                movieid = Convert.ToInt32(rd1["movie_id"].ToString());
                                qtty = Convert.ToInt32(rd1["qtty"].ToString());
                                if (!DBNull.Value.Equals(rd1["amt"]))
                                    amt = Convert.ToDouble(rd1["amt"].ToString());
                            }
                        }
                        rd1.Dispose();
                        cmd1.Dispose();

                        StringBuilder finalqry = new StringBuilder();
                        finalqry.Append("insert into tmp_screening values(0");
                        finalqry.Append(String.Format(",{0}", cinemaid));
                        finalqry.Append(String.Format(",{0}", movieid));
                        finalqry.Append(String.Format(",{0}", schedidcntr));
                        finalqry.Append(String.Format(",{0}", qtty));
                        finalqry.Append(String.Format(",{0}", amt));
                        finalqry.Append(String.Format(",'{0:yyyy/MM/dd}'", startdate));
                        finalqry.Append(String.Format(",'{0}'", frm.UserCode));
                        finalqry.Append(")");
                        MySqlCommand cmd2 = new MySqlCommand(finalqry.ToString(), myconn);
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();
                        finalqry = new StringBuilder();
                    }
                }
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        //7.1.2015 start
        String getDateRange(int movieid, DateTime dtstart, DateTime dtend, string sConnString)
        {
            MySqlConnection myconn = new MySqlConnection(sConnString);
            StringBuilder strrange = new StringBuilder();
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select * from movies_distributor where movie_id = {0} ", movieid.ToString()));
            sqry.Append(String.Format("and effective_date >= '{0:yyyy-MM-dd}' and effective_date < '{1:yyyy-MM-dd}'",dtstart,dtend));
            MySqlCommand cmd1 = new MySqlCommand(sqry.ToString(), myconn);
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            MySqlDataReader rd1 = cmd1.ExecuteReader();
            if (rd1.HasRows)
            {
                while (rd1.Read())
                {
                    strrange.Append("From: "); 
                    DateTime dtstartval = Convert.ToDateTime(rd1["effective_date"]);
                    if (dtstart > dtstartval)
                        strrange.Append(String.Format("{0:MM/dd/yyyy}", dtstart));
                    else
                        strrange.Append(String.Format("{0:MM/dd/yyyy}", dtstartval));

                    strrange.Append(" To: ");
                    DateTime dtendval = dtstartval.AddDays(Convert.ToInt32(rd1["day_count"]) - 1);
                    if (dtend.AddDays(-1) < dtendval)
                        strrange.Append(String.Format("{0:MM/dd/yyyy}", dtend.AddDays(-1)));
                    else
                        strrange.Append(String.Format("{0:MM/dd/yyyy}", dtendval));
                }
            }
            return strrange.ToString();
        }
        //7.1.2015 end

        //6.26.2015 added new parameter cinema id
        public void populateAccountingTbl2(string usercode, String tbl, string sConnString, DateTime startdate, DateTime enddate, int movieid, DateTime effdate, double share, int shareid, int cinemaid)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append("select h.name as dist_nm, c.title as movie_nm,f.name as patron_nm, count(a.ticket_id) qty, ");
                sQuery.Append("a.base_price, sum(a.base_price) as gross_receipt, ");// returned to base_price 1.7.2016//change base_price to price 6.26.2015
                sQuery.Append("f.with_cultural, f.with_amusement, f.id as patron_id, c.id as movie_id, j.name as cinema_name, ");
                sQuery.Append("f.with_gross_margin, f.with_prod_share, ");//added 7.10.2015 for excempting complimentary pass sample
                sQuery.Append("sum(a.ordinance_price) as ord_sum, sum(a.surcharge_price) as sur_sum, "); // added for surcharge and ordinance 1.7.2016
                sQuery.Append("min(b.movie_date), max(b.movie_date), ");//updated 3.11.2016 min(col[15]) and max(col[16]) //added 2.6.2016
                sQuery.Append("a.price ");//3.11.2016 moved to (col[17]) //2.23.2016 list(16)
                sQuery.Append("from movies_schedule_list_reserved_seat a, ");
                sQuery.Append("movies_schedule_list_patron e, patrons f, ");
                sQuery.Append("movies_schedule b, movies c, movies_schedule_list d, ");
                sQuery.Append("distributor h, cinema j ");
                sQuery.Append("where a.movies_schedule_list_id = d.id ");
                sQuery.Append(String.Format("and b.movie_date >= '{0:yyyy-MM-dd}' ", startdate));
                sQuery.Append(String.Format("and b.movie_date < '{0:yyyy-MM-dd}' ", enddate.AddDays(1)));
                sQuery.Append(String.Format("and b.movie_id = {0} ", movieid.ToString()));
                sQuery.Append("and d.movies_schedule_id = b.id ");
                sQuery.Append("and d.status = 1 ");
                sQuery.Append("and a.patron_id = e.id ");
                sQuery.Append("and e.patron_id = f.id ");
                sQuery.Append("and a.status = 1 ");
                sQuery.Append("and c.id = b.movie_id ");
                sQuery.Append("and c.dist_id = h.id ");
                sQuery.Append("and b.cinema_id = j.id ");
                sQuery.Append(String.Format("and b.cinema_id = {0} ",cinemaid));
                sQuery.Append("group by e.patron_id");

                DataTable dt = setDataTable(sQuery.ToString(), sConnString);

                StringBuilder finalsqry = new StringBuilder();

                if (dt.Rows.Count > 0)
                {
                    int qtty = 0;
                    double price = 0;
                    double baseprice = 0; //2.23.2016
                    double surchrg = 0;
                    double ordinance = 0;
                    double grossreceipt = 0;
                    double deductions = 0;
                    double culturaltax = 0;
                    double amusementtax = 0;
                    double netamount = 0;
                    double shareamount = 0;
                    double grossmargin = 0;
                    string sharedaterange = "";// "From: " + String.Format("{0:MM/dd/yyyy}", startdate) + " To: " + String.Format("{0:MM/dd/yyyy}", enddate);//getDateRange(movieid, startdate, enddate.AddDays(1), sConnString);//edited 2.4.2016

                    /*compute for the strt date and the enddate*/
                    DateTime newstartdate = new DateTime();// startdate;
                    DateTime newenddate = new DateTime(); // enddate;
                    DateTime tmpmindate = new DateTime();
                    DateTime tmpmaxdate = new DateTime();
                    int cntr = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tmpmindate = Convert.ToDateTime(dr[15]);
                        if (tmpmindate >= startdate)
                        {
                            if (cntr > 0)
                            {
                                if (tmpmindate <= newstartdate)
                                    newstartdate = tmpmindate;
                            }
                            else
                                newstartdate = tmpmindate;
                        }

                        tmpmaxdate = Convert.ToDateTime(dr[16]);
                        if (tmpmaxdate <= enddate)
                        {
                            if (cntr > 0)
                            {
                                if (tmpmaxdate >= newenddate)
                                    newenddate = tmpmaxdate;
                            }
                            else
                                newenddate = tmpmaxdate;
                        }
                        cntr += 1;
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        /*if (Convert.ToDateTime(dr[15]) > startdate)
                        {
                            if (Convert.ToDateTime(dr[15]) >= newstartdate)
                                newstartdate = Convert.ToDateTime(dr[15]);
                            else
                                newenddate = Convert.ToDateTime(dr[15]);
                        }
                        else
                        {
                            if (Convert.ToDateTime(dr[15]) >= newstartdate)
                                newstartdate = Convert.ToDateTime(dr[15]);
                           else
                                newenddate = Convert.ToDateTime(dr[15]);
                        }*///remarked 3.11.2016

                        amusementtax = 0;
                        culturaltax = 0;
                        shareamount = 0;
                        grossmargin = 0;
                        ordinance = 0;
                        grossreceipt = 0;
                        surchrg = 0;

                        finalsqry = new StringBuilder();
                        qtty = 0;
                        baseprice = price = surchrg = ordinance = grossreceipt = deductions = culturaltax = amusementtax = netamount = shareamount = grossmargin = 0;
                        finalsqry.Append(String.Format("INSERT INTO {0} VALUES(0,", tbl));
                        finalsqry.Append(String.Format("'{0}',", dr[10].ToString()));//cinema name
                        finalsqry.Append(String.Format("'{0}',", dr[0].ToString().Replace("'", "''")));//distributir name // handle apostrophe 2.26.2016
                        finalsqry.Append(String.Format("'{0}',", dr[1].ToString().Replace("'", "''")));//movie name //handle apostrophe 2.26.2016
                        finalsqry.Append(String.Format("'{0}',", dr[2].ToString().Replace("'", "''")));//patron_nm //handle apostrophe 2.26.2016
                        if (dr[3].ToString() != "")
                        {
                            qtty = Convert.ToInt32(dr[3].ToString());
                            finalsqry.Append(String.Format("{0},", qtty));//qty
                        }
                        if (dr[4].ToString() != "")
                        {//change price to baseprice 2.23.2016
                            baseprice = Convert.ToDouble(dr[4].ToString());
                            finalsqry.Append(String.Format("{0},", baseprice));//price
                        }

                        /*updated to row[17] 3.11.2016*/
                        /*added price of the ticket per patron start 2.23.2016*/
                        if (dr[17].ToString() != "")
                            price = Convert.ToDouble(dr[17].ToString());
                        /*end 2.23.2016*/

                        if (dr[5].ToString() != "")
                        {
                            grossreceipt = Convert.ToDouble(dr[5]);
                            finalsqry.Append(String.Format("{0},", grossreceipt));//gross_receipt
                        }

                        double dblculturaltax = 0;//added 11.28.2017
                        if (dr[6].ToString() != "")
                        {
                            //added 11.28.2017
                            double dblout = 0;

                            if (double.TryParse(dr[6].ToString(), out dblout))
                            {
                                if (dblout == 0)
                                    _withculturaltax = false;
                                else
                                {
                                    _withculturaltax = true;
                                    dblculturaltax = dblout;
                                }
                            }
                            //if (Convert.ToInt32(dr[6].ToString()) == 0)//cultural tax//remarked 11.28.2017
                            if(dblculturaltax == 0)
                                finalsqry.Append(String.Format("{0},", "0"));//without cultural tax
                            else
                            {
                                if (qtty > 0)
                                {
                                    culturaltax = qtty * 0.25;
                                    finalsqry.Append(String.Format("{0},", culturaltax));//with cultural tax
                                }
                            }
                            deductions += culturaltax;
                        }
                        bool _withamusementtax = false;//added 8.1.2018
                        if (dr[7].ToString() != "")
                        {
                            if (Convert.ToInt32(dr[7].ToString()) == 0)//amusement tax
                            {
                                finalsqry.Append(String.Format("{0},", "0"));//without amusement tax rate
                                finalsqry.Append(String.Format("{0},", "0"));//without amusement tax
                            }
                            else
                            {
                                _withamusementtax = true;//added 8.1.2018
                                if (qtty > 0)
                                {
                                    string strid = "";
                                    if (!DBNull.Value.Equals(dr[8]))
                                    {
                                        if (dr[8].ToString() != "")
                                        {
                                            if (Convert.ToInt32(dr[8].ToString()) > 0)//patron id
                                                strid = dr[8].ToString();
                                        }
                                    }
                                    /*needed intval with value if the basis is the price*/
                                    //double intval = getsurchargeval(sConnString, startdate, enddate, strid);//remarked 2.23.2016
                                    /*if thebasis is a base_price then intval shoul be 0*/
                                    //double intval = 0;//remarked 2.23.2016
                                    //wit amusement tax rate
                                    double taxrate = 0;
                                    /*if(intval > 0) remarked 2.23.2016
                                        taxrate = (price - 0.25 - intval) * 0.090909090909091;// price * 0.0908 remarked ver 1 7.8.2015
                                    else*/
                                    if (baseprice > 0)
                                    {
                                        //added 11.28.2017
                                        if((_withculturaltax == true) && (dblculturaltax > 0))
                                            taxrate = (baseprice - 0.25) * 0.090909090909091;//change price to baseprice 2.23.2016 // price * 0.0908 remarked ver 1 7.8.2015
                                        else
                                            taxrate = baseprice * 0.090909090909091;//added 11.28.2017
                                    }
                                    //added 5.8.2018 to change taxrate to 2 decimal places
                                    taxrate = Math.Round(taxrate, 2, MidpointRounding.AwayFromZero);
                                    amusementtax = qtty * taxrate;//edited 5.8.2018 taxrate is rounded to 2 decimal place as per randy request for fsm //remarked ver 1 7.8.2015
                                    //amusementtax = ((grossreceipt - culturaltax) / 1.1) * 0.1; ver 2
                                    finalsqry.Append(String.Format("{0},", taxrate));//with amusement tax rate
                                    finalsqry.Append(String.Format("{0},", amusementtax));//with amusement tax
                                }
                            }
                            deductions += amusementtax;
                        }

                        //compute for ordinance
                        string patronid = "";
                        if (!DBNull.Value.Equals(dr[8]))
                        {
                            if (dr[8].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[8].ToString()) > 0)//patron id
                                    patronid = dr[8].ToString();
                            }
                        }

                        string strcinemaid = "";
                        if (!DBNull.Value.Equals(dr[9]))
                        {
                            if (dr[9].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[9].ToString()) > 0)//cinema id
                                    strcinemaid = dr[9].ToString();
                            }
                        }
                        
                        //remarked 1.7.2016
                        //ordinance = ordinancevalue(sConnString, startdate, enddate, patronid, strcinemaid, cinemaid.ToString());
                        if (!DBNull.Value.Equals(dr[13]))
                        {
                            if (dr[13].ToString() != "")
                                ordinance = Convert.ToDouble(dr[13].ToString());
                        }

                        //1.6.2016 compute for surcharge
                        //surchrg = surchargevalue(sConnString, startdate, enddate, patronid, strcinemaid, cinemaid.ToString());
                        if (!DBNull.Value.Equals(dr[14]))
                        {
                            if (dr[14].ToString() != "")
                                surchrg = Convert.ToDouble(dr[14].ToString());
                        }

                        finalsqry.Append(String.Format("{0},", surchrg));//for surchrge
                        //deductions += surchrg; //remarked 1.7.2016 //deduction for surcharge for the whole report


                        finalsqry.Append(String.Format("{0},", ordinance));//for ordinance
                        //deductions += ordinance;//remarked 1.7.2016 // deduction for surcharge for the whole report
                        netamount = grossreceipt - deductions;
                        //finalsqry.Append(String.Format("{0},", netamount));//net amount//moved to another location

                        //validate for with_prod_share
                        bool boolprodshare = false;
                        if (!DBNull.Value.Equals(dr[12]))
                        {
                            if (dr[12].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[12].ToString()) == 1)//with_prod_share
                                    boolprodshare = true;
                            }
                        }

                        //validate for with_gross_margin
                        bool boolgrossmargin = false;
                        if (!DBNull.Value.Equals(dr[11]))
                        {
                            if (dr[11].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[11].ToString()) == 1)//with_gross_margin
                                    boolgrossmargin = true;
                            }
                        }
                        if (boolgrossmargin)//DISREGARD gross margin share
                            finalsqry.Append(String.Format("{0},", "NULL"));
                        else
                            finalsqry.Append(String.Format("{0},", netamount));

                        if (boolprodshare)//DISREGARD production share
                        {
                            finalsqry.Append(String.Format("'{0:yyyy/MM/dd}',", effdate));//effective date//8.1.2018 edit from null
                            finalsqry.Append(String.Format("{0},", "NULL"));//share
                            finalsqry.Append(String.Format("{0},", "NULL"));//share amount
                        }
                        else
                        {
                            ///if the amusement tax is unchecked the sharing should not be computed
                            if (_withamusementtax)//added 8.1.2018
                            {
                                finalsqry.Append(String.Format("'{0:yyyy/MM/dd}',", effdate));//effective date
                                finalsqry.Append(String.Format("{0},", share));//share
                                shareamount = netamount * (share / 100);
                                finalsqry.Append(String.Format("{0},", shareamount));//share amount
                            }//added 8.1.2018
                            else//added 8.1.2018
                            {//added 8.1.2018
                                finalsqry.Append(String.Format("'{0:yyyy/MM/dd}',", effdate));//effective date ////added 8.1.2018 //edit from null
                                finalsqry.Append(String.Format("{0},", "NULL"));//share //added 8.1.2018
                                finalsqry.Append(String.Format("{0},", "NULL"));//share amount //added 8.1.2018
                            }//added 8.1.2018
                        }
                        
                        if (boolgrossmargin)//DISREGARD gross margin share
                            finalsqry.Append(String.Format("{0},", "NULL"));//gross margin
                        else
                        {
                            grossmargin = netamount - shareamount;
                            finalsqry.Append(String.Format("{0},", grossmargin));//gross margin
                        }

                        finalsqry.Append(String.Format("{0},", shareid));
                        //2.6.2016
                        sharedaterange = "From: " + String.Format("{0:MM/dd/yyyy}", newstartdate) + " To: " + String.Format("{0:MM/dd/yyyy}", newenddate);
                        finalsqry.Append(String.Format("'{0}',", sharedaterange));
                        finalsqry.Append(String.Format("'{0}')", usercode));

                        MySqlCommand cmd2 = new MySqlCommand(finalsqry.ToString(), myconn);
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();
                        finalsqry = new StringBuilder();
                    }
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        public void populateAccountingTbl(string usercode, String tbl, string sConnString, DateTime startdate, DateTime enddate, int movieid, DateTime effdate, double share, int shareid)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append("select h.name as dist_nm, c.title as movie_nm,f.name as patron_nm, count(a.ticket_id) qty, ");
                sQuery.Append("a.base_price, sum(a.base_price) as gross_receipt, ");
                sQuery.Append("f.with_cultural, f.with_amusement, f.id as patron_id, c.id as movie_id, j.name as cinema_name ");
                sQuery.Append("from movies_schedule_list_reserved_seat a, ");
                sQuery.Append("movies_schedule_list_patron e, patrons f, ");
                sQuery.Append("movies_schedule b, movies c, movies_schedule_list d, ");
                sQuery.Append("distributor h, cinema j ");
                sQuery.Append("where a.movies_schedule_list_id = d.id ");
                sQuery.Append(String.Format("and b.movie_date >= '{0:yyyy-MM-dd}' ",startdate));
                sQuery.Append(String.Format("and b.movie_date < '{0:yyyy-MM-dd}' ",enddate.AddDays(1)));
                sQuery.Append(String.Format("and b.movie_id = {0} ",movieid.ToString()));
                sQuery.Append("and d.movies_schedule_id = b.id ");
                sQuery.Append("and d.status = 1 ");
                sQuery.Append("and a.patron_id = e.id ");
                sQuery.Append("and e.patron_id = f.id ");
                sQuery.Append("and a.status = 1 ");
                sQuery.Append("and c.id = b.movie_id ");
                sQuery.Append("and c.dist_id = h.id ");
                sQuery.Append("and b.cinema_id = j.id ");
                sQuery.Append("group by e.patron_id");

                DataTable dt = setDataTable(sQuery.ToString(), sConnString);

                StringBuilder finalsqry = new StringBuilder();

                if (dt.Rows.Count > 0)
                {
                    int qtty = 0;
                    double price = 0;
                    double surchrg = 0;
                    double ordinance = 0;
                    double grossreceipt = 0;
                    double deductions = 0;
                    double culturaltax = 0;
                    double amusementtax = 0;
                    double netamount = 0;
                    double shareamount = 0;
                    double grossmargin = 0;
                    string sharedaterange = "From: " + String.Format("{0:MM/dd/yyyy}", startdate) + " To: " + String.Format("{0:MM/dd/yyyy}", enddate);
                    foreach (DataRow dr in dt.Rows)
                    {
                        finalsqry = new StringBuilder();
                        qtty = 0;
                        price = surchrg = ordinance = grossreceipt = deductions = culturaltax = amusementtax = netamount = shareamount = grossmargin = 0;
                        finalsqry.Append(String.Format("INSERT INTO {0} VALUES(0,", tbl));
                        finalsqry.Append(String.Format("'{0}',", dr[10].ToString()));//cinema name
                        finalsqry.Append(String.Format("'{0}',",dr[0].ToString()));//distributir name
                        finalsqry.Append(String.Format("'{0}',", dr[1].ToString()));//movie name
                        finalsqry.Append(String.Format("'{0}',", dr[2].ToString()));//patron_nm
                        if (dr[3].ToString() != "")
                        {
                            qtty = Convert.ToInt32(dr[3].ToString());
                            finalsqry.Append(String.Format("{0},",qtty));//qty
                        }
                        if (dr[4].ToString() != "")
                        {
                            price = Convert.ToDouble(dr[4].ToString());
                            finalsqry.Append(String.Format("{0},", price));//price
                        }
                        if(dr[5].ToString() != "")
                        {
                            grossreceipt = Convert.ToDouble(dr[5]);
                            finalsqry.Append(String.Format("{0},", grossreceipt));//gross_receipt
                        }
                        if (dr[6].ToString() != "")
                        {
                            if (Convert.ToInt32(dr[6].ToString()) == 0)//cultural tax
                                finalsqry.Append(String.Format("{0},", "0"));//without cultural tax
                            else
                            {
                                if (qtty > 0)
                                {
                                    culturaltax = qtty * 0.25;
                                    finalsqry.Append(String.Format("{0},", culturaltax));//with cultural tax
                                }
                            }
                            deductions += culturaltax;
                        }
                        if (dr[7].ToString() != "")
                        {
                            if (Convert.ToInt32(dr[7].ToString()) == 0)//amusement tax
                            {
                                finalsqry.Append(String.Format("{0},", "0"));//without amusement tax rate
                                finalsqry.Append(String.Format("{0},", "0"));//without amusement tax
                            }
                            else
                            {
                                if (qtty > 0)
                                {
                                    double dblval = price * 0.0908;//wit amusement tax rate
                                    amusementtax = qtty * dblval;
                                    finalsqry.Append(String.Format("{0},", dblval));//with amusement tax rate
                                    finalsqry.Append(String.Format("{0},", amusementtax));//with amusement tax
                                }
                            }
                            deductions += amusementtax;
                        }
                        finalsqry.Append(String.Format("{0},", surchrg));//for surchrge
                        deductions += surchrg; //deduction for surcharge for the whole report

                        //compute for ordinance
                        string patronid = "";
                        if (!DBNull.Value.Equals(dr[8]))
                        {
                            if (dr[8].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[8].ToString()) > 0)//patron id
                                    patronid = dr[8].ToString();
                            }
                        }

                        string cinemaid = "";
                        if (!DBNull.Value.Equals(dr[9]))
                        {
                            if (dr[9].ToString() != "")
                            {
                                if (Convert.ToInt32(dr[9].ToString()) > 0)//cinema id
                                    cinemaid = dr[9].ToString();
                            }
                        }

                        ordinance = ordinancevalue(sConnString, startdate, enddate, patronid, movieid.ToString(), cinemaid);// added cinemaid 6.26.2015
                       
                        finalsqry.Append(String.Format("{0},", ordinance));//for ordinance
                        deductions += ordinance; // deduction for surcharge for the whole report
                        netamount = grossreceipt - deductions;
                        finalsqry.Append(String.Format("{0},", netamount));//net amount
                        finalsqry.Append(String.Format("'{0:yyyy/MM/dd}',", effdate));//effective date
                        finalsqry.Append(String.Format("{0},", share));//share
                        shareamount = netamount * (share / 100);
                        finalsqry.Append(String.Format("{0},", shareamount));//share amount
                        grossmargin = netamount - shareamount;
                        finalsqry.Append(String.Format("{0},", grossmargin));//gross margin
                        finalsqry.Append(String.Format("{0},", shareid));
                        finalsqry.Append(String.Format("'{0}',", sharedaterange));
                        finalsqry.Append(String.Format("'{0}')", usercode));

                        MySqlCommand cmd2 = new MySqlCommand(finalsqry.ToString(), myconn);
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();
                        finalsqry = new StringBuilder();
                    }
                }


                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        double surchargevalue(string sConnString, DateTime startdate, DateTime enddate, string patronid, string movieid, string cinemaid)
        {
            double sur_val = 0;
            MySqlConnection myconn = new MySqlConnection(sConnString);
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("select z.Date ");
                sQuery.Append("from ( ");
                sQuery.Append("select curdate() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY as Date ");
                sQuery.Append("from (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as a ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as b ");
                sQuery.Append("cross join (select 0 as a union all select 1 union all select 2 union all select 3 union all select 4 union all select 5 union all select 6 union all select 7 union all select 8 union all select 9) as c ");
                sQuery.Append(") z ");
                sQuery.Append(String.Format("where z.Date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", startdate, enddate));
                sQuery.Append("order by z.Date asc");
                MySqlCommand cmd = new MySqlCommand(sQuery.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                ArrayList datecoll = new ArrayList();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        DateTime dtout = new DateTime();
                        if (DateTime.TryParse(rd[0].ToString(), out dtout))
                            datecoll.Add(dtout);
                    }
                }
                rd.Dispose();
                cmd.Dispose();

                StringBuilder finalsqry = new StringBuilder();
                foreach (DateTime recdate in datecoll)
                {
                    finalsqry = new StringBuilder();
                    finalsqry.Append("select (aa.cnt * IF(g.patron_id IS NULL, 0, IF(g.in_pesovalue, g.amount_val, aa.cnt * g.amount_val))) surcharge_val from ");
                    finalsqry.Append("(select count(d.ticket_id) as cnt, c.patron_id ");
                    finalsqry.Append("from movies_schedule a, movies_schedule_list b, ");
                    finalsqry.Append("movies_schedule_list_patron c, movies_schedule_list_reserved_seat d, cinema h ");
                    finalsqry.Append(String.Format("where a.movie_date = '{0:yyyy-MM-dd}' ", recdate));
                    finalsqry.Append(String.Format("and a.movie_id = {0} ", movieid));
                    finalsqry.Append("and a.id = b.movies_schedule_id ");
                    finalsqry.Append("and b.id = c.movies_schedule_list_id ");
                    finalsqry.Append("and d.patron_id = c.id ");
                    finalsqry.Append("and d.status = 1 ");
                    finalsqry.Append("and b.status = 1 ");
                    finalsqry.Append("and a.cinema_id = h.id ");
                    finalsqry.Append(String.Format("and a.cinema_id = {0} ", cinemaid)); 
                    finalsqry.Append(String.Format("and c.patron_id = {0}) aa left outer join ", patronid));
                    finalsqry.Append("((SELECT e.patron_id, f.amount_val, f.in_pesovalue FROM patrons_surcharge e, surcharge_tbl f WHERE e.surcharge_id = f.id ");
                    finalsqry.Append(String.Format("AND ((f.with_enddate = 0 && '{0:yyyy-MM-dd}' >= f.effective_date) || (f.with_enddate = 1 && '{0:yyyy-MM-dd}' >= f.effective_date && '{0:yyyy-MM-dd}' <= f.end_date)))) g ", recdate));
                    finalsqry.Append("ON aa.patron_id = g.patron_id ");

                    cmd = new MySqlCommand(finalsqry.ToString(), myconn);
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlDataReader rd1 = cmd.ExecuteReader();
                    if (rd1.HasRows)
                    {
                        while (rd1.Read())
                        {
                            double dblout = 0;
                            if (double.TryParse(rd1[0].ToString(), out dblout))
                                sur_val += dblout;
                        }
                    }
                    rd1.Dispose();
                    cmd.Dispose();
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                Cursor.Current = Cursors.Default;

                return sur_val;
            }
            catch
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                Cursor.Current = Cursors.Default;

                return sur_val;
            }
        }

        public void populatePOSTable(frmMain frm, String tbl, string sConnString, DateTime startdate, DateTime enddate)
        {
            try
            {
                DataTable dtsched = new DataTable();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                MySqlConnection myconn = new MySqlConnection(sConnString);
                StringBuilder sQuery = new StringBuilder();

                sQuery.Append("select table1.totsc,table1.totreg,table1.lastor,table2.serial_no,table2.permit_no,table2.min_no from (SELECT b.terminal, sum(IF(d.name LIKE '%SC %',a.base_price, NULL)) totsc, ");
                sQuery.Append("sum(IF(d.name NOT LIKE '%SC %',a.base_price, NULL)) totreg, max(a.or_number) lastor ");
                sQuery.Append("FROM movies_schedule_list_reserved_seat a, ticket b, movies_schedule_list_patron c, patrons d ");
                sQuery.Append("WHERE a.ticket_id = b.id ");
                sQuery.Append("AND a.patron_id = c.id ");
                sQuery.Append("AND c.patron_id = d.id ");
                sQuery.Append("AND a.status = 1 ");
                sQuery.Append("AND b.status = 1 ");
                sQuery.AppendFormat("AND b.ticket_datetime BETWEEN '{0:yyyy/MM/dd}' AND '{1:yyyy/MM/dd}' ", startdate, enddate.AddDays(1));
                sQuery.Append("GROUP BY b.terminal)table1 ");
                sQuery.Append("RIGHT JOIN ");
                sQuery.Append("(select * from tmp_pos_terminal)table2 ");
                sQuery.Append("ON table1.terminal = table2.terminal ");
                sQuery.Append("ORDER BY table2.id ASC");

                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();

                if (myconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sQuery.ToString(), myconn);
                    da.Fill(dtsched);
                    myconn.Close();
                }

                if (dtsched.Rows.Count > 0)
                {
                    double totreg = 0;
                    double totsc = 0;
                    int lastor = 0;
                    string serialno = String.Empty;
                    string permitno = String.Empty;
                    string minno = String.Empty;     
                    int rowcntr = 0;
                    int colcntr = 0;
                    double sumtotsc = 0;
                    double sumtotreg = 0;

                    string[,] strval = new string[dtsched.Columns.Count,dtsched.Rows.Count];

                    foreach (DataRow dr in dtsched.Rows)
                    {
                        object value = dr[4];
                        if (value == DBNull.Value)
                            permitno = "";
                        else
                            permitno = dr[4].ToString();
                        strval[colcntr,rowcntr] = permitno;
                        colcntr += 1;

                        value = dr[5];
                        if (value == DBNull.Value)
                            minno = "";
                        else
                            minno = dr[5].ToString();
                        strval[colcntr,rowcntr] = minno;
                        colcntr += 1;

                        value = dr[3];
                        if (value == DBNull.Value)
                            serialno = "";
                        else
                            serialno = dr[3].ToString();
                        strval[colcntr,rowcntr] = serialno;
                        colcntr += 1;

                        value = dr[1];
                        if (value == DBNull.Value)
                            totreg = 0;
                        else
                            totreg = Convert.ToDouble(dr[1]);
                        strval[colcntr, rowcntr] = String.Format("{0:n2}", totreg);
                        colcntr += 1;
                        sumtotreg += totreg;

                        value = dr[0];
                        if (value == DBNull.Value)
                            totsc = 0;
                        else
                            totsc = Convert.ToDouble(dr[0]);
                        strval[colcntr,rowcntr] = String.Format("{0:n2}",totsc);
                        colcntr += 1;
                        sumtotsc += totsc;

                        value = dr[2];
                        if (value == DBNull.Value)
                            lastor = 0;
                        else
                            lastor = Convert.ToInt32(dr[2]);
                        strval[colcntr,rowcntr] = lastor.ToString();
                        colcntr += 1;

                        rowcntr += 1;
                        colcntr = 0;
                    }

                    StringBuilder finalqry = new StringBuilder();

                    int bound0 = strval.GetUpperBound(0);
	                int bound1 = strval.GetUpperBound(1);
                    
	                for (int i = 0; i <= bound0; i++)
	                {
                        string sval = String.Empty;
                        switch(i){
                            case 0:
                                sval = "PERMIT NO.";
                                break;
                            case 1:
                                sval = "MIN";
                                break;
                            case 2:
                                sval = "SERIAL NO.";
                                break;
                            case 3:
                                sval = "SALES";
                                break;
                            case 4:
                                sval = "SENIOR CITIZEN";
                                break;
                            case 5:
                                sval = "LAST OR USED";
                                break;
                        }

                        finalqry.Append("insert into tmp_pos(id,pos_colnm");
                        for (int x = 0; x <= bound1; x++)
                        {
                            finalqry.AppendFormat(",{0}", "pos_col" + (x+1));
                        }
                        finalqry.AppendFormat(",pos_total,userid) values(0,'{0}'", sval);
                        for (int x = 0; x <= bound1; x++)
                        {
                            finalqry.AppendFormat(",'{0}'", strval[i, x]);
                        }
                        if (i == 3 || i == 4)
                        {
                            if (i == 3)
                                finalqry.AppendFormat(",'{0}'", sumtotreg.ToString());
                            else if (i == 4)
                                finalqry.AppendFormat(",'{0}'", sumtotsc.ToString());
                        }
                        else
                            finalqry.Append(",0");

                        finalqry.AppendFormat(",'{0}'", frm.UserCode);
                        finalqry.Append(")");

                        /*finalqry.AppendFormat("insert into tmp_pos values(0,'{0}'",sval);
	                    for (int x = 0; x <= bound1; x++)
	                    {
                            finalqry.AppendFormat(",'{0}'", strval[i, x]);
	                    }

                        if (i == 3 || i == 4)
                        {
                            if(i == 3)
                                finalqry.AppendFormat(",'{0}'", sumtotreg.ToString());
                            else if(i == 4)
                                finalqry.AppendFormat(",'{0}'", sumtotsc.ToString());
                        }
                        else
                            finalqry.Append(",''");

                        finalqry.AppendFormat(",'{0}'", frm.UserCode);
                        finalqry.Append(")");*///remarked 12.20.2017
                        MySqlCommand cmd2 = new MySqlCommand(finalqry.ToString(), myconn);
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();

                        finalqry = new StringBuilder();
	                }
                }
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
    }


}
