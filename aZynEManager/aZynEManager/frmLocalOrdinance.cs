using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using ComponentFactory.Krypton.Toolkit;

namespace aZynEManager
{
    public partial class frmLocalOrdinance : Form
    {
        public event ThumbnailImageEventHandler OnImageSizeChanged;
        private ImageViewer m_ActiveImageViewer;
        //private ImageDialog m_ImageDialog;
        bool allowSpace = false;
        frmMain m_frmM;
        clscommon m_clscom;
        int intclear = 0;
        string m_loadpath = @"C:\";
        string m_sfullfilename = String.Empty;

        DataTable m_dt;
        MySqlConnection myconn = new MySqlConnection();

        public frmLocalOrdinance()
        {
            InitializeComponent();
        }

        public void frmInit(frmMain frm, clscommon cls)
        {
            m_frmM = frm;
            m_clscom = cls;
            unselectbutton();

            refreshCntrl();
            setnormal();
        }

        public void refreshCntrl()
        {
            txtordno.Text = "";
            cbxcancel.Checked = false;
            cbxpesos.Checked = true;
            if(cbxpesos.Checked)
                txtsymbol.Text = "P";
            else
                txtsymbol.Text = "%";
            txtval.Text = "";
            txtordno.Focus();
            dtstart.Value = DateTime.Now;
            dtend.Value = DateTime.Now;
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void frmLocalOrdinance_Load(object sender, EventArgs e)
        {
        }

        private void txtlgu_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {


                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
             keyInput.Equals(negativeSign))
            {
                // Decimal separator is OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            else if (this.allowSpace && e.KeyChar == ' ')
            {

            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }
        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }

        public decimal DecimalValue
        {
            get
            {
                return Decimal.Parse(this.Text);
            }
        }

        public bool AllowSpace
        {
            set
            {
                this.allowSpace = value;
            }

            get
            {
                return this.allowSpace;
            }
        }

        private void cbxpesos_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxpesos.Checked)
                txtsymbol.Text = "P";
            else
                txtsymbol.Text = "%";
        }

