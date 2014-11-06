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
    public partial class frmRating : Form
    {
        frmMainUtility frmutility = null;
        frmMovieList m_frmlist = null;
        frmMain m_frmM;
        clscommon m_clscom = null;
        MySqlConnection myconn = new MySqlConnection();

        DataTable m_dt = new DataTable();

        public frmRating()
        {
            InitializeComponent();
        }

        public void frmInitII(frmMain frmM, frmMovieList frmlist, clscommon cls)
        {
            m_frmlist = frmlist;
            m_frmM = frmM;
            m_clscom = cls;

            txtname.Text = "";
            txtname.ReadOnly = true;
            txtdesc.Text = "";
            txtdesc.ReadOnly = true;

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            setnormal();
        }


        public void frmInit(frmMain frmM,clscommon cls)
        {
            //ClearControls();
            //m_frmM = frmM;
            //m_frmlist = frmlist;

            m_frmM = frmM;
            m_clscom = cls;

            txtname.Text = "";
            txtname.ReadOnly = true;
            txtdesc.Text = "";
            txtdesc.ReadOnly = true;

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            dgvResult.ClearSelection();

            setnormal();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("SELECT a.id, a.name, a.description, a.remarks ");
            sbqry.Append("FROM mtrcb a order by a.name asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
            dgvResult.ClearSelection();
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
                dgvResult.Columns[1].Width = iwidth-13;
                dgvResult.Columns[1].HeaderText = "Rating";
                dgvResult.Columns[2].Width = iwidth*2;
                dgvResult.Columns[2].HeaderText = "Name";
                dgvResult.Columns[3].HeaderText = "Remarks";
                dgvResult.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }
        }

        public void setnormal()
        {
            txtname.Text = "";
            txtname.ReadOnly = true;
            txtdesc.Text = "";
            txtdesc.ReadOnly = true;
            txtRemarks.Text = string.Empty;
            txtRemarks.ReadOnly = true;

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

        private void btnApply_Click(object sender, EventArgs e)
        {
            unselectbutton();
            //ClearControls();
            if (this.txtname.Text != "" || this.txtdesc.Text != "")
            {
                string strQuery = "INSERT INTO mtrcb VALUES (0, @name, '@description, @remarks)";

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(strQuery, myconn);
                    cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtdesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text.Trim());
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    myconn.Close();
                    ClearControls();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            unselectbutton();

            txtdesc.Text = "";
            txtname.Text = "";
            txtRemarks.Text = string.Empty;

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
                if (con is KryptonGroupPanel)
                {
                    ControlCollection coll2 = (ControlCollection)con.Controls;
                    foreach (Control con2 in coll2)
                    {
                        if (con2 is TextBox)
                        {
                            con2.Text = "";
                        }
                        if (con2 is ListBox)
                        {
                            con2.Text = "";
                            ((ListBox)con2).Items.Clear();
                        }
                    }
                }
                if (con is KryptonGroup)
                {
                    if(con.Controls.Contains(txtdesc))
                        MessageBox.Show("A");
                    else
                        MessageBox.Show("b");
                    ControlCollection coll3 = (ControlCollection)con.Controls;
                    foreach (Control con3 in coll3)
                    {
                        if (con3 is TextBox)
                        {
                            con3.Text = "";
                        }
                        if (con3 is ListBox)
                        {
                            con3.Text = "";
                            ((ListBox)con3).Items.Clear();
                        }
                    }
                }
            
            }
        }

        private void frmRating_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_frmlist !=null)
            {
                m_frmM.m_dtrating = m_clscom.setDataTable("select * from mtrcb order by id asc", m_frmM._connection);
                m_frmlist.populateratings();
            }
        }

        private void frmRating_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "RATING_ADD", m_frmM._connection);
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

                txtname.Text = "";
                txtname.ReadOnly = false;
                txtdesc.Text = "";
                txtdesc.ReadOnly = false;
                txtRemarks.Text = string.Empty;
                txtRemarks.ReadOnly = false;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtname.SelectAll();
                txtname.Focus();
            }
            else
            {
                string strstatus = String.Empty;
                if (txtname.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtname.SelectAll();
                    txtname.Focus();
                    return;
                }
                if (txtdesc.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtdesc.SelectAll();
                    txtdesc.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                string strValidateQuery = "SELECT COUNT(*) FROM mtrcb WHERE name = @name AND description = @description";
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(strValidateQuery, myconn);
                cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtdesc.Text.Trim());
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
                //melvin 10-14-14
              
                string strQuery = "INSERT INTO mtrcb VALUES (0, @name , @description, @remarks)";

                try
                {
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strQuery, myconn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtdesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text.Trim());

                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "RATING_ADD", "MTRCB",
                            Environment.MachineName.ToString(), "ADD NEW MTRCB RATING INFO: NAME=" + txtname.Text
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "RATING_DELETE", m_frmM._connection);
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
                    string strValidateQuery = "SELECT COUNT(*) FROM movies WHERE rating_id = @id";

                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(strValidateQuery, myconn);
                    cmd.Parameters.AddWithValue("@id", intid);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text,MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }

                    string strValidateQuery1 = "SELECT COUNT(*) FROM mtrcb WHERE id = @id";
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strValidateQuery1, myconn);
                    cmd.Parameters.AddWithValue("@id", intid);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        string strQuery = "DELETE FROM mtrcb WHERE id = @id";
                        try
                        {
                            //delete value for the movies table
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(strQuery, myconn);
                            cmd.Parameters.AddWithValue("@id", intid);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            m_clscom.AddATrail(m_frmM.m_userid, "RATING_DELETE", "MTRCB",
                                Environment.MachineName.ToString(), "REMOVED MTRCB RATING INFO: NAME=" + txtname.Text
                                + " | ID=" + intid.ToString(), m_frmM._connection);

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
            refreshDGV();
            setnormal();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "RATING_EDIT", m_frmM._connection);
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
                txtname.ReadOnly = false;
                txtdesc.ReadOnly = false;
                txtRemarks.ReadOnly = false;

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
                if (txtname.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtname.SelectAll();
                    txtname.Focus();
                    return;
                }
                if (txtdesc.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtname.SelectAll();
                    txtdesc.Focus();
                    return;
                }
                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                string strValidateQuery = "SELECT COUNT(*) FROM mtrcb WHERE name = @name AND description = @description AND id <> @id";
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(strValidateQuery, myconn);
                cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtdesc.Text.Trim());
                cmd.Parameters.AddWithValue("@id", intid);

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
                string strQuery = "UPDATE mtrcb SET name = @name, description = @description, remarks = @remarks WHERE id = @id";

                try
                {
                    //update the movies table
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strQuery, myconn);
                    cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtdesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", intid);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "RATING_EDIT", "MTRCB",
                            Environment.MachineName.ToString(), "UPDATED MTRCB RATING INFO: NAME=" + txtname.Text
                            + " | ID=" + intid.ToString(), m_frmM._connection);

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

                txtname.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                txtdesc.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                txtRemarks.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void frmRating_Shown(object sender, EventArgs e)
        {
            dgvResult.ClearSelection();
        }

    
    }
}
