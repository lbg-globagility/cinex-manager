using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace aZynEManager
{
    public class clscommon
    {
        public clsConfig m_clscon = new clsConfig();

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
                sqry.Append(String.Format("{0},", getModuleID(sModule,sConnString)));
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
                    //cinema address
                    sbqry = new StringBuilder();
                    sbqry.Append("[system_code] = '002'");
                    foundRows = dt.Select(sbqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        newdt = foundRows.CopyToDataTable();
                        m_clscon.CinemaAddress = newdt.Rows[0]["system_value"].ToString();
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
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(err.Message);
            }
        }

    }


}
