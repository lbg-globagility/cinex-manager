using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aZynEManager
{
    public partial class frmDiscount : Form
    {
        frmPatron m_frmPatron = null;
        frmMainUtility frmutility = null;
        frmMovieList m_frmlist = null;
        frmMain m_frmM;
        clscommon m_clscom = null;
        MySqlConnection myconn = new MySqlConnection();
        DataTable m_dt = new DataTable();

        public frmDiscount()
        {
            InitializeComponent();
        }

        private void txtrate_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {


                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }

            //  if (!char.IsControl(e.KeyChar)
            //&& !char.IsDigit(e.KeyChar)
            //&& e.KeyChar != '.')
            //  {

            //      e.Handled = true;
            //  }

            //  // only allow one decimal point

            //  if (e.KeyChar == '.'
            //      && (sender as TextBox).Text.IndexOf('.') > -1)
            //  {
            //      e.Handled = true;

            //  }
            //  if (e.KeyChar == '.'
            //      && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
            //  {
            //      e.Handled = true;
            //  }
        }

        public void frmInit(frmMain frm, clscommon cls, frmPatron frmP)
        {
            m_frmM = frm;
            m_clscom = cls;
            m_frmPatron = frmP;

            /*txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtrate.Text = "0";
            txtrate.ReadOnly = true;*/

            refreshDGV();

            grpcontrol.Visible = true;
            unselectbutton();

            dgvResult.ClearSelection();

            setnormal();
        }

        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtrate.Text = "";
            txtrate.ReadOnly = true;

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

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.discount_code, a.discount_rate ");
            sbqry.Append("from discount_table a ");
            sbqry.Append("order by a.discount_code asc");
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
                int iwidth = dgvResult.Width / 3;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[0].Visible = false;
                dgvResult.Columns[1].Width = iwidth + 10;
                dgvResult.Columns[1].HeaderText = "Code";
                dgvResult.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvResult.Columns[2].Width = iwidth + 10;
                dgvResult.Columns[2].HeaderText = "Rate";
                dgvResult.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            unselectbutton();
            if (btnAdd.Text == "new")
            {
                dgvResult.Enabled = false;

                txtcode.Text = "";
                txtcode.ReadOnly = false;

                txtrate.Text = "";
                txtrate.ReadOnly = false;

                unselectbutton();
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
                string strstatus = String.Empty;
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }

                if (txtrate.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtrate.SelectAll();
                    txtrate.Focus();
                    return;
                }

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from discount_table ");
                sqry.Append(String.Format("where discount_code = '{0}' ", txtcode.Text.Trim()));
                sqry.Append(String.Format("and discount_rate = {0}", txtrate.Text.Trim()));
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

                sqry = new StringBuilder();
                sqry.Append(String.Format("insert into discount_table value(0,'{0}',{1})",
                    txtcode.Text.Trim(), txtrate.Text.Trim()));

                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "DISCOUNTCODE", "DISCOUNT_TABLE",
                                Environment.MachineName.ToString(), "ACCESSED DISCOUNT_TABLE MODULE: ADDED=" + txtcode.Text
                                + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmDiscount_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_frmPatron != null)
            {
                m_frmPatron.updateCMBDisc();
                /*m_frmPatron.cmbdisc.DataSource = null;
                DataView view = new DataView(m_dt);
                view.Sort = "discount_code";
                DataTable dtsort = view.ToTable(true, "discount_code", "id");
                DataRow row = dtsort.NewRow();
                row["id"] = "0";
                row["discount_code"] = "";
                dtsort.Rows.InsertAt(row, 0);
                m_frmPatron.cmbdisc.DataSource = dtsort;
                m_frmPatron.cmbdisc.ValueMember = "id";
                m_frmPatron.cmbdisc.DisplayMember = "discount_code";*/
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

                    /*//validate the database for records being used
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from patrons where base_price = {0}", intid));
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

                    else if (rowCount == 0)
                    {*/
                    //delete from the moview table where the status is inactive or = 0
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from discount_table where id = {0}", intid));
                    try
                    {
                        //delete value for the movies table
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        m_clscom.AddATrail(m_frmM.m_userid, "DICOUNTCODE", "DISCOUNT_TABLE",
                            Environment.MachineName.ToString(), "ACCESSED DISCOUNT_TABLE MODULE: REMOVED=" + txtcode.Text
                            + " | ID=" + intid.ToString(), m_frmM._connection);

                        MessageBox.Show("You have successfully removed \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (MySqlException er)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show(er.Message, this.Text);
                    }
                   // }

                }
            }
            refreshDGV();//melvin 10-13-2014
            setnormal();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
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
                txtrate.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";
            }
            else if (btnEdit.Text == "update")
            {
                string strstatus = String.Empty;
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if (txtrate.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtrate.SelectAll();
                    txtrate.Focus();
                    return;
                }

                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from discount_table ");
                sqry.Append(String.Format("where discount_code = '{0}' ", txtcode.Text.Trim()));
                sqry.Append(String.Format("and discount_rate = {0} ", txtrate.Text.Trim()));
                sqry.Append(String.Format("and id != {0}", intid));
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

                //update the selected record
                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update discount_table set discount_code = '{0}', ", txtcode.Text.Trim()));
                strqry.Append(String.Format("discount_rate = {0} ", txtrate.Text.Trim()));
                strqry.Append(String.Format("where id = {0}", intid));

                try
                {
                    //update the table
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "DISCOUNTCODE", "DISCOUNT_TABLE",
                                Environment.MachineName.ToString(), "ACCESSED DISCOUNT_TABLE MODULE: UPDATED=" + txtcode.Text
                                + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the discount table table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                txtrate.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
            }
        }
    }
}
