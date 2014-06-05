using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace aZynEManager
{
    public class clscommon
    {
        public DataTable setDataTable(string sqry, string conn)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = conn;
                myconn.Open();
                if (myconn.State == ConnectionState.Open)
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(sqry, myconn);
                    da.Fill(dt);
                    myconn.Close();
                }
            }
            catch
            {
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
    }
}
