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

            //sparkle = new Sparkle(string.Format("http://{0}/cinex/versioninfo_{1}.xml", strServer, strVersion));
            sparkle = new Sparkle(string.Format("http://{0}/cinex/versioninfo.xml", strServer));
            //sparkle.ShowDiagnosticWindow = true;
            //sparkle.EnableSilentMode = true;
            sparkle.StartLoop(true, true, new TimeSpan(0, 15, 0));
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

            //CurrentDateTime.Text = string.Format("{0:MMMM dd, yyyy, ddd hh:mmtt}", paradisoObjectManager.CurrentDate).ToUpper();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ParadisoObjectManager.GetInstance().GetPrinterPort(ParadisoObjectManager.GetInstance().DefaultPrinterName);
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.UpdateDashboard();


            string strContent = string.Empty;
            if (e.Content != null)
                strContent = e.Content.ToString();
            if (strContent != "Paradiso.TenderAmountPage") //option to go back
            {
                try
                {
                    while (MainFrame.NavigationService.CanGoBack)
                        MainFrame.NavigationService.RemoveBackEntry();
                }
                catch { }
            }
            
            if (strContent == "Paradiso.MovieCalendarPage")
            {
                ScreeningDate.IsEnabled = true;
            }
            else
            {
                ScreeningDate.IsEnabled = false;
            }

        }

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
            ParadisoObjectManager.GetInstance().UserId = 0;
            ParadisoObjectManager.GetInstance().UserName = string.Empty;
            ParadisoObjectManager.GetInstance().SetNewSessionId();
            MainFrame.Source = new Uri("LoginPage.xaml", UriKind.Relative);
           
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

            if (oldScreeningDate != ParadisoObjectManager.GetInstance().ScreeningDate && MainFrame.Source != null &&

                (MainFrame.Source.ToString() == "MovieCalendarPage.xaml" || MainFrame.Source.ToString() == "Paradiso;component/MovieCalendarPage.xaml"))
            {
                //MainFrame.Source = null;
                //MainFrame.Source = new Uri("MovieCalendarPage.xaml", UriKind.Relative);
                MainFrame.NavigationService.Refresh();
            }

        }
    }
}
