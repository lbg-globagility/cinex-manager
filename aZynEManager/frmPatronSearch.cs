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
    public partial class frmPatronSearch : Form
    {
        frmMain m_frmM;
        MySqlConnection myconn;
        clscommon m_clscom = null;
        DataTable m_frdt = new DataTable();
        DataTable m_todt = new DataTable();

        public frmPatronSearch()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.code, a.name, a.unit_price as unitprice, ");
            sbqry.Append("a.seat_position, b.price as baseprice, b.effective_date,a.id ");
            sbqry.Append("from patrons a, ticket_prices b ");
            sbqry.Append("where a.base_price = b.id ");
            sbqry.Append("order by a.name asc");
            
            m_frdt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_frdt);

            /*int cnt = 0;
            bool boolchk = true;
            //setCheck(dgvResult, boolchk);
            dgvResult.Refresh();

            if (rbtnactive.Checked == true)
                validateList();
            else if(rbtnAll.Checked == true)
                setCheck(dgvResult, boolchk);*/
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

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.Columns.Clear();
            this.dgvResult.DataSource = null;
            if (dt.Rows.Count > 0)
                this.dgvResult.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;
                /*cbx.ValueType = typeof(bool);
                cbx.TrueValue = 1;
                cbx.FalseValue = 0;*/

                int iwidth = dgvResult.Width / 4;
                dgvResult.DataSource = dt;
                dgvResult.Columns.Insert(0, cbx);
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Code";
                dgvResult.Columns[2].Width = iwidth * 2;
                dgvResult.Columns[2].HeaderText = "Name";
                dgvResult.Columns[3].Width = iwidth / 2;
                dgvResult.Columns[3].HeaderText = "Unit Price";
                dgvResult.Columns[4].Width = iwidth / 2;
                dgvResult.Columns[4].HeaderText = "Position";
                dgvResult.Columns[5].Width = iwidth / 2;
                dgvResult.Columns[5].HeaderText = "Base Price";
                dgvResult.Columns[6].Width = iwidth / 2;
                dgvResult.Columns[6].HeaderText = "Effectivity Date";
                dgvResult.Columns[7].Width = 0;
                dgvResult.Columns[7].HeaderText = "ID";
                dgvResult.Columns[7].Visible = false;
            }
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            refreshDGV();

            grpfilter.Visible = true;
            dgvResult.ClearSelection();

            setnormal();
        }

        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = false;
            txtname.Text = "";
            txtname.ReadOnly = false;

            btnPatron.Enabled = true;
            btnPatron.Text = "data \nentry";
            btnPatron.Values.Image = Properties.Resources.buttonapply;

            dgvResult.Enabled = true;

            dtstart.Value = DateTime.Now;
            dtend.Value = DateTime.Now;

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            unselectbutton();
            setnormal();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (dtstart.Value > dtend.Value)
            {
                MessageBox.Show("Please check the effectivity date values.", this.Text);
                dtstart.Focus();
                return;
            }

            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.code, a.name, a.unit_price as unitprice, ");
            sbqry.Append("a.seat_position, b.price as baseprice, b.effective_date,a.id ");
            sbqry.Append("from patrons a, ticket_prices b ");
            sbqry.Append("where a.base_price = b.id ");

            if(txtcode.Text.Trim() != "")
                sbqry.AppendFormat("and a.code like '%{0}%' ",txtcode.Text.Trim());

            if(txtname.Text.Trim() != "")
                sbqry.AppendFormat("and a.name like '%{0}%' ", txtname.Text.Trim());

            if (cbxeffdate.Checked == true)
            {
                if (dtend.Value.Date > dtstart.Value.Date)
                    sbqry.AppendFormat("and b.effective_date between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", dtstart.Value, dtend.Value);
                else if (dtend.Value.Date == dtstart.Value.Date)
                    sbqry.AppendFormat("and b.effective_date = '{0:yyyy/MM/dd}' ", dtstart.Value);
            }

            sbqry.Append("order by a.name asc");
            m_frdt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_frdt);

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
        }

        private void btnPatron_Click(object sender, EventArgs e)
        {
            unselectbutton();
            using (frmPatron frm = new frmPatron())
            {
                frm.frmInit(m_frmM, m_clscom, this);
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void cbxeffdate_CheckedChanged(object sender, EventArgs e)
        {
            if(cbxeffdate.Checked == true) { 
                dtstart.Enabled = true;
                dtend.Enabled = true;
            }else{
                dtstart.Enabled = false;
                dtend.Enabled = false;
            }
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAll.Checked == true)
                setCheck(dgvResult, true);
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            unselectbutton();

            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            if (rbtnactive.Checked == true)
            {
                StringBuilder sqry = new StringBuilder();
                sqry.Append("truncate table patrons_active");
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    bool booltrue = true;
                    for (int i = 0; i < dgvResult.Rows.Count; i++)
                    {
                        if ((bool)dgvResult[0, i].Value)
                        {
                            sqry = new StringBuilder();
                            sqry = sqry.Append(String.Format("insert into patrons_active values ({0},{1})", "0", dgvResult[7, i].Value.ToString()));
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
                catch (Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                }
            }
            else
            {
                MessageBox.Show(this,this.Text,"Please select the active button.",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
        }
        private void rbtnactive_CheckedChanged(object sender, EventArgs e)
        {
            validateList();
        }

        public void validateList()
        {
            if (rbtnactive.Checked == true)
            {
                setCheck(dgvResult, false);
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id ");
                sbqry.Append("from patrons a, patrons_active c ");
                sbqry.Append("where a.id = c.patron_id");
                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                List<PatronList> list = new List<PatronList>();
                list = (from DataRow row in dt.Rows
                        select new PatronList()
                        {
                            PatronId = Convert.ToInt32(row["id"].ToString())
                        }).ToList();

                for (int i = 0; i < dgvResult.Rows.Count; i++)
                {
                    bool boolexist = list.Exists(x => x.PatronId == Convert.ToInt32(dgvResult[7, i].Value.ToString()));
                    dgvResult[0, i].Value = (object)boolexist;
                }
            }
        }

        private void frmPatronSearch_Load(object sender, EventArgs e)
        {
            bool booltrue = true;
            if (rbtnAll.Checked == true)
                setCheck(dgvResult, booltrue);
        }
    }
}

public class PatronList
{
    public int PatronId { get; set; }
}
