using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;
using CinemaCustomControlLibrary;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using MahApps.Metro;
using System.Collections;
using aZynEManager.EF.Cinemas;

namespace aZynEManager
{
    public partial class frmCinema : Form
    {
        frmMain m_frmM;
        clscommon m_clscom;
        DataTable m_dt = new DataTable();
        DataTable m_dtsounds = new DataTable();
        MySqlConnection myconn = new MySqlConnection();
        CinemaCustomControlLibrary.SeatWindow window ;

        public frmCinema()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            refreshDGV();

            grpcontrol.Visible = true;
            grpfilter.Visible = false;

            dgvResult.ClearSelection();

            populatesounds();

            setnormal();

            //StringBuilder sbqry = new StringBuilder();
            //sbqry.Append("select a.name, a.unit_price, a.id ");
            //sbqry.Append("from patrons a ");
            //sbqry.Append("order by a.id asc");
            //m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            //setDataGridViewII(m_dt);
        }

        public void populatesounds()
        {
            cmbsounds.Items.Clear();
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.sound_system_type ");
            sbqry.Append("from sound_system a order by a.sound_system_type asc");
            m_dtsounds = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

            if (m_dtsounds.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataView view = new DataView(m_dtsounds);
                view.Sort = "sound_system_type";
                DataTable dtsort = view.ToTable(true, "sound_system_type","id");
                DataRow row = dtsort.NewRow();
                row["id"] = "0";
                row["sound_system_type"] = "";
                dtsort.Rows.InsertAt(row, 0);
                cmbsounds.DataSource = dtsort;
                cmbsounds.DisplayMember = "sound_system_type";
                cmbsounds.ValueMember = "id";
            }
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.name, a.capacity, b.sound_system_type ");
            sbqry.Append("from cinema a left join sound_system b ");
            sbqry.Append("on a.sound_id = b.id order by a.in_order asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                this.dgvResult.DataSource = dt;
                int iwidth = dgvResult.Width / 4;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[0].Visible = false;
                dgvResult.Columns[1].Width = iwidth * 2;
                dgvResult.Columns[1].HeaderText = "Cinema Name";
                dgvResult.Columns[2].Width = iwidth;
                dgvResult.Columns[2].HeaderText = "Capacity";
                dgvResult.Columns[3].Width = 0;
                dgvResult.Columns[3].HeaderText = "Sound System";
                dgvResult.Columns[3].Visible = false;
            }
        }

