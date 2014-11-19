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

        public ObservableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }

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

            Users = new ObservableCollection<UserModel>();
            this.PopulateUsers();

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

        private void PopulateUsers() 
        {
            Users.Clear();
            Users.Add(new UserModel() { Key = 0, Name = "ALL" });
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
                             moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
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
                        catch { }

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
            if (_printerName.ToUpper().StartsWith("CITIZEN"))
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

                //print.SetFontData(0, -1, 91, "THIS SERVES AS AN OFFICIAL RECEIPT");
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
            else if (_printerName.ToUpper().StartsWith("POSTEK"))
            {

                //PrintLab.OpenPort(_printerName);
                /*
                PrintLab.PTK_ClearBuffer();
                PrintLab.PTK_SetPrintSpeed(4);
                PrintLab.PTK_SetDarkness(10);
                PrintLab.PTK_SetLabelHeight(400, 16);


                PrintLab.PTK_PrintLabel(1, 1);
                PrintLab.ClosePort();
                */

                PostekPrinter1 print = new PostekPrinter1();
                print.Open(_printerName);


                //starting column is 200 instead of 50
                //15
                //18
                //20
                //22
                //30
                
                //start 
                //200 - ?
                //10 - ?

                //final
                //20 +18 34
                //25 +20 
                //30 +22
                //35 +28
                //40 +36
                //

                //PrintLab.PTK_DrawTextTrueTypeW(215, 120, 20, 0, "Arial", 2, 400, false, false, false, "A1", "ABCDEFGHIJ");
                //PrintLab.PTK_DrawTextTrueTypeW(235, 90, 30, 0, "Arial", 2, 400, false, false, false, "A2", "ABCDEFGHIJ");
                //PrintLab.PTK_DrawTextTrueTypeW(257, 120, 35, 0, "Arial", 2, 400, false, false, false, "A4", "ABCDEFGHIJ");
                //PrintLab.PTK_DrawTextTrueTypeW(282, 120, 40, 0, "Arial", 2, 400, false, false, false, "A5", "ABCDEFGHIJ");


                //350
                /*
                --correct layout ignore others
                PrintLab.PTK_DrawTextTrueTypeW(200, 10, 25, 0, "Arial", 2, 400, false, false, false, "A0", "1ABCDEFGHIJ");
                PrintLab.PTK_DrawTextTrueTypeW(220, 10, 20, 0, "Arial", 2, 400, false, false, false, "A1", "2ABCDEFGHIJ");
                PrintLab.PTK_DrawTextTrueTypeW(238, 10, 30, 0, "Arial", 2, 400, false, false, false, "A3", "3ABCDEFGHIJ");
                PrintLab.PTK_DrawTextTrueTypeW(260, 10, 35, 0, "Arial", 2, 400, false, false, false, "A4", "4ABCDEFGHIJ"); //+28
                PrintLab.PTK_DrawTextTrueTypeW(288, 10, 20, 0, "Arial", 2, 400, false, false, false, "A5", "5ABCDEFGHIJ");
                */

                //test print
                /*
                PrintLab.PTK_DrawTextTrueTypeW(200, 10, 25, 0, "Arial", 2, 400, false, false, false, "C0", "Y");
                PrintLab.PTK_DrawTextTrueTypeW(200, 110, 25, 0, "Arial", 2, 400, false, false, false, "C1", "Y");
                PrintLab.PTK_DrawTextTrueTypeW(200, 210, 25, 0, "Arial", 2, 400, false, false, false, "C2", "Y");
                PrintLab.PTK_DrawTextTrueTypeW(200, 310, 25, 0, "Arial", 2, 400, false, false, false, "C3", "Y");
                PrintLab.PTK_DrawTextTrueTypeW(200, 360, 25, 0, "Arial", 2, 400, false, false, false, "C4", "Y");

                PrintLab.PTK_DrawTextTrueTypeW(220, 10, 20, 0, "Arial", 2, 400, false, false, false, "Z0", "Z");
                PrintLab.PTK_DrawTextTrueTypeW(220, 110, 20, 0, "Arial", 2, 400, false, false, false, "Z1", "Z");
                PrintLab.PTK_DrawTextTrueTypeW(220, 210, 20, 0, "Arial", 2, 400, false, false, false, "Z2", "Z");
                PrintLab.PTK_DrawTextTrueTypeW(220, 310, 20, 0, "Arial", 2, 400, false, false, false, "Z3", "Z");
                PrintLab.PTK_DrawTextTrueTypeW(220, 360, 20, 0, "Arial", 2, 400, false, false, false, "Z4", "Z");

                PrintLab.PTK_DrawTextTrueTypeW(238, 10, 40, 0, "Arial", 2, 400, false, false, false, "X0", "X");
                PrintLab.PTK_DrawTextTrueTypeW(238, 110, 40, 0, "Arial", 2, 400, false, false, false, "X1", "X");
                PrintLab.PTK_DrawTextTrueTypeW(238, 210, 40, 0, "Arial", 2, 400, false, false, false, "X2", "X");
                PrintLab.PTK_DrawTextTrueTypeW(238, 310, 40, 0, "Arial", 2, 400, false, false, false, "X3", "X");
                PrintLab.PTK_DrawTextTrueTypeW(238, 360, 40, 0, "Arial", 2, 400, false, false, false, "X4", "X");

                PrintLab.PTK_DrawTextTrueTypeW(274, 10, 35, 0, "Arial", 2, 400, false, false, false, "K0", "K");
                PrintLab.PTK_DrawTextTrueTypeW(274, 110, 35, 0, "Arial", 2, 400, false, false, false, "K1", "K");
                PrintLab.PTK_DrawTextTrueTypeW(274, 210, 35, 0, "Arial", 2, 400, false, false, false, "K2", "K");
                PrintLab.PTK_DrawTextTrueTypeW(274, 310, 35, 0, "Arial", 2, 400, false, false, false, "K3", "K");
                PrintLab.PTK_DrawTextTrueTypeW(274, 360, 35, 0, "Arial", 2, 400, false, false, false, "K4", "K");

                PrintLab.PTK_DrawTextTrueTypeW(296, 10, 20, 0, "Arial", 2, 400, false, false, false, "L0", "L");
                PrintLab.PTK_DrawTextTrueTypeW(296, 110, 20, 0, "Arial", 2, 400, false, false, false, "L1", "L");
                PrintLab.PTK_DrawTextTrueTypeW(296, 210, 20, 0, "Arial", 2, 400, false, false, false, "L2", "L");
                PrintLab.PTK_DrawTextTrueTypeW(296, 310, 20, 0, "Arial", 2, 400, false, false, false, "L3", "L");
                PrintLab.PTK_DrawTextTrueTypeW(296, 360, 20, 0, "Arial", 2, 400, false, false, false, "L4", "L");
                */

                print.DrawText(1, 2, -1, Ticket.Header1, true);
                print.DrawText(0, 2, -1, Ticket.Header2, true);
                //min not included
                print.DrawText(0, 2, -1, string.Format("TIN:{0}", Ticket.TIN), true);
                print.DrawText(0, 2, -1, string.Format("PN:{0}", Ticket.PN), true);
                print.DrawText(3, 2, -1, Ticket.MovieCode, true);
                print.DrawText(0, 0, -1, string.Format("MTRCB RATING:{0}", Ticket.Rating), true);

                print.DrawText(3, 1, -1, Ticket.CinemaNumber.ToString(), false);
                print.DrawText(0, 0, -1, string.Format("Date  {0:MM/dd/yy ddd}", Ticket.StartTime), true);
                print.DrawText(0, 0, -1, string.Format(" Time  {0:hh:mm tt}", Ticket.StartTime), true);
                print.DrawText(0, 0, -1, string.Format("Peso  {0} {1:#}", Ticket.PatronCode, Ticket.PatronPrice), true);

                print.DrawText(0, 0, -1, " ", true);

                print.DrawText(0, 0, -1, Ticket.SeatTypeName, true);
                print.DrawText(2, 0, -1, "ADMIT ONE", true);

                print.DrawText(0, 0, -1, " ", true);

                print.DrawText(0, 0, -1, string.Format("OR#{0}", Ticket.ORNumber), false);
                print.DrawText(0, 1, -1, string.Format("ct:{0:00.00}", Ticket.CulturalTax), true);

                print.DrawText(0, 0, -1, string.Format("T:{0} {1} {2:HH:mm}", Ticket.TerminalName, Ticket.TellerCode, Ticket.CurrentTime), false);
                print.DrawText(0, 1, -1, string.Format("at:{0:00.00}", Ticket.AmusementTax), true);

                print.DrawText(0, 1, -1, string.Format("vt:{0:00.00}", Ticket.VatTax), true);

                //print.DrawText(0, 0, -1, string.Format("{0}    SN:{1}", Ticket.SessionName, Ticket.SerialNumber), true);
                print.DrawText(0, 0, -1, string.Format("{0}", Ticket.SessionName), true);

                print.Row += 10;
                //cutter here

                print.DrawText(0, 0, -1, Ticket.Header1, true);
                print.DrawText(0, 1, -1, string.Format("OR#{0}", Ticket.ORNumber), true);
                print.DrawText(1, 0, -1, Ticket.MovieCode, false);
                //print.DrawText(0, 1, -1, string.Format(" SN:{0}", Ticket.SerialNumber), false);

                print.DrawText(1, 1, -1, Ticket.CinemaNumber.ToString(), true);
                print.DrawText(0, 0, -1, string.Format("{0:MM/dd/yy ddd hh:mm tt}", Ticket.StartTime), false);
                print.DrawText(0, 1, -1, string.Format("ct:{0:00.00}", Ticket.CulturalTax), true);

                print.DrawText(0, 0, -1, string.Format("Peso {0} {1:#}", Ticket.PatronCode, Ticket.PatronPrice), false);
                print.DrawText(0, 1, -1, string.Format("at:{0:00.00}", Ticket.AmusementTax), true);

                print.DrawText(0, 0, -1, string.Format("T:{0} {1} {2:HH:mm}", Ticket.TerminalName, Ticket.TellerCode, Ticket.CurrentTime), false);
                print.DrawText(0, 1, -1, string.Format("vt:{0:00.00}", Ticket.VatTax), true);

                print.DrawText(0, 1, -1, Ticket.SessionName, true);
                //print.SetFontData(0, 70, 0, string.Format("MIN:{0}", Ticket.MIN));


                //PrintLab.PTK_DrawTextTrueTypeW(80, 120, 20, 0, "Arial", 1, 400, false, false, false, "C1", "TrueTypeFont");                
                /*
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

                //print.SetFontData(0, -1, 91, "THIS SERVES AS AN OFFICIAL RECEIPT");
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
                */

                print.Close();

            }
            else 
            {
                try
                {
                    PrintLab.OpenPort(_printerName);
                    
                    PrintLab.PTK_ClearBuffer();
                    PrintLab.PTK_SetPrintSpeed(4);
                    PrintLab.PTK_SetDarkness(10);

                    //PrintLab.PTK_SetLabelWidth(600); //determine width

                    /*
                    PrintLab.PTK_DrawText(50, 400, 3, 1, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    PrintLab.PTK_DrawText(65, 400, 3, 2, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    PrintLab.PTK_DrawText(85, 400, 3, 3, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    PrintLab.PTK_DrawText(110, 400, 3, 4, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    PrintLab.PTK_DrawText(140, 400, 3, 5, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    //PrintLab.PTK_DrawText(50, 25, 3, 2, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    //PrintLab.PTK_DrawText(50, 45, 3, 3, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    //PrintLab.PTK_DrawText(50, 70, 3, 4, 1, 1, 'N', "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ");


                    /*
                    //PrintLab.PTK_DrawText(50, 10, 0, 4, 1, 1, 'N', Ticket.Header1);

                    PostekPrinter postek = new PostekPrinter();

                    postek.DrawText(4, 2, Ticket.Header1);
                    postek.DrawText(1, 2, Ticket.Header2);
                    postek.DrawText(1, 2, Ticket.Header3);
                    /*
                    postek.DrawText(2, 2, string.Format("Vat Reg Tin#: {0}", Ticket.TIN));
                    postek.DrawText(2, 2, string.Format("Accreditation #: {0}", Ticket.AccreditationNumber));

                    postek.DrawText(75, 2, string.Format("Permit #: {0}", Ticket.PermitNumber), false);
                    postek.DrawText(400, 2, string.Format("Permit #: {0}", Ticket.ServerSerialNumber));
                    postek.DrawText(75, 2, string.Format("MIN: {0}", Ticket.MIN), false);
                    postek.DrawText(400, 2, string.Format("POS #: {0}", Ticket.POSNumber));

                    postek.DrawText(75, 5, Ticket.MovieCode, false);
                    postek.DrawText(500, 2, string.Format("Or#: {0}", Ticket.ORNumber));
                    */


                    PrintLab.PTK_PrintLabel(1, 1);
                    PrintLab.ClosePort();
                }
                catch (Exception ex)
                {
                }
            }
        }

        //put this into thread or create progress so it will not appear to hang
        public void PrintTickets(List<string> tickets)
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
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }


        private void Search(string strSearch)
        {
            if (strSearch == string.Empty && SelectedUser.Key == 0)
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
                               where mslrs.or_number == strSearch && mslrs.status == 1 //

                                     //) && ((SelectedUser.Key != 0 && mslrs.ticket.user.id == SelectedUser.Key) || SelectedUser.Key == 0)

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
                if (strSearch == string.Empty && SelectedUser.Key != 0)
                {
                        tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                     where 
                                     mslrs.status == 1
                                     && mslrs.ticket.user.id == SelectedUser.Key
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

                }

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
                    MessageWindow messageWindow = new MessageWindow();
                    /*
                    if (ParadisoObjectManager.GetInstance().UserName.StartsWith("ADMIN"))
                    {
                        messageWindow.MessageText.Text = string.Format("{0} {1}",
                            strSearch, SelectedUser.Key);
                    }
                    else
                    */
                    {
                        messageWindow.MessageText.Text = "No ticket(s) found.";
                    }
                    messageWindow.ShowDialog();
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
                this.PrintTicket(t.ORNumber);
            }
        }
    }
}
