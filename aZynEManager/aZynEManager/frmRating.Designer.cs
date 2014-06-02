namespace aZynEManager
{
    partial class frmRating
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
            this.txtname = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.txtdesc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnApply = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.White;
            this.txtname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtname.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtname.Location = new System.Drawing.Point(76, 8);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(107, 20);
            this.txtname.TabIndex = 79;
            this.txtname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(7, 14);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(35, 13);
            this.Label8.TabIndex = 80;
            this.Label8.Text = "Name";
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Location = new System.Drawing.Point(2, 3);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.txtdesc);
            this.kryptonGroup1.Panel.Controls.Add(this.label1);
            this.kryptonGroup1.Panel.Controls.Add(this.txtname);
            this.kryptonGroup1.Panel.Controls.Add(this.Label8);
            this.kryptonGroup1.Panel.Margin = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Panel.Padding = new System.Windows.Forms.Padding(3);
            this.kryptonGroup1.Size = new System.Drawing.Size(315, 72);
            this.kryptonGroup1.StateCommon.Back.Color1 = System.Drawing.Color.CornflowerBlue;
            this.kryptonGroup1.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonGroup1.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.kryptonGroup1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup1.StateCommon.Border.Rounding = 5;
            this.kryptonGroup1.StateCommon.Border.Width = 2;
            this.kryptonGroup1.TabIndex = 81;
            // 
            // txtdesc
            // 
            this.txtdesc.BackColor = System.Drawing.Color.White;
            this.txtdesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdesc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtdesc.Location = new System.Drawing.Point(76, 34);
            this.txtdesc.Name = "txtdesc";
            this.txtdesc.Size = new System.Drawing.Size(223, 20);
            this.txtdesc.TabIndex = 81;
            this.txtdesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 82;
            this.label1.Text = "Description";
            // 
            // btnselect
            // 
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(256, 10);
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
            this.btnselect.TabIndex = 84;
            this.toolTip1.SetToolTip(this.btnselect, "Apply");
            this.btnselect.Values.Image = global::aZynEManager.Properties.Resources.buttonapply;
            this.btnselect.Values.Text = "";
            this.btnselect.Visible = false;
            // 
            // btnApply
            // 
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.Location = new System.Drawing.Point(320, 8);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(55, 61);
            this.btnApply.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnApply.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnApply.StateCommon.Back.ColorAngle = 50F;
            this.btnApply.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnApply.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnApply.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnApply.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnApply.StateCommon.Border.Rounding = 1;
            this.btnApply.StateCommon.Border.Width = 3;
            this.btnApply.StateCommon.Content.AdjacentGap = 0;
            this.btnApply.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnApply.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnApply.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnApply.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnApply.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnApply.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnApply.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnApply.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnApply.StateCommon.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnApply.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Black;
            this.btnApply.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnApply.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnApply.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnApply.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnApply.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnApply.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnApply.StateTracking.Border.Rounding = 2;
            this.btnApply.StateTracking.Border.Width = 2;
            this.btnApply.TabIndex = 85;
            this.btnApply.Values.Image = global::aZynEManager.Properties.Resources.buttonapply;
            this.btnApply.Values.Text = "apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Location = new System.Drawing.Point(381, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 61);
            this.btnCancel.StateCommon.Back.Color1 = System.Drawing.Color.Transparent;
            this.btnCancel.StateCommon.Back.Color2 = System.Drawing.Color.Transparent;
            this.btnCancel.StateCommon.Back.ColorAngle = 50F;
            this.btnCancel.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnCancel.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnCancel.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnCancel.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCancel.StateCommon.Border.Rounding = 1;
            this.btnCancel.StateCommon.Border.Width = 3;
            this.btnCancel.StateCommon.Content.AdjacentGap = 0;
            this.btnCancel.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCancel.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCancel.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnCancel.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnCancel.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnCancel.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCancel.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCancel.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCancel.StateCommon.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnCancel.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.Black;
            this.btnCancel.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCancel.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCancel.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.StateTracking.Border.Color1 = System.Drawing.Color.Gray;
            this.btnCancel.StateTracking.Border.Color2 = System.Drawing.Color.Gray;
            this.btnCancel.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCancel.StateTracking.Border.Rounding = 2;
            this.btnCancel.StateTracking.Border.Width = 2;
            this.btnCancel.TabIndex = 86;
            this.btnCancel.Values.Image = global::aZynEManager.Properties.Resources.buttoncancel;
            this.btnCancel.Values.Text = "cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmRating
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 77);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.kryptonGroup1);
            this.Controls.Add(this.btnselect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRating";
            this.Text = "Add MTRCB Rating";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRating_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TextBox txtname;
        internal System.Windows.Forms.Label Label8;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        internal System.Windows.Forms.TextBox txtdesc;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnApply;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
    }
}