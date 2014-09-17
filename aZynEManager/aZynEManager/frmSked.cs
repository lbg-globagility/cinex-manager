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
                            sqry.Append("select b.name, a.price ");
                            sqry.Append("from movies_schedule_list_patron a ");
                            sqry.Append("left join patrons b on a.patron_id = b.id ");
                            sqry.Append(String.Format("where a.movies_schedule_list_id = {0}", dtselect.Rows[0]["id"].ToString()));
                            DataTable dtpatrons = m_clscom.setDataTable(sqry.ToString(), m_frmM._connection);
                            if(dtpatrons.Rows.Count > 0)
                                setDataGridViewIII(dtpatrons, m_popedContainer.dgvResult);

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
            }
        }

        private void calendar_MouseUp(object sender, MouseEventArgs e)
        {
            ICalendarSelectableElement hitted = ((System.Windows.Forms.Calendar.Calendar)sender).HitTest(e.Location);
            CalendarDay hitdat = hitted as CalendarDay;
            if (hitdat != null)
            {
                DateTime seldate = hitdat.Date;
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
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            StringBuilder sqry = new StringBuilder();
            int cntr = 0;
            do
            {
                sqry = new StringBuilder();
                sqry.Append("select a.start_time, a.end_time, c.code, a.id from movies_schedule_list a ");
                sqry.Append("left join movies_schedule b on a.movies_schedule_id = b.id ");
                sqry.Append("left join movies c on b.movie_id = c.id ");
                sqry.Append(String.Format("where b.cinema_id = {0} ", cinemaid));
                sqry.Append(String.Format("and b.movie_date = '{0}' ", String.Format("{0:yyyy-MM-dd HH:mm:ss}", dtref)));
                sqry.Append("order by a.start_time desc");

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
                    int intsw = 0;
                    Color itemcolor = Color.FromArgb(100, 225, 225, 100);
                    int reccntr = 0;
                    while (dr.Read())
                    {
                        reccntr += 1;
                        string sval = dr["code"].ToString();
                        string stime = dr["start_time"].ToString();
                        string etime = dr["end_time"].ToString();
                        DateTime dtstime = Convert.ToDateTime(stime);
                        DateTime dtetime = Convert.ToDateTime(stime);
                        string stimeval = dtstime.ToShortTimeString();// +" - " + dtetime.ToShortTimeString();
                        if (sval != "")
                        {
                            if (intsw == 0)
                            {
                                firsttitle = sval;
                                itemcolor = Color.FromArgb(100, 225, 225, 100);//Color.LightSteelBlue;
                            }

                            if (firsttitle != sval)
                            {
                                calitem = new CalendarItem(cal, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), firsttitle);
                                calitem.BackgroundColor = itemcolor;
                                if (cal.ViewIntersects(calitem))
                                    cal.Items.Add(calitem);

                                intsw = 0;
                                itemcolor = Color.LightSteelBlue;//Color.FromArgb(100, 225, 225, 100);
                                firsttitle = sval;
                            }
                            calitem = new CalendarItem(cal, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), stimeval);
                            calitem.BackgroundColor = itemcolor;
                            if (cal.ViewIntersects(calitem))
                                cal.Items.Add(calitem);

                            if (reccntr == dt.Rows.Count)
                            {
                                calitem = new CalendarItem(cal, dtref, dtref.AddHours((double)23).AddMinutes((double)59).AddSeconds((double)59), firsttitle);
                                calitem.BackgroundColor = itemcolor;
                                if (cal.ViewIntersects(calitem))
                                    cal.Items.Add(calitem);
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
    }
}
