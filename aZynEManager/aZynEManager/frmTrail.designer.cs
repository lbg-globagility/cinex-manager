namespace aZynEManager
{
    partial class frmTrail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrail));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.lvwResults = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpfilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnsearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnclear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.cbxdate = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdetails = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.cmbmodule = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtcn = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbmodulegrp = new System.Windows.Forms.ComboBox();
            this.todate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.frdate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtuser = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtFound = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter.Panel)).BeginInit();
            this.grpfilter.Panel.SuspendLayout();
            this.grpfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Controls.Add(this.lvwResults);
            this.groupBox1.Controls.Add(this.btnselect);
            this.groupBox1.Location = new System.Drawing.Point(3, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(806, 372);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResult.Location = new System.Drawing.Point(6, 12);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(794, 354);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.TabIndex = 31;
            // 
            // lvwResults
            // 
            this.lvwResults.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lvwResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvwResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwResults.FullRowSelect = true;
            this.lvwResults.GridLines = true;
            this.lvwResults.HideSelection = false;
            this.lvwResults.Location = new System.Drawing.Point(61, 46);
            this.lvwResults.MultiSelect = false;
            this.lvwResults.Name = "lvwResults";
            this.lvwResults.ShowItemToolTips = true;
            this.lvwResults.Size = new System.Drawing.Size(685, 284);
            this.lvwResults.TabIndex = 30;
            this.lvwResults.UseCompatibleStateImageBehavior = false;
            this.lvwResults.View = System.Windows.Forms.View.Details;
            this.lvwResults.Visible = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Code";
            this.columnHeader1.Width = 62;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Title";
            this.columnHeader2.Width = 304;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Duration";
            this.columnHeader3.Width = 75;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "MTRCB Rating";
            this.columnHeader4.Width = 106;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Distributor";
            this.columnHeader5.Width = 129;
            // 
            // btnselect
            // 
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(144, 305);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(48, 46);
            this.btnselect.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnselect.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnselect.StateCommon.Back.ColorAngle = 20F;
            this.btnselect.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnselect.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnselect.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnselect.StateCommon.Border.Rounding = 1;
            this.btnselect.StateCommon.Border.Width = 3;
            this.btnselect.StateCommon.Content.AdjacentGap = 10;
            this.btnselect.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnselect.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnselect.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnselect.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnselect.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnselect.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnselect.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnselect.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnselect.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnselect.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnselect.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnselect.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnselect.StateTracking.Border.Rounding = 1;
            this.btnselect.StateTracking.Border.Width = 3;
            this.btnselect.TabIndex = 327;
            this.btnselect.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnselect.Values.Image")));
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // grpfilter
            // 
            this.grpfilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpfilter.Location = new System.Drawing.Point(661, 372);
            this.grpfilter.Name = "grpfilter";
            // 
            // grpfilter.Panel
            // 
            this.grpfilter.Panel.Controls.Add(this.btnsearch);
            this.grpfilter.Panel.Controls.Add(this.btnclear);
            this.grpfilter.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpfilter.Size = new System.Drawing.Size(149, 120);
            this.grpfilter.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpfilter.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpfilter.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpfilter.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.grpfilter.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpfilter.StateCommon.Border.Rounding = 5;
            this.grpfilter.StateCommon.Border.Width = 2;
            this.grpfilter.TabIndex = 296;
            // 
            // btnsearch
            // 
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.Location = new System.Drawing.Point(74, 7);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(59, 99);
            this.btnsearch.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Back.ColorAngle = 50F;
            this.btnsearch.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnsearch.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch.StateCommon.Border.Rounding = 1;
            this.btnsearch.StateCommon.Border.Width = 3;
            this.btnsearch.StateCommon.Content.AdjacentGap = 0;
            this.btnsearch.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnsearch.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnsearch.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnsearch.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnsearch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnsearch.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnsearch.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnsearch.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnsearch.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch.StateTracking.Border.Rounding = 2;
            this.btnsearch.StateTracking.Border.Width = 2;
            this.btnsearch.TabIndex = 32;
            this.btnsearch.Values.Image = global::aZynEManager.Properties.Resources.searchblue;
            this.btnsearch.Values.Text = "search";
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // btnclear
            // 
            this.btnclear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclear.Location = new System.Drawing.Point(8, 7);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(59, 99);
            this.btnclear.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Back.ColorAngle = 50F;
            this.btnclear.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnclear.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear.StateCommon.Border.Rounding = 1;
            this.btnclear.StateCommon.Border.Width = 3;
            this.btnclear.StateCommon.Content.AdjacentGap = 0;
            this.btnclear.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnclear.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnclear.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnclear.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnclear.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnclear.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnclear.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnclear.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnclear.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear.StateTracking.Border.Rounding = 2;
            this.btnclear.StateTracking.Border.Width = 2;
            this.btnclear.TabIndex = 30;
            this.btnclear.Values.Image = global::aZynEManager.Properties.Resources.simple;
            this.btnclear.Values.Text = "clear";
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroup1.Location = new System.Drawing.Point(89, 372);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.cbxdate);
            this.kryptonGroup1.Panel.Controls.Add(this.label7);
            this.kryptonGroup1.Panel.Controls.Add(this.txtdetails);
            this.kryptonGroup1.Panel.Controls.Add(this.cmbmodule);
            this.kryptonGroup1.Panel.Controls.Add(this.label6);
            this.kryptonGroup1.Panel.Controls.Add(this.label5);
            this.kryptonGroup1.Panel.Controls.Add(this.label4);
            this.kryptonGroup1.Panel.Controls.Add(this.txtcn);
            this.kryptonGroup1.Panel.Controls.Add(this.label1);
            this.kryptonGroup1.Panel.Controls.Add(this.cmbmodulegrp);
            this.kryptonGroup1.Panel.Controls.Add(this.todate);
            this.kryptonGroup1.Panel.Controls.Add(this.label3);
            this.kryptonGroup1.Panel.Controls.Add(this.frdate);
            this.kryptonGroup1.Panel.Controls.Add(this.label2);
            this.kryptonGroup1.Panel.Controls.Add(this.txtuser);
            this.kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Size = new System.Drawing.Size(569, 120);
            this.kryptonGroup1.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonGroup1.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonGroup1.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.kryptonGroup1.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup1.StateCommon.Border.Rounding = 5;
            this.kryptonGroup1.StateCommon.Border.Width = 2;
            this.kryptonGroup1.TabIndex = 297;
            // 
            // cbxdate
            // 
            this.cbxdate.AutoSize = true;
            this.cbxdate.BackColor = System.Drawing.Color.Transparent;
            this.cbxdate.ForeColor = System.Drawing.Color.White;
            this.cbxdate.Location = new System.Drawing.Point(337, 10);
            this.cbxdate.Name = "cbxdate";
            this.cbxdate.Size = new System.Drawing.Size(67, 17);
            this.cbxdate.TabIndex = 314;
            this.cbxdate.Text = "use date";
            this.cbxdate.UseVisualStyleBackColor = false;
            this.cbxdate.CheckedChanged += new System.EventHandler(this.cbxdate_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(280, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 100;
            this.label7.Text = "Details";
            // 
            // txtdetails
            // 
            this.txtdetails.BackColor = System.Drawing.Color.White;
            this.txtdetails.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtdetails.Location = new System.Drawing.Point(337, 58);
            this.txtdetails.Name = "txtdetails";
            this.txtdetails.Size = new System.Drawing.Size(220, 20);
            this.txtdetails.TabIndex = 99;
            // 
            // cmbmodule
            // 
            this.cmbmodule.BackColor = System.Drawing.Color.White;
            this.cmbmodule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbmodule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbmodule.FormattingEnabled = true;
            this.cmbmodule.Location = new System.Drawing.Point(95, 83);
            this.cmbmodule.Name = "cmbmodule";
            this.cmbmodule.Size = new System.Drawing.Size(462, 21);
            this.cmbmodule.TabIndex = 98;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(7, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 97;
            this.label6.Text = "Modules";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(7, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 95;
            this.label5.Text = "Module Group";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(7, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = "Computer Name";
            // 
            // txtcn
            // 
            this.txtcn.BackColor = System.Drawing.Color.White;
            this.txtcn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtcn.Location = new System.Drawing.Point(95, 32);
            this.txtcn.Name = "txtcn";
            this.txtcn.Size = new System.Drawing.Size(179, 20);
            this.txtcn.TabIndex = 93;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 92;
            this.label1.Text = "User ID";
            // 
            // cmbmodulegrp
            // 
            this.cmbmodulegrp.BackColor = System.Drawing.Color.White;
            this.cmbmodulegrp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbmodulegrp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbmodulegrp.FormattingEnabled = true;
            this.cmbmodulegrp.Location = new System.Drawing.Point(95, 57);
            this.cmbmodulegrp.Name = "cmbmodulegrp";
            this.cmbmodulegrp.Size = new System.Drawing.Size(179, 21);
            this.cmbmodulegrp.TabIndex = 91;
            this.cmbmodulegrp.SelectedIndexChanged += new System.EventHandler(this.cmbmodulegrp_SelectedIndexChanged);
            // 
            // todate
            // 
            this.todate.CustomFormat = "h:mm";
            this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.todate.Location = new System.Drawing.Point(469, 32);
            this.todate.Name = "todate";
            this.todate.ShowUpDown = true;
            this.todate.Size = new System.Drawing.Size(88, 20);
            this.todate.TabIndex = 89;
            this.todate.Value = new System.DateTime(2014, 6, 4, 0, 0, 0, 0);
            this.todate.ValueChanged += new System.EventHandler(this.todate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(424, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 90;
            this.label3.Text = "To Date";
            // 
            // frdate
            // 
            this.frdate.CustomFormat = "h:mm";
            this.frdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.frdate.Location = new System.Drawing.Point(337, 32);
            this.frdate.Name = "frdate";
            this.frdate.ShowUpDown = true;
            this.frdate.Size = new System.Drawing.Size(86, 20);
            this.frdate.TabIndex = 87;
            this.frdate.Value = new System.DateTime(2014, 6, 4, 0, 0, 0, 0);
            this.frdate.ValueChanged += new System.EventHandler(this.frdate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(280, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 88;
            this.label2.Text = "From Date";
            // 
            // txtuser
            // 
            this.txtuser.BackColor = System.Drawing.Color.White;
            this.txtuser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtuser.Location = new System.Drawing.Point(95, 7);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(179, 20);
            this.txtuser.TabIndex = 70;
            // 
            // txtFound
            // 
            this.txtFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFound.AutoSize = true;
            this.txtFound.BackColor = System.Drawing.Color.Transparent;
            this.txtFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtFound.ForeColor = System.Drawing.Color.Black;
            this.txtFound.Location = new System.Drawing.Point(5, 375);
            this.txtFound.Name = "txtFound";
            this.txtFound.Size = new System.Drawing.Size(38, 13);
            this.txtFound.TabIndex = 298;
            this.txtFound.Text = "Count:";
            // 
            // frmTrail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 495);
            this.Controls.Add(this.txtFound);
            this.Controls.Add(this.kryptonGroup1);
            this.Controls.Add(this.grpfilter);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTrail";
            this.Opacity = 0.98D;
            this.Text = "Audit Trail";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter.Panel)).EndInit();
            this.grpfilter.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).EndInit();
            this.grpfilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        internal System.Windows.Forms.ListView lvwResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpfilter;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        internal System.Windows.Forms.Label txtFound;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtuser;
        private System.Windows.Forms.DateTimePicker todate;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker frdate;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ComboBox cmbmodulegrp;
        internal System.Windows.Forms.ComboBox cmbmodule;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label4;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtcn;
        internal System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        internal System.Windows.Forms.Label label7;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtdetails;
        private System.Windows.Forms.CheckBox cbxdate;
    }
}