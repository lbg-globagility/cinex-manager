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
using MahApps.Metro.Controls;
using aZynEManager;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Amellar.Common.EncryptUtilities;
using MySql.Data.MySqlClient;
using System.Windows.Media.Animation;

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private frmMain main;
        string sysName = String.Empty;
        string sysDesc = String.Empty;
        string sysLoginTitle = "Please login to your account";
        string userID = String.Empty;
        string userNm = String.Empty;

        string mod1title = String.Empty;
        string mod1desc = String.Empty;
        string mod2title = String.Empty;
        string mod2desc = String.Empty;
        string mod3title = String.Empty;
        string mod3desc = String.Empty;
        string mod4title = String.Empty;
        string mod4desc = String.Empty;
        bool canvasrun = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
            main = new frmMain();
            UserName.Focus();
            UserName.TabIndex = 1;
            UserPassword.TabIndex = 2;
            
            DescBorder.Visibility = System.Windows.Visibility.Visible;

            if (main.boolAppAtTest)
            {
                MoviesTile.IsEnabled = true;
                ReportsTile.IsEnabled = true;
                SetupTile.IsEnabled = true;
                AdministrationTile.IsEnabled = true;

                btnLogout.Visibility = System.Windows.Visibility.Visible;
                LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MoviesTile.IsEnabled = false;
                ReportsTile.IsEnabled = false;
                SetupTile.IsEnabled = false;
                AdministrationTile.IsEnabled = false;

                btnLogout.Visibility = System.Windows.Visibility.Collapsed;
                LoginGrid.Visibility = System.Windows.Visibility.Visible;
            }

            CommonLibrary.CommonUtility util = new CommonLibrary.CommonUtility();
            util.SetModuleInfo();
            sysName = util.sysname;
            sysDesc = util.sysdesc;
            mod1title = util.mod1title;
            mod1desc = util.mod1desc;
            mod2title = util.mod2title;
            mod2desc = util.mod2desc;
            mod3title = util.mod3title;
            mod3desc = util.mod3desc;
            mod4title = util.mod4title;
            mod4desc = util.mod4desc;

            ////visibility of the login page

            TileDesc.Text = sysDesc;
            if (LoginGrid.Visibility == System.Windows.Visibility.Visible)
            {
                TileDesc.Visibility = System.Windows.Visibility.Collapsed;
                TileTitle.Text = sysLoginTitle;
            }
            else
            {
                TileDesc.Visibility = System.Windows.Visibility.Visible;
                TileTitle.Text = sysName;
            }
        }



        public void menuItem_CheckedChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem mitem = ((System.Windows.Controls.MenuItem)sender).Parent as System.Windows.Controls.MenuItem;
            foreach (System.Windows.Controls.MenuItem smitem in mitem.Items)
            {
                if (smitem.Header != ((System.Windows.Controls.MenuItem)sender).Header)
                    smitem.IsChecked = false;
                else
                {
                    System.Drawing.Color bgcolor = System.Drawing.Color.Transparent;
                    switch (smitem.Header.ToString())
                    {
                        case "Green":
                            bgcolor = System.Drawing.Color.LawnGreen;
                            break;
                        case "Blue":
                            bgcolor = System.Drawing.Color.DodgerBlue;
                            break;
                        case "Red":
                            bgcolor = System.Drawing.Color.Salmon;
                            break;
                        case "Purple":
                            bgcolor = System.Drawing.Color.Purple;
                            break;
                        case "White":
                            bgcolor = System.Drawing.Color.LightSlateGray;
                            break;
                        case "Yellow":
                            bgcolor = System.Drawing.Color.Gold;
                            break;
                    }
                    main.SkinLineColor = bgcolor;
                    main.SkinBackColor = bgcolor;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginClick();
        }

        public void LoginClick()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            string strUserName = UserName.Text.Trim();
            string strPassword = UserPassword.Password.Trim();

            if (strUserName == string.Empty)
            {
                Mouse.OverrideCursor = null;
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Missing User Name.";
                messageWindow.ShowDialog();
                
                return;
            }
            else if (strPassword == string.Empty)
            {
                Mouse.OverrideCursor = null;
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Missing Password.";
                messageWindow.ShowDialog();
                
                return;
            }

            Encryption encryption = new Encryption();
            strPassword = encryption.EncryptString(strPassword);

            MySqlConnection myconn = new MySqlConnection();
            myconn.ConnectionString = main._connection;

            //validate for the existance of the record
            StringBuilder sqry = new StringBuilder();

            sqry = new StringBuilder();
            sqry.Append("select a.id, a.userid, concat(a.fname,' ',a.mname,'. ',a.lname) as username, b.level_desc  from users a, user_level b ");
            sqry.Append(String.Format("where a.userid = '{0}' ", strUserName));
            sqry.Append("and a.user_level_id = b.id ");
            sqry.Append(String.Format("and a.user_password = '{0}' ", strPassword));
            sqry.Append(String.Format("and a.system_code = {0}", "1"));

            try
            {
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        int intid = Convert.ToInt32(rd["id"].ToString());
                        userID = rd["userid"].ToString();
                        userNm = rd["username"].ToString();
                        string strleveldesc = rd["level_desc"].ToString();
                        rd.Close();
                        cmd.Dispose();

                        sqry = new StringBuilder();
                        sqry.Append(String.Format("select count(*) from user_logs where user_id = {0}", intid));
                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        if (rowCount > 0)
                        {
                            Mouse.OverrideCursor = null;
                            if (myconn.State == ConnectionState.Open)
                                myconn.Close();

                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = "This user is already \n\rlogged-in to the system.";
                            messageWindow.ShowDialog();

                            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
                            messageYesNoWindow.MessageText.Text = "Do you want to access the \n\r task manager control?";
                            messageYesNoWindow.ShowDialog();
                            if (!messageYesNoWindow.IsYes)
                                return;

                            //view the task manager form iff the user has grant
                            bool isEnabled = main.m_clscom.verifyUserRights(intid, "TASK_VIEW", main._connection);
                            if (main.boolAppAtTest == true)
                                isEnabled = main.boolAppAtTest;

                            if (isEnabled == false)
                            {
                                Mouse.OverrideCursor = null;
                                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                                messageWindow = new MessageWindow();
                                messageWindow.MessageText.Text = "You have no access to this module.";
                                messageWindow.ShowDialog();
                                
                                return;
                            }

                            main.m_userid = intid;
                            frmTaskManager frmtm = new frmTaskManager();
                            frmtm.frmInit(main, main.m_clscom);
                            frmtm.ShowDialog();
                            frmtm.Dispose();
                            main.m_userid = -1;
                            Mouse.OverrideCursor = null;
                            return;
                        }

                        main.m_userid = intid;
                        main.UserCode = userID;
                        main.UserID = intid;
                        main.UserName = userNm;

                        ////if logged in
                        LeftToRightMarquee();
                        //LoggedUser.Text = "User Name: " + userNm;// +"  User Level: " + strleveldesc;

                        //RightToLeftMarquee();
                        //LoggedUser.Text = "To God Alone Be The Glory";// +"  User Level: " + strleveldesc;

                        MoviesTile.IsEnabled = true;
                        ReportsTile.IsEnabled = true;
                        SetupTile.IsEnabled = true;
                        AdministrationTile.IsEnabled = true;

                        LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
                        TileDesc.Text = sysDesc;

                        if (LoginGrid.Visibility == System.Windows.Visibility.Visible)
                        {
                            TileDesc.Visibility = System.Windows.Visibility.Collapsed;
                            TileTitle.Text = sysLoginTitle;
                        }
                        else
                        {
                            TileDesc.Visibility = System.Windows.Visibility.Visible;
                            TileTitle.Text = sysName;
                        }

                        btnLogout.Visibility = System.Windows.Visibility.Visible;

                        main.m_clscom.AddATrail(intid, "LOGIN", "USER_LOGS",
                            Environment.MachineName.ToString(), "LOGGED-IN THE SYSTEM: NAME=" + userNm
                            + " | ID=" + intid.ToString(), main._connection);

                        StringBuilder strqry = new StringBuilder();
                        strqry.Append("insert into user_logs values(0,");
                        strqry.Append(String.Format("'{0}',", Environment.MachineName.ToString()));
                        strqry.Append(String.Format("{0},",intid));
                        strqry.Append(String.Format("'{0}',", strleveldesc));
                        strqry.Append(String.Format("{0}","Now())"));

                        if (myconn.State == ConnectionState.Closed)
                            myconn.Open();
                        if (myconn.State == ConnectionState.Open)
                        {
                            cmd = new MySqlCommand(strqry.ToString(), myconn);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            myconn.Close();
                        }

                        break;
                    }
                    rd.Close(); // 1.5.2016
                    cmd.Dispose();
                }
                else
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Invalid username/password.";
                    messageWindow.ShowDialog();
                }
                cmd.Dispose();
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                Mouse.OverrideCursor = null;
            }
            catch
            {
                
                if (myconn != null)
                {
                    if (myconn.State == ConnectionState.Open)
                        myconn.Close();
                }

                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Database Connection Error";
                messageWindow.ShowDialog();
                Mouse.OverrideCursor = null;
            }
        }

        #region mousecontrol
        private void MovieTile_MouseOver(object sender, RoutedEventArgs e)
        {
            TileTitle.Text =  mod1title;
            TileDesc.Text = mod1desc;
        }

        private void ReportTile_MouseOver(object sender, RoutedEventArgs e)
        {
            TileTitle.Text = mod2title;
            TileDesc.Text = mod2desc;
        }

        private void SetupTile_MouseOver(object sender, RoutedEventArgs e)
        {
            TileTitle.Text = mod3title;
            TileDesc.Text = mod3desc;
        }

        private void AdministrationTile_MouseOver(object sender, RoutedEventArgs e)
        {
            TileTitle.Text = mod4title;
            TileDesc.Text = mod4desc;
        }

        private void Tile_MouseLeave(object sender, RoutedEventArgs e)
        {
            TileTitle.Text = sysName;
            TileDesc.Text = sysDesc;
        }
        #endregion

        private void MoviesTile_Click(object sender, RoutedEventArgs e)
        {
 

            main.m_dtcontact = main.m_clscom.setDataTable("select * from people order by id asc", main._connection);
            main.m_dtdistributor = main.m_clscom.setDataTable("select * from distributor order by name asc", main._connection);
            main.m_dtrating = main.m_clscom.setDataTable("select * from mtrcb order by id asc", main._connection);
            main.m_dtclassification = main.m_clscom.setDataTable("select * from classification order by description asc", main._connection );

            using (frmMainMovie frmmovie = new frmMainMovie())
            {
                frmmovie.setSkin(main._backcolor, main._linecolor);
                frmmovie.frmInit(main, main.m_clscom);
                frmMainMovie.SetOwner(frmmovie, this);
                frmmovie.ShowDialog();
                frmmovie.Dispose();
            }
        }

        private void ReportsTile_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(main);
            reportWindow.Owner = this;
            reportWindow.Show();
        }

        private void SetupTile_Click(object sender, RoutedEventArgs e)
        {
            using (frmMainUtility frmmu = new frmMainUtility())
            {
                frmmu.setSkin(main._backcolor, main._linecolor);
                frmmu.frmInit(main, main.m_clscom);
                frmMainMovie.SetOwner(frmmu, this);
                frmmu.ShowDialog();
                frmmu.Dispose();
            }
        }

        private void AdministrationTile_Click(object sender, RoutedEventArgs e)
        {
            //SeatWindow seatWindow = new SeatWindow();
            //seatWindow.LoadCinema(2);
            //seatWindow.Owner = this;
            //seatWindow.Show();
            using (frmMainConfig frmcon = new frmMainConfig())
            {
                frmcon.setSkin(main._backcolor, main._linecolor);
                frmcon.frmInit(main, main.m_clscom);
                frmMainMovie.SetOwner(frmcon, this);
                frmcon.ShowDialog();
                frmcon.Dispose();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (main._connection != null && main != null)
                result = SystemLogout();
            
            ////if logged out to the system
            if (result)
            {
                ////for the canvas of the logged user
                LoggedUser.Text = "";
                LoggedUser.BeginAnimation(Canvas.RightProperty, null);
                canvasrun = false;

                ////for the internal variables
                btnLogout.Visibility = System.Windows.Visibility.Collapsed;
                main.m_userid = -1;
                main.UserCode = "";
                main.UserID = -1;
                main.UserName = "";

                MoviesTile.IsEnabled = false;
                ReportsTile.IsEnabled = false;
                SetupTile.IsEnabled = false;
                AdministrationTile.IsEnabled = false;

                ////for the login grid
                LoginGrid.Visibility = System.Windows.Visibility.Visible;
                if (LoginGrid.Visibility == System.Windows.Visibility.Visible)
                {
                    TileDesc.Visibility = System.Windows.Visibility.Collapsed;
                    TileTitle.Text = sysLoginTitle;
                }
                else
                {
                    TileDesc.Visibility = System.Windows.Visibility.Visible;
                    TileTitle.Text = sysName;
                }

                UserName.SelectAll();
                UserPassword.Clear();
                UserName.Focus();
            }
        }

        public bool SystemLogout()
        {
            bool result = false;
            main.m_clscom.AddATrail(main.m_userid, "LOGOUT", "USER_LOGS, USER_LOGS_TEMP",
                                Environment.MachineName.ToString(), "LOGGED-OUT FROM THE SYSTEM: NAME=" + main.UserName + " | ID=" + (main.m_userid).ToString(), main._connection);

            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_logs where user_id = {0}", main.m_userid));

            MySqlConnection myconn = new MySqlConnection();
            try
            {
                myconn.ConnectionString = main._connection;
                if (myconn.State == ConnectionState.Closed)
                    myconn.Open();
                MySqlCommand cmd = new MySqlCommand(sqry.ToString(), myconn);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                if (rowCount > 0)
                {
                    sqry = new StringBuilder();
                    sqry.Append("insert into user_logs_temp select 0, b.computer_name, b.user_id, b.user_authority, b.time_in, Now() ");
                    sqry.Append(String.Format("from user_logs b where b.user_id = {0}", main.m_userid));

                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    if (myconn.State == ConnectionState.Open)
                    {
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        myconn.Close();
                    }

                    sqry = new StringBuilder();
                    sqry.Append(String.Format("delete from user_logs where user_id = {0}", main.m_userid));
                    if (myconn.State == ConnectionState.Closed)
                        myconn.Open();
                    if (myconn.State == ConnectionState.Open)
                    {
                        cmd = new MySqlCommand(sqry.ToString(), myconn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        myconn.Close();
                    }

                    result = true;
                }
            }
            catch
            {
                if (myconn.State == ConnectionState.Open)
                    myconn.Close();

                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Database Connection Error";
                messageWindow.ShowDialog();
                return result;
            }
            return result;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (main.m_userid != -1)
            {
                string strUserName = UserName.Text.Trim();
                string strPassword = UserPassword.Password.Trim();

                if (strUserName == string.Empty || strPassword == string.Empty)
                {
                    ////close the system if the username and password is not filled
                }
                else
                {
                    bool result = SystemLogout();
                    if (!result)
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "Logout Error";
                        messageWindow.ShowDialog();
                    }
                }
            }
        }

        private void RightToLeftMarquee()
        {
            double height = canMain.ActualHeight - LoggedUser.ActualHeight;
            LoggedUser.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -LoggedUser.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.AutoReverse = false;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:15"));

            LoggedUser.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
            canvasrun = true;
        }

        private void LeftToRightMarquee()
        {
            double height = canMain.ActualHeight - LoggedUser.ActualHeight;
            LoggedUser.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -LoggedUser.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.AutoReverse = false;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:08"));

            LoggedUser.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
            LoggedUser.Text = "Logged User: " + main.UserName;
            canvasrun = true;
        }

        private void canMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string suser = LoggedUser.Text;
            if (canvasrun)
            {
                double height = canMain.ActualHeight - LoggedUser.ActualHeight;
                LoggedUser.Margin = new Thickness(0, height / 2, 0, 0);
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = -LoggedUser.ActualWidth;
                doubleAnimation.To = canMain.ActualWidth;
                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                doubleAnimation.AutoReverse = false;
                doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:15"));

                LoggedUser.Text = "TO GOD ALONE BE THE GLORY";
                LoggedUser.BeginAnimation(Canvas.RightProperty, doubleAnimation);
                canvasrun = false;
            }
            else
                LeftToRightMarquee();
        }

        private void UserPassword_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginClick();
            }
        }

        private void UserName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (UserPassword.Focusable)
                    Keyboard.Focus(UserPassword);
            }
        }

        private void MovieTile_MouseOver(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
    }
    
}
