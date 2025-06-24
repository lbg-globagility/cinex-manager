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
using System.Threading;
using System.Windows.Threading;
using System.Printing;
using Paradiso.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TicketPrintPage.xaml
    /// </summary>
    public partial class TicketPrintPage2 : UserControl, IDisposable
    {
        public TicketModel Ticket { get; set;}

        public ObservableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }

        public ObservableCollection<TicketSessionModel> TicketSessions { get; set; }
        public TicketListModel TicketList { get; set; }

        //public ObservableCollection<TicketModel> Tickets { get; set; }

        public ObservableCollection<string> CancelledORNumbers { get; set; }
        public readonly string OfficialReceiptShortCaptionText;

        private BackgroundWorker worker;
        private BackgroundWorker worker1;

        public TicketPrintPage2()
        {
            OfficialReceiptShortCaptionText = SettingPage.OFFICIAL_RECEIPT_SHORT_CAPTION;

            this.Initialize(false);
        }

        public TicketPrintPage2(bool blnIsReset)
        {
            OfficialReceiptShortCaptionText = SettingPage.OFFICIAL_RECEIPT_SHORT_CAPTION;

            this.Initialize(blnIsReset);
        }


        #region IDisposable Members

        public void Dispose()
        {
            Users.Clear();
            TicketSessions.Clear();
            TicketList.Dispose();
            CancelledORNumbers.Clear();

            try
            {
                worker.DoWork -= worker_DoWork;
                worker.ProgressChanged -= worker_ProgressChanged;
                worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
                worker1.DoWork -= worker1_DoWork;
                worker1.ProgressChanged -= worker1_ProgressChanged;
                worker1.RunWorkerCompleted -= worker1_RunWorkerCompleted;
            }
            catch { }
        }

        #endregion

        public void Initialize(bool blnIsReset)
        {
            InitializeComponent();

            Ticket = new TicketModel();

            TicketSessions = new ObservableCollection<TicketSessionModel>();
            TicketList = new TicketListModel();

            CancelledORNumbers = new ObservableCollection<string>();

            Users = new ObservableCollection<UserModel>();
            this.PopulateUsers();

            if (blnIsReset)
                this.Reset();

            this.DataContext = this;

            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();
            bool blnHasVoid = paradisoObjectManager.HasRights("VOID");
            bool blnHasReprint = paradisoObjectManager.HasRights("REPRINT");

            //blnHasReprint = true; //for debugging

            if (blnHasVoid && !blnHasReprint)
            {
                Grid.SetColumn(Void, 1);
            }
            if (blnHasVoid)
                Void.Visibility = Visibility.Visible;
            else
                Void.Visibility = Visibility.Hidden;
                
            if (blnHasReprint)
                Print.Visibility = Visibility.Visible;
            else
                Print.Visibility = Visibility.Hidden;

            Version.Text = ParadisoObjectManager.GetInstance().Version;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker1 = new BackgroundWorker();
            worker1.WorkerReportsProgress = true;
            worker1.DoWork += new DoWorkEventHandler(worker1_DoWork);
            worker1.ProgressChanged += new ProgressChangedEventHandler(worker1_ProgressChanged);
            worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker1_RunWorkerCompleted);

            var officialReceiptCaptionText = SettingPage.OFFICIAL_RECEIPT_CAPTION;
            TextBoxOfficialReceiptCaption1.Text = officialReceiptCaptionText;
            TextBoxOfficialReceiptCaption2.Text = officialReceiptCaptionText;
        }

        private void EnableControls(bool blnEnable)
        {
            TicketPanel.IsEnabled = blnEnable;
            ORNumberInput.IsEnabled = blnEnable;
            UsersComboBox.IsEnabled = blnEnable;
            chkSessionOnly.IsEnabled = blnEnable;
            SearchTicket.IsEnabled = blnEnable;
            Clear.IsEnabled = blnEnable;
            TicketDataGrid.IsEnabled = blnEnable;
            Void.IsEnabled = blnEnable;
            Print.IsEnabled = blnEnable;
            CancelPrint.IsEnabled = blnEnable;
        }

        void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            string strResult = string.Empty;
            if (e.Result != null)
                strResult = e.Result.ToString();

            Cursor = null;
            this.EnableControls(true);

            if (strResult == "Missing search text.")
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = strResult;
                messageWindow.ShowDialog();

                if (ORNumberInput.Focusable)
                    ORNumberInput.Focus();
            }
            else if (strResult != string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = strResult;
                messageWindow.ShowDialog();
            }
        }

        void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            progressBar2.Value = e.ProgressPercentage;
        }

        void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SearchOptions so = (SearchOptions)e.Argument;

            string error = string.Empty;

            string strSearch = so.searchText;
            var originTextSearch = strSearch;
            if ((strSearch == string.Empty && so.selectedUserKey == 0) ||
                (strSearch == string.Empty && so.selectedUserKey != 0 && so.isPromptNotFound == false))
            {
                e.Result = "Missing search text.";
                return;
            }

            bool blnIsORNumber = false;
            //checks if format is ornumber or session
            if (!so.isSessionOnly)
            {
                if (strSearch.All(Char.IsDigit) && !strSearch.StartsWith("201"))
                {
                    int _orNumber = 0;
                    if (int.TryParse(strSearch, out _orNumber) && _orNumber > 0)
                    {
                        blnIsORNumber = true;
                        strSearch = string.Format("{0:000000000}", _orNumber);
                    }
                    else
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = $"{OfficialReceiptShortCaptionText} Number is invalid.";
                        messageWindow.ShowDialog();

                        if (ORNumberInput.Focusable)
                            ORNumberInput.Focus();
                        return;
                    }
                }
            }

            Dispatcher.Invoke((Action)(()=>
                TicketSessions.Clear()
            ));
            Dispatcher.Invoke((Action)(()=>
            TicketList.Tickets.Clear()
            ));

            //search ornumber or session number
            using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                if (so.isSessionOnly == false)
                {

                    //ticket and details
                    var queryTickets = (from mslrs in context.movies_schedule_list_reserved_seat
                        where mslrs.or_number == strSearch && mslrs.status == 1
                        orderby mslrs.ticket.ticket_datetime descending
                        select mslrs);

                    if (!(queryTickets?.Any() ?? false))
                        queryTickets = (from mslrs in context.movies_schedule_list_reserved_seat
                            where mslrs.status == 1 &&
                            mslrs.ticket.session_id.StartsWith(originTextSearch)
                            orderby mslrs.ticket.ticket_datetime descending
                            select mslrs);

                    var tickets = queryTickets
                        .Select(mslrs => new
                        {
                            ticketid = mslrs.ticket_id,
                            ticketdatetime = mslrs.ticket.ticket_datetime,
                            terminalname = mslrs.ticket.terminal,
                            tellercode = mslrs.ticket.user.userid,
                            session = mslrs.ticket.session_id,
                            id = mslrs.id,
                            cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                            moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                            rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                            seattype = mslrs.movies_schedule_list.seat_type,
                            startdate = mslrs.movies_schedule_list.start_time,
                            patroncode = mslrs.movies_schedule_list_patron.patron.code,
                            patronname = mslrs.movies_schedule_list_patron.patron.name,
                            patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                            seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                            price = mslrs.price,
                            ornumber = mslrs.or_number,
                            at = mslrs.amusement_tax_amount,
                            ct = mslrs.cultural_tax_amount,
                            vt = mslrs.vat_amount,
                            isvoid = (mslrs.void_datetime != null)
                        })
                        .ToList();

                    worker1.ReportProgress(25);
                    
                    if (tickets.Count == 0)
                        blnIsORNumber = false;

                    if (!blnIsORNumber && so.selectedUserKey != 0 && strSearch == string.Empty)
                    {
                        tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where
                                   mslrs.status == 1
                                   && mslrs.ticket.user.id == so.selectedUserKey
                                   orderby mslrs.ticket.ticket_datetime descending
                                   select new
                                   {
                                       ticketid = mslrs.ticket_id,
                                       ticketdatetime = mslrs.ticket.ticket_datetime,
                                       terminalname = mslrs.ticket.terminal,
                                       tellercode = mslrs.ticket.user.userid,
                                       session = mslrs.ticket.session_id,

                                       id = mslrs.id,
                                       cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                       moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                       rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                       seattype = mslrs.movies_schedule_list.seat_type,
                                       startdate = mslrs.movies_schedule_list.start_time,
                                       patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                       patronname = mslrs.movies_schedule_list_patron.patron.name,
                                       patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                       seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                       price = mslrs.price,
                                       ornumber = mslrs.or_number,
                                       at = mslrs.amusement_tax_amount,
                                       ct = mslrs.cultural_tax_amount,
                                       vt = mslrs.vat_amount,

                                       isvoid = (mslrs.void_datetime != null),
                                   }).ToList();
                    }
                    else if (!blnIsORNumber && so.selectedUserKey != 0)
                    {
                        tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.ticket.session_id.StartsWith(strSearch) && mslrs.ticket.user.id == so.selectedUserKey && mslrs.status == 1
                                   orderby mslrs.ticket.ticket_datetime descending
                                   select new
                                   {
                                       ticketid = mslrs.ticket_id,
                                       ticketdatetime = mslrs.ticket.ticket_datetime,
                                       terminalname = mslrs.ticket.terminal,
                                       tellercode = mslrs.ticket.user.userid,
                                       session = mslrs.ticket.session_id,

                                       id = mslrs.id,
                                       cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                       moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                       rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                       seattype = mslrs.movies_schedule_list.seat_type,
                                       startdate = mslrs.movies_schedule_list.start_time,
                                       patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                       patronname = mslrs.movies_schedule_list_patron.patron.name,
                                       patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                       seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                       price = mslrs.price,
                                       ornumber = mslrs.or_number,
                                       at = mslrs.amusement_tax_amount,
                                       ct = mslrs.cultural_tax_amount,
                                       vt = mslrs.vat_amount,

                                       isvoid = (mslrs.void_datetime != null),
                                   }).ToList();

                    }
                    else if (!blnIsORNumber)
                    {
                        tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.ticket.session_id.StartsWith(strSearch) && mslrs.status == 1
                                   orderby mslrs.ticket.ticket_datetime descending
                                   select new
                                   {
                                       ticketid = mslrs.ticket_id,
                                       ticketdatetime = mslrs.ticket.ticket_datetime,
                                       terminalname = mslrs.ticket.terminal,
                                       tellercode = mslrs.ticket.user.userid,
                                       session = mslrs.ticket.session_id,

                                       id = mslrs.id,
                                       cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                       moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                       rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                       seattype = mslrs.movies_schedule_list.seat_type,
                                       startdate = mslrs.movies_schedule_list.start_time,
                                       patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                       patronname = mslrs.movies_schedule_list_patron.patron.name,
                                       patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                       seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                       price = mslrs.price,
                                       ornumber = mslrs.or_number,
                                       at = mslrs.amusement_tax_amount,
                                       ct = mslrs.cultural_tax_amount,
                                       vt = mslrs.vat_amount,

                                       isvoid = (mslrs.void_datetime != null),
                                   }).ToList();
                    }

                    worker1.ReportProgress(50);

                    //show all tickets in initial search
                    int index = 0;
                    int max = tickets.Count;
                    int percentage = 0;

                    DateTime dtCurrentDateTime = ParadisoObjectManager.GetInstance().CurrentDate;
                    foreach (var t in tickets)
                    {
                        index++;
                        percentage = ((index * 50) / max) + 50;
                        worker1.ReportProgress(percentage);

                        if (TicketSessions.Where(ts => ts.Id == t.ticketid).Count() == 0)
                        {
                            Dispatcher.Invoke((Action)(() =>
                            
                                TicketSessions.Add(new TicketSessionModel()
                                {
                                    Id = t.ticketid,
                                    SessionId = t.session,
                                    Terminal = t.terminalname,
                                    User = t.tellercode,
                                    TicketDateTime = (DateTime)t.ticketdatetime
                                })
                            ));
                        }

                        Dispatcher.Invoke((Action)(() =>
                            TicketList.Tickets.Add(new TicketModel()
                            {
                                Id = t.id,
                                CinemaNumber = t.cinemanumber,
                                MovieCode = t.moviecode,
                                Rating = t.rating,
                                SeatType = t.seattype,
                                StartTime = t.startdate,
                                PatronCode = t.patroncode,
                                PatronName = t.patronname,
                                PatronPrice = (decimal)t.price,
                                PatronDescription = t.patrondescription,
                                SeatName = t.seatname,
                                ORNumber = t.ornumber,
                                AmusementTax = (decimal)t.at,
                                CulturalTax = (decimal)t.ct,
                                VatTax = (decimal)t.vt,
                                TerminalName = t.terminalname,
                                TellerCode = t.tellercode,
                                SessionName = t.session,
                                CurrentTime = dtCurrentDateTime,
                                IsVoid = t.isvoid,
                                IsSelected = false
                            })
                        ));
                    }

                    if (tickets.Count == 0 && so.isPromptNotFound)
                    {
                        e.Result = "No ticket(s) found.";
                        return;
                    }
                }
                else
                {
                    //make sure cinema is not yet expired

                    var tickets = (from mslhs in context.movies_schedule_list_house_seat
                                   where mslhs.session_id.StartsWith(strSearch)
                                   orderby mslhs.reserved_date descending
                                   select new
                                   {
                                       ticketid = -1,
                                       ticketdatetime = mslhs.reserved_date,
                                       terminalname = "",
                                       tellercode = "",
                                       session = mslhs.session_id,

                                       id = mslhs.id,
                                       cinemanumber = mslhs.movies_schedule_list.movies_schedule.cinema.in_order,
                                       moviecode = mslhs.movies_schedule_list.movies_schedule.movie.title,
                                       rating = mslhs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                       seattype = mslhs.movies_schedule_list.seat_type,
                                       startdate = mslhs.movies_schedule_list.start_time,
                                       enddate = mslhs.movies_schedule_list.end_time,
                                       patroncode = mslhs.movies_schedule_list_patron.patron.code,
                                       patronname = mslhs.movies_schedule_list_patron.patron.name,
                                       patrondescription = mslhs.movies_schedule_list_patron.patron.name,
                                       seatname = mslhs.cinema_seat.col_name + mslhs.cinema_seat.row_name,
                                       price = mslhs.movies_schedule_list_patron.price,
                                       ornumber = "RESERVED",
                                       at = 0.0,
                                       ct = 0.0,
                                       vt = 0.0,
                                       isvoid = false,
                                   }).ToList();
                    //remove expired

                    DateTime dtCurrentDateTime = ParadisoObjectManager.GetInstance().CurrentDate;
                    tickets = tickets.Where(t => t.enddate > dtCurrentDateTime && t.ticketdatetime > dtCurrentDateTime.AddMinutes(-10)).ToList();
                    //update teller
                    int intCount = tickets.Count;
                    for (int i = 0; i < intCount; i++)
                    {
                        int intUserId = -1;
                        string strTellerCode = string.Empty;

                        var t = tickets[i];


                        int intIndex = t.session.LastIndexOf('-');
                        if (intIndex != -1)
                        {
                            int.TryParse(t.session.Substring(intIndex + 1), out intUserId);
                            if (intUserId != -1)
                            {
                                var user = Users.Where(u => u.Key == intUserId).SingleOrDefault();
                                if (user != null)

                                    strTellerCode = user.Name;
                            }
                        }
                        if (SelectedUser.Key != 0 && intUserId != SelectedUser.Key)
                            continue;

                        if (TicketSessions.Where(ts => ts.SessionId == t.session).Count() == 0)
                        {
                            Dispatcher.Invoke((Action)(() =>
                                TicketSessions.Add(new TicketSessionModel()
                                {
                                    Id = t.ticketid,
                                    SessionId = t.session,
                                    Terminal = t.terminalname,
                                    User = strTellerCode,
                                    TicketDateTime = (DateTime)t.ticketdatetime
                                })
                            ));
                        }
                        Dispatcher.Invoke((Action)(() =>

                            TicketList.Tickets.Add(new TicketModel()
                            {
                                Id = t.id,
                                CinemaNumber = t.cinemanumber,
                                MovieCode = t.moviecode,
                                Rating = t.rating,
                                SeatType = t.seattype,
                                StartTime = t.startdate,
                                PatronCode = t.patroncode,
                                PatronName = t.patronname,
                                PatronPrice = (decimal)t.price,
                                PatronDescription = t.patrondescription,
                                SeatName = t.seatname,
                                ORNumber = t.ornumber,
                                AmusementTax = (decimal)t.at,
                                CulturalTax = (decimal)t.ct,
                                VatTax = (decimal)t.vt,
                                TerminalName = t.terminalname,
                                TellerCode = t.tellercode,
                                SessionName = t.session,
                                CurrentTime = dtCurrentDateTime,
                                IsVoid = t.isvoid,
                                IsSelected = false
                            })
                        ));
                    }

                    if (TicketSessions.Count == 0 && so.isPromptNotFound)
                    {
                        e.Result = "No ticket(s) found.";
                        return;
                    }
                }
            }
            e.Result = so.successPrompt;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string error = string.Empty;
            if (e.Result != null)
                error = e.Result.ToString();

            if (error == string.Empty)
            {
                this.Search(ORNumberInput.Text.Trim(), true, "Successfully voided ticket(s).");
            }
            else
            {
                Cursor = null;
                this.EnableControls(true);

                MessageWindow _messageWindow = new MessageWindow();
                _messageWindow.MessageText.Text = error;
                _messageWindow.ShowDialog();
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dispatcher.Invoke((Action)(()=>
                progressBar2.Value = e.ProgressPercentage
            ));
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool _blnIsSession = (bool)e.Argument;
            var _t = TicketList.Tickets.Where(x => x.IsSelected == true).ToList();

            int max = _t.Count;
            int _idx = 0;
            string error = "";
            foreach (var t in _t)
            {
                _idx++;
                int percentage = (int) (_idx * 100/ max);
                worker.ReportProgress(percentage);

                if (_blnIsSession == true)
                {
                    try
                    {
                        using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                        {

                            var _mslhs = (from mslhs in context.movies_schedule_list_house_seat
                                          where mslhs.id == t.Id
                                          select mslhs).SingleOrDefault();
                            context.movies_schedule_list_house_seat.DeleteObject(_mslhs);
                            context.SaveChanges();
                        }
                    }
                    catch
                    {
                        ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                            string.Format("VOID SESSION {0} - FAIL.", Ticket.Id));
                        error = string.Format("Failed to void Session {0}.", Ticket.Id);
                        return;
                    }

                    ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                        string.Format("VOID SESSION {0} - OK.", Ticket.Id));
                }
                else
                {

                    string strORNumber = t.ORNumber;

                    this.PrintTicket(strORNumber);
                    if (Ticket.ORNumber == string.Empty)
                    {
                        error = string.Format("Ticket is invalid ({0}).", strORNumber);
                        return;
                    }

                    if (Ticket.IsVoid)
                    {
                        error = string.Format("Cannot void an {1} Number that is already void ({0}).", strORNumber, OfficialReceiptShortCaptionText);
                        return;
                    }

                    try
                    {
                        using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                        {

                            var _mslrs = (from mslrs in context.movies_schedule_list_reserved_seat
                                          where mslrs.status == 1 && mslrs.or_number == Ticket.ORNumber
                                          select mslrs).SingleOrDefault();
                            if (_mslrs != null)
                            {
                                _mslrs.status = 2;
                                _mslrs.void_user_id = ParadisoObjectManager.GetInstance().UserId;
                                _mslrs.void_datetime = ParadisoObjectManager.GetInstance().CurrentDate;
                            }
                            context.SaveChanges();


                        }
                    }
                    catch
                    {
                        ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                            string.Format("VOID {0} - FAIL.", Ticket.ORNumber));
                        error = string.Format("Failed to void {1} Number {0}.", strORNumber, OfficialReceiptShortCaptionText);
                        return;
                    }

                    ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                        string.Format("VOID {0} - OK.", Ticket.ORNumber));
                }

            }
            e.Result = error;
        }

        private void PopulateUsers() 
        {
            Users.Clear();
            Users.Add(new UserModel() { Key = 0, Name = "ALL" });
            using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var users = (from u in context.users
                             where u.system_code == 2 && u.status == 1
                             orderby u.userid
                             select new { u.id, u.userid }).ToList();
                if (users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        Users.Add(new UserModel() { Key = user.id, Name = user.userid });
                    }
                }
            }

            SelectedUser = Users[0];
        }

        private void Reset()
        {
            //load pending tickets to be voided (if any)
            CancelledORNumbers.Clear();
            try
            {
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var ornumbers = (from o in context.or_numbers_unpublished_movies_schedule_view
                                     select
                                         new { o.or_number }).ToList();
                    foreach (var ornumber in ornumbers)
                    {
                        CancelledORNumbers.Add(ornumber.or_number);
                    }
                }
            }
            catch { }
            if (CancelledORNumbers.Count > 0)
            {
                TicketSessions.Clear();
                TicketList.Tickets.Clear();

                //search ornumber or session number
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    foreach (string strSearch in CancelledORNumbers)
                    {

                        //ticket and details
                        var tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                       where mslrs.or_number == strSearch  && mslrs.status == 1
                                       select new
                                       {
                                           ticketid = mslrs.ticket_id,
                                           ticketdatetime = mslrs.ticket.ticket_datetime,
                                           terminalname = mslrs.ticket.terminal,
                                           tellercode = mslrs.ticket.user.userid,
                                           session = mslrs.ticket.session_id,

                                           id = mslrs.id,
                                           cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                           moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                           rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                           seattype = mslrs.movies_schedule_list.seat_type,
                                           startdate = mslrs.movies_schedule_list.start_time,
                                           patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                           patronname = mslrs.movies_schedule_list_patron.patron.name,
                                           patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                           seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                           groupname = mslrs.cinema_seat.group_name,
                                           sectionname = mslrs.cinema_seat.section_name,
                                           price = mslrs.price,
                                           baseprice = mslrs.base_price,
                                           ornumber = mslrs.or_number,
                                           at = mslrs.amusement_tax_amount,
                                           ct = mslrs.cultural_tax_amount,
                                           vt = mslrs.vat_amount,

                                           isvoid = (mslrs.void_datetime != null),
                                       }).ToList();

                        //show all tickets in initial search

                        DateTime dtCurrentDateTime = ParadisoObjectManager.GetInstance().CurrentDate;
                        foreach (var t in tickets)
                        {
                            if (TicketSessions.Where(ts => ts.Id == t.ticketid).Count() == 0)
                            {
                                TicketSessions.Add(new TicketSessionModel()
                                {
                                    Id = t.ticketid,
                                    SessionId = t.session,
                                    Terminal = t.terminalname,
                                    User = t.tellercode,
                                    TicketDateTime = (DateTime)t.ticketdatetime
                                });

                                //get buyer information if 
                                Buyer buyer = new Buyer();
                                var _bi = (from __bi in context.buyer_info_reserved
                                           where __bi.mslrs_id == t.id
                                           select __bi).FirstOrDefault();
                                if (_bi != null)
                                {
                                    buyer.LastName = _bi.buyer.lastname;
                                    buyer.FirstName = _bi.buyer.firstname;
                                    buyer.MiddleInitial = _bi.buyer.middleinitial;
                                    buyer.Address = _bi.buyer.address;
                                    buyer.Municipality = _bi.buyer.municipality;
                                    buyer.Province = _bi.buyer.province;
                                    buyer.TIN = _bi.buyer.tin;
                                    buyer.IDNum = _bi.buyer.idnum;
                                }

                                TicketList.Tickets.Add(new TicketModel()
                                {
                                    Id = t.id,
                                    CinemaNumber = t.cinemanumber,
                                    MovieCode = t.moviecode,
                                    Rating = t.rating,
                                    SeatType = t.seattype,
                                    StartTime = t.startdate,
                                    PatronCode = t.patroncode,
                                    PatronName = t.patronname,
                                    PatronPrice = (decimal)t.price,
                                    BasePrice = (decimal)t.baseprice,
                                    PatronDescription = t.patrondescription,
                                    SeatName = t.seatname,
                                    GroupName = t.groupname,
                                    SectionName = t.sectionname,
                                    ORNumber = t.ornumber,
                                    AmusementTax = (decimal)t.at,
                                    CulturalTax = (decimal)t.ct,
                                    VatTax = (decimal)t.vt,
                                    TerminalName = t.terminalname,
                                    TellerCode = t.tellercode,
                                    SessionName = t.session,
                                    CurrentTime = dtCurrentDateTime,
                                    IsVoid = t.isvoid,
                                    IsSelected = false,
                                    BuyerLastName = buyer.LastName,
                                    BuyerFirstName = buyer.FirstName,
                                    BuyerMiddleInitial = buyer.MiddleInitial,
                                    BuyerAddress = buyer.Address,
                                    BuyerMunicipality = buyer.Municipality,
                                    BuyerProvince = buyer.Province,
                                    BuyerTIN = buyer.TIN,
                                    BuyerIDNum = buyer.IDNum
                                });
                            }
                        }
                    }
                }

            }

        }

        private void Void_Click(object sender, RoutedEventArgs e)
        {
            if (!ParadisoObjectManager.GetInstance().HasRights("VOID"))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You don't have access for this page.";
                messageWindow.ShowDialog();

                this.Dispose();

                MainWindow win = (MainWindow)Window.GetWindow(this);
                win.SwitchContent("MovieCalendarPage.xaml");
                //NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
                return;
            }

            if (TicketList.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No ticket is selected.";
                messageWindow.ShowDialog();
                return;
            }

            MessageYesNoWindow window = new MessageYesNoWindow();
            window.MessageText.Text = string.Format("Are you sure you want to void ticket(s)?");
            window.ShowDialog();
            if (window.IsYes && worker.IsBusy != true)
            {
                progressBar2.Value = 0;
                Cursor = Cursors.Wait;
                this.EnableControls(false);
                worker.RunWorkerAsync(chkSessionOnly.IsChecked);
/*
                foreach (var t in TicketList.Tickets)
                {
                    if (!t.IsSelected)
                        continue;
                    if (chkSessionOnly.IsChecked == true)
                    {
                        try
                        {
                            using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                            {

                                var _mslhs = (from mslhs in context.movies_schedule_list_house_seat
                                              where mslhs.id == t.Id
                                              select mslhs).SingleOrDefault();
                                context.movies_schedule_list_house_seat.DeleteObject(_mslhs);
                                context.SaveChanges();
                            }
                        }
                        catch
                        {
                            ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                                string.Format("VOID SESSION {0} - FAIL.", Ticket.Id));
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = string.Format("Failed to void Session {0}.", Ticket.Id);
                            messageWindow.ShowDialog();
                            return;
                        }

                        ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                            string.Format("VOID SESSION {0} - OK.", Ticket.Id));
                    }
                    else
                    {

                        string strORNumber = t.ORNumber;

                        this.PrintTicket(strORNumber);
                        if (Ticket.ORNumber == string.Empty)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = string.Format("Ticket is invalid ({0}).", strORNumber);
                            messageWindow.ShowDialog();
                            return;
                        }

                        if (Ticket.IsVoid)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = string.Format("Cannot void an OR Number that is already void ({0}).", strORNumber);
                            messageWindow.ShowDialog();
                            return;
                        }

                        try
                        {
                            using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                            {

                                var _mslrs = (from mslrs in context.movies_schedule_list_reserved_seat
                                              where mslrs.status == 1 && mslrs.or_number == Ticket.ORNumber
                                              select mslrs).SingleOrDefault();
                                if (_mslrs != null)
                                {
                                    _mslrs.status = 2;
                                    _mslrs.void_user_id = ParadisoObjectManager.GetInstance().UserId;
                                    _mslrs.void_datetime = ParadisoObjectManager.GetInstance().CurrentDate;
                                }
                                context.SaveChanges();


                            }
                        }
                        catch
                        {
                            ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                                string.Format("VOID {0} - FAIL.", Ticket.ORNumber));
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = string.Format("Failed to void OR Number {0}.", strORNumber);
                            messageWindow.ShowDialog();
                            return;
                        }

                        ParadisoObjectManager.GetInstance().Log("VOID", "TICKET|VOID",
                            string.Format("VOID {0} - OK.", Ticket.ORNumber));
                    }

                }

                this.Search(ORNumberInput.Text.Trim(), false);

                MessageWindow _messageWindow = new MessageWindow();
                _messageWindow.MessageText.Text = string.Format("Successfully voided ticket(s).");
                _messageWindow.ShowDialog();
*/
            }
        }

        private void PrintTicket(string strORNumber)
        {
            //Ticket = new TicketModel();

            Ticket.Clear();
            using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var t = (from mslrs in context.movies_schedule_list_reserved_seat
                         where mslrs.or_number == strORNumber && mslrs.status == 1 
                         select new
                         {
                             id = mslrs.id,
                             cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                             moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                             rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                             seattype = mslrs.movies_schedule_list.seat_type,
                             startdate = mslrs.movies_schedule_list.start_time,
                             patroncode = mslrs.movies_schedule_list_patron.patron.code,
                             patronname = mslrs.movies_schedule_list_patron.patron.name,
                             price = mslrs.price,
                             baseprice = mslrs.base_price,
                             ordinanceprice = mslrs.ordinance_price,
                             surchargeprice = mslrs.surcharge_price,
                             ornumber = mslrs.or_number,
                             at = mslrs.amusement_tax_amount,
                             ct = mslrs.cultural_tax_amount,
                             vt = mslrs.vat_amount,
                             terminalname = mslrs.ticket.terminal,
                             tellercode = mslrs.ticket.user.userid,
                             session = mslrs.ticket.session_id,
                             isvoid = (mslrs.void_datetime != null),
                             seatname = mslrs.cinema_seat.col_name +  mslrs.cinema_seat.row_name,
                             groupname = mslrs.cinema_seat.group_name,
                             sectionname = mslrs.cinema_seat.section_name,
                             ishandicapped = (mslrs.cinema_seat.is_handicapped == 1) ? true : false,
                             cinemaSeatId = mslrs.cinema_seat_id
                         }).FirstOrDefault();
                if (t != null)
                {
                    Ticket.CinemaNumber = t.cinemanumber;
                    Ticket.MovieCode = t.moviecode;
                    Ticket.Rating = t.rating;
                    Ticket.StartTime = t.startdate;
                    Ticket.PatronCode = t.patroncode;
                    Ticket.PatronName = t.patronname;
                    Ticket.PatronPrice = (decimal) t.price;
                    Ticket.BasePrice = (decimal)t.baseprice;
                    Ticket.OrdinancePrice = (decimal)t.ordinanceprice;
                    Ticket.SurchargePrice = (decimal)t.surchargeprice;
                    Ticket.ORNumber = t.ornumber;
                    Ticket.AmusementTax = (decimal) t.at;
                    Ticket.CulturalTax = (decimal) t.ct;
                    Ticket.VatTax = (decimal) t.vt;
                    Ticket.TerminalName = t.terminalname;
                    Ticket.TellerCode = t.tellercode;
                    Ticket.SessionName = t.session;
                    Ticket.CurrentTime = ParadisoObjectManager.GetInstance().CurrentDate;
                    Ticket.IsVoid = t.isvoid;

                    var cinemaSeat = (from cs in context.cinema_seat
                        where cs.id == t.cinemaSeatId
                        select cs)
                        .FirstOrDefault();
                    Ticket.SeatName = string.Concat(string.IsNullOrEmpty(cinemaSeat.col_name) ? string.Empty : cinemaSeat.col_name,
                        string.IsNullOrEmpty(cinemaSeat.row_name) ? string.Empty : cinemaSeat.row_name);

                    Ticket.GroupName = t.groupname;
                    Ticket.SectionName = t.sectionname;
                    Ticket.IsHandicapped = t.ishandicapped;
                    Ticket.SeatType = t.seattype;

                    //retrieve buyer information
                    Buyer buyer = new Buyer();
                    var _bi = (from __bi in context.buyer_info_reserved
                               where __bi.mslrs_id == t.id
                               select __bi).FirstOrDefault();
                    if (_bi != null)
                    {
                        buyer.LastName = _bi.buyer.lastname;
                        buyer.FirstName = _bi.buyer.firstname;
                        buyer.MiddleInitial = _bi.buyer.middleinitial;
                        buyer.Address = _bi.buyer.address;
                        buyer.Municipality = _bi.buyer.municipality;
                        buyer.Province = _bi.buyer.province;
                        buyer.TIN = _bi.buyer.tin;
                        buyer.IDNum = _bi.buyer.idnum;
                    }

                    Ticket.BuyerLastName = buyer.LastName;
                    Ticket.BuyerFirstName = buyer.FirstName;
                    Ticket.BuyerMiddleInitial = buyer.MiddleInitial;
                    Ticket.BuyerAddress = buyer.Address;
                    Ticket.BuyerMunicipality = buyer.Municipality;
                    Ticket.BuyerProvince = buyer.Province;
                    Ticket.BuyerTIN = buyer.TIN;
                    Ticket.BuyerIDNum = buyer.IDNum;

                    //retrieve payment mode
                    var _s = (from s in context.sessions
                              where s.session_id == t.session
                              select new { paymentmode = s.payment_mode, cash = s.cash_amount, gc = s.gift_certificate_amount, cc = s.credit_amount }).FirstOrDefault();
                    if (_s != null)
                    {
                        Ticket.PaymentMode = _s.paymentmode;
                        if ((_s.paymentmode & 2) == 2) //has gc
                        {
                            if ((_s.paymentmode & 1) == 1) //cash and gc combination
                            {
                                var t2 = (from mslrs in context.movies_schedule_list_reserved_seat
                                          where mslrs.ticket.session_id == t.session
                                          select new { price = mslrs.price, ornumber = mslrs.or_number }).ToList();
                                if (t2.Count() > 0)
                                {
                                    decimal decTotalPrice = (decimal)t2.Sum(item => item.price);
                                    string strMaxORNumber = t2.Max(item => item.ornumber);
                                    int intCount = t2.Count();


                                    decimal decCash = (decimal)_s.cash;
                                    decimal decGC = decTotalPrice - decCash;
                                    decimal decCashDiv = (decimal)Math.Floor(((float)decCash / intCount) * 100) / 100;
                                    decimal decGCDiv = (decimal)Math.Floor(((float)decGC / intCount) * 100) / 100;

                                    if (strMaxORNumber == t.ornumber)
                                    {
                                        decimal _decCashDiv = decCashDiv + (decCash - (decCashDiv * intCount));
                                        decimal _decGCDiv = decGCDiv + (decGC - (decGCDiv * intCount));

                                        Ticket.Cash = _decCashDiv;
                                        Ticket.GiftCertificate= _decGCDiv;
                                        Ticket.CreditCard = 0;

                                    }
                                    else
                                    {
                                        Ticket.Cash = decCashDiv;
                                        Ticket.GiftCertificate = decGCDiv;
                                        Ticket.CreditCard = 0;
                                    }
                                }
                            }
                            else
                            {
                                Ticket.Cash = 0;
                                Ticket.CreditCard = 0;
                                Ticket.GiftCertificate = (decimal)t.price;
                            }
                        }
                        else if ((_s.paymentmode & 1) == 1) //cash
                        {
                            Ticket.Cash = (decimal)t.price;
                            Ticket.CreditCard = 0;
                            Ticket.GiftCertificate = 0;
                        }
                        else if ((_s.paymentmode & 4) == 4) //credit
                        {
                            Ticket.Cash = 0;
                            Ticket.CreditCard = (decimal)t.price;
                            Ticket.GiftCertificate = 0;
                        }

                    }
                }
            }
        }

        private void PrintTicket(TicketModel t)
        {
            if (t != null)
            {
                Ticket.Clear();
                Ticket.CinemaNumber = t.CinemaNumber;
                Ticket.MovieCode = t.MovieCode;
                Ticket.Rating = t.Rating;
                Ticket.StartTime = t.StartTime;
                Ticket.PatronCode = t.PatronCode;
                Ticket.PatronName = t.PatronName;
                Ticket.PatronPrice = t.PatronPrice;
                Ticket.BasePrice = t.BasePrice;
                Ticket.OrdinancePrice = t.OrdinancePrice;
                Ticket.SurchargePrice = t.SurchargePrice;
                Ticket.ORNumber = t.ORNumber;
                Ticket.AmusementTax = t.AmusementTax;
                Ticket.CulturalTax = t.CulturalTax;
                Ticket.VatTax = t.VatTax;
                Ticket.TerminalName = t.TerminalName;
                Ticket.TellerCode = t.TellerCode;
                Ticket.SessionName = t.SessionName;
                Ticket.CurrentTime = ParadisoObjectManager.GetInstance().CurrentDate;
                Ticket.IsVoid = t.IsVoid;
                Ticket.SeatName = t.SeatName;
                Ticket.GroupName = t.GroupName;
                Ticket.SectionName = t.SectionName;
                Ticket.IsHandicapped = t.IsHandicapped;
                Ticket.SeatType = t.SeatType;
                Ticket.BuyerLastName = t.BuyerLastName;
                Ticket.BuyerFirstName = t.BuyerFirstName;
                Ticket.BuyerMiddleInitial = t.BuyerMiddleInitial;
                Ticket.BuyerAddress = t.BuyerAddress;
                Ticket.BuyerMunicipality = t.BuyerMunicipality;
                Ticket.BuyerProvince = t.BuyerProvince;
                Ticket.BuyerTIN = t.BuyerTIN;
                Ticket.BuyerIDNum = t.BuyerIDNum;
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (!ParadisoObjectManager.GetInstance().HasRights("REPRINT"))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You don't have access for this page.";
                messageWindow.ShowDialog();

                this.Dispose();

                MainWindow win = (MainWindow)Window.GetWindow(this);
                win.SwitchContent("MovieCalendarPage.xaml");
                //NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
                return;
            }

            if (TicketList.Tickets.Count > 0 && TicketList.Tickets.Where (x=>x.IsSelected == true).Count() == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No ticket is selected.";
                messageWindow.ShowDialog();
                return;
            }

            bool? _print = false;
            string _printerName = string.Empty;
            PrintDialog _printDialog = new PrintDialog();
            
            if (ParadisoObjectManager.GetInstance().GetConfigValue("DIRECT PRINT", "YES") != "YES")
            {

                try
                {
                    _print = _printDialog.ShowDialog();
                }
                catch { }

                //raw print

                if (_print == true)
                {
                    _printerName = _printDialog.PrintQueue.FullName;
                }
            }
            else
            {
                _printerName = ParadisoObjectManager.GetInstance().DefaultPrinterName;
                _print = true;
            }


            if (_print == true)
            {
                foreach (var t in TicketList.Tickets)
                {
                    if (t.IsSelected)
                    {
                        this.PrintTicket(t.ORNumber); //redundant but required


                        if (Ticket.ORNumber == string.Empty)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = $"{OfficialReceiptShortCaptionText} Number is invalid.";
                            messageWindow.ShowDialog();
                            return;
                        }

                        if (Ticket.IsVoid)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = $"You cannot reprint an {OfficialReceiptShortCaptionText} Number that is void.";
                            messageWindow.ShowDialog();
                            return;
                        }

                        try
                        {
                            if (ParadisoObjectManager.GetInstance().IsRawPrinter)
                            {
                                this.PrintRawTicket(_printerName);
                            }
                            else
                            {
                                _printDialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                            }

                            ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|REPRINT", string.Format("REPRINT {0}.", Ticket.ORNumber));
                        }
                        catch (Exception ex) 
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            if (ex.InnerException != null)
                                messageWindow.MessageText.Text = ex.InnerException.Message.ToString();
                            else
                                messageWindow.MessageText.Text = ex.Message.ToString();
                            messageWindow.ShowDialog();
                            return;
                        }

                    }
                }

                this.Search(ORNumberInput.Text.Trim(), false, "Successfully printed ticket(s).");
            }
        }

        private void PrintTest_Click(object sender, RoutedEventArgs e)
        {
            CitizenPrinter print = new CitizenPrinter("DEMO");
            
            print.Open(ParadisoObjectManager.GetInstance().DefaultPrinterName);
            //191101001000050ABC
            /*
            print.DrawText(-1, 0, 0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true); //Z,E //A08 //Csh Total (-2)
            print.DrawText(1, 200, 0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true); //N  //A18 //D5 (3)
            print.DrawText(2, 600, 0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true); //K  //A24 
            */
            int spacing = 4;
            int r = spacing;
            /*
            print.Text("A04", r, 0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + 4);
            print.Text("A05", r, 0, "BACDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + 5);
            */
            //print.Text("A05", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            print.Text("A06", 760, 600, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            /*
            r += (spacing + (6 * 2));
            print.Text("A08", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (8*2));
            print.Text("A10", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (10*2));
            print.Text("A12", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (12*2));
            print.Text("A14", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (14*2));
            print.Text("A18", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (18 * 2));
            print.Text("A24", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (24 * 2));
            print.Text("A30", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            r += (spacing + (30 * 2));
            print.Text("A36", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ"); //280
            r += (spacing + (36 * 2));
            print.Text("A48", r, 10, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
            */

            print.Close();

            MessageWindow _messageWindow = new MessageWindow();
            _messageWindow.MessageText.Text = "Successfully printed test.";
            _messageWindow.ShowDialog();
        }

        protected int AddColumn(IPrint print, string s)
        {
            return this.AddColumn(print, s, 7, 56);
        }

        protected int AddColumn(IPrint print, string s, int min, int max)
        {
            int len = s?.Length ?? 0;
            int ac = 0;
            if (len < max)
            {
                int _ac = (max - len) / 2;
                ac = _ac * min;
            }


            return print.AddColumn(ac);
        }

        private void _PrintRawTicket(String _printerName, IPrint print, bool blnIsCinemaCopy)
        {
            int _title_len = 18;

            print.Open(_printerName);

            print.DrawText(4, print.Row, AddColumn(print, Ticket.Header1), Ticket.Header1, true); //C
            print.DrawText(-1, print.Row, AddColumn(print, Ticket.Header2), Ticket.Header2, true); //C
            print.DrawText(-1, print.Row, AddColumn(print, Ticket.Header3), Ticket.Header3, true); //C
            print.DrawText(-1, print.Row, AddColumn(print, Ticket.Header4), Ticket.Header4, true);
            print.DrawText(-1, print.Row, print.Column, " ", true);

            string vrt = string.Format("Vat Reg TIN#: {0}", Ticket.TIN);
            print.DrawText(-1, print.Row, this.AddColumn(print, vrt), vrt, true); //C

            string min = string.Format("MIN: {0}", Ticket.MIN);
            print.DrawText(-1, print.Row, AddColumn(print, min), min, true);
            string sss = string.Format("SS#: {0}", Ticket.ServerSerialNumber);
            print.DrawText(-1, print.Row, AddColumn(print, sss), sss, true);
            string posn = string.Format("POS#: {0}", Ticket.POSNumber);
            print.DrawText(-1, print.Row, AddColumn(print, posn), posn, true);

            print.DrawText(-1, print.Row, print.Column, " ", true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Trans. Date: {0:MMM d, yyyy}", Ticket.CurrentTime), false);
            print.DrawText(-1, print.Row, print.AddColumn(230), string.Format("{1}#: {0}", Ticket.ORNumber, OfficialReceiptShortCaptionText), true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Rating: {0} {1}", Ticket.Rating, Ticket.PatronCode.Length > 15 ? Ticket.PatronCode.Substring(0, 15) : Ticket.PatronCode), false);
            print.DrawText(-1, print.Row, print.AddColumn(230), string.Format("{0:MMM d, yyyy h:mmtt}", Ticket.StartTime), true);

            print.DrawText(-1, print.Row, print.Column, " ", true);

            string seatName = Ticket.SeatName;
            if (string.IsNullOrWhiteSpace(seatName))
                seatName = " ";

            print.DrawText(1, print.Row, print.Column, Ticket.MovieCode.Length > _title_len ? string.Format("{0}...", Ticket.MovieCode.Substring(0, _title_len)) : Ticket.MovieCode, false);
            print.DrawText(1, print.Row, print.AddColumn(325), seatName, true);

            int r0 = print.Row;

            List<string> h1 = new List<string>();
            List<string> h2 = new List<string>();

            print.DrawText(3, print.AddRow(10), print.Column, string.Format("{0}", Ticket.CinemaNumber), false);

            if ((Ticket.IsPWD || Ticket.IsSC) && Ticket.OrdinancePrice > 0m && Ticket.SurchargePrice > 0m) //make font smaller
            {
                h1.Add("Price");
                h2.Add(string.Format("{0:#,##0.00}", Ticket.FullPrice));

                print.DrawText(0, print.Row, print.AddColumn(30), h1[h1.Count-1], false);
                print.DrawText(0, print.Row, print.AddColumn(325), h2[h2.Count - 1], true);

                h1.Add(Ticket.IsPWD ? "PWD Discount:" : "SC Discount:");
                h2.Add(string.Format("{0:#,##0.00}", string.Format("({0:0.00})", Ticket.Discount)));
                print.DrawText(0, print.Row, print.AddColumn(30), h1[h1.Count-1], false);
                print.DrawText(0, print.Row, print.AddColumn(325), h2[h2.Count-1], true);

                h1.Add("Ord. Tax:");
                h2.Add(string.Format("{0:0.00}", Ticket.OrdinancePrice));

                print.DrawText(0, print.Row, print.AddColumn(30), "Ord. Tax:", false);
                print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.OrdinancePrice), true);

                h1.Add("Surcharge:");
                h2.Add(string.Format("{0:0.00}", Ticket.SurchargePrice));

                print.DrawText(0, print.Row, print.AddColumn(30), "Surcharge:", false);
                print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.SurchargePrice), true);
            }
            else
            {
                h1.Add("Price");
                print.DrawText(0, print.Row, print.AddColumn(30), "Price", false);
                if (Ticket.IsPWD || Ticket.IsSC)
                {
                    h2.Add(string.Format("{0:#,##0.00}", Ticket.FullPrice));
                    print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:#,##0.00}", Ticket.FullPrice), true);
                    h1.Add(Ticket.IsPWD ? "PWD Discount:" : "SC Discount:");
                    h2.Add(string.Format("({0:0.00})", Ticket.Discount));
                    print.DrawText(0, print.Row, print.AddColumn(30), Ticket.IsPWD ? "PWD Discount:" : "SC Discount:", false);
                    print.DrawText(0, print.Row, print.AddColumn(325), string.Format("({0:0.00})", Ticket.Discount), true);

                }
                else
                {
                    h2.Add(string.Format("{0:#,##0.00}", Ticket.BasePrice));
                    print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:#,##0.00}", Ticket.BasePrice), true);

                    h1.Add("Discount");
                    h2.Add("0.0");

                    print.DrawText(0, print.Row, print.AddColumn(30), "Discount:", false);
                    print.DrawText(0, print.Row, print.AddColumn(325), "0.00", true);

                }

                if (Ticket.OrdinancePrice > 0m)
                {
                    h1.Add("Ord. Tax");
                    print.DrawText(0, print.Row, print.AddColumn(30), "Ord. Tax:", false);
                    h2.Add(string.Format("{0:0.00}", Ticket.OrdinancePrice));
                    print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.OrdinancePrice), true);
                }
                else
                {
                    h1.Add(" ");
                    h2.Add(" ");
                    print.DrawText(0, print.Row, print.Column, " ", true);
                }
                if (Ticket.SurchargePrice > 0m)
                {
                    h1.Add("Surcharge:");
                    h2.Add(string.Format("{0:0.00}", Ticket.SurchargePrice));
                    print.DrawText(0, print.Row, print.AddColumn(30), "Surcharge:", false);
                    print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.SurchargePrice), true);
                }
                else
                {
                    h1.Add(" ");
                    h2.Add(" ");
                    print.DrawText(0, print.Row, print.Column, " ", true);
                }
			}

            var netTotalPrintRow = print.Row;

            if (Ticket.PaymentMode == 3)
			{
				h1.Add("CSH");
				h2.Add(string.Format("{0:0.00}", Ticket.Cash));
				h1.Add("GC");
				h2.Add(string.Format("{0:0.00}", Ticket.GiftCertificate));
				h1.Add("Total");
				h2.Add(string.Format("{0:0.00}", Ticket.PatronPrice));

				print.DrawText(-1, print.Row, print.AddColumn(30), "CSH", false);
				print.DrawText(-1, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.Cash), true);
				print.DrawText(-1, print.Row, print.AddColumn(30), "GC", false);
				print.DrawText(-1, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.GiftCertificate), true);

				print.DrawText(-2, print.Row, print.AddColumn(30), "Total", false);
				print.DrawText(-2, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.PatronPrice), true);
			}
			else
			{
				string strPaymentMode = "CSH";
				if ((Ticket.PaymentMode & 1) == 1)
					strPaymentMode = "CSH";
				else if ((Ticket.PaymentMode & 2) == 2)
					strPaymentMode = "GC";
				else if ((Ticket.PaymentMode & 4) == 4)
					strPaymentMode = "CC";

                h1.Add(string.Format("{0} Total", strPaymentMode));
                h2.Add(string.Format("{0:0.00}", Ticket.PatronPrice));
                print.DrawText(-2, print.Row, print.AddColumn(30), string.Format("{0} Total", strPaymentMode), false);
                print.DrawText(-2, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.PatronPrice), true);
                netTotalPrintRow = print.Row;

                //Vatable
                var vatableText = "Vatable";
                h1.Add($"{vatableText}:");
                h2.Add(string.Format("{0:0.00}", Ticket.Vatable));

                print.DrawText(0, print.Row, print.AddColumn(30), $"{vatableText}:", false);
                print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.Vatable), true);
                
                //Amusement Tax
                var amusementTaxText = "Am Tax";
                h1.Add($"{amusementTaxText}:");
                h2.Add(string.Format("{0:0.00}", Ticket.AmusementTax));

                print.DrawText(0, print.Row, print.AddColumn(30), $"{amusementTaxText}:", false);
                print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.AmusementTax), true);

                //Culturla Tax
                var culturlaTaxText = "C. Tax";
                h1.Add($"{culturlaTaxText}:");
                h2.Add(string.Format("{0:0.00}", Ticket.CulturalTax));

                print.DrawText(0, print.Row, print.AddColumn(30), $"{culturlaTaxText}:", false);
                print.DrawText(0, print.Row, print.AddColumn(325), string.Format("{0:0.00}", Ticket.CulturalTax), true);
            }


            int r1 = netTotalPrintRow;
            print.DrawRectangle(px: 200, py: (uint) r0, thickness: 2, pEx: 620, pEy: (uint) r1 + 3);

            print.DrawText(-1, print.Row, print.Column, " ", true);

            print.DrawText(-1, print.Row, print.Column, Ticket.SeatTypeName, false);
            print.DrawText(-1, print.Row, print.AddColumn(230), string.Format("{0:MMM d, yyyy h:mmtt}", Ticket.CurrentTime), true);
            print.DrawText(-1, print.Row, print.Column, "ADMIT ONE", false);
            print.DrawText(-1, print.Row, print.AddColumn(230), Ticket.SessionName, true);
            print.DrawText(-1, print.Row, print.Column, string.Format("By:  {0}", Ticket.TellerCode), true);

            print.DrawText(-1, print.Row, print.Column, " ", true);
            //print.DrawText(-1, print.Row, print.Column, "Buyer's Information", true);

            //display buyer info
            if (string.IsNullOrWhiteSpace(Ticket.BuyerName) || Ticket.BuyerName.Trim() == ".")
                print.DrawText(-1, print.Row, print.Column, "Name:", true);
            else
                print.DrawText(-1, print.Row, print.Column, string.Format("Name : {0}", Ticket.BuyerName), true);

            if (string.IsNullOrWhiteSpace(Ticket.BuyerFullAddress) || Ticket.BuyerFullAddress.Trim() == ".")
                print.DrawText(-1, print.Row, print.Column, "Address:", true);
            else
                print.DrawText(-1, print.Row, print.Column, string.Format("Address : {0}", Ticket.BuyerFullAddress), true);

            print.DrawText(-1, print.Row, print.Column, string.Format("TIN : {0}", Ticket.BuyerTIN), true);

            if (Ticket.IsSC)
                print.DrawText(-1, print.Row, print.Column, string.Format("OSCA ID #: {0}", Ticket.BuyerIDNum), true);
            else if (Ticket.IsPWD)
                print.DrawText(-1, print.Row, print.Column, string.Format("PWD ID #: {0}", Ticket.BuyerIDNum), true);
            else
                print.DrawText(-1, print.Row, print.Column, "ID #:", true);


            print.DrawText(-1, print.Row, print.Column, " ", true);
            if (ParadisoObjectManager.GetInstance().GetConfigValue("OFFICIAL RECEIPT", "No") == "Yes")
                print.DrawText(-1, print.Row, print.AddColumn(90), $"This Serves as your {SettingPage.OFFICIAL_RECEIPT_CAPTION}", true);
            else
                print.DrawText(0, print.Row, print.Column, " ", true);

            print.DrawText(-1, print.Row, print.AddColumn(150), "VAT EXEMPT", true);

            print.DrawText(-1, print.Row, print.AddColumn(30), "THIS DOCUMENT IS NOT VALID FOR CLAIM OF INPUT TAX", true);
            
            print.DrawText(-3, print.Row, print.Column, " ", true);
            //print.DrawText(-1, print.Row, print.Column, "Accredited Supplier:", true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Name: {0}", Ticket.SupplierName), true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Address: {0}", Ticket.SupplierAddress), true);
            print.DrawText(-1, print.Row, print.Column, string.Format("TIN : {0}", Ticket.SupplierTIN), true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Accreditation No.: {0}", Ticket.Accreditation), true);
            print.DrawText(-1, print.Row, print.Column, string.Format("Date Issued: {0:MMMM d, yyyy}", Ticket.DateIssued), true);
            print.DrawText(-1, print.Row, print.AddColumn(230), string.Empty, true);

            print.DrawText(-1, print.Row, print.Column, string.Format("PTU #: {0}", Ticket.PN), true);

            print.DrawText(-3, print.Row, print.Column, " ", true);

            print.DrawText(-1, print.Row, print.AddColumn(30), " ", true);
            print.DrawText(-1, print.Row, print.AddColumn(90), " ", true);


            print.DrawText(-3, print.Row, print.Column, " ", true);
            print.DrawText(-3, print.Row, print.Column, " ", true);
            //break
            print.Row = print.Row - 32;
            print.DrawText(-1, print.Row, print.Column, String.Format("{0} {1}", Ticket.Header1, Ticket.MovieCode.Length > _title_len ? string.Format("{0}...", Ticket.MovieCode.Substring(0, _title_len)) : Ticket.MovieCode), true);

            int row1 = print.Row;

            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("Vat Reg TIN# {0}", Ticket.TIN), true);
            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("MIN {0}", Ticket.MIN), true);
            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("SS# {0}", Ticket.ServerSerialNumber), true);
            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("POS# {0}", Ticket.POSNumber), true);
            print.DrawText(-3, print.Row, print.AddColumn(130), Ticket.SeatTypeName, true);
            print.DrawText(-3, print.Row, print.AddColumn(130), "ADMIT ONE", true);
            if (ParadisoObjectManager.GetInstance().GetConfigValue("OFFICIAL RECEIPT", "No") == "Yes")
                print.DrawText(-3, print.Row, print.AddColumn(130), $"{SettingPage.OFFICIAL_RECEIPT_CAPTION}", true);
            else
                print.DrawText(-3, print.Row, print.AddColumn(130), " ", true);
            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("{1}# {0}", Ticket.ORNumber, OfficialReceiptShortCaptionText), true);
            print.DrawText(-3, print.Row, print.AddColumn(130), string.Format("{0} By {1}", Ticket.SessionName, Ticket.TellerCode), true);

            print.Row = row1;
            print.DrawText(1, print.Row, print.Column, string.Format("{0}", Ticket.CinemaNumber), false);
            print.DrawText(-3, print.Row, print.AddColumn(25), string.Format("Date: {0:MMM d, yyyy}", Ticket.StartTime), true);
            print.DrawText(-3, print.Row, print.AddColumn(25), string.Format("Time: {0:hh:mm tt}", Ticket.StartTime), true);
            print.DrawText(-3, print.Row, print.AddColumn(25), string.Format("MTRCB {0}", Ticket.Rating), true);
            print.DrawText(-3, print.Row, print.AddColumn(25), string.Format("PESO {0:#,##0.00}", Ticket.PatronPrice), true);
            int row2 = print.Row;

            print.DrawRectangle(200, (uint)row1, 2, 320, (uint)row2 + 3);

            print.DrawText(-3, print.Row, print.AddColumn(25), " ", true);

            int row3 = print.Row;
            print.DrawText(-3, print.Row, print.Column, "SEAT NO:", true);
            //print.DrawText(-3, print.Row, print.Column, string.IsNullOrEmpty(Ticket.SeatName) ? string.Empty : Ticket.SeatName, true);
            print.DrawText(1, print.Row, print.AddColumn(25), seatName, true);

            int row4 = print.Row;
            print.DrawRectangle(200, (uint)row3, 2, 320, (uint)row4 + 10);


            print.Row = row1;
            int i = 0;
            for (i = 0; i < h1.Count; i++)
            {
                print.DrawText(-3, print.Row, print.AddColumn(280), h1[i], false);
                print.DrawText(-3, print.Row, print.AddColumn(355), h2[i], true);
            }


            print.Close();

        }

        private void PrintRawTicket(string _printerName)
        {
            IPrint print = (IPrint) new DummyPrinter(); //implement dummy printer
            IPrint print2 = (IPrint)new DummyPrinter(); //implement dummy printer


            if (_printerName.ToUpper().StartsWith("POSTEK"))
            {
                print = (IPrint)new PostekPrinterEx();
                //print2 = (IPrint)new PostekPrinterEx();
            }
            else if (_printerName.ToUpper().StartsWith("CITIZEN"))
            {
                print = (IPrint)new CitizenPrinter(Ticket.ORNumber);
                //print2 = (IPrint)new CitizenPrinter(Ticket.ORNumber);
            }
            else
            {
                throw new Exception("Printer not supported.");
            }
            
            this._PrintRawTicket(_printerName, print, true);
            //this._PrintRawTicket(_printerName, print2, false);
 
        }

        //put this into thread or create progress so it will not appear to hang
        public void PrintTickets(string[] tickets)
        {
            PrintDialog dialog = new PrintDialog();
            bool? _print = false;
            string _printerName = string.Empty;

            if (ParadisoObjectManager.GetInstance().GetConfigValue("DIRECT PRINT", "YES") != "YES")
            {

                try
                {
                    _print = dialog.ShowDialog();
                }
                catch { }

                //raw print

                if (_print == true)
                {
                    _printerName = dialog.PrintQueue.FullName;
                }
            }
            else
            {
                _printerName = ParadisoObjectManager.GetInstance().DefaultPrinterName;
                _print = true;
            }


            try
            {
                if (_print == true)
                {
                    //string _printerName = dialog.PrintQueue.FullName;


                    ThreadStart job = new ThreadStart(() =>
                    {
                        foreach (string ticket in tickets)
                        {

                            this.PrintTicket(ticket);
                            if (ParadisoObjectManager.GetInstance().IsRawPrinter)
                            {
                                try
                                {
                                    this.PrintRawTicket(_printerName);
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                                    {
                                        dialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                                    }));
                                    System.Threading.Thread.Sleep(100);
                                }
                                catch { }
                            }
                            ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|PRINT", string.Format("PRINT {0}.", Ticket.ORNumber));
                        }
                    });

                    Thread thread = new Thread(job);
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|PRINT", string.Format("ERROR: {0}.", ex.Message.ToString()));
                MessageBox.Show("An error has occured in printing.");
            }
        }

        private void CancelPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();

            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.SwitchContent("MovieCalendarPage.xaml");
            //NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }

        private struct SearchOptions
        {
            public string searchText;
            public bool isPromptNotFound;
            public string successPrompt;
            public int selectedUserKey;
            public bool isSessionOnly;
        }

        private void Search(string _strSearch, bool promptNotFound, string _successPrompt)
        {
            SearchOptions so = new SearchOptions() { 
                searchText = _strSearch, 
                isPromptNotFound = promptNotFound,
                successPrompt = _successPrompt,
                selectedUserKey = SelectedUser.Key,
                isSessionOnly = (bool) chkSessionOnly.IsChecked
            };

            if (!worker1.IsBusy)
            {
                Cursor = Cursors.Wait;
                progressBar2.Value = 0;
                this.EnableControls(false);
                worker1.RunWorkerAsync(so);
            }

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.Search(ORNumberInput.Text.Trim(), true, string.Empty);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double left = (MainCanvas.ActualWidth - TicketPanel.ActualWidth) / 2;
            if (left < 0)
                left = 0;

            Canvas.SetLeft(TicketPanel, left);

            double top = (MainCanvas.ActualHeight - TicketPanel.ActualHeight) / 4;
            if (top < 0)
                top = 0;
            Canvas.SetTop(TicketPanel, top);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //this.PrintTicket("C100000006");
            if (ORNumberInput.Focusable)
            {
                Keyboard.Focus(ORNumberInput);
            }

            /*
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            */

            if (ParadisoObjectManager.GetInstance().RunOnce)
            {
                if (Users.Count > 0)
                {
                    for (int i = 0; i < Users.Count; i++)
                    {
                        if (Users[i].Key == ParadisoObjectManager.GetInstance().UserId)
                        {
                            SelectedUser = Users[i];
                            chkSessionOnly.IsChecked = true;
                            break;
                        }
                    }
                }
                /*
                this.Search("000000001");
                TicketDataGrid.SelectedIndex = 0;
                */
                ParadisoObjectManager.GetInstance().RunOnce = false;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
            {
                ORNumberInput.Text = e.AddedItems[0].ToString();
                this.PrintTicket(e.AddedItems[0].ToString());   
            }
        }

        private void TicketSessionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TicketList.Tickets.Clear();
            string strSessionId = string.Empty;
            if (TicketSessionDataGrid.SelectedItem != null)
            {
                TicketSessionModel tm = (TicketSessionModel)TicketSessionDataGrid.SelectedItem;
                strSessionId = tm.SessionId;
            }

            if (strSessionId != string.Empty)
            {
                bool blnRunOnce = true;

                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    if (chkSessionOnly.IsChecked == true)
                    {
                        var tickets = (from mslhs in context.movies_schedule_list_house_seat
                                       where mslhs.session_id == strSessionId
                                       orderby mslhs.reserved_date descending
                                       select new
                                       {
                                           ticketid = -1,
                                           ticketdatetime = mslhs.reserved_date,
                                           terminalname = "",
                                           tellercode = "",
                                           session = mslhs.session_id,

                                           id = mslhs.id,
                                           cinemanumber = mslhs.movies_schedule_list.movies_schedule.cinema.in_order,
                                           moviecode = mslhs.movies_schedule_list.movies_schedule.movie.title,
                                           rating = mslhs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                           seattype = mslhs.movies_schedule_list.seat_type,
                                           startdate = mslhs.movies_schedule_list.start_time,
                                           enddate = mslhs.movies_schedule_list.end_time,
                                           patroncode = mslhs.movies_schedule_list_patron.patron.code,
                                           patronname = mslhs.movies_schedule_list_patron.patron.name,
                                           patrondescription = mslhs.movies_schedule_list_patron.patron.name,
                                           seatname = mslhs.cinema_seat.col_name + mslhs.cinema_seat.row_name,
                                           price = mslhs.movies_schedule_list_patron.price,
                                           ornumber = "RESERVED",
                                           at = 0.0,
                                           ct = 0.0,
                                           vt = 0.0,
                                           isvoid = false,
                                       }).ToList();
                        //remove expired

                        DateTime dtCurrentDateTime = ParadisoObjectManager.GetInstance().CurrentDate;
                        tickets = tickets.Where(t => t.enddate > dtCurrentDateTime && t.ticketdatetime > dtCurrentDateTime.AddMinutes(-10)).ToList();
                        //update teller
                        int intCount = tickets.Count;
                        for (int i = 0; i < intCount; i++)
                        {
                            int intUserId = -1;
                            string strTellerCode = string.Empty;

                            var t = tickets[i];


                            int intIndex = t.session.LastIndexOf('-');
                            if (intIndex != -1)
                            {
                                int.TryParse(t.session.Substring(intIndex + 1), out intUserId);
                                if (intUserId != -1)
                                {
                                    var user = Users.Where(u => u.Key == intUserId).SingleOrDefault();
                                    if (user != null)

                                        strTellerCode = user.Name;
                                }
                            }

                            TicketList.Tickets.Add(new TicketModel()
                            {
                                Id = t.id,
                                CinemaNumber = t.cinemanumber,
                                MovieCode = t.moviecode,
                                Rating = t.rating,
                                SeatType = t.seattype,
                                StartTime = t.startdate,
                                PatronCode = t.patroncode,
                                PatronName = t.patronname,
                                PatronPrice = (decimal)t.price,
                                PatronDescription = t.patrondescription,
                                SeatName = t.seatname,
                                ORNumber = t.ornumber,
                                AmusementTax = (decimal)t.at,
                                CulturalTax = (decimal)t.ct,
                                VatTax = (decimal)t.vt,
                                TerminalName = t.terminalname,
                                TellerCode = t.tellercode,
                                SessionName = t.session,
                                CurrentTime = dtCurrentDateTime,
                                IsVoid = t.isvoid,
                                IsSelected = false
                            });

                            if (blnRunOnce)
                            {
                                this.PrintTicket(TicketList.Tickets[0]);
                                blnRunOnce = false;
                            }

                        }

                    }
                    else
                    {
                        //ticket and details
                        var tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                       where mslrs.ticket.session_id == strSessionId && mslrs.status == 1
                                       select new
                                       {
                                           id = mslrs.id,
                                           cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                           moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                           rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                           seattype = mslrs.movies_schedule_list.seat_type,
                                           startdate = mslrs.movies_schedule_list.start_time,
                                           patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                           patronname = mslrs.movies_schedule_list_patron.patron.name,
                                           patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                           seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                           price = mslrs.price,
                                           ornumber = mslrs.or_number,
                                           at = mslrs.amusement_tax_amount,
                                           ct = mslrs.cultural_tax_amount,
                                           vt = mslrs.vat_amount,
                                           terminalname = mslrs.ticket.terminal,
                                           tellercode = mslrs.ticket.user.userid,

                                           session = mslrs.ticket.session_id,
                                           isvoid = (mslrs.void_datetime != null),

                                       }).ToList();

                        DateTime dtCurrentDateTime = ParadisoObjectManager.GetInstance().CurrentDate;
                        foreach (var t in tickets)
                        {

                            TicketList.Tickets.Add(new TicketModel()
                            {
                                Id = t.id,
                                CinemaNumber = t.cinemanumber,
                                MovieCode = t.moviecode,
                                Rating = t.rating,
                                SeatType = t.seattype,
                                StartTime = t.startdate,
                                PatronCode = t.patroncode,
                                PatronName = t.patronname,
                                PatronPrice = (decimal)t.price,
                                PatronDescription = t.patrondescription,
                                SeatName = t.seatname,
                                ORNumber = t.ornumber,
                                AmusementTax = (decimal)t.at,
                                CulturalTax = (decimal)t.ct,
                                VatTax = (decimal)t.vt,
                                TerminalName = t.terminalname,
                                TellerCode = t.tellercode,
                                SessionName = t.session,
                                CurrentTime = dtCurrentDateTime,
                                IsVoid = t.isvoid,
                                IsSelected = false
                            });

                            if (blnRunOnce)
                            {
                                this.PrintTicket(t.ornumber);
                                blnRunOnce = false;
                            }

                        }
                    }

                }
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.Ticket.Clear();
            ORNumberInput.Text = string.Empty;
            SelectedUser = Users[0];
            TicketSessions.Clear();
            TicketList.Tickets.Clear();

            this.Reset();

            if (ORNumberInput.Focusable)
                Keyboard.Focus(ORNumberInput);
        }

        private void SelectAll(bool blnIsChecked)
        {
            int count = TicketList.Tickets.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    TicketList.Tickets[i].IsSelected = blnIsChecked;
                }
            }
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            this.SelectAll(true);
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SelectAll(false);
        }

        private void TicketDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TicketDataGrid.SelectedItem != null)
            {
                TicketModel t = (TicketModel)TicketDataGrid.SelectedItem;
                if (chkSessionOnly.IsChecked == true)
                    this.PrintTicket(t);
                else
                    this.PrintTicket(t.ORNumber);
            }
        }

        private void chkSessionOnly_Checked(object sender, RoutedEventArgs e)
        {
            Print.Visibility = System.Windows.Visibility.Collapsed;
            this.Search(ORNumberInput.Text.Trim(), true, string.Empty);
        }

        private void chkSessionOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ParadisoObjectManager.GetInstance().HasRights("REPRINT"))
                Print.Visibility = System.Windows.Visibility.Visible;
            this.Search(ORNumberInput.Text.Trim(), true, string.Empty);
        }

    }
}