        public void setDataGridViewII(DataTable dt)
        {
            dgvpatrons.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                dgvpatrons.DataSource = dt;
                int iwidth = dgvpatrons.Width / 3;
                dgvpatrons.DataSource = dt;
                dgvpatrons.Columns[0].Width = iwidth * 2;
                dgvpatrons.Columns[0].HeaderText = "Patron Type";
                dgvpatrons.Columns[0].ReadOnly = true;
                dgvpatrons.Columns[1].Width = iwidth / 2 - 8;
                dgvpatrons.Columns[1].HeaderText = "Price";
                dgvpatrons.Columns[2].Width = 0;
                dgvpatrons.Columns[2].HeaderText = "ID";
                dgvpatrons.Columns[2].Visible = false;
                dgvpatrons.Columns.Insert(0, cbx);
            }
        }

        public void setnormal()
        {
            dgvpatrons.DataSource = null;
            dgvpatrons.Columns.Clear();

            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.name, a.unit_price, a.id ");
            sbqry.Append("from patrons a, cinema_patron_default c ");//added cinema_patron_default 7.8.2019
            sbqry.Append("where a.id = c.patron_id ");//added cinema_patron_default 7.8.2019
            sbqry.Append("group by a.id ");//added cinema_patron_default 7.8.2019
            sbqry.Append("order by a.id asc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridViewII(m_dt);

            txtname.Text = "";
            txtname.ReadOnly = true;
            txtcapacity.Text = "";
            txtcapacity.ReadOnly = true;

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

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
            cmbsounds.SelectedIndex = 0;
            setCheck(dgvpatrons, false);
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }



        private void txtcapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
          && !char.IsDigit(e.KeyChar))// && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            //// only allow one decimal point
            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = true;
            //}
            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
            //{
            //    e.Handled = true;
            //}
        }

        private void btnclear2_Click(object sender, EventArgs e)
        {
            txtname.Text = "";
            txtcapacity.Text = "";
            cmbsounds.SelectedIndex = 0;
            setCheck(dgvpatrons, false);
            txtname.Focus();
        }

        private void btngrant_Click(object sender, EventArgs e)
        {
            unselectbutton();
            setCheck(dgvpatrons, true);
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

        

        private void btnrevoke_Click(object sender, EventArgs e)
        {
            unselectbutton();
            setCheck(dgvpatrons, false);
        }

        private void dgvpatrons_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmCinema_Load(object sender, EventArgs e)
        {
            this.dgvpatrons.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgvpatrons_EditingControlShowing);
        }

        void dgvpatrons_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
            {
                TextBox tb = e.Control as TextBox;
                tb.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
                if (this.dgvpatrons.CurrentCell.ColumnIndex == 2)
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != '\b') //allow the backspace key
                    e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CINEMA_ADD", m_frmM._connection);
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
                grpgrant.Visible = true;

                txtname.Text = "";
                txtname.ReadOnly = false;
                txtcapacity.Text = "";
                txtcapacity.ReadOnly = false;

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

                dgvpatrons.DataSource = null;
                dgvpatrons.Columns.Clear();

                StringBuilder sbqry = new StringBuilder();
                /*sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a ");
                sbqry.Append("order by a.id asc");*/ //remarked 7.8.2019
                sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a, cinema_patron_default c ");//added cinema_patron_default 7.8.2019
                sbqry.Append("where a.id = c.patron_id ");//added cinema_patron_default 7.8.2019
                sbqry.Append("group by a.id ");//added cinema_patron_default 7.8.2019
                sbqry.Append("order by a.id asc");
                m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                setDataGridViewII(m_dt);
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
                if (txtcapacity.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcapacity.SelectAll();
                    txtcapacity.Focus();
                    return;
                }

                int chkcnt = 0;
                for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                {
                    if (dgvpatrons[0, i].Value != null)
                    {
                        if ((bool)dgvpatrons[0, i].Value)
                        {
                            chkcnt += 1;
                        }
                    }
                }
                if (chkcnt == 0)
                {
                    MessageBox.Show("Please check the patrons pricing \n\r for the selected cinema.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvpatrons.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from cinema ");
                sqry.Append(String.Format("where name = '{0}' ", txtname.Text.Trim()));
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
                sqry.Append(String.Format("insert into cinema value(0,'{0}',{1},{2},0)",
                    txtname.Text.Trim(), cmbsounds.SelectedValue.ToString(),txtcapacity.Text.Trim()));
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();

                    //validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from cinema_patron ");
                    sqry.Append(String.Format("where cinema_id = {0} ", strid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = 0;
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append("delete from cinema_patron ");
                        sqry.Append(String.Format("where cinema_id = {0} ", strid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    tabinsertcheck(myconn, dgvpatrons, Convert.ToInt32(strid));

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "CINEMA_ADD", "CINEMA|CINEMA_PATRON",
                        Environment.MachineName.ToString(), "ADD NEW CINEMA INFO: NAME=" + txtname.Text
                        + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table."+err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                        //MELVIN 10-15-2014
                        StringBuilder sqry = new StringBuilder();
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into cinema_patron values(0,{0},{1},{2})",
                            intid, dgv[3, i].Value.ToString(), dgv[2, i].Value.ToString()));

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
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CINEMA_EDIT", m_frmM._connection);
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
                int cinemaid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                txtname.ReadOnly = false;
                txtcapacity.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";
                grpgrant.Visible = true;

                //query all patrons //9.27.2019 relaced price to unit_price//remarked
                /*StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a ");
                sbqry.Append(String.Format("where a.id not in(select b.patron_id from cinema_patron b where b.cinema_id = {0}) ", cinemaid));
                sbqry.Append("order by a.id asc");

                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                DataTable dgvdt = (DataTable)dgvpatrons.DataSource;
                if (!(dgvdt == null)){
                    dgvdt.Merge(dt);
                    dgvpatrons.DataSource = dgvdt;
                   
                }else{
                    DataGridViewCheckBoxColumn d1 = new DataGridViewCheckBoxColumn();
                    d1.HeaderText = "";
                    d1.Width = 40;
                    dgvpatrons.Columns.Add(d1);
                    dgvpatrons.DataSource = dt;
                }*/

                setCheck(dgvpatrons, false);
                int rowCount = 0;
                StringBuilder sqry2 = new StringBuilder();
                for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                {
                    int intmid = 0;
                    int intout = -1;
                    if(int.TryParse(dgvpatrons[3, i].Value.ToString(),out intout))
                        intmid = intout;

                    if (intmid > 0)
                    {
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from cinema_patron a where a.cinema_id = {0}", cinemaid));
                        sqry2.Append(String.Format(" and a.patron_id = {0}", intmid));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                               myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                            dgvpatrons[0, i].Value = (object)true;
                    }
                }

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
                if (txtcapacity.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcapacity.SelectAll();
                    txtcapacity.Focus();
                    return;
                }

                int chkcnt = 0;
                for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                {
                    if (dgvpatrons[0, i].Value != null)
                    {
                        if ((bool)dgvpatrons[0, i].Value)
                        {
                            chkcnt += 1;
                        }
                    }
                }
                if (chkcnt == 0)
                {
                    MessageBox.Show("Please check the patrons pricing \n\r for the selected cinema.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvpatrons.Focus();
                    return;
                }


                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from cinema ");
                sqry.Append(String.Format("where name = '{0}' ", txtname.Text.Trim()));
                sqry.Append(String.Format("and capacity = {0} ", txtcapacity.Text.Trim()));
                sqry.Append(String.Format("and sound_id = {0} ", cmbsounds.SelectedValue.ToString()));
                sqry.Append(String.Format("and id != {0} ", intid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                //  if (rowCount > 1)
                //melvin 10-13-2014
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
                strqry.Append(String.Format("update cinema set name = '{0}',", txtname.Text.Trim()));
                strqry.Append(String.Format(" capacity = {0},", txtcapacity.Text.Trim()));
                strqry.Append(String.Format(" sound_id = {0}", cmbsounds.SelectedValue.ToString()));
                strqry.Append(String.Format(" where id = {0}", intid));

                try
                {
                    //update the movies table
                    if (myconn.State == ConnectionState.Closed) 
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //RMB remarked violate the existnace of ticket already sold
                    /*//validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from cinema_patron ");
                    sqry.Append(String.Format("where cinema_id = {0} ", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = 0;
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append("delete from cinema_patron ");
                        sqry.Append(String.Format("where cinema_id = {0} ", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                    }
                    tabinsertcheck(myconn, dgvpatrons, intid);*/

                    //RMB validate for ticket records
                    ArrayList patroncoll = new ArrayList();
                    sqry = new StringBuilder();
                    sqry.Append("select patron_id from cinema_patron ");
                    sqry.Append(String.Format("where cinema_id = {0} ", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            patroncoll.Add(rd[0].ToString());
                        }
                    }
                    rd.Dispose();
                    cmd.Dispose();

                    //to do 12.9.2014
                    //compare the list of checked patrons to the existing patrons in cinema
                    ArrayList newPatronList = new ArrayList(patroncoll);
                    for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                    {
                        if (dgvpatrons[0, i].Value != null)
                        {
                            if ((bool)dgvpatrons[0, i].Value)
                            {
                                foreach (string ptronid in patroncoll)
                                {
                                    if (dgvpatrons[3, i].Value.ToString() == ptronid)
                                        newPatronList.Remove(ptronid);//to get the collection of unchecked patrons
                                }
                            }
                        }
                    }

                    foreach (string ptron in newPatronList)
                    {
                        //vaidate patron @ movies_schedule_list_patron
                        //remarked 9.27.2019
                        //to allow the removal of unchecked cinema patrons
                        //disregarding the existance in movies_schedule_list_patron
                        /*StringBuilder sbqry = new StringBuilder();
                        sbqry.Append("select count(*) from patrons f where f.id in ");
                        sbqry.Append("(select distinct(c.id) from movies_schedule_list_reserved_seat a, ");
                        sbqry.Append("movies_schedule_list_patron b, patrons c, ");
                        sbqry.Append("movies_schedule_list d, movies_schedule e ");
                        sbqry.Append("where a.patron_id = b.id ");
                        sbqry.Append("and b.patron_id = c.id ");
                        sbqry.Append("and a.movies_schedule_list_id = d.id ");
                        sbqry.Append("and d.movies_schedule_id = e.id ");
                        sbqry.Append(String.Format("and e.cinema_id = {0} ", intid));
                        sbqry.Append("order by c.id asc) ");
                        sbqry.Append(String.Format("and f.id = {0} ", ptron));

                        if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                        cmd = new MySqlCommand(sbqry.ToString(), myconn);
                        int rowCount3 = 0;
                        rowCount3 = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount3 == 0)
                        {*/
                            StringBuilder sqry2 = new StringBuilder();
                            sqry2.Append("delete from cinema_patron where ");
                            sqry2.Append(String.Format("cinema_id = {0} and patron_id = {1}",intid, ptron));
                            cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        //}
                    }

                    //will insert records from the database
                    for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                    {
                        if (dgvpatrons[0, i].Value != null)
                        {
                            if ((bool)dgvpatrons[0, i].Value)
                            {
                                sqry = new StringBuilder();
                                sqry.Append("select count(*) from cinema_patron ");
                                sqry.Append(String.Format("where cinema_id = {0} and patron_id = {1}", intid, dgvpatrons[3, i].Value.ToString()));
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                cmd = new MySqlCommand(sqry.ToString(), myconn);
                                rowCount = 0;
                                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Dispose();

                                if (rowCount == 0)
                                {
                                    StringBuilder sqry2 = new StringBuilder();
                                    sqry2.Append(String.Format("insert into cinema_patron values(0,{0},{1},{2})",
                                        intid, dgvpatrons[3, i].Value.ToString(), dgvpatrons[2, i].Value.ToString()));

                                    if (myconn.State == ConnectionState.Closed)
                                        myconn.Open();
                                    MySqlCommand cmd2 = new MySqlCommand(sqry2.ToString(), myconn);
                                    cmd2.ExecuteNonQuery();
                                    cmd2.Dispose();
                                }
                            }
                        }
                    }

                    //sqry = new StringBuilder();
                    //sqry.Append("select count(*) from ticket ");
                    //sqry.Append(String.Format("where name = '{0}' ", txtname.Text.Trim()));
                    //sqry.Append(String.Format("and capacity = {0} ", txtcapacity.Text.Trim()));
                    //sqry.Append(String.Format("and sound_id = {0} ", cmbsounds.SelectedValue.ToString()));
                    //sqry.Append(String.Format("and id != {0} ", intid));
                    //if (myconn.State == ConnectionState.Closed)
                    //    myconn.Open();
                    //cmd = new MySqlCommand(sqry.ToString(), myconn);
                    //int rowCount2 = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.Dispose();
                    //if (rowCount2 > 0)
                    //{
                    //    setnormal();
                    //    if (myconn.State == ConnectionState.Open)
                    //        myconn.Close();
                    //    MessageBox.Show("Can't add this record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    

                    //8-20-2014 UPDATES FROM RAYMOND
                    try
                    {
                        //10-15-2014 MELVIN 
                        if (window == null)
                        {
                            window = new CinemaCustomControlLibrary.SeatWindow();
                        }

                        window.SaveCinemaSeats(intid);
                    }
                    catch (Exception ex)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        if (ex.InnerException != null)
                            MessageBox.Show(ex.InnerException.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            MessageBox.Show(ex.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "CINEMA_EDIT", "CINEMA|CINEMA_PATRON",
                        Environment.MachineName.ToString(), "UPDATED CINEMA INFO: NAME=" + txtname.Text
                        + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV();
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception err)
                {
                    MessageBox.Show("Can't connect to the movies table."+err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CINEMA_DELETE", m_frmM._connection);
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

                    StringBuilder sqry = new StringBuilder();
                    //validate the database for records being used from movies_schedule
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies_schedule where cinema_id = {0}", intid));
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

                    //validate the database for records being used in cinema_patrons
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from cinema_patron where cinema_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from cinema_patron where cinema_id = {0}", intid));
                        try
                        {
                            //delete value for the movies table
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        catch
                        {
                        }
                    }

                     //delete from the moview table where the status is inactive or = 0
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from cinema where id = {0}", intid));
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

                        m_clscom.AddATrail(m_frmM.m_userid, "CINEMA_DELETE", "CINEMA|CINEMA_PATRONS",
                        Environment.MachineName.ToString(), "REMOVED CINEMA INFO: NAME=" + txtname.Text
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

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (rbtnAll.Checked == true)
            { //aded 7.8.2019
                int chklst = 0;
                int cinemaid = -1;
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                KryptonDataGridView dgv = sender as KryptonDataGridView;
                if (dgv == null)
                    return;
                if (dgv.CurrentRow.Selected)
                {
                    cmbsounds.SelectedIndex = 0;
                    btnEdit.Enabled = true;
                    btnEdit.Text = "edit";

                    btnDelete.Enabled = true;
                    btnDelete.Text = "remove";

                    cinemaid = Convert.ToInt32(dgv.SelectedRows[0].Cells[0].Value.ToString());
                    txtname.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                    txtcapacity.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                    string strname = dgv.SelectedRows[0].Cells[3].Value.ToString();
                    for (int i = 0; i < cmbsounds.Items.Count; i++)
                    {
                        DataRowView drv = (DataRowView)cmbsounds.Items[i];
                        if (drv.Row["sound_system_type"].ToString().ToUpper() == strname.ToUpper())
                        {
                            cmbsounds.SelectedIndex = i;
                            break;
                        }
                    }
                }

                dgvpatrons.DataSource = null;
                dgvpatrons.Columns.Clear();

                StringBuilder sbqry = new StringBuilder();
                /*sbqry.Append("select b.name, a.price, a.patron_id as id ");
                sbqry.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                sbqry.Append(String.Format("where a.cinema_id = {0} ",cinemaid));
                sbqry.Append("and b.name is not null ");
                sbqry.Append("order by a.id asc");*/ //remarked 7.8.2019
                /*sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a, cinema_patron b, patrons_active c ");//added cinema_patron_default 7.8.2019
                sbqry.Append("where a.id = c.patron_id ");//added cinema_patron_default 7.8.2019
                sbqry.Append("and b.patron_id = a.id ");//added cinema_patron_default 7.8.2019
                sbqry.Append(String.Format("and b.cinema_id = {0} ", cinemaid));//added cinema_patron_default 7.8.2019
                sbqry.Append("group by a.id ");//added cinema_patron_default 7.8.2019
                sbqry.Append("order by a.id asc");*/

                //added 7.9.2019
                sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a, cinema_patron b, cinema_patron_default c ");
                sbqry.Append("where b.patron_id = a.id ");
                sbqry.Append("and a.id = c.patron_id ");
                sbqry.Append("and b.cinema_id = c.cinema_id ");
                sbqry.AppendFormat("and c.cinema_id = {0} ", cinemaid);
                sbqry.Append("group by c.patron_id ");
                sbqry.Append("union all ");
                sbqry.Append("select e.name, e.unit_price, e.id from patrons_active d, patrons e ");
                sbqry.Append("where e.id = d.patron_id ");
                sbqry.Append("and e.id not in ");
                sbqry.Append("(select f.patron_id ");
                sbqry.Append("from cinema_patron f, cinema_patron_default g ");
                sbqry.AppendFormat("where g.cinema_id = {0} ", cinemaid);
                sbqry.Append("and f.patron_id = g.patron_id) ");
                sbqry.Append("order by 1 asc ");

                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                setDataGridViewII(dt);
                setCheck(dgvpatrons, true);

                //setCheck(dgvpatrons, false);

                //int rowCount = 0;
                //StringBuilder sqry2 = new StringBuilder();
                //// check cinema
                //for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                //{
                //    int intmid = Convert.ToInt32(dgvpatrons[3, i].Value.ToString());
                //    sqry2 = new StringBuilder();
                //    sqry2.Append(String.Format("select count(*) from cinema_patron a where a.cinema_id = {0}", cinemaid));
                //    sqry2.Append(String.Format(" and a.patron_id = {0}", intmid));
                //    if (myconn != null)
                //    {
                //        if (myconn.State == ConnectionState.Closed)
                //            myconn.Open();
                //        MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                //        rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                //        cmd.Dispose();
                //    }

                //    if (rowCount > 0)
                //    {
                //        dgvpatrons[0, i].Value = (object)true;
                //        chklst += 1;
                //    }
                //}
            }
            else if (rbtnDefault.Checked == true)
                validateDefault2();
        }

        private void dgvpatrons_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            int cinemaid = -1;
            if (dgvResult.SelectedRows.Count == 1)
                cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

            setCheck(dgvpatrons, false);
            int rowCount = 0;
            StringBuilder sqry2 = new StringBuilder();
            for (int i = 0; i < dgvpatrons.Rows.Count; i++)
            {
                int intmid = 0;
                int intout = -1;
                if (int.TryParse(dgvpatrons[3, i].Value.ToString(), out intout))
                    intmid = intout;

                if (intmid > 0)
                {
                    sqry2 = new StringBuilder();
                    sqry2.Append(String.Format("select count(*) from cinema_patron a where a.cinema_id = {0}", cinemaid));
                    sqry2.Append(String.Format(" and a.patron_id = {0}", intmid));
                    if (myconn != null)
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                        rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }

                    if (rowCount > 0)
                        dgvpatrons[0, i].Value = (object)true;
                }
            }
        }

        private void btnaddsound_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "SOUNDSYS", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            frmSoundSystem frmss = new frmSoundSystem();
            frmss.frmInit(m_frmM,m_clscom,this);
            frmss.ShowDialog();
            frmss.Dispose();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            unselectbutton();
        }

        private void btnseats_Click(object sender, EventArgs e)
        {
            unselectbutton();
            //MessageBox.Show("This control is not yet functional.","Sorry");
            //System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop();
            //var wpfwindow = new Cinema    Window1();
            //ElementHost.EnableModelessKeyboardInterop(wpfwindow);
            //wpfwindow.Show();

            int intCinemaId = 0;
            string strCinemaName = txtname.Text.Trim();
            int intCapacity = 0;
            if (dgvResult.SelectedRows.Count == 1 && btnEdit.Text == "update")
                int.TryParse(dgvResult.SelectedRows[0].Cells[0].Value.ToString(), out intCinemaId);
            int.TryParse(txtcapacity.Text.Trim(), out intCapacity);
            if (strCinemaName == string.Empty)
            {
                MessageBox.Show("Cinema name is required.", this.Text);
                return;
            }
            else if (intCapacity <= 0)
            {
                MessageBox.Show("Cinema capacity is required.", this.Text);
                return;
            }

            //resets all 
            window = new CinemaCustomControlLibrary.SeatWindow();

            window.LoadCinema(intCinemaId, strCinemaName, intCapacity);
            ElementHost.EnableModelessKeyboardInterop(window);
            window.Show();
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rbtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnDefault.Checked == true)
                validateDefault0();//aded 7.9.2019
        }

        public void validateDefault2()
        {
            try
            {
                if (dgvpatrons.Rows.Count > 0)
                {
                    dgvpatrons.DataSource = null;
                    dgvpatrons.Columns.Clear();
                }

                int cinemaid = -1;
                if (dgvResult.SelectedRows.Count > 0)
                    cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.name, a.unit_price, a.id ");
                sbqry.Append("from patrons a, cinema_patron b, cinema_patron_default c ");
                sbqry.Append("where b.patron_id = a.id ");
                sbqry.Append("and a.id = c.patron_id ");
                sbqry.Append("and b.cinema_id = c.cinema_id ");
                sbqry.AppendFormat("and c.cinema_id = {0} ", cinemaid);
                sbqry.Append("group by c.patron_id ");
                sbqry.Append("union all ");
                sbqry.Append("select e.name, e.unit_price, e.id from patrons_active d, patrons e ");
                sbqry.Append("where e.id = d.patron_id ");
                sbqry.Append("and e.id not in ");
                sbqry.Append("(select f.patron_id ");
                sbqry.Append("from cinema_patron f, cinema_patron_default g ");
                sbqry.AppendFormat("where g.cinema_id = {0} ", cinemaid);
                sbqry.Append("and f.patron_id = g.patron_id) ");
                sbqry.Append("order by 1 asc ");
                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                setDataGridViewII(dt);
                setCheck(dgvpatrons, false);

                validateDefault0();
            }
            catch (Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void validateDefault0()
        {
            try
            {
                int cinemaid = -1;
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                if (rbtnDefault.Checked == true)
                {
                    setCheck(dgvpatrons, false);
                    if (dgvResult.SelectedRows.Count > 0)
                    {
                        cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                        StringBuilder sbqry = new StringBuilder();
                        sbqry.Append("select a.id ");
                        sbqry.Append("from patrons a, cinema_patron b, cinema_patron_default c ");
                        sbqry.Append("where b.patron_id = a.id ");
                        sbqry.Append("and a.id = c.patron_id ");
                        sbqry.Append("and b.cinema_id = c.cinema_id ");
                        sbqry.AppendFormat("and c.cinema_id = {0} ", cinemaid);
                        sbqry.Append("group by c.patron_id ");
                        DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                        List<PatronList> list = new List<PatronList>();
                        list = (from DataRow row in dt.Rows
                                select new PatronList()
                                {
                                    PatronId = Convert.ToInt32(row["id"].ToString())
                                }).ToList();

                        for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                        {
                            bool boolexist = list.Exists(x => x.PatronId == Convert.ToInt32(dgvpatrons[3, i].Value.ToString()));
                            dgvpatrons[0, i].Value = (object)boolexist;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void validateDefault()
        {
            try
            {
                int cinemaid = -1;
                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                if (rbtnDefault.Checked == true)
                {
                    //check if the patron is checked and change the visibility to false
                    /*for (int i = 0; i < dgvpatrons.Rows.Count; i++)//added 5.8.2018
                    {
                        if (dgvpatrons[0, i].Value.ToString().ToLower() == "false")
                            dgvpatrons.Rows[i].Visible = false;
                    }*///remarked 7.8.2019

                    //System.Threading.Thread.Sleep(2000);//added 5.8.2018//remarked 7.8.2019
                    setCheck(dgvpatrons, false);
                    if (dgvResult.SelectedRows.Count > 0)
                    {
                        cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                        /*StringBuilder sbqry = new StringBuilder();
                        sbqry.Append("select b.name, a.price, a.patron_id as id ");
                        sbqry.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                        sbqry.Append("left join cinema_patron_default c on a.patron_id = c.patron_id ");
                        sbqry.Append(String.Format("where a.cinema_id = {0} ", cinemaid));
                        sbqry.Append("and b.name is not null ");
                        sbqry.Append(String.Format("and c.cinema_id = {0} ", cinemaid));
                        sbqry.Append("order by a.id asc");
                        DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);*/

                        StringBuilder sqry2 = new StringBuilder();
                        for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                        {
                            int rowCount = -1;
                            int intmid = 0;
                            int intout = -1;
                            if (int.TryParse(dgvpatrons[3, i].Value.ToString(), out intout))
                                intmid = intout;

                            if (intmid > 0)
                            {
                                sqry2 = new StringBuilder();
                                sqry2.Append(String.Format("select count(*) from cinema_patron_default a where a.cinema_id = {0}", cinemaid));
                                sqry2.Append(String.Format(" and a.patron_id = {0}", intmid));
                                if (myconn != null)
                                {
                                    if (myconn.State == ConnectionState.Closed)
                                        myconn.Open();
                                    MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                                    cmd.Dispose();
                                }

                                if (rowCount > 0)
                                    dgvpatrons[0, i].Value = (object)true;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();
                MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            int cinemaid = -1;
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            if (rbtnAll.Checked == true)
            {
                //for (int i = 0; i < dgvpatrons.Rows.Count; i++)//added 5.8.2018 //remarked 7.9.2019
                //    dgvpatrons.Rows[i].Visible = true; //added 5.8.2018//remarked 7.9.2019

                //System.Threading.Thread.Sleep(3000);//added 5.8.2018//remarked 7.9.2019
                setCheck(dgvpatrons, true);//

                //added7.9.2019
                /*if (dgvResult.SelectedRows.Count > 0)
                    cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id ");
                sbqry.Append("from patrons a, cinema_patron b, cinema_patron_default c ");
                sbqry.Append("where b.patron_id = a.id ");
                sbqry.Append("and a.id = c.patron_id ");
                sbqry.Append("and b.cinema_id = c.cinema_id ");
                sbqry.AppendFormat("and c.cinema_id = {0} ", cinemaid);
                sbqry.Append("group by c.patron_id ");
                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                List<PatronList> list = new List<PatronList>();
                list = (from DataRow row in dt.Rows
                        select new PatronList()
                        {
                            PatronId = Convert.ToInt32(row["id"].ToString())
                        }).ToList();

                for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                {
                    bool boolexist = list.Exists(x => x.PatronId == Convert.ToInt32(dgvpatrons[2, i].Value.ToString()));
                    dgvpatrons[0, i].Value = (object)boolexist;
                }*/

                /*if (dgvResult.SelectedRows.Count > 0)
                {
                    cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                    StringBuilder sqry2 = new StringBuilder();
                    for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                    {
                        int rowCount = -1;
                        int intmid = 0;
                        int intout = -1;
                        if (int.TryParse(dgvpatrons[3, i].Value.ToString(), out intout))
                            intmid = intout;

                        if (intmid > 0)
                        {
                            sqry2 = new StringBuilder();
                            sqry2.Append(String.Format("select count(*) from cinema_patron a where a.cinema_id = {0}", cinemaid));
                            sqry2.Append(String.Format(" and a.patron_id = {0}", intmid));
                            if (myconn != null)
                            {
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Dispose();
                            }

                            if (rowCount > 0)
                                dgvpatrons[0, i].Value = (object)true;
                        }
                    }
                }*/
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            if (rbtnDefault.Checked == false)
            {
                MessageBox.Show("Please set the default patrons by selecting the default radio button", this.Text);
                return;
            }

            if (rbtnDefault.Checked == true)
            {
                int cinemaid = -1;
                if (dgvResult.SelectedRows.Count > 0)
                {
                    cinemaid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                    StringBuilder sqry = new StringBuilder();
                    MySqlCommand cmd = new MySqlCommand();
                    sqry = new StringBuilder();
                    try
                    {
                        sqry.Append(String.Format("delete from cinema_patron_default where cinema_id = {0}", cinemaid));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    catch
                    {
                    }
                    for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                    {
                        if (dgvpatrons[0, i].Value != null)
                        {
                            if ((bool)dgvpatrons[0, i].Value)
                            {
                                StringBuilder sqry2 = new StringBuilder();
                                sqry2.Append(String.Format("insert into cinema_patron_default values(0,{0},{1})",
                                    cinemaid, dgvpatrons[3, i].Value.ToString()));

                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                MySqlCommand cmd2 = new MySqlCommand(sqry2.ToString(), myconn);
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();
                            }
                        }
                    }
                    MessageBox.Show("Done updating the default patrons the selected cinema", this.Text);
                }
            }
        }

        private void linklabelManageCinemaPatrons_LinkClicked(object sender, EventArgs e)
        {
            var dataRow = (dgvResult?.CurrentRow?.DataBoundItem as DataRowView)?.Row;
            var cinemaId = dataRow?.Field<int>("id");
            if (!cinemaId.HasValue) return;
            var form = new frmCinemaPatrons(userId: m_frmM.UserID, cinemaId: cinemaId ?? 0);
            if (!(form.ShowDialog() == DialogResult.OK)) return;

        }
    }
}
