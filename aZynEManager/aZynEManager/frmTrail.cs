using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace aZynEManager
{
    public partial class frmTrail : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        DataTable m_dttrail = new DataTable();
        DataTable m_dtmodule = new DataTable();
        MySqlConnection myconn = new MySqlConnection();

        public frmTrail()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            if (myconn == null)
                myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            refreshDGV();
            grpfilter.Visible = true;

            populatemodules();

            unselectbutton();
            setnormal();
            frdate.Value = DateTime.Now;
            todate.Value = DateTime.Now;
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void populatemodules()
        {
            cmbmodulegrp.Items.Clear();
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.module_desc, a.module_group, module_code ");
            sbqry.Append("from system_module a order by module_desc asc");
            m_dtmodule = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

            if(m_dtmodule.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataView view = new DataView(m_dtmodule);
                view.Sort = "module_group";
                DataTable distinctValues = view.ToTable(true, "module_group");
                cmbmodulegrp.Items.Add("");
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    cmbmodulegrp.Items.Add(distinctValues.Rows[i]["module_group"].ToString().Trim());
                }
            }
        }

        public void setnormal()
        {
            cbxdate.Checked = true;
            if(cmbmodule.Items.Count > 0)
               cmbmodule.SelectedIndex = 0;
            if(cmbmodulegrp.Items.Count > 0)
                cmbmodulegrp.SelectedIndex = 0;
            txtuser.Text = "";
            txtcn.Text = "";
            txtdetails.Text = "";
            txtFound.Text = String.Format("Count: {0}", dgvResult.Rows.Count);

            dgvResult.Enabled = true;

            setDataGridView(dgvResult, m_dttrail);
            txtFound.Text = "Count: " + m_dttrail.Rows.Count.ToString();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.tr_date, b.userid, ");
            sbqry.Append("a.tr_details, c.module_desc, c.module_group, a.computer_name, concat(b.fname , ' ',b.lname) username ");
            sbqry.Append("from a_trail a ");
            sbqry.Append("left join users b on a.user_id = b.id ");
            sbqry.Append("left join system_module c on trim(a.module_code) = trim(c.id) ");
            sbqry.Append("order by tr_date desc");
            m_dttrail = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvResult, m_dttrail);
            txtFound.Text = "Count: " + m_dttrail.Rows.Count.ToString();
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
                dgv.Columns[1].Width = iwidth * 2;
                dgv.Columns[1].HeaderText = "Date/Time";
                dgv.Columns[2].Width = iwidth * 2;
                dgv.Columns[2].HeaderText = "User ID";
                dgv.Columns[3].Width = iwidth * 3;
                dgv.Columns[3].HeaderText = "Details";
                dgv.Columns[4].Width = iwidth * 2;
                dgv.Columns[4].HeaderText = "Module";
                dgv.Columns[5].Width = iwidth * 2;
                dgv.Columns[5].HeaderText = "Module Group";
                dgv.Columns[6].Width = iwidth * 2;
                dgv.Columns[6].HeaderText = "Computer Name";
                dgv.Columns[7].Width = 0;
                dgv.Columns[7].HeaderText = "User Name";
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if(cmbmodule.Items.Count > 0)
                cmbmodule.SelectedIndex = 0;
            if (cmbmodulegrp.Items.Count > 0)
                cmbmodulegrp.SelectedIndex = 0;
            txtuser.Text = "";
            txtcn.Text = "";
            txtdetails.Text = "";
            txtFound.Text = String.Format("Count: {0}",dgvResult.Rows.Count);

            setnormal();
        }

        private void cmbmodulegrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbmodulegrp.Text != "")
            {
                if (m_dtmodule.Rows.Count > 0)
                {
                    if (cmbmodulegrp.Text.ToString() != "System.Data.DataRowView")
                    {
                        cmbmodule.DataSource = null;
                        cmbmodule.Items.Clear();
                        string sqry = String.Format("[module_group] = '{0}'", cmbmodulegrp.Text.ToUpper().Trim());
                        DataTable dt = new DataTable();
                        var foundval = m_dtmodule.Select(sqry, "module_desc");
                        dt = foundval.CopyToDataTable();
                        DataRow row = dt.NewRow();
                        row["id"] = "0";
                        row["module_desc"] = "";
                        dt.Rows.InsertAt(row, 0);
                        cmbmodule.DataSource = dt;
                        cmbmodule.DisplayMember = "module_desc";
                        cmbmodule.ValueMember = "id";
                    }
                }
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            unselectbutton();
            DataTable dt = new DataTable();

            StringBuilder sqry = new StringBuilder();
            sqry.Append("[id] > -1");
            if (txtuser.Text.Trim() != "")
                sqry.Append(String.Format(" and [userid] like '%{0}%'", txtuser.Text.Trim()));
            if (txtcn.Text.Trim() != "")
                sqry.Append(String.Format(" and [computer_name] like '%{0}%'", txtcn.Text.Trim()));
            if (cmbmodulegrp.Text.Trim() != "")
                sqry.Append(String.Format(" and [module_group] = '{0}'", cmbmodulegrp.Text.Trim()));
            if (cmbmodule.Text.Trim() != "")
                sqry.Append(String.Format(" and [module_desc] = '{0}'", cmbmodule.Text.Trim()));
            if (txtdetails.Text.Trim() != "")
                sqry.Append(String.Format(" and [tr_details] like '%{0}%'", txtdetails.Text.Trim()));
            if (frdate.Value != null && frdate.Enabled != false && todate.Value != null && todate.Enabled != false)
            {
                if (frdate.Value.Date.ToString() == todate.Value.Date.ToString())
                    sqry.Append(String.Format(" and [tr_date] >= #{0}# and [tr_date] < #{1}#",
                       frdate.Value.Date.ToString(DateTimeFormatInfo.InvariantInfo), todate.Value.Date.AddDays((double)1).ToString(DateTimeFormatInfo.InvariantInfo)));
                else
                    sqry.Append(String.Format(" and [tr_date] >= #{0}# and [tr_date] <= #{1}#",
                        frdate.Value.Date.ToString(DateTimeFormatInfo.InvariantInfo), todate.Value.Date.ToString(DateTimeFormatInfo.InvariantInfo)));
            }
            if (m_dttrail.Rows.Count == 0)
            {
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id, a.tr_date, a.userid, ");
                sbqry.Append("a.tr_details, c.module_desc, c.module_group, a.computer_name, concat(b.fname , ' ',b.lname) username ");
                sbqry.Append("from a_trail a ");
                sbqry.Append("left join users b on a.userid = b.userid ");
                sbqry.Append("left join system_module c on trim(a.module_code) = trim(c.id) ");
                sbqry.Append("order by tr_date desc");
                m_dttrail = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            }

            if (m_dttrail.Rows.Count > 0)
            {
                var foundRows = m_dttrail.Select(sqry.ToString(),"tr_date desc");
                if (foundRows.Count() == 0)
                {
                    dt = new DataTable();
                    setDataGridView(dgvResult, dt);
                    txtFound.Text = "Count: 0";
                    MessageBox.Show("No records found using the given information.", "Audit Trail", MessageBoxButtons.OK);
                    return;
                }
                else
                    dt = foundRows.CopyToDataTable();
            }

            if (dt.Rows.Count > 0)
            {
                DataView dv = dt.AsDataView();
                dv.Sort = "tr_date desc";
                dt = dv.Table;
                setDataGridView(dgvResult, dt);
                txtFound.Text = "Count: " + String.Format("{0:#,##0}", dt.Rows.Count);

                m_clscom.AddATrail(m_frmM.m_userid, "TRAIL_VIEW", "A_TRAIL",
                               Environment.MachineName.ToString(), "ACCESS THE AUDIT TRAIL MODULE: ", m_frmM._connection);
            }
        }

        private void frdate_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(frdate.Value.Date.ToString());
            todate.Value = date;
        }

        private void todate_ValueChanged(object sender, EventArgs e)
        {
            DateTime datefrom = Convert.ToDateTime(frdate.Value.Date.ToString());
            DateTime dateto = Convert.ToDateTime(todate.Value.Date.ToString());
            if (dateto < datefrom)
            {
                MessageBox.Show("Please check your date parameters.", this.Text, MessageBoxButtons.OK,MessageBoxIcon.Information);
                todate.Value = datefrom;
            }
        }

        private void cbxdate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxdate.Checked == true)
            {
                todate.Enabled = true;
                frdate.Enabled = true;
            }
            else
            {
                todate.Enabled = false;
                frdate.Enabled = false;
            }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            //melvin 10-27-2014
            try
            {
                using (frmReport frmreport = new frmReport())
                {


                    frmreport.setDate = (DateTime)frdate.Value;
                    frmreport.setEndDate = (DateTime)todate.Value.AddDays(1);
                    frmreport.account = txtuser.Text.Trim();
                    frmreport.module = cmbmodulegrp.Text;
                    frmreport.frmInit(m_frmM, m_frmM.m_clscom, "AUDIT");
                    frmreport.ShowDialog();
                    frmreport.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void kryptonGroup1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
