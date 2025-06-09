using Cinex.Core.Entities;
using Cinex.WinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ew = Cinex.Core.Entities.Ewallet;

namespace Paradiso.EF.Ewallet
{
    /// <summary>
    /// Interaction logic for frmEwalletTransaction2.xaml
    /// </summary>
    public partial class frmEwalletTransaction2 : Window
    {
        private readonly int _userId;
        private readonly string _sessionId;

        public SessionEwallet SessionEwallet { get; private set; }

        public bool HasSessionEwallet => SessionEwallet != null;

        private HotKeyHelper _escapeHotKey;
        private HotKeyHelper _enterHotKey;
        private int _throwConfettiKeyId;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            LoadHotKeyHelper();
        }

        private void LoadHotKeyHelper()
        {
            var active = HotKeyHelper.GetActiveWindow();

            var activeWindow = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(window => new WindowInteropHelper(window).Handle == active);

            if (activeWindow == null) return;

            _enterHotKey = new HotKeyHelper(handlerWindow: activeWindow);

            _throwConfettiKeyId = _enterHotKey.ListenForHotKey(
                Key.Enter,
                HotKeyModifiers.None,
                () =>
                {
                    if (!btnOK.IsEnabled) return;

                    _enterHotKey.Dispose();
                    btnOK_Click(btnOK, new RoutedEventArgs());
                });

            _escapeHotKey = new HotKeyHelper(handlerWindow: activeWindow);

            _throwConfettiKeyId = _escapeHotKey.ListenForHotKey(
                Key.Escape,
                HotKeyModifiers.None,
                () =>
                {
                    if (!btnOK.IsEnabled) return;

                    _escapeHotKey.Dispose();
                    SessionEwallet = null;
                    Close();
                });

            // IsEnabled="False" IsDefault="True"
        }

        public frmEwalletTransaction2(int userId,
            string sessionId)
        {
            _userId = userId;
            _sessionId = sessionId;

            InitializeComponent();

            //WindowStartupLocation = WindowStartupLocation.CenterOwner;
            btnOK.IsEnabled = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            var _bool = cboxEwallets.SelectedValue != null;
            if (!_bool) return;

            int eWalletId;
            int.TryParse($"{cboxEwallets.SelectedValue}", result: out eWalletId);

            SessionEwallet = SessionEwallet.New(sessionId: _sessionId,
                contactNo: string.Empty,
                referenceNo: txtRefNo.Text,
                eWalletId: eWalletId,
                remarks: txtRemarks.Text);

            //DialogResult = true;
            Close();
        }

        private async void frmEwalletTransaction2_Loaded(object sender, RoutedEventArgs e)
        {
            btnOK.IsEnabled = false;

            await LoadEwalletDtos()
                .ContinueWith((a) =>
                {
                    if(!a.IsCompleted) return;

                    btnOK.IsEnabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task<List<ew>> GetEwallets()
        {
            var eWalletDataService = DependencyInjectionHelper.GetEwalletDataService;
            return (await eWalletDataService.GetAllAsync()).ToList();
        }

        private async Task LoadEwalletDtos()
        {
            var dataSource = (await GetEwallets())
                .Select(t => new EwalletDto(t))
                .OrderByDescending(t => t.IsDefault)
                    .ThenBy(t => t.DisplayText)
                .ToList();

            cboxEwallets.DisplayMemberPath = "DisplayText";
            cboxEwallets.SelectedValuePath = "Id";
            cboxEwallets.ItemsSource = dataSource;

            cboxEwallets.SelectedValue = dataSource?.FirstOrDefault()?.Id;
        }

        private void frmEwalletTransaction2_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"{_throwConfettiKeyId}");
            _escapeHotKey.Dispose();
            _enterHotKey.Dispose();
        }
    }
}
