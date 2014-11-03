using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using System.Globalization;
using MySql.Data.MySqlClient;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmMovieSched : Form
    {
        frmMain m_frmM = null;
        clscommon m_clscom = null;
        List<CalendarItem> _items = new List<CalendarItem>();
        CalendarItem contextItem = null;
        DataTable m_dtcinema = new DataTable();
        MySqlConnection myconn = new MySqlConnection();
        DataTable m_dtmovies = new DataTable();
        DataTable m_dtpatrons = new DataTable();
        bool boolresultclick = false;
        //string cntrol = String.Empty;

        public frmMovieSched()
        {
            InitializeComponent();

            calview.MonthTitleColor = calview.MonthTitleColorInactive = CalendarColorTable.FromHex("#C2DAFC");
            calview.ArrowsColor = CalendarColorTable.FromHex("#77A1D3");
            calview.DaySelectedBackgroundColor = CalendarColorTable.FromHex("#F4CC52");
            calview.DaySelectedTextColor = calsked.ForeColor;
        }

        private void frmMovieSched_Load(object sender, EventArgs e)
        {

        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            unselectbutton();

            m_frmM = frm;
            m_clscom = cls;

            //refreshDGV();
            setnormal();
            dtcalview.Value = DateTime.Now;

            populateCinema();

            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            DateTime dtstart = GetFirstDayOfWeek(DateTime.Today, defaultCultureInfo);
            calsked.SetViewRange(dtstart, dtstart.AddDays(6));
            
        }

        private void setnormal()
        {
            //dtcalview.Value = datestart.Value;

            grpgrant.Visible = false;
            dateend.Value = DateTime.Now;
            datestart.Value = DateTime.Now;
            
            //cntrol = "";
            txtMC.Text = "";
            txtMT.Text = "";
            if (cmbCinema.Items.Count > 0)
                cmbCinema.SelectedIndex = 0;

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
            dgvMovies.DataSource = null;
            dgvMovies.Columns.Clear();
            dgvResult.DataSource = null;
            dgvResult.Columns.Clear();

            calsked.Enabled = true;
            txtintermision.Text = m_frmM.m_clscom.m_clscon.MovieIntermissionTime.ToString();
        }

        private void addScreeningSched(string strcinemaid, DateTime dtstart)
        {
           
            DateTime dtref = dtstart;
             myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection; 
            int idcnm = 0;
            int id = -1;
            if (int.TryParse(strcinemaid, out id))
                idcnm = id;

            StringBuilder sqry = new StringBuilder();
            int cntr = 0;
            do
            {
                sqry = new StringBuilder();
                sqry.Append("select a.start_time, a.end_time, c.code, a.id, a.status from movies_schedule_list a ");
                sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                sqry.Append("left join movies c on b.movie_id = c.id ");
                sqry.Append(String.Format("where b.cinema_id = {0}  ", idcnm));
                sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", dtref)));
                sqry.Append("order by c.code, a.start_time desc");

                CalendarItem calitem = null;
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    string firsttitle = String.Empty;
                    int result = 0;
                    int intid = 0;
                    int intsw = 0;
                    Color itemcolor = Color.FromArgb(100, 225, 225, 100);//Color.LightSteelBlue;
                    int reccntr = 0;
                    while (dr.Read())
                    {
                        reccntr += 1;
                        
                        
                        string sval = dr["code"].ToString();
                        string stime = dr["start_time"].ToString();
                        string etime = dr["end_time"].ToString();
                        string stat = dr["status"].ToString();
                        DateTime dtstime = Convert.ToDateTime(stime);
                        DateTime dtetime = Convert.ToDateTime(stime);
                        string stimeval = dtstime.ToShortTimeString();// +" - " + dtetime.ToShortTimeString();
                        if (sval != "")
                        {
                            if (intsw == 0)
                            {
                                firsttitle = sval;
                                itemcolor = Color.LightSteelBlue;//Color.FromArgb(100, 100, 225, 225);
                            }
                            
                            if (firsttitle != sval)
                            {
                                calitem = new CalendarItem(calsked, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), firsttitle);
                                calitem.BackgroundColor = itemcolor;
                                if (calsked.ViewIntersects(calitem))
                                    calsked.Items.Add(calitem);

                                intsw = 0;
                                if (stat == "0")
                                {
                                    itemcolor = Color.Red;
                                }
                                else
                                {
                                    itemcolor = Color.FromArgb(100, 100, 225, 225);//Color.FromArgb(100, 225, 225, 100);
                                }
                                firsttitle = sval;
                            }
                            if (stat == "0")
                            {
                                itemcolor = Color.Red;
                            }
                            //RMB remarked 11.3.2014
                            //else
                            //{
                            //    itemcolor = Color.FromArgb(100, 100, 225, 225);
                            //}
                            calitem = new CalendarItem(calsked, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), stimeval);
                            calitem.BackgroundColor = itemcolor;
                            if (calsked.ViewIntersects(calitem))
                                calsked.Items.Add(calitem);

                            if (reccntr == dt.Rows.Count)
                            {
                                calitem = new CalendarItem(calsked, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), firsttitle);
                                calitem.BackgroundColor = itemcolor;
                                if (calsked.ViewIntersects(calitem))
                                    calsked.Items.Add(calitem);
                            }
                            intsw = 1;
                        }
                    }
                }
                cmd.Dispose();

                cntr += 1;
                dtref = dtstart.AddDays((double)cntr);
            } while (cntr < 7);


           
        }

        private void populateCinema()
        {
            cmbCinema.DataSource = null;
            DataTable dt = new DataTable();
            string sqry = "[id] > -1";
            m_dtcinema = m_clscom.setDataTable("select * from cinema order by name asc", m_frmM._connection);

            if (m_dtcinema.Rows.Count > 0)
            {
                DataView dv = m_dtcinema.AsDataView();
                dv.Sort = "name asc";
                dt = dv.ToTable();

                DataRow row = dt.NewRow();
                row["id"] = "0";
                row["name"] = "";
                dt.Rows.InsertAt(row, 0);

                cmbCinema.DataSource = dt;
                cmbCinema.ValueMember = "id";
                cmbCinema.DisplayMember = "name";
            }
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }


        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void calview_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCinema.Items.Count > 0)
                {
                    var val = cmbCinema.SelectedValue;
                    if (val.ToString() != "System.Data.DataRowView" && val.ToString() != "" && val != null)
                    {
                        foreach (CalendarItem item in calsked.GetSelectedItems())
                        {

                        }

                        calsked.SetViewRange(calview.SelectionStart, calview.SelectionEnd);

                        CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                        DateTime dtstart = GetFirstDayOfWeek(calview.SelectionStart, defaultCultureInfo);
                        addScreeningSched(cmbCinema.SelectedValue.ToString(), dtstart);

                        clearCalendarItem();
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void calsked_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            //PlaceItems();
        }

        private void PlaceItems()
        {
            foreach (CalendarItem item in _items)
            {
                if (calsked.ViewIntersects(item))
                {
                    calsked.Items.Add(item);
                }
            }
        }

        private void calsked_ItemClick(object sender, CalendarItemEventArgs e)
        {
            //MessageBox.Show("item click");
            foreach (CalendarItem item in calsked.GetSelectedItems())
            {
                DateTime dt = item.Date;
                if (dt != null)
                {
                    dgvResult.DataSource = null;
                    dgvResult.Columns.Clear();
                    selectcalendardate(dt);

                    if (btnAdd.Text == "new")
                    {
                        dgvpatrons.DataSource = null;
                        dgvpatrons.Columns.Clear();
                    }

                }
                if (dgvResult.DataSource != null)
                {
                    //string st = item.StartDate.ToString();
                    //string ed = item.EndDate.ToString();
                    string str = item.Text.ToString();

                    dgvResult.ClearSelection();
                    for (int i = 0; i < dgvResult.Rows.Count; i++)
                    {
                        if (Convert.ToDateTime(dgvResult.Rows[i].Cells[1].Value.ToString()).ToShortTimeString() == str.Trim())
                        {
                            //cntrol = "resultselect";
                            boolresultclick = true;
                            dgvResult.Rows[i].Selected = true;

                            //melvin 10-29-2014
                            
                            string id = dgvResult.Rows[i].Cells[0].Value.ToString();
                            StringBuilder sqry = new StringBuilder();
                            sqry.Append("select status from movies_schedule_list where id=" + id);
                            MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                           // MessageBox.Show(sqry.ToString());
                            int res = Convert.ToInt32(cmd.ExecuteScalar());
                            if (res == 0)
                            {
                                rbUnpublish.Checked = true;

                            }
                            else
                            {
                                rbPublish.Checked = true;  
                            }
                            btnsearch.PerformClick();
                            break;
                        }
                    }
                   
                }

            }

           
        }

        private void calsked_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("click");
            //DateTime dt = calsked.SelectionEnd.Date;
            //calsked.SelectionEnd
            //DateTime dstart = SelectedElementEnd.Date;
            
        }

        private void calsked_DoubleClick(object sender, EventArgs e)
        {
            //calsked.AllowNew = true;
            
            //MessageBox.Show("double click");
            //return;
        }

        private void cmbCinema_SelectedIndexChanged(object sender, EventArgs e)
        {

            var val = cmbCinema.SelectedValue;
            if (val.ToString() != "System.Data.DataRowView" && val.ToString() != "" && val != null)
            {
                txtLabel.Values.Text = cmbCinema.Text;

                ////clearCalendarItem();

                calsked.SetViewRange(calview.SelectionStart, calview.SelectionEnd);

                //CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                ////DateTime dtstart = GetFirstDayOfWeek(DateTime.Today, defaultCultureInfo);
                //DateTime dtstart = GetFirstDayOfWeek(dtcalview.Value, defaultCultureInfo);
                addScreeningSched(cmbCinema.SelectedValue.ToString(), calview.SelectionStart);

                

                //calsked.Invalidate();
                //calsked.Refresh();

                if (btnAdd.Text == "save")
                {
                    dgvpatrons.DataSource = null;
                    dgvpatrons.Columns.Clear();
                    StringBuilder sbqry = new StringBuilder();
                    sbqry.Append("select b.name, a.price, a.patron_id as id ");
                    sbqry.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                    sbqry.Append(String.Format("where a.cinema_id = {0} ", cmbCinema.SelectedValue.ToString()));
                    sbqry.Append("and b.name is not null ");
                    sbqry.Append("order by a.id asc");
                    m_dtpatrons = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                    setDataGridViewIII(m_dtpatrons, dgvpatrons);
                }
            }
            else
                txtLabel.Values.Text = "";

            clearCalendarItem();
        }


        private void clearCalendarItem()
        {

            Stack<CalendarItem> toDelete = new Stack<CalendarItem>();

            CalendarRenderer calren = calsked.Renderer;
            foreach (CalendarDay day in calren.Calendar.Days)
            {
                
                //if (day.ContainedItems.Count > 0)
                //    day.ContainedItems.Clear();
                //int cntr = 0;
                //do
                //{
                //    CalendarItem item = day.ContainedItems[cntr];
                //    day.ContainedItems.Remove(item);
                //    cntr -= 1;
                //} while (day.ContainedItems.Count != 0);

               foreach (CalendarItem item in day.ContainedItems)
               {
                   toDelete.Push(item);
                   while (toDelete.Count > 0)
                   {
                       CalendarItem item1 = toDelete.Pop();
                       day.ContainedItems.Remove(item1);
                   }
                    //string sval = item.Date.ToString();
                    //if (day.ContainedItems.RemoveAll(item))
                    //{
                    //    MessageBox.Show(sval);
                    //    calren.PerformItemsLayout();
                    //    calsked.Invalidate();
                    //}
                   calren.PerformItemsLayout();
                   break;
                    //    OnItemDeleted(new CalendarItemEventArgs(item));
                }
                //day.ContainedItems.Clear();
                
            }
            
            calsked.Invalidate();
        }

        public void setDataGridView(DataGridView dgv,DataTable dt)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dgv.DataSource = dt;
                int iwidth = dgv.Width / 4;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth - 15;
                dgv.Columns[1].HeaderText = "Code";
                dgv.Columns[2].Width = iwidth * 3;
                dgv.Columns[2].HeaderText = "Title";
                dgv.Columns[3].Width = 0;
                dgv.Columns[3].HeaderText = "Film Length";
            }
        }
        //melvin
        private void txtEnabled(bool con)
        {
            txtMC.Enabled = con;
            txtMT.Enabled = con;
            txtintermision.Enabled = con;
            dateend.Enabled = con;
            datestart.Enabled = con;
            timeend.Enabled = con;
            timestart.Enabled = con;
            dtduration.Enabled = con;
            rbtnGuarateed.Enabled = con;
            rbtnReserved.Enabled = con;
            rbtnUnlimited.Enabled = con;
            cbxintermision.Enabled = con;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIESKED_ADD", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }
            bool addanother = false;
            unselectbutton();

            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            if (btnAdd.Text == "new")
            {
                grpgrant.Visible = true;
                //cntrol = "";
                datestart.Value = DateTime.Today;

                dgvResult.Enabled = true;
                dgvMovies.Enabled = true;
                txtMC.Text = "";
                txtMC.ReadOnly = false;
                txtMT.Text = "";
                txtMT.ReadOnly = false;

                unselectbutton();
                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;
                txtEnabled(true);
                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtMC.SelectAll();
                txtMC.Focus();

                rbtnReserved.Checked = true; 
                //RMB 10.31.2014
                //rbtnGuarateed.Checked = true;

                dgvMovies.DataSource = null;
                dgvMovies.Columns.Clear();

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id, a.code, a.title,a.duration ");
                sbqry.Append("from movies a ");
                sbqry.Append(String.Format(" where a.encoded_date >= '{0:yyyy-MM-dd}' order by a.title asc", m_clscom.m_clscon.MovieListCutOffDate));
                //sbqry.Append("order by a.title asc");

                m_dtmovies = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                setDataGridView(dgvMovies,m_dtmovies);

                if (cmbCinema.Items.Count > 0)
                {
                    var val = cmbCinema.SelectedValue;
                    if (val.ToString() != "System.Data.DataRowView")
                    {
                        int cinemaid = -1;
                        if (int.TryParse(val.ToString(), out cinemaid))
                        {
                            dgvpatrons.DataSource = null;
                            dgvpatrons.Columns.Clear();
                            sbqry = new StringBuilder();
                            sbqry.Append("select b.name, a.price, a.patron_id as id ");
                            sbqry.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                            sbqry.Append(String.Format("where a.cinema_id = {0} ", cinemaid));
                            sbqry.Append("and b.name is not null ");
                            sbqry.Append("order by a.id asc");
                            m_dtpatrons = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                            setDataGridViewIII(m_dtpatrons, dgvpatrons);
                        }
                    }
                }
                dgvResult.Enabled = false;
                calsked.Enabled = false;
            }
            else
            {
                string strstatus = String.Empty;

                if (txtMC.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtMC.SelectAll();
                    txtMC.Focus();
                    return;
                }
                if (txtMT.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtMT.SelectAll();
                    txtMT.Focus();
                    return;
                }

                if (datestart.Text == "" || datestart.Value == null)
                {
                    MessageBox.Show("Please fill the start date information.");
                    datestart.Focus();
                    return;
                }

                if (dateend.Text == "" || dateend.Value == null)
                {
                    MessageBox.Show("Please fill the end date information.");
                    dateend.Focus();
                    return;
                }

                if (timestart.Text == "" || timestart.Value == null)
                {
                    MessageBox.Show("Please fill the start time information.");
                    timestart.Focus();
                    return;
                }

                if (timeend.Text == "" || timeend.Value == null)
                {
                    MessageBox.Show("Please fill the end time information.");
                    timeend.Focus();
                    return;
                }
                int inttype = -1;
                if (rbtnReserved.Checked == true)
                    inttype = 1;
                else if (this.rbtnGuarateed.Checked == true)
                    inttype = 2;
                else if (rbtnUnlimited.Checked == true)
                    inttype = 3;

                if (inttype == -1)
                {
                    MessageBox.Show("Please select the seating information.");
                    return;
                }

                int movieid = 0;
                int cinemaid = 0;
                if (dgvMovies.SelectedRows.Count == 1)
                {
                    movieid = Convert.ToInt32(dgvMovies.SelectedRows[0].Cells[0].Value.ToString());
                    cinemaid = Convert.ToInt32(cmbCinema.SelectedValue.ToString());
                }
                else if (dgvMovies.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a movie.");
                    dgvMovies.Focus();
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
                    MessageBox.Show("Please check the patrons pricing \n\r for the selected movies.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvpatrons.Focus();
                    return;
                }

                if (cbxintermision.Checked == true)
                {
                    if (txtintermision.Text.Trim() == "")
                    {
                        MessageBox.Show("Please provide a value\n\r for the itermission minutes.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtintermision.Focus();
                        return;
                    }
                }


                chkcnt = 0;
                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append(String.Format("where title = '{0}' ", txtMT.Text.Trim()));
                sqry.Append(String.Format("and code = '{0}' ", txtMC.Text.Trim()));
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
                    MessageBox.Show("Can't add this record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEnabled(false);
                    return;

                }
                else if (rowCount == 1)
                {
                    string movieschedid = String.Empty;
                    string moviescheslistid = String.Empty;
                    DateTime currentdate = datestart.Value;
                    do
                    {
                        if (currentdate == dateend.Value.Date)
                            movieschedid = "";

                        if (movieschedid == "")
                        {
                            TimeSpan timestartspan = timestart.Value.TimeOfDay;
                            TimeSpan timeendspan = timeend.Value.TimeOfDay;

                            ////added july 21 2014 
                            ////for the auto generate of sked per day with intermission time
                            DateTime movieenddate = currentdate.Date;
                            if (cbxintermision.Checked == true && txtintermision.Text.Trim() != "")
                            {
                                movieenddate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                                int showcntr = 0;
                                while (currentdate.ToShortDateString() == movieenddate.ToShortDateString())
                                {
                                    if (showcntr > 0)
                                    {
                                        TimeSpan movielengthspan = dtduration.Value.TimeOfDay;
                                        TimeSpan lengthintermission = movielengthspan.Add(TimeSpan.FromMinutes(Convert.ToDouble(txtintermision.Text)));
                                        timestartspan = timestartspan.Add(lengthintermission);
                                        timeendspan = timestartspan.Add(movielengthspan);
                                    }
                                    addsked(currentdate, cinemaid, movieid, movieschedid, moviescheslistid, timestartspan, timeendspan);

                                    if (timeendspan.TotalDays >= 1)
                                    {
                                        DateTime dtnew = currentdate.Add(timeendspan);
                                        movieenddate = dtnew;//new DateTime(datestart.Value.Year, datestart.Value.Month, datestart.Value.AddDays(timeendspan.TotalDays).Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                                    }
                                    else
                                        movieenddate = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                                    showcntr += 1;
                                }
                            }
                            else
                            {
                                //melvin 2014-10-28

                                //addsked(currentdate, cinemaid, movieid, movieschedid, moviescheslistid, timestartspan, timeendspan);]
                                if (!(addsked(currentdate, cinemaid, movieid, movieschedid, moviescheslistid, timestartspan, timeendspan)))
                                {
                                    return;
                                }
                            }

                        }
                        currentdate = currentdate.AddDays((double)1);
                        movieschedid = "";
                    }while(currentdate <= dateend.Value);

                    m_clscom.AddATrail(m_frmM.m_userid, "MOVIESKED_ADD", "MOVIES_SCHEDULE|MOVIES_SCHEDULE_LIST|MOVIES_SCHEDULE_LIST_PATRON",
                            Environment.MachineName.ToString(), "ADD NEW MOVIE SKED INFO: ID=" + movieschedid.ToString(), m_frmM._connection);

                    //update the moviestatus to active if new
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies where id = {0} and status = 0", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("update movies set status = 1 where id = {0}", movieid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    //update movies startdate and enddate
                    sqry = new StringBuilder();
                    sqry.Append("update movies a set a.start_date = (select min(b.movie_date) ");
                    sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                    sqry.Append(String.Format("where a.id = {0}", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn);
                    cmd4.ExecuteNonQuery();
                    cmd4.Dispose();

                    sqry = new StringBuilder();
                    sqry.Append("update movies a set a.end_date = (select max(b.movie_date) ");
                    sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                    sqry.Append(String.Format("where a.id = {0}", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd5 = new MySqlCommand(sqry.ToString(), myconn);
                    cmd5.ExecuteNonQuery();
                    cmd5.Dispose();

                    setnormal();
                    cmbCinema.SelectedValue = cinemaid;
                    dtcalview.Value = currentdate.AddDays(-1).Date;
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEnabled(false);
                }
            }
        }

        public bool addsked(DateTime currentdate, int cinemaid, int movieid, string movieschedid, string moviescheslistid, TimeSpan timestartspan, TimeSpan timeendspan)
        {
            StringBuilder sqry = new StringBuilder();
            sqry.Append("select count(*) from movies_schedule ");
            sqry.Append(String.Format("where movie_date = '{0}' ", currentdate.Date.ToString("yyyy-MM-dd")));
            sqry.Append(String.Format("and cinema_id = {0} ", cinemaid));
            sqry.Append(String.Format("and movie_id = {0}", movieid));
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
            int rowCount = Convert.ToInt32(cmd2.ExecuteScalar());
            cmd2.Dispose();

            if (rowCount > 0)
            {
                sqry = new StringBuilder();
                sqry.Append("select a.id from movies_schedule a ");
                sqry.Append(String.Format("where a.movie_date = '{0}' ", currentdate.Date.ToString("yyyy-MM-dd")));
                sqry.Append(String.Format("and a.cinema_id = {0} ", cinemaid));
                sqry.Append(String.Format("and a.movie_id = {0}", movieid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd3 = new MySqlCommand(sqry.ToString(), myconn);
                MySqlDataReader reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    movieschedid = reader["id"].ToString();
                }
                reader.Close();
                cmd3.Dispose();
            }

            if (movieschedid == "")
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("insert into movies_schedule value(0,{0},{1},'{2}')",
                    cinemaid, movieid, currentdate.Date.ToString("yyyy-MM-dd")));
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    movieschedid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString());
                    return false;
                }
            }

            
            try
            {
                //validate movies_schedule_list
                sqry = new StringBuilder();
                sqry.Append("select a.start_time, a.end_time, c.code, a.id from movies_schedule_list a ");
                sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                sqry.Append("left join movies c on b.movie_id = c.id ");
                sqry.Append(String.Format("where b.cinema_id = {0} ", cinemaid));
                sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", currentdate.Date)));
                sqry.Append("order by a.start_time desc");
                DataTable movieschedlist = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);

                if (timestartspan.Days < 1 && timeendspan.TotalDays < 1)
                {
                    DateTime curtimestart = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timestartspan.TotalHours, timestartspan.Minutes, 0);
                    DateTime curtimeend = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);

                    if (movieschedlist.Rows.Count > 0)
                    {
                        ////timestart.Value replaces by curtimestart(7/21/2014)
                        ////timeend.Value replaces by curtimeend(7/21/2014)
                        StringBuilder strqry = new StringBuilder();
                        //rmb REMARKED 11.3.2014 CALCULATE FOR ENDTIME
                        //strqry.Append(String.Format("and [start_time] <= '{0}' ", curtimestart));
                        //strqry.Append(String.Format("and [end_time] >= '{0}' ", curtimestart));
                        strqry.Append(String.Format("[id] > 0 ", curtimestart));
                        strqry.Append(String.Format("and [start_time] <= '{0}' ", curtimestart));
                        strqry.Append(String.Format("and [end_time] >= '{0}' ", curtimestart));
                        var foundRows = movieschedlist.Select(strqry.ToString());
                        if (foundRows.Count() > 0)
                        {
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            //setnormal();
                            MessageBox.Show("Please check your time schedule values.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        strqry = new StringBuilder();
                        strqry.Append(String.Format("[id] > 0 ", curtimestart));
                        //RMB remarked 11.3.2014
                        //strqry.Append(String.Format("and [start_time] >= '{0}' ", curtimeend));
                        //strqry.Append(String.Format("and [end_time] <= '{0}' ", curtimeend));
                        strqry.Append(String.Format("and [start_time] <= '{0}' ", curtimeend));
                        strqry.Append(String.Format("and [end_time] >= '{0}' ", curtimeend));
                        foundRows = movieschedlist.Select(strqry.ToString());
                        if (foundRows.Count() > 0)
                        {
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            setnormal();
                            MessageBox.Show("Please check your time schedule values.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    int intval = 0;
                    if (rbtnReserved.Checked == true)
                        intval = 1;
                    else if (this.rbtnGuarateed.Checked == true)
                        intval = 2;
                    else if (this.rbtnUnlimited.Checked == true)
                        intval = 3;

                    sqry = new StringBuilder();
                    sqry.Append(String.Format("insert into movies_schedule_list value(0,{0},'{1}','{2}',{3},0,1)", movieschedid, String.Format("{0: yyyy-MM-dd HH:mm:00}", curtimestart), String.Format("{0: yyyy-MM-dd HH:mm:00}", curtimeend), intval));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd1 = new MySqlCommand(sqry.ToString(), myconn);
                    cmd1.ExecuteNonQuery();
                    moviescheslistid = cmd1.LastInsertedId.ToString();
                    cmd1.Dispose();

                    if (moviescheslistid != "")
                    {
                        try
                        {
                            tabinsertcheck(myconn, dgvpatrons, Convert.ToInt32(moviescheslistid));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message, "tabinsertcheck");
                            return false;
                        }
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "addsked");
                return false;
            }
            return true;
        }


        //insert into movies_schedule_list_patron values(0,25336,27,5)	
        //Error Code: 1452. Cannot add or update a child row: a foreign key constraint fails 
        //    (`azynema2`.`movies_schedule_list_patron`, CONSTRAINT `fk_movies_schedule_list_patron_movies_schedule1` FOREIGN KEY
        //        (`movies_schedule_list_id`) REFERENCES `movies_schedule` (`id`) ON DELETE NO )	0.062 sec
        public void tabinsertcheck(MySqlConnection myconn, DataGridView dgv, int intid)
        {
            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv[0, i].Value != null)
                    {
                        if ((bool)dgv[0, i].Value)
                        {
                            StringBuilder sqry = new StringBuilder();
                            sqry.Append(String.Format("insert into movies_schedule_list_patron values(0,{0},{1},{2},0)",
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
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //public void refreshDGV()
        //{
        //    if(
        //    StringBuilder sbqry = new StringBuilder();
        //    sbqry.Append("SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as distributor, a.share_perc as share, d.status_desc as status ");
        //    sbqry.Append("FROM movies a, mtrcb b, distributor c, movies_status d WHERE a.rating_id = b.id and a.dist_id = c.id and a.status = d.status_id");
        //    m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
        //    setDataGridView(m_dt);
        //}

        private void datestart_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = datestart.Value;
            if (dt != null)
            {
                if (boolresultclick==false)
                {
                    dgvResult.DataSource = null;
                    dgvResult.Columns.Clear();
                    selectcalendardate(dt.Date);
                }
            }
            boolresultclick = false;

            //if (cntrol != "resultselect")
            //{
                DateTime date = Convert.ToDateTime(datestart.Value.Date.ToString());
                dateend.Value = date;
                if (dgvResult.RowCount == 0)
                    timestart.Value = dt.Add(new TimeSpan(10, 0, 0));//DateTime.Now; to add a new @ 10 am
                else
                    timestart.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.RowCount - 1].Cells[2].Value.ToString()).Add(new TimeSpan(0,m_frmM.m_clscom.m_clscon.MovieIntermissionTime,0));
                    ////added august 13 2014 for the adjustment of time for new sched
            //}
        }

        private void dateend_ValueChanged(object sender, EventArgs e)
        {
            DateTime datefrom = Convert.ToDateTime(datestart.Value.Date.ToString());
            DateTime dateto = Convert.ToDateTime(dateend.Value.Date.ToString());
            if (dateto < datefrom)
            {
              //  MessageBox.Show("Please check your date parameters.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dateend.Value = datefrom;
            }
        }

        private void timestart_ValueChanged(object sender, EventArgs e)
        {
            //int totaltime = calculatetime(timestart);
            //timeend.Value = timestart.Value.Date.AddMinutes(totaltime);
            setendtime();
        }

        public void setendtime()
        {
            try
            {
                TimeSpan timestartspan = timestart.Value.TimeOfDay;
                TimeSpan movielengthspan = dtduration.Value.TimeOfDay;
                TimeSpan timeendspan = timestartspan.Add(movielengthspan);
                
                if (timeendspan != null)
                {
                    string hours = ((int)timeendspan.TotalHours).ToString();
                    string minutes = String.Format("{0:00}", timeendspan.Minutes);
                    try
                    {
                        if (timeendspan.TotalDays >= 1)
                        {
                            DateTime dtnew = datestart.Value.Add(timeendspan);
                            timeend.Value = dtnew;//new DateTime(datestart.Value.Year, datestart.Value.Month, datestart.Value.AddDays(timeendspan.TotalDays).Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                        }
                        else
                            timeend.Value = new DateTime(datestart.Value.Year, datestart.Value.Month, datestart.Value.Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                    }
                    catch
                    {
                        timeend.Value = timestart.Value;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        private int calculatetime(DateTimePicker dttime)
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

        private void dgvMovies_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvMovies.Columns[e.ColumnIndex].Name == "duration")
                ShortFormatTimeFormat(e);
        }

        private static void ShortFormatTimeFormat(DataGridViewCellFormattingEventArgs formatting)
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

        private void dgvMovies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                txtMC.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                txtMT.Text = dgv.SelectedRows[0].Cells[2].Value.ToString();
                TimeSpan result = TimeSpan.FromMinutes(Convert.ToDouble(dgv.SelectedRows[0].Cells[3].Value.ToString()));
                string hours = ((int)result.TotalHours).ToString();
                string minutes = String.Format("{0:00}", result.Minutes);
                dtduration.Value = new DateTime(2014, 6, 2, (int)result.TotalHours, result.Minutes, 0);
                
                
            }
        }

 
        private void dtduration_ValueChanged(object sender, EventArgs e)
        { 
            //if(cntrol != "resultselect")
                setendtime();
        }


        private void calsked_MouseUp(object sender, MouseEventArgs e)
        {
            ICalendarSelectableElement hitted = calsked.HitTest(e.Location);
            CalendarDay hitdat = hitted as CalendarDay;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (hitdat != null)
                {
                    contextItem = calsked.ItemAt(contextMenuStrip1.Bounds.Location);
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (hitdat != null)
                {
                    DateTime seldate = hitdat.Date;

                    dgvResult.DataSource = null;
                    dgvResult.Columns.Clear();
                    selectcalendardate(seldate);

                    if (btnAdd.Text == "add")
                    {
                        dgvpatrons.DataSource = null;
                        dgvpatrons.Columns.Clear();
                    }

                    if (dgvResult.Rows.Count == 0 && btnAdd.Values.Text == "save")
                        datestart.Value = hitdat.Date;
                }
            }
        }

        public void selectcalendardate(DateTime dt)
        {
            string cinemaid = String.Empty;
            if (cmbCinema.Items.Count > 0)
            {
                var val = cmbCinema.SelectedValue;
                if (val.ToString() != "System.Data.DataRowView")
                    cinemaid = val.ToString();
                int intout = -1;
                if (int.TryParse(cinemaid, out intout))
                {
                    StringBuilder sqry = new StringBuilder();
                    sqry = new StringBuilder();
                    sqry.Append("select a.id, a.start_time, a.end_time, a.seat_type, b.id, c.id from movies_schedule_list a ");
                    sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                    sqry.Append("left join movies c on b.movie_id = c.id ");
                    sqry.Append(String.Format("where b.cinema_id = {0} ", intout));
                    sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt)));
                    sqry.Append("order by a.start_time asc");
                    DataTable m_dtlist = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
                    setDataGridViewII(dgvResult, m_dtlist);
                }
            }

            
        }

        public void setDataGridViewII(DataGridView dgv, DataTable dt)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dgv.DataSource = dt;
                int iwidth = dgv.Width / 3;
                dgv.Columns[0].Width = 0;
                dgv.Columns[0].HeaderText = "ID";
                dgv.Columns[1].Width = iwidth - 15;
                dgv.Columns[1].HeaderText = "Start Time";
                dgv.Columns[2].Width = iwidth - 15;
                dgv.Columns[2].HeaderText = "End Time";
                dgv.Columns[3].Width = iwidth * 2;
                dgv.Columns[3].HeaderText = "Seat Type";
                dgv.Columns[4].Width = 0;
                dgv.Columns[4].HeaderText = "Movie Sked ID";
                dgv.Columns[5].Width = 0;
                dgv.Columns[5].HeaderText = "Movie ID";
            }
        }
        public void setDataGridViewIII(DataTable dt, DataGridView dgv)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                //dgv.DataSource = dt;
                int iwidth = dgv.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = iwidth * 2;
                dgv.Columns[0].HeaderText = "Patron Type";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Width = iwidth / 2 ;
                dgv.Columns[1].HeaderText = "Price";
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "ID";
                dgv.Columns.Insert(0, cbx);
            }
        }


        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //cntrol = "resultselect";
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                setSelected(dgv);
                string id = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[0].Value.ToString();

                StringBuilder sqry = new StringBuilder();
                sqry.Append("select status from movies_schedule_list where id=" + id);
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                if (res == 0)
                {
                    rbUnpublish.Checked = true;
                    
                }
                else
                {
                    rbPublish.Checked = true;
                }
            }

            
        }

        public void setSelected(KryptonDataGridView dgv)
        {
            StringBuilder sbqry = new StringBuilder();
            if (dgv.SelectedRows.Count == 1)
            {
                int movieid = Convert.ToInt32(dgv.SelectedRows[0].Cells[5].Value.ToString());
                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("[id] = {0}", movieid));

                if (m_dtmovies.Rows.Count == 0)
                {
                    sbqry = new StringBuilder();
                    sbqry.Append("select a.id, a.code, a.title, a.duration ");
                    sbqry.Append("from movies a ");
                    sbqry.Append("order by a.title asc");
                    m_dtmovies = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                }

                if (m_dtmovies.Rows.Count > 0)
                {
                    var foundRows = m_dtmovies.Select(sqry.ToString());
                    if (foundRows.Count() > 0)
                    {
                        DataTable dt = foundRows.CopyToDataTable();
                        txtMT.Text = dt.Rows[0]["title"].ToString();
                        txtMC.Text = dt.Rows[0]["code"].ToString();
                        TimeSpan result = TimeSpan.FromMinutes(Convert.ToDouble(dt.Rows[0]["duration"].ToString()));
                        string hours = ((int)result.TotalHours).ToString();
                        string minutes = String.Format("{0:00}", result.Minutes);
                        dtduration.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (int)result.TotalHours, result.Minutes, 0);
                    }
                }

                if (btnAdd.Text == "new")
                {
                    boolresultclick = true;
                    datestart.Value = Convert.ToDateTime(dgv.SelectedRows[0].Cells[1].Value.ToString());
                    dateend.Value = Convert.ToDateTime(dgv.SelectedRows[0].Cells[2].Value.ToString());

                    if (dgv.SelectedRows[0].Cells[3].Value.ToString() == "1")
                        rbtnReserved.Checked = true;
                    else if (dgv.SelectedRows[0].Cells[3].Value.ToString() == "2")
                        this.rbtnGuarateed.Checked = true;
                    else if (dgv.SelectedRows[0].Cells[3].Value.ToString() == "3")
                        this.rbtnUnlimited.Checked = true;

                    timestart.Value = Convert.ToDateTime(dgv.SelectedRows[0].Cells[1].Value.ToString());
                    timeend.Value = Convert.ToDateTime(dgv.SelectedRows[0].Cells[2].Value.ToString());

                    dgvpatrons.DataSource = null;
                    dgvpatrons.Columns.Clear();
                    sbqry = new StringBuilder();
                    sbqry.Append("select b.name, a.price, a.patron_id as id ");
                    sbqry.Append("from movies_schedule_list_patron a ");
                    sbqry.Append("left join patrons b on a.patron_id = b.id ");
                    sbqry.Append(String.Format("where a.movies_schedule_list_id = {0}", dgv.SelectedRows[0].Cells[0].Value.ToString()));
                    m_dtpatrons = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                    setDataGridViewIII(m_dtpatrons, dgvpatrons);

                    if (dgvpatrons.Rows.Count > 0)
                    {
                        tabModule.SelectedPage = pagePatrons;
                        setCheck(dgvpatrons, true);
                    }
                }

                btnEdit.Enabled = true;
                btnEdit.Text = "edit";

                btnDelete.Enabled = true;
                btnDelete.Text = "remove";
            }
        }

        private static void ShortTimeFormat(DataGridViewCellFormattingEventArgs formatting)
        {
            if (formatting.Value != null)
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(formatting.Value.ToString());
                    string fromTimeString = dt.ToShortTimeString();
                    formatting.Value = fromTimeString;
                    formatting.FormattingApplied = true;
                }
                catch
                {
                    formatting.FormattingApplied = false;
                }
            }
        }

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvResult.Columns[e.ColumnIndex].Name == "seat_type")
                ColumnItemFormat(e);
            else if (dgvResult.Columns[e.ColumnIndex].Name == "start_time")
                ShortTimeFormat(e);
            else if (dgvResult.Columns[e.ColumnIndex].Name == "end_time")
                ShortTimeFormat(e);
        }

        public void ColumnItemFormat(DataGridViewCellFormattingEventArgs formatting)
        {
            string sval = String.Empty;
            if (formatting.Value != null)
            {
                try
                {
                    if (formatting.Value.ToString() == "1")
                        sval = "Reserved Seating";
                    else if(formatting.Value.ToString() == "2")
                        sval = "Free Seating (Guaranteed)";
                    else if (formatting.Value.ToString() == "3")
                        sval = "Free Seating (Unlimited)";
                 
                    formatting.Value = sval;
                    formatting.FormattingApplied = true;
                }
                catch
                {
                    formatting.FormattingApplied = false;
                }
            }
        }

        private void dgvResult_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResult.SelectedRows.Count == 1)
            {
                if(boolresultclick)
                    setSelected(dgvResult);
            }
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

        private void txtMC_Enter(object sender, EventArgs e)
        {
            tabModule.SelectedPage = pageMovies;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intidx = cmbCinema.SelectedIndex;

            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIESKED_DELETE", m_frmM._connection);
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
                    int movieid = -1;
                    if (dgvResult.SelectedRows.Count == 1)
                    {
                        intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());
                        movieid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[5].Value.ToString());
                    }

                    //validate the database for records being used
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("select count(*) from movies_schedule_list where id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();


                    if (rowCount > 0)
                    {

                        //delete from the moview table where the status is inactive or = 0
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from movies_schedule_list where id = {0}", intid));
                        try
                        {
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            sqry = new StringBuilder();
                            sqry.Append(String.Format("select count(*) from movies_schedule_list_patron where movies_schedule_list_id = {0}", intid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd2.ExecuteScalar());
                            cmd2.Dispose();

                            if (rowCount > 0)
                            {
                                sqry = new StringBuilder();
                                sqry.Append(String.Format("delete from movies_schedule_list where id = {0}", intid));
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                cmd = new MySqlCommand(sqry.ToString(), myconn);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }

                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            //update the moviestatus to new if all records will be deleted
                            sqry = new StringBuilder();
                            sqry.Append(String.Format("select count(*) from movies_distributor where movie_id = {0}", movieid));
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
                                sqry.Append(String.Format("select count(*) from movies_schedule where movie_id = {0}", movieid));
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
                                    sqry.Append(String.Format("update movies set status = 0 where id = {0}", movieid));
                                    if (myconn.State == ConnectionState.Closed)
                                        myconn.Open();
                                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                                    cmd.ExecuteNonQuery();
                                    cmd.Dispose();
                                }
                            }

                            //update movies startdate and enddate
                            sqry = new StringBuilder();
                            sqry.Append("update movies a set a.start_date = (select min(b.movie_date) ");
                            sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                            sqry.Append(String.Format("where a.id = {0}", movieid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn);
                            cmd4.ExecuteNonQuery();
                            cmd4.Dispose();

                            sqry = new StringBuilder();
                            sqry.Append("update movies a set a.end_date = (select max(b.movie_date) ");
                            sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                            sqry.Append(String.Format("where a.id = {0}", movieid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd5 = new MySqlCommand(sqry.ToString(), myconn);
                            cmd5.ExecuteNonQuery();
                            cmd5.Dispose();

                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            m_clscom.AddATrail(m_frmM.m_userid, "MOVIESKED_REMOVE", "MOVIES_SCHEDULE|MOVIES_SCHEDULE_LIST|MOVIES_SCHEDULE_LIST_PATRON",
                            Environment.MachineName.ToString(), "REMOVE MOVIE SKED INFO: ID=" + intid.ToString(), m_frmM._connection);

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
            else
            {
                txtEnabled(false);
            }
            setnormal();
            dtcalview.Value = datestart.Value;
            cmbCinema.SelectedIndex = intidx;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "MOVIESKED_EDIT", m_frmM._connection);
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
            //else if (dgvResult.SelectedRows.Count == 1)
            //    dgvResult.Enabled = false;

            if (btnEdit.Text == "edit")
            {
                grpgrant.Visible = true;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                dgvResult.Enabled = false;
                calsked.Enabled = false;
                dateend.Enabled = false;

                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id, a.code, a.title,a.duration ");
                sbqry.Append("from movies a ");
                sbqry.Append("order by a.title asc");
                m_dtmovies = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
                setDataGridView(dgvMovies, m_dtmovies);

                string cinemaid = String.Empty;
                var val = cmbCinema.SelectedValue;
                if (val.ToString() != "System.Data.DataRowView")
                    cinemaid = val.ToString();
                int intout = -1;
                if (int.TryParse(cinemaid, out intout))
                {
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select b.name, a.price, a.patron_id as id ");
                    sqry.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                    sqry.Append(String.Format("where a.cinema_id = {0} ", intout));
                    sqry.Append("and b.name is not null ");
                    sqry.Append("and a.patron_id not in (");
                    sqry.Append("select c.patron_id ");
                    sqry.Append("from movies_schedule_list_patron c ");
                    sqry.Append("left join patrons d on c.patron_id = d.id ");
                    sqry.Append(String.Format("where c.movies_schedule_list_id = {0}", dgvResult.SelectedRows[0].Cells[0].Value.ToString()));
                    sqry.Append(") order by a.id asc");
                    DataTable dtpatronsaddnl = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
                    DataTable dtpatrons = new DataTable();
                    /// RMB added if the datatsource is empty 10.31.2014
                    if (dgvpatrons.DataSource != null)
                    {
                        dtpatrons = (DataTable)dgvpatrons.DataSource;
                        dtpatrons.Merge(dtpatronsaddnl);
                        dgvpatrons.DataSource = dtpatrons;
                    }
                    else
                    {                    
                        dgvpatrons.DataSource = null;
                        dgvpatrons.Columns.Clear();
                        StringBuilder sbqry1 = new StringBuilder();
                        sbqry1.Append("select b.name, a.price, a.patron_id as id ");
                        sbqry1.Append("from cinema_patron a left join patrons b on a.patron_id = b.id ");
                        sbqry1.Append(String.Format("where a.cinema_id = {0} ", cinemaid));
                        sbqry1.Append("and b.name is not null ");
                        sbqry1.Append("order by a.id asc");
                        m_dtpatrons = m_clscom.setDataTable(sbqry1.ToString(), m_frmM._connection);
                        setDataGridViewIII(m_dtpatrons, dgvpatrons);
                    }

                    setCheck(dgvpatrons, false);
                    int rowCount = 0;
                    StringBuilder sqry2 = new StringBuilder();
                    for (int i = 0; i < dgvpatrons.Rows.Count; i++)
                    {
                        int intmid = 0;
                        intout = -1;
                        if (int.TryParse(dgvpatrons[3, i].Value.ToString(), out intout))
                            intmid = intout;

                        if (intmid > 0)
                        {
                            sqry2 = new StringBuilder();
                            sqry2.Append(String.Format("select count(*) from movies_schedule_list_patron a where a.movies_schedule_list_id = {0}", dgvResult.SelectedRows[0].Cells[0].Value.ToString()));
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
                btnsearch.PerformClick();
                txtEnabled(true);

            }
            else if (btnEdit.Text == "update")
            {
                

                string strstatus = String.Empty;
                if (txtMC.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtMC.SelectAll();
                    txtMC.Focus();
                    return;
                }
                if (txtMT.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtMT.SelectAll();
                    txtMT.Focus();
                    return;
                }

                if (datestart.Text == "" || datestart.Value == null)
                {
                    MessageBox.Show("Please fill the start date information.");
                    datestart.Focus();
                    return;
                }

                if (dateend.Text == "" || dateend.Value == null)
                {
                    MessageBox.Show("Please fill the end date information.");
                    dateend.Focus();
                    return;
                }

                if (timestart.Text == "" || timestart.Value == null)
                {
                    MessageBox.Show("Please fill the start time information.");
                    timestart.Focus();
                    return;
                }

                if (timeend.Text == "" || timeend.Value == null)
                {
                    MessageBox.Show("Please fill the end time information.");
                    timeend.Focus();
                    return;
                }
                int inttype = -1;
                if (rbtnReserved.Checked == true)
                    inttype = 1;
                else if (this.rbtnGuarateed.Checked == true)
                    inttype = 2;
                else if (rbtnUnlimited.Checked == true)
                    inttype = 3;

                if (inttype == -1)
                {
                    MessageBox.Show("Please select the seating information.");
                    return;
                }

                int movieid = 0;
                int cinemaid = 0;
                if (dgvMovies.SelectedRows.Count == 1)
                {
                    movieid = Convert.ToInt32(dgvMovies.SelectedRows[0].Cells[0].Value.ToString());
                    cinemaid = Convert.ToInt32(cmbCinema.SelectedValue.ToString());
                }

                    
                else if (dgvMovies.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a movie.");
                    dgvMovies.Focus();
                    return;
                }
                //MessageBox.Show(movieid.ToString() + "cine" + cinemaid.ToString());
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
                    MessageBox.Show("Please check the patrons pricing \n\r for the selected movies.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvpatrons.Focus();
                    return;
                }


                chkcnt = 0;
                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies ");
                sqry.Append(String.Format("where title = '{0}' ", txtMT.Text.Trim()));
                sqry.Append(String.Format("and code = '{0}' ", txtMC.Text.Trim()));
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
                    MessageBox.Show("Can't add this record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (rowCount == 1)
                {
                    string movieschedid = String.Empty;
                    string moviescheslistid = String.Empty;
                    DateTime currentdate = datestart.Value;//timestart.Value;
                    do
                    {
                        if (currentdate == dateend.Value.Date)
                            movieschedid = "";

                        if (movieschedid == "")
                        {
                            sqry = new StringBuilder();
                            sqry.Append("select count(*) from movies_schedule ");
                            sqry.Append(String.Format("where movie_date = '{0}' ", currentdate.Date.ToString("yyyy-MM-dd")));
                            sqry.Append(String.Format("and cinema_id = {0} ", cinemaid));
                            sqry.Append(String.Format("and movie_id = {0}", movieid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
                            rowCount = Convert.ToInt32(cmd2.ExecuteScalar());
                            cmd2.Dispose();

                            if (rowCount > 0)
                            {
                                sqry = new StringBuilder();
                                sqry.Append("select a.id from movies_schedule a ");
                                sqry.Append(String.Format("where a.movie_date = '{0}' ", currentdate.Date.ToString("yyyy-MM-dd")));
                                sqry.Append(String.Format("and a.cinema_id = {0} ", cinemaid));
                                sqry.Append(String.Format("and a.movie_id = {0}", movieid));
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                MySqlCommand cmd3 = new MySqlCommand(sqry.ToString(), myconn);
                                MySqlDataReader reader = cmd3.ExecuteReader();
                                while (reader.Read())
                                {
                                    movieschedid = reader["id"].ToString();
                                }
                                reader.Close();
                                cmd3.Dispose();
                            }

                            if (movieschedid == "")
                            {
                                sqry = new StringBuilder();
                                sqry.Append(String.Format("insert into movies_schedule value(0,{0},{1},'{2}')",
                                    cinemaid, movieid, currentdate.Date.ToString("yyyy-MM-dd")));
                                try
                                {
                                    if (myconn.State == ConnectionState.Closed)
                                        myconn.Open();
                                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                                    cmd.ExecuteNonQuery();

                                    movieschedid = cmd.LastInsertedId.ToString();
                                    cmd.Dispose();
                                }
                                catch
                                {
                                }
                            }

                            try
                            {
                                //validate movies_schedule_list
                                sqry = new StringBuilder();
                                sqry.Append("select a.start_time, a.end_time, c.code, a.id from movies_schedule_list a ");
                                sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                                sqry.Append("left join movies c on b.movie_id = c.id ");
                                sqry.Append(String.Format("where b.cinema_id = {0} ", cinemaid));
                                sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", currentdate.Date)));
                                sqry.Append("order by a.start_time desc");
                                DataTable movieschedlist = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);

                                if (movieschedlist.Rows.Count > 0)
                                {
                                    StringBuilder strqry = new StringBuilder();
                                    strqry.Append(String.Format("[id] > 0 ", timestart.Value));
                                    //RMB remarked 11.3.2014 checked for the current time is already avalilable
                                    //strqry.Append(String.Format("and [start_time] <= '{0}' ", timestart.Value));
                                    //strqry.Append(String.Format("and [end_time] >= '{0}' ", timestart.Value));
                                    strqry.Append(String.Format("and [start_time] <= '{0}' ", timestart.Value));
                                    strqry.Append(String.Format("and [end_time] >= '{0}' ", timestart.Value));
                                    var foundRows = movieschedlist.Select(strqry.ToString());
                                    if (foundRows.Count() > 0)
                                    {
                                        if (foundRows.CopyToDataTable().Rows[0]["id"].ToString() != dgvResult.SelectedRows[0].Cells[0].Value.ToString())
                                        {
                                            //rmb REMARKED 10.31.2014
                                        //    continue;//break;//
                                        //else
                                        //{
                                            if (myconn.State == ConnectionState.Open)
                                                myconn.Close();
                                            MessageBox.Show("Please check your time schedule values.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }

                                    strqry = new StringBuilder();
                                    strqry.Append(String.Format("[id] > 0 ", timestart.Value));
                                    //remarked 11.3.2014 validate sched
                                    //strqry.Append(String.Format("and [start_time] >= '{0}' ", timeend.Value));
                                    //strqry.Append(String.Format("and [end_time] <= '{0}' ", timeend.Value));
                                    strqry.Append(String.Format("and [start_time] <= '{0}' ", timeend.Value));
                                    strqry.Append(String.Format("and [end_time] >= '{0}' ", timeend.Value));
                                    foundRows = movieschedlist.Select(strqry.ToString());
                                    if (foundRows.Count() > 0)
                                    {
                                        //rmb COMMENT 10.31.2014
                                        //if (foundRows.CopyToDataTable().Rows[0]["id"].ToString() == dgvResult.SelectedRows[0].Cells[0].Value.ToString())
                                        //    continue;
                                        //else
                                        if (foundRows.CopyToDataTable().Rows[0]["id"].ToString() != dgvResult.SelectedRows[0].Cells[0].Value.ToString())
                                        {
                                            if (myconn.State == ConnectionState.Open)
                                                myconn.Close();
                                            //setnormal();
                                            MessageBox.Show("Please check your time schedule values.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }
                                }

                                int intval = 0;
                                if(rbtnReserved.Checked == true)
                                    intval = 1;
                                else if(this.rbtnGuarateed.Checked == true)
                                    intval = 2;
                                else if(this.rbtnUnlimited.Checked == true)
                                    intval = 3;

                                
                                TimeSpan timestartspan = timestart.Value.TimeOfDay;
                                TimeSpan timeendspan = timeend.Value.TimeOfDay;
                                DateTime curtimestart = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timestartspan.TotalHours, timestartspan.Minutes, 0);
                                DateTime curtimeend = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, (int)timeendspan.TotalHours, timeendspan.Minutes, 0);
                                sqry = new StringBuilder();
                                sqry.Append(String.Format("update movies_schedule_list set movies_schedule_id = {0},",movieschedid));
                                sqry.Append(String.Format("start_time = '{0}',",String.Format("{0: yyyy-MM-dd HH:mm:00}",curtimestart)));
                                sqry.Append(String.Format("end_time = '{0}',", String.Format("{0: yyyy-MM-dd HH:mm:00}",curtimeend)));
                                sqry.Append(String.Format("seat_type = {0} ",intval));
                                sqry.Append(String.Format("where id = {0}", dgvResult.SelectedRows[0].Cells[0].Value.ToString()));
                                if (myconn.State == ConnectionState.Closed)
                                    myconn.Open();
                                cmd = new MySqlCommand(sqry.ToString(), myconn);
                                cmd.ExecuteNonQuery();
                                //rmb REMARKED 10.31.2014 //moviescheslistid = cmd.LastInsertedId.ToString();
                                moviescheslistid = dgvResult.SelectedRows[0].Cells[0].Value.ToString();
                                cmd.Dispose();

                                if (moviescheslistid != "")
                                {
                                    try
                                    {
                                        //validate for the existance of the record
                                        sqry = new StringBuilder();
                                        sqry.Append(String.Format("select count(*) from movies_schedule_list_patron a where a.movies_schedule_list_id = {0}", dgvResult.SelectedRows[0].Cells[0].Value.ToString()));
                                        if (myconn.State == ConnectionState.Closed)
                                            myconn.Open();
                                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                                        rowCount = 0;
                                        rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                                        cmd.Dispose();

                                        if (rowCount > 0)
                                        {
                                            sqry = new StringBuilder();
                                            sqry.Append("delete from movies_schedule_list_patron ");
                                            sqry.Append(String.Format("where movies_schedule_list_id = {0} ", dgvResult.SelectedRows[0].Cells[0].Value.ToString()));
                                            if (myconn.State == ConnectionState.Closed)
                                                myconn.Open();
                                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                                            cmd.ExecuteNonQuery();
                                        }

                                        tabinsertcheck(myconn, dgvpatrons, Convert.ToInt32(moviescheslistid));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        currentdate = currentdate.AddDays((double)1);
                        movieschedid = "";
                    }while(currentdate <= dateend.Value);

                    //update movies startdate and enddate
                    sqry = new StringBuilder();
                    sqry.Append("update movies a set a.start_date = (select min(b.movie_date) ");
                    sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                    sqry.Append(String.Format("where a.id = {0}", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn);
                    cmd4.ExecuteNonQuery();
                    cmd4.Dispose();

                    sqry = new StringBuilder();
                    sqry.Append("update movies a set a.end_date = (select max(b.movie_date) ");
                    sqry.Append(String.Format("from movies_schedule b where b.movie_id = {0}) ", movieid));
                    sqry.Append(String.Format("where a.id = {0}", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd5 = new MySqlCommand(sqry.ToString(), myconn);
                    cmd5.ExecuteNonQuery();
                    cmd5.Dispose();

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "MOVIESKED_EDIT", "MOVIES_SCHEDULE|MOVIES_SCHEDULE_LIST|MOVIES_SCHEDULE_LIST_PATRON",
                            Environment.MachineName.ToString(), "UPDATED MOVIE SKED INFO: ID=" + movieschedid.ToString(), m_frmM._connection);

                    if (rbPublish.Checked == true)
                        publish("1");
                    else if (rbUnpublish.Checked == true)
                        publish("0");

                    setnormal();
                    txtEnabled(false);
                    cmbCinema.SelectedValue = cinemaid;
                    dtcalview.Value = currentdate.AddDays(-1).Date;
                    MessageBox.Show("You have successfully updated the selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void removeMovieSchedulesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calsked.ItemAt(contextMenuStrip1.Bounds.Location);
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            unselectbutton();
            txtEnabled(true);
            DataTable dt = new DataTable();
            tabModule.SelectedPage = pageMovies;

            StringBuilder sqry = new StringBuilder();
            sqry.Append("[id] > -1");
            if (txtMC.Text.Trim() != "")
                sqry.Append(String.Format(" and [code] like '%{0}%'", txtMC.Text.Trim()));
            if (txtMT.Text.Trim() != "")
                sqry.Append(String.Format(" and [title] like '%{0}%'", txtMT.Text.Trim()));

            if (m_dtmovies.Rows.Count == 0)
            {
                StringBuilder sbqry = new StringBuilder();
                sbqry.Append("select a.id, a.code, a.title,a.duration ");
                sbqry.Append("from movies a ");
                sbqry.Append("order by a.title asc");
                m_dtmovies = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            }

            if (m_dtmovies.Rows.Count > 0)
            {
                var foundRows = m_dtmovies.Select(sqry.ToString());
                if (foundRows.Count() == 0)
                {
                    setDataGridView(this.dgvMovies, m_dtmovies);
                    MessageBox.Show("No records found using the given information.", "Search Movies", MessageBoxButtons.OK);
                    return;
                }
                else
                    dt = foundRows.CopyToDataTable();
            }

            if (dt.Rows.Count > 0)
            {
                setDataGridView(dgvMovies, dt);
            }
        }

        private void dtcalview_ValueChanged(object sender, EventArgs e)
        {
            calview.ViewStart = dtcalview.Value;
            calview.SelectWeek(dtcalview.Value);
            calview.Refresh();
            calview.Invalidate();
        }

        private void btnSked_Click(object sender, EventArgs e)
        {
            frmSked frmskd = new frmSked();
            frmskd.frmInit(m_frmM, m_clscom);
            frmskd.ShowDialog();
            frmskd.Dispose();
        }

        private void calsked_RegionChanged(object sender, EventArgs e)
        {
            MessageBox.Show(((System.Windows.Forms.Calendar.Calendar)sender).ToString());
        }

        private void calsked_ItemMouseHover(object sender, CalendarItemEventArgs e)
        {

        }

        private void txtmin_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cbxintermision_CheckedChanged(object sender, EventArgs e)
        {
            txtintermision.Text = m_frmM.m_clscom.m_clscon.MovieIntermissionTime.ToString();
            if (cbxintermision.Checked == true)
            {
                txtintermision.SelectAll();
                txtintermision.ReadOnly = false;
                txtintermision.Focus();
            }
            else
                txtintermision.ReadOnly = true;
        }

        private void txtintermision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void dgvMovies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtintermision.Text = null;
            txtMC.Text = null;
            txtMT.Text = null;
            cbxintermision.Checked = false;
            dtduration.Enabled = false;
            txtEnabled(false);
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            //melvin 10-28-2014
           
        }

        private void btnUnPublish_Click(object sender, EventArgs e)
        {
            //melvin 10-28-2014
           
        }

        private void publish(string stat)
        {
            //melvin 10-27-2014
            if (dgvResult.SelectedCells.Count > 0)
            {
                string id = dgvResult.SelectedRows[0].Cells[0].Value.ToString();
               // MessageBox.Show(id);
                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("update movies_schedule_list set status={0}  ", stat));
                sqry.Append(String.Format("where id={0} ", id));
                //MessageBox.Show(sqry.ToString());
                //validate if connection is open
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                MessageBox.Show("Select Movie First", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtintermision_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rbPublish_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void rbUnpublish_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
           
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //melvin 10-30-2014 add refresh
            try
            {
                if (cmbCinema.Items.Count > 0)
                {
                    var val = cmbCinema.SelectedValue;
                    if (val.ToString() != "System.Data.DataRowView" && val.ToString() != "" && val != null)
                    {
                        foreach (CalendarItem item in calsked.GetSelectedItems())
                        {

                        }

                        calsked.SetViewRange(calview.SelectionStart, calview.SelectionEnd);

                        CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                        DateTime dtstart = GetFirstDayOfWeek(calview.SelectionStart, defaultCultureInfo);
                        addScreeningSched(cmbCinema.SelectedValue.ToString(), dtstart);

                        clearCalendarItem();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void cmsRefresh_Opening(object sender, CancelEventArgs e)
        {

        }

        private void kryptonGroup1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

   
    }
}
