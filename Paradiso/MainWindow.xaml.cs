using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Threading;
using System.Globalization;
using AppLimit.NetSparkle;
using System.Configuration;
using System.Net.NetworkInformation;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MainWindow1.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Sparkle sparkle;
        public string Content { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //checks if ip can be ping

            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "MMMM d, yyyy";
            ci.DateTimeFormat.DateSeparator = "";
            Thread.CurrentThread.CurrentCulture = ci;

            string strServer = "localhost";
            strServer = ConfigurationManager.AppSettings["Host"];

            Ping myPing = new Ping();
            try
            {
                PingReply reply = myPing.Send(strServer, 1000);
                if (reply != null)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Cannot connect to server.");
                        Application.Current.Shutdown();
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Cannot connect to server.");
                Application.Current.Shutdown();
                return;
            }

            /*
            string strVersion = "x86";
            if (System.Environment.Is64BitOperatingSystem)
                strVersion = "x64";
            */

            /*
            Amellar.Common.EncryptUtilities.Encryption e = new Amellar.Common.EncryptUtilities.Encryption();
            string _e = e.EncryptString("Am3L74rS");
            */

            //sparkle = new Sparkle(string.Format("http://{0}/cinex/versioninfo_{1}.xml", strServer, strVersion));
            sparkle = new Sparkle(string.Format("http://{0}/cinex/versioninfo.php", strServer));
            //sparkle.ShowDiagnosticWindow = true;
            //sparkle.EnableSilentMode = true;
            //sparkle.EnableServiceMode = true;
            sparkle.StartLoop(true, true, new TimeSpan(0, 15, 0));
        }

        public void SwitchContent(string _content)
        {
            if (_content == "LoginPage.xaml")
            {
                this.SwitchContent(_content, new LoginPage());
            }
            else if (_content == "MovieCalendarPage.xaml")
            {
                this.SwitchContent(_content, new MovieCalendarPage());
            }
            else if (_content == "TicketPrintPage2.xaml")
            {
                this.SwitchContent(_content, new TicketPrintPage2());
            }
            else if (_content == "SettingPage.xaml")
            {
                this.SwitchContent(_content,  new SettingPage());
            }
            else if (_content == "ReservedSeatingPage.xaml")
            {
                this.SwitchContent(_content, new ReservedSeatingPage(ParadisoObjectManager.GetInstance().CurrentMovieSchedule));
            }
            else if (_content == "SeatingPage.xaml")
            {
                this.SwitchContent(_content, new SeatingPage(ParadisoObjectManager.GetInstance().CurrentMovieSchedule));
            }
            else if (_content == "EndTellerSessionPage.xaml")
            {
                this.SwitchContent(_content, new EndTellerSessionPage());
            }
            else if (_content == "TenderAmountPage.xaml")
            {
                this.SwitchContent(_content, new TenderAmountPage());
            }
            else if (_content == "CinemaPage.xaml")
            {
                this.SwitchContent(_content, new CinemaPage());
            }
        }
        /*
        public void SwitchContent(string _content, Model.MovieScheduleListModel model)
        {
            if (_content == "ReservedSeatingPage.xaml")
            {
                this.SwitchContent(_content, new ReservedSeatingPage(model));
            }
            else if (_content == "SeatingPage.xaml")
            {
                this.SwitchContent(_content, new SeatingPage(model));
            }
        }
        */

        public void SwitchContent(string _content, object _c)
        {
            if (MainContent.Content != null)
            {
                ((IDisposable)MainContent.Content).Dispose();
                MainContent.Content = null;
            }
            Content = _content;

            MainContent.Content = _c;
            this.UpdateDashboard();
        }

        private void UpdateDashboard()
        {
            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();

            TicketingTerminalTitle.Text = paradisoObjectManager.Title;

            if (paradisoObjectManager.UserName == string.Empty)
            {
                ScreeningDate.Visibility = System.Windows.Visibility.Hidden;
                ScreeningDate.Visibility = System.Windows.Visibility.Hidden;
                Dashboard.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                ScreeningDate.Visibility = System.Windows.Visibility.Visible;
                ScreeningDate.Visibility = System.Windows.Visibility.Visible;
                ScreeningDate.Value = paradisoObjectManager.ScreeningDate;
                //ScreeningDate.Text = string.Format("{0:MMMM dd, yyyy}", paradisoObjectManager.ScreeningDate);
                UserName.Text = paradisoObjectManager.UserName; 
                Dashboard.Visibility = System.Windows.Visibility.Visible;
            }

            ScreeningDate.IsEnabled = (Content == "MovieCalendarPage.xaml");

            GC.Collect();
            Memory.Text = string.Format("{0:0}", GC.GetTotalMemory(true) / (1024 * 1024));

            //CurrentDateTime.Text = string.Format("{0:MMMM dd, yyyy, ddd hh:mmtt}", paradisoObjectManager.CurrentDate).ToUpper();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ParadisoObjectManager.GetInstance().GetPrinterPort(ParadisoObjectManager.GetInstance().DefaultPrinterName);

            this.SwitchContent("LoginPage.xaml");
           
        }

        /*
        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.UpdateDashboard();


            string strContent = string.Empty;
            if (e.Content != null)
                strContent = e.Content.ToString();
            try
            {
                while (MainFrame.NavigationService.CanGoBack)
                    MainFrame.NavigationService.RemoveBackEntry();
            }
            catch { }
            
            if (strContent == "Paradiso.MovieCalendarPage")
            {
                ScreeningDate.IsEnabled = true;
            }
            else
            {
                ScreeningDate.IsEnabled = false;
            }

            //long totalMemory2 = GC.GetTotalMemory(true);

        }
        */

        private void UserPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((DockPanel)sender).ContextMenu.PlacementTarget = this;
            ((DockPanel)sender).ContextMenu.IsOpen = true;
            e.Handled = true;
        }

        private void UserPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Dashboard.ContextMenu.IsOpen = false;
            //perform logout
            ParadisoObjectManager.GetInstance().Log("LOGIN", "TICKET|LOGOUT",
                string.Format("LOGOUT OK-{0} ({1})", ParadisoObjectManager.GetInstance().UserLogInName, ParadisoObjectManager.GetInstance().SessionId));

            ParadisoObjectManager.GetInstance().UserId = 0;
            ParadisoObjectManager.GetInstance().UserLogInName = string.Empty;
            ParadisoObjectManager.GetInstance().UserName = string.Empty;
            ParadisoObjectManager.GetInstance().SetNewSessionId();

            this.SwitchContent("LoginPage.xaml");
            //MainFrame.Source = new Uri("LoginPage.xaml", UriKind.Relative);
           
        }

        private void ScreeningDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Xceed.Wpf.Toolkit.DateTimePicker dtp = (Xceed.Wpf.Toolkit.DateTimePicker)sender;
            
            DateTime screeningDate = (DateTime)dtp.Value;
            DateTime currentDate = ParadisoObjectManager.GetInstance().CurrentDate.Date;
            DateTime oldScreeningDate = ParadisoObjectManager.GetInstance().ScreeningDate;


            if (!ParadisoObjectManager.GetInstance().HasRights("ADVANCEDATE") &&  screeningDate > currentDate )
            {
                ParadisoObjectManager.GetInstance().ScreeningDate = currentDate;
                ScreeningDate.Text = string.Format("{0:MMMM dd, yyyy}", currentDate);
                ScreeningDate.Value = currentDate;
                
            }
            else if (!ParadisoObjectManager.GetInstance().HasRights("PRIORDATE") && currentDate > screeningDate)
            {
                ParadisoObjectManager.GetInstance().ScreeningDate = currentDate;
                ScreeningDate.Text = string.Format("{0:MMMM dd, yyyy}", currentDate);
                ScreeningDate.Value = currentDate;
            }
            else
            {
                ParadisoObjectManager.GetInstance().ScreeningDate = screeningDate.Date;
                ScreeningDate.Text = string.Format("{0:MMMM dd, yyyy}", screeningDate);
                ScreeningDate.Value = screeningDate;
            }

            if (oldScreeningDate != ParadisoObjectManager.GetInstance().ScreeningDate && Content == "MovieCalendarPage.xaml")
            {
                this.SwitchContent(Content);
            }

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //perform logout
            try
            {
                ParadisoObjectManager.GetInstance().Log("LOGIN", "TICKET|LOGOUT",
                    string.Format("LOGOUT OK-{0} ({1})", ParadisoObjectManager.GetInstance().UserLogInName, ParadisoObjectManager.GetInstance().SessionId));
            }
            catch { }
        }
    }
}
