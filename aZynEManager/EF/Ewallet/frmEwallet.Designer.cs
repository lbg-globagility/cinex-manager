namespace aZynEManager.EF.Ewallet
{
    partial class frmEwallet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEwallet));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.datagridEwallets = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonPanel2 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridEwallets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonAdd,
            this.toolStripLabel1,
            this.ToolStripButtonCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(490, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ToolStripButtonAdd
            // 
            this.ToolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonAdd.Image")));
            this.ToolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonAdd.Name = "ToolStripButtonAdd";
            this.ToolStripButtonAdd.Size = new System.Drawing.Size(49, 22);
            this.ToolStripButtonAdd.Text = "&Add";
            this.ToolStripButtonAdd.Click += new System.EventHandler(this.ToolStripButtonAdd_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(52, 22);
            this.toolStripLabel1.Text = "               ";
            // 
            // ToolStripButtonCancel
            // 
            this.ToolStripButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonCancel.Image")));
            this.ToolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonCancel.Name = "ToolStripButtonCancel";
            this.ToolStripButtonCancel.Size = new System.Drawing.Size(63, 22);
            this.ToolStripButtonCancel.Text = "Cancel";
            this.ToolStripButtonCancel.Click += new System.EventHandler(this.ToolStripButtonCancel_Click);
            // 
            // datagridEwallets
            // 
            this.datagridEwallets.AllowUserToAddRows = false;
            this.datagridEwallets.AllowUserToDeleteRows = false;
            this.datagridEwallets.AllowUserToOrderColumns = true;
            this.datagridEwallets.BackgroundColor = System.Drawing.Color.White;
            this.datagridEwallets.ColumnHeadersHeight = 40;
            this.datagridEwallets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.datagridEwallets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridEwallets.EnableHeadersVisualStyles = false;
            this.datagridEwallets.Location = new System.Drawing.Point(0, 74);
            this.datagridEwallets.Name = "datagridEwallets";
            this.datagridEwallets.RowHeadersWidth = 56;
            this.datagridEwallets.Size = new System.Drawing.Size(490, 335);
            this.datagridEwallets.TabIndex = 19;
            this.datagridEwallets.DataSourceChanged += new System.EventHandler(this.datagridEwallets_DataSourceChanged);
            this.datagridEwallets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridEwallets_CellContentClick);
            this.datagridEwallets.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridEwallets_CellContentDoubleClick);
            this.datagridEwallets.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridEwallets_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Name";
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "AccountNo";
            this.Column2.HeaderText = "Account No./Mobile No.";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "IsActive";
            this.Column3.HeaderText = "Is Active?";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "IsDefault";
            this.Column4.HeaderText = "Is Default eWallet?";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 25);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(490, 49);
            this.kryptonPanel1.TabIndex = 20;
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 360);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(490, 49);
            this.kryptonPanel2.TabIndex = 21;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.AutoSize = false;
            this.kryptonLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonLabel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.kryptonLabel1.Size = new System.Drawing.Size(490, 20);
            this.kryptonLabel1.TabIndex = 13;
            this.kryptonLabel1.Values.Text = "To EDIT an eWallet, just double-click it";
            // 
            // frmEwallet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(490, 409);
            this.Controls.Add(this.kryptonPanel2);
            this.Controls.Add(this.datagridEwallets);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmEwallet";
            this.Load += new System.EventHandler(this.frmEwallet_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridEwallets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripButton ToolStripButtonAdd;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        internal System.Windows.Forms.ToolStripButton ToolStripButtonCancel;
        private System.Windows.Forms.DataGridView datagridEwallets;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}