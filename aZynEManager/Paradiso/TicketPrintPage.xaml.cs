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

        }

        private void PrintTicket(string strORNumber)
        {
            //Ticket = new TicketModel();

            Ticket.Clear();
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var t = (from mslrs in context.movies_schedule_list_reserved_seat
                         where mslrs.or_number == strORNumber
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
                if (t == null)
                {
                }
                else
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
            }
        }

        public void PrintTickets(List<string> tickets)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                foreach (string ticket in tickets)
                {
                    this.PrintTicket(ticket);
                    dialog.PrintVisual(TicketCanvas, Ticket.ORNumber);
                    //System.Threading.Thread.Sleep(100);
                }
            }
        }

        private void CancelPrint_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new MovieCalendarPage1());
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
