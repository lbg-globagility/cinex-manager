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
using Amellar.Common.EncryptUtilities;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for LoginPage1.xaml
    /// </summary>
    public partial class LoginPage : UserControl, IDisposable   
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
            DateTime dtCurrent = paradisoObjectManager.CurrentDate;

            try
            {
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {

                    var username = (from u in context.users where u.userid == strUserName && u.user_password == strPassword && u.system_code == 2 && u.status == 1 select new { u.id, u.lname, u.fname, u.mname }).FirstOrDefault();

                    if (username != null)
                    {
                        //get last trail for LOGIN 
                        var trail = (from at in context.a_trail
                                     where at.user.userid == strUserName && at.module_code == 42 && at.tr_date.Year == dtCurrent.Year &&
                                     at.tr_date.Month == dtCurrent.Month && at.tr_date.Day == dtCurrent.Day && at.module_code == 42  //modify if login module id has been replaced
                                     orderby at.id descending
                                     select
                                         new { at.computer_name, at.aff_table_layer }).FirstOrDefault();

                        //checks if already logged in on different machine on same date
                        if (trail != null)
                        {
                            if (trail.aff_table_layer == "TICKET|LOGIN" && trail.computer_name != string.Format("{0}", Environment.MachineName.ToString()))
                            {
                                MessageWindow messageWindow = new MessageWindow();
                                messageWindow.MessageText.Text = string.Format("Teller is already logged in at terminal {0}.", trail.computer_name);
                                messageWindow.ShowDialog();
                                UserName.Text = string.Empty;
                                Password.Clear();
                                if (UserName.Focusable)
                                    Keyboard.Focus(UserName);
                                return;
                            }
                        }
                        

                        paradisoObjectManager.UserId = username.id;
                        paradisoObjectManager.UserLogInName = strUserName;
                        paradisoObjectManager.UserName = string.Format("{0} {1}. {2}", username.fname, username.mname, username.lname);
                        
                        paradisoObjectManager.SessionId = paradisoObjectManager.NewSessionId;

                        paradisoObjectManager.Log("LOGIN", "TICKET|LOGIN", string.Format("LOGIN OK-{0} ({1})", strUserName, paradisoObjectManager.SessionId));

                        /*
                        if (NavigationService != null) 
                            //NavigationService.Navigate(new Uri("TenderCreditPage.xaml", UriKind.Relative));
                            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                        */

                        MainWindow window = (MainWindow)Window.GetWindow(this);
                        window.SwitchContent("MovieCalendarPage.xaml");
                    }
                    else
                    {

                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "Invalid username/password.";
                        messageWindow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                ParadisoObjectManager.GetInstance().MessageBox(ex);
            }

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double top = (MainCanvas.ActualHeight - PopcornImage.ActualHeight) * 0.3;
            if (top < 0)
                top = 0;

            double left = (MainCanvas.ActualWidth - PopcornImage.ActualWidth - LoginBorder.ActualWidth) * 0.5;
            if (left < 0)
                left = 0;

            Canvas.SetTop(PopcornImage, top);
            Canvas.SetTop(LoginBorder, top + (PopcornImage.ActualHeight - LoginBorder.ActualHeight) + 20);
            Canvas.SetLeft(PopcornImage, left);
            Canvas.SetLeft(LoginBorder, left + PopcornImage.ActualWidth);

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Version.Text = ParadisoObjectManager.GetInstance().Version;
            if (UserName.Focusable)
                Keyboard.Focus(UserName);
            /*
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            */
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.Collect();
        }

        #endregion
    }
}
