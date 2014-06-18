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

        public List<CinemaPatron> cinemaPatrons;
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
            cinemaPatrons = new List<CinemaPatron>();

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

            //load values
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
                CinemaTickets.Add(new CinemaTicket(0, this.MovieTimeKey, 0, 0, 0.0M, cinemaPatrons));
                //TicketGrid.Items.Add(
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
            //if (Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to cancel?", string.Empty, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new SeatingPage(this.MovieKey, this.MovieTimeKey, this.SelectedCinemaSeats));
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show("Insert saving and printing option here");
            MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
            messageYesNoWindow.MessageText.Text = "Click PRINT to continue";
            messageYesNoWindow.YesButton.Content = "CANCEL";
            messageYesNoWindow.NoButton.Content = "PRINT";
            messageYesNoWindow.ShowDialog();
            if (!messageYesNoWindow.IsYes)
            {
                NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
            }
        }
    }
}
