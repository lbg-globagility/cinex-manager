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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TicketPrintPage.xaml
    /// </summary>
    public partial class TicketPrintPage : Page
    {
        public TicketModel Ticket { get; set;}

        public TicketPrintPage()
        {
            InitializeComponent();

            Ticket = new TicketModel();

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
            if (ParadisoObjectManager.GetInstance().HasRights("VOID"))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "You don't have access for this page.";
                messageWindow.ShowDialog();

                NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
                return;
            }

            string strORNumber = ORNumberInput.Text.Trim();

            this.PrintTicket(strORNumber);
            if (Ticket.ORNumber == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "OR Number is invalid.";
                messageWindow.ShowDialog();
                return;
            }

            MessageYesNoWindow window = new MessageYesNoWindow();
            window.MessageText.Text = string.Format("Are you sure you want to void OR Number {0}?", strORNumber);
            window.ShowDialog();
            if (window.IsYes)
            {

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

                ORNumberInput.Text = string.Empty;
                Ticket.Clear();

                MessageWindow _messageWindow = new MessageWindow();
                _messageWindow.MessageText.Text = string.Format("Successfully voided OR Number {0}.", strORNumber);
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
                             session = mslrs.ticket.session_id
                         }).FirstOrDefault();
                if (t != null)
                {
                    Ticket.CinemaNumber = t.cinemanumber;
                    Ticket.MovieCode = t.moviecode;
                    Ticket.Rating = t.rating;
                    Ticket.SeatType = t.seattype;
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

            if (Ticket.ORNumber == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "OR Number is invalid.";
                messageWindow.ShowDialog();
                return;
            }
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            { 
                dialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|REPRINT", string.Format("REPRINT {0}.", Ticket.ORNumber));
            }
        }

        //put this into thread or create progress so it will not appear to hang
        public void PrintTickets(List<string> tickets)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {

                ThreadStart job = new ThreadStart(() =>
                {
                    foreach (string ticket in tickets)
                    {

                        this.PrintTicket(ticket);
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            dialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                        }));
                        //System.Threading.Thread.Sleep(100);
                        ParadisoObjectManager.GetInstance().Log("PRINT", "TICKET|PRINT", string.Format("PRINT {0}.", Ticket.ORNumber));
                    }
                });

                Thread thread = new Thread(job);
                thread.Start();
            }
        }

        private void CancelPrint_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage());
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string strORNumber = ORNumberInput.Text.Trim();
            this.PrintTicket(strORNumber);
            if (Ticket.ORNumber == string.Empty)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "OR Number is invalid.";
                messageWindow.ShowDialog();
            }
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
    }
}
