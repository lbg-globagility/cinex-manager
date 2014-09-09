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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MainWindow1.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateDashboard()
        {
            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();

            TicketingTerminalTitle.Text = paradisoObjectManager.Title;

            if (paradisoObjectManager.UserName == string.Empty)
            {
                ScreeningDate.Visibility = System.Windows.Visibility.Hidden;
                ScreeningDate.Text = string.Empty;
                Dashboard.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                ScreeningDate.Visibility = System.Windows.Visibility.Visible;
                ScreeningDate.Text = string.Format("{0:MMMM dd, yyyy}", paradisoObjectManager.ScreeningDate);
                UserName.Text = paradisoObjectManager.UserName; 
                Dashboard.Visibility = System.Windows.Visibility.Visible;
            }

            //CurrentDateTime.Text = string.Format("{0:MMMM dd, yyyy, ddd hh:mmtt}", paradisoObjectManager.CurrentDate).ToUpper();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.UpdateDashboard();
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
    }
}
