namespace aZynEManager
{
    partial class frmMainUtility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainUtility));
            this.btnPatrons = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClass = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDistributor = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRating = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnTitle = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnselect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCinema = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOrdinance = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSurcharge = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.pnlClose = new System.Windows.Forms.Panel();
            this.linklabelManageEwallets = new ComponentFactory.Krypton.Toolkit.KryptonLinkLabel();
            this.SuspendLayout();
            // 
            // btnPatrons
            // 
            this.btnPatrons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatrons.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPatrons.Location = new System.Drawing.Point(10, 105);
            this.btnPatrons.Name = "btnPatrons";
            this.btnPatrons.Size = new System.Drawing.Size(283, 60);
            this.btnPatrons.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnPatrons.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnPatrons.StateCommon.Back.ColorAngle = 20F;
            this.btnPatrons.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnPatrons.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnPatrons.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPatrons.StateCommon.Border.Rounding = 1;
            this.btnPatrons.StateCommon.Border.Width = 3;
            this.btnPatrons.StateCommon.Content.AdjacentGap = 10;
            this.btnPatrons.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPatrons.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatrons.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnPatrons.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnPatrons.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatrons.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnPatrons.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPatrons.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPatrons.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatrons.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnPatrons.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnPatrons.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnPatrons.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatrons.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnPatrons.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPatrons.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPatrons.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnPatrons.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnPatrons.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPatrons.StateTracking.Border.Rounding = 1;
            this.btnPatrons.StateTracking.Border.Width = 3;
            this.btnPatrons.TabIndex = 36;
            this.btnPatrons.Values.ExtraText = "group of \r\npatronizers";
            this.btnPatrons.Values.Image = global::aZynEManager.Properties.Resources.icon_patron;
            this.btnPatrons.Values.Text = "Patrons";
            this.btnPatrons.Click += new System.EventHandler(this.btnPatrons_Click);
            // 
            // btnClass
            // 
            this.btnClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClass.Location = new System.Drawing.Point(10, 291);
            this.btnClass.Name = "btnClass";
            this.btnClass.Size = new System.Drawing.Size(283, 60);
            this.btnClass.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnClass.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnClass.StateCommon.Back.ColorAngle = 20F;
            this.btnClass.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnClass.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnClass.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnClass.StateCommon.Border.Rounding = 1;
            this.btnClass.StateCommon.Border.Width = 3;
            this.btnClass.StateCommon.Content.AdjacentGap = 10;
            this.btnClass.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnClass.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnClass.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnClass.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnClass.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClass.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnClass.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnClass.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnClass.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnClass.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnClass.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnClass.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnClass.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClass.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnClass.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnClass.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnClass.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnClass.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnClass.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnClass.StateTracking.Border.Rounding = 1;
            this.btnClass.StateTracking.Border.Width = 3;
            this.btnClass.TabIndex = 35;
            this.btnClass.Values.ExtraText = "original film\r\nclassification";
            this.btnClass.Values.Image = global::aZynEManager.Properties.Resources.icon_movie_class;
            this.btnClass.Values.Text = "Classification";
            this.btnClass.Click += new System.EventHandler(this.btnClass_Click);
            // 
            // btnDistributor
            // 
            this.btnDistributor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDistributor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDistributor.Location = new System.Drawing.Point(10, 229);
            this.btnDistributor.Name = "btnDistributor";
            this.btnDistributor.Size = new System.Drawing.Size(283, 60);
            this.btnDistributor.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnDistributor.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnDistributor.StateCommon.Back.ColorAngle = 20F;
            this.btnDistributor.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnDistributor.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnDistributor.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDistributor.StateCommon.Border.Rounding = 1;
            this.btnDistributor.StateCommon.Border.Width = 3;
            this.btnDistributor.StateCommon.Content.AdjacentGap = 10;
            this.btnDistributor.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDistributor.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDistributor.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnDistributor.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnDistributor.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistributor.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnDistributor.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDistributor.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDistributor.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDistributor.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnDistributor.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnDistributor.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnDistributor.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistributor.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnDistributor.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDistributor.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDistributor.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnDistributor.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnDistributor.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnDistributor.StateTracking.Border.Rounding = 1;
            this.btnDistributor.StateTracking.Border.Width = 3;
            this.btnDistributor.TabIndex = 34;
            this.btnDistributor.Values.ExtraText = "list of \r\nproducers";
            this.btnDistributor.Values.Image = global::aZynEManager.Properties.Resources.icon_distributors;
            this.btnDistributor.Values.Text = "Distributor";
            this.btnDistributor.Click += new System.EventHandler(this.btnDistributor_Click);
            // 
            // btnRating
            // 
            this.btnRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRating.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRating.Location = new System.Drawing.Point(10, 167);
            this.btnRating.Name = "btnRating";
            this.btnRating.Size = new System.Drawing.Size(283, 60);
            this.btnRating.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnRating.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnRating.StateCommon.Back.ColorAngle = 20F;
            this.btnRating.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnRating.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnRating.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRating.StateCommon.Border.Rounding = 1;
            this.btnRating.StateCommon.Border.Width = 3;
            this.btnRating.StateCommon.Content.AdjacentGap = 10;
            this.btnRating.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRating.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnRating.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnRating.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnRating.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRating.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnRating.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRating.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRating.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnRating.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnRating.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnRating.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnRating.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRating.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnRating.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRating.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnRating.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnRating.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnRating.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRating.StateTracking.Border.Rounding = 1;
            this.btnRating.StateTracking.Border.Width = 3;
            this.btnRating.TabIndex = 32;
            this.btnRating.Values.ExtraText = "MTRCB \r\nclassification rating";
            this.btnRating.Values.Image = global::aZynEManager.Properties.Resources.icon_mtrcb_ratings;
            this.btnRating.Values.Text = "Rating";
            this.btnRating.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnTitle
            // 
            this.btnTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTitle.Enabled = false;
            this.btnTitle.Location = new System.Drawing.Point(5, 1);
            this.btnTitle.Name = "btnTitle";
            this.btnTitle.Size = new System.Drawing.Size(294, 35);
            this.btnTitle.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.btnTitle.StateCommon.Back.Color2 = System.Drawing.Color.Black;
            this.btnTitle.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnTitle.StateCommon.Content.AdjacentGap = 10;
            this.btnTitle.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnTitle.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnTitle.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnTitle.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTitle.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnTitle.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnTitle.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnTitle.TabIndex = 31;
            this.btnTitle.Values.Text = "Setup Utility";
            // 
            // btnselect
            // 
            this.btnselect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselect.Location = new System.Drawing.Point(23, 260);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(96, 60);
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
            this.btnselect.TabIndex = 37;
            this.btnselect.Values.Text = "unselect";
            this.btnselect.Visible = false;
            // 
            // btnCinema
            // 
            this.btnCinema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCinema.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCinema.Location = new System.Drawing.Point(10, 43);
            this.btnCinema.Name = "btnCinema";
            this.btnCinema.Size = new System.Drawing.Size(283, 60);
            this.btnCinema.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnCinema.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnCinema.StateCommon.Back.ColorAngle = 20F;
            this.btnCinema.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnCinema.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnCinema.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCinema.StateCommon.Border.Rounding = 1;
            this.btnCinema.StateCommon.Border.Width = 3;
            this.btnCinema.StateCommon.Content.AdjacentGap = 10;
            this.btnCinema.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCinema.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCinema.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnCinema.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnCinema.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCinema.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnCinema.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCinema.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCinema.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCinema.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnCinema.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnCinema.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnCinema.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCinema.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnCinema.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCinema.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCinema.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnCinema.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnCinema.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnCinema.StateTracking.Border.Rounding = 1;
            this.btnCinema.StateTracking.Border.Width = 3;
            this.btnCinema.TabIndex = 39;
            this.btnCinema.Values.ExtraText = "records of cinema";
            this.btnCinema.Values.Image = global::aZynEManager.Properties.Resources.icon_cinema;
            this.btnCinema.Values.Text = "Cinema";
            this.btnCinema.Click += new System.EventHandler(this.btnCinema_Click);
            // 
            // btnOrdinance
            // 
            this.btnOrdinance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrdinance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrdinance.Location = new System.Drawing.Point(10, 354);
            this.btnOrdinance.Name = "btnOrdinance";
            this.btnOrdinance.Size = new System.Drawing.Size(283, 60);
            this.btnOrdinance.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnOrdinance.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnOrdinance.StateCommon.Back.ColorAngle = 20F;
            this.btnOrdinance.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnOrdinance.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnOrdinance.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnOrdinance.StateCommon.Border.Rounding = 1;
            this.btnOrdinance.StateCommon.Border.Width = 3;
            this.btnOrdinance.StateCommon.Content.AdjacentGap = 10;
            this.btnOrdinance.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnOrdinance.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnOrdinance.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnOrdinance.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnOrdinance.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrdinance.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnOrdinance.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnOrdinance.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnOrdinance.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnOrdinance.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnOrdinance.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnOrdinance.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnOrdinance.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrdinance.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnOrdinance.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnOrdinance.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnOrdinance.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnOrdinance.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnOrdinance.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnOrdinance.StateTracking.Border.Rounding = 1;
            this.btnOrdinance.StateTracking.Border.Width = 3;
            this.btnOrdinance.TabIndex = 40;
            this.btnOrdinance.Values.ExtraText = "local\r\nordinances";
            this.btnOrdinance.Values.Image = global::aZynEManager.Properties.Resources.ordinance;
            this.btnOrdinance.Values.Text = "Ordinance";
            this.btnOrdinance.Click += new System.EventHandler(this.btnOrdinance_Click);
            // 
            // btnSurcharge
            // 
            this.btnSurcharge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSurcharge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSurcharge.Location = new System.Drawing.Point(10, 417);
            this.btnSurcharge.Name = "btnSurcharge";
            this.btnSurcharge.Size = new System.Drawing.Size(283, 60);
            this.btnSurcharge.StateCommon.Back.Color1 = System.Drawing.Color.WhiteSmoke;
            this.btnSurcharge.StateCommon.Back.Color2 = System.Drawing.Color.Salmon;
            this.btnSurcharge.StateCommon.Back.ColorAngle = 20F;
            this.btnSurcharge.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.btnSurcharge.StateCommon.Border.Color1 = System.Drawing.Color.Salmon;
            this.btnSurcharge.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSurcharge.StateCommon.Border.Rounding = 1;
            this.btnSurcharge.StateCommon.Border.Width = 3;
            this.btnSurcharge.StateCommon.Content.AdjacentGap = 10;
            this.btnSurcharge.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnSurcharge.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnSurcharge.StateCommon.Content.LongText.Color1 = System.Drawing.Color.Black;
            this.btnSurcharge.StateCommon.Content.LongText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnSurcharge.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSurcharge.StateCommon.Content.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.btnSurcharge.StateCommon.Content.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnSurcharge.StateCommon.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnSurcharge.StateCommon.Content.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnSurcharge.StateCommon.Content.Padding = new System.Windows.Forms.Padding(5, 2, -1, -1);
            this.btnSurcharge.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.OrangeRed;
            this.btnSurcharge.StateCommon.Content.ShortText.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Solid;
            this.btnSurcharge.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSurcharge.StateCommon.Content.ShortText.Hint = ComponentFactory.Krypton.Toolkit.PaletteTextHint.AntiAlias;
            this.btnSurcharge.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnSurcharge.StateCommon.Content.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnSurcharge.StateTracking.Border.Color1 = System.Drawing.Color.Black;
            this.btnSurcharge.StateTracking.Border.Color2 = System.Drawing.Color.Black;
            this.btnSurcharge.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnSurcharge.StateTracking.Border.Rounding = 1;
            this.btnSurcharge.StateTracking.Border.Width = 3;
            this.btnSurcharge.TabIndex = 41;
            this.btnSurcharge.Values.ExtraText = "additional cinema\r\ncharges";
            this.btnSurcharge.Values.Image = global::aZynEManager.Properties.Resources.surcharge2;
            this.btnSurcharge.Values.Text = "Surcharge";
            this.btnSurcharge.Click += new System.EventHandler(this.btnSurcharge_Click);
            // 
            // pnlClose
            // 
            this.pnlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlClose.BackColor = System.Drawing.Color.Gray;
            this.pnlClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlClose.BackgroundImage")));
            this.pnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlClose.Location = new System.Drawing.Point(268, 5);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(25, 25);
            this.pnlClose.TabIndex = 33;
            this.pnlClose.Click += new System.EventHandler(this.pnlClose_Click);
            this.pnlClose.MouseLeave += new System.EventHandler(this.pnlClose_MouseLeave);
            this.pnlClose.MouseHover += new System.EventHandler(this.pnlClose_MouseHover);
            // 
            // linklabelManageEwallets
            // 
            this.linklabelManageEwallets.AutoSize = false;
            this.linklabelManageEwallets.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linklabelManageEwallets.LinkBehavior = ComponentFactory.Krypton.Toolkit.KryptonLinkBehavior.HoverUnderline;
            this.linklabelManageEwallets.Location = new System.Drawing.Point(0, 478);
            this.linklabelManageEwallets.Name = "linklabelManageEwallets";
            this.linklabelManageEwallets.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.linklabelManageEwallets.Size = new System.Drawing.Size(304, 20);
            this.linklabelManageEwallets.TabIndex = 288;
            this.linklabelManageEwallets.Values.Text = "Manage eWallets";
            this.linklabelManageEwallets.LinkClicked += new System.EventHandler(this.linklabelManageEwallets_LinkClicked);
            // 
            // frmMainUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 498);
            this.Controls.Add(this.linklabelManageEwallets);
            this.Controls.Add(this.btnSurcharge);
            this.Controls.Add(this.btnOrdinance);
            this.Controls.Add(this.btnCinema);
            this.Controls.Add(this.btnPatrons);
            this.Controls.Add(this.btnClass);
            this.Controls.Add(this.btnDistributor);
            this.Controls.Add(this.pnlClose);
            this.Controls.Add(this.btnRating);
            this.Controls.Add(this.btnTitle);
            this.Controls.Add(this.btnselect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainUtility";
            this.Text = "System Utility";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPatrons;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClass;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDistributor;
        private System.Windows.Forms.Panel pnlClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRating;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTitle;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnselect;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCinema;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOrdinance;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSurcharge;
        private ComponentFactory.Krypton.Toolkit.KryptonLinkLabel linklabelManageEwallets;
    }
}