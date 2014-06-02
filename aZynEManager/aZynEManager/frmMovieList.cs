using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aZynEManager
{
    public partial class frmMovieList : Form
    {
        frmRating frmrating = null;
        frmMain m_frmM = null;
        frmDistributor frmdistributor = null;
        public frmMovieList()
        {
            InitializeComponent();
        }

        private void frmMovieList_Load(object sender, EventArgs e)
        {
            unselectbutton();
        }

        public void frmInit(frmMain frm, DataSet ds)
        {
            m_frmM = frm;
            if (ds.Tables.Count > 0)
            {
                int iwidth = dgvResult.Width / 6;
                dgvResult.DataSource = ds.Tables[0];
                dgvResult.Columns[0].Width = 0;
                dgvResult.Columns[0].HeaderText = "ID";
                dgvResult.Columns[1].Width = iwidth;
                dgvResult.Columns[1].HeaderText = "Movie Code";
                dgvResult.Columns[2].Width = iwidth * 2;
                dgvResult.Columns[2].HeaderText = "Movie Title";
                dgvResult.Columns[3].Width = iwidth;
                dgvResult.Columns[3].HeaderText = "Film Length";
                dgvResult.Columns[4].Width = iwidth;
                dgvResult.Columns[4].HeaderText = "Movie Rating";
                dgvResult.Columns[5].Width = iwidth;
                dgvResult.Columns[5].HeaderText = "Distributor";
            }

            grpcontrol.Visible = true;
            grpfilter.Visible = false;
            unselectbutton();

            populatedistributor();
            populateratings();
            populateclassification();

            cmbdistributor.Enabled = false;
            cmbrating.Enabled = false;
            setnormal();

        }

        public void setnormal()
        {
            btnAdd.Enabled = true;
            btnAdd.Text = "add";
            btnAdd.Values.Image = Properties.Resources.buttonadd;

            btnEdit.Enabled = false;
            btnEdit.Text = "edit";
            btnEdit.Values.Image = Properties.Resources.buttonapply;

            btnDelete.Enabled = false;
            btnDelete.Text = "remove";
            btnDelete.Values.Image = Properties.Resources.buttondelete;

            dgvResult.Enabled = true;
            grpcontrol.Enabled = true;

        }

        private void populateclassification()
        {
            lstcls.Items.Clear();
            ListViewItem lvitem = new ListViewItem();
            string sqry = "[id] > -1";
            var foundRows = m_frmM.m_dtclassification.Select(sqry).CopyToDataTable();
            
            if (foundRows.Rows.Count > 0)
            {
                for(int i = 0; i < foundRows.Rows.Count; i++)
                {
                    lvitem = lstcls.Items.Add(foundRows.Rows[i]["description"].ToString().Trim());
                    lvitem.SubItems.Add(foundRows.Rows[i]["id"].ToString());
                }

                ListViewSorter sorter = new ListViewSorter();
                this.lstcls.ListViewItemSorter = sorter;
                if (!(lstcls.ListViewItemSorter is ListViewSorter))
                {
                    return;
                }
                sorter = (ListViewSorter)lstcls.ListViewItemSorter;
                lstcls.Sorting = SortOrder.Descending;
                lstcls.Sort();
            }
        }

        private int calculatetime()
        {
            int totalmin = 0;
            string[] Parts = dttime.Text.Trim().Split(":".ToCharArray());

            int Hours = 0;
            if (!int.TryParse(Parts[0], out Hours))
            {
                Hours = 0;
            }
            if (Hours >= 24)
            {
                Hours = 0;
            }

            int Minutes = 0;
            if (!int.TryParse(Parts[1], out Minutes))
            {
                Minutes = 0;
            }
            if (Minutes >= 60)
            {
                Minutes = 0;
            }

            totalmin = (Hours * 60) + Minutes;

            return totalmin;
        }

        private void populatedistributor()
        {
            string sqry = "[id] > -1";
            var foundRows = m_frmM.m_dtdistributor.Select(sqry).CopyToDataTable();
            if (foundRows.Rows.Count > 0)
            {
                cmbdistributor.DataSource = foundRows;
                cmbdistributor.ValueMember = "id";
                cmbdistributor.DisplayMember = "name";
            }
        }

        private void populateratings()
        {
            string sqry = "[id] > -1";
            var foundRows = m_frmM.m_dtrating.Select(sqry).CopyToDataTable();
            if (foundRows.Rows.Count > 0)
            {
                cmbrating.DataSource = foundRows;
                cmbrating.ValueMember = "id";
                cmbrating.DisplayMember = "name";
            }
        }

        private void btnaddrating_Click(object sender, EventArgs e)
        {
            if (frmrating == null)
                frmrating = new frmRating();

            frmrating.frmInit(m_frmM,this);
            frmrating.ShowDialog();
        }

        public void unselectbutton()
        {
            btnselect.Select();
        }

        private void cbxfilter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxfilter.Checked == true)
            {
                grpcontrol.Visible = false;
                grpfilter.Visible = true;
            }
            else
            {
                grpcontrol.Visible = true;
                grpfilter.Visible = false;
            }

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            unselectbutton();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            unselectbutton();
        }

        private void btnadddistributor_Click(object sender, EventArgs e)
        {
            if (frmdistributor == null)
                frmdistributor = new frmDistributor();

            frmdistributor.frmInit(m_frmM, this);
            frmdistributor.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvResult.Enabled = false;

            txtcode.Text = "";
            txttitle.Text = "";
            
            unselectbutton();
            btnAdd.Text = "save";
            btnAdd.Values.Image = Properties.Resources.buttonsave;

            btnEdit.Enabled = false;
            btnDelete.Text = "cancel";
            btnDelete.Values.Image = Properties.Resources.buttoncancel;

            txtcode.SelectAll();
            txtcode.Focus();


        }

        private void lstcls_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewSorter Sorter = new ListViewSorter();
            lstcls.ListViewItemSorter = Sorter;
            if (!(lstcls.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)lstcls.ListViewItemSorter;

            if (Sorter.LastSort == e.Column)
            {
                if (lstcls.Sorting == SortOrder.Ascending)
                    lstcls.Sorting = SortOrder.Descending;
                else
                    lstcls.Sorting = SortOrder.Ascending;
            }
            else
            {
                lstcls.Sorting = SortOrder.Descending;
            }
            Sorter.ByColumn = e.Column;

            lstcls.Sort();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            unselectbutton();


        }

        private void txtshare_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}
