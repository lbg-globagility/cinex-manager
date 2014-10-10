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

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : NavigationWindow
    {
        public NewWindow()
        {
            Thread.Sleep(500); //delays display of splash screen

            InitializeComponent();

            this.ShowsNavigationUI = false;

            //load page
            NavigationService.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
        }

        private void NavigationWindow_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
