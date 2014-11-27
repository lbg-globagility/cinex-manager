using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Amellar.Common.EncryptUtilities;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmUser : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        DataTable dtmodule = new DataTable();
        DataTable dtuserlevel = new DataTable();
        DataTable dtusers = new DataTable();
        DataTable dtsystems = new DataTable();
        MySqlConnection myconn = null;

        public frmUser()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;

            refreshDGV();

            grpcontrol.Visible = true;
            grpgrant.Visible = false;
            grpfilter.Visible = false;

            //populateuserlevel();
            populatesystem();
            unselectbutton();
            setnormal();
 
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

        public void populateuserlevel(int intsystemcode)
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.level_desc, a.id ");
            sbqry.Append(String.Format("from user_level a where a.system_code = {0} order by a.level_desc asc", intsystemcode));
            dtuserlevel = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
           // MessageBox.Show(sbqry.ToString());
            DataRow row = dtuserlevel.NewRow();
            row["id"] = "0";
            row["level_desc"] = "";
            dtuserlevel.Rows.InsertAt(row, 0);
            cmbAuth.DataSource = dtuserlevel;
            cmbAuth.DisplayMember = "level_desc";
            cmbAuth.ValueMember = "id";
        }

        public void setnormal()
        {
            txtUID.Text = "";
            txtLName.Text = "";
            txtFName.Text = "";
            txtMName.Text = "";
            txtDes.Text = "";
            txtPW.Text = "";
            txtCPW.Text = "";
            cmbAuth.SelectedIndex = 0;
            txtUID.ReadOnly = true;
            txtLName.ReadOnly = true;
            txtFName.ReadOnly = true;
            txtMName.ReadOnly = true;
            txtDes.ReadOnly = true;
            txtPW.ReadOnly = true;
            txtCPW.ReadOnly = true;

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
            grpgrant.Visible = false;
            cmbAuth.Enabled = false;
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.userid, a.lname, a.fname, a.designation, b.level_desc,(case when a.status=1 then 'Active' else 'Inactive' end) as Status, a.mname, a.user_password, c.system_name  ");
            sbqry.Append("from users a left join user_level b on a.user_level_id = b.id ");
            sbqry.Append("left join application c on a.system_code = c.system_code ");
            //rmb 11.26.2014 REMOVED GROUP BY
            //sbqry.Append(" group by a.userid order by a.userid asc");//where a.system_code = 1 
            sbqry.Append("order by a.userid asc");
            dtusers = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvResult, dtusers);
            txtFound.Text = "Count: " + dtusers.Rows.Count.ToString();

        }

        public void setDataGridView(DataGridView dgv, DataTable dt)
        {
            dgv.Columns.Clear();
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dgv.DataSource = dt;
                int iwidth = dgv.Width / 8;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth;
                dgv.Columns[1].HeaderText = "User ID";
                dgv.Columns[2].Width = iwidth * 2 - 10;
                dgv.Columns[2].HeaderText = "Last Name";
                dgv.Columns[3].Width = iwidth * 2 - 10;
                dgv.Columns[3].HeaderText = "First Name";
                dgv.Columns[4].Width = iwidth * 2 - 10;
                dgv.Columns[4].HeaderText = "Designation";
                dgv.Columns[5].Width = iwidth * 2 - 10;
                dgv.Columns[5].HeaderText = "User Level";
                dgv.Columns[6].Width = iwidth * 2 - 10;
                dgv.Columns[6].HeaderText = "Status";
                dgv.Columns[7].Width = 0;
                dgv.Columns[7].HeaderText = "Middle Initial";
                dgv.Columns[8].Width = 0;
                dgv.Columns[8].HeaderText = "Password";
                dgv.Columns[9].Width = 0;
                dgv.Columns[9].HeaderText = "System";
            }

   
   
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {

        }

        private void btnclear2_Click(object sender, EventArgs e)
        {
            unselectbutton();
            txtUID.Text = "";
            txtLName.Text = "";
            txtFName.Text = "";
            txtMName.Text = "";
            txtDes.Text = "";
            txtPW.Text = "";
            txtCPW.Text = "";
            cmbAuth.SelectedIndex = 0;
        }

        private void btngrant_Click(object sender, EventArgs e)
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
            else if (tabModule.SelectedPage == pageTicket)
                setCheck(dgvModuleTicket, true);
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
           
            //txtcnt.Text = "Count: " + cnt.ToString() + " / " + dgv.Rows.Count.ToString();
        }

        private void btnrevoke_Click(object sender, EventArgs e)
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
            else if (tabModule.SelectedPage == pageTicket)
                setCheck(dgvModuleTicket, false);
        }

        private void cmbAuth_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectAuth();
        }

        private void selectAuth()
        {
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            int chklst = 0;
            string sid = String.Empty;
            int intid = -1;
            int intuserid = -1;
            var val = cmbAuth.SelectedValue;
            if (val.ToString() != "System.Data.DataRowView")
                sid = cmbAuth.SelectedValue.ToString();

            if (!int.TryParse(sid, out intid))
                intid = -1;

            if (intid != -1)
            {
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select b.module_desc, b.module_code, b.module_group, a.module_id ");
                sbqry.Append("from user_level_rights a left join system_module b ");
                sbqry.Append("on a.module_id = b.id ");
                sbqry.Append(String.Format("where a.user_level = {0} ", intid));
                sbqry.Append(String.Format("and a.system_code = {0}", cmbSystem.SelectedValue.ToString()));
                //MessageBox.Show(sbqry.ToString());
                dtmodule = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            }
            if (dtmodule.Rows.Count > 0)
            {
                DataTable dtmodulecinema = new DataTable();

                //for cinema
                string sqry = "[module_group] = 'CINEMA'";
                var foundRows = dtmodule.Select(sqry);
                if (foundRows.Count() == 0)
                    dtmodulecinema = new DataTable();
                else
                    dtmodulecinema = foundRows.CopyToDataTable();

                if (dtmodulecinema.Rows.Count > 0)
                    setDataGridViewII(dgvModuleCinema, dtmodulecinema);
                else
                {
                    dgvReset(dgvModuleCinema);
                }
               
                //for reports
                sqry = "[module_group] = 'REPORT'";
                foundRows = null;
                foundRows = dtmodule.Select(sqry);
                DataTable dtmodulereport = new DataTable();
                if (foundRows.Count() > 0)
                    dtmodulereport = foundRows.CopyToDataTable();
                if (dtmodulereport.Rows.Count > 0)
                    setDataGridViewII(dgvModuleReport, dtmodulereport);
                else
                {
                    dgvReset(dgvModuleReport);
                }                //for utility
                sqry = "[module_group] = 'UTILITY'";
                foundRows = null;
                foundRows = dtmodule.Select(sqry);
                DataTable dtmoduleutility = new DataTable();
                if (foundRows.Count() > 0)
                    dtmoduleutility = foundRows.CopyToDataTable();
                if (dtmoduleutility.Rows.Count > 0)
                    setDataGridViewII(dgvModuleUtility, dtmoduleutility);
                else
                {
                    dgvReset(dgvModuleUtility);
                }
                //for config
                sqry = "[module_group] = 'CONFIG'";
                foundRows = null;
                foundRows = dtmodule.Select(sqry);
                DataTable dtmoduleuconfig = new DataTable();
                if (foundRows.Count() > 0)
                    dtmoduleuconfig = foundRows.CopyToDataTable();
                if (dtmoduleuconfig.Rows.Count > 0)
                    setDataGridViewII(dgvModuleConfig, dtmoduleuconfig);
                else
                {
                    dgvReset(dgvModuleConfig);
                }
                //for ticket
                sqry = "[module_group] = 'TICKET'";
                foundRows = null;
                foundRows = dtmodule.Select(sqry);
                DataTable dtmoduleuticket = new DataTable();
                if (foundRows.Count() > 0)
                    dtmoduleuticket = foundRows.CopyToDataTable();
                if (dtmoduleuticket.Rows.Count > 0)
                    setDataGridViewII(dgvModuleTicket, dtmoduleuticket);

                    //melvin for reseting value of datagrid
                else
                {
                    dgvReset(dgvModuleTicket);
                    //dgvModuleTicket.DataSource = null;
                    //dgvModuleTicket.Rows.Clear();
                    //int iwidth = dgvModuleTicket.Width / 3;
                    //DataGridViewColumn c1 = new DataGridViewColumn();
                    //DataGridViewColumn c2 = new DataGridViewColumn();
                    //DataGridViewColumn c3 = new DataGridViewColumn();
                    //DataGridViewColumn c4 = new DataGridViewColumn();
                    //DataGridViewColumn c5 = new DataGridViewColumn();
                    //dgvModuleTicket.Columns.Add(c1);
                    //dgvModuleTicket.Columns.Add(c2);
                    //dgvModuleTicket.Columns.Add(c3);
                    //dgvModuleTicket.Columns.Add(c4);
     
                    //dgvModuleTicket.Columns.Add(c5);
                    //dgvModuleTicket.Columns[0].Width = 30;
                    //dgvModuleTicket.Columns[1].Width = iwidth * 3 - 45;
                    //dgvModuleTicket.Columns[1].HeaderText = "Module Description";
                    //dgvModuleTicket.Columns[2].ReadOnly = true;
                    //dgvModuleTicket.Columns[2].Width = 0;
                    //dgvModuleTicket.Columns[3].HeaderText = "Module Code";
                    //dgvModuleTicket.Columns[3].Width = 0;
                    //dgvModuleTicket.Columns[4].HeaderText = "Module Group";
                    //dgvModuleTicket.Columns[4].Width = 0;
                    //dgvModuleTicket.Columns[5].HeaderText = "Module ID";
                    
                }

                if (dgvResult.SelectedRows.Count != 1)
                {
                    setCheck(dgvModuleCinema, false);
                    setCheck(dgvModuleReport, false);
                    setCheck(dgvModuleUtility, false);
                    setCheck(dgvModuleConfig, false);
                    setCheck(dgvModuleTicket, false);
                    return;
                }
                else
                {
                    if (!int.TryParse(dgvResult.SelectedRows[0].Cells[0].Value.ToString(), out intuserid))
                        intuserid = -1;
                }

                if (intuserid != -1)
                {
                    int rowCount = 0;
                    StringBuilder sqry2 = new StringBuilder();
                    // check cinema
                    for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
                    {
                        int intmid = Convert.ToInt32(dgvModuleCinema[4, i].Value.ToString());
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from user_rights a where a.user_id = {0}", intuserid));
                        sqry2.Append(String.Format(" and a.module_id = {0}", intmid));
                        sqry2.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "CINEMA"));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                        {
                            dgvModuleCinema[0, i].Value = (object)true;
                            chklst += 1;
                        }
                    }
                    // check report
                    for (int i = 0; i < dgvModuleReport.Rows.Count; i++)
                    {
                        int intmid = Convert.ToInt32(dgvModuleReport[4, i].Value.ToString());
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from user_rights a where a.user_id = {0}", intuserid));
                        sqry2.Append(String.Format(" and a.module_id = {0}", intmid));
                        sqry2.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "REPORT"));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                        {
                            dgvModuleReport[0, i].Value = (object)true;
                            chklst += 1;
                        }
                    }
                    // check report
                    for (int i = 0; i < dgvModuleUtility.Rows.Count; i++)
                    {
                        int intmid = Convert.ToInt32(dgvModuleUtility[4, i].Value.ToString());
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from user_rights a where a.user_id = {0}", intuserid));
                        sqry2.Append(String.Format(" and a.module_id = {0}", intmid));
                        sqry2.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "UTILITY"));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                        {
                            dgvModuleUtility[0, i].Value = (object)true;
                            chklst += 1;
                        }
                    }
                    // check config
                    for (int i = 0; i < dgvModuleConfig.Rows.Count; i++)
                    {
                        int intmid = Convert.ToInt32(dgvModuleConfig[4, i].Value.ToString());
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from user_rights a where a.user_id = {0}", intuserid));
                        sqry2.Append(String.Format(" and a.module_id = {0}", intmid));
                        sqry2.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "CONFIG"));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                        {
                            dgvModuleConfig[0, i].Value = (object)true;
                            chklst += 1;
                        }
                    }

                    // check ticket
                    for (int i = 0; i < dgvModuleTicket.Rows.Count; i++)
                    {
                        int intmid = Convert.ToInt32(dgvModuleTicket[4, i].Value.ToString());
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from user_rights a where a.user_id = {0}", intuserid));
                        sqry2.Append(String.Format(" and a.module_id = {0}", intmid));
                        sqry2.Append(String.Format(" and a.module_id in(select b.id from system_module b where b.module_group = '{0}')", "TICKET"));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                        {
                            dgvModuleTicket[0, i].Value = (object)true;
                            chklst += 1;
                        }
                    }

                    //RMB added validation for the user not showing if the
                    //system is different
                    if (cmbSystem.SelectedValue.ToString() == "1")
                    {
                        dgvModuleTicket.DataSource = null;
                        dgvModuleTicket.Rows.Clear();
                    }
                    else
                    {
                        dgvModuleCinema.DataSource = null;
                        dgvModuleCinema.Rows.Clear();
                        dgvModuleReport.DataSource = null; 
                        dgvModuleReport.Rows.Clear();
                        dgvModuleUtility.DataSource = null;
                        dgvModuleUtility.Rows.Clear();
                        dgvModuleConfig.DataSource = null;
                        dgvModuleConfig.Rows.Clear();
                    }
                }
            }
        }

        private void dgvReset(DataGridView dt)
        {
            //melvin 10-30-2014
            dt.DataSource = null;
            dt.Rows.Clear();
            int iwidth = dgvModuleTicket.Width / 3;
            DataGridViewColumn c1 = new DataGridViewColumn();
            DataGridViewColumn c2 = new DataGridViewColumn();
            DataGridViewColumn c3 = new DataGridViewColumn();
            DataGridViewColumn c4 = new DataGridViewColumn();
            DataGridViewColumn c5 = new DataGridViewColumn();
            dt.Columns.Add(c1);
            dt.Columns.Add(c2);
            dt.Columns.Add(c3);
            dt.Columns.Add(c4);

            dt.Columns.Add(c5);
            dt.Columns[0].Width = 30;
            dt.Columns[1].Width = iwidth * 3;
            dt.Columns[1].HeaderText = "Module Description";
            dt.Columns[2].ReadOnly = true;
            dt.Columns[2].Width = 0;
            dt.Columns[3].HeaderText = "Module Code";
            dt.Columns[3].Width = 0;
            dt.Columns[4].HeaderText = "Module Group";
            dt.Columns[4].Width = 0;
        }
        public void tabinsertcheck(MySqlConnection myconn, DataGridView dgv, StringBuilder sqry)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv[0, i].Value != null)
                {
                    if ((bool)dgv[0, i].Value)
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
        }

        public void setDataGridViewII(DataGridView dgv, DataTable dt)
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
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "Module Group";
                dgv.Columns[3].Width = 0;
                dgv.Columns[3].HeaderText = "Module ID";
                dgv.Columns.Insert(0, cbx);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "USER_ADD", m_frmM._connection);
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
                txtUID.Text = "";
                txtLName.Text = "";
                txtFName.Text = "";
                txtMName.Text = "";
                txtDes.Text = "";
                txtPW.Text = "";
                txtCPW.Text = "";
                txtUID.ReadOnly = false;
                txtLName.ReadOnly = false;
                txtFName.ReadOnly = false;
                txtMName.ReadOnly = false;
                txtDes.ReadOnly = false;
                txtPW.ReadOnly = false;
                txtCPW.ReadOnly = false;

                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtUID.SelectAll();
                txtUID.Focus();
                grpgrant.Visible = true;

                dgvResult.Enabled = false;

                cmbAuth.Enabled = true;
                dgvResult.ClearSelection();

                setCheck(dgvModuleCinema, false);
                setCheck(dgvModuleReport, false);
                setCheck(dgvModuleUtility, false);
                setCheck(dgvModuleConfig, false);
                setCheck(dgvModuleTicket, false);

                cmbSystem.SelectedIndex = 0;

                KryptonDataGridView dgv = sender as KryptonDataGridView;

                
                for (int i = 0; i < cmbSystem.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbSystem.Items[i];
                    if (!(drv.Row["system_name"].ToString()==""))
                    {
                        cmbSystem.SelectedIndex = i;
                        break;
                    }
                }

               // populateuserlevel(cmbSystem.SelectedIndex);

                //string strname = dgv.SelectedRows[0].Cells[5].Value.ToString();
                for (int i = 0; i < cmbAuth.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbAuth.Items[i];
                    if (!(drv.Row["level_desc"].ToString()==""))
                    {
                        cmbAuth.SelectedIndex = i;
                        break;
                    }
                }

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select b.module_desc, b.module_code, b.module_group, a.module_id ");
                sbqry.Append("from user_level_rights a left join system_module b ");
                sbqry.Append("on a.module_id = b.id ");
                sbqry.Append("where a.user_level = 2 and a.system_code =2");
                dtmodule = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

             
            }
            else
            {
                string strstatus = String.Empty;
                if (txtUID.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtUID.SelectAll();
                    txtUID.Focus();
                    return;
                }
                if (txtLName.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtLName.SelectAll();
                    txtLName.Focus();
                    return;
                }
                if (txtFName.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtFName.SelectAll();
                    txtFName.Focus();
                    return;
                }
                if (this.txtDes.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtDes.SelectAll();
                    txtDes.Focus();
                    return;
                }
                if (this.txtPW.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtPW.SelectAll();
                    txtPW.Focus();
                    return;
                }
                if (this.txtCPW.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtCPW.SelectAll();
                    txtCPW.Focus();
                    return;
                }
                if (txtPW.Text.Trim() != txtCPW.Text.Trim())
                {
                    MessageBox.Show("Please check your password.");
                    txtCPW.SelectAll();
                    txtCPW.Focus();
                    return;
                }

                int chkcnt = 0;
                if (cmbSystem.SelectedValue.ToString() == "1")
                {
                    for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
                    {
                        if (dgvModuleCinema[0, i].Value != null)
                        {
                            if ((bool)dgvModuleCinema[0, i].Value)
                            {
                                chkcnt += 1;
                            }
                        }
                    }
                }
                else if (cmbSystem.SelectedValue.ToString() == "2")
                {
                    for (int i = 0; i < dgvModuleTicket.Rows.Count; i++)
                    {
                        if (dgvModuleTicket[0, i].Value != null)
                        {
                            if ((bool)dgvModuleTicket[0, i].Value)
                            {
                                chkcnt += 1;
                            }
                        }
                    }
                }
                if (chkcnt == 0)
                {
                    MessageBox.Show("Please check the required grants \n\r for the user level.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUID.SelectAll();
                    txtUID.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from users ");
                sqry.Append(String.Format("where userid = '{0}' ", this.txtUID.Text.ToUpper().Trim()));
                //melvin added for multiple access of account
                sqry.Append(string.Format("and user_level_id = {0} ", cmbAuth.SelectedValue.ToString()));
                
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
               
               // MessageBox.Show(cmbAuth.SelectedText.ToString() + "-" + cmbSystem.SelectedText.ToString());
                Encryption encrypt = new Encryption();
                sqry = new StringBuilder();
               /* sqry.Append(String.Format("insert into users values(0,'{0}','{1}','{2}',{3},'{4}','{5}','{6}',{7},1)",
                    this.txtUID.Text.ToUpper().Trim(), encrypt.EncryptString(txtPW.Text.Trim()).ToString(), txtDes.Text.ToUpper().Trim(), cmbAuth.SelectedValue.ToString(),
                    txtLName.Text.ToUpper().Trim(), txtFName.Text.ToUpper().Trim(), txtMName.Text.ToUpper().Trim(), cmbSystem.SelectedValue.ToString()));*/
                string pw= encrypt.EncryptString(txtPW.Text.Trim()).ToString();
                
                sqry.Append("insert into users values(0,@userid,@pw,@des,@auth,@lname,@fname,@mname,@system,1)");
               
                try
                {
                    int idout = -1;
                
                        idout = -1;
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        if (myconn.State == ConnectionState.Open)
                        {
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.Parameters.AddWithValue("@userid", txtUID.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@pw", pw);
                            cmd.Parameters.AddWithValue("@des", txtDes.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@auth", cmbAuth.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@lname", txtLName.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@fname", txtFName.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@mname", txtMName.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@system",cmbSystem.SelectedValue.ToString());
                            
                            cmd.ExecuteNonQuery();
                            idout = Convert.ToInt32(cmd.LastInsertedId);
                            cmd.Dispose();
                        }
                        if (idout > -1)
                        {
                            //for cinema
                            tabinsertcheck(myconn, dgvModuleCinema, idout);
                            //for report
                            tabinsertcheck(myconn, dgvModuleReport, idout);
                            //for utility
                            tabinsertcheck(myconn, dgvModuleUtility, idout);
                            //for config
                            tabinsertcheck(myconn, dgvModuleConfig, idout);
                            //for ticketing
                            tabinsertcheck(myconn, dgvModuleTicket, idout);
                        }
 
                     
                   
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "USER_ADD", "USERS|USER_RIGHTS",
                               Environment.MachineName.ToString(), "ADD NEW SYSTEM USER: USERNAME=" + this.txtUID.Text
                               + " | ID=" + idout.ToString(), m_frmM._connection);


                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the user table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
            }
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
                        sqry.Append(String.Format("insert into user_rights values(0,{0},{1})",
                            intid, dgv[4, i].Value.ToString()));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "USER_DELETE", m_frmM._connection);
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
                    if (dgvResult.SelectedRows.Count == 1)
                    {
                        strid = dgvResult.SelectedRows[0].Cells[1].Value.ToString();
                        intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                    }

                    //validate the database for records being used
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from user_logs where user_authority = '{0}'", strid));
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
                    //sqry.Append(String.Format("select count(*) from users where id = {0} and system_code = {1}", intid, "1"));
                    sqry.Append(String.Format("select count(*) from users where id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
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
                    sqry.Append(String.Format("update users set status=0 where id = '{0}' ", intid));
                    try
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        //validate the database for records being used
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("select count(*) from user_rights where user_id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount2 > 0)
                        {
                            sqry = new StringBuilder();
                            sqry.Append(String.Format("delete from user_rights where user_id = {0}", intid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        m_clscom.AddATrail(m_frmM.m_userid, "USER_DELETE", "USERS|USER_RIGHTS",
                           Environment.MachineName.ToString(), "REMOVED SYSTEM USER: USERNAME=" + this.txtUID.Text.Trim()
                           + " | ID=" + strid, m_frmM._connection);


                        MessageBox.Show("You have successfully inactivate \n\rthe selected record. This account cannot login now", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            unselectbutton();
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "USER_EDIT", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            if (dgvResult.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (btnEdit.Text == "edit")
            {
                txtUID.ReadOnly = true;
                txtLName.ReadOnly = false;
                txtFName.ReadOnly = false;
                txtMName.ReadOnly = false;
                txtDes.ReadOnly = false;
                txtPW.ReadOnly = false;
                txtCPW.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                grpgrant.Visible = true;
                txtLName.SelectAll();
                txtLName.Focus();
                cmbAuth.Enabled = true;
                
                //RMB 11.6.2014 disable selection from the list of user
                dgvResult.Enabled = false;

            }
            else if (btnEdit.Text == "update")
            {
                string strstatus = String.Empty;
                if (txtUID.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtUID.SelectAll();
                    txtUID.Focus();
                    return;
                }
                if (txtLName.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtLName.SelectAll();
                    txtLName.Focus();
                    return;
                }
                if (txtFName.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtFName.SelectAll();
                    txtFName.Focus();
                    return;
                }
                if (this.txtDes.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtDes.SelectAll();
                    txtDes.Focus();
                    return;
                }
                if (this.txtPW.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtPW.SelectAll();
                    txtPW.Focus();
                    return;
                }
                if (this.txtCPW.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtCPW.SelectAll();
                    txtCPW.Focus();
                    return;
                }
                if (txtPW.Text.Trim() != txtCPW.Text.Trim())
                {
                    MessageBox.Show("Please check your password.");
                    txtCPW.SelectAll();
                    txtCPW.Focus();
                    return;
                }

                StringBuilder sqry = new StringBuilder();
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                MySqlCommand cmd = new MySqlCommand();
                int rowCount = -1;

                string strid = String.Empty;
                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                {
                    strid = dgvResult.SelectedRows[0].Cells[1].Value.ToString();
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                }

                //validate for the existance of the logs table record
                sqry = new StringBuilder();
                sqry.Append("select count(*) from user_logs ");
                sqry.Append(String.Format("where user_id = '{0}' ", strid));

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
                    MessageBox.Show("Can't update this user's record, \n\rit is currently logged in the system.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                //validate for the existance of the record
                sqry = new StringBuilder();
                sqry.Append("select count(*) from users ");
                sqry.Append(String.Format("where userid = '{0}' ", this.txtUID.Text.Trim()));
                sqry.Append(String.Format("and system_code = {0} ", cmbSystem.SelectedValue.ToString()));
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
                    //setModule(myconn, intid);
                    //if (myconn.State == ConnectionState.Open)
                    //    myconn.Close();

                    //refreshDGV();
                    //setnormal();
                    //MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Encryption encrypt = new Encryption();
                    StringBuilder strqry = new StringBuilder();
                    strqry.Append(String.Format("update users set lname = '{0}', ", this.txtLName.Text.Trim()));
                    strqry.Append(String.Format("fname = '{0}', ", this.txtFName.Text.ToUpper().Trim()));
                    strqry.Append(String.Format("user_password = '{0}', ", encrypt.EncryptString(this.txtPW.Text.Trim())));
                    strqry.Append(String.Format("mname = '{0}', ", this.txtMName.Text.ToUpper().Trim()));
                    strqry.Append(String.Format("designation = '{0}', ", this.txtDes.Text.ToUpper().Trim()));
                    strqry.Append(String.Format("status = {0}, ", "1"));
                    strqry.Append(String.Format("user_level_id = {0} ", this.cmbAuth.SelectedValue.ToString()));
                    strqry.Append(String.Format("where userid = '{0}' ", strid));
                    strqry.Append(String.Format("and id = {0}", intid));

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

                        m_clscom.AddATrail(m_frmM.m_userid, "USER_EDIT", "USERS|USER_RIGHTS",
                           Environment.MachineName.ToString(), "UPDATED SYSTEM USER: USERNAME=" + this.txtUID.Text.Trim()
                           + " | ID=" + strid, m_frmM._connection);

                        refreshDGV();
                        setnormal();

                        MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Can't connect to the user level table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void setModule(MySqlConnection myconn, int intid)
        {
            //validate the database for records being used
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_rights where user_id = {0}", intid));
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
            int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();

            if (rowCount2 > 0)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("delete from user_rights where user_id = {0}", intid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            //for cinema
            tabinsertcheck(myconn, dgvModuleCinema, intid);
            //for report
            tabinsertcheck(myconn, dgvModuleReport, intid);
            //for utility
            tabinsertcheck(myconn, dgvModuleUtility, intid);
            //for config
            tabinsertcheck(myconn, dgvModuleConfig, intid);
            //for ticket
            tabinsertcheck(myconn, dgvModuleTicket, intid);
            //for (int i = 0; i < dgvModuleCinema.Rows.Count; i++)
            //{
            //    if (dgvModuleCinema[0, i].Value != null)
            //    {
            //        if ((bool)dgvModuleCinema[0, i].Value)
            //        {
            //            sqry = new StringBuilder();
            //            sqry.Append(String.Format("insert into user_rights values(0,{0},{1})",
            //                intid, dgvModuleCinema[3, i].Value.ToString()));

            //            if (myconn.State == ConnectionState.Closed)
            //                myconn.Open();
            //            cmd = new MySqlCommand(sqry.ToString(), myconn);
            //            cmd.ExecuteNonQuery();
            //            cmd.Dispose();
            //        }
            //    }
            //}


            if (myconn.State == ConnectionState.Open)
                myconn.Close();
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Encryption encrypt = new Encryption();
            int chklst = 0;

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
                cmbAuth.SelectedIndex = 0;
                btnEdit.Enabled = true;
                btnEdit.Text = "edit";

                btnDelete.Enabled = true;
                btnDelete.Text = "remove";

                txtUID.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                txtLName.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                txtFName.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();
                txtMName.Text = dgv.SelectedRows[0].Cells[7].Value.ToString();
                txtDes.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();
                txtPW.Text = encrypt.DecryptString(dgv.SelectedRows[0].Cells[8].Value.ToString());
                txtCPW.Text = encrypt.DecryptString(dgv.SelectedRows[0].Cells[8].Value.ToString());

                //txtSystem.Text = dgv.SelectedRows[0].Cells[8].Value.ToString();
                string systemname = dgv.SelectedRows[0].Cells[9].Value.ToString();
                for (int i = 0; i < cmbSystem.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbSystem.Items[i];
                    if (drv.Row["system_name"].ToString().ToUpper() == systemname.ToUpper())
                    {
                        cmbSystem.SelectedIndex = i;
                        break;
                    }
                }

                populateuserlevel(cmbSystem.SelectedIndex);

                string strname = dgv.SelectedRows[0].Cells[5].Value.ToString();
                for (int i = 0; i < cmbAuth.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)cmbAuth.Items[i];
                    if (drv.Row["level_desc"].ToString().ToUpper() == strname.ToUpper())
                    {
                        cmbAuth.SelectedIndex = i;
                        break;
                    }
                }
            }
         //   MessageBox.Show(dgvResult.Rows[0].Cells[6].Value.ToString());

        }

        private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateuserlevel(cmbSystem.SelectedIndex);
            if (btnAdd.Text == "save")
            {
                if (cmbSystem.SelectedValue.ToString() == "1")
                {
                    dgvModuleCinema.Enabled = true;
                    dgvModuleConfig.Enabled = true;
                    dgvModuleReport.Enabled = true;
                    dgvModuleUtility.Enabled = true;
                    dgvModuleTicket.Enabled = true;

                    tabModule.SelectedPage = pageCinema;

                    ///
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
                    tabModule.SelectedPage = pageTicket;
                }
            }
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //melvin 11-3-2014
            for (int x = 0; x < dgvResult.Rows.Count; x++)
            {
                if (dgvResult[6, x].Value.ToString() == "Inactive")
                {
                  //  dgvResult.Rows[x].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvResult.Rows[x].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            if (dgvResult.Rows[0].Cells[6].Value.ToString() == "Inactive")
            {
                dgvResult.Rows[0].DefaultCellStyle.ForeColor= Color.Red;
            }
        }

        private void dgvResult_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //melvin 11-4-2014
            //if (dgvResult.SelectedRows[0].Cells[6].Value.ToString() == "Inactive")
          //  {
            //    dgvResult.DefaultCellStyle.SelectionForeColor = Color.Red;
            //}
            //else
            //{
            //    dgvResult.DefaultCellStyle.SelectionForeColor = Color.Black;
            //}
        }
    }
}
