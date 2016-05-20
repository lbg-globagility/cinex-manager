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
using Paradiso.Model;
using System.Collections.ObjectModel;
using System.Collections;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for EndTellerSessionPage.xaml
    /// </summary>
    public partial class EndTellerSessionPage : UserControl, IDisposable
    {

        public ObservableCollection<TicketSessionModel> TellerSessions { get; set; }

        public EndTellerSessionPage()
        {
            InitializeComponent();

            TellerSessions = new ObservableCollection<TicketSessionModel>();
            this.LoadTellerSessions();

            this.DataContext = this;
        }

        private void LoadTellerSessions()
        {
            TellerSessions.Clear();
            DateTime dtCurrent = ParadisoObjectManager.GetInstance().CurrentDate;
            try
            {
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var trail = (from at in context.a_trail
                                 where at.tr_date.Year == dtCurrent.Year && at.tr_date.Month == dtCurrent.Month
                                     && at.tr_date.Day == dtCurrent.Day && at.module_code == 42 //hack
                                 group at by at.user_id into g
                                 select new { g.Key, MaxId = g.Max(x => x.id) }).ToList()
                                 ;
                    foreach (var __trail in trail)
                    {
                        var _trail = (from at in context.a_trail where at.id == __trail.MaxId select at).FirstOrDefault();
                        if (_trail != null)
                        {
                            string strDetails = _trail.tr_details;
                            if (strDetails.StartsWith("LOGIN OK-"))
                            {
                                string strSessionId = string.Empty;
                                if (strDetails.IndexOf("(") != -1)
                                    strSessionId = strDetails.Substring(strDetails.IndexOf("(") + 1);
                                if (strSessionId != string.Empty)
                                    strSessionId = strSessionId.Substring(0, strSessionId.Length - 1);
                                TellerSessions.Add(new TicketSessionModel()
                                {
                                    Id = _trail.user_id,
                                    Terminal = _trail.computer_name,
                                    User = _trail.user.userid,
                                    TicketDateTime = _trail.tr_date,
                                    SessionId = strSessionId
                                });
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void EndSession_Click(object sender, RoutedEventArgs e)
        {
            if (TicketSessionDataGrid.SelectedIndex != -1)
            {
                try
                {
                    TicketSessionModel ts = (TicketSessionModel) TicketSessionDataGrid.SelectedItem;

                    ParadisoObjectManager.GetInstance().Log(ts.Id, "LOGIN", "TICKET|LOGOUT",
                        string.Format("LOGOUT OK-{0} ({1})",  ts.User , ts.SessionId));
                    this.LoadTellerSessions();
                }
                catch { }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.SwitchContent("MovieCalendarPage.xaml");

            //NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.LoadTellerSessions();
        }

        #region IDisposable Members

        public void Dispose()
        {
            TellerSessions.Clear();
        }

        #endregion
    }
}
