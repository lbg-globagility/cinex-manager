namespace aZynEManager
{
    partial class frmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveas = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmbZoom = new System.Windows.Forms.ComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdlViewer1
            // 
            this.rdlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rdlViewer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(243)))), ((int)(((byte)(238)))));
            this.rdlViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.rdlViewer1.Folder = null;
            this.rdlViewer1.HighlightAll = false;
            this.rdlViewer1.HighlightAllColor = System.Drawing.Color.Fuchsia;
            this.rdlViewer1.HighlightCaseSensitive = false;
            this.rdlViewer1.HighlightItemColor = System.Drawing.Color.Aqua;
            //this.rdlViewer1.HighlightPageItem = null;
            this.rdlViewer1.HighlightText = null;
            this.rdlViewer1.Location = new System.Drawing.Point(0, 61);
            this.rdlViewer1.Name = "rdlViewer1";
            this.rdlViewer1.PageCurrent = 1;
            this.rdlViewer1.Parameters = null;
            this.rdlViewer1.ReportName = null;
            this.rdlViewer1.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
            this.rdlViewer1.SelectTool = false;
            this.rdlViewer1.ShowFindPanel = false;
            this.rdlViewer1.ShowParameterPanel = false;
            this.rdlViewer1.ShowWaitDialog = true;
            this.rdlViewer1.Size = new System.Drawing.Size(905, 489);
            this.rdlViewer1.SourceFile = null;
            this.rdlViewer1.SourceRdl = null;
            this.rdlViewer1.TabIndex = 246;
            this.rdlViewer1.Text = "RdlViewer1";
            this.rdlViewer1.UseTrueMargins = true;
            this.rdlViewer1.Zoom = 1.053922F;
            this.rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            this.rdlViewer1.Click += new System.EventHandler(this.rdlViewer1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnSaveas);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.cmbZoom);
            this.groupBox1.Location = new System.Drawing.Point(7, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 54);
            this.groupBox1.TabIndex = 245;
            this.groupBox1.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Peru;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::aZynEManager.Properties.Resources.print_32x32;
            this.btnPrint.Location = new System.Drawing.Point(306, 9);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(88, 42);
            this.btnPrint.TabIndex = 245;
            this.btnPrint.Text = " Print";
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSaveas
            // 
            this.btnSaveas.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSaveas.FlatAppearance.BorderSize = 0;
            this.btnSaveas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Peru;
            this.btnSaveas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveas.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveas.ForeColor = System.Drawing.Color.White;
            this.btnSaveas.Image = global::aZynEManager.Properties.Resources.save_32x32;
            this.btnSaveas.Location = new System.Drawing.Point(202, 9);
            this.btnSaveas.Name = "btnSaveas";
            this.btnSaveas.Size = new System.Drawing.Size(97, 42);
            this.btnSaveas.TabIndex = 246;
            this.btnSaveas.Text = "Save As";
            this.btnSaveas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveas.UseVisualStyleBackColor = false;
            this.btnSaveas.Click += new System.EventHandler(this.btnSaveas_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(9, 24);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(53, 13);
            this.Label1.TabIndex = 41;
            this.Label1.Text = "Zoom To:";
            // 
            // cmbZoom
            // 
            this.cmbZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbZoom.FormattingEnabled = true;
            this.cmbZoom.Items.AddRange(new object[] {
            "Actual Size",
            "Fit Page",
            "Fit Width",
            "800%",
            "400%",
            "200%",
            "150%",
            "125%",
            "100%",
            "75%",
            "50%",
            "25%"});
            this.cmbZoom.Location = new System.Drawing.Point(68, 20);
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new System.Drawing.Size(102, 21);
            this.cmbZoom.TabIndex = 40;
            this.cmbZoom.SelectedIndexChanged += new System.EventHandler(this.cmbZoom_SelectedIndexChanged);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::aZynEManager.Properties.Resources.bg2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(903, 537);
            this.Controls.Add(this.rdlViewer1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReport";
            this.Text = "Reports";
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.Resize += new System.EventHandler(this.frmReport_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal fyiReporting.RdlViewer.RdlViewer rdlViewer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSaveas;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cmbZoom;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;


    }
}