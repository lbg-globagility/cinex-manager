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
using Paradiso.Model;
using System.Threading;
using System.Windows.Threading;
using System.Printing;
using Paradiso.Helpers;
using System.Collections.ObjectModel;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TicketPrintPage.xaml
    /// </summary>
    public partial class TicketPrintPage : Page
    {
        public TicketModel Ticket { get; set;}

        public ObservableCollection<TicketSessionModel> TicketSessions { get; set; }
        public TicketListModel TicketList { get; set; }

        //public ObservableCollection<TicketModel> Tickets { get; set; }

        public ObservableCollection<string> CancelledORNumbers { get; set; }

        public TicketPrintPage()
        {
            InitializeComponent();

            Ticket = new TicketModel();

            TicketSessions = new ObservableCollection<TicketSessionModel>();
            TicketList = new TicketListModel();

            CancelledORNumbers = new ObservableCollection<string>();

            this.Reset();

            this.DataContext = this;

            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();
            bool blnHasVoid = paradisoObjectManager.HasRights("VOID");
            bool blnHasReprint = paradisoObjectManager.HasRights("REPRINT");

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

        }


        private void Reset()
        {
            //load pending tickets to be voided (if any)
            CancelledORNumbers.Clear();
            try
            {
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
                                           moviecode = mslrs.movies_schedule_list.movies_schedule.movie.code,
                                           rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                           seattype = mslrs.movies_schedule_list.seat_type,
                                           startdate = mslrs.movies_schedule_list.start_time,
                                           patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                           patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                           seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                           price = mslrs.price,
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

                                TicketList.Tickets.Add(new TicketModel()
                                {
                                    Id = t.id,
                                    CinemaNumber = t.cinemanumber,
                                    MovieCode = t.moviecode,
                                    Rating = t.rating,
                                    SeatType = t.seattype,
                                    StartTime = t.startdate,
                                    PatronCode = t.patroncode,
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
                            }
                        }
                    }
                }

            }

        }
        /*
        public void Print(string strORNumber)
        {
            Search.Visibility = Visibility.Hidden;
            Void.Visibility = Visibility.Hidden;
            ORNumberInput.IsEnabled = false;

            this.PrintTicket(strORNumber);
        }
        */

        private void Void_Click(object sender, RoutedEventArgs e)
        {
            if (!ParadisoObjectManager.GetInstance().HasRights("VOID"))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You don't have access for this page.";
                messageWindow.ShowDialog();

                NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
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
            if (window.IsYes)
            {
                foreach (var t in TicketList.Tickets)
                {
                    if (!t.IsSelected)
                        continue;

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
                        using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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

                this.Search(ORNumberInput.Text.Trim());

                MessageWindow _messageWindow = new MessageWindow();
                _messageWindow.MessageText.Text = string.Format("Successfully voided ticket(s).");
                _messageWindow.ShowDialog();
            }
        }

        private void PrintTicket(string strORNumber)
        {
            //Ticket = new TicketModel();

            Ticket.Clear();
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var t = (from mslrs in context.movies_schedule_list_reserved_seat
                         where mslrs.or_number == strORNumber && mslrs.status == 1 
                         select new
                         {
                             cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                             moviecode = mslrs.movies_schedule_list.movies_schedule.movie.code,
                             rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                             seattype = mslrs.movies_schedule_list.seat_type,
                             startdate = mslrs.movies_schedule_list.start_time,
                             patroncode = mslrs.movies_schedule_list_patron.patron.code,
                             price = mslrs.price,
                             ornumber = mslrs.or_number,
                             at = mslrs.amusement_tax_amount,
                             ct = mslrs.cultural_tax_amount,
                             vt = mslrs.vat_amount,
                             terminalname = mslrs.ticket.terminal,
                             tellercode = mslrs.ticket.user.userid,
                             session = mslrs.ticket.session_id,
                             isvoid = (mslrs.void_datetime != null),
                             seatname = mslrs.cinema_seat.col_name +  mslrs.cinema_seat.row_name,
                             ishandicapped = (mslrs.cinema_seat.is_handicapped == 1) ? true : false

                         }).FirstOrDefault();
                if (t != null)
                {
                    Ticket.CinemaNumber = t.cinemanumber;
                    Ticket.MovieCode = t.moviecode;
                    Ticket.Rating = t.rating;
                    Ticket.StartTime = t.startdate;
                    Ticket.PatronCode = t.patroncode;
                    Ticket.PatronPrice = (decimal) t.price;
                    Ticket.ORNumber = t.ornumber;
                    Ticket.AmusementTax = (decimal) t.at;
                    Ticket.CulturalTax = (decimal) t.ct;
                    Ticket.VatTax = (decimal) t.vt;
                    Ticket.TerminalName = t.terminalname;
                    Ticket.TellerCode = t.tellercode;
                    Ticket.SessionName = t.session;
                    Ticket.CurrentTime = ParadisoObjectManager.GetInstance().CurrentDate;
                    Ticket.IsVoid = t.isvoid;
                    Ticket.SeatName = t.seatname;
                    Ticket.IsHandicapped = t.ishandicapped;
                    Ticket.SeatType = t.seattype;
                }
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (!ParadisoObjectManager.GetInstance().HasRights("REPRINT"))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You don't have access for this page.";
                messageWindow.ShowDialog();

                NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
                return;
            }

            if (TicketList.Tickets.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No ticket is selected.";
                messageWindow.ShowDialog();
                return;
            }

            PrintDialog _printDialog = new PrintDialog();

            bool? _print = false;
            
            try
            {
                _print = _printDialog.ShowDialog();
            }
            catch { }

            //raw print

            if (_print == true)
            {
                string _printerName = _printDialog.PrintQueue.FullName;

                foreach (var t in TicketList.Tickets)
                {
                    if (t.IsSelected)
                    {
                        this.PrintTicket(t.ORNumber); //redundant but required


                        if (Ticket.ORNumber == string.Empty)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = "OR Number is invalid.";
                            messageWindow.ShowDialog();
                            return;
                        }

                        if (Ticket.IsVoid)
                        {
                            MessageWindow messageWindow = new MessageWindow();
                            messageWindow.MessageText.Text = "You cannot reprint an OR Number that is void.";
                            messageWindow.ShowDialog();
                            return;
                        }

                        if (ParadisoObjectManager.GetInstance().IsCitizenPrinter)
                        {
                            this.PrintRawTicket(_printerName);
                        }
                        else
                        {
                            _printDialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                        }

                        ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|REPRINT", string.Format("REPRINT {0}.", Ticket.ORNumber));

                    }
                }

                this.Search(ORNumberInput.Text.Trim());

                MessageWindow _messageWindow = new MessageWindow();
                _messageWindow.MessageText.Text = string.Format("Successfully printed ticket(s).");
                _messageWindow.ShowDialog();
            }
        }

        private void PrintRawTicket(string _printerName)
        {
            CitizenPrint print = new CitizenPrint();
            print.SetUnitsToInch();
            print.StartLabelFormatMode();
            print.SetPixelSize11();

            //print.BoxDefinition(050, 050, 100, 100); //not working at all

            print.SetFontData(1, -2, 220, Ticket.Header1);
            print.SetFontData(0, -2, 212, Ticket.Header2);
            print.SetFontData(0, -2, 205, string.Format("MIN:{0}", Ticket.MIN));
            print.SetFontData(0, -2, 198, string.Format("TIN:{0}", Ticket.TIN));
            print.SetFontData(0, -2, 191, string.Format("PN:{0}", Ticket.PN));
            print.SetFontData(3, -2, 173, Ticket.MovieCode);
            print.SetFontData(0, -1, 166, string.Format("MTRCB RATING:{0}", Ticket.Rating));

            print.SetFontData(0, 150, 158, string.Format("Date  {0:MM/dd/yy ddd}", Ticket.StartTime));
            print.SetFontData(0, 150, 150, string.Format("Time  {0:hh:mm tt}", Ticket.StartTime));
            print.SetFontData(0, 150, 142, string.Format("Peso  {0} {1:#}", Ticket.PatronCode, Ticket.PatronPrice));
            print.SetFontData(2, 180, 148, Ticket.CinemaNumber.ToString());

            print.SetFontData(0, -1, 125, Ticket.SeatTypeName);
            print.SetFontData(2, -1, 110, "ADMIT ONE");

            print.SetFontData(0, -1, 91, "THIS SERVES AS AN OFFICIAL RECEIPT");
            print.SetFontData(0, -1, 96, Ticket.Code);

            print.SetFontData(0, -1, 89, string.Format("OR#{0}", Ticket.ORNumber));
            print.SetFontData(0, 60, 89, string.Format("ct:{0:00.00}", Ticket.CulturalTax));

            print.SetFontData(0, -1, 82, string.Format("T:{0} {1} {2:HH:mm}", Ticket.TerminalName, Ticket.TellerCode, Ticket.CurrentTime));
            print.SetFontData(0, 60, 82, string.Format("at:{0:00.00}", Ticket.AmusementTax));

            print.SetFontData(0, 60, 75, string.Format("vt:{0:00.00}", Ticket.VatTax));

            print.SetFontData(0, -1, 68, string.Format("{0}    SN:{1}", Ticket.SessionName, Ticket.SerialNumber));

            //cutter here

            print.SetFontData(0, -1, 53, Ticket.Header1);
            print.SetFontData(0, 80, 53, Ticket.Code);

            print.SetFontData(0, 80, 44, string.Format("OR#{0}", Ticket.ORNumber));
            print.SetFontData(1, -1, 35, Ticket.MovieCode);
            print.SetFontData(0, 70, 33, string.Format(" SN:{0}", Ticket.SerialNumber));

            print.SetFontData(0, 170, 24, string.Format("{0:MM/dd/yy ddd hh:mm tt}", Ticket.StartTime));
            print.SetFontData(0, 70, 24, string.Format("ct:{0:00.00}", Ticket.CulturalTax));

            print.SetFontData(2, -1, 17, Ticket.CinemaNumber.ToString());
            print.SetFontData(0, 170, 17, string.Format("Peso {0} {1:#}", Ticket.PatronCode, Ticket.PatronPrice));
            print.SetFontData(0, 70, 17, string.Format("at:{0:00.00}", Ticket.AmusementTax));

            print.SetFontData(0, -1, 10, string.Format("T:{0} {1} {2:HH:mm}", Ticket.TerminalName, Ticket.TellerCode, Ticket.CurrentTime));
            print.SetFontData(0, 70, 10, string.Format("vt:{0:00.00}", Ticket.VatTax));

            print.SetFontData(0, -1, 0, Ticket.SessionName);
            print.SetFontData(0, 70, 0, string.Format("MIN:{0}", Ticket.MIN));

            print.EndLabelFormatModeAndPrint();
            RawPrinterHelper.SendStringToPrinter(Ticket.ORNumber, _printerName, print.Print);
        }

        //put this into thread or create progress so it will not appear to hang
        public void PrintTickets(List<string> tickets)
        {
            PrintDialog dialog = new PrintDialog();
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    string _printerName = dialog.PrintQueue.FullName;


                    ThreadStart job = new ThreadStart(() =>
                    {
                        foreach (string ticket in tickets)
                        {

                            this.PrintTicket(ticket);
                            if (ParadisoObjectManager.GetInstance().IsCitizenPrinter)
                            {
                                this.PrintRawTicket(_printerName);
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
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }


        private void Search(string strSearch)
        {
            if (strSearch == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "Missing search text.";
                messageWindow.ShowDialog();

                if (ORNumberInput.Focusable)
                    ORNumberInput.Focus();
                return;
            }

            TicketSessions.Clear();
            TicketList.Tickets.Clear();

            //search ornumber or session number
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                //ticket and details
                var tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                               where (mslrs.or_number == strSearch || mslrs.ticket.session_id.Contains(strSearch)) && mslrs.status == 1
                               select new
                               {
                                   ticketid = mslrs.ticket_id,
                                   ticketdatetime = mslrs.ticket.ticket_datetime,
                                   terminalname = mslrs.ticket.terminal,
                                   tellercode = mslrs.ticket.user.userid,
                                   session = mslrs.ticket.session_id,

                                   id = mslrs.id,
                                   cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                   moviecode = mslrs.movies_schedule_list.movies_schedule.movie.code,
                                   rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                   seattype = mslrs.movies_schedule_list.seat_type,
                                   startdate = mslrs.movies_schedule_list.start_time,
                                   patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                   patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                   seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
                                   price = mslrs.price,
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

                        TicketList.Tickets.Add(new TicketModel()
                        {
                            Id = t.id,
                            CinemaNumber = t.cinemanumber,
                            MovieCode = t.moviecode,
                            Rating = t.rating,
                            SeatType = t.seattype,
                            StartTime = t.startdate,
                            PatronCode = t.patroncode,
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
                    }
                }

                if (tickets.Count == 0)
                {

                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.Search(ORNumberInput.Text.Trim());
            /*
            string strORNumber = ORNumberInput.Text.Trim();
            this.PrintTicket(strORNumber);
            if (Ticket.ORNumber == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "OR Number is invalid.";
                messageWindow.ShowDialog();
            }
            */
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

                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    //ticket and details
                    var tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.ticket.session_id == strSessionId && mslrs.status == 1
                                   select new
                                   {
                                       id = mslrs.id,
                                       cinemanumber = mslrs.movies_schedule_list.movies_schedule.cinema.in_order,
                                       moviecode = mslrs.movies_schedule_list.movies_schedule.movie.code,
                                       rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                       seattype = mslrs.movies_schedule_list.seat_type,
                                       startdate = mslrs.movies_schedule_list.start_time,
                                       patroncode = mslrs.movies_schedule_list_patron.patron.code,
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.Ticket.Clear();
            ORNumberInput.Text = string.Empty;
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
    }
}
