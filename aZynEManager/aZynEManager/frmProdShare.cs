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
using System.Globalization;

namespace aZynEManager
{
    public partial class frmProdShare : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        DataTable m_dt = new DataTable();
        DataTable movie_dt = new DataTable();
        MySqlConnection myconn = null;
        //melvin 10-24-2014
        int daycnt = 1; 
        bool allowSpace = false;

        public frmProdShare()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;

            refreshDGV(true);

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            populatemovies();

            cmbtitle.Enabled = false;
            dgvResult.ClearSelection();

            setnormal();
        }
        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = false;
            cmbtitle.Text = "";
            cmbtitle.Enabled = true;
            txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString();
            txtshare.ReadOnly = true;

            if (cmbtitle.Items.Count > 0)
                cmbtitle.SelectedIndex = 0;

            dtdate.Value = DateTime.Now;

            btnAdd.Enabled = true;
            btnAdd.Text = "new";
            btnAdd.Values.Image = Properties.Resources.buttonadd;

            btnEdit.Enabled = false;
            btnEdit.Text = "edit";
            btnEdit.Values.Image = Properties.Resources.buttonapply;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;

            dgvMovies.Enabled = true;
            dgvResult.Enabled = true;
            grpcontrol.Enabled = true;

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void populatemovies()
        {
            cmbtitle.DataSource = null;
            DataTable dt = new DataTable();
            string sqry = "[id] > -1";
            if (m_frmM.m_dtmovies.Rows.Count == 0)

                //melvin 10-24-2014, update for date format
                m_frmM.m_dtmovies = m_clscom.setDataTable("select id, "+
                    "code,title,dist_id,share_perc,rating_id,duration, "+
                    "date_format(encoded_date,'%m-%d-%Y'),date_format(start_date,'%m-%d-%Y'),"+
                    "date_format(end_date,'%m-%d-%Y') from movies order by title asc", m_frmM._connection);
            
            if (m_frmM.m_dtmovies.Rows.Count > 0)
            {
                var foundRows = m_frmM.m_dtmovies.Select(sqry);
                if (foundRows.Count() == 0)
                {
                    cmbtitle.Items.Clear();
                }
                else
                    dt = foundRows.CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "title asc";
                    dt = dv.ToTable();

                    DataRow row = dt.NewRow();
                    row["id"] = "0";
                    row["title"] = "";
                    dt.Rows.InsertAt(row, 0);

                    cmbtitle.DataSource = dt;
                    cmbtitle.ValueMember = "id";
                    cmbtitle.DisplayMember = "title";
                }
            }
          //  MessageBox.Show(cmbtitle.Items.Count.ToString());
        }


        public void refreshDGV(bool withCutOff)
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, b.code, b.title, c.name as distributor, a.share_perc as share, a.effective_date,a.day_count ");
            sbqry.Append("from movies_distributor a ");
            sbqry.Append("left join movies b on a.movie_id = b.id ");
            sbqry.Append("inner join distributor c on b.dist_id = c.id ");
            if (withCutOff)
                sbqry.Append(String.Format("where b.encoded_date >= '{0:yyyy-MM-dd}' order by a.id asc", m_clscom.m_clscon.MovieListCutOffDate));
            else
                sbqry.Append("order by a.id asc");

            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(dgvResult, m_dt);

            sbqry = new StringBuilder();
            sbqry.Append("SELECT a.id, a.code, a.title ");
            sbqry.Append("FROM movies a ");
            if (withCutOff)
                sbqry.Append(String.Format("where a.encoded_date >= '{0:yyyy-MM-dd}' order by a.encoded_date desc", m_clscom.m_clscon.MovieListCutOffDate));
            else
                sbqry.Append("order by a.encoded_date desc");
            movie_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            if (movie_dt.Rows.Count > 0)
                setDataGridViewII(dgvMovies, movie_dt);
        }

        public void setDataGridView(DataGridView dgv, DataTable dt)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
                dgv.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgv.Width / 6;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth - 5;
                dgv.Columns[1].HeaderText = "Movie Code";
                dgv.Columns[2].Width = iwidth * 2;
                dgv.Columns[2].HeaderText = "Movie Title";
                dgv.Columns[3].Width = (int)(iwidth * 1.5);
                dgv.Columns[3].HeaderText = "Distributor / Producer";
                dgv.Columns[4].Width = iwidth / 2;
                dgv.Columns[4].HeaderText = "Share";
                dgv.Columns[5].Width = iwidth;
                dgv.Columns[5].HeaderText = "Effective Date";
                dgv.Columns[6].Width = 0;
                dgv.Columns[6].HeaderText = "Days Cover";
            }
        }

        public void setDataGridViewII(DataGridView dgv, DataTable dt)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
                dgv.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgvMovies.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth - 15;
                dgv.Columns[1].HeaderText = "Movie Code";
                dgv.Columns[2].Width = iwidth * 2;
                dgv.Columns[2].HeaderText = "Movie Title";
            }
        }

        private void dgvMovies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                txtcode.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                string strname = dgv.SelectedRows[0].Cells[2].Value.ToString();
                cmbtitle.DropDownStyle = ComboBoxStyle.DropDown;
               
              
                if (cmbtitle.Text == null)
                {
                    cmbtitle.Items.Add(strname);
                   
                }
                cmbtitle.Text = strname;
                cmbtitle.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void cmbtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbtitle.SelectedValue != null && cmbtitle.Text != "")
                {
                    var val = cmbtitle.SelectedValue;
                    if (val.ToString() != "System.Data.DataRowView")
                    {
                        string strid = cmbtitle.SelectedValue.ToString();
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append(String.Format("[id] = {0}", strid));
                        if (cmbtitle.Text.Trim() != "")
                            sqry.Append(String.Format(" and [title] = '{0}'", cmbtitle.Text.ToString()));
                        var foundRows = movie_dt.Select(sqry.ToString());
                        if (foundRows.Count() == 0)
                        {
                        }
                        else
                        {
                            DataTable dt = foundRows.CopyToDataTable();
                            txtcode.Text = dt.Rows[0]["code"].ToString();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void cmbtitle_Click(object sender, EventArgs e)
        {
    
        }

        private void btnsearch2_Click(object sender, EventArgs e)
        {
            unselectbutton();
         
                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("[code] like '%{0}%'", txtcode.Text.Trim()));
                var foundRows = movie_dt.Select(sqry.ToString());
                if (foundRows.Count() == 0)
                {
                    
                }
                //else if (foundRows.Count() == 1)
                //{
                //    setDataGridViewII(dgvMovies, foundRows.CopyToDataTable());
                //    txtcode.Text = foundRows.CopyToDataTable().Rows[0]["code"].ToString();
                //}
                else
                {
                    setDataGridViewII(dgvMovies, foundRows.CopyToDataTable());
                    
                }
         
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            daycnt = 7;
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PRODSHARE_ADD", m_frmM._connection);
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

                txtcode.ReadOnly = false;
                cmbtitle.Enabled = true;
                txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString();
                txtshare.ReadOnly = false;
                dtdate.Enabled = true;
                txtdaycnt.ReadOnly = false;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtcode.SelectAll();
                txtcode.Focus();

                dtdate.Value = DateTime.Today;
            }
            else
            {
                int intday = 0;
                string strstatus = String.Empty;
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if (cmbtitle.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    cmbtitle.SelectedIndex = 0;
                    cmbtitle.Focus();
                    return;
                }
                if (txtshare.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtshare.Focus();
                    return;
                }
                else
                {
                    double outdblshare = 0;
                    if (double.TryParse(txtshare.Text, out outdblshare))
                    {
                        if (outdblshare > 100 || outdblshare < 0)
                        {
                            txtshare.SelectAll();
                            MessageBox.Show("Please check your movie share info.");
                            txtshare.Focus();
                            return;
                        }
                    }
                }

                if (dtdate.Text == "" || dtdate.Value == null)
                {
                    MessageBox.Show("Please fill the required information.");
                    dtdate.Focus();
                    return;
                }

                if (txtdaycnt.Text.Trim() != "")
                {
                    
                    int intout = -1;
                    if (int.TryParse(txtdaycnt.Text.Trim(), out intout))
                        intday = intout;
                    else
                    {
                        MessageBox.Show("Please fill the box with a valid value.");
                        txtdaycnt.Focus();
                        return;
                    }
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies_distributor ");
                sqry.Append(String.Format("where movie_id = {0} ", cmbtitle.SelectedValue));
                //sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                sqry.Append(String.Format("and effective_date = '{0}'", dtdate.Value.Date.ToString("yyyy-MM-dd")));
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

                //insert value for the movies table
                sqry = new StringBuilder();
                sqry.Append(String.Format("insert into movies_distributor value(0,{0},{1},'{2}',{3})",
                    cmbtitle.SelectedValue, txtshare.Text.Trim(), dtdate.Value.Date.ToString("yyyy-MM-dd"),intday));
                try
                {
                    if(myconn.State== ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();

                    cmd.Dispose();
                   
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    //update the moviestatus to active if new
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies where id = {0} and status = 0", cmbtitle.SelectedValue));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("update movies set status = 1 where id = {0}", cmbtitle.SelectedValue));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    m_clscom.AddATrail(m_frmM.m_userid, "PRODSHARE_ADD", "MOVIES|DISTRIBUTOR|MOVIES_DISTRIBUTOR",
                            Environment.MachineName.ToString(), "ADD NEW DISTRIBUTOR SHARE INFO: MOVIEID=" + cmbtitle.SelectedValue.ToString() + " | ID=" + strid, m_frmM._connection);

                    DialogResult ans = MessageBox.Show("You have successfully added the new record, \n\rDo you want to add again?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ans == System.Windows.Forms.DialogResult.Yes)
                    {
                        StringBuilder sbqry = new StringBuilder();
                        sbqry.Append("select a.id, b.code, b.title, c.name as distributor, a.share_perc, a.effective_date ");
                        sbqry.Append("from movies_distributor a ");
                        sbqry.Append("left join movies b on a.movie_id = b.id ");
                        sbqry.Append("left join distributor c on b.dist_id = c.id ");
                        sbqry.Append(String.Format("where b.encoded_date >= '{0:yyyy-MM-dd}' order by a.id asc", m_clscom.m_clscon.MovieListCutOffDate));
                        m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                        setDataGridView(dgvResult, m_dt);

                        dtdate.Value = dtdate.Value.AddDays(intday);
                        txtcode.SelectAll();
                        txtcode.Focus();
                    }
                    else if (ans == System.Windows.Forms.DialogResult.No)
                    {
                        refreshDGV(true);
                        setnormal();
                    }
                }
                catch
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnclear2_Click(object sender, EventArgs e)
        {
            unselectbutton();
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("SELECT a.id, a.code, a.title ");
            sbqry.Append("FROM movies a ");
            sbqry.Append(String.Format("where a.encoded_date >= '{0:yyyy-MM-dd}' order by a.encoded_date desc", m_clscom.m_clscon.MovieListCutOffDate));
            movie_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            if (movie_dt.Rows.Count > 0)
                setDataGridViewII(dgvMovies, movie_dt);

            txtcode.Text = "";
            cmbtitle.SelectedIndex = 0;
            txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString();
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PRODSHARE_EDIT", m_frmM._connection);
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
                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                cmbtitle.Enabled = false;
                txtcode.ReadOnly = true;
                txtdaycnt.ReadOnly = false;
                txtshare.ReadOnly = false;
                txtshare.SelectAll();
                txtshare.Focus();
                int.TryParse(txtdaycnt.Text, out daycnt);
                dtdate.Enabled = true;

                dgvMovies.Enabled = false;
            }
            else if (btnEdit.Text == "update")
            {
                string strstatus = String.Empty;
                if (txtcode.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if (cmbtitle.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    cmbtitle.Focus();
                    return;
                }
                if (txtshare.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtshare.Focus();
                    return;
                }
                if (dtdate.Value == null || dtdate.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    dtdate.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                int intid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
               

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies_distributor ");
                sqry.Append(String.Format("where movie_id = {0} ", cmbtitle.SelectedValue));
                sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                sqry.Append(String.Format("and effective_date = '{0}'", dtdate.Value.Date.ToString("yyyy-MM-dd")));
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
                    MessageBox.Show("Cannot find the movies.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder strqry = new StringBuilder();
                strqry.Append("update movies_distributor set");
                strqry.Append(String.Format(" movie_id = {0},", cmbtitle.SelectedValue));
                strqry.Append(String.Format(" share_perc = {0},", txtshare.Text.Trim()));
                strqry.Append(String.Format(" effective_date = '{0}',", dtdate.Value.Date.ToString("yyyy-MM-dd")));
                strqry.Append(String.Format(" day_count={0}", txtdaycnt.Text));
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

                    m_clscom.AddATrail(m_frmM.m_userid, "PRODSHARE_EDIT", "MOVIES|DISTRIBUTOR|MOVIES_DISTRIBUTOR",
                            Environment.MachineName.ToString(), "UPDATED DISTRIBUTOR SHARE INFO: MOVIE ID=" + cmbtitle.SelectedValue.ToString()
                            + " | ID=" + intid, m_frmM._connection);

                    refreshDGV(true);
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the movies table.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "PRODSHARE_DELETE", m_frmM._connection);
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
                    sqry.Append(String.Format("select count(*) from movies_distributor where id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from movies_distributor where id = {0}", intid));
                        try
                        {
                            if(myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            m_clscom.AddATrail(m_frmM.m_userid, "PRODSHARE_DELETE", "MOVIES_DISTRIBUTOR",
                            Environment.MachineName.ToString(), "REMOVED DISTRIBUTOR SHARE INFO: MOVIEID=" + cmbtitle.SelectedValue.ToString() + " | ID=" + intid.ToString(), m_frmM._connection);
                            MessageBox.Show("Successfully Deleted",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                            //update the moviestatus to new if all records will be deleted
                            sqry = new StringBuilder();
                            sqry.Append(String.Format("select count(*) from movies_distributor where movie_id = {0}", cmbtitle.SelectedValue.ToString()));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                            if (rowCount > 0)
                            {
                            }
                            else
                            {
                                sqry = new StringBuilder();
                                sqry.Append(String.Format("select count(*) from movies_schedule where movie_id = {0}", cmbtitle.SelectedValue.ToString()));
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                cmd = new MySqlCommand(sqry.ToString(), myconn);
                                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Dispose();
                                if (rowCount > 0)
                                {
                                }
                                else
                                {
                                    sqry = new StringBuilder();
                                    sqry.Append(String.Format("update movies set status = 0 where id = {0}", cmbtitle.SelectedValue.ToString()));
                                    if (myconn.State == ConnectionState.Closed)
                                        myconn.Open();
                                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

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
            refreshDGV(true);
            setnormal();
        }

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResult.Columns[e.ColumnIndex].Name == "effective_date")
                ShortDateFormat(e);
        }

        private static void ShortDateFormat(DataGridViewCellFormattingEventArgs formatting)
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

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                //if (cbxfilter.Checked == false)
                //{
                    btnEdit.Enabled = true;
                    btnEdit.Text = "edit";

                    btnDelete.Enabled = true;
                    btnDelete.Text = "remove";

                    txtcode.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                    string strname = dgv.SelectedRows[0].Cells[2].Value.ToString();
                    for (int i = 0; i < cmbtitle.Items.Count; i++)
                    {
                        DataRowView drv = (DataRowView)cmbtitle.Items[i];
                        if (drv.Row["title"].ToString().ToUpper() == strname.ToUpper())
                        {
                            cmbtitle.SelectedIndex = i;
                            break;
                        }
                    }
                    txtshare.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();
                    string sdt = dgv.SelectedRows[0].Cells[5].Value.ToString();
                    //DateTime date = DateTime.ParseExact(sdt, "MM|dd|yyyy HH:mm:ss",CultureInfo.InvariantCulture);
                    txtdaycnt.Text = dgv.SelectedRows[0].Cells[6].Value.ToString();
                    DateTime date = Convert.ToDateTime(sdt);
                    dtdate.Value = date;
                //}
            }
        }

        private void txtshare_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar)
            //    && !char.IsDigit(e.KeyChar)
            //    && e.KeyChar != '.')
            //{
            //    e.Handled = true;
            //}

            ////// only allow one decimal point
            ////if (e.KeyChar == '.'
            ////    && (sender as TextBox).Text.IndexOf('.') > -1)
            ////{
            ////    e.Handled = true;
            ////}

            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
            //{
            //    e.Handled = true;
            //}
            
            
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
   
            if (Char.IsDigit(e.KeyChar))
            {

               
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (this.allowSpace && e.KeyChar == ' ')
            {

            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }

        public decimal DecimalValue
        {
            get
            {
                return Decimal.Parse(this.Text);
            }
        }

        public bool AllowSpace
        {
            set
            {
                this.allowSpace = value;
            }

            get
            {
                return this.allowSpace;
            }
        }

        private void cbxfilter_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cbxfilter.Checked == true)
            {
                refreshDGV(false);
                setnormal();
                txtcode.Text = "";
                txtcode.ReadOnly = false;
                cmbtitle.Enabled = true;
                txtshare.Text = "";
                txtshare.ReadOnly = false;
                cmbtitle.SelectedIndex = 0;

                //m_frmM.m_dt = m_dt;// m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                grpcontrol.Visible = true;
                grpfilter.Visible = true;

                cbxdate.Visible = true;
                cbxdate.Checked = true;
                cmbDate.Visible = true;
                cmbDate.SelectedIndex = 4;
                btnsearch2.Visible = false;
                btnclear2.Visible = false;
                cbxdate.Enabled = true;
                cmbDate.Enabled = true;
            }
            else
            {
                refreshDGV(true);
                setnormal();
                grpcontrol.Visible = true;
                grpfilter.Visible = false;
                cbxdate.Visible = false;
                cbxdate.Checked = false;
                cmbDate.Visible = false;
                cmbDate.SelectedIndex = 4;
                dtdate.Enabled = true;
                btnsearch2.Visible = true;
                btnclear2.Visible = true;
                cbxdate.Enabled = false;
                cmbDate.Enabled = false;
            }
        }

        private void frmProdShare_Load(object sender, EventArgs e)
        {

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            unselectbutton();

            refreshDGV(false);
            setnormal();

            txtcode.Text = "";
            txtcode.ReadOnly = false;
            cmbtitle.Enabled = false;
            cmbtitle.SelectedIndex = 0;
            txtshare.Text = "";
            txtshare.ReadOnly = false;
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            unselectbutton();
            DataTable dt = new DataTable();

            StringBuilder sqry = new StringBuilder();
            sqry.Append("[id] > -1");
            if (txtcode.Text.Trim() != "")
                sqry.Append(String.Format(" and [code] like '%{0}%'", txtcode.Text.Trim()));
            if (cmbtitle.Text.Trim() != "")
                sqry.Append(String.Format(" and [title] like '%{0}%'", cmbtitle.Text.Trim()));
            if (txtshare.Text.Trim() != "")
                sqry.Append(String.Format(" and [share] = '{0}'", txtshare.Text.Trim()));
            if (dtdate.Value != null && dtdate.Enabled == true)
                sqry.Append(String.Format(" and [effective_date] {0} '{1}'",cmbDate.Text , dtdate.Value.Date.ToString()));

            if (m_dt.Rows.Count == 0)
            {
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id, b.code, b.title, c.name as distributor, a.share_perc as share, a.effective_date ");
                sbqry.Append("from movies_distributor a ");
                sbqry.Append("left join movies b on a.movie_id = b.id ");
                sbqry.Append("inner join distributor c on b.dist_id = c.id");
                m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            }

            if (m_dt.Rows.Count > 0)
            {
                var foundRows = m_dt.Select(sqry.ToString());
                if (foundRows.Count() == 0)
                {
                    setDataGridView(dgvResult, m_dt);
                    txtcnt.Text = "Count: 0";
                    MessageBox.Show("No records found using the given information.", "Search Movies", MessageBoxButtons.OK);
                    return;
                }
                else
                    dt = foundRows.CopyToDataTable();
            }

            if (dt.Rows.Count > 0)
            {
                setDataGridView(dgvResult,dt);
                txtcnt.Text = "Count: " + String.Format("{0:#,##0}", dt.Rows.Count);
            }
        }

        private void kryptonGroup1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbxdate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxdate.Checked == true)
                dtdate.Enabled = true;
            else
                dtdate.Enabled = false;
        }

        private void dgvMovies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtdaycnt_TextChanged(object sender, EventArgs e)
        {
            //melvin 10-24-2014 for limiting number only
            if (!(txtdaycnt.Text == ""))
            {
                try
                {
                    if (Convert.ToInt32(txtdaycnt.Text) <= 0)
                    {
                        MessageBox.Show("Invalid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtdaycnt.Text = daycnt.ToString();
                        int textLength = txtdaycnt.Text.Length;
                        txtdaycnt.SelectionStart = textLength;
                        txtdaycnt.SelectionLength = 0;

                    }
                    else
                    {
                        int.TryParse(txtdaycnt.Text, out daycnt);
                    }
                }
                catch
                {
                    txtdaycnt.Text = daycnt.ToString();
                    int textLength = txtdaycnt.Text.Length;
                    txtdaycnt.SelectionStart = textLength;
                    txtdaycnt.SelectionLength = 0;

                }
            }
        }

        private void txtshare_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
