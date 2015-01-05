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
    public partial class frmPatron : Form
    {
        frmMain m_frmM;
        MySqlConnection myconn;
        clscommon m_clscom = null;
        DataTable m_dt = new DataTable();
        //melvin
        int m_val = 0;
        int m_dot = 0;
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
                refreshpatrons();

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

        public void refreshpatrons()
        {
            m_frmM.m_dtpatrons = m_clscom.setDataTable("select * from patrons order by name asc", m_frmM._connection);
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
            lbllgu.Visible = false;
            txtlgu.Text = "0.00";
            txtlgu.Visible = false;
            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtname.Text = "";
            txtname.ReadOnly = true;
            txtprice.Text = "";
            txtprice.ReadOnly = true;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PATRON_ADD", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

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
                txtlgu.Text = "0.00";
                txtlgu.ReadOnly = false;
                txtposition.Text = "";
                txtposition.ReadOnly = false;
                btncolor.Enabled = true;

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
                //RMB 1.5.2015
                if (txtprice.Text != "")
                {
                    int cntr = 0;
                    int i = 0;
                    while ((i = txtprice.Text.IndexOf('.', i)) != -1)
                    {
                        i++;
                        cntr += 1;
                    }
                    if (cntr > 1)
                    {
                        MessageBox.Show("Please check the price information.");
                        txtprice.SelectAll();
                        txtprice.Focus();
                        return;
                    }
                }

                //melvin 10-5-2014
                if (txtposition.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtposition.SelectAll();
                    txtposition.Focus();
                    return;
                }
                if (txtposition.Text != "")
                {
                    int cntr = 0;
                    int i = 0;
                    while ((i = txtposition.Text.IndexOf('.', i)) != -1)
                    {
                        i++;
                        cntr += 1;
                    }
                    if (cntr > 0)
                    {
                        MessageBox.Show("Please check the position information.");
                        txtposition.SelectAll();
                        txtposition.Focus();
                        return;
                    }
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                /*
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from patrons ");
                sqry.Append(String.Format("where code = '{0}' ", txtcode.Text.Trim()));
                sqry.Append(String.Format("and name = '{0}' ", txtname.Text.Trim()));
                sqry.Append(String.Format("and unit_price = {0} ", txtprice.Text.Trim()));
                sqry.Append(String.Format("and seat_position = {0}", txtposition.Text.Trim()));
                */
                //melvin 11-5-2014 for sql injection
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from patrons where code = @code and name = @name ");
                sqry.Append(String.Format("and unit_price = {0} ", txtprice.Text.Trim()));
                sqry.Append(String.Format("and seat_position = {0}", txtposition.Text.Trim()));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim());
                cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
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

                
               // sqry.Append("Select Max(id) from patrons");
               // MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
                //int max_id= Convert.ToInt32(cmd2.ExecuteScalar())+1;
                sqry = new StringBuilder();
                //with queries
                /*
                sqry.Append(String.Format("insert into patrons values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})",
                    0,txtcode.Text.Trim(), txtname.Text.Trim(), txtprice.Text.Trim(), Convert.ToInt32(cbxpromo.CheckState),
                    Convert.ToInt32(cbxamusement.CheckState), Convert.ToInt32(cbxcultural.CheckState), Convert.ToInt32(cbxlgu.CheckState),//7
                    Convert.ToInt32(cbxgross.CheckState), Convert.ToInt32(cbxproducer.CheckState), clscommon.Get0BGR(btncolor.SelectedColor),//btncolor.SelectedColor.ToArgb(),
                    txtposition.Text.Trim(), txtlgu.Text.Trim()));*/
                //melvin 11-5-2014 for sql injection

                sqry.Append("insert into patrons values(0,@code,@name,@price,@promo,@amusement,");
                sqry.Append("@cultural,@lgubox,@gross,@producer,@color,@position,@lgu)");
               
                try
                {
                    int intid = -1;
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    if (myconn.State == ConnectionState.Open)
                    {
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.Parameters.AddWithValue("@code",txtcode.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                        cmd.Parameters.AddWithValue("@price", txtprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@promo", Convert.ToInt32(cbxpromo.CheckState));
                        cmd.Parameters.AddWithValue("@amusement", Convert.ToInt32(cbxamusement.CheckState));
                        cmd.Parameters.AddWithValue("@cultural", Convert.ToInt32(cbxcultural.CheckState));
                        cmd.Parameters.AddWithValue("@lgubox", Convert.ToInt32(cbxlgu.CheckState));
                        cmd.Parameters.AddWithValue("@gross", Convert.ToInt32(cbxgross.CheckState));
                        cmd.Parameters.AddWithValue("@producer", Convert.ToInt32(cbxproducer.CheckState));
                        cmd.Parameters.AddWithValue("@color", clscommon.Get0BGR(btncolor.SelectedColor));
                        cmd.Parameters.AddWithValue("@position", txtposition.Text.Trim());
                        cmd.Parameters.AddWithValue("@lgu", txtlgu.Text.Trim());
                        cmd.ExecuteNonQuery();
                        intid = Convert.ToInt32(cmd.LastInsertedId);
                        cmd.Dispose();
                        myconn.Close();
                    }

                    m_clscom.AddATrail(m_frmM.m_userid, "PATRON_ADD", "PATRONS",
                            Environment.MachineName.ToString(), "ADDED NEW PATRON INFO: NAME=" + this.txtname.Text
                            + " | ID=" + intid.ToString(), m_frmM._connection);

                    txtcode.Text = "";
                    txtname.Text = "";
                    txtprice.Text = "";

                    refreshpatrons();
                    refreshDGV();
                    setnormal();
                    chkButton(false);
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the patrons table."+err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PATRON_EDIT", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }
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
                txtprice.ReadOnly = false;
                txtlgu.ReadOnly = false;
                txtposition.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";
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
                if (txtprice.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtprice.SelectAll();
                    txtprice.Focus();
                    return;
                }
                //RMB 1.5.2015
                if (txtprice.Text != "")
                {
                    int cntr = 0;
                    int i = 0;
                    while ((i = txtprice.Text.IndexOf('.', i)) != -1)
                    {
                        i++;
                        cntr += 1;
                    }
                    if (cntr > 1)
                    {
                        MessageBox.Show("Please check the price information.");
                        txtprice.SelectAll();
                        txtprice.Focus();
                        return;
                    }
                }

                StringBuilder sqry = new StringBuilder();
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                MySqlCommand cmd = new MySqlCommand();
                int rowCount = -1;

                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //validate for the existance of the record
                sqry = new StringBuilder();
                sqry.Append("select count(*) from patrons");
                sqry.Append(String.Format(" where code = '{0}'", txtcode.Text.Trim()));
                sqry.Append(String.Format(" and name = '{0}'", txtname.Text.Trim()));
                sqry.Append(String.Format(" and unit_price = {0}", txtprice.Text.Trim()));
                sqry.Append(String.Format(" and with_promo = {0}", Convert.ToInt32(cbxpromo.CheckState)));
                sqry.Append(String.Format(" and with_amusement = {0}", Convert.ToInt32(cbxamusement.CheckState)));
                sqry.Append(String.Format(" and with_cultural = {0}", Convert.ToInt32(cbxcultural.CheckState)));
                sqry.Append(String.Format(" and with_citytax = {0}", Convert.ToInt32(cbxlgu.CheckState)));
                sqry.Append(String.Format(" and with_gross_margin = {0}", Convert.ToInt32(cbxgross.CheckState)));
                sqry.Append(String.Format(" and with_prod_share = {0}", Convert.ToInt32(cbxproducer.CheckState)));
                sqry.Append(String.Format(" and seat_color = {0}", clscommon.Get0BGR(btncolor.SelectedColor)));//Convert.ToInt32(btncolor.SelectedColor.ToArgb())));
                sqry.Append(String.Format(" and seat_position = {0}", Convert.ToInt32(txtposition.Text.Trim())));
                sqry.Append(String.Format(" and lgutax = {0} ", txtlgu.Text.Trim()));
                sqry.Append(String.Format(" and id != {0} ", intid));

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
                    MessageBox.Show("Can't add this patrons record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update patrons set code = '{0}'", txtcode.Text.Trim()));
                strqry.Append(String.Format(", name = '{0}'", txtname.Text.Trim()));
                strqry.Append(String.Format(", unit_price = {0}", txtprice.Text.Trim()));
                strqry.Append(String.Format(", with_promo = {0}", Convert.ToInt32(cbxpromo.CheckState)));
                strqry.Append(String.Format(", with_amusement = {0}", Convert.ToInt32(cbxamusement.CheckState)));
                strqry.Append(String.Format(", with_cultural = {0}", Convert.ToInt32(cbxcultural.CheckState)));
                strqry.Append(String.Format(", with_citytax = {0}", Convert.ToInt32(cbxlgu.CheckState)));
                strqry.Append(String.Format(", with_gross_margin = {0}", Convert.ToInt32(cbxgross.CheckState)));
                strqry.Append(String.Format(", with_prod_share = {0}", Convert.ToInt32(cbxproducer.CheckState)));
                strqry.Append(String.Format(", seat_color = {0}", clscommon.Get0BGR(btncolor.SelectedColor)));//Convert.ToInt32(btncolor.SelectedColor.ToArgb())));
                strqry.Append(String.Format(", seat_position = {0}", Convert.ToInt32(txtposition.Text.Trim())));
                strqry.Append(String.Format(", lgutax = {0}", txtlgu.Text.Trim()));
                strqry.Append(String.Format(" where id = {0}", intid));

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    //update the movies table
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "PATRON_EDIT", "PATRONS",
                            Environment.MachineName.ToString(), "UPDATED PATRON INFO: NAME=" + this.txtname.Text
                            + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshpatrons();
                    refreshDGV();
                    setnormal();
                    chkButton(false);
                    
                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the patrons table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void chkButton(bool con)
        {
            cbxamusement.Checked = con;
            cbxcultural.Checked = con;
            cbxgross.Checked = con;
            cbxlgu.Checked = con;
            cbxproducer.Checked = con;
            cbxpromo.Checked = con;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PATRON_DELETE", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }
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
                    sqry.Append(String.Format("select count(*) from cinema_patron where patron_id = {0}", intid));
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

                    //validate the database for records being used
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies_schedule_list_patron where patron_id = {0}", intid));
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
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                    //delete from the moview table where the status is inactive or = 0
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from patrons where id = {0}", intid));
                    try
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        m_clscom.AddATrail(m_frmM.m_userid, "PATRON_DELETE", "PATRONS|CINEMA_PATRON|MOVIES_SCHEDULE_LIST_PATRON",
                            Environment.MachineName.ToString(), "REMOVED PATRON INFO: NAME=" + this.txtname.Text
                            + " | ID=" + intid.ToString(), m_frmM._connection);

                        refreshpatrons();
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

            refreshDGV();
            setnormal();
            refreshpatrons();
        }

        private void cbxlgu_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxlgu.Checked == true)
            {
                lbllgu.Visible = true;
                txtlgu.Text = "0.00";
                txtlgu.Visible = true;
                txtlgu.SelectAll();
                txtlgu.Focus();
            }
            else
            {
                lbllgu.Visible = false;
                txtlgu.Text = "0.00";
                txtlgu.Visible = false;
            }
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = new DataTable();
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
                //melvin 10-10-14
                txtprice.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();
                txtposition.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();

                string strid = dgv.SelectedRows[0].Cells[0].Value.ToString();
                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("[id] = {0}", strid));
                var foundRows = m_frmM.m_dtpatrons.Select(sqry.ToString());
                if (foundRows.Count() != 0)
                    dt = foundRows.CopyToDataTable();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["with_promo"].ToString() == "1" ? cbxpromo.Checked = true : cbxpromo.Checked = false) ;
                    if (dt.Rows[0]["with_amusement"].ToString() == "1" ? cbxamusement.Checked = true : cbxamusement.Checked = false) ;
                    if (dt.Rows[0]["with_cultural"].ToString() == "1" ? cbxcultural.Checked = true : cbxcultural.Checked = false) ;
                    if (dt.Rows[0]["with_citytax"].ToString() == "1" ? cbxlgu.Checked = true : cbxlgu.Checked = false) ;
                    if (dt.Rows[0]["with_gross_margin"].ToString() == "1" ? cbxgross.Checked = true : cbxgross.Checked = false) ;
                    if (dt.Rows[0]["with_prod_share"].ToString() == "1" ? cbxproducer.Checked = true : cbxproducer.Checked = false) ;

                    txtlgu.Text = dt.Rows[0]["with_citytax"].ToString();
                    int intcolor = (Convert.ToInt32(dt.Rows[0]["seat_color"]));

                    btncolor.SelectedColor = clscommon.From0BGR(intcolor);//m_clscom.UIntToColor(intcolor);//Color.FromArgb(intcolor);
                }
            }
        }

        private void txtlgu_KeyPress(object sender, KeyPressEventArgs e )
        {


            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

                //// only allow one decimal point
                //if (e.KeyChar == '.')
                //{
                //    e.Handled = true;
                    
                //}

                //if (e.KeyChar == '.'
                //    && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
                //{
                //    e.Handled = true;
                //}
            
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btncolor_SelectedColorChanged(object sender, ColorEventArgs e)
        {

        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            ////melvin 10-28-2014
            //try
            //{
            //    if (txtprice.Text.Length > 0)
            //    {
            //        Convert.ToInt32(txtprice.Text);
            //        int.TryParse(txtprice.Text, out m_val);
            //        int textLength = txtprice.Text.Length;
            //        txtprice.SelectionStart = textLength;
            //        txtprice.SelectionLength = 0;
            //    }
            //}
            //catch
            //{
            //    txtprice.Text = m_val.ToString();
            //    return;
            //}

        }

        private void txtlgu_TextChanged(object sender, EventArgs e)
        {
            int dot = 0;
            for (int x = 0; x < txtlgu.Text.Length; x++)
            {
                if (txtlgu.Text.Substring(x, 1) == ".")
                {
                    dot++;
                }
                if (dot > 1)
                {
                    txtlgu.Text = txtlgu.Text.Substring(0, x);
                    txtlgu.SelectionStart = txtlgu.Text.Length;
                    txtlgu.SelectionLength = 0;
                }

            }
        }

        private void txtlgu_Leave(object sender, EventArgs e)
        {
            //melvin 10-30 for auto add .00
            if (txtlgu.Text.Contains(".") == false)
            {
                    txtlgu.Text = txtlgu.Text + ".00";
            }
            else
            {
               if (txtlgu.Text.IndexOf(".") == txtlgu.Text.Length-1)
                {
                    txtlgu.Text = txtlgu.Text + "00";
                }
            }
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
