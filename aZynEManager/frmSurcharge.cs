using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmSurcharge : Form
    {

        bool allowSpace = false;
        frmMain m_frmM;
        clscommon m_clscom;
        int intclear = 0;
        string m_loadpath = @"C:\";
        string m_sfullfilename = String.Empty;

        DataTable m_dt;
        MySqlConnection myconn = new MySqlConnection();

        public frmSurcharge()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            refreshCntrl();
            setnormal();
        }

        public void refreshCntrl()
        {
            txtcode.Text = "";
            cbxcancel.Checked = false;
            cbxpesos.Checked = true;
            if (cbxpesos.Checked)
                txtsymbol.Text = "P";
            else
                txtsymbol.Text = "%";
            txtval.Text = "";
            txtcode.Focus();
            dtstart.Value = DateTime.Now;
            dtend.Value = DateTime.Now;
            txtdetails.Text = "";
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void setnormal()
        {
            dgvResult.DataSource = null;
            dgvResult.Columns.Clear();

            refreshDGV();

            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txtval.Text = "";
            txtval.ReadOnly = true;
            txtdetails.Text = "";
            txtdetails.ReadOnly = true;

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

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.effective_date, a.code, a.details, a.amount_val, a.with_enddate, a.end_date, a.in_pesovalue ");
            sbqry.Append("from surcharge_tbl a ");
            sbqry.Append("order by a.effective_date desc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                this.dgvResult.DataSource = dt;
                int iwidth = dgvResult.Width / 3;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[0].Visible = false;
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Effectivity Date";
                dgvResult.Columns[2].Width = iwidth;
                dgvResult.Columns[2].HeaderText = "Code";
                dgvResult.Columns[3].Width = iwidth;
                dgvResult.Columns[3].HeaderText = "Details";
                dgvResult.Columns[4].Width = iwidth;
                dgvResult.Columns[4].HeaderText = "Value";
                dgvResult.Columns[5].Width = 0;
                dgvResult.Columns[5].HeaderText = "With EndDate";
                dgvResult.Columns[5].Visible = false;
                dgvResult.Columns[6].Width = 0;
                dgvResult.Columns[6].HeaderText = "End Date";
                dgvResult.Columns[6].Visible = false;
                dgvResult.Columns[7].Width = 0;
                dgvResult.Columns[7].HeaderText = "In Peso Value";
                dgvResult.Columns[7].Visible = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "SURCHARGE_ADD", m_frmM._connection);
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
                txtval.Text = "";
                txtval.ReadOnly = false;
                txtdetails.Text = "";
                txtdetails.ReadOnly = false;

                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtcode.SelectAll();
                txtcode.Focus();

                dgvResult.DataSource = null;
                dgvResult.Columns.Clear();

                refreshDGV();
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
                if (txtdetails.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtdetails.SelectAll();
                    txtdetails.Focus();
                    return;
                }
                if (txtval.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtval.SelectAll();
                    txtval.Focus();
                    return;
                }
                DateTime datestart = new DateTime();
                if (dtstart.Text == "" || dtstart.Value == null)
                {
                    MessageBox.Show("Please fill the effective date information.");
                    dtstart.Focus();
                    return;
                }
                else
                    datestart = dtstart.Value;

                DateTime dateend = new DateTime();
                int intenddate = 0;//with end date 1-true 0-false
                if (this.cbxcancel.Checked == true)
                {
                    if (dtend.Text == "" || dtend.Value == null)
                    {
                        MessageBox.Show("Please fill the end date information.");
                        dtend.Focus();
                        return;
                    }
                    else
                    {
                        dateend = dtend.Value;
                        intenddate = 1;
                    }
                }

                int intpeso = 0;//in peso value 1-true 0-false(pectage)
                if (cbxpesos.Checked)
                    intpeso = 1;

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from surcharge_tbl ");
                sqry.Append(String.Format("where code = '{0}' ", this.txtcode.Text.Trim()));
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
                sqry.Append(String.Format("insert into surcharge_tbl value(0,'{0}','{1}','{2}','{3}',{4},{5},{6})",
                    txtcode.Text.Trim(), txtdetails.Text.Trim(), datestart.Date.ToString("yyyy-MM-dd"), dateend.Date.ToString("yyyy-MM-dd"),
                    intenddate, txtval.Text.Trim(), intpeso));
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

                    m_clscom.AddATrail(m_frmM.m_userid, "SURCHARGE_ADD", "SURCHARGE_TBL",
                        Environment.MachineName.ToString(), "ADD NEW SURCHARGE INFO: CODE=" + txtcode.Text
                        + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "SURCHARGE_EDIT", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

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
                txtval.ReadOnly = false;
                txtdetails.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

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
                if (txtdetails.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtdetails.SelectAll();
                    txtdetails.Focus();
                    return;
                }
                if (txtval.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtval.SelectAll();
                    txtval.Focus();
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

                DateTime dateend = new DateTime();
                int intenddate = 0;//with end date 1-true 0-false
                if (this.cbxcancel.Checked)
                {
                    if (dtend.Text == "" || dtend.Value == null)
                    {
                        MessageBox.Show("Please fill the end date information.");
                        dtend.Focus();
                        return;
                    }
                    else
                        dateend = dtend.Value;

                    intenddate = 1;
                }

                int intpeso = 0;//in peso value 1-true 0-false(pectage)
                if (cbxpesos.Checked)
                    intpeso = 1;

                KryptonDataGridView dgv = dgvResult;
                string strid = "-1";
                if (dgv == null)
                    return;
                if (dgv.CurrentRow.Selected)
                {
                    strid = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
                }
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //query for the existance of ordinance on other table
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from patrons_surcharge ");
                sqry.Append(String.Format("where surcharge_id = '{0}'", strid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (rowCount > 0)
                {
                    myconn.Close();
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't update this record, \n\rits being used by a patron.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool boolstatus = false;
                    //validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select id from surcharge_tbl ");
                    sqry.Append(String.Format("where code = '{0}' ", this.txtcode.Text.Trim()));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            int intid = Convert.ToInt32(rd[0].ToString());
                            if (intid != Convert.ToInt32(strid))
                            {
                                cmd.Dispose();
                                myconn.Close();
                                setnormal();
                                if (myconn.State == ConnectionState.Open)
                                    myconn.Close();
                                MessageBox.Show("This surcharge code \n\ris already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                                boolstatus = true;
                        }
                    
                    }
                    else
                        boolstatus = true;

                    rd.Close();
                    cmd.Dispose();

                    if(boolstatus)
                    {
                        sqry = new StringBuilder();
                        sqry.Append("update surcharge_tbl ");
                        sqry.Append("set code = @code, ");
                        sqry.Append("details = @details, ");
                        sqry.Append("effective_date = @effdate, ");
                        sqry.Append("end_date = @enddate, ");
                        sqry.Append("with_enddate = @wenddate, ");
                        sqry.Append("amount_val = @amtval, ");
                        sqry.Append("in_pesovalue = @inpesoval ");   
                         sqry.Append("where id = @id");   

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim());
                        cmd.Parameters.AddWithValue("@details", txtdetails.Text.Trim());
                        cmd.Parameters.AddWithValue("@effdate", datestart.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@enddate", dateend.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@wenddate", intenddate);
                        cmd.Parameters.AddWithValue("@amtval", txtval.Text.Trim());
                        cmd.Parameters.AddWithValue("@inpesoval", intpeso);
                        cmd.Parameters.AddWithValue("@id", strid);

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "SURCHARGE_EDIT", "SURCHARGE_TBL",
                        Environment.MachineName.ToString(), "EDIT SURCHARGE INFO: CODE=" + txtcode.Text
                        + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully updated the existing record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "SURCHARGE_DELETE", m_frmM._connection);
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

                    //query for the existance of ordinance on other table
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select count(*) from patrons_surcharge ");
                    sqry.Append(String.Format("where surcharge_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        myconn.Close();
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit's being used by a patron.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //delete from ordinances given that the ordinance is not used by a patron
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from surcharge_tbl where id = {0}", intid));
                    try
                    {
                        //delete value for the ordinance_tbl table
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        m_clscom.AddATrail(m_frmM.m_userid, "SURCHARGE_DELETE", "SURCHARGE_TBL",
                        Environment.MachineName.ToString(), "REMOVED SURCHARGE INFO: NAME=" + this.txtcode.Text
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
            refreshDGV();
            setnormal();
        }

        private void cbxcancel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxcancel.Checked)
                dtend.Enabled = true;
            else
                dtend.Enabled = false;
        }

        private void cbxpesos_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxpesos.Checked)
                txtsymbol.Text = "P";
            else
                txtsymbol.Text = "%";
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            try
            {
                if (dgv == null)
                    return;
                if (dgv.CurrentRow.Selected)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Text = "edit";

                    btnDelete.Enabled = true;
                    btnDelete.Text = "remove";

                    int ordinanceid = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[0].Value.ToString());
                    string strno = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    txtcode.Text = strno;
                    txtdetails.Text = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[3].Value.ToString();
                    txtval.Text = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    int inpeso = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[7].Value.ToString());
                    if (inpeso == 0)
                        cbxpesos.Checked = false;
                    else
                        cbxpesos.Checked = true;

                    dtstart.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[1].Value.ToString());

                    int isenddate = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[5].Value.ToString());
                    if (isenddate == 0)
                    {
                        dtend.Value = DateTime.Now;
                        cbxcancel.Checked = false;
                    }
                    else
                    {
                        dtend.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[6].Value.ToString());
                        cbxcancel.Checked = true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
