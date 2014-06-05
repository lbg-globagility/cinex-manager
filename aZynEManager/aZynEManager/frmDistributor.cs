using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmDistributor : Form
    {
        frmMain m_frmM;
        frmMovieList m_frmlist;
        MySqlConnection myconn;
        DataSet m_ds = new DataSet();
        clscommon m_clscom = null;

        DataTable m_dt = new DataTable();
        public frmDistributor()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, frmMovieList frml)
        {
            m_frmM = frm;
            m_clscom = frml.m_clscom;
            m_frmlist = frml;
            unselectbutton();
            
            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;

            dgvResult.ClearSelection();

            setnormal();
            populatecontacts();
        }

        public void frmInitII(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;

            unselectbutton();

            dgvResult.ClearSelection();

            setnormal();
            populatecontacts();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.code, a.name, b.name as contact, a.share_perc ");
            sbqry.Append("from distributor a left join people b on a.contact_id = b.id ");
            sbqry.Append("order by a.name asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.DataSource = null;
            if (dt.Rows.Count > 0)
                this.dgvResult.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgvResult.Width / 5;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Code";
                dgvResult.Columns[2].Width = iwidth * 2 - 15;
                dgvResult.Columns[2].HeaderText = "Name";
                dgvResult.Columns[3].Width = iwidth * 2;
                dgvResult.Columns[3].HeaderText = "Resource Person";
                dgvResult.Columns[4].Width = 0;
                dgvResult.Columns[4].HeaderText = "Share";
            }
        }

        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtname.Text = "";
            txtname.ReadOnly = true;
            txtshare.Text = "";
            txtshare.ReadOnly = true;
            txtln.Text = "";
            txtln.ReadOnly = true;
            txtfn.Text = "";
            txtfn.ReadOnly = true;
            txtaddr.Text = "";
            txtaddr.ReadOnly = true;
            txtcity.Text = "";
            txtcity.ReadOnly = true;
            txtcountry.Text = "";
            txtcountry.ReadOnly = true;
            txtcontactno.Text = "";
            txtcontactno.ReadOnly = true;
            txtemail.Text = "";
            txtemail.ReadOnly = true;
            txtposition.Text = "";
            txtposition.ReadOnly = true;

            if(cmbcontacts.Items.Count > 0)
                cmbcontacts.SelectedIndex = 0;
            cbxnew.Checked = true;

            btnAdd.Enabled = true;
            btnAdd.Text = "new";
            btnAdd.Values.Image = Properties.Resources.buttonadd;

            btnEdit.Enabled = false;
            btnEdit.Text = "edit";
            btnEdit.Values.Image = Properties.Resources.buttonapply;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;

            dgvResult.Enabled = true;
            grpcontrol.Enabled = true;

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
        }

        public void populatecontacts()
        {
            cmbcontacts.DataSource = null;
            DataTable dt = new DataTable();
            string sqry = "[id] > -1";
            if (m_frmM.m_dtcontact.Rows.Count == 0)
                m_frmM.m_dtcontact = m_clscom.setDataTable("select * from people order by id asc", m_frmM._connection);

            if (m_frmM.m_dtcontact.Rows.Count > 0)
            {
                var foundRows = m_frmM.m_dtcontact.Select(sqry);
                if (foundRows.Count() == 0)
                    cmbcontacts.Items.Clear();
                else
                    dt = foundRows.CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "name asc";
                    dt = dv.ToTable();

                    DataRow dr = dt.NewRow();
                    dr["id"] = 0;
                    dr["name"] = "";
                    dt.Rows.InsertAt(dr,0);
                    cmbcontacts.DataSource = dt;
                    cmbcontacts.ValueMember = "id";
                    cmbcontacts.DisplayMember = "name";
                }
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
            if (m_frmlist != null)
            {
                m_frmM.m_dtdistributor = m_clscom.setDataTable("select * from distributor order by name asc", m_frmM._connection);
                m_frmlist.populatedistributor();
            }
            //e.Cancel = true;
            //this.Hide();
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
            //ClearControls();
            if (cbxnew.Checked == true)
            {
                cmbcontacts.Enabled = false;
                txtln.SelectAll();
                txtln.Focus();

                txtln.Text = "";
                txtln.ReadOnly = false;
                txtfn.Text = "";
                txtfn.ReadOnly = false;
                txtaddr.Text = "";
                txtaddr.ReadOnly = false;
                txtcity.Text = "";
                txtcity.ReadOnly = false;
                txtcountry.Text = "";
                txtcountry.ReadOnly = false;
                txtcontactno.Text = "";
                txtcontactno.ReadOnly = false;
                txtemail.Text = "";
                txtemail.ReadOnly = false;
                txtposition.Text = "";
                txtposition.ReadOnly = false;
            }
            else
            {
                cmbcontacts.Enabled = true;
                txtln.ReadOnly = true;
                txtfn.ReadOnly = true;
                txtaddr.ReadOnly = true;
                txtcity.ReadOnly = true;
                txtcountry.ReadOnly = true;
                txtcontactno.ReadOnly = true;
                txtemail.ReadOnly = true;
                txtposition.ReadOnly = true;
            }
        }

        private void cmbcontacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
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
                        var foundRows = m_frmM.m_dtcontact.Select(sqry);
                        if(foundRows.Count() == 0)
                        {
                            txtln.Text = "";
                            txtfn.Text = "";
                            txtaddr.Text = "";
                            txtcity.Text = "";
                            txtcountry.Text = "";
                            txtemail.Text = "";
                            txtcontactno.Text = "";
                        }
                        else
                            dt = foundRows.CopyToDataTable();

                        if (dt.Rows.Count > 0)
                        {
                            txtln.Text = dt.Rows[0]["lname"].ToString();
                            txtfn.Text = dt.Rows[0]["fname"].ToString();
                            txtaddr.Text = dt.Rows[0]["address"].ToString();
                            txtcity.Text = dt.Rows[0]["city"].ToString();
                            txtcountry.Text = dt.Rows[0]["country"].ToString();
                            txtemail.Text = dt.Rows[0]["email_addr"].ToString();
                            txtcontactno.Text = dt.Rows[0]["contact_no"].ToString();
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
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
            {
                e.Handled = true;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (btnAdd.Text == "new")
            {
                dgvResult.Enabled = false;

                txtcode.Text = "";
                txtcode.ReadOnly = false;
                txtname.Text = "";
                txtname.ReadOnly = false;
                txtshare.Text = "";
                txtshare.ReadOnly = false;
                txtln.Text = "";
                txtln.ReadOnly = false;
                txtfn.Text = "";
                txtfn.ReadOnly = false;
                txtaddr.Text = "";
                txtaddr.ReadOnly = false;
                txtcity.Text = "";
                txtcity.ReadOnly = false;
                txtcountry.Text = "";
                txtcountry.ReadOnly = false;
                txtcontactno.Text = "";
                txtcontactno.ReadOnly = false;
                txtemail.Text = "";
                txtemail.ReadOnly = false;
                txtposition.Text = "";
                txtposition.ReadOnly = false;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                populatecontacts();
                cbxnew.Checked = true;

                txtcode.SelectAll();
                txtcode.Focus();
            }
            else
            {
                string strid = "0";
                string strstatus = String.Empty;
                if (txtname.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtname.SelectAll();
                    txtname.Focus();
                    return;
                }
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if (txtshare.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtshare.SelectAll();
                    txtshare.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                if (this.txtfn.Text != "" || this.txtfn.Text != "")
                {
                    if (cbxnew.Checked == true)
                    {
                        //validate for the existance of the record
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append("select count(*) from people ");
                        sqry.Append(String.Format("where name = '{0}' ", txtfn.Text.Trim() + " " + txtln.Text.Trim()));
                        sqry.Append(String.Format("and lname = '{0}' ", txtln.Text.Trim()));
                        sqry.Append(String.Format("and fname = '{0}' ", txtfn.Text.Trim()));
                        sqry.Append(String.Format("and position = '{0}' ", txtposition.Text.Trim()));
                        sqry.Append(String.Format("and contact_no = '{0}' ", txtcontactno.Text.Trim()));
                        sqry.Append(String.Format("and email_addr = '{0}' ", txtemail.Text.Trim()));
                        sqry.Append(String.Format("and address = '{0}' ", txtaddr.Text.Trim()));
                        sqry.Append(String.Format("and city = '{0}' ", txtcity.Text.Trim()));
                        sqry.Append(String.Format("and country = '{0}'", txtcountry.Text.Trim()));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount > 0)
                        {
                            setnormal();
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            MessageBox.Show("Can't add this contact person's record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string strqry = String.Format("insert into people values(0,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'); select last_insert_id();",
                            txtfn.Text.Trim() + " " + txtln.Text.Trim(), txtln.Text.Trim(), txtfn.Text.Trim(), "",
                            txtposition.Text.Trim(), txtcontactno.Text.Trim(), txtemail.Text.Trim(),
                            txtaddr.Text.Trim(), txtcity.Text.Trim(), txtcountry.Text.Trim());
                        try
                        {
                            if(myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(strqry, myconn);
                            cmd.ExecuteNonQuery();

                            strid = cmd.LastInsertedId.ToString();

                            cmd.Dispose();
                        }
                        catch
                        {
                            MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                ////insert a new distributor record
                if (txtname.Text.Trim() != "" && txtcode.Text.Trim() != "" && txtshare.Text.Trim() != "")
                {
                    if (cbxnew.Checked == false)
                        strid = cmbcontacts.SelectedValue.ToString();

                    //validate for the existance of the record
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select count(*) from distributor ");
                    sqry.Append(String.Format("where code = '{0}' ", txtcode.Text.Trim()));
                    sqry.Append(String.Format("and name = '{0}' ", txtname.Text.Trim()));
                    sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                    if(strid == "0")
                        sqry.Append(String.Format("and contact_id is null", strid));
                    else
                        sqry.Append(String.Format("and contact_id = {0}", strid));

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't add this distributor's record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sqry.Append(String.Format("insert into distributor values(0,'{0}','{1}',{2},{3})",
                        txtcode.Text.Trim(), txtname.Text.Trim(), txtshare.Text.Trim(), strid));
                    
                    try
                    {
                        if(myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        if (myconn.State == ConnectionState.Open)
                        {
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            myconn.Close();
                        }
                        txtcode.Text = "";
                        txtname.Text = "";
                        txtshare.Text = "";
                        //ClearControls();

                        refreshDGV();
                        setnormal();

                        MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't connect to the distributor table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please fill the required(*) information.");
                }

                //update combobox
                populatecontacts();


                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                

                //string sqry = String.Format("insert into classification value(0,'{0}','{1}')",
                //    txtname.Text.Trim(), txtdesc.Text.Trim());

                //myconn = new MySqlConnection();
                //myconn.ConnectionString = m_frmM._connection;
                //try
                //{
                //    myconn.Open();
                //    MySqlCommand cmd = new MySqlCommand(sqry, myconn);
                //    cmd.ExecuteNonQuery();

                //    string strid = cmd.LastInsertedId.ToString();
                //    cmd.Dispose();

                //    if (myconn.State == ConnectionState.Open)
                //        myconn.Close();

                //    refreshDGV();
                //    setnormal();
                //    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //catch
                //{
                //    if (myconn.State == ConnectionState.Open)
                //        myconn.Close();
                //    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (dgvResult.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (dgvResult.SelectedRows.Count == 1)
                dgvResult.Enabled = false;

            if (btnEdit.Text == "edit")
            {
                txtcode.ReadOnly = false;
                txtname.ReadOnly = false;
                txtshare.ReadOnly = false;
                txtln.ReadOnly = false;
                txtfn.ReadOnly = false;
                txtaddr.ReadOnly = false;
                txtcity.ReadOnly = false;
                txtcountry.ReadOnly = false;
                txtcontactno.ReadOnly = false;
                txtemail.ReadOnly = false;
                txtposition.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                if(cmbcontacts.Items.Count == 0)
                    populatecontacts();

                cbxnew.Checked = false;
            }
            else if (btnEdit.Text == "update")
            {
                string strid = "0";
                string strstatus = String.Empty;
                if (txtname.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtname.SelectAll();
                    txtname.Focus();
                    return;
                }
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if (txtshare.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtshare.SelectAll();
                    txtshare.Focus();
                    return;
                }

                StringBuilder sqry = new StringBuilder();
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                MySqlCommand cmd = new MySqlCommand();
                int rowCount = -1;

                if (this.txtfn.Text != "" || this.txtfn.Text != "")
                {
                    //validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from people ");
                    sqry.Append(String.Format("where name = '{0}' ", txtfn.Text.Trim() + " " + txtln.Text.Trim()));
                    sqry.Append(String.Format("and lname = '{0}' ", txtln.Text.Trim()));
                    sqry.Append(String.Format("and fname = '{0}' ", txtfn.Text.Trim()));
                    sqry.Append(String.Format("and position = '{0}' ", txtposition.Text.Trim()));
                    sqry.Append(String.Format("and contact_no = '{0}' ", txtcontactno.Text.Trim()));
                    sqry.Append(String.Format("and email_addr = '{0}' ", txtemail.Text.Trim()));
                    sqry.Append(String.Format("and address = '{0}' ", txtaddr.Text.Trim()));
                    sqry.Append(String.Format("and city = '{0}' ", txtcity.Text.Trim()));
                    sqry.Append(String.Format("and country = '{0}'", txtcountry.Text.Trim()));

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't add this contact person's record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cbxnew.Checked == true)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into people values(0,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'); select last_insert_id();",
                            txtfn.Text.Trim() + txtln.Text.Trim(), txtln.Text.Trim(), txtfn.Text.Trim(), "",
                            txtposition.Text.Trim(), txtcontactno.Text.Trim(), txtemail.Text.Trim(),
                            txtaddr.Text.Trim(), txtcity.Text.Trim(), txtcountry.Text.Trim()));

                        try
                        {
                            if(myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();

                            strid = cmd.LastInsertedId.ToString();

                            cmd.Dispose();
                        }
                        catch
                        {
                            MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string sid = String.Empty;
                        if (cmbcontacts.SelectedValue != null)
                        {
                            var id = cmbcontacts.SelectedValue;
                            try
                            {
                                sid = id.ToString();
                            }
                            catch
                            {
                            }
                        }

                        //Update for the people conatct
                        StringBuilder strqry2 = new StringBuilder();
                        strqry2.Append(String.Format("update people set name = '{0}'", txtfn.Text.Trim() + " " + txtln.Text.Trim()));
                        strqry2.Append(String.Format(", lname = '{0}'", txtln.Text.Trim()));
                        strqry2.Append(String.Format(", fname = '{0}'", txtfn.Text.Trim()));
                        strqry2.Append(String.Format(", address = '{0}'", txtaddr.Text.Trim()));
                        strqry2.Append(String.Format(", city = '{0}'", txtcity.Text.Trim()));
                        strqry2.Append(String.Format(", country = '{0}'", txtcountry.Text.Trim()));
                        strqry2.Append(String.Format(", contact_no = '{0}'", txtcontactno.Text.Trim()));
                        strqry2.Append(String.Format(", email_addr = '{0}'", txtemail.Text.Trim()));
                        strqry2.Append(String.Format(", position = '{0}'", txtposition.Text.Trim()));
                        strqry2.Append(String.Format(" where id = {0}", sid));

                        myconn = new MySqlConnection();
                        myconn.ConnectionString = m_frmM._connection;
                        try
                        {
                            if(myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(strqry2.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        catch
                        {
                        }
                    }
                }

                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                if (cbxnew.Checked == false)
                    strid = cmbcontacts.SelectedValue.ToString();

                //validate for the existance of the record
                sqry = new StringBuilder();
                sqry.Append("select count(*) from distributor ");
                sqry.Append(String.Format("where code = '{0}' ", txtcode.Text.Trim()));
                sqry.Append(String.Format("and name = '{0}' ", txtname.Text.Trim()));
                sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                if(strid == "0")
                    sqry.Append("and contact_id is null");
                else
                    sqry.Append(String.Format("and contact_id = {0}", intid));

                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);

                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (rowCount > 0)
                {
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't add this distributor's record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update distributor set code = '{0}'", txtcode.Text.Trim()));
                strqry.Append(String.Format(", name = '{0}'", txtname.Text.Trim()));
                strqry.Append(String.Format(", share_perc = {0}", txtshare.Text.Trim()));
                strqry.Append(String.Format(", contact_id = {0}", strid));
                strqry.Append(String.Format(" where id = {0}", intid));

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    //update the movies table
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the movies table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            unselectbutton();

            if (btnDelete.Text == "remove")
            {
                DialogResult ans = MessageBox.Show("Are you sure you want to remove \n\rthis record, continue?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == System.Windows.Forms.DialogResult.Yes)
                {
                    myconn = new MySqlConnection();
                    myconn.ConnectionString = m_frmM._connection;
                    int intid = -1;
                    if (dgvResult.SelectedRows.Count == 1)
                        intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                    //validate the database for records being used
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies where dist_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //delete from the moview table where the status is inactive or = 0
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from distributor where id = {0}", intid));
                    try
                    {
                        if(myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                    }
                    catch (MySqlException er)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show(er.Message, this.Text);
                    }
                }
            }
            
            refreshDGV();
            setnormal();
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                btnEdit.Enabled = true;
                btnEdit.Text = "edit";

                btnDelete.Enabled = true;
                btnDelete.Text = "remove";

                txtcode.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                txtname.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                txtshare.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();

                string strname = dgv.SelectedRows[0].Cells[3].Value.ToString();
                for (int i = 0; i < cmbcontacts.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbcontacts.Items[i];
                    if(drv.Row["name"].ToString().ToUpper() == strname.ToUpper())
                    {
                        cbxnew.Checked = false;
                        cmbcontacts.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


    }
}