        private void cbxcancel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxcancel.Checked)
                dtend.Enabled = true;
            else
                dtend.Enabled = false;
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            kpImageViewer1.Image = new Bitmap(kpImageViewer1.PanelWidth,kpImageViewer1.PanelHeight);
            
            kpImageViewer1.InitControl();
            kpImageViewer1.Refresh();
            kpImageViewer1.Invalidate();

            intclear = 1;
            unselectbutton();
        }

        private void btnattach_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (intclear == 0)
            {
                if (kpImageViewer1.Image != null)
                    AddImageII();
            }
        }

        private int ImageSize
        {
            get
            {
                return (32 * (this.trackBarSize.Value + 1));
            }
        }

        delegate void DelegateAddImage(string imageFilename);
        private DelegateAddImage m_AddImageDelegate;

        private void AddImage(string imageFilename)
        {
            // thread safe
            if (this.InvokeRequired)
            {
                this.Invoke(m_AddImageDelegate, imageFilename);
            }
            else
            {
                int size = ImageSize;

                ImageViewer imageViewer = new ImageViewer();
                imageViewer.Dock = DockStyle.Bottom;
                imageViewer.LoadImage(imageFilename, 256, 256);
                imageViewer.Width = size;
                imageViewer.Height = size;
                imageViewer.IsThumbnail = true;
                imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick);

                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                this.flowLayoutPanelMain.Controls.Add(imageViewer);
            }
        }

        private void AddImageII()
        {
            int size = ImageSize;

            ImageViewer imageViewer = new ImageViewer();
            imageViewer.Dock = DockStyle.Bottom;
            imageViewer.LoadImageII(kpImageViewer1.Image, 64, 64);
            imageViewer.Width = size;
            imageViewer.Height = size;
            imageViewer.IsThumbnail = true;
            imageViewer.ImageFullFileName = m_sfullfilename;
            imageViewer.ImageToolTip = new ToolTip();
            imageViewer.ImageToolTip.SetToolTip(imageViewer, "Use the right click to detach this image.");
            imageViewer.MouseClick += new MouseEventHandler(imageViewer1_MouseClick);

            this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

            this.flowLayoutPanelMain.Controls.Add(imageViewer);

            kpImageViewer1.Image = new Bitmap(kpImageViewer1.PanelWidth, kpImageViewer1.PanelHeight);
            kpImageViewer1.InitControl();
            kpImageViewer1.Refresh();
            kpImageViewer1.Invalidate();

            m_sfullfilename = "";
            intclear = 1;

        }

        private void AddImageIII(Image img, string filenm)
        {
            int size = ImageSize;

            ImageViewer imageViewer = new ImageViewer();
            imageViewer.Dock = DockStyle.Bottom;
            imageViewer.LoadImageIII(img, 64, 64);
            imageViewer.Width = size;
            imageViewer.Height = size;
            imageViewer.IsThumbnail = true;
            imageViewer.ImageFullFileName = filenm;
            imageViewer.BlobFileStream = null;// new FileStream(imageFilename, FileMode.Open, FileAccess.Read);
            imageViewer.ImageToolTip = new ToolTip();
            imageViewer.ImageToolTip.SetToolTip(imageViewer, "Use the right click to detach this image.");
            imageViewer.MouseClick += new MouseEventHandler(imageViewer1_MouseClick);

            this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

            this.flowLayoutPanelMain.Controls.Add(imageViewer);

            kpImageViewer1.Image = new Bitmap(kpImageViewer1.PanelWidth, kpImageViewer1.PanelHeight);
            kpImageViewer1.InitControl();
            kpImageViewer1.Refresh();
            kpImageViewer1.Invalidate();

            m_sfullfilename = "";
            intclear = 1;

        }

        private void imageViewer1_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_ActiveImageViewer != null)
            {
                m_ActiveImageViewer.IsActive = false;
            }

            m_ActiveImageViewer = (ImageViewer)sender;
            m_ActiveImageViewer.IsActive = true;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                kpImageViewer1.Image = new Bitmap(m_ActiveImageViewer.ImageFullFileName);
                intclear = 1;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DialogResult ans = MessageBox.Show("Are you sure you want to remove \n\rthis image, continue?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == System.Windows.Forms.DialogResult.Yes)
                {
                    this.flowLayoutPanelMain.Controls.Remove(m_ActiveImageViewer);

                    kpImageViewer1.Image = new Bitmap(kpImageViewer1.PanelWidth, kpImageViewer1.PanelHeight);
                    kpImageViewer1.InitControl();
                    kpImageViewer1.Refresh();
                    kpImageViewer1.Invalidate();

                    m_sfullfilename = "";
                    intclear = 1;
                }
            }
        }

        private void imageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_ActiveImageViewer != null)
            {
                m_ActiveImageViewer.IsActive = false;
            }

            m_ActiveImageViewer = (ImageViewer)sender;
            m_ActiveImageViewer.IsActive = true;

            kpImageViewer1.Image = new Bitmap(m_ActiveImageViewer.Image);
            intclear = 1;
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            this.OnImageSizeChanged(this, new ThumbnailImageEventArgs(ImageSize));
        }

        private void btndetach_Click(object sender, EventArgs e)
        {
            unselectbutton();
            if (intclear > 0)
            {
                if (flowLayoutPanelMain.Controls.Count > 0)
                {

                }
            }
        }

        private void trackBarSize_ValueChanged_1(object sender, EventArgs e)
        {
            if (flowLayoutPanelMain.Controls.Count > 0)
                this.OnImageSizeChanged(this, new ThumbnailImageEventArgs(ImageSize));
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            unselectbutton();
            using (System.Windows.Forms.FileDialog fileDlg = new System.Windows.Forms.OpenFileDialog())
            {
                fileDlg.InitialDirectory = m_loadpath;
                fileDlg.Filter = "Image File (*.bmp;*.gif;*.jpg;*.png;*.tif)|*.bmp;*.gif;*.jpg;*.png;*.tif";

                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    m_loadpath = Path.GetDirectoryName(fileDlg.FileName);
                    kpImageViewer1.Image = new Bitmap(fileDlg.FileName);
                    m_sfullfilename = fileDlg.FileName;
                    intclear = 0;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "ORDINANCE_ADD", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();

            if (btnAdd.Text == "new")
            {
                dgvResult.Enabled = false;

                txtordno.Text = "";
                txtordno.ReadOnly = false;
                txtval.Text = "";
                txtval.ReadOnly = false;

                btnAdd.Text = "save";
                btnAdd.Enabled = true;
                btnAdd.Values.Image = Properties.Resources.buttonsave;

                btnEdit.Enabled = false;

                btnDelete.Text = "cancel";
                btnDelete.Enabled = true;
                btnDelete.Values.Image = Properties.Resources.buttoncancel;

                txtordno.SelectAll();
                txtordno.Focus();

                dgvResult.DataSource = null;
                dgvResult.Columns.Clear();

                refreshDGV();
            }
            else
            {
                string strstatus = String.Empty;
                if (txtordno.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtordno.SelectAll();
                    txtordno.Focus();
                    return;
                }
                if (txtval.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtval.SelectAll();
                    txtval.Focus();
                    return;
                }
                DateTime datestart = new DateTime();
                if (dtstart.Text == "" || dtstart.Value == null)
                {
                    MessageBox.Show("Please fill the effctive date information.");
                    dtstart.Focus();
                    return;
                }
                else
                    datestart = dtstart.Value;

                DateTime dateend = new DateTime();
                int intenddate = 0;//with end date 1-true 0-false
                if (this.cbxcancel.Checked == true)
                {
                    if (dtend.Text == "" || dtend.Value == null)
                    {
                        MessageBox.Show("Please fill the end date information.");
                        dtend.Focus();
                        return;
                    }
                    else
                    {
                        dateend = dtend.Value;
                        intenddate = 1;
                    }
                }

                int intpeso = 0;//in peso value 1-true 0-false(pectage)
                if(cbxpesos.Checked)
                    intpeso = 1;

                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //validate for the existance of the record
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from ordinance_tbl ");
                sqry.Append(String.Format("where ordinance_no = '{0}' ", this.txtordno.Text.Trim()));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (rowCount > 0)
                {
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't add this record, \n\rit is already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                sqry = new StringBuilder();
                sqry.Append(String.Format("insert into ordinance_tbl value(0,'{0}','{1}','{2}',{3},{4},{5})",
                    txtordno.Text.Trim(), datestart.Date.ToString("yyyy-MM-dd"), dateend.Date.ToString("yyyy-MM-dd"), 
                    intenddate, txtval.Text.Trim(),intpeso));
                try
                {
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.ExecuteNonQuery();

                    string strid = cmd.LastInsertedId.ToString();
                    cmd.Dispose();

                    //check for the imagelist from flowLayoutPanelMain
                    if (flowLayoutPanelMain.Controls.Count > 0)
                    {
                        //validate for the existance of the record
                        sqry = new StringBuilder();
                        sqry.Append("select count(*) from ordinance_blob ");
                        sqry.Append(String.Format("where ordinance_id = {0} ", strid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        rowCount = 0;
                        rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount > 0)
                        {
                            sqry = new StringBuilder();
                            sqry.Append("delete from ordinance_blob ");
                            sqry.Append(String.Format("where ordinance_id = {0} ", strid));
                            if (myconn.State == ConnectionState.Closed)
                                myconn.Open();
                            cmd = new MySqlCommand(sqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        List<Control> listControls = flowLayoutPanelMain.Controls.Cast<Control>().ToList();
                        byte[] rawData;
                        UInt32 FileSize;
                        string strfilenm = String.Empty;

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();

                        foreach (Control cntrl in listControls)
                        {
                            if (cntrl is ImageViewer)
                            {
                                byte[] blobData = null;
                                
                                strfilenm = ((ImageViewer)cntrl).ImageFullFileName;
                                using (FileStream fileStream = new FileStream(strfilenm, FileMode.Open, FileAccess.Read))
                                {
                                    FileSize = Convert.ToUInt32(fileStream.Length);
                                    BinaryReader br = new BinaryReader(fileStream);
                                    blobData = br.ReadBytes(Convert.ToInt32(fileStream.Length));
                                    fileStream.Read(blobData, 0, Convert.ToInt32(fileStream.Length));
                                    br.Close();
                                    fileStream.Close();
                                }

                                string strsql = "INSERT INTO ordinance_blob VALUES(0, @OrdinanceId, @Mime, @File, @FileSize)";

                                cmd = new MySqlCommand();
                                cmd.Connection = myconn;
                                cmd.CommandText = strsql;
                                cmd.Parameters.AddWithValue("@OrdinanceId", strid);
                                cmd.Parameters.AddWithValue("@Mime", GetMimeTypeFromExtension(Path.GetExtension(strfilenm)));
                                cmd.Parameters.AddWithValue("@File", blobData);
                                cmd.Parameters.AddWithValue("@FileSize", FileSize);

                                cmd.ExecuteNonQuery();

                            }
                        }
                    }

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "ORDINANCE_ADD", "ORDINANCE_TBL|ORDINANCE_BLOB",
                        Environment.MachineName.ToString(), "ADD NEW LOCAL TAX ORDINANCE INFO: ORDINANCE_NO=" + txtordno.Text
                        + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully added the new record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static string GetMimeTypeFromExtension(string extension)
        {
            string result;
            RegistryKey key;
            object value;

            if (!extension.StartsWith("."))
                extension = "." + extension;

            key = Registry.ClassesRoot.OpenSubKey(extension, false);
            value = key != null ? key.GetValue("Content Type", null) : null;
            result = value != null ? value.ToString() : string.Empty;

            return result;
        }

        public void refreshDGV()
        {
            StringBuilder sbqry = new StringBuilder();
            sbqry.Append("select a.id, a.effective_date, a.ordinance_no, a.amount_val, a.with_enddate, a.end_date, a.in_pesovalue ");
            sbqry.Append("from ordinance_tbl a ");
            sbqry.Append("order by a.effective_date desc");
            m_dt = m_clscom.setDataTable(sbqry.ToString(), m_frmM._connection);
            setDataGridView(m_dt);
        }

        public void setnormal()
        {
            dgvResult.DataSource = null;
            dgvResult.Columns.Clear();

            this.flowLayoutPanelMain.Controls.Clear();

            refreshDGV();

            txtordno.Text = "";
            txtordno.ReadOnly = true;
            txtval.Text = "";
            txtval.ReadOnly = true;

            btnAdd.Enabled = true;
            btnAdd.Text = "new";
            btnAdd.Values.Image = Properties.Resources.buttonadd;

            btnEdit.Enabled = false;
            btnEdit.Text = "edit";
            btnEdit.Values.Image = Properties.Resources.buttonapply;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;

            dgvResult.Enabled = true;
            grpcontrol.Enabled = true;

            txtcnt.Text = String.Format("Count: {0:#,##0}", dgvResult.RowCount);
        }

        public void setDataGridView(DataTable dt)
        {
            this.dgvResult.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                this.dgvResult.DataSource = dt;
                int iwidth = dgvResult.Width / 3;
                dgvResult.DataSource = dt;
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Effectivity Date";
                dgvResult.Columns[2].Width = iwidth;
                dgvResult.Columns[2].HeaderText = "Ordinance No";
                dgvResult.Columns[3].Width = iwidth;
                dgvResult.Columns[3].HeaderText = "Value";
                dgvResult.Columns[4].Width = 0;
                dgvResult.Columns[4].HeaderText = "With EndDate";
                dgvResult.Columns[5].Width = 0;
                dgvResult.Columns[5].HeaderText = "End Date";
                dgvResult.Columns[6].Width = 0;
                dgvResult.Columns[6].HeaderText = "In Peso Value";
            }
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;
            KryptonDataGridView dgv = sender as KryptonDataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                btnEdit.Enabled = true;
                btnEdit.Text = "edit";

                btnDelete.Enabled = true;
                btnDelete.Text = "remove";

                int ordinanceid = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[0].Value.ToString());
                string strordno = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[2].Value.ToString();
                txtordno.Text = strordno;
                txtval.Text = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[3].Value.ToString();
                int inpeso = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[6].Value.ToString());
                if (inpeso == 0)
                    cbxpesos.Checked = false;
                else
                    cbxpesos.Checked = true;

                dtstart.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[1].Value.ToString());

                int isenddate = Convert.ToInt32(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[4].Value.ToString());
                if (isenddate == 0)
                {
                    dtend.Value = DateTime.Now;
                    cbxcancel.Checked = false;
                }
                else
                {
                    dtend.Value = Convert.ToDateTime(dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[5].Value.ToString());
                    cbxcancel.Checked = true;
                }

                StringBuilder sqry = new StringBuilder();
                sqry.Append(String.Format("select * from ordinance_blob where ordinance_id = {0}", ordinanceid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();

                MySqlDataReader myData;
                MySqlCommand cmd = new MySqlCommand();
                byte[] rawData;
                MemoryStream ms;
                UInt32 FileSize;
                Bitmap outImage;

                try
                {
                    cmd.Connection = myconn;
                    cmd.CommandText = sqry.ToString();

                    myData = cmd.ExecuteReader();

                    if (!myData.HasRows)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        cmd.Dispose();
                        flowLayoutPanelMain.Controls.Clear();
                        MessageBox.Show("There are no attached images found", this.Text);
                        return;
                    }
                    else
                    {
                        string strdir = String.Empty;
                        while(myData.Read())
                        {
                            if (!DBNull.Value.Equals(myData["blob_data"]))
                            {
                                int imgid = Convert.ToInt32(myData["id"]);
                                byte[] blob = (Byte[])myData["blob_data"];

                                using (MemoryStream memStream = new MemoryStream(blob,0,blob.Length))
                                {
                                    try
                                    {
                                        memStream.Write(blob, 0, blob.Length);
                                        memStream.Seek(0, SeekOrigin.Begin);
                                        Image img = Image.FromStream(memStream);

                                        //AddImageIII(img);//working adds image as thumbnails

                                        //write to file 
                                        if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\BlobImages_" + strordno))
                                            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + @"\BlobImages_" + strordno);
                                        else
                                        {
                                            DirectoryInfo folderInfo = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath) + @"\BlobImages_" + strordno);
                                            foreach (FileInfo file in folderInfo.GetFiles())
                                            {
                                                file.Delete();
                                            }
                                        }

                                        string sfilenm = Path.GetDirectoryName(Application.ExecutablePath) + @"\BlobImages_" + strordno + @"\img" + imgid + ".jpg";
                                        img.Save(sfilenm, System.Drawing.Imaging.ImageFormat.Jpeg);

                                        strdir = Path.GetDirectoryName(sfilenm);

                                        memStream.Close();
                                    }
                                    catch(Exception err)
                                    {
                                        MessageBox.Show(err.Message,this.Text);
                                    }
                                }
                            }
                        }

                        flowLayoutPanelMain.Controls.Clear();
                        if (Directory.Exists(strdir)) 
                        {
                            string [] fileEntries = Directory.GetFiles(strdir,"*.jpg");
                            foreach (string fileName in fileEntries)
                            {
                                if (Path.GetExtension(fileName).ToUpper() == ".JPG")
                                {
                                    try
                                    {
                                        AddImageIII(Image.FromFile(fileName), fileName);
                                    }
                                    catch (Exception err)
                                    {
                                        MessageBox.Show(err.Message, this.Text);
                                    }
                                }
                            }
                        }

                        myData.Close();
                        myData.Dispose();

                        cmd.Dispose();
                        myconn.Close();
                    }

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "ORDINANCE_DELETE", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            if (btnDelete.Text == "remove")
            {
                DialogResult ans = MessageBox.Show("Are you sure you want to remove \n\rthis record, continue?",
                    this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == System.Windows.Forms.DialogResult.Yes)
                {
                    myconn = new MySqlConnection();
                    myconn.ConnectionString = m_frmM._connection;

                    int intid = -1;
                    if (dgvResult.SelectedRows.Count == 1)
                        intid = Convert.ToInt32(dgvResult.SelectedRows[0].Cells[0].Value.ToString());

                    //query for the existance of ordinance on other table
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("select count(*) from patrons_ordinance ");
                    sqry.Append(String.Format("where ordinance_id = {0}", intid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        myconn.Close();
                        setnormal();
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show("Can't remove this record, \n\rit's being used by a patron.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //delete from ordinances given that the ordinance is not used by a patron
                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from ordinance_tbl where id = {0}", intid));
                    try
                    {
                        //delete value for the ordinance_tbl table
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();

                        //3.13.2015 remove blob
                        sqry = new StringBuilder();
                        sqry.Append(String.Format("delete from ordinance_blob where ordinance_id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        m_clscom.AddATrail(m_frmM.m_userid, "ORDINANCE_DELETE", "ORDINANCE_TBL|ORDINANCE_BLOB",
                        Environment.MachineName.ToString(), "REMOVED ORDINANCE INFO: NAME=" + this.txtordno.Text
                        + " | ID=" + intid.ToString(), m_frmM._connection);

                        MessageBox.Show("You have successfully removed \n\rthe selected record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (MySqlException er)
                    {
                        if (myconn.State == ConnectionState.Open)
                            myconn.Close();
                        MessageBox.Show(er.Message, this.Text);
                    }

                }
            }
            refreshDGV();
            setnormal();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //QUERY IF THE USER HAS RIGHTS FOR THE MODULE
            bool isEnabled = m_clscom.verifyUserRights(m_frmM.m_userid, "ORDINANCE_EDIT", m_frmM._connection);
            if (m_frmM.boolAppAtTest == true)
                isEnabled = m_frmM.boolAppAtTest;

            if (isEnabled == false)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("You have no access rights for this module.", this.Text);
                return;
            }

            unselectbutton();
            myconn = new MySqlConnection();
            myconn.ConnectionString = m_frmM._connection;

            if (dgvResult.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (dgvResult.SelectedRows.Count == 1)
                dgvResult.Enabled = false;

            if (btnEdit.Text == "edit")
            {
                txtordno.ReadOnly = false;
                txtval.ReadOnly = false;

                btnAdd.Enabled = false;
                btnAdd.Text = "new";

                btnEdit.Text = "update";
                btnEdit.Enabled = true;

                btnDelete.Enabled = true;
                btnDelete.Text = "cancel";

                txtordno.SelectAll();
                txtordno.Focus();
            }
            else
            {
                string strstatus = String.Empty;
                if (txtordno.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtordno.SelectAll();
                    txtordno.Focus();
                    return;
                }
                if (txtval.Text == "")
                {
                    MessageBox.Show("Please fill the required information.");
                    txtval.SelectAll();
                    txtval.Focus();
                    return;
                }
                DateTime datestart = new DateTime();
                if (dtstart.Text == "" || dtstart.Value == null)
                {
                    MessageBox.Show("Please fill the effctive date information.");
                    dtstart.Focus();
                    return;
                }
                else
                    datestart = dtstart.Value;

                DateTime dateend = new DateTime();
                int intenddate = 0;//with end date 1-true 0-false
                if (this.cbxcancel.Checked)
                {
                    if (dtend.Text == "" || dtend.Value == null)
                    {
                        MessageBox.Show("Please fill the end date information.");
                        dtend.Focus();
                        return;
                    }
                    else
                        dateend = dtend.Value;
                }

                int intpeso = 0;//in peso value 1-true 0-false(pectage)
                if (cbxpesos.Checked)
                    intpeso = 1;

                KryptonDataGridView dgv = dgvResult;
                string strid = "-1";
                if (dgv == null)
                    return;
                if (dgv.CurrentRow.Selected)
                {
                    strid = dgvResult.Rows[dgvResult.CurrentCell.RowIndex].Cells[0].Value.ToString();
                }
                myconn = new MySqlConnection();
                myconn.ConnectionString = m_frmM._connection;

                //query for the existance of ordinance on other table
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select count(*) from patrons_ordinance ");
                sqry.Append(String.Format("where ordinance_id = '{0}'", strid));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (rowCount > 0)
                {
                    myconn.Close();
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't update this record, \n\rits being used by a patron.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool boolstatus = false;
                //validate for the existance of the record
                sqry = new StringBuilder();
                sqry.Append("select id from ordinance_tbl ");
                sqry.Append(String.Format("where ordinance_no = '{0}' ", this.txtordno.Text.Trim()));
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                cmd = new MySqlCommand(sqry.ToString(), myconn);
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        int intid = Convert.ToInt32(rd[0].ToString());
                        if (intid != Convert.ToInt32(strid))
                        {
                            cmd.Dispose();
                            myconn.Close();
                            setnormal();
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();
                            MessageBox.Show("This ordinance number \n\ris already existing from the list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                            boolstatus = true;
                    }
                    
                }
                else
                    boolstatus = true;

                rd.Close();
                cmd.Dispose();

                /*{
                    cmd.Dispose();
                    myconn.Close();
                    setnormal();
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't find this record, \n\rfrom the existing list.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }*/

                if(boolstatus)
                {
                    sqry = new StringBuilder();
                    sqry.Append("update ordinance_tbl ");
                    sqry.Append("set ordinance_no = @ordinance_no, ");
                    sqry.Append("effective_date = @effdate, ");
                    sqry.Append("end_date = @enddate, ");
                    sqry.Append("with_enddate = @wenddate, ");
                    sqry.Append("amount_val = @amtval, ");
                    sqry.Append("in_pesovalue = @inpesoval ");   
                     sqry.Append("where id = @id");   

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    cmd.Parameters.AddWithValue("@ordinance_no",txtordno.Text.Trim());
                    cmd.Parameters.AddWithValue("@effdate", datestart.Date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@enddate", dateend.Date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@wenddate", intenddate);
                    cmd.Parameters.AddWithValue("@amtval", txtval.Text.Trim());
                    cmd.Parameters.AddWithValue("@inpesoval", intpeso);
                    cmd.Parameters.AddWithValue("@id", strid);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                
                try
                {
                    //validate for the existance of the record
                    sqry = new StringBuilder();
                    sqry.Append("select count(*) from ordinance_blob ");
                    sqry.Append(String.Format("where ordinance_id = {0} ", strid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    cmd = new MySqlCommand(sqry.ToString(), myconn);
                    rowCount = 0;
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (rowCount > 0)
                    {
                        sqry = new StringBuilder();
                        sqry.Append("delete from ordinance_blob ");
                        sqry.Append(String.Format("where ordinance_id = {0} ", strid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //check for the imagelist from flowLayoutPanelMain
                    if (flowLayoutPanelMain.Controls.Count > 0)
                    {
                        List<Control> listControls = flowLayoutPanelMain.Controls.Cast<Control>().ToList();
                        byte[] rawData;
                        UInt32 FileSize;
                        string strfilenm = String.Empty;

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();

                        foreach (Control cntrl in listControls)
                        {
                            if (cntrl is ImageViewer)
                            {
                                byte[] blobData = null;

                                strfilenm = ((ImageViewer)cntrl).ImageFullFileName;
                                using (FileStream fileStream = new FileStream(strfilenm, FileMode.Open, FileAccess.Read))
                                {
                                    FileSize = Convert.ToUInt32(fileStream.Length);
                                    BinaryReader br = new BinaryReader(fileStream);
                                    blobData = br.ReadBytes(Convert.ToInt32(fileStream.Length));
                                    fileStream.Read(blobData, 0, Convert.ToInt32(fileStream.Length));
                                    br.Close();
                                    fileStream.Close();
                                }

                                string strsql = "INSERT INTO ordinance_blob VALUES(0, @OrdinanceId, @Mime, @File, @FileSize)";

                                cmd = new MySqlCommand();
                                cmd.Connection = myconn;
                                cmd.CommandText = strsql;
                                cmd.Parameters.AddWithValue("@OrdinanceId", strid);
                                cmd.Parameters.AddWithValue("@Mime", GetMimeTypeFromExtension(Path.GetExtension(strfilenm)));
                                cmd.Parameters.AddWithValue("@File", blobData);
                                cmd.Parameters.AddWithValue("@FileSize", FileSize);

                                cmd.ExecuteNonQuery();

                            }
                        }
                    }

                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();

                    m_clscom.AddATrail(m_frmM.m_userid, "ORDINANCE_EDIT", "ORDINANCE_TBL|ORDINANCE_BLOB",
                        Environment.MachineName.ToString(), "EDIT LOCAL TAX ORDINANCE INFO: ORDINANCE_NO=" + txtordno.Text
                        + " | ID=" + strid, m_frmM._connection);

                    refreshDGV();
                    setnormal();
                    MessageBox.Show("You have successfully updated the existing record.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception err)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                    MessageBox.Show("Can't connect to the contact table." + err.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public class ThumbnailImageEventArgs : EventArgs
    {
        public ThumbnailImageEventArgs(int size)
        {
            this.Size = size;
        }

        public int Size;
    }

    public delegate void ThumbnailImageEventHandler(object sender, ThumbnailImageEventArgs e);
}
