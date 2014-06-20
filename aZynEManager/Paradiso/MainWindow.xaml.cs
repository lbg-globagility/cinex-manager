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
using System.Windows.Navigation;
using System.Threading;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            Thread.Sleep(500);

            InitializeComponent();


            this.ShowsNavigationUI = false;

            //load page
            NavigationService.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
            
            //2,3, 1214,1215,1216
            //NavigationService.Navigate(new TicketingPage(2, 3, new List<int>() {1214, 1215, 1216}),  UriKind.Relative);  
        }

        private void NavigationWindow_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
