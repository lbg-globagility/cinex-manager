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
        frmPatronSearch m_frmPS;
        frmMain m_frmM;
        MySqlConnection myconn;
        clscommon m_clscom = null;
        DataTable m_dt = new DataTable();
        DataTable m_orddt = new DataTable();//3.23.2015
        DataTable m_ticketdt = new DataTable();//5.26.2015
        DataTable m_surdt = new DataTable();//6.8.2015
        //melvin
        int m_val = 0;
        int m_dot = 0;
        public frmPatron()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls, frmPatronSearch frmparent = null)
        {
            m_frmPS = frmparent;
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            if (m_frmM.m_dtpatrons.Rows.Count == 0)
                refreshpatrons();

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;

            dgvResult.ClearSelection();
            dgvOrdinance.ClearSelection();//3.26.2015
            dgvSurcharge.ClearSelection();//6.8.2015

            cmbprices.Items.Clear();
            StringBuilder sqry = new StringBuilder();
            sqry.Append("select id,price,effective_date from ticket_prices order by price asc");
            m_ticketdt = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);

            sqry = new StringBuilder();
            sqry.Append("select id,price from ticket_prices order by price asc");
            DataTable dt = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
            DataRow row = dt.NewRow();
            //row["id"] = "0";
            //row["price"] = ;
            //dt.Rows.InsertAt(row, 0);
            cmbprices.DataSource = dt;
            cmbprices.DisplayMember = "price";
            cmbprices.ValueMember = "id";

            //added7.1.2019
            sqry = new StringBuilder();
            sqry.Append("select id,discount_code from discount_table order by discount_code asc");
            DataTable dt1 = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
            DataRow row1 = dt1.NewRow();
            row1["id"] = "0";
            row1["discount_code"] = "";
            dt1.Rows.InsertAt(row1, 0);
            cmbdisc.DataSource = dt1;
            cmbdisc.DisplayMember = "discount_code";
            cmbdisc.ValueMember = "id";

            setnormal();
        }

        public void updateCMBDisc()
        {
            //added 7.1.2019
            //cmbdisc.DataSource = null;
            StringBuilder sqry = new StringBuilder();
            sqry.Append("select id,discount_code from discount_table order by discount_code asc");
            DataTable dt1 = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
            DataRow row1 = dt1.NewRow();
            row1["id"] = "0";
            row1["discount_code"] = "";
            dt1.Rows.InsertAt(row1, 0);
            cmbdisc.DataSource = dt1;
            cmbdisc.DisplayMember = "discount_code";
            cmbdisc.ValueMember = "id";
        }

        public void updateCMBPrice()
        {
            //added 7.1.2019
            //cmbprices.DataSource = null;
            StringBuilder sqry = new StringBuilder();
            sqry.Append("select id,price from ticket_prices order by price asc");
            DataTable dt = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
            DataRow row = dt.NewRow();
            //row["id"] = "0";
            //row["price"] = ;
            //dt.Rows.InsertAt(row, 0);
            cmbprices.DataSource = dt;
            cmbprices.DisplayMember = "price";
            cmbprices.ValueMember = "id";
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            //sbqry.Append("select a.id, a.code, a.name, a.unit_price as unitprice, a.seat_position ");
            //sbqry.Append("from patrons a ");
            //sbqry.Append("order by a.name asc");
            sbqry.Append("select a.id, a.code, a.name, a.unit_price as unitprice, ");
            sbqry.Append("a.seat_position, b.price as baseprice, b.effective_date ");
            sbqry.Append("from patrons a, ticket_prices b ");
            sbqry.Append("where a.base_price = b.id ");
            sbqry.Append("order by a.name asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);

            //3.23.2015
            sbqry = new StringBuilder();
            sbqry.Append("select a.ordinance_no, a.amount_val, a.id ");
            sbqry.Append("from ordinance_tbl a ");
            sbqry.Append("order by a.ordinance_no asc");
            m_orddt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridViewII(m_orddt);

            //6.8.2015
            sbqry = new StringBuilder();
            sbqry.Append("select a.code, a.amount_val, a.id ");
            sbqry.Append("from surcharge_tbl a ");
            sbqry.Append("order by a.code asc");
            m_surdt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridViewIII(m_surdt);
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
                dgvResult.Columns[0].Visible = false;
                dgvResult.Columns[1].Width = iwidth / 2 + 5;
                dgvResult.Columns[1].HeaderText = "Code";
                dgvResult.Columns[2].Width = iwidth * 2 - 15;
                dgvResult.Columns[2].HeaderText = "Name";
                dgvResult.Columns[3].Width = iwidth / 2;
                dgvResult.Columns[3].HeaderText = "Unit Price";
                dgvResult.Columns[4].Width = iwidth / 2;
                dgvResult.Columns[4].HeaderText = "Position";
                dgvResult.Columns[5].Width = iwidth / 2;
                dgvResult.Columns[5].HeaderText = "Base Price";
                dgvResult.Columns[6].Width = iwidth / 2 + 5;
                dgvResult.Columns[6].HeaderText = "Effectivity Date";
            }
        }

        public void setDataGridViewII(DataTable dt)
        {
            this.dgvOrdinance.DataSource = null;
            //if (dt.Rows.Count > 0)
            //    this.dgvOrdinance.DataSource = dt;
            this.dgvOrdinance.Columns.Clear();
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                int iwidth = (dgvOrdinance.Width) / 2;
                dgvOrdinance.DataSource = dt;
                dgvOrdinance.ColumnHeadersHeight = 25;
                dgvOrdinance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvOrdinance.Columns[0].Width = iwidth - 15;
                dgvOrdinance.Columns[0].HeaderText = "Ordinance No";
                dgvOrdinance.Columns[1].Width = iwidth - 40;
                dgvOrdinance.Columns[1].HeaderText = "Value";
                dgvOrdinance.Columns[2].Width = 0;
                dgvOrdinance.Columns[2].HeaderText = "ID";
                dgvOrdinance.Columns[2].Visible = false;
                dgvOrdinance.Columns.Insert(0, cbx);
                
            }
        }

        public void setDataGridViewIII(DataTable dt)
        {
            this.dgvSurcharge.DataSource = null;
            this.dgvSurcharge.Columns.Clear();
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                int iwidth = (dgvOrdinance.Width) / 2;
                dgvSurcharge.DataSource = dt;
                dgvSurcharge.ColumnHeadersHeight = 25;
                dgvSurcharge.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvSurcharge.Columns[0].Width = iwidth - 15;
                dgvSurcharge.Columns[0].HeaderText = "Surcharge Code";
                dgvSurcharge.Columns[1].Width = iwidth - 40;
                dgvSurcharge.Columns[1].HeaderText = "Value";
                dgvSurcharge.Columns[2].Width = 0;
                dgvSurcharge.Columns[2].HeaderText = "ID";
                dgvSurcharge.Columns[2].Visible = false;
                dgvSurcharge.Columns.Insert(0, cbx);
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

            dgvOrdinance.Enabled = true;//3.23.2015
            dtstart.Value = DateTime.Now;
            if(cmbprices.Items.Count > 0)
                cmbprices.SelectedIndex = 0;

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);

            txtdiscount.Text = "0";
            txtdiscount.ReadOnly = true;
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

                txtdiscount.ReadOnly = false;
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

                sqry.Append("insert into patrons (`id`, `code`, `name`, `unit_price`, `with_promo`, `with_amusement`, `with_cultural`, `with_citytax`, `with_gross_margin`, `with_prod_share`, `seat_color`, `seat_position`, `lgutax`, `base_price`, `with_surcharge`) values(0,@code,@name,@price,@promo,@amusement,");
                sqry.Append("@cultural,@lgubox,@gross,@producer,@color,@position,@lgu,@baseprice,@surcharge)");
               
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
                        cmd.Parameters.AddWithValue("@baseprice",cmbprices.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@surcharge", Convert.ToInt32(cbxSurcharge.CheckState));
                        cmd.ExecuteNonQuery();
                        intid = Convert.ToInt32(cmd.LastInsertedId);
                        cmd.Dispose();
                        myconn.Close();
                    }

                    //3.24.2015 added new record to patrons_ordinance
                    insertPatronOrdinanceCheck(myconn, dgvOrdinance, intid);

                    //6.8.2015 added new record to patron_surcharge
                    insertPatronSurchargeCheck(myconn, dgvSurcharge, intid);

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

        public void insertPatronOrdinanceCheck(MySqlConnection myconn, DataGridView dgv, int intid)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv[0, i].Value != null)
                {
                    if ((bool)dgv[0, i].Value)
                    {
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into patrons_ordinance values(0,{0},{1})",
                            intid, dgv[3, i].Value.ToString()));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
        }

        public void insertPatronSurchargeCheck(MySqlConnection myconn, DataGridView dgv, int intid)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv[0, i].Value != null)
                {
                    if ((bool)dgv[0, i].Value)
                    {
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into patrons_surcharge values(0,{0},{1})",
                            intid, dgv[3, i].Value.ToString()));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
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
                txtdiscount.ReadOnly = false;
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
                sqry.Append(String.Format(" and base_price = {0} ", cmbprices.SelectedValue.ToString()));
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

                //validate patron being used by another table 3.24.2015
                sqry = new StringBuilder();
                sqry.Append("select count(*) from movies_schedule_list_patron where patron_id = @patronid");
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.Parameters.AddWithValue("@patronid", intid);
                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                StringBuilder strqry = new StringBuilder();
                if (rowCount > 0)
                {
                    /*2.5.2016 update to allow editing of patron name and seat color*/
                    strqry = new StringBuilder();
                    strqry.Append(String.Format("update patrons set name = '{0}'", txtname.Text.Trim()));
                    strqry.Append(String.Format(", seat_color = {0}", clscommon.Get0BGR(btncolor.SelectedColor)));//Convert.ToInt32(btncolor.SelectedColor.ToArgb())));
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

                        refreshpatrons();
                        refreshDGV();
                        setnormal();
                        chkButton(false);
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        return;
                    }
                    catch
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                    }



                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't update this patrons record, \n\rit is already being used by another table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                strqry = new StringBuilder();
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
                strqry.Append(String.Format(", base_price = {0} ", cmbprices.SelectedValue.ToString()));
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


                    //3.24.2015 added new record to patrons_ordinance (start)
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from patrons_ordinance where patron_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount2 > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from patrons_ordinance where patron_id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    insertPatronOrdinanceCheck(myconn, dgvOrdinance, intid);
                    //3.24.2015 added new record to patrons_ordinance (end)

                    //6.8.2015 added new record to patrons_surcharge (start)
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from patrons_surcharge where patron_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount3 = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount3 > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from patrons_surcharge where patron_id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    insertPatronSurchargeCheck(myconn, dgvSurcharge, intid);
                    //6.8.2015 added new record to patrons_surcharge (end)

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
            //if (cbxlgu.Checked == true)
            //{
            //    lbllgu.Visible = true;
            //    txtlgu.Text = "0.00";
            //    txtlgu.Visible = true;
            //    txtlgu.SelectAll();
            //    txtlgu.Focus();
            //}
            //else
            //{
            //    lbllgu.Visible = false;
            //    txtlgu.Text = "0.00";
            //    txtlgu.Visible = false;
            //}

            for (int i = 0; i < dgvOrdinance.Rows.Count; i++)
            {
                if (dgvOrdinance[0, i].Value != null)
                {
                    if ((bool)dgvOrdinance[0, i].Value)
                        dgvOrdinance[0, i].Value = (object)false;
                }
            }//added 7.1.2019

            if (cbxlgu.Checked == true)
            {
                pageOrdinance.Enabled = true;
                dgvOrdinance.Enabled = true;
            }
            else
            {
                pageOrdinance.Enabled = false;
                dgvOrdinance.Enabled = false;
            }

        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = new DataTable();
            onDGVSelect(sender as KryptonDataGridView, dt);
            /*KryptonDataGridView dgv = sender as KryptonDataGridView;
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
                    bool boolok = false;//added 6.8.2015
                    boolok = (dt.Rows[0]["with_promo"].ToString() == "1" ? cbxpromo.Checked = true : cbxpromo.Checked = false) ;
                    boolok = (dt.Rows[0]["with_amusement"].ToString() == "1" ? cbxamusement.Checked = true : cbxamusement.Checked = false);
                    boolok = (dt.Rows[0]["with_cultural"].ToString() == "1" ? cbxcultural.Checked = true : cbxcultural.Checked = false);
                    boolok = (dt.Rows[0]["with_citytax"].ToString() == "1" ? cbxlgu.Checked = true : cbxlgu.Checked = false);
                    boolok = (dt.Rows[0]["with_gross_margin"].ToString() == "1" ? cbxgross.Checked = true : cbxgross.Checked = false);
                    boolok = (dt.Rows[0]["with_prod_share"].ToString() == "1" ? cbxproducer.Checked = true : cbxproducer.Checked = false);
                    boolok = (dt.Rows[0]["with_surcharge"].ToString() == "1" ? cbxSurcharge.Checked = true : cbxSurcharge.Checked = false);//new for surcharge 6.8.2015

                    txtlgu.Text = dt.Rows[0]["with_citytax"].ToString();
                    int intcolor = (Convert.ToInt32(dt.Rows[0]["seat_color"]));

                    btncolor.SelectedColor = clscommon.From0BGR(intcolor);//m_clscom.UIntToColor(intcolor);//Color.FromArgb(intcolor);

                    if (m_ticketdt.Rows.Count > 0)
                    {
                        cmbprices.SelectedValue = dt.Rows[0]["base_price"];
                        //dtstart.Value = Convert.ToDateTime(dt.Rows[0]["effective_date"].ToString());
                    }
                }

                //3.23.2015 START
                setCheck(dgvOrdinance, false);

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                DataTable dt2 = new DataTable();
                sqry = new StringBuilder();
                sqry.Append("select ordinance_id from patrons_ordinance where patron_id = @patid");
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = myconn;
                if(cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                cmd.CommandText = sqry.ToString();
                cmd.Parameters.AddWithValue("@patid", strid);
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        int intid = Convert.ToInt32(rd[0].ToString());
                        for (int i = 0; i < dgvOrdinance.Rows.Count; i++)
                        {
                            if (intid == Convert.ToInt32(dgvOrdinance[3, i].Value.ToString()))
                                dgvOrdinance[0, i].Value = (object)true;
                        }
                    }
                }
                rd.Close();
                cmd.Dispose();

                //3.23.2015 END

                //6.8.2015 START refresh the chx for surcharges
                setCheck(dgvSurcharge, false);

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                DataTable dt3 = new DataTable();
                sqry = new StringBuilder();
                sqry.Append("select surcharge_id from patrons_surcharge where patron_id = @patid");
                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.Connection = myconn;
                if(cmd3.Connection.State == ConnectionState.Closed)
                    cmd3.Connection.Open();
                cmd3.CommandText = sqry.ToString();
                cmd3.Parameters.AddWithValue("@patid", strid);
                MySqlDataReader rd3 = cmd3.ExecuteReader();
                if (rd3.HasRows)
                {
                    while (rd3.Read())
                    {
                        int intid = Convert.ToInt32(rd3[0].ToString());
                        for (int i = 0; i < dgvSurcharge.Rows.Count; i++)
                        {
                            if (intid == Convert.ToInt32(dgvSurcharge[3, i].Value.ToString()))
                                dgvSurcharge[0, i].Value = (object)true;
                        }
                    }
                }
                rd3.Close();
                cmd3.Dispose();

                //3.23.2015 END
            }*/
        }

        public void onDGVSelect(KryptonDataGridView dgv, DataTable dt)
        {
            // = sender as KryptonDataGridView;
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
                    bool boolok = false;//added 6.8.2015
                    boolok = (dt.Rows[0]["with_promo"].ToString() == "1" ? cbxpromo.Checked = true : cbxpromo.Checked = false);
                    boolok = (dt.Rows[0]["with_amusement"].ToString() == "1" ? cbxamusement.Checked = true : cbxamusement.Checked = false);
                    boolok = (dt.Rows[0]["with_cultural"].ToString() == "1" ? cbxcultural.Checked = true : cbxcultural.Checked = false);
                    boolok = (dt.Rows[0]["with_citytax"].ToString() == "1" ? cbxlgu.Checked = true : cbxlgu.Checked = false);
                    boolok = (dt.Rows[0]["with_gross_margin"].ToString() == "1" ? cbxgross.Checked = true : cbxgross.Checked = false);
                    boolok = (dt.Rows[0]["with_prod_share"].ToString() == "1" ? cbxproducer.Checked = true : cbxproducer.Checked = false);
                    boolok = (dt.Rows[0]["with_surcharge"].ToString() == "1" ? cbxSurcharge.Checked = true : cbxSurcharge.Checked = false);//new for surcharge 6.8.2015

                    txtlgu.Text = dt.Rows[0]["with_citytax"].ToString();
                    int intcolor = (Convert.ToInt32(dt.Rows[0]["seat_color"]));

                    btncolor.SelectedColor = clscommon.From0BGR(intcolor);//m_clscom.UIntToColor(intcolor);//Color.FromArgb(intcolor);

                    if (m_ticketdt.Rows.Count > 0)
                    {
                        cmbprices.SelectedValue = dt.Rows[0]["base_price"];
                        //dtstart.Value = Convert.ToDateTime(dt.Rows[0]["effective_date"].ToString());
                    }
                }

                //3.23.2015 START
                setCheck(dgvOrdinance, false);

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                DataTable dt2 = new DataTable();
                sqry = new StringBuilder();
                sqry.Append("select ordinance_id from patrons_ordinance where patron_id = @patid");
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = myconn;
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                cmd.CommandText = sqry.ToString();
                cmd.Parameters.AddWithValue("@patid", strid);
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        int intid = Convert.ToInt32(rd[0].ToString());
                        for (int i = 0; i < dgvOrdinance.Rows.Count; i++)
                        {
                            if (intid == Convert.ToInt32(dgvOrdinance[3, i].Value.ToString()))
                                dgvOrdinance[0, i].Value = (object)true;
                        }
                    }
                }
                rd.Close();
                cmd.Dispose();

                //3.23.2015 END

                //6.8.2015 START refresh the chx for surcharges
                setCheck(dgvSurcharge, false);

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                DataTable dt3 = new DataTable();
                sqry = new StringBuilder();
                sqry.Append("select surcharge_id from patrons_surcharge where patron_id = @patid");
                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.Connection = myconn;
                if (cmd3.Connection.State == ConnectionState.Closed)
                    cmd3.Connection.Open();
                cmd3.CommandText = sqry.ToString();
                cmd3.Parameters.AddWithValue("@patid", strid);
                MySqlDataReader rd3 = cmd3.ExecuteReader();
                if (rd3.HasRows)
                {
                    while (rd3.Read())
                    {
                        int intid = Convert.ToInt32(rd3[0].ToString());
                        for (int i = 0; i < dgvSurcharge.Rows.Count; i++)
                        {
                            if (intid == Convert.ToInt32(dgvSurcharge[3, i].Value.ToString()))
                                dgvSurcharge[0, i].Value = (object)true;
                        }
                    }
                }
                rd3.Close();
                cmd3.Dispose();

                //3.23.2015 END

                //added 7.3.2019
                
                if(btnAdd.Text.ToUpper() == "NEW"){
                    computeDiscount();
                    computeCasierTotal();
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

        private void frmPatron_Load(object sender, EventArgs e)
        {

        }

        public void setCheck(DataGridView dgv, bool boolchk)
        {
            int cnt = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv[0, i].Value = (object)boolchk;
                if (boolchk)
                    cnt += 1;
            }
        }

        private void cmbprices_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder sqry = new StringBuilder();
            if (cmbprices.SelectedValue != null && cmbprices.Text != "")
            {
                int intout = -1;
                if(int.TryParse(cmbprices.SelectedValue.ToString(),out intout))
                    sqry.Append(String.Format("[id] = {0}", intout));
                //string val = cmbprices.SelectedValue.ToString();
                
                var foundRows = m_ticketdt.Select(sqry.ToString());
                DataTable dt = new DataTable();
                if (foundRows.Count() != 0)
                    dt = foundRows.CopyToDataTable();

                if (dt.Rows.Count > 0)
                    dtstart.Value = Convert.ToDateTime(dt.Rows[0]["effective_date"].ToString());

                if (cmbprices.SelectedIndex == 0)
                    dtstart.Value = DateTime.Now;
            }

            computeDiscount();
            //if (cbxlgu.Checked == true || cbxSurcharge.Checked == true)
            computeCasierTotal();//added 7.1.2019

        }

        private void cbxSurcharge_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSurcharge.Rows.Count; i++)
            {
                if (dgvSurcharge[0, i].Value != null)
                {
                    if ((bool)dgvSurcharge[0, i].Value)
                        dgvSurcharge[0, i].Value = (object)false;
                }
            }//added 7.1.2019


            if (cbxSurcharge.Checked == true)
            {
                pageSurcharge.Enabled = true;
                dgvSurcharge.Enabled = true;
            }
            else
            {
                pageSurcharge.Enabled = false;
                dgvSurcharge.Enabled = false;

            }
        }

        private void frmPatron_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_frmPS?.refreshDGV();
            m_frmPS?.setnormal();
        }

        private void btnaddbaseprice_Click(object sender, EventArgs e)
        {
            //added 6.30.2019
            using (frmBasePrice frm = new frmBasePrice())
            {
                frm.frmInit(m_frmM, m_clscom, this);
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtdiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                computeDiscount();
            }
        }

        public void computeDiscount()
        {
            //added 6.30.2019
            try
            {
                if (btnAdd.Text.ToUpper() == "SAVE" || btnEdit.Text.ToUpper() == "UPDATE")
                {
                    if (txtcode.Text.Trim() != "")
                    {
                        int intcode = 0;
                        int intout = -1;
                        if (int.TryParse(cmbdisc.SelectedValue.ToString(), out intout))
                            intcode = intout;

                        StringBuilder sqry = new StringBuilder();
                        sqry.Append("select * from discount_table ");
                        sqry.AppendFormat("where id = {0} ",intcode);
                        sqry.Append("order by discount_code asc");
                        MySqlCommand cmd = new MySqlCommand();

                        if (myconn == null)
                        {
                            myconn = new MySqlConnection();
                            myconn.ConnectionString = m_frmM._connection;
                        }
                            
                        cmd.Connection = myconn;
                        if (cmd.Connection.State == ConnectionState.Closed)
                            cmd.Connection.Open();
                        cmd.CommandText = sqry.ToString();
                        MySqlDataReader rd = cmd.ExecuteReader();

                        double discrate = 0;
                        if (rd.HasRows)
                        {
                            while (rd.Read())
                            {
                                double dblout = 0;
                                if (double.TryParse(rd[2].ToString(), out dblout))
                                    discrate = dblout;
                                break;
                            }
                        }
                        rd.Close();
                        cmd.Dispose();

                        double baseprice = Convert.ToDouble(cmbprices.Text);
                        double discount = 0;//baseprice * (discrate / 100);

                        if(discrate > 0)
                            discount = baseprice * (discrate / 100);

                        /*if (discrate > 0)
                        {
                            if (txtdiscount.Text.Trim() != "")
                            {
                                double dblout = 0;
                                if(double.TryParse(txtdiscount.Text.Trim(),out dblout))
                                {
                                    discount = baseprice * (dblout / 100);
                                }
                                    
                            }
                        }*/

                        txtprice.Text = (baseprice - discount).ToString();
                    }
                }
            }
            catch (Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show("Can't connect to the patrons table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cmbdisc_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeDiscount();
            //if (cbxlgu.Checked == true || cbxSurcharge.Checked == true)//REMARKED
                computeCasierTotal();//added 7.1.2019
        }

        private void btndisc_Click(object sender, EventArgs e)
        {
            //added 7.1.2019
            using (frmDiscount frm = new frmDiscount())
            {
                frm.frmInit(m_frmM, m_clscom, this);
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void dgvOrdinance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
                computeCasierTotal();
        }

        private void dgvSurcharge_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
                computeCasierTotal();
        }

        private void computeCasierTotal()
        {
            double dblunitprice = 0;
            double addnl = 0;
            for (int i = 0; i < dgvSurcharge.Rows.Count; i++)
            {
                if (dgvSurcharge[0, i].Value != null)
                {
                    if ((bool)dgvSurcharge[0, i].Value)
                    {
                        double dblout = -1;
                        if (double.TryParse(dgvSurcharge[2, i].Value.ToString(), out dblout))
                            addnl += dblout;
                    }
                }
            }

            for (int i = 0; i < dgvOrdinance.Rows.Count; i++)
            {
                if (dgvOrdinance[0, i].Value != null)
                {
                    if ((bool)dgvOrdinance[0, i].Value)
                    {
                        double dblout = -1;
                        if (double.TryParse(dgvOrdinance[2, i].Value.ToString(), out dblout))
                            addnl += dblout;
                    }
                }
            }

            double dblout1 = -1;
            if (double.TryParse(txtprice.Text.Trim(), out dblout1))
                dblunitprice = dblout1;


            lbltotal.Text = "Cashier Total: " + string.Format("{0:0.00}", dblunitprice + addnl);
        }

        private void dgvSurcharge_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
                dgvSurcharge.EndEdit();
        }

        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            onDGVSelect(sender as KryptonDataGridView, dt);
        }
    }
}
