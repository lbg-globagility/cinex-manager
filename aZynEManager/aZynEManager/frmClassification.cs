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
    public partial class frmClassification : Form
    {
        frmMainUtility frmutility = null;
        frmMain m_frmM = null;
        MySqlConnection myconn = new MySqlConnection();
        clscommon m_clscom = null;
        DataTable m_dt = new DataTable();

        public frmClassification()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, frmMainUtility frmu)
        {
            frmutility = frmu;
            m_frmM = frm;
            m_clscom = frmu.m_clscom;

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
            sbqry.Append("SELECT a.id, a.name, a.description ");
            sbqry.Append("FROM classification a order by a.description asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.DataSource = null;
            //if (dt.Rows.Count > 0)
            //    this.dgvResult.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgvResult.Width / 3;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth - 13;
                dgvResult.Columns[1].HeaderText = "Name";
                dgvResult.Columns[2].Width = (iwidth * 2) - 10;
                dgvResult.Columns[2].HeaderText = "Description";
            }
        }

        public void setnormal()
        {
            txtname.Text = "";
            txtname.ReadOnly = true;
            txtdesc.Text = "";
            txtdesc.ReadOnly = true;

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CLASSIFICATION_ADD", m_frmM._connection);
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
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from classification ");
                sqry.Append(String.Format("where name = '{0}' ", txtname.Text.Trim()));
                sqry.Append(String.Format("and description = '{0}' ", txtdesc.Text.Trim()));

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
                sqry.Append(String.Format("insert into classification value(0,'{0}','{1}')",
                    txtname.Text.Trim(), txtdesc.Text.Trim()));
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

                    m_clscom.AddATrail(m_frmM.m_userid, "CLASSIFICATION_ADD", "CLASSIFICATION",
                        Environment.MachineName.ToString(), "ADD NEW MOVIE CLASSIFICATION INFO: NAME=" + txtname.Text
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
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CLASSIFICATION_DELETE", m_frmM._connection);
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
                    sqry.Append(String.Format("select count(*) from movies_class where class_id = {0}", intid));
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

                    ////validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from classification ");
                    sqry.Append(String.Format("where id = {0} ", intid));

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from classification where id = {0}", intid));
                        try
                        {
                            //delete value for the movies table
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            m_clscom.AddATrail(m_frmM.m_userid, "CLASSIFICATION_DELETE", "CLASSIFICATION",
                        Environment.MachineName.ToString(), "REMOVED MOVIE CLASSIFICATION INFO: NAME=" + txtname.Text
                        + " | ID=" + intid, m_frmM._connection);

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
            }
            refreshDGV();
            setnormal();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CLASSIFICATION_EDIT", m_frmM._connection);
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
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from classification ");
                sqry.Append(String.Format("where name = '{0}' ", txtname.Text.Trim()));
                sqry.Append(String.Format("and description = '{0}' ", txtdesc.Text.Trim()));
                sqry.Append(String.Format("and id = {0} ", intid));

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

                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update classification set name = '{0}',", txtname.Text.Trim()));
                strqry.Append(String.Format(" description = '{0}'", txtdesc.Text.Trim()));
                strqry.Append(String.Format(" where id = {0}", intid));
                try
                {
                    //update the movies table
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    ////remove the movies_class values
                    //strqry = new StringBuilder();
                    //strqry.Append(String.Format("delete from movies_class where movie_id = {0}", intid));
                    //try
                    //{
                    //    if (myconn.State == ConnectionState.Closed)
                    //        myconn.Open();
                    //    MySqlCommand cmd3 = new MySqlCommand(strqry.ToString(), myconn);
                    //    cmd3.ExecuteNonQuery();
                    //    cmd3.Dispose();
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "CLASSIFICATION_EDIT", "CLASSIFICATION",
                        Environment.MachineName.ToString(), "UPDATED MOVIE CLASSIFICATION INFO: NAME=" + txtname.Text
                        + " | ID=" + intid, m_frmM._connection);

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
            }
        }

        private void frmClassification_Resize(object sender, EventArgs e)
        {
            if (this.Size.Width > 375)
            {
                this.Size = new Size(375, this.Size.Height);
            }
        }

        private void frmClassification_Load(object sender, EventArgs e)
        {

        }
    }
}
