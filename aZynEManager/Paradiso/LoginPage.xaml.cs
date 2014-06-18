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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strUserName = UserName.Text;
            string strPassword = Password.Password;

            if (strUserName == string.Empty)
            {
                MessageBox.Show("Missing User Name.");
                return;
            }
            else if (strPassword == string.Empty)
            {
                MessageBox.Show("Missing Password.");
                return;
            }

            using (var context = new paradisoEntities())
            {
                var username = (from u in context.users where u.username == strUserName && u.activated == true select u.full_name).FirstOrDefault();
                if (username != null)
                {
                    ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();
                    paradisoObjectManager.UserName = username;
                    NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Invalid username/password.");
                }
            }

        }
    }
}
