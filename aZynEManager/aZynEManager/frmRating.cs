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
    public partial class frmRating : Form
    {
        frmMovieList m_frmMList;
        frmMain m_frmM;
        MySqlConnection myconn = new MySqlConnection();

        public frmRating()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frmM, frmMovieList frmmlist)
        {
            ClearControls();
            m_frmM = frmM;
            m_frmMList = frmmlist;
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_frmMList.unselectbutton();
            unselectbutton();
            ClearControls();
            this.Close();
        }

        public void ClearControls()
        {
            ControlCollection coll = (ControlCollection)this.Controls;
            foreach (Control con in coll)
            {
                if (con is TextBox)
                {
                    con.Text = "";
                }
                if (con is ComboBox)
                {
                    con.Text = "";
                    ((ComboBox)con).Items.Clear();
                }
                if (con is ListBox)
                {
                    con.Text = "";
                    ((ListBox)con).Items.Clear();
                }
            }
        }

        private void frmRating_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
