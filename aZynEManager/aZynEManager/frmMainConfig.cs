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
    public partial class frmMainConfig : Form
    {
        frmMain m_frmM = null;
        MySqlConnection myconn;
        public DataSet m_ds = new DataSet();
        public clscommon m_clscom = null;

        private bool drag = false;
        private System.Drawing.Point start_point = new System.Drawing.Point(0, 0);
        private bool draggable = true;
        private string exclude_list = "";

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

        public void frmInit(frmMain frm, clscommon cls)
        {
            unselectbutton();
            m_frmM = frm;
            m_clscom = cls;
        }

        public frmMainConfig()
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            this.MouseUp += new MouseEventHandler(Form_MouseUp);
            this.MouseMove += new MouseEventHandler(Form_MouseMove);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Height - this.Width) / 20, 10);
            setFocus(pnlClose);
            unselectbutton();
            pnlClose.Focus();
            btnGrants.Select();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        public void setFocus(Control cntrl)
        {
            cntrl.Focus();
            btnselect.Select();
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

        private void pnlClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlClose_MouseHover(object sender, EventArgs e)
        {
            pnlClose.BackgroundImage = Properties.Resources.buttonclosered;
        }

        private void pnlClose_MouseLeave(object sender, EventArgs e)
        {
            pnlClose.BackgroundImage = Properties.Resources.buttonclose1;
        }

        private void btnGrants_Click(object sender, EventArgs e)
        {
            unselectbutton();
            frmUserLevel frmul = new frmUserLevel();
            frmul.frmInit(m_frmM, m_clscom);
            frmul.ShowDialog();
            frmul.Dispose();
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "TASK_VIEW", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            frmTaskManager frmtm = new frmTaskManager();
            frmtm.frmInit(m_frmM, m_clscom);
            frmtm.ShowDialog();
            frmtm.Dispose();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            unselectbutton();
            frmUser frmu = new frmUser();
            frmu.frmInit(m_frmM, m_clscom);
            frmu.ShowDialog();
            frmu.Dispose();
        }

        private void btnATrail_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "TRAIL_VIEW", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            frmTrail frmtrail = new frmTrail();
            frmtrail.frmInit(m_frmM, m_clscom);
            frmtrail.ShowDialog();
            frmtrail.Dispose();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "CONFIG_SYS", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
        }
    }
}
