namespace aZynEManager
{
    partial class frmClassification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClassification));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpfilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnsearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnclear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpcontrol = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtcnt = new System.Windows.Forms.Label();
            this.grpValues = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.txtdesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtname = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).BeginInit();
            this.grpfilter.Panel.SuspendLayout();
            this.grpfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).BeginInit();
            this.grpcontrol.Panel.SuspendLayout();
            this.grpcontrol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpValues)).BeginInit();
            this.grpValues.Panel.SuspendLayout();
            this.grpValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvResult);
            this.groupBox1.Controls.Add(this.btnselect);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 276);
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
            this.dgvResult.Location = new System.Drawing.Point(5, 12);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(350, 257);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.TabIndex = 31;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            this.dgvResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellContentClick);
            // 
            // btnselect
            // 
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(7, 365);
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
            this.btnselect.TabIndex = 298;
            this.btnselect.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnselect.Values.Image")));
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // grpfilter
            // 
            this.grpfilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpfilter.Location = new System.Drawing.Point(211, 340);
            this.grpfilter.Name = "grpfilter";
            // 
            // grpfilter.Panel
            // 
            this.grpfilter.Panel.Controls.Add(this.btnsearch);
            this.grpfilter.Panel.Controls.Add(this.btnclear);
            this.grpfilter.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpfilter.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpfilter.Size = new System.Drawing.Size(152, 103);
            this.grpfilter.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpfilter.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpfilter.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpfilter.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.grpfilter.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpfilter.StateCommon.Border.Rounding = 5;
            this.grpfilter.StateCommon.Border.Width = 2;
            this.grpfilter.TabIndex = 297;
            // 
            // btnsearch
            // 
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.Location = new System.Drawing.Point(75, 6);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(59, 82);
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
            // 
            // btnclear
            // 
            this.btnclear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclear.Location = new System.Drawing.Point(9, 6);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(55, 82);
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
            // 
            // grpcontrol
            // 
            this.grpcontrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpcontrol.Location = new System.Drawing.Point(137, 340);
            this.grpcontrol.Name = "grpcontrol";
            // 
            // grpcontrol.Panel
            // 
            this.grpcontrol.Panel.Controls.Add(this.btnEdit);
            this.grpcontrol.Panel.Controls.Add(this.btnAdd);
            this.grpcontrol.Panel.Controls.Add(this.btnDelete);
            this.grpcontrol.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpcontrol.Size = new System.Drawing.Size(226, 103);
            this.grpcontrol.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpcontrol.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpcontrol.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpcontrol.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.grpcontrol.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpcontrol.StateCommon.Border.Rounding = 5;
            this.grpcontrol.StateCommon.Border.Width = 2;
            this.grpcontrol.TabIndex = 296;
            // 
            // btnEdit
            // 
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Location = new System.Drawing.Point(78, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(59, 82);
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
            this.btnEdit.TabIndex = 32;
            this.btnEdit.Values.Image = global::aZynEManager.Properties.Resources.buttonedit1;
            this.btnEdit.Values.Text = "edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Location = new System.Drawing.Point(9, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 82);
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
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Values.Image")));
            this.btnAdd.Values.Text = "new";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Location = new System.Drawing.Point(151, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 82);
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
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Values.Image = global::aZynEManager.Properties.Resources.buttondelete;
            this.btnDelete.Values.Text = "remove";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtcnt
            // 
            this.txtcnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtcnt.AutoSize = true;
            this.txtcnt.BackColor = System.Drawing.Color.Transparent;
            this.txtcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcnt.ForeColor = System.Drawing.Color.Black;
            this.txtcnt.Location = new System.Drawing.Point(9, 343);
            this.txtcnt.Name = "txtcnt";
            this.txtcnt.Size = new System.Drawing.Size(38, 13);
            this.txtcnt.TabIndex = 298;
            this.txtcnt.Text = "Count:";
            // 
            // grpValues
            // 
            this.grpValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpValues.Location = new System.Drawing.Point(3, 277);
            this.grpValues.Name = "grpValues";
            // 
            // grpValues.Panel
            // 
            this.grpValues.Panel.Controls.Add(this.txtdesc);
            this.grpValues.Panel.Controls.Add(this.label1);
            this.grpValues.Panel.Controls.Add(this.txtname);
            this.grpValues.Panel.Controls.Add(this.Label8);
            this.grpValues.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.grpValues.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.grpValues.Size = new System.Drawing.Size(362, 63);
            this.grpValues.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.grpValues.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.grpValues.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.grpValues.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.grpValues.StateCommon.Border.Rounding = 5;
            this.grpValues.StateCommon.Border.Width = 2;
            this.grpValues.TabIndex = 299;
            // 
            // txtdesc
            // 
            this.txtdesc.BackColor = System.Drawing.Color.White;
            this.txtdesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdesc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtdesc.Location = new System.Drawing.Point(48, 30);
            this.txtdesc.Name = "txtdesc";
            this.txtdesc.Size = new System.Drawing.Size(272, 20);
            this.txtdesc.TabIndex = 81;
            this.txtdesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 82;
            this.label1.Text = "Name";
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.White;
            this.txtname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtname.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtname.Location = new System.Drawing.Point(48, 6);
            this.txtname.Name = "txtname";
            this.txtname.ReadOnly = true;
            this.txtname.Size = new System.Drawing.Size(150, 20);
            this.txtname.TabIndex = 79;
            this.txtname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(7, 12);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(32, 13);
            this.Label8.TabIndex = 80;
            this.Label8.Text = "Code";
            this.Label8.Click += new System.EventHandler(this.Label8_Click);
            // 
            // frmClassification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 444);
            this.Controls.Add(this.grpcontrol);
            this.Controls.Add(this.grpValues);
            this.Controls.Add(this.txtcnt);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpfilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(375, 600);
            this.Name = "frmClassification";
            this.Opacity = 0.98D;
            this.Text = "Movie Classification";
            this.Load += new System.EventHandler(this.frmClassification_Load);
            this.Resize += new System.EventHandler(this.frmClassification_Resize);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.grpfilter.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpfilter)).EndInit();
            this.grpfilter.ResumeLayout(false);
            this.grpcontrol.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpcontrol)).EndInit();
            this.grpcontrol.ResumeLayout(false);
            this.grpValues.Panel.ResumeLayout(false);
            this.grpValues.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpValues)).EndInit();
            this.grpValues.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpfilter;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnsearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnclear;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpcontrol;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        internal System.Windows.Forms.Label txtcnt;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpValues;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtdesc;
        internal System.Windows.Forms.Label label1;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtname;
        internal System.Windows.Forms.Label Label8;
    }
}