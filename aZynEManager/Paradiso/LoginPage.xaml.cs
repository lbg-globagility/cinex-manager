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
using System.Windows.Threading;
using System.ComponentModel;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page //, INotifyPropertyChanged
    {

        private DispatcherTimer dispatcherTimer;

        //private string m_strCurrentDate;

        public LoginPage()
        {
            InitializeComponent();
            this.DataContext = this;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            //dispatcherTimer.Start();

        }

        /*
        public string XCurrentDate
        {
            get { return m_strCurrentDate; }
            set
            {
                m_strCurrentDate = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("XCurrentDate"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        */

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("Hello");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            //using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Invalid username/password.";
                    messageWindow.ShowDialog();
                }
            }

        }
    }
}
