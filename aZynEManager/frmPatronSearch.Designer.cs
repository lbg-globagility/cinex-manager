namespace aZynEManager
{
    partial class frmPatronSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatronSearch));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnupdate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.rbtnactive = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbtnAll = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.cbxeffdate = new System.Windows.Forms.CheckBox();
            this.dtend = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtstart = new System.Windows.Forms.DateTimePicker();
            this.txtname = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnPatron = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtcnt = new System.Windows.Forms.Label();
            this.grpfilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnclear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnsearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter.Panel)).BeginInit();
            this.grpfilter.Panel.SuspendLayout();
            this.grpfilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Location = new System.Drawing.Point(4, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(662, 444);
            this.groupBox1.TabIndex = 313;
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
            this.dgvResult.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.List;
            this.dgvResult.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.GridStyles.StyleColumn = ComponentFactory.Krypton.Toolkit.GridStyle.List;
            this.dgvResult.GridStyles.StyleDataCells = ComponentFactory.Krypton.Toolkit.GridStyle.List;
            this.dgvResult.GridStyles.StyleRow = ComponentFactory.Krypton.Toolkit.GridStyle.List;
            this.dgvResult.Location = new System.Drawing.Point(5, 12);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(652, 425);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.StateCommon.HeaderColumn.Content.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.dgvResult.StateCommon.HeaderColumn.Content.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.dgvResult.TabIndex = 31;
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroup1.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonGroup1.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlClient;
            this.kryptonGroup1.Location = new System.Drawing.Point(672, 7);
            this.kryptonGroup1.Name = "kryptonGroup1";
            this.kryptonGroup1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.btnPatron);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxeffdate);
            this.kryptonGroup1.Panel.Controls.Add(this.btnclear);
            this.kryptonGroup1.Panel.Controls.Add(this.dtend);
            this.kryptonGroup1.Panel.Controls.Add(this.label2);
            this.kryptonGroup1.Panel.Controls.Add(this.btnsearch);
            this.kryptonGroup1.Panel.Controls.Add(this.dtstart);
            this.kryptonGroup1.Panel.Controls.Add(this.txtname);
            this.kryptonGroup1.Panel.Controls.Add(this.label1);
            this.kryptonGroup1.Panel.Controls.Add(this.txtcode);
            this.kryptonGroup1.Panel.Controls.Add(this.Label8);
            this.kryptonGroup1.Panel.Controls.Add(this.label10);
            this.kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Size = new System.Drawing.Size(302, 226);
            this.kryptonGroup1.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonGroup1.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonGroup1.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.kryptonGroup1.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup1.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.kryptonGroup1.StateCommon.Border.Rounding = 5;
            this.kryptonGroup1.StateCommon.Border.Width = 3;
            this.kryptonGroup1.TabIndex = 312;
            // 
            // btnupdate
            // 
            this.btnupdate.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnupdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnupdate.Location = new System.Drawing.Point(100, 13);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.btnupdate.Size = new System.Drawing.Size(178, 51);
            this.btnupdate.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnupdate.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnupdate.StateCommon.Back.ColorAngle = 50F;
            this.btnupdate.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnupdate.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnupdate.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnupdate.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnupdate.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateCommon.Border.Rounding = 1;
            this.btnupdate.StateCommon.Border.Width = 3;
            this.btnupdate.StateCommon.Content.AdjacentGap = 0;
            this.btnupdate.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Inherit;
            this.btnupdate.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnupdate.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnupdate.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnupdate.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnupdate.StateCommon.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnupdate.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnupdate.StateCommon.Content.Padding = new System.Windows.Forms.Padding(12, 2, 15, 1);
            this.btnupdate.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnupdate.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnupdate.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateCommon.Content.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnupdate.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnupdate.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnupdate.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnupdate.StateTracking.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnupdate.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnupdate.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnupdate.StateTracking.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnupdate.StateTracking.Border.Rounding = 2;
            this.btnupdate.StateTracking.Border.Width = 2;
            this.btnupdate.TabIndex = 314;
            this.btnupdate.Values.Image = global::aZynEManager.Properties.Resources.buttonedit1;
            this.btnupdate.Values.Text = "update \r\nactive patron";
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // rbtnactive
            // 
            this.rbtnactive.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalControl;
            this.rbtnactive.Location = new System.Drawing.Point(25, 40);
            this.rbtnactive.Name = "rbtnactive";
            this.rbtnactive.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.rbtnactive.Size = new System.Drawing.Size(56, 20);
            this.rbtnactive.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.rbtnactive.StateCommon.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.rbtnactive.StateCommon.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.rbtnactive.TabIndex = 313;
            this.rbtnactive.Values.Text = "Active";
            this.rbtnactive.CheckedChanged += new System.EventHandler(this.rbtnactive_CheckedChanged);
            // 
            // rbtnAll
            // 
            this.rbtnAll.Checked = true;
            this.rbtnAll.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalControl;
            this.rbtnAll.Location = new System.Drawing.Point(25, 15);
            this.rbtnAll.Name = "rbtnAll";
            this.rbtnAll.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.rbtnAll.Size = new System.Drawing.Size(36, 20);
            this.rbtnAll.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.rbtnAll.StateCommon.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.rbtnAll.StateCommon.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.rbtnAll.TabIndex = 312;
            this.rbtnAll.Values.Text = "All";
            this.rbtnAll.CheckedChanged += new System.EventHandler(this.rbtnAll_CheckedChanged);
            // 
            // cbxeffdate
            // 
            this.cbxeffdate.AutoSize = true;
            this.cbxeffdate.BackColor = System.Drawing.Color.Transparent;
            this.cbxeffdate.ForeColor = System.Drawing.Color.White;
            this.cbxeffdate.Location = new System.Drawing.Point(89, 62);
            this.cbxeffdate.Name = "cbxeffdate";
            this.cbxeffdate.Size = new System.Drawing.Size(120, 17);
            this.cbxeffdate.TabIndex = 311;
            this.cbxeffdate.Text = "Use Effectivity Date";
            this.cbxeffdate.UseVisualStyleBackColor = false;
            this.cbxeffdate.CheckedChanged += new System.EventHandler(this.cbxeffdate_CheckedChanged);
            // 
            // dtend
            // 
            this.dtend.CustomFormat = "h:mm";
            this.dtend.Enabled = false;
            this.dtend.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtend.Location = new System.Drawing.Point(202, 85);
            this.dtend.Name = "dtend";
            this.dtend.ShowUpDown = true;
            this.dtend.Size = new System.Drawing.Size(85, 20);
            this.dtend.TabIndex = 310;
            this.dtend.Value = new System.DateTime(2014, 6, 4, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(180, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 309;
            this.label2.Text = "to";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dtstart
            // 
            this.dtstart.CustomFormat = "h:mm";
            this.dtstart.Enabled = false;
            this.dtstart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtstart.Location = new System.Drawing.Point(89, 85);
            this.dtstart.Name = "dtstart";
            this.dtstart.ShowUpDown = true;
            this.dtstart.Size = new System.Drawing.Size(85, 20);
            this.dtstart.TabIndex = 307;
            this.dtstart.Value = new System.DateTime(2014, 6, 4, 0, 0, 0, 0);
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.White;
            this.txtname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtname.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtname.InputControlStyle = ComponentFactory.Krypton.Toolkit.InputControlStyle.Standalone;
            this.txtname.Location = new System.Drawing.Point(89, 32);
            this.txtname.Name = "txtname";
            this.txtname.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.txtname.Size = new System.Drawing.Size(198, 23);
            this.txtname.TabIndex = 1;
            this.txtname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 82;
            this.label1.Text = "Code";
            // 
            // txtcode
            // 
            this.txtcode.BackColor = System.Drawing.Color.White;
            this.txtcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtcode.InputControlStyle = ComponentFactory.Krypton.Toolkit.InputControlStyle.Standalone;
            this.txtcode.Location = new System.Drawing.Point(89, 5);
            this.txtcode.Name = "txtcode";
            this.txtcode.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.txtcode.Size = new System.Drawing.Size(91, 23);
            this.txtcode.TabIndex = 0;
            this.txtcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(8, 39);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(35, 13);
            this.Label8.TabIndex = 80;
            this.Label8.Text = "Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(8, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 308;
            this.label10.Text = "Effective Date";
            // 
            // btnPatron
            // 
            this.btnPatron.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnPatron.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPatron.Location = new System.Drawing.Point(233, 111);
            this.btnPatron.Name = "btnPatron";
            this.btnPatron.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.btnPatron.Size = new System.Drawing.Size(54, 97);
            this.btnPatron.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnPatron.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnPatron.StateCommon.Back.ColorAngle = 50F;
            this.btnPatron.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnPatron.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnPatron.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnPatron.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPatron.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateCommon.Border.Rounding = 1;
            this.btnPatron.StateCommon.Border.Width = 3;
            this.btnPatron.StateCommon.Content.AdjacentGap = 0;
            this.btnPatron.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Inherit;
            this.btnPatron.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPatron.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnPatron.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnPatron.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatron.StateCommon.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnPatron.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnPatron.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnPatron.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPatron.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatron.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatron.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnPatron.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPatron.StateTracking.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnPatron.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnPatron.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPatron.StateTracking.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnPatron.StateTracking.Border.Rounding = 2;
            this.btnPatron.StateTracking.Border.Width = 2;
            this.btnPatron.TabIndex = 1;
            this.btnPatron.Values.Image = global::aZynEManager.Properties.Resources.buttonadd;
            this.btnPatron.Values.Text = "data\r\nentry";
            this.btnPatron.Click += new System.EventHandler(this.btnPatron_Click);
            // 
            // txtcnt
            // 
            this.txtcnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtcnt.AutoSize = true;
            this.txtcnt.BackColor = System.Drawing.Color.Transparent;
            this.txtcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcnt.ForeColor = System.Drawing.Color.Black;
            this.txtcnt.Location = new System.Drawing.Point(672, 423);
            this.txtcnt.Name = "txtcnt";
            this.txtcnt.Size = new System.Drawing.Size(38, 13);
            this.txtcnt.TabIndex = 316;
            this.txtcnt.Text = "Count:";
            // 
            // grpfilter
            // 
            this.grpfilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpfilter.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.grpfilter.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlClient;
            this.grpfilter.Location = new System.Drawing.Point(676, 236);
            this.grpfilter.Name = "grpfilter";
            this.grpfilter.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            // 
            // grpfilter.Panel
            // 
            this.grpfilter.Panel.Controls.Add(this.rbtnactive);
            this.grpfilter.Panel.Controls.Add(this.btnupdate);
            this.grpfilter.Panel.Controls.Add(this.rbtnAll);
            this.grpfilter.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpfilter.Size = new System.Drawing.Size(298, 91);
            this.grpfilter.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpfilter.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpfilter.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.grpfilter.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpfilter.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpfilter.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.grpfilter.StateCommon.Border.Rounding = 5;
            this.grpfilter.StateCommon.Border.Width = 2;
            this.grpfilter.TabIndex = 317;
            // 
            // btnclear
            // 
            this.btnclear.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnclear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclear.Location = new System.Drawing.Point(11, 111);
            this.btnclear.Name = "btnclear";
            this.btnclear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.btnclear.Size = new System.Drawing.Size(54, 97);
            this.btnclear.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Back.ColorAngle = 50F;
            this.btnclear.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnclear.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnclear.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateCommon.Border.Rounding = 1;
            this.btnclear.StateCommon.Border.Width = 3;
            this.btnclear.StateCommon.Content.AdjacentGap = 0;
            this.btnclear.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Inherit;
            this.btnclear.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnclear.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnclear.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear.StateCommon.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnclear.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnclear.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnclear.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnclear.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnclear.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnclear.StateTracking.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnclear.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnclear.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear.StateTracking.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnclear.StateTracking.Border.Rounding = 2;
            this.btnclear.StateTracking.Border.Width = 2;
            this.btnclear.TabIndex = 30;
            this.btnclear.Values.Image = global::aZynEManager.Properties.Resources.simple;
            this.btnclear.Values.Text = "clear";
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // btnsearch
            // 
            this.btnsearch.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.Location = new System.Drawing.Point(72, 111);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.btnsearch.Size = new System.Drawing.Size(54, 97);
            this.btnsearch.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Back.ColorAngle = 50F;
            this.btnsearch.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnsearch.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateCommon.Border.Rounding = 1;
            this.btnsearch.StateCommon.Border.Width = 3;
            this.btnsearch.StateCommon.Content.AdjacentGap = 0;
            this.btnsearch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Inherit;
            this.btnsearch.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnsearch.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnsearch.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.StateCommon.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnsearch.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnsearch.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnsearch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnsearch.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnsearch.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnsearch.StateTracking.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnsearch.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnsearch.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch.StateTracking.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnsearch.StateTracking.Border.Rounding = 2;
            this.btnsearch.StateTracking.Border.Width = 2;
            this.btnsearch.TabIndex = 32;
            this.btnsearch.Values.Image = global::aZynEManager.Properties.Resources.searchblue;
            this.btnsearch.Values.Text = "search";
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // btnselect
            // 
            this.btnselect.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(917, 392);
            this.btnselect.Name = "btnselect";
            this.btnselect.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Global;
            this.btnselect.Size = new System.Drawing.Size(46, 46);
            this.btnselect.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnselect.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnselect.StateCommon.Back.ColorAngle = 50F;
            this.btnselect.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnselect.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnselect.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnselect.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnselect.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnselect.StateCommon.Border.Rounding = 1;
            this.btnselect.StateCommon.Border.Width = 3;
            this.btnselect.StateCommon.Content.AdjacentGap = 10;
            this.btnselect.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Inherit;
            this.btnselect.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnselect.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnselect.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnselect.StateCommon.Content.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnselect.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnselect.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateCommon.Content.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnselect.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnselect.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnselect.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnselect.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnselect.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnselect.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnselect.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnselect.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnselect.StateCommon.Content.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            this.btnselect.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnselect.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnselect.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnselect.StateTracking.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.btnselect.StateTracking.Border.Rounding = 3;
            this.btnselect.StateTracking.Border.Width = 1;
            this.btnselect.TabIndex = 318;
            this.btnselect.Values.Image = global::aZynEManager.Properties.Resources.buttonapply;
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // frmPatronSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 450);
            this.Controls.Add(this.btnselect);
            this.Controls.Add(this.grpfilter);
            this.Controls.Add(this.txtcnt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.kryptonGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPatronSearch";
            this.Text = "Patron List";
            this.Load += new System.EventHandler(this.frmPatronSearch_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter.Panel)).EndInit();
            this.grpfilter.Panel.ResumeLayout(false);
            this.grpfilter.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).EndInit();
            this.grpfilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        private System.Windows.Forms.DateTimePicker dtstart;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtname;
        internal System.Windows.Forms.Label label1;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtcode;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtend;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPatron;
        internal System.Windows.Forms.Label txtcnt;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpfilter;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        private System.Windows.Forms.CheckBox cbxeffdate;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbtnAll;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnupdate;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbtnactive;
    }
}