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
    public partial class frmSoundSystem : Form
    {
        frmCinema m_frmC = null;
        frmMainUtility frmutility = null;
        frmMovieList m_frmlist = null;
        frmMain m_frmM;
        clscommon m_clscom = null;
        MySqlConnection myconn = new MySqlConnection();
        DataTable m_dt = new DataTable();

        public frmSoundSystem()
        {
            InitializeComponent();
        }

        private void frmSoundSystem_Load(object sender, EventArgs e)
        {

        }

        public void frmInit(frmMain frm, clscommon cls, frmCinema frmcnm)
        {
            m_frmM = frm;
            m_clscom = cls;
            m_frmC = frmcnm;

            txtname.Text = "";
            txtname.ReadOnly = true;

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
            sbqry.Append("SELECT a.id, a.sound_system_type ");
            sbqry.Append("FROM sound_system a order by a.sound_system_type asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setnormal()
        {
            txtname.Text = "";
            txtname.ReadOnly = true;

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
                dgvResult.Columns[1].Width = iwidth * 3 - 20;
                dgvResult.Columns[1].HeaderText = "Name";
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

                txtname.Text = "";
                txtname.ReadOnly = false;

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

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from sound_system ");
                sqry.Append(String.Format("where sound_system_type = '{0}' ", txtname.Text.Trim()));
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
                sqry.Append(String.Format("insert into sound_system value(0,'{0}')",txtname.Text.Trim()));
      
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

                    m_clscom.AddATrail(m_frmM.m_userid, "SOUNDSYS", "SOUND_SYSTEM",
                                Environment.MachineName.ToString(), "ACCESSED SOUND_SYSTEM MODULE: ADDED=" + txtname.Text
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
                txtname.ReadOnly = false;

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
                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from sound_system ");
                sqry.Append(String.Format("where sound_system_type = '{0}' ", txtname.Text.Trim()));
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
                strqry.Append(String.Format("update sound_system set sound_system_type = '{0}'", txtname.Text.Trim()));
                strqry.Append(String.Format(" where id = {0}", intid));

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

                    m_clscom.AddATrail(m_frmM.m_userid, "SOUNDSYS", "SOUND_SYSTEM",
                                Environment.MachineName.ToString(), "ACCESSED SOUND_SYSTEM MODULE: UPDATED=" + txtname.Text
                                + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the sound system table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    sqry.Append(String.Format("select count(*) from cinema where sound_id = {0}", intid));
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
                    {
                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from sound_system where id = {0}", intid));
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

                            m_clscom.AddATrail(m_frmM.m_userid, "SOUNDSYS", "SOUND_SYSTEM",
                               Environment.MachineName.ToString(), "ACCESSED SOUND_SYSTEM MODULE: REMOVED=" + txtname.Text
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
            refreshDGV();
            setnormal();
        }

        private void frmSoundSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_frmC != null)
            {
                m_frmC.cmbsounds.DataSource = null;
                DataView view = new DataView(m_dt);
                view.Sort = "sound_system_type";
                DataTable dtsort = view.ToTable(true, "sound_system_type", "id");
                DataRow row = dtsort.NewRow();
                row["id"] = "0";
                row["sound_system_type"] = "";
                dtsort.Rows.InsertAt(row, 0);
                m_frmC.cmbsounds.DataSource = dtsort;
                m_frmC.cmbsounds.ValueMember = "id";
                m_frmC.cmbsounds.DisplayMember = "sound_system_type";
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
            }
        }
    }
}
