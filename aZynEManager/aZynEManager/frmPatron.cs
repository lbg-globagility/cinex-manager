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
    public partial class frmPatron : Form
    {
        frmMain m_frmM;
        MySqlConnection myconn;
        clscommon m_clscom = null;
        DataTable m_dt = new DataTable();

        public frmPatron()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            if (m_frmM.m_dtpatrons.Rows.Count == 0)
                m_frmM.m_dtpatrons = m_clscom.setDataTable("select * from patrons order by name asc",m_frmM._connection);

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;

            dgvResult.ClearSelection();

            setnormal();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.code, a.name, a.unit_price as unitprice, a.seat_position ");
            sbqry.Append("from patrons a ");
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
                int iwidth = dgvResult.Width / 4;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Code";
                dgvResult.Columns[2].Width = iwidth;
                dgvResult.Columns[2].HeaderText = "Name";
                dgvResult.Columns[3].Width = iwidth;
                dgvResult.Columns[3].HeaderText = "Unit Price";
                dgvResult.Columns[4].Width = iwidth;
                dgvResult.Columns[4].HeaderText = "Position";
            }
        }

        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtname.Text = "";
            txtname.ReadOnly = true;
            txtprice.Text = "";
            txtprice.ReadOnly = true;
            txtlgu.Text = "";
            txtlgu.ReadOnly = true;
            txtposition.Text = "";
            txtposition.ReadOnly = true;
            txtposition.Text = "";
            txtposition.ReadOnly = true;

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

        private void txtposition_KeyPress(object sender, KeyPressEventArgs e)
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
                txtprice.Text = "";
                txtprice.ReadOnly = false;
                txtlgu.Text = "";
                txtlgu.ReadOnly = false;
                txtposition.Text = "";
                txtposition.ReadOnly = false;
                btncolor.Enabled = false;

                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

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
                if (txtprice.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtprice.SelectAll();
                    txtprice.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                    //validate for the existance of the record
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select count(*) from patrons ");
                    sqry.Append(String.Format("where code = '{0}' ", txtcode.Text.Trim()));
                    sqry.Append(String.Format("and name = '{0}' ", txtname.Text.Trim()));
                    sqry.Append(String.Format("and unit_price = {0} ", txtprice.Text.Trim()));
                    sqry.Append(String.Format("and seat_position = {0}", txtposition.Text.Trim()));

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
                        MessageBox.Show("Can't add this record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //sqry.Append(String.Format("insert into patrons values(0,'{0}','{1}',{2},{3})",
                    //    txtcode.Text.Trim(), txtname.Text.Trim(), txtshare.Text.Trim(), strid));

                    try
                    {
                        if (myconn.State == ConnectionState.Closed)
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
                        //txtshare.Text = "";
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

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
            }
        }
    }
}
