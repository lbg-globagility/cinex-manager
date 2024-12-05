using Cinex.WinForm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ew = Cinex.Core.Entities.Ewallet;

namespace aZynEManager.EF.Ewallet
{
    public partial class frmEwalletAddEdit : CustomDefaultForm
    {
        private readonly int _userId;
        private readonly ew _eWallet;

        public frmEwalletAddEdit(int userId, ew eWallet)
        {
            _userId = userId;
            _eWallet = eWallet;

            InitializeComponent();
        }

        private void frmEwalletAddEdit_Load(object sender, EventArgs e)
        {
            var updateMode = DataSourceUpdateMode.OnPropertyChanged;

            txtName.DataBindings.Add("Text",
                _eWallet,
                "Name",
                false,
                updateMode,
                nullValue: string.Empty);

            txtAcctNo.DataBindings.Add("Text",
                _eWallet,
                "AccountNo",
                false,
                updateMode,
                nullValue: string.Empty);

            chkboxIsActive.DataBindings.Add("Checked",
                _eWallet,
                "IsActive",
                false,
                updateMode,
                nullValue: false);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(text: "Please input a valid `eWallet Name`",
                    caption: "Invalid Name",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtAcctNo.Text))
            {
                MessageBox.Show(text: "Please input a valid `Acct. No.` or `Mobile No.`",
                    caption: "Invalid Acct. No.",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Warning);
                return;
            }

            btnSave.Enabled = false;

            var newEwallets = new List<ew>();
            if (_eWallet.IsNewEntity) newEwallets = new List<ew>() { _eWallet };

            var updateEwallets = new List<ew>();
            if (!_eWallet.IsNewEntity) updateEwallets = new List<ew>() { _eWallet };

            var eWalletDataService = DependencyInjectionHelper.GetEwalletDataService;

            await eWalletDataService.SaveManyAsync(_userId,
                added: newEwallets,
                updated: updateEwallets)
                .ContinueWith((a) => {
                    DialogResult = DialogResult.OK;
                    btnSave.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
