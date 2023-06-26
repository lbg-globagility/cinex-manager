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
using System.Globalization;

namespace aZynEManager
{
    public partial class frmBasePrice : Form
    {
        frmPatron m_frmPatron = null;
        frmMainUtility frmutility = null;
        frmMovieList m_frmlist = null;
        frmMain m_frmM;
        clscommon m_clscom = null;
        MySqlConnection myconn = new MySqlConnection();
        DataTable m_dt = new DataTable();

        public frmBasePrice()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls, frmPatron frmP)
        {
            m_frmM = frm;
            m_clscom = cls;
            m_frmPatron = frmP;

            txtprice.Text = "";
            txtprice.ReadOnly = true;

            refreshDGV();

            grpcontrol.Visible = true;
            unselectbutton();

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
            sbqry.Append("SELECT a.id, a.price, a.effective_date ");
            sbqry.Append("FROM ticket_prices a order by a.price asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setnormal()
        {
            txtprice.Text = "";
            txtprice.ReadOnly = true;
            dtstart.Enabled = false;

            dtstart.Value = DateTime.Now;

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
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Price";
                dgvResult.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvResult.Columns[2].Width = iwidth + 10;
                dgvResult.Columns[2].HeaderText = "Effectivity Date";
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

                txtprice.Text = "";
                txtprice.ReadOnly = false;

                dtstart.Enabled = true;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtprice.SelectAll();
                txtprice.Focus();
            }
            else
            {
                string strstatus = String.Empty;
                if (txtprice.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtprice.SelectAll();
                    txtprice.Focus();
                    return;
                }

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from ticket_prices ");
                sqry.Append(String.Format("where price = {0}", txtprice.Text.Trim()));
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

                DateTime datestart = new DateTime();
                if (dtstart.Text == "" || dtstart.Value == null)
                {
                    MessageBox.Show("Please fill the effctive date information.");
                    dtstart.Focus();
                    return;
                }
                else
                    datestart = dtstart.Value;

                sqry = new StringBuilder();
                sqry.Append(String.Format("insert into ticket_prices value(0,{0},'{1}')", 
                    txtprice.Text.Trim(), datestart.Date.ToString("yyyy-MM-dd")));
      
                try
                {
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "TICKETPRICE", "TICKET_PRICES",
                                Environment.MachineName.ToString(), "ACCESSED TICKET_PRICES MODULE: ADDED=" + txtprice.Text
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
                txtprice.ReadOnly = false;
                dtstart.Enabled = true;

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
                if (txtprice.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtprice.SelectAll();
                    txtprice.Focus();
                    return;
                }
                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from ticket_prices ");
                sqry.Append(String.Format("where price = {0} ", txtprice.Text.Trim()));
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

                DateTime datestart = new DateTime();
                if (dtstart.Text == "" || dtstart.Value == null)
                {
                    MessageBox.Show("Please fill the effctive date information.");
                    dtstart.Focus();
                    return;
                }
                else
                    datestart = dtstart.Value;

                //update the selected record
                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update ticket_prices set price = {0}, ", txtprice.Text.Trim()));
                strqry.Append(String.Format("effective_date = '{0}' " , datestart.Date.ToString("yyyy-MM-dd")));
                strqry.Append(String.Format("where id = {0}", intid));

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

                    m_clscom.AddATrail(m_frmM.m_userid, "TICKETPRICE", "TICKET_PRICES",
                                Environment.MachineName.ToString(), "ACCESSED TICKET_PRICES MODULE: UPDATED=" + txtprice.Text
                                + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the ticket prices table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    sqry.Append(String.Format("select count(*) from patrons where base_price = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    //if (rowCount > 1)
                    //melvin 10-13-2014
                    if (rowCount > 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //else if (rowCount == 1)
                    //melvin 10-13-2014
                    else if (rowCount == 0)
                    {
                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from ticket_prices where id = {0}", intid));
                        try
                        {
                            //delete value for the movies table
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            m_clscom.AddATrail(m_frmM.m_userid, "TICKETPRICE", "TICKET_PRICES",
                               Environment.MachineName.ToString(), "ACCESSED TICKET_PRICES MODULE: REMOVED=" + txtprice.Text
                               + " | ID=" + intid.ToString(), m_frmM._connection);
                            
                            MessageBox.Show("You have successfully removed \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (MySqlException er)
                        {
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            MessageBox.Show(er.Message, this.Text);
                        }
                    }

                }
            }
            refreshDGV();//melvin 10-13-2014
            setnormal();
        }

        /*private void frmSoundSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_frmPatron != null)
            {
                m_frmPatron.cmbprices.DataSource = null;
                DataView view = new DataView(m_dt);
                view.Sort = "ticket_prices";
                DataTable dtsort = view.ToTable(true, "ticket_prices", "id");
                DataRow row = dtsort.NewRow();
                row["id"] = "0";
                row["ticket_prices"] = "";
                dtsort.Rows.InsertAt(row, 0);
                m_frmPatron.cmbprices.DataSource = dtsort;
                m_frmPatron.cmbprices.ValueMember = "id";
                m_frmPatron.cmbprices.DisplayMember = "ticket_prices";
            }
        }*/

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

                txtprice.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                dtstart.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[2].Value.ToString());
            }
        }

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
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
        }

        private void frmBasePrice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_frmPatron != null)
            {
                m_frmPatron.updateCMBPrice();
                /*m_frmPatron.cmbprices.DataSource = null;
                DataView view = new DataView(m_dt);
                view.Sort = "price";
                DataTable dtsort = view.ToTable(true, "price", "id");
                m_frmPatron.cmbprices.DataSource = dtsort;
                m_frmPatron.cmbprices.ValueMember = "id";
                m_frmPatron.cmbprices.DisplayMember = "price";*/
            }
            /*else
            {
                MessageBox.Show("base price", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;

            if (dgv.CurrentRow == null)
                return;

            if (dgv.CurrentRow.Selected)
            {
                btnEdit.Enabled = true;
                btnEdit.Text = "edit";

                btnDelete.Enabled = true;
                btnDelete.Text = "remove";

                txtprice.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                dtstart.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[2].Value.ToString());
            }
        }
    }
}
