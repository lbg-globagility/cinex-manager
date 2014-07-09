using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aZynEManager
{
    public partial class ContextMenuCalendarItem : PopedCotainer
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txttimeend;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtcinema;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txttitle;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtdate;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txttimestart;
        internal ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvResult;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtseat;
        internal ComponentFactory.Krypton.Toolkit.KryptonTextBox txtduration;
        private System.Windows.Forms.Label label4;
    
        public ContextMenuCalendarItem()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvResult = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.txttimestart = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtdate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txttitle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtcinema = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txttimeend = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtseat = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtduration = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.ColumnHeadersVisible = false;
            this.dgvResult.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResult.Location = new System.Drawing.Point(116, 161);
            this.dgvResult.MultiSelect = false;
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(210, 136);
            this.dgvResult.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvResult.StateCommon.HeaderColumn.Content.Color1 = System.Drawing.SystemColors.Highlight;
            this.dgvResult.StateCommon.HeaderColumn.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.dgvResult.TabIndex = 29;
            // 
            // txttimestart
            // 
            this.txttimestart.BackColor = System.Drawing.Color.White;
            this.txttimestart.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txttimestart.Location = new System.Drawing.Point(116, 111);
            this.txttimestart.Multiline = true;
            this.txttimestart.Name = "txttimestart";
            this.txttimestart.ReadOnly = true;
            this.txttimestart.Size = new System.Drawing.Size(104, 23);
            this.txttimestart.TabIndex = 28;
            this.txttimestart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtdate
            // 
            this.txtdate.BackColor = System.Drawing.Color.White;
            this.txtdate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtdate.Location = new System.Drawing.Point(116, 86);
            this.txtdate.Multiline = true;
            this.txtdate.Name = "txtdate";
            this.txtdate.ReadOnly = true;
            this.txtdate.Size = new System.Drawing.Size(151, 23);
            this.txtdate.TabIndex = 27;
            this.txtdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txttitle
            // 
            this.txttitle.BackColor = System.Drawing.Color.White;
            this.txttitle.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txttitle.Location = new System.Drawing.Point(116, 30);
            this.txttitle.Multiline = true;
            this.txttitle.Name = "txttitle";
            this.txttitle.ReadOnly = true;
            this.txttitle.Size = new System.Drawing.Size(210, 54);
            this.txttitle.TabIndex = 26;
            this.txttitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtcinema
            // 
            this.txtcinema.BackColor = System.Drawing.Color.White;
            this.txtcinema.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtcinema.Location = new System.Drawing.Point(116, 5);
            this.txtcinema.Multiline = true;
            this.txtcinema.Name = "txtcinema";
            this.txtcinema.ReadOnly = true;
            this.txtcinema.Size = new System.Drawing.Size(210, 23);
            this.txtcinema.TabIndex = 25;
            this.txtcinema.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txttimeend
            // 
            this.txttimeend.BackColor = System.Drawing.Color.White;
            this.txttimeend.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txttimeend.Location = new System.Drawing.Point(222, 111);
            this.txttimeend.Multiline = true;
            this.txttimeend.Name = "txttimeend";
            this.txttimeend.ReadOnly = true;
            this.txttimeend.Size = new System.Drawing.Size(104, 23);
            this.txttimeend.TabIndex = 24;
            this.txttimeend.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 136);
            this.label6.TabIndex = 16;
            this.label6.Text = "Patrons:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 23);
            this.label5.TabIndex = 15;
            this.label5.Text = "Cinema:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 54);
            this.label4.TabIndex = 14;
            this.label4.Text = "Movie Title:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "     Seat Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "    Showing Time:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Showing Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtseat
            // 
            this.txtseat.BackColor = System.Drawing.Color.White;
            this.txtseat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtseat.Location = new System.Drawing.Point(116, 136);
            this.txtseat.Multiline = true;
            this.txtseat.Name = "txtseat";
            this.txtseat.ReadOnly = true;
            this.txtseat.Size = new System.Drawing.Size(210, 23);
            this.txtseat.TabIndex = 30;
            this.txtseat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtduration
            // 
            this.txtduration.BackColor = System.Drawing.Color.White;
            this.txtduration.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtduration.Location = new System.Drawing.Point(269, 86);
            this.txtduration.Multiline = true;
            this.txtduration.Name = "txtduration";
            this.txtduration.ReadOnly = true;
            this.txtduration.Size = new System.Drawing.Size(57, 23);
            this.txtduration.TabIndex = 31;
            this.txtduration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ContextMenuCalendarItem
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Controls.Add(this.txtduration);
            this.Controls.Add(this.txtseat);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.txttimestart);
            this.Controls.Add(this.txtdate);
            this.Controls.Add(this.txttitle);
            this.Controls.Add(this.txtcinema);
            this.Controls.Add(this.txttimeend);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ContextMenuCalendarItem";
            this.Size = new System.Drawing.Size(331, 302);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
