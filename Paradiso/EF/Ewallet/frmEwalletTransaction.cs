using Cinex.WinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ew = Cinex.Core.Entities.Ewallet;

namespace Paradiso.EF.Ewallet
{
    public partial class frmEwalletTransaction : CustomDefaultForm
    {
        private readonly int _userId;

        public frmEwalletTransaction(int userId)
        {
            _userId = userId;

            InitializeComponent();
        }

        private async void frmEwalletTransaction_Load(object sender, EventArgs e)
        {
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

            cboxEwallets.BindingContext = new BindingContext();
            cboxEwallets.DataSource = dataSource;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
