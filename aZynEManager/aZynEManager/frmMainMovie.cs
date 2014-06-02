using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace aZynEManager
{
    public partial class frmMainMovie : Form
    {
        private bool drag = false;
        private System.Drawing.Point start_point = new System.Drawing.Point(0, 0);
        private bool draggable = true;
        private string exclude_list = "";

        frmMain m_frmM = null;
        MySqlConnection myconn;
        public DataSet m_ds = new DataSet(); 

        #region MouseDragControl
        protected override void OnControlAdded(ControlEventArgs e)
        {
            //
            //Add Mouse Event Handlers for each control added into the form,
            //if Draggable property of the form is set to true and the control
            //name is not in the ExcludeList.Exclude list is the comma separated
            //list of the Controls for which you do not require the mouse handler 
            //to be added. For Example a button.  
            //
            if (this.Draggable && (this.ExcludeList.IndexOf(e.Control.Name) == -1))
            {
                e.Control.MouseDown += new MouseEventHandler(Form_MouseDown);
                e.Control.MouseUp += new MouseEventHandler(Form_MouseUp);
                e.Control.MouseMove += new MouseEventHandler(Form_MouseMove);
            }
            base.OnControlAdded(e);
        }
        void Form_MouseDown(object sender, MouseEventArgs e)
        {
            //
            //On Mouse Down set the flag drag=true and 
            //Store the clicked point to the start_point variable
            //
            this.drag = true;
            this.start_point = new System.Drawing.Point(e.X, e.Y);
        }
        void Form_MouseUp(object sender, MouseEventArgs e)
        {
            //
            //Set the drag flag = false;
            //
            this.drag = false;
        }
        void Form_MouseMove(object sender, MouseEventArgs e)
        {
            //
            //If drag = true, drag the form
            //
            if (this.drag)
            {
                System.Drawing.Point p1 = new System.Drawing.Point(e.X, e.Y);
                System.Drawing.Point p2 = this.PointToScreen(p1);
                System.Drawing.Point p3 = new System.Drawing.Point(p2.X - this.start_point.X,
                                     p2.Y - this.start_point.Y);
                this.Location = p3;
            }
        }
        public string ExcludeList
        {
            set
            {
                this.exclude_list = value;
            }
            get
            {
                return this.exclude_list.Trim();
            }
        }
        public bool Draggable
        {
            set
            {
                this.draggable = value;
            }
            get
            {
                return this.draggable;
            }
        }
        #endregion

        public frmMainMovie()
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            this.MouseUp += new MouseEventHandler(Form_MouseUp);
            this.MouseMove += new MouseEventHandler(Form_MouseMove);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Height - this.Width) / 20, 10);
            setFocus(pnlClose);
            unselectButton();
            pnlClose.Focus();
            btnTrailer.Select();
        }

        public void frmInit(frmMain frm)
        {
            m_frmM = frm;
        }

        public void unselectButton()
        {
            ControlCollection coll = (ControlCollection)this.Controls;
            foreach (Control con in coll)
            {
                if (con is ComponentFactory.Krypton.Toolkit.KryptonButton)
                {
                    //((ComponentFactory.Krypton.Toolkit.KryptonButton)con)
                }
            }
        }

        public void setFocus(Control cntrl)
        {
            cntrl.Focus();
        }

        

        private void frmMainMovie_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void pnlClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlClose_MouseHover(object sender, EventArgs e)
        {
            pnlClose.BackgroundImage = Properties.Resources.buttonclosered;
            //pnlClose.BackColor = BackColor;
        }

        private void pnlClose_MouseLeave(object sender, EventArgs e)
        {
            pnlClose.BackgroundImage = Properties.Resources.buttonclose1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;
                myconn.Open();
                //MySqlCommand cmd = new MySqlCommand("Show Databases", myconn);
                //MySqlDataReader dr = cmd.ExecuteReader();
                //while (dr.Read())
                //{
                //    this.cmbdbase.Items.Add(dr[0].ToString());
                //}
                //dr.Close();
                if (myconn.State == ConnectionState.Open)
                {
                    string strqry = "SELECT a.id, a.code, a.title, a.duration, b.name as rating, c.name as description ";
                        strqry += "FROM movies a,  mtrcb b, distributor c ";
                        strqry += "WHERE a.rating_id = b.id and a.dist_id = c.id";
                    MySqlDataAdapter da = new MySqlDataAdapter(strqry, myconn);
                    da.Fill(m_ds);
                    myconn.Close();
                }
            }
            catch(MySqlException err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                //check for the user grant for the module
                //yes
                if (m_ds != null && m_ds.Tables.Count > 0)
                {
                    frmMovieList frmlist = new frmMovieList();
                    frmlist.frmInit(m_frmM, m_ds);
                    frmlist.ShowDialog();
                }
            }
        }

        public void setSkin(Color backColor, Color lineColor)
        {
            ControlCollection coll = (ControlCollection)this.Controls;
            foreach (Control con in coll)
            {
                if (con is ComponentFactory.Krypton.Toolkit.KryptonButton)
                {
                    ((ComponentFactory.Krypton.Toolkit.KryptonButton)con).StateCommon.Back.Color2 = backColor;
                    ((ComponentFactory.Krypton.Toolkit.KryptonButton)con).StateCommon.Border.Color1 = lineColor;
                }
            }

            pnlClose.BackColor = backColor;
        }
    }
}
