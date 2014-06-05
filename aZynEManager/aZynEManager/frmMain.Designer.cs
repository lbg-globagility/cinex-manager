namespace aZynEManager
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlControl = new System.Windows.Forms.Panel();
            this.pnlReport = new System.Windows.Forms.Panel();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.pnlUtility = new System.Windows.Forms.Panel();
            this.pnlCinema = new System.Windows.Forms.Panel();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.pnlClose = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purpleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlControl.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.Black;
            this.pnlControl.Controls.Add(this.pnlReport);
            this.pnlControl.Controls.Add(this.pnlUser);
            this.pnlControl.Controls.Add(this.pnlUtility);
            this.pnlControl.Controls.Add(this.pnlCinema);
            this.pnlControl.Location = new System.Drawing.Point(6, 29);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(331, 85);
            this.pnlControl.TabIndex = 0;
            // 
            // pnlReport
            // 
            this.pnlReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pnlReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlReport.BackgroundImage")));
            this.pnlReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlReport.Location = new System.Drawing.Point(87, 5);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(76, 74);
            this.pnlReport.TabIndex = 1;
            // 
            // pnlUser
            // 
            this.pnlUser.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnlUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlUser.BackgroundImage")));
            this.pnlUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlUser.Location = new System.Drawing.Point(168, 5);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(76, 74);
            this.pnlUser.TabIndex = 1;
            // 
            // pnlUtility
            // 
            this.pnlUtility.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlUtility.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlUtility.BackgroundImage")));
            this.pnlUtility.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlUtility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlUtility.Location = new System.Drawing.Point(249, 5);
            this.pnlUtility.Name = "pnlUtility";
            this.pnlUtility.Size = new System.Drawing.Size(76, 74);
            this.pnlUtility.TabIndex = 1;
            this.pnlUtility.Click += new System.EventHandler(this.pnlUtility_Click);
            // 
            // pnlCinema
            // 
            this.pnlCinema.BackColor = System.Drawing.Color.Fuchsia;
            this.pnlCinema.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlCinema.BackgroundImage")));
            this.pnlCinema.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlCinema.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCinema.Location = new System.Drawing.Point(6, 5);
            this.pnlCinema.Name = "pnlCinema";
            this.pnlCinema.Size = new System.Drawing.Size(76, 74);
            this.pnlCinema.TabIndex = 0;
            this.pnlCinema.Click += new System.EventHandler(this.pnlCinema_Click);
            // 
            // pnlDescription
            // 
            this.pnlDescription.BackColor = System.Drawing.Color.White;
            this.pnlDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDescription.Location = new System.Drawing.Point(6, 115);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(331, 105);
            this.pnlDescription.TabIndex = 1;
            // 
            // pnlClose
            // 
            this.pnlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlClose.BackgroundImage")));
            this.pnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlClose.Location = new System.Drawing.Point(312, 3);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(25, 25);
            this.pnlClose.TabIndex = 2;
            this.pnlClose.Click += new System.EventHandler(this.plnClose_Click);
            this.pnlClose.MouseLeave += new System.EventHandler(this.pnlClose_MouseLeave);
            this.pnlClose.MouseHover += new System.EventHandler(this.pnlClose_MouseHover);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blueToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.pinkToolStripMenuItem,
            this.purpleToolStripMenuItem,
            this.whiteToolStripMenuItem,
            this.yellowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowCheckMargin = true;
            this.contextMenuStrip1.Size = new System.Drawing.Size(132, 136);
            this.contextMenuStrip1.Text = "System Skin Color";
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.CheckOnClick = true;
            this.blueToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.blue;
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.blueToolStripMenuItem.Text = "&Blue";
            this.blueToolStripMenuItem.CheckedChanged += new System.EventHandler(this.blueToolStripMenuItem_CheckedChanged);
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.CheckOnClick = true;
            this.greenToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.green;
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.greenToolStripMenuItem.Text = "&Green";
            this.greenToolStripMenuItem.CheckedChanged += new System.EventHandler(this.greenToolStripMenuItem_CheckedChanged);
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
            // 
            // pinkToolStripMenuItem
            // 
            this.pinkToolStripMenuItem.Checked = true;
            this.pinkToolStripMenuItem.CheckOnClick = true;
            this.pinkToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pinkToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.red;
            this.pinkToolStripMenuItem.Name = "pinkToolStripMenuItem";
            this.pinkToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.pinkToolStripMenuItem.Text = "&Red";
            // 
            // purpleToolStripMenuItem
            // 
            this.purpleToolStripMenuItem.CheckOnClick = true;
            this.purpleToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.purple;
            this.purpleToolStripMenuItem.Name = "purpleToolStripMenuItem";
            this.purpleToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.purpleToolStripMenuItem.Text = "&Purple";
            this.purpleToolStripMenuItem.CheckedChanged += new System.EventHandler(this.purpleToolStripMenuItem_CheckedChanged);
            this.purpleToolStripMenuItem.Click += new System.EventHandler(this.purpleToolStripMenuItem_Click);
            // 
            // whiteToolStripMenuItem
            // 
            this.whiteToolStripMenuItem.CheckOnClick = true;
            this.whiteToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.white;
            this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
            this.whiteToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.whiteToolStripMenuItem.Text = "&White";
            this.whiteToolStripMenuItem.CheckedChanged += new System.EventHandler(this.whiteToolStripMenuItem_CheckedChanged);
            this.whiteToolStripMenuItem.Click += new System.EventHandler(this.whiteToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem
            // 
            this.yellowToolStripMenuItem.CheckOnClick = true;
            this.yellowToolStripMenuItem.Image = global::aZynEManager.Properties.Resources.yellow;
            this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            this.yellowToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.yellowToolStripMenuItem.Text = "&Yellow";
            this.yellowToolStripMenuItem.CheckedChanged += new System.EventHandler(this.yellowToolStripMenuItem_CheckedChanged);
            this.yellowToolStripMenuItem.Click += new System.EventHandler(this.yellowToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(343, 227);
            this.Controls.Add(this.pnlClose);
            this.Controls.Add(this.pnlDescription);
            this.Controls.Add(this.pnlControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.pnlControl.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Panel pnlCinema;
        private System.Windows.Forms.Panel pnlReport;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Panel pnlUtility;
        private System.Windows.Forms.Panel pnlDescription;
        private System.Windows.Forms.Panel pnlClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purpleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;

    }
}

