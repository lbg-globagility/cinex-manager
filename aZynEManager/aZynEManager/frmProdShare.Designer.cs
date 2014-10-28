namespace aZynEManager
{
    partial class frmProdShare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProdShare));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpValues = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.txtdaycnt = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbDate = new System.Windows.Forms.ComboBox();
            this.cbxdate = new System.Windows.Forms.CheckBox();
            this.cbxfilter = new System.Windows.Forms.CheckBox();
            this.btnclear2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnsearch2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbtitle = new System.Windows.Forms.ComboBox();
            this.dtdate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtshare = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtcnt = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.btnsearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnclear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvMovies = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpfilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpcontrol = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpValues)).BeginInit();
            this.grpValues.Panel.SuspendLayout();
            this.grpValues.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).BeginInit();
            this.grpfilter.Panel.SuspendLayout();
            this.grpfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).BeginInit();
            this.grpcontrol.Panel.SuspendLayout();
            this.grpcontrol.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Controls.Add(this.btnselect);
            this.groupBox1.Location = new System.Drawing.Point(361, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(614, 356);
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
            this.dgvResult.Size = new System.Drawing.Size(602, 338);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.TabIndex = 1;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            this.dgvResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellContentClick);
            this.dgvResult.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResult_CellFormatting);
            // 
            // btnselect
            // 
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(23, 273);
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
            this.btnselect.TabIndex = 310;
            this.btnselect.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnselect.Values.Image")));
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // grpValues
            // 
            this.grpValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpValues.Location = new System.Drawing.Point(3, 321);
            this.grpValues.Name = "grpValues";
            // 
            // grpValues.Panel
            // 
            this.grpValues.Panel.Controls.Add(this.txtdaycnt);
            this.grpValues.Panel.Controls.Add(this.label9);
            this.grpValues.Panel.Controls.Add(this.cmbDate);
            this.grpValues.Panel.Controls.Add(this.cbxdate);
            this.grpValues.Panel.Controls.Add(this.cbxfilter);
            this.grpValues.Panel.Controls.Add(this.btnclear2);
            this.grpValues.Panel.Controls.Add(this.btnsearch2);
            this.grpValues.Panel.Controls.Add(this.cmbtitle);
            this.grpValues.Panel.Controls.Add(this.dtdate);
            this.grpValues.Panel.Controls.Add(this.label2);
            this.grpValues.Panel.Controls.Add(this.txtshare);
            this.grpValues.Panel.Controls.Add(this.label3);
            this.grpValues.Panel.Controls.Add(this.label1);
            this.grpValues.Panel.Controls.Add(this.txtcode);
            this.grpValues.Panel.Controls.Add(this.Label8);
            this.grpValues.Panel.Controls.Add(this.label7);
            this.grpValues.Panel.Controls.Add(this.label5);
            this.grpValues.Panel.Controls.Add(this.label13);
            this.grpValues.Panel.Controls.Add(this.label4);
            this.grpValues.Panel.Controls.Add(this.label6);
            this.grpValues.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpValues.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpValues.Size = new System.Drawing.Size(357, 163);
            this.grpValues.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpValues.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpValues.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpValues.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpValues.StateCommon.Border.Rounding = 5;
            this.grpValues.StateCommon.Border.Width = 2;
            this.grpValues.TabIndex = 0;
            // 
            // txtdaycnt
            // 
            this.txtdaycnt.BackColor = System.Drawing.Color.White;
            this.txtdaycnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdaycnt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtdaycnt.Location = new System.Drawing.Point(233, 101);
            this.txtdaycnt.MaxLength = 5;
            this.txtdaycnt.Name = "txtdaycnt";
            this.txtdaycnt.ReadOnly = true;
            this.txtdaycnt.Size = new System.Drawing.Size(95, 20);
            this.txtdaycnt.TabIndex = 315;
            this.txtdaycnt.Text = "7";
            this.txtdaycnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtdaycnt.TextChanged += new System.EventHandler(this.txtdaycnt_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(152, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 316;
            this.label9.Text = "Days Covered";
            // 
            // cmbDate
            // 
            this.cmbDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDate.FormattingEnabled = true;
            this.cmbDate.Items.AddRange(new object[] {
            "<",
            "<=",
            ">",
            ">=",
            "="});
            this.cmbDate.Location = new System.Drawing.Point(282, 126);
            this.cmbDate.Name = "cmbDate";
            this.cmbDate.Size = new System.Drawing.Size(46, 21);
            this.cmbDate.TabIndex = 314;
            // 
            // cbxdate
            // 
            this.cbxdate.AutoSize = true;
            this.cbxdate.BackColor = System.Drawing.Color.Transparent;
            this.cbxdate.ForeColor = System.Drawing.Color.White;
            this.cbxdate.Location = new System.Drawing.Point(211, 129);
            this.cbxdate.Name = "cbxdate";
            this.cbxdate.Size = new System.Drawing.Size(67, 17);
            this.cbxdate.TabIndex = 313;
            this.cbxdate.Text = "use date";
            this.cbxdate.UseVisualStyleBackColor = false;
            this.cbxdate.CheckedChanged += new System.EventHandler(this.cbxdate_CheckedChanged);
            // 
            // cbxfilter
            // 
            this.cbxfilter.AutoSize = true;
            this.cbxfilter.BackColor = System.Drawing.Color.Transparent;
            this.cbxfilter.ForeColor = System.Drawing.Color.White;
            this.cbxfilter.Location = new System.Drawing.Point(11, 130);
            this.cbxfilter.Name = "cbxfilter";
            this.cbxfilter.Size = new System.Drawing.Size(179, 17);
            this.cbxfilter.TabIndex = 312;
            this.cbxfilter.Text = "use the information for searching";
            this.cbxfilter.UseVisualStyleBackColor = false;
            this.cbxfilter.CheckedChanged += new System.EventHandler(this.cbxfilter_CheckedChanged);
            // 
            // btnclear2
            // 
            this.btnclear2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclear2.Location = new System.Drawing.Point(283, 7);
            this.btnclear2.Name = "btnclear2";
            this.btnclear2.Size = new System.Drawing.Size(42, 42);
            this.btnclear2.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnclear2.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnclear2.StateCommon.Back.ColorAngle = 50F;
            this.btnclear2.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnclear2.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnclear2.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnclear2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear2.StateCommon.Border.Rounding = 1;
            this.btnclear2.StateCommon.Border.Width = 3;
            this.btnclear2.StateCommon.Content.AdjacentGap = 0;
            this.btnclear2.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear2.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear2.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnclear2.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnclear2.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear2.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnclear2.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear2.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnclear2.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear2.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1);
            this.btnclear2.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnclear2.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear2.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear2.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnclear2.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnclear2.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnclear2.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnclear2.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnclear2.StateTracking.Border.Rounding = 2;
            this.btnclear2.StateTracking.Border.Width = 2;
            this.btnclear2.TabIndex = 301;
            this.toolTip1.SetToolTip(this.btnclear2, "clear");
            this.btnclear2.Values.Image = global::aZynEManager.Properties.Resources.simple;
            this.btnclear2.Values.Text = "";
            this.btnclear2.Click += new System.EventHandler(this.btnclear2_Click);
            // 
            // btnsearch2
            // 
            this.btnsearch2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch2.Location = new System.Drawing.Point(226, 7);
            this.btnsearch2.Name = "btnsearch2";
            this.btnsearch2.Size = new System.Drawing.Size(42, 42);
            this.btnsearch2.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch2.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch2.StateCommon.Back.ColorAngle = 50F;
            this.btnsearch2.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnsearch2.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnsearch2.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnsearch2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch2.StateCommon.Border.Rounding = 1;
            this.btnsearch2.StateCommon.Border.Width = 3;
            this.btnsearch2.StateCommon.Content.AdjacentGap = 0;
            this.btnsearch2.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch2.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch2.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnsearch2.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnsearch2.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch2.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnsearch2.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch2.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnsearch2.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch2.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1);
            this.btnsearch2.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnsearch2.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch2.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch2.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnsearch2.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnsearch2.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnsearch2.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnsearch2.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnsearch2.StateTracking.Border.Rounding = 2;
            this.btnsearch2.StateTracking.Border.Width = 2;
            this.btnsearch2.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnsearch2, "search");
            this.btnsearch2.Values.Image = global::aZynEManager.Properties.Resources.searchblue;
            this.btnsearch2.Values.Text = "";
            this.btnsearch2.Click += new System.EventHandler(this.btnsearch2_Click);
            // 
            // cmbtitle
            // 
            this.cmbtitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbtitle.FormattingEnabled = true;
            this.cmbtitle.Location = new System.Drawing.Point(14, 54);
            this.cmbtitle.MaxLength = 1333333333;
            this.cmbtitle.Name = "cmbtitle";
            this.cmbtitle.Size = new System.Drawing.Size(314, 21);
            this.cmbtitle.TabIndex = 1;
            this.cmbtitle.SelectedIndexChanged += new System.EventHandler(this.cmbtitle_SelectedIndexChanged);
            this.cmbtitle.Click += new System.EventHandler(this.cmbtitle_Click);
            // 
            // dtdate
            // 
            this.dtdate.CustomFormat = "h:mm";
            this.dtdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtdate.Location = new System.Drawing.Point(233, 79);
            this.dtdate.Name = "dtdate";
            this.dtdate.ShowUpDown = true;
            this.dtdate.Size = new System.Drawing.Size(95, 20);
            this.dtdate.TabIndex = 3;
            this.dtdate.Value = new System.DateTime(2014, 6, 4, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(152, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 86;
            this.label2.Text = "Effective Date";
            // 
            // txtshare
            // 
            this.txtshare.BackColor = System.Drawing.Color.White;
            this.txtshare.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtshare.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtshare.Location = new System.Drawing.Point(48, 79);
            this.txtshare.MaxLength = 5;
            this.txtshare.Name = "txtshare";
            this.txtshare.ReadOnly = true;
            this.txtshare.Size = new System.Drawing.Size(72, 20);
            this.txtshare.TabIndex = 2;
            this.txtshare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtshare.TextChanged += new System.EventHandler(this.txtshare_TextChanged);
            this.txtshare.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtshare_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Share";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 82;
            this.label1.Text = "Movie Title";
            // 
            // txtcode
            // 
            this.txtcode.BackColor = System.Drawing.Color.White;
            this.txtcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtcode.Location = new System.Drawing.Point(48, 8);
            this.txtcode.Name = "txtcode";
            this.txtcode.ReadOnly = true;
            this.txtcode.Size = new System.Drawing.Size(136, 20);
            this.txtcode.TabIndex = 0;
            this.txtcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(11, 12);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(32, 13);
            this.Label8.TabIndex = 80;
            this.Label8.Text = "Code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(326, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 24);
            this.label7.TabIndex = 300;
            this.label7.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(326, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 24);
            this.label5.TabIndex = 299;
            this.label5.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(129, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 24);
            this.label13.TabIndex = 296;
            this.label13.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(182, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 24);
            this.label4.TabIndex = 298;
            this.label4.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(119, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 295;
            this.label6.Text = "%";
            // 
            // txtcnt
            // 
            this.txtcnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtcnt.AutoSize = true;
            this.txtcnt.BackColor = System.Drawing.Color.Transparent;
            this.txtcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcnt.ForeColor = System.Drawing.Color.Black;
            this.txtcnt.Location = new System.Drawing.Point(887, 357);
            this.txtcnt.Name = "txtcnt";
            this.txtcnt.Size = new System.Drawing.Size(38, 13);
            this.txtcnt.TabIndex = 307;
            this.txtcnt.Text = "Count:";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DimGray;
            this.label18.Location = new System.Drawing.Point(900, 473);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 9);
            this.label18.TabIndex = 306;
            this.label18.Text = "required information";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(886, 469);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 24);
            this.label17.TabIndex = 305;
            this.label17.Text = "*";
            // 
            // btnsearch
            // 
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.Location = new System.Drawing.Point(88, 11);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(65, 97);
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
            this.btnclear.Location = new System.Drawing.Point(10, 11);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(65, 97);
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.dgvMovies);
            this.groupBox2.Controls.Add(this.kryptonButton1);
            this.groupBox2.Location = new System.Drawing.Point(2, -3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 324);
            this.groupBox2.TabIndex = 311;
            this.groupBox2.TabStop = false;
            // 
            // dgvMovies
            // 
            this.dgvMovies.AllowUserToAddRows = false;
            this.dgvMovies.AllowUserToDeleteRows = false;
            this.dgvMovies.AllowUserToResizeRows = false;
            this.dgvMovies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMovies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMovies.Location = new System.Drawing.Point(6, 12);
            this.dgvMovies.Name = "dgvMovies";
            this.dgvMovies.RowHeadersVisible = false;
            this.dgvMovies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMovies.Size = new System.Drawing.Size(345, 306);
            this.dgvMovies.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvMovies.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvMovies.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvMovies.TabIndex = 1;
            this.dgvMovies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMovies_CellClick);
            this.dgvMovies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMovies_CellContentClick);
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.kryptonButton1.Location = new System.Drawing.Point(23, 272);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(48, 46);
            this.kryptonButton1.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.kryptonButton1.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.kryptonButton1.StateCommon.Back.ColorAngle = 20F;
            this.kryptonButton1.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.kryptonButton1.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.kryptonButton1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateCommon.Border.Rounding = 1;
            this.kryptonButton1.StateCommon.Border.Width = 3;
            this.kryptonButton1.StateCommon.Content.AdjacentGap = 10;
            this.kryptonButton1.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonButton1.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.kryptonButton1.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton1.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonButton1.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.kryptonButton1.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonButton1.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonButton1.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.kryptonButton1.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.kryptonButton1.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.kryptonButton1.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonButton1.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.kryptonButton1.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonButton1.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonButton1.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.kryptonButton1.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.kryptonButton1.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonButton1.StateTracking.Border.Rounding = 1;
            this.kryptonButton1.StateTracking.Border.Width = 3;
            this.kryptonButton1.TabIndex = 310;
            this.kryptonButton1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonButton1.Values.Image")));
            this.kryptonButton1.Values.Text = "";
            this.kryptonButton1.Visible = false;
            // 
            // grpfilter
            // 
            this.grpfilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpfilter.Location = new System.Drawing.Point(609, 357);
            this.grpfilter.Name = "grpfilter";
            // 
            // grpfilter.Panel
            // 
            this.grpfilter.Panel.Controls.Add(this.btnclear);
            this.grpfilter.Panel.Controls.Add(this.btnsearch);
            this.grpfilter.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.kryptonGroup1_Panel_Paint);
            this.grpfilter.Size = new System.Drawing.Size(174, 127);
            this.grpfilter.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpfilter.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpfilter.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpfilter.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpfilter.StateCommon.Border.Rounding = 5;
            this.grpfilter.StateCommon.Border.Width = 2;
            this.grpfilter.TabIndex = 312;
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Location = new System.Drawing.Point(10, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 97);
            this.btnAdd.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnAdd.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnAdd.StateCommon.Back.ColorAngle = 50F;
            this.btnAdd.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnAdd.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnAdd.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnAdd.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAdd.StateCommon.Border.Rounding = 1;
            this.btnAdd.StateCommon.Border.Width = 3;
            this.btnAdd.StateCommon.Content.AdjacentGap = 0;
            this.btnAdd.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAdd.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAdd.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnAdd.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnAdd.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnAdd.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAdd.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAdd.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAdd.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnAdd.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAdd.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAdd.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAdd.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnAdd.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnAdd.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnAdd.StateTracking.Border.Rounding = 2;
            this.btnAdd.StateTracking.Border.Width = 2;
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Values.Image")));
            this.btnAdd.Values.Text = "new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Location = new System.Drawing.Point(164, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 97);
            this.btnDelete.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnDelete.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnDelete.StateCommon.Back.ColorAngle = 50F;
            this.btnDelete.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnDelete.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnDelete.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnDelete.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDelete.StateCommon.Border.Rounding = 1;
            this.btnDelete.StateCommon.Border.Width = 3;
            this.btnDelete.StateCommon.Content.AdjacentGap = 0;
            this.btnDelete.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDelete.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDelete.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnDelete.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnDelete.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnDelete.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDelete.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDelete.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDelete.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnDelete.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnDelete.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDelete.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDelete.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelete.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnDelete.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnDelete.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDelete.StateTracking.Border.Rounding = 2;
            this.btnDelete.StateTracking.Border.Width = 2;
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Values.Image = global::aZynEManager.Properties.Resources.buttondelete;
            this.btnDelete.Values.Text = "remove";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Location = new System.Drawing.Point(87, 11);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(65, 97);
            this.btnEdit.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnEdit.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnEdit.StateCommon.Back.ColorAngle = 50F;
            this.btnEdit.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnEdit.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnEdit.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnEdit.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEdit.StateCommon.Border.Rounding = 1;
            this.btnEdit.StateCommon.Border.Width = 3;
            this.btnEdit.StateCommon.Content.AdjacentGap = 0;
            this.btnEdit.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEdit.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnEdit.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnEdit.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnEdit.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnEdit.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnEdit.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnEdit.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEdit.StateCommon.Content.Padding = new System.Windows.Forms.Padding(1, 8, 1, 1);
            this.btnEdit.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnEdit.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEdit.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEdit.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEdit.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnEdit.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnEdit.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnEdit.StateTracking.Border.Rounding = 2;
            this.btnEdit.StateTracking.Border.Width = 2;
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Values.Image = global::aZynEManager.Properties.Resources.buttonedit1;
            this.btnEdit.Values.Text = "edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // grpcontrol
            // 
            this.grpcontrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpcontrol.Location = new System.Drawing.Point(360, 357);
            this.grpcontrol.Name = "grpcontrol";
            // 
            // grpcontrol.Panel
            // 
            this.grpcontrol.Panel.Controls.Add(this.btnAdd);
            this.grpcontrol.Panel.Controls.Add(this.btnEdit);
            this.grpcontrol.Panel.Controls.Add(this.btnDelete);
            this.grpcontrol.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Size = new System.Drawing.Size(248, 127);
            this.grpcontrol.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpcontrol.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpcontrol.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpcontrol.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpcontrol.StateCommon.Border.Rounding = 5;
            this.grpcontrol.StateCommon.Border.Width = 2;
            this.grpcontrol.TabIndex = 313;
            // 
            // frmProdShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 487);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtcnt);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.grpValues);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpfilter);
            this.Controls.Add(this.grpcontrol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProdShare";
            this.Opacity = 0.98D;
            this.Text = "Film Sharing";
            this.Load += new System.EventHandler(this.frmProdShare_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.grpValues.Panel.ResumeLayout(false);
            this.grpValues.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpValues)).EndInit();
            this.grpValues.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovies)).EndInit();
            this.grpfilter.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).EndInit();
            this.grpfilter.ResumeLayout(false);
            this.grpcontrol.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).EndInit();
            this.grpcontrol.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpValues;
        internal System.Windows.Forms.Label label1;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtcode;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label txtcnt;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.Label label2;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtshare;
        internal System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear;
        private System.Windows.Forms.DateTimePicker dtdate;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ComboBox cmbtitle;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        private System.Windows.Forms.GroupBox groupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvMovies;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear2;
        private System.Windows.Forms.CheckBox cbxfilter;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpfilter;
        internal System.Windows.Forms.ComboBox cmbDate;
        private System.Windows.Forms.CheckBox cbxdate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpcontrol;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtdaycnt;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}