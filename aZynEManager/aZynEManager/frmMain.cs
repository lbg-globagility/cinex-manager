using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CommonLibrary;

namespace aZynEManager
{
    public partial class frmMain : Form
    {
        private bool drag = false;
        private System.Drawing.Point start_point = new System.Drawing.Point(0, 0);
        private bool draggable = true;
        private string exclude_list = "";
        private Panel _activePanel = null;
        //public string _connection = String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};","localhost", "3306", "azynema", "root", "");
        //public string _connection = String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", "192.168.0.218", "3306", "cinema", "as-admi_mvie1", "G23ticketV5");

        //supporting background and line color
        public Color _linecolor = Color.CornflowerBlue;
        public Color _backcolor = Color.CornflowerBlue;

        

        //form of the main modules
        //frmMainMovie frmmovie = null;

        public clscommon m_clscom = new clscommon();
        public DataTable m_dtdistributor = new DataTable();
        public DataTable m_dtcontact = new DataTable();
        public DataTable m_dtrating = new DataTable();
        public DataTable m_dtclassification = new DataTable();
        public DataTable m_dtmovies = new DataTable();
        public DataTable m_dtpatrons = new DataTable();

        public string m_usernm;
        public string m_usercode;
        public int m_userid = -1; //
        public bool boolAppAtTest = false;//

        

        public frmMain()
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            this.MouseUp += new MouseEventHandler(Form_MouseUp);
            this.MouseMove += new MouseEventHandler(Form_MouseMove);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, 10);

            _activePanel = pnlControl;

            if(m_clscom == null)
                m_clscom = new clscommon();

            m_clscom.initConfig(this);
        }

        

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

        public Color SkinLineColor
        {
            get { return _linecolor; }
            set { _linecolor = value; }
        }

        public Color SkinBackColor
        {
            get { return _backcolor; }
            set { _backcolor = value; }
        }

        private void plnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


        private void pnlClose_MouseHover(object sender, EventArgs e)
        {
            pnlClose.BorderStyle = BorderStyle.FixedSingle;
            pnlClose.BackgroundImage = Properties.Resources.buttonclosered;
        }

        private void pnlClose_MouseLeave(object sender, EventArgs e)
        {
            pnlClose.BorderStyle = BorderStyle.None;
            pnlClose.BackgroundImage = Properties.Resources.buttonclose1;
        }

        public int UserID
        {
            get { return m_userid; }
            set { m_userid = value; }
        }

        public string UserName
        {
            get { return m_usernm; }
            set { m_usernm = value; }
        }

        public string UserCode
        {
            get { return m_usercode; }
            set { m_usercode = value; }
        }

        public string _connection
        {
            get
            {
                return CommonUtility.ConnectionString;
            }
        }

        public string _odbcconnection
        {
            get
            {
                return CommonUtility.ODBCConnectionString;
            }
        }

        private void pnlCinema_Click(object sender, EventArgs e)
        {
            _activePanel.Hide();

            m_dtcontact = m_clscom.setDataTable("select * from people order by id asc", _connection);
            m_dtdistributor = m_clscom.setDataTable("select * from distributor order by name asc",_connection);
            m_dtrating = m_clscom.setDataTable("select * from mtrcb order by id asc", _connection);
            m_dtclassification = m_clscom.setDataTable("select * from classification order by description asc",_connection);

            frmMainMovie frmmovie = new frmMainMovie();
            frmmovie.setSkin(_backcolor, _linecolor);
            frmmovie.frmInit(this,m_clscom);
            frmmovie.ShowDialog();
            frmmovie.Dispose();

        }

        

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(MousePosition.X, MousePosition.Y);
            //pnlControl.Hide();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ToolStripMenuItem Item in contextMenuStrip1.Items)
                {
                    if (Item.CheckState == CheckState.Checked)
                        Item.Checked = true;
                }
                this.contextMenuStrip1.Show(pt);
            }
        }

        #region the skinColors
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.CornflowerBlue;
            SkinBackColor = Color.CornflowerBlue;
            blueToolStripMenuItem.Checked = true;
        }

        private void pinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.Salmon;
            SkinBackColor = Color.Salmon;
            pinkToolStripMenuItem.Checked = true;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.LawnGreen;
            SkinBackColor = Color.LawnGreen;
            greenToolStripMenuItem.Checked = true;
        }

        private void purpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.Purple;
            SkinBackColor = Color.Purple;
            purpleToolStripMenuItem.Checked = true;
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.LightGray;
            SkinBackColor = Color.LightGray;
            whiteToolStripMenuItem.Checked = true;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkinLineColor = Color.Gold;
            SkinBackColor = Color.Gold;
            yellowToolStripMenuItem.Checked = true;
        }

        private void blueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, blueToolStripMenuItem);
        }

        private void pinkToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, pinkToolStripMenuItem);
        }

        private void greenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, greenToolStripMenuItem);
        }

        private void purpleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, purpleToolStripMenuItem);
        }

        private void whiteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, whiteToolStripMenuItem);
        }

        private void yellowToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            clearAllChecked(contextMenuStrip1, yellowToolStripMenuItem);
        }

        public void clearAllChecked(ContextMenuStrip cms, ToolStripMenuItem mnuitem)
        {
            foreach (ToolStripMenuItem Item in cms.Items)
            {
                if (Item.Name != mnuitem.Name)
                    Item.Checked = false;
            }
        }
        #endregion

        private void pnlUtility_Click(object sender, EventArgs e)
        {
            frmMainUtility frmmu = new frmMainUtility();
            frmmu.setSkin(_backcolor, _linecolor);
            frmmu.frmInit(this, m_clscom);
            frmmu.ShowDialog();
            frmmu.Dispose();
        }

        private void pnlReport_Paint(object sender, PaintEventArgs e)
        {

        }



    }
}
