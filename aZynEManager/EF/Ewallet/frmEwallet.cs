using Cinex.WinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ew = Cinex.Core.Entities.Ewallet;

namespace aZynEManager.EF.Ewallet
{
    public partial class frmEwallet : CustomDefaultForm
    {
        private readonly int _userId;

        public frmEwallet(int userId)
        {
            _userId = userId;

            InitializeComponent();
        }

        private async void frmEwallet_Load(object sender, EventArgs e)
        {
            datagridEwallets.AutoGenerateColumns = false;

            await LoadEwallets();
        }

        private async Task<List<ew>> GetEwallets()
        {
            var eWalletDataService = DependencyInjectionHelper.GetEwalletDataService;
            return (await eWalletDataService.GetAllAsync()).ToList();
        }

        private async Task LoadEwallets()
        {
            var dataSource = (await GetEwallets())
                .Select(t => new EwalletDto(t))
                .OrderByDescending(t => t.IsDefault)
                    .ThenBy(t => t.DisplayText)
                .ToList();

            datagridEwallets.BindingContext = new BindingContext();
            datagridEwallets.DataSource = dataSource;
        }

        private async void ToolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var newEwallet = ew.New(string.Empty);
            var form = new frmEwalletAddEdit(_userId, newEwallet);
            if (form.ShowDialog() != DialogResult.OK) return;
            await LoadEwallets();
        }

        private async void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            ToolStripButtonCancel.Enabled = false;

            await LoadEwallets()
                .ContinueWith(async (a) =>
                {
                    if (a.IsCompleted) ToolStripButtonCancel.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void datagridEwallets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Column4.Index == e.ColumnIndex)
            {
                datagridEwallets.EndEdit();
                var currentRow = datagridEwallets.CurrentRow;
                var _bool = (bool)currentRow.Cells[Column4.Name].Value;

                if(_bool)
                {
                    var _ew = currentRow.DataBoundItem as EwalletDto;
                    var prompt = MessageBox.Show(
                        text: $"Are you sure you want `{_ew.DisplayText}`{Environment.NewLine}to make the DEFAULT eWallet?{Environment.NewLine}{Environment.NewLine}NOTE: ONLY a single eWallet could be assigned.",
                        caption: "Assign Default eWallet",
                        buttons: MessageBoxButtons.YesNoCancel,
                        icon: MessageBoxIcon.Question);

                    if (prompt != DialogResult.Yes) return;

                    var eWalletDataService = DependencyInjectionHelper.GetEwalletDataService;
                    await eWalletDataService.SetDefaultEwalletAsync(_userId, _ew.Ewallet)
                        .ContinueWith(async (a) =>
                         {
                             if (a.IsCompleted) await LoadEwallets();
                         }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private async void datagridEwallets_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private async void datagridEwallets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var eWallet = datagridEwallets.CurrentRow.DataBoundItem as EwalletDto;
            var form = new frmEwalletAddEdit(_userId, eWallet.Ewallet);
            if (form.ShowDialog() != DialogResult.OK) return;
            await LoadEwallets();
        }

        private void datagridEwallets_DataSourceChanged(object sender, EventArgs e)
        {
            var rows = datagridEwallets.Rows.OfType<DataGridViewRow>();
            if (!rows?.Any() ?? false) return;

            var defaultEwalletRow = rows
                .Select(t => new { row = t, isDefault = (t.DataBoundItem as EwalletDto).IsDefault })
                .Where(t => t.isDefault)
                .Select(t => t.row)
                .ToList();

            foreach (var row in defaultEwalletRow)
            {
                var font = row.InheritedStyle.Font;
                row.DefaultCellStyle.Font = new Font(prototype: font, newStyle: FontStyle.Bold);
            }
        }
    }
}
