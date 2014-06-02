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
    public partial class frmDistributor : Form
    {
        frmMain m_frmM;
        frmMovieList m_frmMList;
        MySqlConnection myconn;
        DataSet m_ds = new DataSet();
        public frmDistributor()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, frmMovieList frmlist)
        {
            m_frmM = frm;
            m_frmMList = frmlist;
            unselectbutton();
            ClearControls();

            cmbcontacts.Enabled = false;
            populatecontacts();
            //cmbcontacts.DataSource = null;
            //if (m_ds.Tables.Count > 0)
            //{
            //    cmbcontacts.DataSource = m_ds.Tables[0];
            //    cmbcontacts.DisplayMember = "name";
            //    cmbcontacts.ValueMember = "id";
            //}

            //cmbcontacts.SelectedIndex = 0;
        }

        public void populatecontacts()
        {
            string sqry = "[id] > -1";
            var foundRows = m_frmM.m_dtcontact.Select(sqry).CopyToDataTable();
            if (foundRows.Rows.Count > 0)
            {
                cmbcontacts.DataSource = foundRows;
                cmbcontacts.ValueMember = "id";
                cmbcontacts.DisplayMember = "name";
            }
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

            txtln.Text = "";
            txtfn.Text = "";
            txtaddr.Text = "";
            txtcity.Text = "";
            txtcountry.Text = "";
            txtemail.Text = "";
            txtcontactno.Text = "";
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void frmDistributor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            unselectbutton();
            string strid = String.Empty;
            //save the contact info
            if (this.txtfn.Text != "" || this.txtfn.Text != "")
            {
                string strqry = String.Format("insert into people values(0,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'); select last_insert_id();",
                    txtfn.Text.Trim() + txtln.Text.Trim(),txtln.Text.Trim(),txtfn.Text.Trim(),"",
                    txtposition.Text.Trim(),txtcontactno.Text.Trim(),txtemail.Text.Trim(),
                    txtaddr.Text.Trim(),txtcity.Text.Trim(),txtcountry.Text.Trim());

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(strqry, myconn);
                    cmd.ExecuteNonQuery();

                    strid = cmd.LastInsertedId.ToString();
                    
                    cmd.Dispose();
                    myconn.Close();
                }
                catch
                {
                    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ////insert a new distributor record
            if (txtname.Text.Trim() != "" && txtcode.Text.Trim() != "" && txtshare.Text.Trim() != "")
            {

                string sqry = String.Format("insert into distributor values(0,'{0}','{1}',{2},{3})",
                    txtcode.Text.Trim(),txtname.Text.Trim(),txtshare.Text.Trim(),strid);
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    myconn.Open();
                    if (myconn.State == ConnectionState.Open)
                    {
                        MySqlCommand cmd = new MySqlCommand(sqry, myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        myconn.Close();
                    }
                    txtcode.Text = "";
                    txtname.Text = "";
                    txtshare.Text = "";
                    ClearControls();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the distributor table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill the required(*) information.");
            }
        }

        private void cbxnew_CheckedChanged(object sender, EventArgs e)
        {
            ClearControls();
            if (cbxnew.Checked == true)
            {
                cmbcontacts.Enabled = false;
                txtname.SelectAll();
                txtname.Focus();
            }
            else
                cmbcontacts.Enabled = true;
        }

        private void cmbcontacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxnew.Checked == false)
            {
                txtln.Text = "";
                txtfn.Text = "";
                txtaddr.Text = "";
                txtcity.Text = "";
                txtcountry.Text = "";
                txtemail.Text = "";
                txtcontactno.Text = "";

                if (m_frmM.m_dtcontact.Rows.Count > 0)
                {
                    if (this.cmbcontacts.SelectedValue != null)
                    {
                        string val = cmbcontacts.SelectedValue.ToString();
                        string sqry = String.Format("[id] = '{0}'", val);
                        var foundRows = m_frmM.m_dtcontact.Select(sqry).CopyToDataTable();
                        if (foundRows.Rows.Count > 0)
                        {
                            txtln.Text = foundRows.Rows[0]["lname"].ToString();
                            txtfn.Text = foundRows.Rows[0]["fname"].ToString();
                            txtaddr.Text = foundRows.Rows[0]["address"].ToString();
                            txtcity.Text = foundRows.Rows[0]["city"].ToString();
                            txtcountry.Text = foundRows.Rows[0]["country"].ToString();
                            txtemail.Text = foundRows.Rows[0]["email_addr"].ToString();
                            txtcontactno.Text = foundRows.Rows[0]["contact_no"].ToString();
                        }
                    }
                }
            }
        }

        private void txtshare_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }


    }
}
