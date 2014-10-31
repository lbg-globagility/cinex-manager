using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace aZynEManager
{
    public partial class frmConfig : Form
    {

        clscommon m_clscom = null;
        frmMain m_frmM = null;

        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_clscom = cls;
            m_frmM = frm;

            propertyGrid1.SelectedObject = m_clscom.m_clscon;
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void btnapply_Click(object sender, EventArgs e)
        {
            unselectbutton();
            try
            {
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                //for cinema name
                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.CinemaName));
                sqry.Append(String.Format("where system_code = '{0}' ", "001"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for cinema address
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.CinemaAddress));
                sqry.Append(String.Format("where system_code = '{0}' ", "002"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for report subheader 1
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.ReportSubHeader1));
                sqry.Append(String.Format("where system_code = '{0}' ", "003"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for report subheader 2
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.ReportSubHeader2));
                sqry.Append(String.Format("where system_code = '{0}' ", "004"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for report subheader 3
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.ReportSubHeader3));
                sqry.Append(String.Format("where system_code = '{0}' ", "005"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for movie default share
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.MovieDefaultShare));
                sqry.Append(String.Format("where system_code = '{0}' ", "006"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for movie list cut off date
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.MovieListCutOffDate));
                sqry.Append(String.Format("where system_code = '{0}' ", "007"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                //for movie default intermission time
                sqry = new StringBuilder();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.MovieListCutOffDate));
                sqry.Append(String.Format("where system_code = '{0}' ", "008"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sqry.Clear();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.TIN));
                sqry.Append(String.Format("where system_code = '{0}' ", "009"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sqry.Clear();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.PN));
                sqry.Append(String.Format("where system_code = '{0}' ", "010"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sqry.Clear();
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", m_clscom.m_clscon.Printer));
                sqry.Append(String.Format("where system_code = '{0}' ", "011"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sqry.Clear();
                string strRemoveReserved = "NO";
                if (m_clscom.m_clscon.IsRemoveReservedSeat)
                    strRemoveReserved = "YES";
                
                sqry.Append(String.Format("update config_table set system_value = '{0}' ", strRemoveReserved));
                sqry.Append(String.Format("where system_code = '{0}' ", "012"));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                m_clscom.initConfig(m_frmM);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,"system config table");
            }
        }

        private void frmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_clscom.initConfig(m_frmM);
        }
    }
}
