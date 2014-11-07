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
    public partial class frmMovieList : Form
    {
        frmRating frmrating = null;
        frmMain m_frmM = null;
        frmDistributor frmdistributor = null;
        MySqlConnection myconn = new MySqlConnection();
        public clscommon m_clscom = null;
        DataTable m_dt = new DataTable();
        bool allowSpace = false;

        public frmMovieList()
        {
            InitializeComponent();
        }

        private void frmMovieList_Load(object sender, EventArgs e)
        {
           
            unselectbutton();

        }

        public void refreshDGV(bool withCutOff)
        {
            StringBuilder sbqry = new StringBuilder();
            //first version working 
            //sbqry.Append("SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as distributor, a.share_perc as share, d.status_desc as status, a.encoded_date ");
            //sbqry.Append("FROM movies a, mtrcb b, distributor c, movies_status d WHERE a.rating_id = b.id and a.dist_id = c.id and a.status = d.status_id ");

            //second version test
            //sbqry.Append("select b.id, b.code, b.title, b.duration, d.name as rating, e.name as distributor, ");
            //sbqry.Append("min(c.start_time) as start_date, max(c.end_time) as end_date, ");
            //sbqry.Append("b.share_perc as share, f.status_desc as status, b.encoded_date ");
            //sbqry.Append("from movies_schedule a ");
            //sbqry.Append("left join movies b on a.movie_id = b.id ");
            //sbqry.Append("join movies_schedule_list c on a.id = c.movies_schedule_id ");
            //sbqry.Append("join mtrcb d on b.rating_id = d.id  ");
            //sbqry.Append("join distributor e on b.dist_id = e.id ");
            //sbqry.Append("join movies_status f on b.status = f.status_id ");
            //if(withCutOff)
            //    sbqry.Append(String.Format("where b.encoded_date >= '{0:yyyy-MM-dd}' ",m_clscom.m_clscon.MovieListCutOffDate));
            //sbqry.Append("group by a.movie_id ");
            //sbqry.Append("order by b.encoded_date desc");

            //3rd version for testing with startdate and enddate 
            sbqry.Append("SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as distributor, ");
            sbqry.Append("DATE_FORMAT(a.start_date,'%m-%d-%Y'), DATE_FORMAT(a.end_date,'%m-%d-%Y'), a.share_perc as share, d.status_desc as status, a.encoded_date ");
            sbqry.Append("FROM movies a, mtrcb b, distributor c, movies_status d WHERE a.rating_id = b.id ");
            sbqry.Append("and a.dist_id = c.id and a.status = d.status_id ");
          //  sbqry.Append("select * from movies");

           // if (withCutOff)
           //     sbqry.Append(String.Format("where encoded_date >= '{0:yyyy-MM-dd}'",m_clscom.m_clscon.MovieListCutOffDate));
           // MessageBox.Show(sbqry.ToString());
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);

            //sbqry = new StringBuilder();
            //sbqry.Append("SELECT b.description, b.name, b.id FROM classification b ");
            //sbqry.Append("order by b.description asc");
            //DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            //setDataGridViewII(dgvClass, dt);
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;

            refreshDGV(true);

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            populatedistributor();
            populateratings();
            //populateclassification();

            cmbdistributor.Enabled = false;
            cmbrating.Enabled = false;
            dgvResult.ClearSelection();

            setnormal();
            if(dgvClass.DataSource != null)
                setCheck(dgvClass, false);
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
            this.dgvResult.DataSource = null;
          
            if (dt.Rows.Count > 0)
                this.dgvResult.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                int iwidth = dgvResult.Width / 6;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth - 5;
                dgvResult.Columns[1].HeaderText = "Movie Code";
                dgvResult.Columns[2].Width = iwidth * 2;
                dgvResult.Columns[2].HeaderText = "Movie Title";
                dgvResult.Columns[3].Width = iwidth - 15;
                dgvResult.Columns[3].HeaderText = "Film Length";
                dgvResult.Columns[4].Width = iwidth - 5;
                dgvResult.Columns[4].HeaderText = "Movie Rating";
                dgvResult.Columns[5].Width = iwidth;
                dgvResult.Columns[5].HeaderText = "Distributor";
                dgvResult.Columns[6].Width = iwidth;
                dgvResult.Columns[6].HeaderText = "Start Date";
                dgvResult.Columns[7].Width = iwidth;
                dgvResult.Columns[7].HeaderText = "End Date";
                dgvResult.Columns[8].Width = 0;
                dgvResult.Columns[8].HeaderText = "Share Percentage";
                dgvResult.Columns[9].Width = 0;
                dgvResult.Columns[9].HeaderText = "Status";
                dgvResult.Columns[10].Width = 0;
                dgvResult.Columns[10].HeaderText = "Encoded Date";
            }
        }

        public void setDataGridViewII(DataGridView dgv,DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                int iwidth = dgv.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = (iwidth * 2) + 5;
                dgv.Columns[0].HeaderText = "Description";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Width = 0;
                dgv.Columns[1].HeaderText = "Name";
                dgv.Columns[1].ReadOnly = true;
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "ID";
                dgv.Columns[2].ReadOnly = true;
                dgv.Columns.Insert(0, cbx);
            }
        }

        public void setnormal()
        {
            txtcode.Text = "";
            txtcode.ReadOnly = true;
            txttitle.Text = "";
            txttitle.ReadOnly = true;
            txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString();
            txtshare.ReadOnly = true;
            cmbdistributor.Enabled = false;
            cmbrating.Enabled = false;

            if (cmbdistributor.Items.Count > 0)
                cmbdistributor.SelectedIndex = 0;

            if (cmbrating.Items.Count > 0)
                cmbrating.SelectedIndex = 0;

            //for (int i = 0; i < this.lstcls.Items.Count; i++)
            //    this.lstcls.Items[i].Checked = false;


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

            txtcnt.Text = String.Format("Count: {0:#,##0}",dgvResult.RowCount);
        }

        private void populateclassification()
        {
            DataTable dt = new DataTable();
            //lstcls.Items.Clear();
            ListViewItem lvitem = new ListViewItem();
            string sqry = "[id] > -1";

            if (m_frmM.m_dtclassification.Rows.Count == 0)
                m_frmM.m_dtclassification = m_clscom.setDataTable("select * from classification order by description asc", m_frmM._connection);

            if (m_frmM.m_dtclassification.Rows.Count > 0)
            {
                var foundRows = m_frmM.m_dtclassification.Select(sqry);
                if (foundRows.Count() == 0)
                {
                    lstcls.Items.Clear();
                }
                else
                    dt = foundRows.CopyToDataTable();

                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "description asc";
                    dt = dv.ToTable();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lvitem = lstcls.Items.Add(dt.Rows[i]["description"].ToString().Trim());
                        lvitem.SubItems.Add(dt.Rows[i]["id"].ToString().Trim());
                    }

                    //ListViewSorter sorter = new ListViewSorter();
                    //this.lstcls.ListViewItemSorter = sorter;
                    //if (!(lstcls.ListViewItemSorter is ListViewSorter))
                    //{
                    //    return;
                    //}
                    //sorter = (ListViewSorter)lstcls.ListViewItemSorter;
                    //lstcls.Sorting = SortOrder.Descending;
                    //lstcls.Sort();
                }
            }
        }

        private int calculatetime()
        {
            int totalmin = 0;
            string[] Parts = dttime.Text.Trim().Split(":".ToCharArray());

            int Hours = 0;
            if (!int.TryParse(Parts[0], out Hours))
            {
                Hours = 0;
            }
            if (Hours >= 24)
            {
                Hours = 0;
            }

            int Minutes = 0;
            if (!int.TryParse(Parts[1], out Minutes))
            {
                Minutes = 0;
            }
            if (Minutes >= 60)
            {
                Minutes = 0;
            }

            totalmin = (Hours * 60) + Minutes;

            return totalmin;
        }

        public void populatedistributor()
        {
            cmbdistributor.DataSource = null;
            DataTable dt = new DataTable();
            string sqry = "[id] > -1";
            if (m_frmM.m_dtdistributor.Rows.Count == 0)
                m_frmM.m_dtdistributor = m_clscom.setDataTable("select * from distributor order by name asc", m_frmM._connection);

            if (m_frmM.m_dtdistributor.Rows.Count > 0)
            {
                var foundRows = m_frmM.m_dtdistributor.Select(sqry);
                if (foundRows.Count() == 0)
                {
                    cmbdistributor.Items.Clear();
                }
                else
                    dt = foundRows.CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "name asc";
                    dt = dv.ToTable();

                    DataRow row = dt.NewRow();
                    row["id"] = "0";
                    row["name"] = "";
                    dt.Rows.InsertAt(row, 0);
                   
                    cmbdistributor.DataSource = dt;
                    cmbdistributor.ValueMember = "id";
                    cmbdistributor.DisplayMember = "name";
                }
            }
        }

        public void populateratings()
        {
            cmbrating.DataSource = null;
            DataTable dt = new DataTable();
            string sqry = "[id] > -1";

            if (m_frmM.m_dtrating.Rows.Count == 0)
                m_frmM.m_dtrating = m_clscom.setDataTable("select * from mtrcb order by id asc", m_frmM._connection);

            if (m_frmM.m_dtrating.Rows.Count > 0)
            {
                var foundRows = m_frmM.m_dtrating.Select(sqry);
                if (foundRows.Count() == 0)
                {
                    cmbrating.Items.Clear();
                }
                else
                    dt = foundRows.CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "name asc";
                    dt = dv.ToTable();

                    DataRow row = dt.NewRow();
                    row["id"] = "0";
                    row["name"] = "";
                    dt.Rows.InsertAt(row, 0);
                    cmbrating.DataSource = dt;
                    cmbrating.ValueMember = "id";
                    cmbrating.DisplayMember = "name";
                }
            }
        }

        private void btnaddrating_Click(object sender, EventArgs e)
        {
            if (frmrating == null)
                frmrating = new frmRating();

            frmrating.frmInitII(m_frmM,this,m_clscom);
            frmrating.ShowDialog();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void cbxfilter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxfilter.Checked == true)
            {
                refreshDGV(false);
                setnormal();
                txtcode.Text = "";
                txtcode.ReadOnly = false;
                txttitle.Text = "";
                txttitle.ReadOnly = false;
                txtshare.Text= "";
                txtshare.ReadOnly = false;
                cmbdistributor.SelectedIndex = 0;
                cmbdistributor.Enabled = true;
                cmbrating.SelectedIndex = 0;
                cmbrating.Enabled = true;

                //melvin 10-14-14
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                btnAdd.Enabled = false;

                //for further study classification for search
                //for (int i = 0; i < this.lstcls.Items.Count; i++)
                //    this.lstcls.Items[i].Checked = false;
                //lstcls.Enabled = false;

                m_frmM.m_dtmovies = m_dt;// m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                grpcontrol.Visible = true;
                grpfilter.Visible = true;
                btnAdd.Enabled = false;
            }
            else
            {
               
                refreshDGV(true);
                setnormal();
                grpcontrol.Visible = true;
                grpfilter.Visible = false;
                //melvin 10-14-14
                btnAdd.Enabled = true;
            }

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            unselectbutton();
        }

        private void btnadddistributor_Click(object sender, EventArgs e)
        {
            if (frmdistributor == null)
                frmdistributor = new frmDistributor();

            frmdistributor.frmInit(m_frmM, this);
            frmdistributor.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIE_ADD", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            if (btnAdd.Text  == "new")
            {
                dgvResult.Enabled = false;

                txtcode.Text = "";
                txtcode.ReadOnly = false;
                txttitle.Text = "";
                txttitle.ReadOnly = false;
                txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString(); 
                txtshare.ReadOnly = false;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;
               
                //melvin 10-13-14
                cbxfilter.Checked = false;
                cbxfilter.Enabled = false;
                
                txtcode.SelectAll();
                txtcode.Focus();

                cmbdistributor.Enabled = true;
                cmbrating.Enabled = true;

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("SELECT b.description, b.name, b.id FROM classification b ");
                sbqry.Append("order by b.description asc");
                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                if (dt.Rows.Count > 0)
                {
                    dgvClass.DataSource = null;
                    dgvClass.Columns.Clear();
                    setDataGridViewII(dgvClass, dt);
                }
            }
            else
            {
                string strstatus = String.Empty;
                if (txtcode.Text == "" || txttitle.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
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
                int inttime = calculatetime();
                if(inttime == 0)
                {
                    MessageBox.Show("Please fill the required information.");
                    dttime.Focus();
                    return;
                }
                //if(lstcls.CheckedItems.Count == 0)
                //{

                //    DialogResult ans = MessageBox.Show("You have forgotten to specify \n\rthe movie classification, continue?","Information",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                //    if(ans== System.Windows.Forms.DialogResult.No)
                //    {
                //        lstcls.Focus();
                //        return;
                //    }
                //}

                int chkcnt = 0;
                for (int i = 0; i < dgvClass.Rows.Count; i++)
                {
                    if (dgvClass[0, i].Value != null)
                    {
                        if ((bool)dgvClass[0, i].Value)
                        {
                            chkcnt += 1;
                        }
                    }
                }
                if (chkcnt == 0)
                {
                    MessageBox.Show("Please check the classification \n\r for the movie.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dgvClass.DataSource != null)
                        dgvClass.Focus();
                    return;
                }

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append("where code = @code " );
                sqry.Append("and title = @title ");
                sqry.Append("and dist_id = @dist ");
                sqry.Append("and share_perc = @share ");
                sqry.Append("and rating_id = @rating ");
                sqry.Append("and duration = @duration");
               
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);

                //melvin 10-31-2014 to secure in sql injection
                cmd.Parameters.AddWithValue("@code",txtcode.Text.Trim());
                cmd.Parameters.AddWithValue("@title", txttitle.Text.Trim());
                cmd.Parameters.AddWithValue("@dist",cmbdistributor.SelectedValue);
                cmd.Parameters.AddWithValue("@share",txtshare.Text.Trim());
                cmd.Parameters.AddWithValue("@rating",cmbrating.SelectedValue);
                cmd.Parameters.AddWithValue("@duration",inttime);
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
                //sqry.Append(String.Format("insert into movies value({0},'{1}','{2}',{3},{4},{5},{6},0,Now(),CURDATE(),CURDATE())",
                  //  0,txtcode.Text.Trim(),txttitle.Text.Trim(),cmbdistributor.SelectedValue,txtshare.Text.Trim(),
                    //cmbrating.SelectedValue,inttime));

                sqry.Append("insert into movies values(0,@code,@title,@dist,@share,@rating,@duration,0,Now(),CURDATE(),CURDATE())");

                try
                {
                    //insert value for the movies table
                    //MessageBox.Show(sqry.ToString());
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    //melvin 10-31-2014 to secure in sql injection
                    cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim());
                    cmd.Parameters.AddWithValue("@title", txttitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@dist", cmbdistributor.SelectedValue);
                    cmd.Parameters.AddWithValue("@share", txtshare.Text.Trim());
                    cmd.Parameters.AddWithValue("@rating", cmbrating.SelectedValue);
                    cmd.Parameters.AddWithValue("@duration", inttime);
                    cmd.ExecuteNonQuery();
                    int intid = Convert.ToInt32(cmd.LastInsertedId.ToString());

                    string strid = cmd.LastInsertedId.ToString();

                    cmd.Dispose();
                    //foreach (int i in lstcls.CheckedIndices)
                    //{
                    //    ListViewItem lvitem = lstcls.Items[i];
                    //    int intval = Convert.ToInt32(lvitem.SubItems[1].Text.ToString());
                    //    string strqry = String.Format("insert into movies_class value(0,{0},{1})", strid, intval);
                    //    try
                    //    {
                    //        if (myconn.State == ConnectionState.Closed)
                    //            myconn.Open();
                    //        MySqlCommand cmd2 = new MySqlCommand(strqry, myconn);

                    //        cmd2.ExecuteNonQuery();
                    //        cmd2.Dispose();
                    //    }
                    //    catch
                    //    {
                    //        if (myconn.State == ConnectionState.Open)
                    //            myconn.Close();
                    //        setnormal();
                    //        return;
                    //    }
                    //}
                    //int.TryParse(strid, out intid);

                    tabinsertcheck(myconn, dgvClass, intid);

                    if(myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "MOVIE_ADD", "MOVIES|MOVIES_CLASS",
                        Environment.MachineName.ToString(), "ADD NEW MOVIE INFO: NAME=" + 
                        this.txtcode.Text.Replace("'","''")
                        + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV(true);
                    setnormal();
                    
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refreshDGV(false);
                    dgvClass.DataSource = null;
                    dgvClass.Columns.Clear();
                    cbxfilter.Enabled = true;
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
                        StringBuilder sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into movies_class values(0,{0},{1})",
                           intid, dgv[3, i].Value.ToString()));
                       // MessageBox.Show(sqry.ToString());
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        myconn.Close();
                    }
                }

            }
        }

        private void lstcls_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewSorter Sorter = new ListViewSorter();
            lstcls.ListViewItemSorter = Sorter;
            if (!(lstcls.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)lstcls.ListViewItemSorter;

            if (Sorter.LastSort == e.Column)
            {
                if (lstcls.Sorting == SortOrder.Ascending)
                    lstcls.Sorting = SortOrder.Descending;
                else
                    lstcls.Sorting = SortOrder.Ascending;
            }
            else
            {
                lstcls.Sorting = SortOrder.Descending;
            }
            Sorter.ByColumn = e.Column;

            lstcls.Sort();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIE_DELETE", m_frmM._connection);
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
                    sqry.Append(String.Format("select count(*) from movies_distributor where movie_id = {0}", intid));
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
                    sqry.Append(String.Format("select count(*) from movies where id = {0} and status = 1", intid));
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
                        MessageBox.Show("Can't remove this record, \n\rit is being used by other records.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from movies_class where movie_id = {0}", intid));
                        MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from movies where id = {0} and status = 0", intid));
                
                        if(myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();

                        MessageBox.Show("Successfully deleted", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_clscom.AddATrail(m_frmM.m_userid, "MOVIE_DELETE", "MOVIES|MOVIES_CLASS",
                            Environment.MachineName.ToString(), "UPDATED MOVIE INFO: NAME=" + 
                            this.txtcode.Text.Replace("'","''")
                            + " | ID=" + intid.ToString(), m_frmM._connection);

                        dgvClass.DataSource = null;
                        dgvClass.Columns.Clear();
                    }
                    catch (MySqlException er)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show(er.Message, this.Text);
                    }
                }
            }
            else
            {
                dgvClass.DataSource = null;
                dgvClass.Columns.Clear();
                cbxfilter.Enabled = true;
            }
            refreshDGV(true);
            setnormal();
           
        }

        private void txtshare_KeyPress(object sender, KeyPressEventArgs e)
        {
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
            //if (!char.IsControl(e.KeyChar)
            //    && !char.IsDigit(e.KeyChar)
            //    && e.KeyChar != '.')
            //{
            //    e.Handled = true;
            //}

            ////// only allow one decimal point
            ////if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            ////{
            ////    e.Handled = true;
            ////}

            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1 && (sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length >= 3)
            //{
            //    e.Handled = true;
            //}
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

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResult.Columns[e.ColumnIndex].Name == "duration")
                ShortFormtTimeFormat(e);
            else if(dgvResult.Columns[e.ColumnIndex].Name == "start_date" || dgvResult.Columns[e.ColumnIndex].Name == "end_date")
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

        private static void ShortFormtTimeFormat(DataGridViewCellFormattingEventArgs formatting)
        {
            if (formatting.Value != null)
            {
                try
                {
                    TimeSpan result = TimeSpan.FromMinutes(Convert.ToDouble(formatting.Value.ToString()));
                    string hours = ((int)result.TotalHours).ToString();
                    string minutes = String.Format("{0:00}", result.Minutes);

                    if (minutes.ToString() == "0")
                        minutes = "00";
                    string fromTimeString = hours + ":" + minutes;
                    formatting.Value = fromTimeString;
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
                if (cbxfilter.Checked == false)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Text = "edit";
                }
                    btnDelete.Enabled = true;
                    btnDelete.Text = "remove";

                    //set values
                    txtcode.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                    this.txttitle.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                    TimeSpan result = TimeSpan.FromMinutes(Convert.ToDouble(dgv.SelectedRows[0].Cells[3].Value.ToString()));
                    string hours = ((int)result.TotalHours).ToString();
                    string minutes = String.Format("{0:00}", result.Minutes);
                    this.dttime.Value = new DateTime(2014, 6, 2, (int)result.TotalHours, result.Minutes, 0);
                    cmbrating.Text = dgv.SelectedRows[0].Cells[4].Value.ToString();
                    cmbdistributor.Text = dgv.SelectedRows[0].Cells[5].Value.ToString();
                    txtshare.Text = dgv.SelectedRows[0].Cells[8].Value.ToString();

                    StringBuilder sbqry = new StringBuilder();
                    sbqry.Append("SELECT b.description, b.name, a.class_id as id FROM movies_class a, classification b ");
                    sbqry.Append(String.Format("where a.movie_id = {0} and a.class_id = b.id order by b.description asc", dgv.SelectedRows[0].Cells[0].Value.ToString()));
                    DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);

                    if (dt.Rows.Count > 0)
                    {
                        dgvClass.DataSource = null;
                        dgvClass.Columns.Clear();
                        setDataGridViewII(dgvClass, dt);
                    }//rmb ADDED ELSE 10.31.2014
                    else
                    {
                        dgvClass.DataSource = null;
                        dgvClass.Columns.Clear();
                    }
                        //melvin for clearing dgvClass
                    //else
                    //{
                        
                    //    dgvClass.DataSource = null;
                    //}

                    setCheck(dgvClass, true);

                    //checkList(dgv.SelectedRows[0].Cells[0].Value.ToString());
                    //for (int i = 0; i < this.lstcls.Items.Count; i++)
                    //    this.lstcls.Items[i].Checked = true;
                //}
            }
        }

        public void checkList(string strid)
        {
            for (int i = 0; i < this.lstcls.Items.Count; i++)
                this.lstcls.Items[i].Checked = false;

            List<int> intlist = new List<int>();
            string sqry = String.Format("select class_id from movies_class where movie_id = {0}", strid);
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();

            if (myconn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand(sqry, myconn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    for (int ii = 0; ii < this.lstcls.Items.Count; ii++)
                        if (String.Compare(this.lstcls.Items[ii].SubItems[1].Text,dataReader[0].ToString()) == 0)
                            this.lstcls.Items[ii].Checked = true;
                }
                dataReader.Close();
                myconn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIE_EDIT", m_frmM._connection);
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
                MessageBox.Show("Please select a record from the list.", this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            else if(dgvResult.SelectedRows.Count == 1)
                dgvResult.Enabled = false;

            if (btnEdit.Text == "edit")
            {
                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                cmbdistributor.Enabled = true;
                cmbrating.Enabled = true;

                txtcode.ReadOnly = false;
                txttitle.ReadOnly = false;
                txtshare.ReadOnly = false;

                MySqlConnection myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
 
                int movieid = -1;
                if (dgvResult.SelectedRows.Count == 1)
                    movieid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.description, a.name, a.id FROM classification a ");
                sbqry.Append(String.Format("where a.id not in(select b.class_id from movies_class b where b.movie_id = {0}) ", movieid));
                sbqry.Append("order by a.description asc");
                DataTable dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                DataTable dgvdt = (DataTable)dgvClass.DataSource;
                //melvin 10-29-2014
                if (!(dgvdt == null))
                {
                    dgvdt.Merge(dt);
                    //RMB added sort upon edit 10.31.2014
                    DataView dv = dgvdt.AsDataView();
                    dv.Sort = "description asc";
                    dgvdt = dv.ToTable();
                    dgvClass.DataSource = dgvdt;

                }
                else
                {
                    //RMB remarked
                    //DataGridViewCheckBoxColumn d1 = new DataGridViewCheckBoxColumn();
                    //d1.HeaderText = "";
                    //d1.Width = 40;
                    //dgvClass.Columns.Add(d1);
                    dgvClass.DataSource = null;
                    dgvClass.Columns.Clear();
                    setDataGridViewII(dgvClass, dt);
                    //dgvClass.DataSource = dt;
                }
                
               // dgvdt.Merge(dt);
               // dgvClass.DataSource = dgvdt;

                setCheck(dgvClass, false);
                int rowCount = 0;
                StringBuilder sqry2 = new StringBuilder();
                for (int i = 0; i < dgvClass.Rows.Count; i++)
                {
                    int intmid = 0;
                    int intout = -1;
                    if (int.TryParse(dgvClass[3, i].Value.ToString(), out intout))
                        intmid = intout;

                    if (intmid > 0)
                    {
                        sqry2 = new StringBuilder();
                        sqry2.Append(String.Format("select count(*) from movies_class a where a.movie_id = {0}", movieid));
                        sqry2.Append(String.Format(" and a.class_id = {0}", intmid));
                        if (myconn != null)
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd = new MySqlCommand(sqry2.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }

                        if (rowCount > 0)
                            dgvClass[0, i].Value = (object)true;
                    }
                }
            }
            else if (btnEdit.Text == "update")
            {
                string strstatus = String.Empty;
                if (txtcode.Text == "" || txttitle.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtcode.SelectAll();
                    txtcode.Focus();
                    return;
                }
                if(txtshare.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtshare.Focus();
                    return;
                }
                int inttime = calculatetime();
                if(inttime == 0)
                {
                    MessageBox.Show("Please fill the required information.");
                    dttime.Focus();
                    return;
                }
                //if(lstcls.CheckedItems.Count == 0)
                //{

                //    DialogResult ans = MessageBox.Show("You have forgotten to specify \n\rthe movie classification, continue?","Information",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                //    if(ans== System.Windows.Forms.DialogResult.No)
                //    {
                //        lstcls.Focus();
                //        return;
                //    }
                //}

                int chkcnt = 0;
                for (int i = 0; i < dgvClass.Rows.Count; i++)
                {
                    if (dgvClass[0, i].Value != null)
                    {
                        if ((bool)dgvClass[0, i].Value)
                        {
                            chkcnt += 1;
                        }
                    }
                }
                if (chkcnt == 0)
                {
                    MessageBox.Show("Please check the classification \n\r for the movie.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dgvClass.DataSource != null)
                        dgvClass.Focus();
                    return;
                }

                int totaltime = calculatetime();
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                int intid = -1;
                if(dgvResult.SelectedRows.Count == 1)
                    intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                //first check if the id is existing
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append(String.Format("where id = {0}", intid));

                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

               /* if (rowCount > 0)
                {
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't update this record, \n\rit is already being used by the system.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }*/

                //validate for the existance of the record
                //sqry = new StringBuilder();
                //sqry.Append("select count(*) from movies ");
                //sqry.Append(String.Format("where code = '{0}' ", txtcode.Text.Trim()));
                //sqry.Append(String.Format("and title = '{0}' ", txttitle.Text.Trim()));
                //sqry.Append(String.Format("and dist_id = {0} ", cmbdistributor.SelectedValue));
                //sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                //sqry.Append(String.Format("and rating_id = {0} ", cmbrating.SelectedValue));
                //sqry.Append(String.Format("and duration = {0} ", totaltime));
                //sqry.Append(String.Format("and id != {0}",intid));

                //melvin 11-4-2014
                sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append("where code = @code and title = @title ");
                sqry.Append(String.Format("and dist_id = {0} ", cmbdistributor.SelectedValue));
                sqry.Append(String.Format("and share_perc = {0} ", txtshare.Text.Trim()));
                sqry.Append(String.Format("and rating_id = {0} ", cmbrating.SelectedValue));
                sqry.Append(String.Format("and duration = {0} ", totaltime));
                sqry.Append(String.Format("and id != {0}", intid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim());
                cmd.Parameters.AddWithValue("@title", txttitle.Text.Trim());
                rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (rowCount > 0)
                {
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't update this record, \n\rit is already being used by the system.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //check for new and active movies
                sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append(String.Format("where id = {0}  and status = 1", intid));

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
                    MessageBox.Show("Can't update this record, \n\rit is already being used by the system.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StringBuilder strqry = new StringBuilder();
                //melvin 10-4-2014
                strqry.Append("update movies set code=@code, title=@title,");
                strqry.Append(String.Format(" share_perc = {0},",txtshare.Text.Trim()));
                strqry.Append(String.Format(" duration = {0},",totaltime));
                strqry.Append(String.Format(" dist_id = {0},", cmbdistributor.SelectedValue));
                strqry.Append(String.Format(" rating_id = {0} ", cmbrating.SelectedValue));
                strqry.Append(String.Format(" where id = {0}", intid));

                try
                {
                    //update the movies table
                    if(myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(strqry.ToString(), myconn);
                    //melvin 11-4-2014
                    cmd.Parameters.AddWithValue("@code", txtcode.Text.Trim());
                    cmd.Parameters.AddWithValue("@title", txttitle.Text.Trim());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //remove the movies_class values
                    strqry = new StringBuilder();
                    strqry.Append(String.Format("delete from movies_class where movie_id = {0}",intid));
                    try
                    {
                        if(myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd3 = new MySqlCommand(strqry.ToString(), myconn);
                        cmd3.ExecuteNonQuery();
                        cmd3.Dispose();

                        tabinsertcheck(myconn, dgvClass, intid);
                    }
                    catch(MySqlException err)
                    {
                        MessageBox.Show("Can't connect to the contact table."+err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    ////insert the movie_class update
                    //foreach (int i in lstcls.CheckedIndices)
                    //{
                    //    ListViewItem lvitem = lstcls.Items[i];
                    //    int intval = Convert.ToInt32(lvitem.SubItems[1].Text.ToString());
                    //    strqry = new StringBuilder();
                    //    strqry.Append(String.Format("insert into movies_class value(0,{0},{1})", intid, intval));
                    //    try
                    //    {
                    //        if (myconn.State == ConnectionState.Closed)
                    //            myconn.Open();
                    //        MySqlCommand cmd2 = new MySqlCommand(strqry.ToString(), myconn);
                    //        cmd2.ExecuteNonQuery();
                    //        cmd2.Dispose();
                    //    }
                    //    catch
                    //    {
                    //        if (myconn.State == ConnectionState.Open)
                    //            myconn.Close();
                    //        setnormal();
                    //        return;
                    //    }
                    //}
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    //update start date and end date
                    strqry = new StringBuilder();
                    //melvin 10-30-2014
                    try
                    {
                        strqry.Append("update movies a set a.start_date = (select min(b.movie_date) ");
                        strqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", intid));

                        //MessageBox.Show(strqry.ToString());
                        strqry.Append(String.Format("where a.id = {0}", intid));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd4 = new MySqlCommand(strqry.ToString(), myconn);
                        cmd4.ExecuteNonQuery();
                        cmd4.Dispose();
                    }
                    catch
                    {
                    }
                    try
                    {
                        strqry = new StringBuilder();
                        strqry.Append("update movies a set a.end_date = (select max(b.movie_date) ");
                        strqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", intid));
                        strqry.Append(String.Format("where a.id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd5 = new MySqlCommand(strqry.ToString(), myconn);
                        cmd5.ExecuteNonQuery();
                        cmd5.Dispose();
                    }
                    catch 
                    {
                    }
                    m_clscom.AddATrail(m_frmM.m_userid, "MOVIE_EDIT", "MOVIES|MOVIES_CLASS",
                        Environment.MachineName.ToString(), "UPDATED MOVIE INFO: NAME=" +
                        this.txtcode.Text.Replace("'","''")
                        + " | ID=" + intid.ToString(), m_frmM._connection);

                    refreshDGV(true);
                    setnormal();

                    MessageBox.Show("You have successfully updated \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvClass.DataSource = null;
                    dgvClass.Columns.Clear();
                }
                catch(Exception err)
                {
                    MessageBox.Show("Can't connect to the movies table."+ err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnsearch_Click(object sender, EventArgs e)
        {
            int tottime = calculatetime();
            unselectbutton();
            DataTable dt = new DataTable();

            StringBuilder sqry = new StringBuilder();
            sqry.Append("[id] > -1");
            if (txtcode.Text.Trim() != "")
                sqry.Append(String.Format(" and [code] like '%{0}%'",txtcode.Text.Trim()));
            if(txttitle.Text.Trim() != "")
                sqry.Append(String.Format(" and [title] like '%{0}%'", txttitle.Text.Trim()));
            if(cmbdistributor.Text != "")
                sqry.Append(String.Format(" and [distributor] = '{0}'", cmbdistributor.Text));
            if(cmbrating.Text != "")
                sqry.Append(String.Format(" and [rating] = '{0}'", cmbrating.Text));
            if(txtshare.Text.Trim() != "")
                sqry.Append(String.Format(" and [share] = '{0}'", txtshare.Text.Trim()));
            if(tottime > 0)
                sqry.Append(String.Format(" and [duration] >= '{0}'", tottime));

            if (m_frmM.m_dtmovies.Rows.Count == 0)
            {
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as distributor, a.share_perc as share, d.status_desc as status ");
                sbqry.Append("FROM movies a,  mtrcb b, distributor c, movie_status d WHERE a.rating_id = b.id and a.dist_id = c.id and a.status = d.status_id");
                m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                m_frmM.m_dtmovies = m_dt;
            }

            //if (lstcls.CheckedItems.Count > 0)
            //{
            //    StringBuilder newsb = new StringBuilder();
            //    newsb.Append("SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as distributor, a.share_perc as share, d.status_desc as status ");
            //    newsb.Append("FROM movies a,  mtrcb b, distributor c, movie_status d ");
            //    newsb.Append(sqry.ToString());

            //}
            //else if (lstcls.CheckedItems.Count == 0)
            //{
            var foundRows = m_dt.Select(sqry.ToString());
            if (foundRows.Count() == 0)
            {
                setDataGridView(m_dt);
                txtcnt.Text = "Count: 0";
                MessageBox.Show("No records found using the given information.", "Search Movies", MessageBoxButtons.OK);
                return;
            }
            else
                dt = foundRows.CopyToDataTable();
            //}
             
            if (dt.Rows.Count > 0)
            {
                setDataGridView(dt);
                txtcnt.Text = "Count: " + String.Format("{0:#,##0}", dt.Rows.Count);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            unselectbutton();

            refreshDGV(false);
            setnormal();

            txtcode.Text = "";
            txtcode.ReadOnly = false;
            txttitle.Text = "";
            txttitle.ReadOnly = false;
            txtshare.Text = m_clscom.m_clscon.MovieDefaultShare.ToString();
            txtshare.ReadOnly = false;
            cmbdistributor.Enabled = true;
            cmbrating.Enabled = true;
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grpparameter_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
