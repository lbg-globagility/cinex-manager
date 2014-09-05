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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using CinemaCustomControlLibrary;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SeatingPage1.xaml
    /// </summary>
    public partial class ReservedSeatingPage : Page
    {
        public int Key { get; set; }
        public MovieScheduleListModel MovieSchedule { get; set; }
        
        public ObservableCollection<PatronModel> Patrons { get; set; }
        public ObservableCollection<SeatModel> Seats { get; set; }
        public PatronSeatListModel SelectedPatronSeatList { get; set; }

        DispatcherTimer timer;
        private bool blnIsUpdating = false;

        public ReservedSeatingPage(int intKey) 
        {
            InitializeComponent();

            Key = intKey;
            MovieSchedule = new MovieScheduleListModel();
            Seats = new ObservableCollection<SeatModel>();

            SelectedPatronSeatList = new PatronSeatListModel();
            this.UpdateMovieSchedule();
            
            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 * Constants.ReservedSeatingUiInterval);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void ClearSelection()
        {
            string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var selectedseats = (from mslhsv in context.movies_schedule_list_house_seat_view
                                     where mslhsv.movies_schedule_list_id == this.Key && mslhsv.session_id == strSessionId
                                     select mslhsv.id).ToList();
                if (selectedseats != null)
                {
                    foreach (var sid in selectedseats)
                    {
                        var selectedseat = (from mslhs in context.movies_schedule_list_house_seat
                                            where mslhs.id == sid
                                            select mslhs).FirstOrDefault();
                        if (selectedseat != null)
                        {
                            context.movies_schedule_list_house_seat.DeleteObject(selectedseat);
                        }
                    }

                    context.SaveChanges();
                }

            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelection();
            ParadisoObjectManager.GetInstance().SetNewSessionId();
            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }


        void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateMovieSchedule();
        }

        private void UpdateMovieSchedule()
        {
            blnIsUpdating = true;
            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                //checks if movie schedule exists
                var _movie_schedule_list = (from msl in context.movies_schedule_list
                                                where msl.id == this.Key
                                                select new
                                                {
                                                    mslkey = msl.id,
                                                    cinemakey = msl.movies_schedule.cinema_id,
                                                    moviekey = msl.movies_schedule.movie_id,
                                                    moviename = msl.movies_schedule.movie.title,
                                                    duration = msl.movies_schedule.movie.duration,
                                                    rating = msl.movies_schedule.movie.mtrcb.name,
                                                    starttime = msl.start_time,
                                                    endtime = msl.end_time,
                                                    seattype = msl.seat_type,
                                                    laytime = msl.laytime,
                                                    capacity = msl.movies_schedule.cinema.capacity
                                                }).FirstOrDefault();
                if (_movie_schedule_list == null)
                {
                    blnIsUpdating = false;
                    //prompt error message
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Movie Schedule is invalid.";
                    messageWindow.ShowDialog();

                    NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

                //checks if movie already expired
                DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
                if (_movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime) < dtNow)
                {
                    blnIsUpdating = false;
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Movie Schedule is already expired.";
                    messageWindow.ShowDialog();

                    NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

                string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                {
                    MovieSchedule.Key = _movie_schedule_list.mslkey;
                    MovieSchedule.CinemaKey = _movie_schedule_list.cinemakey;
                    MovieSchedule.MovieKey = _movie_schedule_list.moviekey;
                    MovieSchedule.MovieName = _movie_schedule_list.moviename;
                    MovieSchedule.RunningTimeInSeconds = _movie_schedule_list.duration;
                    MovieSchedule.Rating = _movie_schedule_list.rating;
                    MovieSchedule.StartTime = _movie_schedule_list.starttime;
                    MovieSchedule.EndTime = _movie_schedule_list.endtime;
                    MovieSchedule.SeatType = _movie_schedule_list.seattype;
                    MovieSchedule.LayTime = _movie_schedule_list.laytime;
                }

                //taken seats
                var takenseats = (from mslrs in context.movies_schedule_list_reserved_seat
                                  where mslrs.movies_schedule_list_id == this.Key
                                  select mslrs.cinema_seat_id).ToList();


                //reserved seats from other sessions
                var reservedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id != strSessionId
                                     select mcths.cinema_seat_id).ToList();

                //selected seats 
                var selectedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id == strSessionId
                                     select new { mcths.id, mcths.cinema_seat_id, mcths.movies_schedule_list_patron_id, mcths.reserved_date
                                     }).ToList();

                /*
                //load 
                var patrons = (from mslrs in context.movies_schedule_list_reserved_seat
                                where mslrs.movies_schedule_list_id == this.Key
                                select mslrs.cinema_seat_id).Count();

                //reserved
                var reserved = (from mcths in context.movies_schedule_list_house_seat_view
                                where mcths.movies_schedule_list_id == this.Key
                                select mcths.cinema_seat_id).Count();
                */
                MovieSchedule.Selected = selectedseats.Count;
                MovieSchedule.Booked = reservedseats.Count;
                MovieSchedule.Available = (int)(_movie_schedule_list.capacity - takenseats.Count - reservedseats.Count - selectedseats.Count);
                    

                var price = (from mslp in context.movies_schedule_list_patron
                                where mslp.movies_schedule_list_id == this.Key && mslp.is_default == 1
                                select mslp.price).FirstOrDefault();
                if (price != null)
                    MovieSchedule.Price = (decimal)price;


                if (MovieSchedule.Available == 0 && MovieSchedule.SeatType != 3) //except unlimited seating
                {
                    MovieSchedule.IsEnabled = false;
                }
                else
                {

                    if (dtNow < _movie_schedule_list.starttime)
                    {
                        MovieSchedule.IsEnabled = true;
                    }
                    else if (dtNow < _movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime))
                    {
                        MovieSchedule.IsEnabled = true;
                    }
                    else
                    {
                        MovieSchedule.IsEnabled = false;
                    }
                }

                //MovieSchedule = _movie_schedule_list_item;

                //load all patrons
                Patrons = new ObservableCollection<PatronModel>();
                Patrons.Add(null);
                var _patrons = (from mslp in context.movies_schedule_list_patron
                                where mslp.movies_schedule_list_id == this.Key
                                select new
                                {
                                    key = mslp.id,
                                    patronkey = mslp.patron_id,
                                    patroncode = mslp.patron.code,
                                    patronname = mslp.patron.name,
                                    price = mslp.price
                                }).ToList();
                if (_patrons != null)
                {
                    foreach (var _patron in _patrons)
                    {
                        Patrons.Add(new PatronModel()
                        {
                            Key = _patron.key,
                            PatronKey = _patron.patronkey,
                            Code = _patron.patroncode,
                            Name = _patron.patronname,
                            Price = (decimal) _patron.price
                        });
                    }
                }

                //TODO add scrollbar and set maximum height and width, get maximum width to center 
                Seats.Clear();
                SelectedPatronSeatList.PatronSeats.Clear();

                var seats = (from s in context.cinema_seat
                                where s.cinema_id == MovieSchedule.CinemaKey
                                select s).ToList();
                foreach (var seat in seats)
                {
                    SeatModel seatModel = new SeatModel()
                    {
                        Key = seat.id,
                        Name = string.Format("{0}{1}", seat.row_name, seat.col_name),
                        X = (int)seat.x1,
                        Y = (int)seat.y1,
                        Width = (int)seat.x2 - (int) seat.x1,
                        Height = (int)seat.y2 - (int)seat.y1,
                        Type = (int) seat.object_type,
                        SeatType = 1
                    };

                    //get seat type
                    if (takenseats.Count > 0)
                    {
                        if (takenseats.IndexOf(seat.id) != -1)
                            seatModel.SeatType = 3;
                    }
                    if (seatModel.SeatType == 1)
                    {
                        if (reservedseats.Count > 0)
                        {
                            if (reservedseats.IndexOf(seat.id) != -1)
                            {
                                seatModel.SeatType = 3;
                            }
                        }
                        

                        if (seatModel.SeatType == 1 && selectedseats.Count > 0)
                        {
                            //TODO please find a better function to set patron key
                            foreach (var ss in selectedseats)
                            {
                                if (ss.cinema_seat_id == seat.id)
                                {
                                    seatModel.SeatType = 2;
                                    seatModel.PatronKey = ss.movies_schedule_list_patron_id;

                                    string strPatronName = string.Empty;
                                    decimal decPrice = 0;
                                    foreach (var p in Patrons)
                                    {
                                        if (p != null && p.Key == seatModel.PatronKey)
                                        {
                                            strPatronName = p.Name;
                                            decPrice = p.Price;
                                            break;
                                        }
                                    }

                                    SelectedPatronSeatList.PatronSeats.Add(new PatronSeatModel()
                                    {
                                        Key = ss.id,
                                        SeatKey = seatModel.Key,
                                        SeatName = seatModel.Name,
                                        PatronName = strPatronName,
                                        Price = decPrice,
                                        ReservedDate = (DateTime)ss.reserved_date
                                    });

                                    break;
                                }
                            }
                        }
                    }
                    
                    //TODO add timer for reserved (least priority)


                    Seats.Add(seatModel);
                }
                
            }
            blnIsUpdating = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SeatIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (blnIsUpdating)
                return;
            timer.Stop();

            bool blnHasChanges = false;
            try
            {
                SeatModel seatModel = (SeatModel)((Canvas)sender).DataContext;
                if (seatModel.Type == 1) //only seats
                {
                    if (seatModel.SeatType == 1 || seatModel.SeatType == 2) //available
                    {
                        PatronModel patron = null;
                        if (seatModel.SeatType == 2) //get patron
                        {
                            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                            {
                                string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                                var patronid = (from mcths in context.movies_schedule_list_house_seat_view
                                                where mcths.cinema_seat_id == seatModel.Key && mcths.movies_schedule_list_id == MovieSchedule.Key &&
                                                mcths.session_id == strSessionId
                                                select mcths.movies_schedule_list_patron_id).FirstOrDefault();
                                if (patronid != null)
                                {
                                    if (Patrons.Count > 0)
                                    {
                                        foreach (var p in Patrons)
                                        {
                                            if (p != null && p.Key == patronid)
                                            {
                                                patron = p;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        PatronWindow patronWindow = new PatronWindow(MovieSchedule, seatModel, Patrons, patron);
                        patronWindow.Owner = Window.GetWindow(this);
                        patronWindow.ShowDialog();
                        blnHasChanges = patronWindow.IsUpdated;
                    }
                }
            }
            catch { }

            if (blnHasChanges)
                this.UpdateMovieSchedule();
            timer.Start();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.ClearSelection();
            this.UpdateMovieSchedule();
            timer.Start();
        }

        private void RemovePatronSeat_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            PatronSeatModel patronSeatModel = (PatronSeatModel)((Button)sender).DataContext;

            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
            {
                var ps = (from mslhs in context.movies_schedule_list_house_seat
                          where mslhs.id == patronSeatModel.Key
                          select mslhs).FirstOrDefault();
                if (ps != null)
                {
                    context.movies_schedule_list_house_seat.DeleteObject(ps);
                    context.SaveChanges();
                }
            }

            this.UpdateMovieSchedule();
            timer.Start();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            if (SelectedPatronSeatList.PatronSeats.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No seat has been selected.";
                messageWindow.ShowDialog();
                timer.Start();
                return;
            }
            else
            {
                //call payment
                NavigationService.Navigate(new Uri("TenderAmountPage.xaml", UriKind.Relative));
            }
        }
    }
}
