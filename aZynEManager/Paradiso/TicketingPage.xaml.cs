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
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TicketingPage.xaml
    /// </summary>
    public partial class TicketingPage : Page
    {
        public int MovieKey { get; set; }
        public int MovieTimeKey { get; set; }
        public List<int> SelectedCinemaSeats { get; set; }

        public ObservableCollection<CinemaPatron> cinemaPatrons;
        public ObservableCollection<CinemaTicket> cinemaTickets;

        public TicketingPage(int intMovieKey, int intMovieTimeKey, List<int> selectedCinemaSeats)
        {
            cinemaTickets = new ObservableCollection<CinemaTicket>();

            InitializeComponent();

            MovieKey = intMovieKey;
            MovieTimeKey = intMovieTimeKey;
            SelectedCinemaSeats = new List<int>();
            for (int i = 0; i < selectedCinemaSeats.Count; i++)
                SelectedCinemaSeats.Add(selectedCinemaSeats[i]);
            cinemaPatrons = new ObservableCollection<CinemaPatron>();

            //using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParidisoModel")))
            using (var context = new paradisoEntities())
            {
                var patrons = (from mctp in context.movie_calendar_time_patrons
                               where mctp.movie_calendar_time_key == this.MovieTimeKey
                               select new { mctp.key, mctp.patron_key, mctp.patron.name, mctp.price }).ToList();
                if (patrons.Count > 0)
                {
                    foreach (var patron in patrons)
                    {
                        cinemaPatrons.Add(new CinemaPatron() { Key = patron.key, PatronKey = patron.patron_key, PatronName = patron.name, Price = patron.price});
                    }
                }
            }

            this.DataContext = this;
        }

        public ObservableCollection<CinemaTicket> CinemaTickets
        {
            get { return cinemaTickets; }
            set { cinemaTickets = value; }
        }


        private void LoadValues()
        {
            //no databinding yet
            SeatSelected.Text = "0";
            SeatBooked.Text = "0";
            SeatAvailable.Text = "0";
            //TicketGrid.Items.Clear();

            CinemaTickets.Clear();
            TicketPanel.Children.Clear();

            //load values
            //using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParidisoModel")))
            using (var context = new paradisoEntities())
            {
                var movietimes = (from mct in context.movie_calendar_times
                                  where mct.key == this.MovieTimeKey
                                  select new
                                  {
                                      mct.key,
                                      movie_key = mct.movie_calendar.movie_database.key,
                                      movie_name = mct.movie_calendar.movie_database.name,
                                      cinema_key = mct.movie_calendar.cinema_key,
                                      cinema_name = mct.movie_calendar.cinema.name,
                                      mct.start_time,
                                      mct.end_time
                                  }).ToList();
                if (movietimes.Count > 0)
                {
                    MovieName.Text = movietimes[0].movie_name;
                    CinemaName.Text = movietimes[0].cinema_name;
                    ScreenDate.Text = string.Format("{0:MMMM dd, yyy}", movietimes[0].start_time);
                    ScreenTime.Text = string.Format("{0:HH:mm} - {1:HH:mm}", movietimes[0].start_time, movietimes[0].end_time);
                    var maxprice = (from mctp in context.movie_calendar_time_patrons
                                    where mctp.movie_calendar_time_key == this.MovieTimeKey
                                    select mctp.price).Max();
                    Price.Text = string.Format("Php {0:#,##0}", maxprice);

                    var CinemaKey = movietimes[0].cinema_key;

                    var capacity = (from c in context.cinemas
                                    where c.key == CinemaKey
                                    select c.capacity).First();
                    //get paid
                    var patrons = (from mctp in context.movie_calendar_time_patrons
                                   where mctp.movie_calendar_time_key == MovieTimeKey
                                   select mctp.key).Count();
                    //get booked
                    var bookings = (from mcths in context.movie_calendar_time_house_seats
                                    where mcths.movie_calendar_time_key == MovieTimeKey
                                    select mcths.cinema_seat_key).Count();

                    SeatSelected.Text = string.Format("{0}", this.SelectedCinemaSeats.Count);
                    SeatBooked.Text = string.Format("{0}", (int)bookings);
                    SeatAvailable.Text = string.Format("{0}", capacity - patrons - bookings - this.SelectedCinemaSeats.Count);

                }

            }

            if (this.SelectedCinemaSeats.Count > 0)
            {
                CinemaTickets.Add(new CinemaTicket(0, this.MovieTimeKey, 0, 0, 1, SelectedCinemaSeats.Count, 0.0M, cinemaPatrons));

                TicketPatronControl ticketPatronControl = new TicketPatronControl(cinemaTickets[cinemaTickets.Count - 1]);
                ticketPatronControl.TicketPatronClicked += new EventHandler(ticketPatronControl_TicketPatronClicked);
                TicketPanel.Children.Add(ticketPatronControl);
            }
        }

        private void ticketPatronControl_TicketPatronClicked(object sender, EventArgs e)
        {
            TicketPatronArgs tpa = (TicketPatronArgs)e;
            int intIndex = -1;
            int intControlCount = TicketPanel.Children.Count;

            //disable all event handlers to prevent error
            for (int x = 0; x < intControlCount; x++)
            {
                TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[x];
                ticketPatronControl.TicketPatronClicked -= new EventHandler(ticketPatronControl_TicketPatronClicked);
            }

            int intTicketCount = CinemaTickets.Count;
            if (tpa.Ticket.PatronKey == 0) //no selection
            {

            }
            else if (tpa.Ticket.Quantity == 0) //remove
            {
                if (intTicketCount > 1) //applicable to more than one entry only
                {
                    //remove collection
                    for (int i = 0; i < intTicketCount; i++)
                    {
                        if (CinemaTickets[i].PatronKey == tpa.Ticket.PatronKey)
                        {
                            CinemaTickets.RemoveAt(i);
                            break;
                        }
                    }

                    //remove control
                    for (int j = 0; j < intControlCount; j++)
                    {
                        TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[j];
                        if (ticketPatronControl.Ticket.PatronKey == tpa.Ticket.PatronKey)
                        {
                            TicketPanel.Children.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (intTicketCount > 0)
                {
                    for (int i = 0; i < intTicketCount; i++)
                    {
                        if ((CinemaTickets[i].PatronKey == tpa.Ticket.PatronKey && tpa.PrevPatronKey == -1) ||
                             (CinemaTickets[i].PatronKey == tpa.PrevPatronKey && tpa.PrevPatronKey != -1))
                        {
                            intIndex = i;
                            break;
                        }
                    }
                }

                if (intIndex == -1) //add to collection
                {
                }
                else
                {
                    CinemaTickets[intIndex] = new CinemaTicket(tpa.Ticket);
                }

            }

            intTicketCount = CinemaTickets.Count;
            intControlCount = TicketPanel.Children.Count;

            int intTotalQty = 0;
            int intAdditionalQty = 0;
            decimal decTotalAmount = 0.0M;
            if (intTicketCount > 0)
            {
                foreach (CinemaTicket cinemaTicket in CinemaTickets)
                {
                    if (cinemaTicket.Quantity == 0) //usually last element
                        intAdditionalQty++;

                    intTotalQty += cinemaTicket.Quantity;
                    decTotalAmount += cinemaTicket.Price;
                }
            }
            TotalQty.Text = string.Format("{0}", intTotalQty);
            TotalAmount.Text = string.Format("{0:#,##0.00}", decTotalAmount);


            int intRemainQty = SelectedCinemaSeats.Count - intTotalQty;
            List<int> patronKeys = new List<int>();

            //update listing
            if (intTicketCount > 0)
            {
                foreach (CinemaTicket cinemaTicket in CinemaTickets)
                {
                    if (intTicketCount == 1 || cinemaTicket.PatronKey == 0)
                        cinemaTicket.MinQuantity = 1;
                    else
                        cinemaTicket.MinQuantity = 0;

                    if (cinemaTicket.PatronKey == 0)
                        cinemaTicket.MaxQuantity = intRemainQty;
                    else
                        cinemaTicket.MaxQuantity = intRemainQty + cinemaTicket.Quantity;
                }
            }

            bool blnHasEmptyPatron = false;
            if (intControlCount > 0)
            {
                for (int j = 0; j < intControlCount; j++)
                {
                    TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[j];
                    if (ticketPatronControl.Ticket.PatronKey == 0)
                        blnHasEmptyPatron = true;
                }

                for (int j = 0; j < intControlCount; j++)
                {
                    TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[j];
                    if (ticketPatronControl.Ticket.PatronKey != 0)
                        patronKeys.Add(ticketPatronControl.Ticket.PatronKey);

                    if (intControlCount == 0 || ticketPatronControl.Ticket.PatronKey == 0 )
                        ticketPatronControl.Ticket.MinQuantity = 1;
                    else
                        ticketPatronControl.Ticket.MinQuantity = 0;

                    if (ticketPatronControl.Ticket.PatronKey == 0)
                        ticketPatronControl.Ticket.MaxQuantity = intRemainQty;
                    else
                        ticketPatronControl.Ticket.MaxQuantity = intRemainQty + ticketPatronControl.Ticket.Quantity;
                }
            }

            //update patrons for other controls
            if (intControlCount > 0)
            {
                for (int j = 0; j < intControlCount; j++)
                {
                    TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[j];
                    int intPatronKey = ticketPatronControl.Ticket.PatronKey;
                    ObservableCollection<CinemaPatron> remainPatrons = new ObservableCollection<CinemaPatron>();
                    if (cinemaPatrons.Count > 0)
                    {
                        foreach (CinemaPatron _cinemaPatron in cinemaPatrons)
                        {
                            if (patronKeys.IndexOf(_cinemaPatron.Key) == -1 ||
                                _cinemaPatron.Key == ticketPatronControl.Ticket.PatronKey)
                                remainPatrons.Add(new CinemaPatron(_cinemaPatron));
                        }
                        ticketPatronControl.Ticket.Patrons = remainPatrons;
                        ticketPatronControl.Ticket.PatronKey = intPatronKey;
                    }
                }
            }

            
            if (!blnHasEmptyPatron && intRemainQty > 0 && patronKeys.Count < cinemaPatrons.Count) //add an empty patron if tickets are still available
            {
                ObservableCollection<CinemaPatron> remainPatrons = new ObservableCollection<CinemaPatron>();
                if (cinemaPatrons.Count > 0)
                {
                    foreach (CinemaPatron _cinemaPatron in cinemaPatrons)
                    {
                        if (patronKeys.IndexOf(_cinemaPatron.Key) == -1)
                            remainPatrons.Add(new CinemaPatron(_cinemaPatron));
                    }
                }

                CinemaTickets.Add(new CinemaTicket(0, this.MovieTimeKey, 0, 0, 1, intRemainQty, 0.0M, remainPatrons));
                TicketPatronControl ticketPatronControl = new TicketPatronControl(cinemaTickets[cinemaTickets.Count - 1]);
                ticketPatronControl.TicketPatronClicked += new EventHandler(ticketPatronControl_TicketPatronClicked);
                TicketPanel.Children.Add(ticketPatronControl);
            }
            else if (blnHasEmptyPatron && intRemainQty == 0) //remove empty patron if remaining quantity is zero
            {
                CinemaTickets.RemoveAt(intTicketCount-1);
                TicketPanel.Children.RemoveAt(intControlCount - 1);
                intControlCount = TicketPanel.Children.Count;
            }

            //enable all event handlers to prevent error
            for (int x = 0; x < intControlCount; x++)
            {
                TicketPatronControl ticketPatronControl = (TicketPatronControl)TicketPanel.Children[x];
                ticketPatronControl.TicketPatronClicked += new EventHandler(ticketPatronControl_TicketPatronClicked);
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadValues();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
            messageYesNoWindow.MessageText.Text = "Are you sure you want to cancel?";
            messageYesNoWindow.ShowDialog();
            if (!messageYesNoWindow.IsYes)
                return;
            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new SeatingPage(this.MovieKey, this.MovieTimeKey, this.SelectedCinemaSeats));
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            //validate if all selected
            //get total count
            int intTicketCount = CinemaTickets.Count;
            int intControlCount = TicketPanel.Children.Count;
            int intTicketQty = 0;
            int intControlQty = 0;
            if (TicketPanel.Children.Count > 0)
            {
                foreach (TicketPatronControl ticketPatronControl in TicketPanel.Children)
                {
                    if (ticketPatronControl.Ticket.PatronKey != 0 &&
                        ticketPatronControl.Ticket.Quantity > 0)
                        intControlQty += ticketPatronControl.Ticket.Quantity;
                }
            }
            if (CinemaTickets.Count > 0)
            {
                foreach (CinemaTicket cinemaTicket in CinemaTickets)
                {
                    if (cinemaTicket.PatronKey != 0 &&
                        cinemaTicket.Quantity > 0)
                        intTicketQty += cinemaTicket.Quantity;
                }
            }

            int intRemainQty = SelectedCinemaSeats.Count - intTicketQty;
            if (intRemainQty != 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                if (intRemainQty == 1)
                    messageWindow.MessageText.Text = string.Format("You have {0} remaining seat.", intRemainQty);
                else
                    messageWindow.MessageText.Text = string.Format("You have {0} remaining seats.", intRemainQty);
                messageWindow.ShowDialog();
                return;
            }

            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
            messageYesNoWindow.MessageText.Text = "Click PRINT to continue";
            messageYesNoWindow.YesButton.Content = "CANCEL";
            messageYesNoWindow.NoButton.Content = "PRINT";
            messageYesNoWindow.ShowDialog();
            if (!messageYesNoWindow.IsYes)
            {
                //save


                NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
            }
        }
    }
}
