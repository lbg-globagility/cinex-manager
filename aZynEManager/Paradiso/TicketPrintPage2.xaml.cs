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
    public partial class TicketPrintPage2 : Page
    {
        public TicketModel Ticket { get; set;}

        public ObservableCollection<UserModel> Users { get; set; }
        public UserModel SelectedUser { get; set; }

        public ObservableCollection<TicketSessionModel> TicketSessions { get; set; }
        public TicketListModel TicketList { get; set; }

        //public ObservableCollection<TicketModel> Tickets { get; set; }

        public ObservableCollection<string> CancelledORNumbers { get; set; }

        public TicketPrintPage2()
        {
            this.Initialize(false);
        }

        public TicketPrintPage2(bool blnIsReset)
        {
            this.Initialize(blnIsReset);
        }

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
                                           moviecode = mslrs.movies_schedule_list.movies_schedule.movie.title,
                                           rating = mslrs.movies_schedule_list.movies_schedule.movie.mtrcb.name,
                                           seattype = mslrs.movies_schedule_list.seat_type,
                                           startdate = mslrs.movies_schedule_list.start_time,
                                           patroncode = mslrs.movies_schedule_list_patron.patron.code,
                                           patronname = mslrs.movies_schedule_list_patron.patron.name,
                                           patrondescription = mslrs.movies_schedule_list_patron.patron.name,
                                           seatname = mslrs.cinema_seat.col_name + mslrs.cinema_seat.row_name,
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
                                    buyer.Name = _bi.name;
                                    buyer.Address = _bi.address;
                                    buyer.TIN = _bi.tin;
                                    buyer.IDNum = _bi.idnum;
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
                                    BuyerName = buyer.Name,
                                    BuyerAddress = buyer.Address,
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
                    if (chkSessionOnly.IsChecked == true)
                    {
                        try
                        {
                            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
                             ishandicapped = (mslrs.cinema_seat.is_handicapped == 1) ? true : false

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
                    Ticket.SeatName = t.seatname;
                    Ticket.IsHandicapped = t.ishandicapped;
                    Ticket.SeatType = t.seattype;

                    //retrieve buyer information
                    Buyer buyer = new Buyer();
                    var _bi = (from __bi in context.buyer_info_reserved
                               where __bi.mslrs_id == t.id
                               select __bi).FirstOrDefault();
                    if (_bi != null)
                    {
                        buyer.Name = _bi.name;
                        buyer.Address = _bi.address;
                        buyer.TIN = _bi.tin;
                        buyer.IDNum = _bi.idnum;
                    }

                    Ticket.BuyerName = buyer.Name;
                    Ticket.BuyerAddress = buyer.Address;
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
                Ticket.IsHandicapped = t.IsHandicapped;
                Ticket.SeatType = t.SeatType;
                Ticket.BuyerName = t.BuyerName;
                Ticket.BuyerAddress = t.BuyerAddress;
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
            if (_printerName.ToUpper().StartsWith("POSTEK"))
            {

                PostekPrinter print = new PostekPrinter();
                print.Open(_printerName);

                print.Column += 25;



                //PrintLab.PTK_DrawTextTrueTypeW(580, 0, 20, 0, "Arial Narrow", 3, 800, false, false, false, "A1", "ABCDEFGHIJ");
                print.DrawText(1, print.Row, print.Column, print.CenterString(33, Ticket.Header1), true);
                print.DrawText(-1, print.Row, print.Column, print.CenterString(126, Ticket.Header2), true);
                print.DrawText(-1, print.Row, print.Column, print.CenterString(126, Ticket.Header3), true);
                print.DrawText(-1, print.Row, print.Column, print.CenterString(126, string.Format("Vat Reg Tin#: {0}", Ticket.TIN)), true);
                //print.DrawText(-1, print.Row, print.Column, print.CenterString(126, string.Format("Accreditation #: {0}", Ticket.AccreditationNumber)), true);
                //print.DrawText(-1, print.Row, print.Column, string.Format("Permit #: {0}", Ticket.PN), false);
                //print.DrawText(-1, print.Row + 250, print.Column, string.Format("Server Serial#: {0}", Ticket.ServerSerialNumber), true);
                print.DrawText(-1, print.Row, print.Column, string.Format("MIN: {0}", Ticket.MIN), false);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("Server Serial#: {0}", Ticket.ServerSerialNumber), false);
                print.DrawText(-1, print.Row + 300, print.Column, string.Format("POS#: {0}", Ticket.POSNumber), true);
                print.DrawText(2, print.Row, print.Column, Ticket.MovieCode, true);
                print.DrawText(0, print.Row + 310, print.Column, string.Format("Or#: {0}", Ticket.ORNumber), true);

                int intx1a = print.Row;
                int inty1a = print.Column;
                
                int intx1b = print.Row + 142;
                int inty1b = print.Column;

                print.DrawText(3, print.Row, print.Column, string.Format("{0}", Ticket.CinemaNumber), false);
                print.DrawText(-1, print.Row + 145, print.Column, "SEAT NO:", false);
                if (Ticket.SeatName.Length == 2)
                    print.DrawText(3, print.Row + 275, print.Column, Ticket.SeatName, false);
                else if (Ticket.SeatName.Length == 3)
                    print.DrawText(3, print.Row + 225, print.Column, Ticket.SeatName, false);
                
                print.Column += 10;
                print.DrawText(-1, print.Row + 45, print.Column, string.Format("Date {0:MMM dd, yyyy}", Ticket.StartTime), true);
                print.DrawText(-1, print.Row + 45, print.Column, string.Format("Time {0:hh:mm tt}", Ticket.StartTime), true);
                print.DrawText(-1, print.Row + 45, print.Column, Ticket.PatronCode.Length > 15 ? Ticket.PatronCode.Substring(0, 15) : Ticket.PatronCode , true);
                print.DrawText(-1, print.Row + 45, print.Column, string.Format("PESO {0:#,##0.00}", Ticket.PatronPrice), true);

                int intx2a = print.Row + 135;
                int inty2a = print.Column + 10;
                int intx2b = print.Row + 440;
                int inty2b = print.Column + 10;

                PrintLab.PTK_DrawRectangle((uint)intx1a, (uint)inty1a, 2, (uint)intx2a, (uint)inty2a);
                PrintLab.PTK_DrawRectangle((uint)intx1b, (uint)inty1b, 2, (uint)intx2b, (uint)inty2b);

                //print.Column += 23; 
                print.Column += 8; //23 div 3

                //totals
                //right align?
                if ((Ticket.IsPWD || Ticket.IsSC) && Ticket.OrdinancePrice > 0m && Ticket.SurchargePrice > 0m) //make font smaller
                {
                    print.DrawText(-1, print.Row + 310, print.Column, "Amount:", false);
                    print.DrawText(-1, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.FullPrice), false);
                    print.DrawText(-1, print.Row + 310, print.Column + 12, Ticket.IsPWD ? "PWD Discount:" : "SC Discount:", false);
                    print.DrawText(-1, print.Row + 400, print.Column + 12, string.Format("({0:0.00})", Ticket.Discount), false);

                    print.DrawText(-1, print.Row + 310, print.Column + 24, "Ord. Tax:", false);
                    print.DrawText(-1, print.Row + 400, print.Column + 24, string.Format("{0:0.00}", Ticket.OrdinancePrice), false);
                    print.DrawText(-1, print.Row + 310, print.Column + 36, "Surcharge:", false);
                    print.DrawText(-1, print.Row + 400, print.Column + 36, string.Format("{0:0.00}", Ticket.SurchargePrice), false);
                }
                else
                {
                    print.DrawText(0, print.Row + 310, print.Column, "Amount:", false);
                    int cidx = 0;
                    if (Ticket.IsPWD || Ticket.IsSC)
                    {
                        print.DrawText(0, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.FullPrice), false);
                        cidx += 15;

                        print.DrawText(0, print.Row + 310, print.Column + cidx, Ticket.IsPWD ? "PWD Discount:" : "SC Discount:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("({0:0.00})", Ticket.Discount), false);

                        cidx += 15;
                    }
                    else
                    {
                        print.DrawText(0, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.BasePrice), false);
                        cidx += 15;
                    }

                    if (Ticket.OrdinancePrice > 0m)
                    {
                        print.DrawText(0, print.Row + 310, print.Column + cidx, "Ord. Tax:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("{0:0.00}", Ticket.OrdinancePrice), false);
                        cidx += 15;
                    }
                    if (Ticket.SurchargePrice > 0m)
                    {
                        print.DrawText(0, print.Row + 310, print.Column + cidx, "Surcharge:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("{0:0.00}", Ticket.SurchargePrice), false);
                    }
                }
                /*
                print.DrawText(-1, print.Row + 340, print.Column, "ct:", false);
                print.DrawText(-1, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.CulturalTax), false);
                print.DrawText(-1, print.Row + 340, print.Column + 15, "at:", false);
                print.DrawText(-1, print.Row + 400, print.Column + 15, string.Format("{0:0.00}", Ticket.AmusementTax), false);
                print.DrawText(-1, print.Row + 340, print.Column + 30, "vt:", false);
                print.DrawText(-1, print.Row + 400, print.Column + 30, string.Format("{0:0.00}", Ticket.VatTax), false);
                */
                if (Ticket.PaymentMode == 3)
                {
                    print.DrawText(0, print.Row + 310, print.Column + 50, "CSH", false);
                    print.DrawText(0, print.Row + 400, print.Column + 50, string.Format("{0:0.00}", Ticket.Cash), false);
                    print.DrawText(0, print.Row + 310, print.Column + 65, "GC", false);
                    print.DrawText(0, print.Row + 400, print.Column + 65, string.Format("{0:0.00}", Ticket.GiftCertificate), false);

                    print.DrawText(-2, print.Row + 310, print.Column + 85, "Total", false);
                    print.DrawText(-2, print.Row + 400, print.Column + 85, string.Format("{0:0.00}", Ticket.PatronPrice), false);
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

                    //display buyer info
                    print.DrawText(-1, print.Row + 310, print.Column + 48, Ticket.BuyerNameAddress, false);
                    print.DrawText(-1, print.Row + 310, print.Column + 60, string.Format("TIN No. {0} : ", Ticket.BuyerTIN), false);
                    if (Ticket.IsSC)
                        print.DrawText(-1, print.Row + 310, print.Column + 72, string.Format("OSCA ID No. : {0}", Ticket.BuyerIDNum), false);
                    else if (Ticket.IsPWD)
                        print.DrawText(-1, print.Row + 310, print.Column + 72, string.Format("PWD ID No. : {0}", Ticket.BuyerIDNum), false);

                    print.DrawText(-2, print.Row + 310, print.Column + 85, string.Format("{0} Total", strPaymentMode), false);
                    print.DrawText(-2, print.Row + 400, print.Column + 85, string.Format("{0:0.00}", Ticket.PatronPrice), false);
                }

                print.DrawText(-1, print.Row + 310, print.Column + 105, "VAT EXEMPT", false);

                print.DrawText(0, print.Row, print.Column, Ticket.SeatTypeName, true);
                print.DrawText(2, print.Row, print.Column, "ADMIT ONE", true);
                if (ParadisoObjectManager.GetInstance().GetTerminalConfigValue("OFFICIAL RECEIPT", "No") == "Yes")
                    print.DrawText(0, print.Row, print.Column, "OFFICIAL RECEIPT", true);
                else
                    print.DrawText(0, print.Row, print.Column, " ", true);
                print.DrawText(0, print.Row, print.Column, string.Format("MTRCB RATING: {0}", Ticket.Rating), true);

                //print.Column += 15;
                print.Column += 6;

                print.DrawText(-1, print.Row, print.Column, string.Format("Sess. # {0}", Ticket.SessionName), true);
                print.DrawText(-1, print.Row, print.Column, string.Format("By:    {0}", Ticket.TellerCode), true);

                print.DrawText(-1, print.Row, print.Column, Ticket.Supplier, true);
                print.DrawText(-1, print.Row, print.Column, string.Format("Accreditation #: {0}", Ticket.Accreditation), true);
                print.DrawText(-1, print.Row, print.Column, string.Format("PTU #: {0}", Ticket.PN), true);
                print.DrawText(-1, print.Row, print.Column, "THIS INVOICE/RECEIPT SHALL BE VALID FOR FIVE (5) YEARS FROM THE DATE OF THE PERMIT TO USE.", true);

                print.Column += 15;
                //cutter
                print.DrawText(0, print.Row, print.Column, string.Format("{0}  {1}", Ticket.Header1, Ticket.MovieCode), true);

                int x1a = print.Row;
                int y1a = print.Column + 2;
                int x1b= print.Row;
                int y1b = print.Column + 50;
                int x2a = print.Row + 120;
                int y2a = print.Column + 55;
                int x2b = print.Row + 120;
                int y2b = print.Column + 130;

                PrintLab.PTK_DrawRectangle((uint)x1a, (uint)y1a, 2, (uint)x2a, (uint)y2a);
                PrintLab.PTK_DrawRectangle((uint)x1b, (uint)y1b, 2, (uint)x2b, (uint)y2b);


                print.DrawText(2, print.Row, print.Column + 5, string.Format("{0}", Ticket.CinemaNumber), false);
                print.DrawText(-1, print.Row + 25, print.Column + 5, string.Format("Date {0:MMM dd, yyyy}", Ticket.StartTime), false);
                print.DrawText(-1, print.Row + 25, print.Column + 17, string.Format("Time {0:hh:mm tt}", Ticket.StartTime), false);
                print.DrawText(-1, print.Row + 25, print.Column + 29, Ticket.PatronCode.Length > 15 ? Ticket.PatronCode.Substring(0, 15) : Ticket.PatronCode, false);

                print.DrawText(-1, print.Row, print.Column + 58, "SEAT NO:", false);
                if (Ticket.SeatName.Length == 2)
                    print.DrawText(3, print.Row + 35, print.Column + 65, Ticket.SeatName, false);
                else if (Ticket.SeatName.Length == 3)
                    print.DrawText(3, print.Row + 15, print.Column + 65, Ticket.SeatName, false);

                //totals
                //right align?
                if ((Ticket.IsPWD || Ticket.IsSC) && Ticket.OrdinancePrice > 0m && Ticket.SurchargePrice > 0m) //make font smaller
                {
                    print.DrawText(-1, print.Row + 310, print.Column, "Amount:", false);
                    print.DrawText(-1, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.FullPrice), false);
                    
                    print.DrawText(-1, print.Row + 310, print.Column, Ticket.IsPWD ? "PWD Discount:" :"SC Discount:", false);
                    print.DrawText(-1, print.Row + 400, print.Column, string.Format("({0:0.00})", Ticket.Discount), false);

                    print.DrawText(-1, print.Row + 310, print.Column + 24, "Ord. Tax:", false);
                    print.DrawText(-1, print.Row + 400, print.Column + 24, string.Format("{0:0.00}", Ticket.OrdinancePrice), false);
                    
                    print.DrawText(-1, print.Row + 310, print.Column + 36, "Surcharge:", false);
                    print.DrawText(-1, print.Row + 400, print.Column + 36, string.Format("{0:0.00}", Ticket.SurchargePrice), false);
                }
                else
                {

                    print.DrawText(0, print.Row + 310, print.Column, "Amount:", false);
                    int cidx = 0;
                    if (Ticket.IsPWD || Ticket.IsSC)
                    {
                        print.DrawText(0, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.FullPrice), false);
                        cidx += 15;
                        
                        print.DrawText(0, print.Row + 310, print.Column + cidx, Ticket.IsPWD ? "PWD Discount:" : "SC Discount:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("({0:0.00})", Ticket.Discount), false);

                        cidx += 15;
                    }
                    else
                    {
                        print.DrawText(0, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.BasePrice), false);
                        cidx += 15;
                    }

                    if (Ticket.OrdinancePrice > 0m)
                    {
                        print.DrawText(0, print.Row + 310, print.Column + cidx, "Ord. Tax:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("{0:0.00}", Ticket.OrdinancePrice), false);
                        cidx += 15;
                    }

                    if (Ticket.SurchargePrice > 0m)
                    {
                        print.DrawText(0, print.Row + 310, print.Column + cidx, "Surcharge:", false);
                        print.DrawText(0, print.Row + 400, print.Column + cidx, string.Format("{0:0.00}", Ticket.SurchargePrice), false);
                    }
                }
                /*
                print.DrawText(-1, print.Row + 340, print.Column, "ct:", false);
                print.DrawText(-1, print.Row + 400, print.Column, string.Format("{0:0.00}", Ticket.CulturalTax), false);
                print.DrawText(-1, print.Row + 340, print.Column + 15, "at:", false);
                print.DrawText(-1, print.Row + 400, print.Column + 15, string.Format("{0:0.00}", Ticket.AmusementTax), false);
                print.DrawText(-1, print.Row + 340, print.Column + 30, "vt:", false);
                print.DrawText(-1, print.Row + 400, print.Column + 30, string.Format("{0:0.00}", Ticket.VatTax), false);
                print.DrawText(-1, print.Row + 340, print.Column + 45, "", false);
                print.DrawText(-1, print.Row + 340, print.Column + 60, "", false);
                */

                if (Ticket.PaymentMode == 3)
                {
                    print.DrawText(0, print.Row + 310, print.Column + 50, "CSH", false);
                    print.DrawText(0, print.Row + 400, print.Column + 50, string.Format("{0:0.00}", Ticket.Cash), false);
                    print.DrawText(0, print.Row + 310, print.Column + 65, "GC", false);
                    print.DrawText(0, print.Row + 400, print.Column + 65, string.Format("{0:0.00}", Ticket.GiftCertificate), false);

                    print.DrawText(-2, print.Row + 310, print.Column + 85, "Total", false);
                    print.DrawText(-2, print.Row + 400, print.Column + 85, string.Format("{0:0.00}", Ticket.PatronPrice), false);
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
                    
                    //display buyer info
                    print.DrawText(-1, print.Row + 310, print.Column + 48, Ticket.BuyerNameAddress, false);
                    print.DrawText(-1, print.Row + 310, print.Column + 60, string.Format("TIN No. {0} : ", Ticket.BuyerTIN), false);
                    if (Ticket.IsSC)
                        print.DrawText(-1, print.Row + 310, print.Column + 72, string.Format("OSCA ID No. : {0}", Ticket.BuyerIDNum), false);
                    else if (Ticket.IsPWD)
                        print.DrawText(-1, print.Row + 310, print.Column + 72, string.Format("PWD ID No. : {0}", Ticket.BuyerIDNum), false);

                    print.DrawText(-2, print.Row + 310, print.Column + 85, string.Format("{0} Total", strPaymentMode), false);
                    print.DrawText(-2, print.Row + 400, print.Column + 85, string.Format("{0:0.00}", Ticket.PatronPrice), false);
                }

                print.DrawText(-1, print.Row + 310, print.Column + 105, "VAT EXEMPT", false);
                print.DrawText(-1, print.Row + 310, print.Column + 114, string.Format("A#: {0}", Ticket.Accreditation), false);
                print.DrawText(-1, print.Row + 310, print.Column + 126, string.Format("P#: {0}", Ticket.PN), false);

                /*
                print.DrawText(-2, print.Row + 310, print.Column + 85, "Total", false);
                print.DrawText(-2, print.Row + 400, print.Column + 85, string.Format("{0:0.00}", Ticket.PatronPrice), false);
                */

                print.DrawText(-1, print.Row + 125, print.Column, string.Format("Vat Reg Tin#: {0}", Ticket.TIN), true);
                //print.DrawText(-1, print.Row + 125, print.Column, string.Format("A#: {0}", Ticket.AccreditationNumber), true);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("SS#: {0}", Ticket.ServerSerialNumber), true);
                //print.DrawText(-1, print.Row + 125, print.Column, string.Format("P#: {0}", Ticket.PN), true);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("MIN: {0}", Ticket.MIN), true);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("POS # {0}", Ticket.POSNumber), true);
                print.DrawText(-1, print.Row + 125, print.Column, Ticket.SeatTypeName, true);
                print.DrawText(-1, print.Row + 125, print.Column, "ADMIT ONE", true);
                if (ParadisoObjectManager.GetInstance().GetTerminalConfigValue("OFFICIAL RECEIPT", "No") == "Yes")
                    print.DrawText(-1, print.Row + 125, print.Column, "OFFICIAL RECEIPT", true);
                else
                    print.DrawText(-1, print.Row + 125, print.Column, " ", true);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("Sess. # {0}", Ticket.SessionName), true);
                print.DrawText(0, print.Row + 125, print.Column, string.Format("Or#: {0}", Ticket.ORNumber), true);
                print.DrawText(-1, print.Row + 125, print.Column, string.Format("By:    {0}", Ticket.TellerCode), true);

                print.DrawText(-1, print.Row, print.Column, Ticket.Supplier, true);
                print.DrawText(-1, print.Row, print.Column, "THIS INVOICE/RECEIPT SHALL BE VALID FOR FIVE (5) YEARS FROM THE DATE OF THE PERMIT TO USE.", true);

                print.Close();

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
            if ((strSearch == string.Empty && SelectedUser.Key == 0) || 
                (strSearch == string.Empty && SelectedUser.Key != 0 && chkSessionOnly.IsChecked == false))
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
                if (chkSessionOnly.IsChecked == false)
                {
                    //ticket and details
                    var tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where mslrs.or_number == strSearch && mslrs.status == 1 //
                                   //) && ((SelectedUser.Key != 0 && mslrs.ticket.user.id == SelectedUser.Key) || SelectedUser.Key == 0)
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
                    if (SelectedUser.Key == 0 && tickets.Count == 0)
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
                    else if (SelectedUser.Key != 0)
                    {
                        tickets = (from mslrs in context.movies_schedule_list_reserved_seat
                                   where
                                   mslrs.status == 1
                                   && mslrs.ticket.user.id == SelectedUser.Key
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
                        if (strSearch != string.Empty)
                        {
                            tickets = tickets.Where(x => (x.ornumber == strSearch || x.session.StartsWith(strSearch))).ToList();
                        }
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
                        }
                    }

                    if (tickets.Count == 0)
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "No ticket(s) found.";
                        messageWindow.ShowDialog();
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
                    tickets = tickets.Where(t => t.enddate > dtCurrentDateTime && t.ticketdatetime > dtCurrentDateTime.AddMinutes(-10) ).ToList();
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

                            TicketSessions.Add(new TicketSessionModel()
                            {
                                Id = t.ticketid,
                                SessionId = t.session,
                                Terminal = t.terminalname,
                                User = strTellerCode,
                                TicketDateTime = (DateTime)t.ticketdatetime
                            });
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
                    }

                    if (TicketSessions.Count == 0)
                    {
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "No ticket(s) found.";
                        messageWindow.ShowDialog();
                    }
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.Search(ORNumberInput.Text.Trim());
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

                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
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
            this.Search(ORNumberInput.Text.Trim());
        }

        private void chkSessionOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ParadisoObjectManager.GetInstance().HasRights("REPRINT"))
                Print.Visibility = System.Windows.Visibility.Visible;
            this.Search(ORNumberInput.Text.Trim());
        }
    }
}
