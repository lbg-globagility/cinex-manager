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
using Amellar.Common.EncryptUtilities;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for LoginPage1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Login();
        }

        private void Login()
        {
            string strUserName = UserName.Text;
            string strPassword = Password.Password;

            if (strUserName == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Missing User Name.";
                messageWindow.ShowDialog();
                return;
            }
            else if (strPassword == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Missing Password.";
                messageWindow.ShowDialog();
                return;
            }

            Encryption encryption = new Encryption();
            strPassword = encryption.EncryptString(strPassword);
            
            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var username = (from u in context.users where u.userid == strUserName && u.user_password == strPassword && u.system_code == 2 select new { u.id, u.lname, u.fname, u.mname }).FirstOrDefault();
                if (username != null)
                {
                    paradisoObjectManager.UserId = username.id;
                    paradisoObjectManager.UserName = string.Format("{0} {1}. {2}", username.fname, username.mname, username.lname);
                    paradisoObjectManager.SessionId = paradisoObjectManager.NewSessionId;
                    
                    paradisoObjectManager.Log("LOGIN", "TICKET|LOGIN", string.Format("LOGIN OK-{0} ({1})", strUserName, paradisoObjectManager.SessionId));

                    NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                }
                else
                {

                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Invalid username/password.";
                    messageWindow.ShowDialog();
                }
            }

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double top = (MainCanvas.ActualHeight - LoginBorder.ActualHeight) * 0.5;
            if (top < 0)
                top = 0;
            Canvas.SetTop(LoginBorder, top);
            Canvas.SetTop(PopcornImage, top + 85);
        }

        private void UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Password.Focusable = true;
                Keyboard.Focus(Password);
            }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Login();
            }
        }
    }
}
