using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Windows.Forms.Calendar;
using System.Threading;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmSked : Form
    {
        clscommon m_clscom = null;
        frmMain m_frmM = null;
        ComponentFactory.Krypton.Toolkit.KryptonLabel label = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
        DataTable m_dtcinema = new DataTable();

        ContextMenuCalendarItem m_popedContainer;
        PoperContainer m_poperContainer;

        DateTime hitDate = new DateTime();// 1.4.2016
        int hitCalendar = -1;//1.4.2016
        StringBuilder copyQry = new StringBuilder(); // 1.4.2016
        MySqlConnection myconn = new MySqlConnection(); //1.14.2016
        Boolean copyAll = false; // 2.18.2016
        DateTime copyAllDate = new DateTime();//2.18.2016

        public frmSked()
        {
            InitializeComponent();

            m_popedContainer = new ContextMenuCalendarItem();
            m_poperContainer = new PoperContainer(m_popedContainer);
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_clscom = cls;
            m_frmM = frm;
            populatecinema();
            unselectbutton();
        }

        public void populatecinema()
        {
            m_dtcinema = m_clscom.setDataTable("select * from cinema order by in_order asc", m_frmM._connection);
            if (m_dtcinema.Rows.Count > 0)
            {
                DataView dv = m_dtcinema.AsDataView();
                dv.Sort = "in_order asc";
                m_dtcinema = dv.ToTable();
            }
        }

        private void frmSked_Load(object sender, EventArgs e)
        {
            int intheight = 207;
            int intlocy = 4;
            System.Windows.Forms.Calendar.Calendar calendar = new System.Windows.Forms.Calendar.Calendar();
            for (int i = 0; i < m_dtcinema.Rows.Count; i++)
            {
                string cinemaname = m_dtcinema.Rows[i]["name"].ToString();
                int cinemaid = Convert.ToInt32(m_dtcinema.Rows[i]["id"].ToString());

                calendar = new System.Windows.Forms.Calendar.Calendar();
                calendar.AllowItemEdit = false;
                calendar.AllowItemResize = false;
                calendar.AllowNew = false;
                calendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
                calendar.Font = new System.Drawing.Font("Segoe UI", 9F);
                calendar.HighlightRanges = new System.Windows.Forms.Calendar.CalendarHighlightRange[0];
                calendar.MaximumFullDays = 6;
                calendar.MaximumViewDays = 7;
                calendar.Name = cinemaid.ToString();//cinemaname.Replace(" ","");
                calendar.Size = new System.Drawing.Size(838, intheight);
                calendar.TabIndex = intheight + i;
                calendar.Text = cinemaname.Replace(" ", "");//Convert.ToString(cinemaid);//
                calendar.ItemClick += new System.Windows.Forms.Calendar.Calendar.CalendarItemEventHandler(calendar_ItemClick);
                calendar.MouseUp += new System.Windows.Forms.MouseEventHandler(calendar_MouseUp);
                calendar.ItemMouseHover += new System.Windows.Forms.Calendar.Calendar.CalendarItemEventHandler(calendar_ItemMouseHover);

                if (i == 0)//170, 166
                    calendar.Location = new System.Drawing.Point(170, intlocy);
                else if(i ==1)
                    calendar.Location = new System.Drawing.Point(170, intheight + (intlocy * 2));
                else
                    calendar.Location = new System.Drawing.Point(170, ((intheight + intlocy) * i ) + 4);

                this.grpfilter.Panel.Controls.Add(calendar);

                //CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                //DateTime dtstart = GetFirstDayOfWeek(calview.SelectionStart, defaultCultureInfo);
                //addScreeningSched(calendar, cinemaid, dtstart);

                label = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
                label.Location = new System.Drawing.Point(15, (calendar.Location.Y - 8) + (calendar.Height / 2));
                label.Name = cinemaname;
                label.Size = new System.Drawing.Size(143, 29);
                label.StateCommon.ShortText.Color1 = System.Drawing.Color.Brown;
                label.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
                label.StateCommon.ShortText.ColorAngle = 90F;
                label.StateCommon.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear33;
                label.StateCommon.ShortText.Font = new System.Drawing.Font("Baskerville Old Face", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label.TabIndex = intlocy + 1;
                label.Values.Text = cinemaname;

                this.grpfilter.Panel.Controls.Add(label);
            }

            ////for the anchoring of the calendar august 13 2014
            List<System.Windows.Forms.Calendar.Calendar> calList3 = this.grpfilter.Panel.Controls.OfType<System.Windows.Forms.Calendar.Calendar>().ToList();
            for (int ii = 0; ii < calList3.Count; ii++)
            {
                calList3[ii].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right))); 
            }

            dtcalview.Value = DateTime.Now.Date;
            calview.Invalidate();
            calview.Refresh();
        }

        private void calendar_ItemMouseHover(object sender, CalendarItemEventArgs e)
        {
            //m_poperContainer.Hide();
            //if (((System.Windows.Forms.Calendar.Calendar)sender).GetSelectedItems().Count() > 0)
            //{
                //setCalendarItem((System.Windows.Forms.Calendar.Calendar)sender);
                //m_poperContainer.Show((System.Windows.Forms.Calendar.Calendar)sender);
            //}
        }

        private void calendar_ItemClick(object sender, CalendarItemEventArgs e)
        {
            if (((System.Windows.Forms.Calendar.Calendar)sender).GetSelectedItems().Count() > 0)
                setCalendarItem((System.Windows.Forms.Calendar.Calendar)sender);
        }

        public void setCalendarItem(System.Windows.Forms.Calendar.Calendar cal)
        {
            foreach (CalendarItem item in cal.GetSelectedItems())
            {
                try
                {
                    DateTime dt = item.Date;
                    DateTime itemTime = Convert.ToDateTime(item.Text); //DateTime.ParseExact(item.Text, "HH:mm tt", CultureInfo.InvariantCulture);
                    DateTime dtstarttime = dt.Add(itemTime.TimeOfDay);

                    if (dtstarttime != null && dt != null)
                    {
                        StringBuilder sqry = new StringBuilder();
                        sqry = new StringBuilder();
                        sqry.Append("select a.id, a.start_time, a.end_time, a.seat_type, b.id, c.id, c.title, c.duration, d.name as cinemaname from movies_schedule_list a ");
                        sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                        sqry.Append("left join movies c on b.movie_id = c.id ");
                        sqry.Append("left join cinema d on b.cinema_id = d.id ");
                        sqry.Append(String.Format("where b.cinema_id = {0} ", cal.Name));//((System.Windows.Forms.Calendar.Calendar)sender).Name));
                        sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt)));
                        sqry.Append(String.Format("and a.start_time = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", dtstarttime)));
                        sqry.Append("order by a.start_time asc");
                        DataTable dtselect = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);

                        if (dtselect.Rows.Count > 0)
                        {
                            m_popedContainer.dgvResult.DataSource = null;
                            m_popedContainer.dgvResult.Columns.Clear();
                            sqry = new StringBuilder();
                            sqry.Append("select b.name, a.price, a.patron_id, a.is_default ");
                            sqry.Append("from movies_schedule_list_patron a ");
                            sqry.Append("left join patrons b on a.patron_id = b.id ");
                            sqry.Append(String.Format("where a.movies_schedule_list_id = {0}", dtselect.Rows[0]["id"].ToString()));
                            DataTable dtpatrons = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
                            if(dtpatrons.Rows.Count > 0)
                                setDataGridViewIII(dtpatrons, m_popedContainer.dgvResult);
                            //RMB added 11.14.2014
                            m_popedContainer.dgvResult.ClearSelection();
                            m_popedContainer.dgvResult.CurrentCell = null;
                            //m_popedContainer.dgvResult.CurrentCell.Selected = false;

                            for (int i = 0; i < m_popedContainer.dgvResult.Rows.Count; i++)
                            {
                                if (m_popedContainer.dgvResult.Rows[i].Cells[3].Value.ToString() == "1")
                                    m_popedContainer.dgvResult.Rows[i].Selected = true;
                            }

                            m_popedContainer.txtcinema.Text = dtselect.Rows[0]["cinemaname"].ToString();
                            m_popedContainer.txttitle.Text = dtselect.Rows[0]["title"].ToString();
                            m_popedContainer.txtdate.Text = dt.Date.ToShortDateString();
                            m_popedContainer.txttimestart.Text = Convert.ToDateTime(dtselect.Rows[0]["start_time"]).ToShortTimeString();
                            m_popedContainer.txttimeend.Text = Convert.ToDateTime(dtselect.Rows[0]["end_time"]).ToShortTimeString();

                            TimeSpan result = TimeSpan.FromMinutes(Convert.ToDouble(dtselect.Rows[0]["duration"].ToString()));
                            string hours = ((int)result.TotalHours).ToString();
                            string minutes = String.Format("{0:00}", result.Minutes);
                            m_popedContainer.txtduration.Text = hours + ":" + minutes;

                            if (Convert.ToInt32(dtselect.Rows[0]["seat_type"].ToString()) == 1)
                                m_popedContainer.txtseat.Text = "Reserved Seating";
                            else if (Convert.ToInt32(dtselect.Rows[0]["seat_type"].ToString()) == 2)
                                m_popedContainer.txtseat.Text = "Free Seating (Guaranteed)";
                            else if (Convert.ToInt32(dtselect.Rows[0]["seat_type"].ToString()) == 3)
                                m_popedContainer.txtseat.Text = "Free Seating (Unlimited)";

                            //m_poperContainer.Show((System.Windows.Forms.Calendar.Calendar)sender);
                            m_poperContainer.Show(cal);
                            
                        }
                        break;
                    }
                }
                catch
                {
                }
            }
        }



        public void setDataGridViewIII(DataTable dt, DataGridView dgv)
        {
            dgv.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                DataGridViewCheckBoxColumn cbx = new DataGridViewCheckBoxColumn();
                cbx.Width = 30;

                dgv.DataSource = dt;
                int iwidth = dgv.Width / 3;
                dgv.DataSource = dt;
                dgv.Columns[0].Width = iwidth * 2;
                dgv.Columns[0].HeaderText = "Patron Type";
                dgv.Columns[0].ReadOnly = true;
                dgv.Columns[1].Width = iwidth / 2;
                dgv.Columns[1].HeaderText = "Price";
                dgv.Columns[2].Width = 0;
                dgv.Columns[2].HeaderText = "Patron ID";
                dgv.Columns[2].Visible = false;
                dgv.Columns[3].Width = 0;
                dgv.Columns[3].HeaderText = "Default";
                dgv.Columns[3].Visible = false;
            }
            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        private void calendar_MouseUp(object sender, MouseEventArgs e)
        {
            ICalendarSelectableElement hitted = ((System.Windows.Forms.Calendar.Calendar)sender).HitTest(e.Location);
            CalendarDay hitdat = hitted as CalendarDay;
            if (hitdat != null)
            {
                DateTime seldate = hitdat.Date;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    hitCalendar = Convert.ToInt32(hitdat.Calendar.Name);
                    hitDate = seldate; // added 1.4.2016 
                }
            }
        }

        private void dtcalview_ValueChanged(object sender, EventArgs e)
        {
            calview.ViewStart = dtcalview.Value;
            calview.SelectWeek(dtcalview.Value);
            calview.Refresh();
            calview.Invalidate();
        }

        private void calview_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < m_dtcinema.Rows.Count; i++)
                {
                    string cinemaname = m_dtcinema.Rows[i]["name"].ToString();
                    int cinemaid = Convert.ToInt32(m_dtcinema.Rows[i]["id"].ToString());
                    
                    Control.ControlCollection coll = grpfilter.Panel.Controls;
                    foreach (Control con in coll)
                    {
                        if(con is System.Windows.Forms.Calendar.Calendar)
                        {
                            if (con.Text.ToString().ToUpper() == cinemaname.Replace(" ", "").ToUpper())
                            {
                                ((System.Windows.Forms.Calendar.Calendar)con).SetViewRange(calview.SelectionStart, calview.SelectionEnd);

                                CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                                DateTime dtstart = GetFirstDayOfWeek(calview.SelectionStart, defaultCultureInfo);
                                addScreeningSched((System.Windows.Forms.Calendar.Calendar)con, cinemaid, dtstart);
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
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

        private void addScreeningSched(System.Windows.Forms.Calendar.Calendar cal, int cinemaid, DateTime dtstart)
        {
            DateTime dtref = dtstart;
            MySqlConnection myconn = new MySqlConnection()
            {
                ConnectionString = m_frmM._connection
            };
            StringBuilder sqry = new StringBuilder();

            sqry = new StringBuilder();
            sqry.Append("select a.start_time, a.end_time, c.code, a.id, a.status from movies_schedule_list a ");
            sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
            sqry.Append("left join movies c on b.movie_id = c.id ");
            sqry.Append(String.Format("where b.cinema_id = {0} ", cinemaid));
            sqry.Append(String.Format("and DATE(b.movie_date) between '{0}' ", String.Format("{0:yyyy-MM-dd}", dtref)));
            sqry.Append(String.Format("and '{0}' ", String.Format("{0:yyyy-MM-dd}", dtref.AddDays(cal.MaximumViewDays - 1))));
            sqry.Append("order by a.start_time DESC, a.end_time DESC;");

            CalendarItem calitem = null;
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            using (var cmd = new MySqlCommand(sqry.ToString(), myconn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                var movieCodes = new List<string>();
                int colorsw = 0;

                var groupByDate = dt.Rows.OfType<DataRow>()
                    .GroupBy(r => ((DateTime)r["start_time"]).Date)
                    .ToList();

                foreach (var itemDate in groupByDate)
                {
                    var groupByCode = itemDate
                        .GroupBy(r => (string)r["code"])
                        .ToList();

                    var titleIterator = 1;

                    foreach (var group in groupByCode)
                    {
                        var itemcolor = groupByCode.Count() == 1 ? Color.LightSteelBlue : titleIterator % 2 == 0 ?
                            Color.LightSteelBlue :
                            Color.FromArgb(100, 225, 225, 100);

                        foreach (var dr in group)
                        {
                            var startDateTimeData = dr["start_time"].ToString();
                            var startDateTime = Convert.ToDateTime(startDateTimeData);
                            var startShortDateTime = startDateTime.ToShortTimeString();

                            var calendarItem = new CalendarItem(cal, startDateTime, startShortDateTime)
                            {
                                BackgroundColor = itemcolor
                            };

                            if (cal.ViewIntersects(calendarItem))
                                cal.Items.Add(calendarItem);
                        }

                        var groupCode = group.Key;

                        var fsdfsd = group?.Min(t => (DateTime)t["start_time"]).AddSeconds(-1) ?? DateTime.Now.Date;

                        var calendarItemTitle = new CalendarItem(cal, fsdfsd, groupCode)
                        {
                            BackgroundColor = itemcolor
                        };

                        if (cal.ViewIntersects(calendarItemTitle))
                            cal.Items.Add(calendarItemTitle);

                        titleIterator += 1;
                    }
                }
            }
        }

        private void btnsked_Click(object sender, EventArgs e)
        {
            unselectbutton();
            frmMovieSched frmsked = new frmMovieSched();
            frmsked.frmInit(m_frmM, m_clscom);
            frmsked.ShowDialog();
            frmsked.Dispose();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void toolRefresh_Click(object sender, EventArgs e)
        {
            //melvin 10-27-2014 add right-click refresh
            refreshCal();
        }

        private void grpfilter_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolCopy_Click(object sender, EventArgs e) // 1.4.2016 added copy 
        {
            try
            {
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select b.id as movieschedid, b.cinema_id, b.movie_id, b.movie_date, a.id as movieschedlistid from movies_schedule_list a ");
                sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                sqry.Append("left join movies c on b.movie_id = c.id ");
                sqry.Append(String.Format("where b.cinema_id = {0} ", hitCalendar));
                sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", hitDate)));
                sqry.Append("order by c.code, a.start_time asc");

                copyQry = sqry;
               
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void toolPaste_Click(object sender, EventArgs e)
        {
            if (copyAll == true)
            {//2.18.2016
                StringBuilder sqry = new StringBuilder();
                DialogResult ans = MessageBox.Show("You are about to add \n\rall day movie schedule, continue?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == System.Windows.Forms.DialogResult.Yes)
                {
                    for (int i = 0; i < m_dtcinema.Rows.Count; i++)
                    {
                        int cinemaid = Convert.ToInt32(m_dtcinema.Rows[i]["id"].ToString());
                        sqry = new StringBuilder();
                        sqry.Append("select b.id as movieschedid, b.cinema_id, b.movie_id, b.movie_date, a.id as movieschedlistid from movies_schedule_list a ");
                        sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                        sqry.Append("left join movies c on b.movie_id = c.id ");
                        sqry.Append(String.Format("where b.cinema_id = {0} ", cinemaid));
                        sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", copyAllDate)));
                        sqry.Append("order by c.code, a.start_time asc");

                        pasteSched(cinemaid, sqry.ToString(), hitDate);
                    }
                }
            }//1.14.2016
            else if (copyQry.ToString() == "")
            {
                MessageBox.Show("Please copy a movie schedule first.", this.Text);
                return;
            }
            else
            {
                DialogResult ans = MessageBox.Show("You are about to add \n\rthe copied movie schedule, continue?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == System.Windows.Forms.DialogResult.Yes)
                    pasteSched(hitCalendar, copyQry.ToString(),hitDate);
            }

        }

        public void refreshCal()
        {
            try
            {
                for (int i = 0; i < m_dtcinema.Rows.Count; i++)
                {
                    string cinemaname = m_dtcinema.Rows[i]["name"].ToString();
                    int cinemaid = Convert.ToInt32(m_dtcinema.Rows[i]["id"].ToString());

                    Control.ControlCollection coll = grpfilter.Panel.Controls;
                    foreach (Control con in coll)
                    {
                        if (con is System.Windows.Forms.Calendar.Calendar)
                        {
                            if (con.Text.ToString().ToUpper() == cinemaname.Replace(" ", "").ToUpper())
                            {
                                ((System.Windows.Forms.Calendar.Calendar)con).SetViewRange(calview.SelectionStart, calview.SelectionEnd);

                                CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
                                DateTime dtstart = GetFirstDayOfWeek(calview.SelectionStart, defaultCultureInfo);
                                addScreeningSched((System.Windows.Forms.Calendar.Calendar)con, cinemaid, dtstart);
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)//2.18.2016
        {
            try
            {
                copyAll = true;
                copyAllDate = hitDate;
            }
            catch (Exception err)
            {
                copyAll = false;
                copyAllDate = new DateTime();
                MessageBox.Show(err.Message);
            }
        }
        
        public void pasteSched(int inttargetcinema, string strqry, DateTime pasteDate)
        {
            int movieschedid = -1;
            int cinemaid = -1;
            int movieid = -1;
            DateTime moviedate = new DateTime();
            int movieschedlistid = -1;

            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            if (myconn.State == ConnectionState.Closed)
                myconn.Open();
            DataTable dt = new DataTable();
            dt = m_clscom.setDataTable(strqry, m_frmM._connection);
            foreach (DataRow dr in dt.Rows)
            {
                string firsttitle = String.Empty;
                cinemaid = (int)dr["cinema_id"];
                movieid = (int)dr["movie_id"];
                moviedate = Convert.ToDateTime(dr["movie_date"].ToString());
                movieschedid = (int)dr["movieschedid"];
                movieschedlistid = (int)dr["movieschedlistid"];

                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from movies_schedule ");
                sqry.Append(String.Format("where movie_date = '{0}' ", pasteDate.Date.ToString("yyyy-MM-dd")));
                sqry.Append(String.Format("and cinema_id = {0} ", inttargetcinema));
                sqry.Append(String.Format("and movie_id = {0}", movieid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd2 = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd2.ExecuteScalar());
                cmd2.Dispose();

                int newmovieschedid = -1;
                if (rowCount > 0)
                {
                    sqry = new StringBuilder();
                    sqry.Append("select a.id from movies_schedule a ");
                    sqry.Append(String.Format("where a.movie_date = '{0}' ", pasteDate.Date.ToString("yyyy-MM-dd")));
                    sqry.Append(String.Format("and a.cinema_id = {0} ", inttargetcinema));
                    sqry.Append(String.Format("and a.movie_id = {0}", movieid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd3 = new MySqlCommand(sqry.ToString(), myconn);
                    MySqlDataReader reader3 = cmd3.ExecuteReader();
                    while (reader3.Read())
                    {
                        newmovieschedid = Convert.ToInt32(reader3["id"].ToString());
                    }
                    reader3.Close();
                    cmd3.Dispose();
                }
                else
                {
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("insert into movies_schedule value(0,{0},{1},'{2}')",
                        inttargetcinema, movieid, pasteDate.Date.ToString("yyyy-MM-dd")));
                    try
                    {
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd0 = new MySqlCommand(sqry.ToString(), myconn);
                        cmd0.ExecuteNonQuery();

                        newmovieschedid = Convert.ToInt32(cmd0.LastInsertedId.ToString());
                        cmd0.Dispose();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.ToString());
                        return;
                    }
                }

                int newmoviescheslistid =  -1;
                if(newmovieschedid > 0)
                {
                    DateTime curtimestart = new DateTime();
                    DateTime curtimeend = new DateTime();
                    int intval = -1;
                    sqry = new StringBuilder();
                    sqry.Append("select a.* from movies_schedule_list a ");
                    sqry.Append(String.Format("where a.id = {0}", movieschedlistid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd4 = new MySqlCommand(sqry.ToString(), myconn);
                    MySqlDataReader reader4 = cmd4.ExecuteReader();
                    while (reader4.Read())
                    {
                        DateTime stdate = Convert.ToDateTime(reader4["start_time"].ToString());
                        curtimestart = new DateTime(hitDate.Year, hitDate.Month, hitDate.Day, stdate.Hour, stdate.Minute, 0);
                        DateTime enddate = Convert.ToDateTime(reader4["end_time"].ToString());
                        curtimeend = new DateTime(hitDate.Year, hitDate.Month, hitDate.Day, enddate.Hour, enddate.Minute, 0);
                        intval = Convert.ToInt32(reader4["seat_type"].ToString());
                    }
                    reader4.Close();
                    cmd4.Dispose();

                    //validate for existing sched 1.5.2016
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from movies_schedule_list ");
                    sqry.Append(String.Format("where movies_schedule_id = '{0}' ", newmovieschedid));
                    sqry.Append(String.Format("and start_time <= '{0: yyyy-MM-dd HH:mm:00}' ", curtimestart));
                    sqry.Append(String.Format("and end_time >= '{0: yyyy-MM-dd HH:mm:00}'", curtimestart));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd44 = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount44 = Convert.ToInt32(cmd44.ExecuteScalar());
                    cmd44.Dispose();

                    if (rowCount44 == 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append("select count(*) from movies_schedule_list ");
                        sqry.Append(String.Format("where movies_schedule_id = '{0}' ", newmovieschedid));
                        sqry.Append(String.Format("and start_time <= '{0: yyyy-MM-dd HH:mm:00}' ", curtimeend));
                        sqry.Append(String.Format("and end_time >= '{0: yyyy-MM-dd HH:mm:00}'", curtimeend));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        MySqlCommand cmd444 = new MySqlCommand(sqry.ToString(), myconn);
                        int rowCount444 = Convert.ToInt32(cmd444.ExecuteScalar());
                        cmd444.Dispose();

                        if (rowCount444 == 0)
                        {
                            sqry = new StringBuilder();
                            sqry.Append(String.Format("insert into movies_schedule_list value(0,{0},'{1}','{2}',{3},0,1)",
                                newmovieschedid, String.Format("{0: yyyy-MM-dd HH:mm:00}", curtimestart), String.Format("{0: yyyy-MM-dd HH:mm:00}", curtimeend), intval));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd1 = new MySqlCommand(sqry.ToString(), myconn);
                            cmd1.ExecuteNonQuery();
                            newmoviescheslistid = Convert.ToInt32(cmd1.LastInsertedId.ToString());
                            cmd1.Dispose();
                        }
                    }
                }

                if (newmoviescheslistid > 0)
                {
                    sqry = new StringBuilder();
                    sqry.Append("select a.* from movies_schedule_list_patron a ");
                    sqry.Append(String.Format("where a.movies_schedule_list_id = {0}", movieschedlistid));
                    DataTable movieschedlistpatrons = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
                    if (movieschedlistpatrons.Rows.Count > 0)
                    {
                        foreach (DataRow row in movieschedlistpatrons.Rows)
                        {
                            int patronid = Convert.ToInt32(row["patron_id"].ToString());
                            double ticketprice = Convert.ToDouble(row["price"].ToString());
                            int isdefault = Convert.ToInt32(row["is_default"].ToString());

                            sqry = new StringBuilder();
                            sqry.Append(String.Format("insert into movies_schedule_list_patron values(0,{0},{1},{2},{3})",
                                newmoviescheslistid, patronid, ticketprice, isdefault));

                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd5 = new MySqlCommand(sqry.ToString(), myconn);
                            cmd5.ExecuteNonQuery();
                            cmd5.Dispose();
                        }
                    }
                }
                //for movie distributor
                //5.8.2018 added for the initial values of movies_distributor start
                sqry = new StringBuilder();
                sqry.Append(String.Format("select count(*) from movies_distributor where movie_id = {0}", movieid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd6 = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount2 = Convert.ToInt32(cmd6.ExecuteScalar());
                cmd6.Dispose();

                if (rowCount2 == 0)
                {
                    try
                    {
                        StringBuilder qry1 = new StringBuilder();
                        qry1.Append(String.Format("(select b.share_perc from movies b where b.id = {0})", movieid));

                        StringBuilder qry2 = new StringBuilder();
                        qry2.Append("(select min(b.movie_date) ");
                        qry2.Append(String.Format("from movies_schedule b where b.movie_id = {0})", movieid));

                        sqry = new StringBuilder();
                        sqry.Append(String.Format("insert into movies_distributor values (0,{0},{1},{2},{3})",
                            movieid, qry1.ToString(), qry2.ToString(), "7"));

                        MySqlCommand cmd7 = new MySqlCommand(sqry.ToString(), myconn);
                        cmd7.ExecuteNonQuery();
                        cmd7.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("An error have occured while \n adding the initial movie producer's share", "Movie Distributor");
                    }
                }//2.15.2016 added for the initial values of movies_distributor end
                else//2.18.2016 added recor for movies_distributor start
                {
                    //need to find out the new sched date > last sched date + day_count
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("select datediff(date_add(max(effective_date),interval day_count day),'{0:yyyy-MM-dd HH:mm:ss}') ", 
                        pasteDate.Date.ToString("yyyy-MM-dd")));//not sure for pastedate
                    sqry.Append("from movies_distributor ");
                    sqry.Append(String.Format("where movie_id = {0} ", movieid));
                    sqry.Append("order by effective_date desc limit 1");

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd8 = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount8 = Convert.ToInt32(cmd8.ExecuteScalar());
                    cmd8.Dispose();

                    if (rowCount8 <= 0)
                    {
                        try
                        {
                            sqry = new StringBuilder();
                            sqry.Append("insert into movies_distributor ");
                            sqry.Append("select '0',a.movie_id,a.share_perc,date_add(max(a.effective_date),interval a.day_count day),a.day_count ");
                            sqry.Append(String.Format("from movies_distributor a where a.movie_id = {0}", movieid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            MySqlCommand cmd7 = new MySqlCommand(sqry.ToString(), myconn);
                            cmd7.ExecuteNonQuery();
                            cmd7.Dispose();
                        }
                        catch
                        {
                            MessageBox.Show("An error have occured while adding \n the movie producer's share record", "Movie Distributor");
                        }
                    }
                }//2.18.2016 added recor for movies_distributor start

            }
            refreshCal();
        }
    }
}
