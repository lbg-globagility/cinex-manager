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
    public partial class frmTaskManager : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        DataTable dtmodule = new DataTable();
        DataTable dtuserlevel = new DataTable();
        MySqlConnection myconn = null;

        public frmTaskManager()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            unselectbutton();

            m_frmM = frm;
            m_clscom = cls;

            refreshDGV();
            setnormal();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.user_name, a.computer_name, a.user_authority, a.time_in ");
            sbqry.Append("from user_logs a order by a.user_name asc");
            dtmodule = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvResult, dtmodule);

        }

        public void setDataGridView(DataGridView dgv, DataTable dt)
        {
            //dgv.Columns.Clear();
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dgv.Columns.Clear();
                dgv.DataSource = dt;
                int iwidth = dgv.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth;
                dgv.Columns[1].HeaderText = "User Name";
                dgv.Columns[2].Width = iwidth;
                dgv.Columns[2].HeaderText = "Computer Name";
                dgv.Columns[3].Width = iwidth;
                dgv.Columns[3].HeaderText = "Authoritization";
                dgv.Columns[4].Width = iwidth;
                dgv.Columns[4].HeaderText = "Time In";
            }
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void setnormal()
        {
            txtWSN.Text = String.Empty;
            txtName.Text = String.Empty;
            txtAuth.Text = String.Empty;
            txtDate.Text = String.Empty;
            txtTime.Text = String.Empty;
            txtWSN.ReadOnly = true;
            txtName.ReadOnly = true;
            txtAuth.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtTime.ReadOnly = true;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;
        }

        private void dgvResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                btnDelete.Enabled = true;
                btnDelete.Text = "remove";

                txtWSN.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                txtName.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                txtAuth.Text = dgv.SelectedRows[0].Cells[3].Value.ToString();

                DateTime dt = DateTime.Parse(dgv.SelectedRows[0].Cells[4].Value.ToString());
                try
                {
                    txtDate.Text = dt.ToString("d");
                    txtTime.Text = dt.ToString("t");
                }
                catch
                {
                    txtDate.Text = "";
                    txtTime.Text = "";

                    MessageBox.Show("Can't read the date and time value", this.Text, MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
        }

        private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResult.Columns[e.ColumnIndex].Name == "time_in")
                ShortFormtTimeFormat(e);
        }

        private static void ShortFormtTimeFormat(DataGridViewCellFormattingEventArgs formatting)
        {
            if (formatting.Value != null)
            {
                try
                {
                    System.Text.StringBuilder dateString = new System.Text.StringBuilder();
                    DateTime theDate = DateTime.Parse(formatting.Value.ToString());

                    dateString.Append(String.Format("{0:MM/dd/yyyy}", theDate));
                    formatting.Value = dateString.ToString();
                    formatting.FormattingApplied = true;
                }
                catch
                {
                    formatting.FormattingApplied = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "TASK_DELETE", m_frmM._connection);
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
                    sqry.Append(String.Format("select count(*) from user_logs where id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount == 1)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from user_logs where id = {0}", intid));
                        try
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            m_clscom.AddATrail(m_frmM.m_userid, "TASK_DELETE", "USER_LOGS",
                               Environment.MachineName.ToString(), "REMOVED LOGGED USER: USERNAME=" + this.txtName.Text
                               + " | ID=" + intid.ToString(), m_frmM._connection);
                        }
                        catch
                        {
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            MessageBox.Show("Can't remove this record from the user logged list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                refreshDGV();
                setnormal();
            }
        }
    }
}
