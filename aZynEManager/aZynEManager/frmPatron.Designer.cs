namespace aZynEManager
{
    partial class frmPatron
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatron));
            this.cbxlgu = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.txtcnt = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnsearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpcontrol = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpfilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnclear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtposition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtlgu = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.cbxpromo = new System.Windows.Forms.CheckBox();
            this.lbllgu = new System.Windows.Forms.Label();
            this.cbxproducer = new System.Windows.Forms.CheckBox();
            this.cbxgross = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btncolor = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
            this.cbxcultural = new System.Windows.Forms.CheckBox();
            this.cbxamusement = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtprice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtname = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).BeginInit();
            this.grpcontrol.Panel.SuspendLayout();
            this.grpcontrol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).BeginInit();
            this.grpfilter.Panel.SuspendLayout();
            this.grpfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // cbxlgu
            // 
            this.cbxlgu.AutoSize = true;
            this.cbxlgu.BackColor = System.Drawing.Color.Transparent;
            this.cbxlgu.ForeColor = System.Drawing.Color.White;
            this.cbxlgu.Location = new System.Drawing.Point(73, 180);
            this.cbxlgu.Name = "cbxlgu";
            this.cbxlgu.Size = new System.Drawing.Size(94, 17);
            this.cbxlgu.TabIndex = 7;
            this.cbxlgu.Text = "With LGU Tax";
            this.cbxlgu.UseVisualStyleBackColor = false;
            this.cbxlgu.CheckedChanged += new System.EventHandler(this.cbxlgu_CheckedChanged);
            // 
            // btnselect
            // 
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(83, 301);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(46, 46);
            this.btnselect.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnselect.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnselect.StateCommon.Back.ColorAngle = 50F;
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
            this.btnselect.StateTracking.Border.Rounding = 3;
            this.btnselect.StateTracking.Border.Width = 1;
            this.btnselect.TabIndex = 308;
            this.toolTip1.SetToolTip(this.btnselect, "Apply");
            this.btnselect.Values.Image = global::aZynEManager.Properties.Resources.buttonapply;
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Location = new System.Drawing.Point(3, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 407);
            this.groupBox1.TabIndex = 311;
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
            this.dgvResult.Location = new System.Drawing.Point(5, 12);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(522, 388);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.TabIndex = 31;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            this.dgvResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellContentClick);
            // 
            // txtcnt
            // 
            this.txtcnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtcnt.AutoSize = true;
            this.txtcnt.BackColor = System.Drawing.Color.Transparent;
            this.txtcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcnt.ForeColor = System.Drawing.Color.Black;
            this.txtcnt.Location = new System.Drawing.Point(539, 387);
            this.txtcnt.Name = "txtcnt";
            this.txtcnt.Size = new System.Drawing.Size(38, 13);
            this.txtcnt.TabIndex = 314;
            this.txtcnt.Text = "Count:";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DimGray;
            this.label18.Location = new System.Drawing.Point(550, 300);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 9);
            this.label18.TabIndex = 310;
            this.label18.Text = "required information";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(103, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 15);
            this.label11.TabIndex = 275;
            this.label11.Text = "Taxes";
            // 
            // btnsearch
            // 
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.Location = new System.Drawing.Point(87, 6);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(59, 87);
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
            this.btnsearch.TabIndex = 1;
            this.btnsearch.Values.Image = global::aZynEManager.Properties.Resources.searchblue;
            this.btnsearch.Values.Text = "search";
            // 
            // grpcontrol
            // 
            this.grpcontrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpcontrol.Location = new System.Drawing.Point(625, 298);
            this.grpcontrol.Name = "grpcontrol";
            // 
            // grpcontrol.Panel
            // 
            this.grpcontrol.Panel.Controls.Add(this.btnEdit);
            this.grpcontrol.Panel.Controls.Add(this.btnDelete);
            this.grpcontrol.Panel.Controls.Add(this.btnAdd);
            this.grpcontrol.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Size = new System.Drawing.Size(226, 107);
            this.grpcontrol.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpcontrol.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpcontrol.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpcontrol.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.grpcontrol.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpcontrol.StateCommon.Border.Rounding = 5;
            this.grpcontrol.StateCommon.Border.Width = 2;
            this.grpcontrol.TabIndex = 312;
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Location = new System.Drawing.Point(77, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(59, 87);
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
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Location = new System.Drawing.Point(149, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 87);
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
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Location = new System.Drawing.Point(9, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 87);
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
            // grpfilter
            // 
            this.grpfilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpfilter.Location = new System.Drawing.Point(683, 298);
            this.grpfilter.Name = "grpfilter";
            // 
            // grpfilter.Panel
            // 
            this.grpfilter.Panel.Controls.Add(this.btnsearch);
            this.grpfilter.Panel.Controls.Add(this.btnclear);
            this.grpfilter.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpfilter.Size = new System.Drawing.Size(168, 107);
            this.grpfilter.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpfilter.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpfilter.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpfilter.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.grpfilter.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpfilter.StateCommon.Border.Rounding = 5;
            this.grpfilter.StateCommon.Border.Width = 2;
            this.grpfilter.TabIndex = 313;
            // 
            // btnclear
            // 
            this.btnclear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclear.Location = new System.Drawing.Point(12, 6);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(64, 87);
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
            this.btnclear.TabIndex = 0;
            this.btnclear.Values.Image = global::aZynEManager.Properties.Resources.simple;
            this.btnclear.Values.Text = "clear";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(8, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 272;
            this.label6.Text = "Seat Code";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(536, 296);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 24);
            this.label17.TabIndex = 309;
            this.label17.Text = "*";
            // 
            // txtposition
            // 
            this.txtposition.BackColor = System.Drawing.Color.White;
            this.txtposition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtposition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtposition.Location = new System.Drawing.Point(224, 81);
            this.txtposition.Name = "txtposition";
            this.txtposition.Size = new System.Drawing.Size(72, 20);
            this.txtposition.TabIndex = 4;
            this.txtposition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtposition_KeyPress);
            // 
            // txtlgu
            // 
            this.txtlgu.BackColor = System.Drawing.Color.White;
            this.txtlgu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlgu.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtlgu.Location = new System.Drawing.Point(224, 177);
            this.txtlgu.Name = "txtlgu";
            this.txtlgu.Size = new System.Drawing.Size(72, 20);
            this.txtlgu.TabIndex = 8;
            this.txtlgu.TextChanged += new System.EventHandler(this.txtlgu_TextChanged);
            this.txtlgu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtlgu_KeyPress);
            this.txtlgu.Leave += new System.EventHandler(this.txtlgu_Leave);
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroup1.Location = new System.Drawing.Point(538, 6);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.cbxpromo);
            this.kryptonGroup1.Panel.Controls.Add(this.lbllgu);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxproducer);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxgross);
            this.kryptonGroup1.Panel.Controls.Add(this.label7);
            this.kryptonGroup1.Panel.Controls.Add(this.pictureBox3);
            this.kryptonGroup1.Panel.Controls.Add(this.pictureBox4);
            this.kryptonGroup1.Panel.Controls.Add(this.label4);
            this.kryptonGroup1.Panel.Controls.Add(this.btncolor);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxcultural);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxamusement);
            this.kryptonGroup1.Panel.Controls.Add(this.cbxlgu);
            this.kryptonGroup1.Panel.Controls.Add(this.label11);
            this.kryptonGroup1.Panel.Controls.Add(this.pictureBox1);
            this.kryptonGroup1.Panel.Controls.Add(this.label6);
            this.kryptonGroup1.Panel.Controls.Add(this.txtposition);
            this.kryptonGroup1.Panel.Controls.Add(this.txtlgu);
            this.kryptonGroup1.Panel.Controls.Add(this.pictureBox2);
            this.kryptonGroup1.Panel.Controls.Add(this.label3);
            this.kryptonGroup1.Panel.Controls.Add(this.txtprice);
            this.kryptonGroup1.Panel.Controls.Add(this.txtname);
            this.kryptonGroup1.Panel.Controls.Add(this.label1);
            this.kryptonGroup1.Panel.Controls.Add(this.txtcode);
            this.kryptonGroup1.Panel.Controls.Add(this.Label8);
            this.kryptonGroup1.Panel.Controls.Add(this.label14);
            this.kryptonGroup1.Panel.Controls.Add(this.label16);
            this.kryptonGroup1.Panel.Controls.Add(this.label15);
            this.kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Size = new System.Drawing.Size(315, 289);
            this.kryptonGroup1.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonGroup1.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonGroup1.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup1.StateCommon.Border.Rounding = 5;
            this.kryptonGroup1.StateCommon.Border.Width = 3;
            this.kryptonGroup1.TabIndex = 0;
            // 
            // cbxpromo
            // 
            this.cbxpromo.AutoSize = true;
            this.cbxpromo.BackColor = System.Drawing.Color.Transparent;
            this.cbxpromo.ForeColor = System.Drawing.Color.White;
            this.cbxpromo.Location = new System.Drawing.Point(180, 57);
            this.cbxpromo.Name = "cbxpromo";
            this.cbxpromo.Size = new System.Drawing.Size(93, 17);
            this.cbxpromo.TabIndex = 300;
            this.cbxpromo.Text = "Is a Promotion";
            this.cbxpromo.UseVisualStyleBackColor = false;
            // 
            // lbllgu
            // 
            this.lbllgu.AutoSize = true;
            this.lbllgu.BackColor = System.Drawing.Color.Transparent;
            this.lbllgu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllgu.ForeColor = System.Drawing.Color.White;
            this.lbllgu.Location = new System.Drawing.Point(177, 181);
            this.lbllgu.Name = "lbllgu";
            this.lbllgu.Size = new System.Drawing.Size(34, 13);
            this.lbllgu.TabIndex = 299;
            this.lbllgu.Text = "Value";
            // 
            // cbxproducer
            // 
            this.cbxproducer.AutoSize = true;
            this.cbxproducer.BackColor = System.Drawing.Color.Transparent;
            this.cbxproducer.ForeColor = System.Drawing.Color.White;
            this.cbxproducer.Location = new System.Drawing.Point(73, 255);
            this.cbxproducer.Name = "cbxproducer";
            this.cbxproducer.Size = new System.Drawing.Size(148, 17);
            this.cbxproducer.TabIndex = 10;
            this.cbxproducer.Text = "Disregard Producer Share";
            this.cbxproducer.UseVisualStyleBackColor = false;
            // 
            // cbxgross
            // 
            this.cbxgross.AutoSize = true;
            this.cbxgross.BackColor = System.Drawing.Color.Transparent;
            this.cbxgross.ForeColor = System.Drawing.Color.White;
            this.cbxgross.Location = new System.Drawing.Point(73, 231);
            this.cbxgross.Name = "cbxgross";
            this.cbxgross.Size = new System.Drawing.Size(136, 17);
            this.cbxgross.TabIndex = 9;
            this.cbxgross.Text = "Disregard Gross Margin";
            this.cbxgross.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(103, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 15);
            this.label7.TabIndex = 295;
            this.label7.Text = "Notice";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox3.Location = new System.Drawing.Point(165, 215);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(130, 3);
            this.pictureBox3.TabIndex = 294;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox4.Location = new System.Drawing.Point(10, 215);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(80, 3);
            this.pictureBox4.TabIndex = 293;
            this.pictureBox4.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(177, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 292;
            this.label4.Text = "Position";
            // 
            // btncolor
            // 
            this.btncolor.DropDownOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Right;
            this.btncolor.Location = new System.Drawing.Point(72, 77);
            this.btncolor.Name = "btncolor";
            this.btncolor.SelectedColor = System.Drawing.Color.CornflowerBlue;
            this.btncolor.SelectedRect = new System.Drawing.Rectangle(0, 0, 16, 16);
            this.btncolor.Size = new System.Drawing.Size(90, 25);
            this.btncolor.TabIndex = 3;
            this.btncolor.Values.Image = global::aZynEManager.Properties.Resources.colorpicker1;
            this.btncolor.SelectedColorChanged += new System.EventHandler<ComponentFactory.Krypton.Toolkit.ColorEventArgs>(this.btncolor_SelectedColorChanged);
            // 
            // cbxcultural
            // 
            this.cbxcultural.AutoSize = true;
            this.cbxcultural.BackColor = System.Drawing.Color.Transparent;
            this.cbxcultural.ForeColor = System.Drawing.Color.White;
            this.cbxcultural.Location = new System.Drawing.Point(73, 156);
            this.cbxcultural.Name = "cbxcultural";
            this.cbxcultural.Size = new System.Drawing.Size(107, 17);
            this.cbxcultural.TabIndex = 6;
            this.cbxcultural.Text = "With Cultural Tax";
            this.cbxcultural.UseVisualStyleBackColor = false;
            // 
            // cbxamusement
            // 
            this.cbxamusement.AutoSize = true;
            this.cbxamusement.BackColor = System.Drawing.Color.Transparent;
            this.cbxamusement.ForeColor = System.Drawing.Color.White;
            this.cbxamusement.Location = new System.Drawing.Point(73, 132);
            this.cbxamusement.Name = "cbxamusement";
            this.cbxamusement.Size = new System.Drawing.Size(127, 17);
            this.cbxamusement.TabIndex = 5;
            this.cbxamusement.Text = "With Amusement Tax";
            this.cbxamusement.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(165, 118);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 3);
            this.pictureBox1.TabIndex = 274;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(10, 118);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(80, 3);
            this.pictureBox2.TabIndex = 263;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 87;
            this.label3.Text = "Unit Price";
            // 
            // txtprice
            // 
            this.txtprice.BackColor = System.Drawing.Color.White;
            this.txtprice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtprice.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtprice.Location = new System.Drawing.Point(73, 53);
            this.txtprice.MaxLength = 3;
            this.txtprice.Name = "txtprice";
            this.txtprice.Size = new System.Drawing.Size(66, 20);
            this.txtprice.TabIndex = 2;
            this.txtprice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtprice.TextChanged += new System.EventHandler(this.txtprice_TextChanged);
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.White;
            this.txtname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtname.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtname.Location = new System.Drawing.Point(73, 29);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(223, 20);
            this.txtname.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 11);
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
            this.txtcode.Location = new System.Drawing.Point(73, 5);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(107, 20);
            this.txtcode.TabIndex = 0;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(8, 34);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(35, 13);
            this.Label8.TabIndex = 80;
            this.Label8.Text = "Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(178, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 24);
            this.label14.TabIndex = 281;
            this.label14.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(137, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 24);
            this.label16.TabIndex = 283;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(293, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 24);
            this.label15.TabIndex = 282;
            this.label15.Text = "*";
            // 
            // frmPatron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 407);
            this.Controls.Add(this.grpcontrol);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtcnt);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.grpfilter);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.kryptonGroup1);
            this.Controls.Add(this.btnselect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPatron";
            this.Opacity = 0.98D;
            this.Text = "Patron Information";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.grpcontrol.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).EndInit();
            this.grpcontrol.ResumeLayout(false);
            this.grpfilter.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).EndInit();
            this.grpfilter.ResumeLayout(false);
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbxlgu;
        private System.Windows.Forms.ToolTip toolTip1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        internal System.Windows.Forms.Label txtcnt;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label11;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpcontrol;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpfilter;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear;
        private System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label17;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtposition;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtlgu;
        private System.Windows.Forms.PictureBox pictureBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        internal System.Windows.Forms.Label label3;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprice;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtname;
        internal System.Windows.Forms.Label label1;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtcode;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        internal System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonColorButton btncolor;
        private System.Windows.Forms.CheckBox cbxcultural;
        private System.Windows.Forms.CheckBox cbxamusement;
        private System.Windows.Forms.CheckBox cbxproducer;
        private System.Windows.Forms.CheckBox cbxgross;
        internal System.Windows.Forms.Label lbllgu;
        private System.Windows.Forms.CheckBox cbxpromo;
    }
}