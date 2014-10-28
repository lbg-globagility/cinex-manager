using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using MySql.Data.MySqlClient;

namespace aZynEManager
{
    public partial class frmUserLevel : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        DataTable dtmodulecinema = new DataTable();
        DataTable dtmodulereport = new DataTable();
        DataTable dtmoduleutility = new DataTable();
        DataTable dtmoduleconfig = new DataTable();
        DataTable dtmoduleticket = new DataTable();
        DataTable dtuserlevel = new DataTable();
        DataTable dtsystems = new DataTable();
        MySqlConnection myconn = null;

        public frmUserLevel()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;

            txtuserlevel.Text = "";
            txtuserlevel.ReadOnly = true;

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            dgvUserLevel.ClearSelection();

            setnormal();
            populateuserlevel();
            //populateuserlevel();
            populatesystem();
        }

        public void populatesystem()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select * ");
            sbqry.Append("from application");
            dtsystems = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            DataRow row = dtsystems.NewRow();
            row["system_code"] = "0";
            row["system_name"] = "";
            dtsystems.Rows.InsertAt(row, 0);
            cmbSystem.DataSource = dtsystems;
            cmbSystem.DisplayMember = "system_name";
            cmbSystem.ValueMember = "system_code";
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.module_desc, a.module_code, a.module_group, a.id ");
            sbqry.Append("from system_module a where a.system_code = 1 ");
            sbqry.Append(String.Format("and a.module_group = '{0}' order by a.module_desc asc", "CINEMA"));
            dtmodulecinema = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvModuleCinema, dtmodulecinema);

            sbqry = new StringBuilder();
            sbqry.Append("select a.module_desc, a.module_code, a.module_group, a.id ");
            sbqry.Append("from system_module a where a.system_code = 1 ");
            sbqry.Append(String.Format("and a.module_group = '{0}' order by a.module_desc asc", "REPORT"));
            dtmodulereport = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvModuleReport, dtmodulereport);

            sbqry = new StringBuilder();
            sbqry.Append("select a.module_desc, a.module_code, a.module_group, a.id ");
            sbqry.Append("from system_module a where a.system_code = 1 ");
            sbqry.Append(String.Format("and a.module_group = '{0}' order by a.module_desc asc", "UTILITY"));
            dtmoduleutility = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvModuleUtility, dtmoduleutility);

            sbqry = new StringBuilder();
            sbqry.Append("select a.module_desc, a.module_code, a.module_group, a.id ");
            sbqry.Append("from system_module a where a.system_code = 1 ");
            sbqry.Append(String.Format("and a.module_group = '{0}' order by a.module_desc asc", "CONFIG"));
            dtmoduleconfig = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvModuleConfig, dtmoduleconfig);

            sbqry = new StringBuilder();
            sbqry.Append("select a.module_desc, a.module_code, a.module_group, a.id ");
            sbqry.Append("from system_module a where a.system_code = 2 ");
            sbqry.Append(String.Format("and a.module_group = '{0}' order by a.module_desc asc", "TICKET"));
            dtmoduleticket = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvModuleTicket, dtmoduleticket);
        }

        public void populateuserlevel()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.level_desc, a.id, a.system_code ");
            sbqry.Append("from user_level a order by a.level_desc asc");
            dtuserlevel = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridViewII(dgvUserLevel, dtuserlevel);
        }

        public void setuserlevel(int intsyscode)
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.level_desc, a.id, a.system_code ");
            sbqry.Append(String.Format("from user_level a where a.system_code = {0} order by a.level_desc asc", intsyscode));
            dtuserlevel = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridViewII(dgvUserLevel, dtuserlevel);
        }

        public void setDataGridView(DataGridView dgv, DataTable dt)
        {
            dgv.Columns.Clear();
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dgv.DataSource = dt;
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                int iwidth = dgv.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = iwidth * 3 - 45;
                dgv.Columns[0].HeaderText = "Module Description";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Width = 0;
                dgv.Columns[1].HeaderText = "Module Code";
                dgv.Columns[1].ReadOnly = true;
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "Module Group";
                dgv.Columns[2].ReadOnly = true;
                dgv.Columns[3].Width = 0;
                dgv.Columns[3].HeaderText = "ID";
                dgv.Columns[3].ReadOnly = true;
                dgv.Columns.Insert(0, cbx);
            }
        }

        public void setDataGridViewII(DataGridView dgv, DataTable dt)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
                dgv.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgv.Width / 2;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = iwidth * 2 - 25;
                dgv.Columns[0].HeaderText = "User Level";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Width = 0;
                dgv.Columns[1].HeaderText = "ID";
                dgv.Columns[1].ReadOnly = true;
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "System Code";
                dgv.Columns[2].ReadOnly = true;
            }
            txtcnt.Text = "Count: 0 / " + dgvModuleCinema.Rows.Count.ToString();
        }

        public void setnormal()
        {
            txtuserlevel.Text = "";
            txtuserlevel.ReadOnly = true;

            btnAdd.Enabled = true;
            btnAdd.Text = "new";
            btnAdd.Values.Image = Properties.Resources.buttonadd;

            btnEdit.Enabled = false;
            btnEdit.Text = "edit";
            btnEdit.Values.Image = Properties.Resources.buttonapply;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;

            dgvUserLevel.Enabled = true;
            grpcontrol.Enabled = true;
            grpgrant.Visible = false;

            int cnt = 0;
            if (tabModule.SelectedPage == pageCinema)
                cnt = dgvModuleCinema.Rows.Count;
            else if (tabModule.SelectedPage == pageReport)
                cnt = dgvModuleReport.Rows.Count;
            else if (tabModule.SelectedPage == pageUtility)
                cnt = dgvModuleUtility.Rows.Count;
            else if (tabModule.SelectedPage == pageConfig)
                cnt = dgvModuleConfig.Rows.Count;
            else if (tabModule.SelectedPage == pageTicket)
                cnt = dgvModuleTicket.Rows.Count;

            txtcnt.Text = "Count: 0 / " + cnt.ToString();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void dgvUserLevel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int chklstcinema = 0;
            int chklstreport = 0;
            int chklstutility = 0;
            int chklstconfig = 0;
            int chklstticket = 0;

            setCheck(dgvModuleCinema, false);
            setCheck(dgvModuleReport, false);
            setCheck(dgvModuleUtility, false);
            setCheck(dgvModuleConfig, false);
            setCheck(dgvModuleTicket, false);

            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

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

                this.txtuserlevel.Text = dgv.SelectedRows[0].Cells[0].Value.ToString();
                int intid = Convert.ToInt32(dgvUserLevel.SelectedRows[0].Cells[1].Value.ToString());

                string systemname = dgvUserLevel.SelectedRows[0].Cells[2].Value.ToString();
                for (int i = 0; i < cmbSystem.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbSystem.Items[i];
                    if (drv.Row["system_code"].ToString().ToUpper() == systemname.ToUpper())
                    {
                        cmbSystem.SelectedIndex = i;
                        break;
                    }
                }

                //for cinema tabpage
                for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
                {
                    int intmid = Convert.ToInt32(dgvModuleCinema[4, i].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level_rights a where a.user_level = {0}", intid));
                    sqry.Append(String.Format(" and a.module_id = {0} and a.system_code = {1}", intmid, cmbSystem.SelectedValue));
                    sqry.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "CINEMA"));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        dgvModuleCinema[0, i].Value = (object)true;
                        chklstcinema += 1;
                    }
                }
                //for report tabpage
                for (int i = 0; i < dgvModuleReport.Rows.Count; i++)
                {
                    int intmid = Convert.ToInt32(dgvModuleReport[4, i].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level_rights a where a.user_level = {0}", intid));
                    sqry.Append(String.Format(" and a.module_id = {0} and a.system_code = {1}", intmid, cmbSystem.SelectedValue));
                    sqry.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "REPORT"));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        dgvModuleReport[0, i].Value = (object)true;
                        chklstreport += 1;
                    }
                }
                //for UTILITY tabpage
                for (int i = 0; i < dgvModuleUtility.Rows.Count; i++)
                {
                    int intmid = Convert.ToInt32(dgvModuleUtility[4, i].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level_rights a where a.user_level = {0}", intid));
                    sqry.Append(String.Format(" and a.module_id = {0} and a.system_code = {1}", intmid, cmbSystem.SelectedValue));
                    sqry.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "UTILITY"));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        dgvModuleUtility[0, i].Value = (object)true;
                        chklstutility += 1;
                    }
                }
                //for config tabpage
                for (int i = 0; i < dgvModuleConfig.Rows.Count; i++)
                {
                    int intmid = Convert.ToInt32(dgvModuleConfig[4, i].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level_rights a where a.user_level = {0}", intid));
                    sqry.Append(String.Format(" and a.module_id = {0} and a.system_code = {1}", intmid, cmbSystem.SelectedValue));
                    sqry.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "CONFIG"));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        dgvModuleConfig[0, i].Value = (object)true;
                        chklstconfig += 1;
                    }
                }
                //for ticket tabpage
                for (int i = 0; i < dgvModuleTicket.Rows.Count; i++)
                {
                    int intmid = Convert.ToInt32(dgvModuleTicket[4, i].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level_rights a where a.user_level = {0}", intid));
                    sqry.Append(String.Format(" and a.module_id = {0} and a.system_code = {1}", intmid, cmbSystem.SelectedValue));
                    sqry.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "TICKET"));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        dgvModuleTicket[0, i].Value = (object)true;
                        chklstticket += 1;
                    }
                }

                int cnt = 0;
                int cntchk = 0;
                if (tabModule.SelectedPage == pageCinema)
                {
                    cnt = dgvModuleCinema.Rows.Count;
                    cntchk = chklstcinema;
                }
                else if (tabModule.SelectedPage == pageReport)
                {
                    cnt = dgvModuleReport.Rows.Count;
                    cntchk = chklstreport;
                }
                else if (tabModule.SelectedPage == pageUtility)
                {
                    cnt = dgvModuleUtility.Rows.Count;
                    cntchk = chklstutility;
                }
                else if (tabModule.SelectedPage == pageConfig)
                {
                    cnt = dgvModuleConfig.Rows.Count;
                    cntchk = chklstconfig;
                }
                else if (tabModule.SelectedPage == pageTicket)
                {
                    cnt = dgvModuleTicket.Rows.Count;
                    cntchk = chklstticket;
                }

                txtcnt.Text = String.Format("Count: {0} / {1}", cntchk.ToString(), cnt.ToString());
                //txtcnt.Text = "Count: " + chklst.ToString() + " / " + dgvModuleCinema.Rows.Count.ToString();
            }
        }

        private void btngrant_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (cmbSystem.SelectedIndex == 1)
            {
                unselectbutton();
                if (tabModule.SelectedPage == pageCinema)
                    setCheck(dgvModuleCinema, true);
                else if (tabModule.SelectedPage == pageReport)
                    setCheck(dgvModuleReport, true);
                else if (tabModule.SelectedPage == pageUtility)
                    setCheck(dgvModuleUtility, true);
                else if (tabModule.SelectedPage == pageConfig)
                    setCheck(dgvModuleConfig, true);
            }
            else
            {
                if (tabModule.SelectedPage == pageTicket)
                    setCheck(dgvModuleTicket, true);
            }
        }

        private void btnrevoke_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (cmbSystem.SelectedIndex == 1)
            {
                unselectbutton();
                if (tabModule.SelectedPage == pageCinema)
                    setCheck(dgvModuleCinema, false);
                else if (tabModule.SelectedPage == pageReport)
                    setCheck(dgvModuleReport, false);
                else if (tabModule.SelectedPage == pageUtility)
                    setCheck(dgvModuleUtility, false);
                else if (tabModule.SelectedPage == pageConfig)
                    setCheck(dgvModuleConfig, false);
            }
            else
            {
                if (tabModule.SelectedPage == pageTicket)
                    setCheck(dgvModuleTicket, false);
            }
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
            txtcnt.Text = "Count: " + cnt.ToString() + " / " + dgv.Rows.Count.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "GRANT_ADD", m_frmM._connection);
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
                txtuserlevel.Text = "";
                txtuserlevel.ReadOnly = false;

                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtuserlevel.SelectAll();
                txtuserlevel.Focus();
                grpgrant.Visible = true;

                dgvUserLevel.Enabled = false;

                if (tabModule.SelectedPage == pageCinema)
                    setCheck(dgvModuleCinema, false);
                else if (tabModule.SelectedPage == pageReport)
                    setCheck(dgvModuleReport, false);
                else if (tabModule.SelectedPage == pageUtility)
                    setCheck(dgvModuleUtility, false);
                else if (tabModule.SelectedPage == pageConfig)
                    setCheck(dgvModuleConfig, false);
                else if (tabModule.SelectedPage == pageTicket)
                    setCheck(dgvModuleTicket, false);
            }
            else
            {
                string strid = "0";
                string strstatus = String.Empty;
                if (txtuserlevel.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtuserlevel.SelectAll();
                    txtuserlevel.Focus();
                    return;
                }

                if (cmbSystem.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    cmbSystem.SelectAll();
                    cmbSystem.Focus();
                    return;
                }
                //int chkcnt = 0;
                //for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
                //{
                //    if (dgvModuleCinema[0, i].Value != null)
                //    {
                //        if ((bool)dgvModuleCinema[0, i].Value)
                //        {
                //            chkcnt += 1;
                //        }
                //    }
                //}
                //if (chkcnt == 0)
                //{
                //    MessageBox.Show("Please check the required grants \n\r for the user level.", 
                //        this.Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
                //    txtuserlevel.SelectAll();
                //    txtuserlevel.Focus();
                //    return;
                //}

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from user_level ");
                sqry.Append(String.Format("where level_desc = '{0}' ", this.txtuserlevel.Text.ToUpper().Trim()));

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
                //with queries
                sqry.Append(String.Format("insert into user_level values(0,'{0}',{1})",
                    txtuserlevel.Text.ToUpper().Trim(), cmbSystem.SelectedValue));

                try
                {
                    int idout = -1;
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    if (myconn.State == ConnectionState.Open)
                    {
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        idout = Convert.ToInt32(cmd.LastInsertedId);

                        cmd.Dispose();
                    }
                    if (idout > -1)
                    {
                        //for cinema page
                        tabinsertcheck(myconn, dgvModuleCinema, idout);
                        //for report page
                        tabinsertcheck(myconn, dgvModuleReport, idout);
                        //for utility page
                        tabinsertcheck(myconn, dgvModuleUtility, idout);
                        //for config page
                        tabinsertcheck(myconn, dgvModuleConfig, idout);
                        //for ticketing system  
                        tabinsertcheck(myconn, dgvModuleTicket, idout);
                    }

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    txtuserlevel.Text = "";

                    m_clscom.AddATrail(m_frmM.m_userid, "GRANT_ADD", "USER_LEVEL|USER_LEVEL_RIGHTS",
                           Environment.MachineName.ToString(), "ADDED NEW USER LEVEL: NAME=" + this.txtuserlevel.Text.Trim()
                           + " | ID=" + idout.ToString(), m_frmM._connection);

                    populateuserlevel();
                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the user level table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "GRANT_DELETE", m_frmM._connection);
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
                    string strid = String.Empty;
                    int intid = -1;
                    if (dgvUserLevel.SelectedRows.Count == 1)
                    {
                        strid = dgvUserLevel.SelectedRows[0].Cells[0].Value.ToString();
                        intid = Convert.ToInt32(dgvUserLevel.SelectedRows[0].Cells[1].Value.ToString());

                        string systemcode = dgvUserLevel.SelectedRows[0].Cells[2].Value.ToString();
                        for (int i = 0; i < cmbSystem.Items.Count; i++)
                        {
                            DataRowView drv = (DataRowView)cmbSystem.Items[i];
                            if (drv.Row["system_code"].ToString().ToUpper() == systemcode.ToUpper())
                            {
                                cmbSystem.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    //validate the database for records being used
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_level where level_desc = '{0}' and system_code = {1}", strid, cmbSystem.SelectedValue));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount == 0)
                    {
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from user_level where level_desc = '{0}' and system_code = {1}", strid, cmbSystem.SelectedValue));
                    try
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        //validate the database for records being used
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("select count(*) from user_level_rights where user_level = {0} and system_code = {1}", intid, cmbSystem.SelectedValue));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount2 > 0)
                        {
                            sqry = new StringBuilder();
                            sqry.Append(String.Format("delete from user_level_rights where user_level = {0} and system_code = {1}", intid, cmbSystem.SelectedValue));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        m_clscom.AddATrail(m_frmM.m_userid, "GRANT_DELETE", "USER_LEVEL|USER_LEVEL_RIGHTS",
                           Environment.MachineName.ToString(), "REMOVED USER LEVEL: NAME=" + this.txtuserlevel.Text.Trim()
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

            populateuserlevel();
            refreshDGV();
            setnormal();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "GRANT_EDIT", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            if (dgvUserLevel.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (dgvUserLevel.SelectedRows.Count == 1)
                dgvUserLevel.Enabled = false;

            if (btnEdit.Text == "edit")
            {
                this.txtuserlevel.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                grpgrant.Visible = true;
            }
            else if (btnEdit.Text == "update")
            {
                string strid = "0";
                string strstatus = String.Empty;
                if (txtuserlevel.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtuserlevel.SelectAll();
                    txtuserlevel.Focus();
                    return;
                }

                StringBuilder sqry = new StringBuilder();
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                MySqlCommand cmd = new MySqlCommand();
                int rowCount = -1;

                int intid = -1;
                if (dgvUserLevel.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvUserLevel.SelectedRows[0].Cells[1].Value.ToString());

                //validate for the existance of the record
                sqry = new StringBuilder();
                sqry.Append("select count(*) from user_level ");
                sqry.Append(String.Format("where level_desc = '{0}' ", this.txtuserlevel.Text.Trim()));
                sqry.Append(String.Format("and id != {0}", intid));

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
                //else if (rowCount == 1)
                //{
                //    setModule(myconn, intid);
                //    if (myconn.State == ConnectionState.Open)
                //        myconn.Close();
                //    refreshDGV();
                //    setnormal();
                //    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else if (rowCount == 0)
                //{
                StringBuilder strqry = new StringBuilder();
                strqry.Append(String.Format("update user_level set level_desc = '{0}'", txtuserlevel.Text.Trim()));
                strqry.Append(String.Format(" where id = {0}", intid));

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    setModule(myconn, intid);
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "GRANT_EDIT", "USER_LEVEL|USER_LEVEL_RIGHTS",
                       Environment.MachineName.ToString(), "UPDATED USER LEVEL: NAME=" + this.txtuserlevel.Text.Trim()
                       + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Can't connect to the user level table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //}
            }
        }

        public void setModule(MySqlConnection myconn, int intid)
        {
            //validate the database for records being used
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_level_rights where user_level = {0} and system_code = {1}", intid, cmbSystem.SelectedValue));
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
            int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();

            if (rowCount2 > 0)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("delete from user_level_rights where user_level = {0} and system_code = {1}", intid, cmbSystem.SelectedValue));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            //for cinema
            tabinsertcheck(myconn, dgvModuleCinema, intid);
            //for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
            //{
            //    if (dgvModuleCinema[0, i].Value != null)
            //    {
            //        if ((bool)dgvModuleCinema[0, i].Value)
            //        {
            //            sqry = new StringBuilder();
            //            sqry.Append(String.Format("insert into user_level_rights values(0,{0},{1},1)",
            //                intid, dgvModuleCinema[4, i].Value.ToString()));

            //            if (myconn.State == ConnectionState.Closed)
            //                myconn.Open();
            //            cmd = new MySqlCommand(sqry.ToString(), myconn);
            //            cmd.ExecuteNonQuery();
            //            cmd.Dispose();
            //        }
            //    }

            //}

            //for report page
            tabinsertcheck(myconn, dgvModuleReport, intid);
            //for (int i = 0; i < dgvModuleReport.Rows.Count; i++)
            //{
            //    if (dgvModuleReport[0, i].Value != null)
            //    {
            //        if ((bool)dgvModuleReport[0, i].Value)
            //        {
            //            sqry = new StringBuilder();
            //            sqry.Append(String.Format("insert into user_level_rights values(0,{0},{1},1)",
            //                intid, dgvModuleReport[4, i].Value.ToString()));

            //            if (myconn.State == ConnectionState.Closed)
            //                myconn.Open();
            //            cmd = new MySqlCommand(sqry.ToString(), myconn);
            //            cmd.ExecuteNonQuery();
            //            cmd.Dispose();
            //        }
            //    }
            //}
            //for utility page
            tabinsertcheck(myconn, dgvModuleUtility, intid);
            //for (int i = 0; i < dgvModuleUtility.Rows.Count; i++)
            //{
            //    if (dgvModuleUtility[0, i].Value != null)
            //    {
            //        if ((bool)dgvModuleUtility[0, i].Value)
            //        {
            //            sqry = new StringBuilder();
            //            sqry.Append(String.Format("insert into user_level_rights values(0,{0},{1},1)",
            //                intid, dgvModuleUtility[4, i].Value.ToString()));

            //            if (myconn.State == ConnectionState.Closed)
            //                myconn.Open();
            //            cmd = new MySqlCommand(sqry.ToString(), myconn);
            //            cmd.ExecuteNonQuery();
            //            cmd.Dispose();
            //        }
            //    }
            //}
            //for config page
            tabinsertcheck(myconn, dgvModuleConfig, intid);
            //for (int i = 0; i < dgvModuleConfig.Rows.Count; i++)
            //{
            //    if (dgvModuleConfig[0, i].Value != null)
            //    {
            //        if ((bool)dgvModuleConfig[0, i].Value)
            //        {
            //            sqry = new StringBuilder();
            //            sqry.Append(String.Format("insert into user_level_rights values(0,{0},{1},1)",
            //                intid, dgvModuleConfig[4, i].Value.ToString()));

            //            if (myconn.State == ConnectionState.Closed)
            //                myconn.Open();
            //            cmd = new MySqlCommand(sqry.ToString(), myconn);
            //            cmd.ExecuteNonQuery();
            //            cmd.Dispose();
            //        }
            //    }
            //} 
            //for ticketing system
            tabinsertcheck(myconn, dgvModuleTicket, intid);

            if (myconn.State == ConnectionState.Open)
                myconn.Close();
        }

        public void tabinsertcheck(MySqlConnection myconn, DataGridView dgv, int intid)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv[0, i].Value != null)
                {
                    if ((bool)dgv[0, i].Value)
                    {
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into user_level_rights values(0,{0},{1},{2})",
                            intid, dgv[4, i].Value.ToString(), cmbSystem.SelectedValue));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
        }

        private void tabModule_SelectedPageChanged(object sender, EventArgs e)
        {
            int cntchk = 0;
            int cnt = 0;
            if (tabModule.SelectedPage == pageCinema)
            {
                cnt = dgvModuleCinema.Rows.Count;
                for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
                {
                    if (dgvModuleCinema[0, i].Value != null)
                    {
                        bool ok = false;
                        if (Boolean.TryParse(dgvModuleCinema[0, i].Value.ToString(), out ok))
                        {
                            if (ok)
                                cntchk += 1;
                        }
                    }
                }
            }
            else if (tabModule.SelectedPage == pageReport)
            {
                cnt = dgvModuleReport.Rows.Count;
                for (int i = 0; i < dgvModuleReport.Rows.Count; i++)
                {
                    if (dgvModuleReport[0, i].Value != null)
                    {
                        bool ok = false;
                        if (Boolean.TryParse(dgvModuleReport[0, i].Value.ToString(), out ok))
                        {
                            if (ok)
                                cntchk += 1;
                        }
                    }
                }
            }
            else if (tabModule.SelectedPage == pageUtility)
            {
                cnt = dgvModuleUtility.Rows.Count;
                for (int i = 0; i < dgvModuleUtility.Rows.Count; i++)
                {
                    if (dgvModuleUtility[0, i].Value != null)
                    {
                        bool ok = false;
                        if (Boolean.TryParse(dgvModuleUtility[0, i].Value.ToString(), out ok))
                        {
                            if (ok)
                                cntchk += 1;
                        }
                    }
                }
            }
            else if (tabModule.SelectedPage == pageConfig)
            {
                cnt = dgvModuleConfig.Rows.Count;
                for (int i = 0; i < dgvModuleConfig.Rows.Count; i++)
                {
                    if (dgvModuleConfig[0, i].Value != null)
                    {
                        bool ok = false;
                        if (Boolean.TryParse(dgvModuleConfig[0, i].Value.ToString(), out ok))
                        {
                            if (ok)
                                cntchk += 1;
                        }
                    }
                }
            }

            txtcnt.Text = String.Format("Count: {0} / {1}", cntchk.ToString(), cnt.ToString());
        }

        private void frmUserLevel_Load(object sender, EventArgs e)
        {

        }

        private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cmbSystem.SelectedValue.ToString() == "1")
            {
                //setuserlevel(cmbSystem.SelectedIndex);

                dgvModuleCinema.Enabled = true;
                dgvModuleConfig.Enabled = true;
                dgvModuleReport.Enabled = true;
                dgvModuleUtility.Enabled = true;
               // dgvModuleTicket.Enabled = false;

                tabModule.SelectedPage = pageCinema;
                txtuserlevel.Focus();
            }
            else if (cmbSystem.SelectedValue.ToString() == "2")
            {
                dgvModuleCinema.Enabled = false;
                dgvModuleConfig.Enabled = false;
                dgvModuleReport.Enabled = false;
                dgvModuleUtility.Enabled = false;
                dgvModuleTicket.Enabled = true;

                if (tabModule.SelectedPage == pageCinema)
                    setCheck(dgvModuleCinema, false);
                else if (tabModule.SelectedPage == pageReport)
                    setCheck(dgvModuleReport, false);
                else if (tabModule.SelectedPage == pageUtility)
                    setCheck(dgvModuleUtility, false);
                else if (tabModule.SelectedPage == pageConfig)
                    setCheck(dgvModuleConfig, false);
                else if (tabModule.SelectedPage == pageTicket)
                    setCheck(dgvModuleTicket, false);

                tabModule.SelectedPage = pageTicket;
                txtuserlevel.Focus();
            }
        }

        private void dgvUserLevel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvModuleTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
