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
using System.Transactions;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SeatingPage.xaml
    /// </summary>
    public partial class SeatingPage : Page
    {
        public MovieScheduleListModel MovieScheduleList { get; set; }
        
        public MovieScheduleListModel MovieSchedule { get; set; }
        
        public ObservableCollection<PatronModel> Patrons { get; set; }
        public ObservableCollection<SeatModel> Seats { get; set; }
        public PatronSeatListModel SelectedPatronSeatList { get; set; }
        public PatronModel SelectedPatron { get; set; }

        DispatcherTimer timer;
        private bool blnIsUpdating = false;

        public SeatModel Seat { get; set; }

        public bool IsReservedSeating { get; set; }
        public bool IsFreeSeating { get; set; }
        public bool IsLimitedFreeSeating { get; set; }
        public bool IsUnlimitedFreeSeating { get; set; }
        public bool IsReadOnly { get; set; }

        private bool isDragInProgress = false;
        private Point origCursorLocation;
        private double origHorizOffset;
        private double origVertOffset;
        private bool modifyLeftOffset;
        private bool modifyTopOffset;
        private UIElement elementBeingDragged;
        private bool IsEllapsed = false;

        public SeatingPage(MovieScheduleListModel movieScheduleListModel) 
        {
            InitializeComponent();

            MovieScheduleList = new MovieScheduleListModel(movieScheduleListModel);
            if (movieScheduleListModel.SeatType == 1) //reserved
            {
                IsReservedSeating = true;
                IsFreeSeating = false;
                IsLimitedFreeSeating = false;
                IsUnlimitedFreeSeating = false;
            }
            else if (movieScheduleListModel.SeatType == 2) //limited free seating
            {
                IsReservedSeating = false;
                IsFreeSeating = true;
                IsLimitedFreeSeating = true;
                IsUnlimitedFreeSeating = false;
            }
            else if (movieScheduleListModel.SeatType == 3) //unlimited free seating
            {
                IsReservedSeating = false;
                IsFreeSeating = true;
                IsLimitedFreeSeating = false;
                IsUnlimitedFreeSeating = true;
            }
            MovieSchedule = new MovieScheduleListModel();
            Seats = new ObservableCollection<SeatModel>();

            SelectedPatronSeatList = new PatronSeatListModel();
            Seat = new SeatModel();
            
            this.UpdateMovieSchedule();
            

            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000 * Constants.ReservedSeatingUiInterval);
            timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }

        public int Key 
        {
            get
            {
                if (MovieScheduleList == null)
                    return 0;
                else
                    return MovieScheduleList.Key;
            }
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

            var window = Window.GetWindow(this);
            window.KeyDown -= Page_PreviewKeyDown;

            NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
        }


        void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateMovieSchedule();
            this.setFocus();
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

                    var window = Window.GetWindow(this);
                    window.KeyDown -= Page_PreviewKeyDown;

                    NavigationService.Navigate(new Uri("MovieCalendarPage.xaml", UriKind.Relative));
                    return;
                }

                //checks if movie already expired
                DateTime dtNow = ParadisoObjectManager.GetInstance().CurrentDate;
                if (_movie_schedule_list.starttime.AddMinutes(_movie_schedule_list.laytime) < dtNow)
                    IsEllapsed = true;
                if (IsEllapsed && !ParadisoObjectManager.GetInstance().HasRights("PRIORDATE"))
                {
                    blnIsUpdating = false;
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Movie Schedule is already expired.";
                    messageWindow.ShowDialog();

                    var window = Window.GetWindow(this);
                    if (window != null)
                        window.KeyDown -= Page_PreviewKeyDown;

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
                if (IsFreeSeating)
                    reservedseats = (from mcths in context.movies_schedule_list_house_seat_free_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id != strSessionId
                                     select mcths.cinema_seat_id).ToList();
                //selected seats 
                var selectedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id == strSessionId
                                     select new { mcths.id, mcths.cinema_seat_id, mcths.movies_schedule_list_patron_id, mcths.reserved_date
                                     }).ToList();
                if (IsFreeSeating)
                    selectedseats = (from mcths in context.movies_schedule_list_house_seat_free_view
                                     where mcths.movies_schedule_list_id == this.Key && mcths.session_id == strSessionId
                                     select new
                                     {
                                         mcths.id,
                                         mcths.cinema_seat_id,
                                         mcths.movies_schedule_list_patron_id,
                                         mcths.reserved_date
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
                if (MovieSchedule.Available < 0)
                    MovieSchedule.Available = 0;

                var price = (from mslp in context.movies_schedule_list_patron
                                where mslp.movies_schedule_list_id == this.Key && mslp.is_default == 1
                                select mslp.price).FirstOrDefault();
                if (price != null)
                    MovieSchedule.Price = (decimal)price;


                if (MovieSchedule.Available <= 0 && MovieSchedule.SeatType != 3) //except unlimited seating
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
                if (MovieScheduleList.SeatType == 1) //for reserved seating only
                    Patrons.Add(null);
                var _patrons = (from mslp in context.movies_schedule_list_patron
                                where mslp.movies_schedule_list_id == this.Key
                                select new
                                {
                                    key = mslp.id,
                                    patronkey = mslp.patron_id,
                                    patroncode = mslp.patron.code,
                                    patronname = mslp.patron.name,
                                    price = mslp.price,
                                    seatcolor = mslp.patron.seat_color
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
                            Price = (decimal) _patron.price,
                            SeatColor = (int) _patron.seatcolor
                        });
                    }
                }

                //TODO add scrollbar and set maximum height and width, get maximum width to center 
                //just add padding
                Seats.Clear();
                SelectedPatronSeatList.PatronSeats.Clear();

                var seats = (from s in context.cinema_seat
                                where s.cinema_id == MovieSchedule.CinemaKey
                                select s).ToList();
                int padding = 50;
                var max_x = seats.Max(s => s.x2);
                var max_y = seats.Max(s => s.y2);

                SeatItemsControl.Width = (int) (max_x) + padding;
                SeatItemsControl.Height = (int)(max_y) + padding;


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
                    if (takenseats.Count > 0 && IsReservedSeating)
                    {
                        if (takenseats.IndexOf(seat.id) != -1)
                            seatModel.SeatType = 3;
                    }
                    if (seatModel.SeatType == 1)
                    {
                        if (reservedseats.Count > 0 && IsReservedSeating)
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
                                    int intSeatColor = 0;
                                    decimal decPrice = 0;
                                    foreach (var p in Patrons)
                                    {
                                        if (p != null && p.Key == seatModel.PatronKey)
                                        {
                                            strPatronName = p.Name;
                                            decPrice = p.Price;
                                            intSeatColor = p.SeatColor;
                                            break;
                                        }
                                    }

                                    SelectedPatronSeatList.PatronSeats.Add(new PatronSeatModel(
                                        ss.id,
                                        seatModel.Key,
                                        seatModel.Name,
                                        strPatronName,
                                        decPrice,
                                        (DateTime)ss.reserved_date,
                                        intSeatColor
                                    ));

                                    seatModel.SeatColor = intSeatColor;

                                    if (IsReservedSeating)
                                        break;
                                }
                            }
                        }
                    }
                    
                    Seats.Add(seatModel);
                }

                
            }
            blnIsUpdating = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Page_PreviewKeyDown;
            IsReadOnly = false;

            ParadisoObjectManager paradisoObjectManager = ParadisoObjectManager.GetInstance();
            if (paradisoObjectManager.HasRights("PRIORDATE") && (paradisoObjectManager.CurrentDate.Date > paradisoObjectManager.ScreeningDate
                || (paradisoObjectManager.CurrentDate.Date == paradisoObjectManager.ScreeningDate && IsEllapsed)
                ))
            {
                //disables everything
                PurchaseDetailsPanel.Visibility = Visibility.Hidden;
                clearButton.IsEnabled = false;
                clearButton.Visibility = Visibility.Collapsed;
                confirmButton.IsEnabled = false;
                confirmButton.Visibility = Visibility.Collapsed;
                IsReadOnly = true;
            }
        }

        private void SeatIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (blnIsUpdating)
                return;
            if (IsReadOnly || !MovieSchedule.IsEnabled)
                return;
            //timer.Stop();
            
            Canvas seatCanvas = (Canvas)sender;
            Seat = null;
            SelectedPatron = null;

            try
            {
                SeatModel seatModel = (SeatModel)(seatCanvas).DataContext;
                if (seatModel.Type == 1)
                {
                    if (seatModel.SeatType == 1 || seatModel.SeatType == 2) //available
                    {
                        Seat = seatModel;

                        //get selected patron
                        if (seatModel.SeatType == 2)
                        {
                            using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                            {
                                string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

                                var patronid = (from mcths in context.movies_schedule_list_house_seat_view
                                                where mcths.cinema_seat_id == seatModel.Key && mcths.movies_schedule_list_id == MovieSchedule.Key &&
                                                mcths.session_id == strSessionId
                                                select mcths.movies_schedule_list_patron_id).FirstOrDefault();
                                if (patronid != 0)
                                {
                                    if (Patrons.Count > 0)
                                    {
                                        foreach (var p in Patrons)
                                        {
                                            if (p != null && p.Key == patronid)
                                            {
                                                SelectedPatron = p;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        seatCanvas.ContextMenu.PlacementTarget = this;
                        seatCanvas.ContextMenu.IsOpen = true;

                    }
                    else if (seatModel.SeatType == 3)
                    {
                        if (!ParadisoObjectManager.GetInstance().HasRights("RESERVE"))
                            return;

                        string strSessionId = string.Empty;
                        //verify if reserved
                        ReservedSeatListModel reservedSeats = new ReservedSeatListModel();

                        using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                        {
                            var sessionid = (from mcths in context.movies_schedule_list_house_seat_view
                                             where mcths.cinema_seat_id == seatModel.Key && mcths.movies_schedule_list_id == MovieSchedule.Key &&
                                             mcths.notes == "RESERVED"
                                             select mcths.session_id).SingleOrDefault();
                            if (sessionid != null)
                                strSessionId = sessionid;
                            var reservedseats = (from mcths in context.movies_schedule_list_house_seat
                                                 where mcths.session_id == strSessionId && mcths.notes == "RESERVED" && mcths.movies_schedule_list_id == MovieSchedule.Key
                                                 orderby mcths.cinema_seat.id
                                                 select new
                                                 {
                                                     mcths.id,
                                                     sn = mcths.cinema_seat.row_name + mcths.cinema_seat.col_name,
                                                 }
                                                 ).ToList();

                            foreach (var _rs in reservedseats)
                            {
                                reservedSeats.ReservedSeats.Add(new ReservedSeatModel() { Key = _rs.id, Name = _rs.sn });
                            }

                        }

                        if (strSessionId != string.Empty)
                        {
                            ContextMenu cm = this.FindResource("cmUnReserve") as ContextMenu;
                            cm.PlacementTarget = sender as Canvas;


                            cm.DataContext = reservedSeats;
                            cm.IsOpen = true;
                        }
                    }
                }
                else if (IsFreeSeating)
                {
                    if (MovieSchedule.SeatType == 2 && MovieSchedule.Available == 0)
                    {
                        //prevent selection
                    }
                    else
                    {
                        seatCanvas.ContextMenu.PlacementTarget = this;
                        seatCanvas.ContextMenu.IsOpen = true;
                    }
                }
                e.Handled = true;
            }
            catch //(Exception ex) 
            { }
            this.setFocus();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.ClearSelection();
            this.UpdateMovieSchedule();
            //timer.Start();
            this.setFocus();
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
            //timer.Start();
            this.setFocus();

        }

        private void confirmPatrons()
        {
            timer.Stop();
            if (SelectedPatronSeatList.PatronSeats.Count == 0)
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageText.Text = "No seat has been selected.";
                messageWindow.ShowDialog();
                //timer.Start();
                this.setFocus();
                return;
            }
            else
            {
                //call payment
                var window = Window.GetWindow(this);
                window.KeyDown -= Page_PreviewKeyDown;

                NavigationService.Navigate(new Uri("TenderAmountPage.xaml", UriKind.Relative));
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.confirmPatrons();
        }

        private void setFocus()
        {
            this.Focus();
        }

        private void SeatCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void PatronsListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            bool IsUpdated = false;

            var item = ItemsControl.ContainerFromElement( (ListView) sender, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                PatronModel _SelectedPatron = (PatronModel)item.Content;

                string strSessionId = ParadisoObjectManager.GetInstance().SessionId;
                
                if (IsReservedSeating)
                {
                    {
                        using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                        {
                            //selected seats 

                            //taken seats
                            var takenseats = (from mslrs in context.movies_schedule_list_reserved_seat
                                              where mslrs.movies_schedule_list_id == MovieSchedule.Key && mslrs.cinema_seat_id == Seat.Key
                                              select mslrs.cinema_seat_id).Count();

                            //reserved seats from other sessions
                            var reservedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                                 where mcths.movies_schedule_list_id == MovieSchedule.Key && mcths.session_id != strSessionId && mcths.cinema_seat_id == Seat.Key
                                                 select mcths.cinema_seat_id).Count();

                            //selected seats 
                            var selectedseats = (from mcths in context.movies_schedule_list_house_seat_view
                                                 where mcths.movies_schedule_list_id == MovieSchedule.Key && mcths.session_id == strSessionId && mcths.cinema_seat_id == Seat.Key
                                                 select new { mcths.cinema_seat_id }).Count();

                            if (takenseats > 0 || reservedseats > 0) //seat already taken?
                            {

                            }
                            else if (Seat.SeatType == 1 && selectedseats > 0) //available but already selected?
                            {

                            }
                            else if (Seat.SeatType == 2 && selectedseats == 0) //selected but already expired?
                            {

                            }
                            else
                            {
                                //TODO do saving or updating here
                                if (Seat.SeatType == 1 && _SelectedPatron != null) //add
                                {
                                    movies_schedule_list_house_seat mslhs = new movies_schedule_list_house_seat()
                                    {
                                        movies_schedule_list_id = MovieSchedule.Key,
                                        cinema_seat_id = Seat.Key,
                                        reserved_date = ParadisoObjectManager.GetInstance().CurrentDate,
                                        session_id = strSessionId,
                                        movies_schedule_list_patron_id = _SelectedPatron.Key
                                    };
                                    context.movies_schedule_list_house_seat.AddObject(mslhs);
                                    context.SaveChanges();

                                    IsUpdated = true;
                                }
                                else if (Seat.SeatType == 2) //update
                                {
                                    //remove reservation
                                    var r = (from mslsh in context.movies_schedule_list_house_seat
                                             where mslsh.movies_schedule_list_id == MovieSchedule.Key && mslsh.session_id == strSessionId && mslsh.cinema_seat_id == Seat.Key
                                             select mslsh).FirstOrDefault();
                                    if (r != null)
                                    {
                                        if (_SelectedPatron == null)
                                        {
                                            context.movies_schedule_list_house_seat.DeleteObject(r);
                                        }
                                        else //update reservation
                                        {
                                            r.movies_schedule_list_patron_id = _SelectedPatron.Key;
                                        }

                                        context.SaveChanges();
                                        IsUpdated = true;
                                    }
                                }

                            }
                        }

                    }
                }
                else
                {
                    //check for limited seating if 
                    if (MovieSchedule.SeatType == 2 && MovieSchedule.Available == 0)
                    {
                        //do nothing
                        MessageWindow messageWindow = new MessageWindow();
                        messageWindow.MessageText.Text = "No more available seats.";
                        messageWindow.ShowDialog();
                        //timer.Start();
                        this.setFocus();

                        return;

                    }
                    else
                    {

                        using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                        {
                            //get screen object type
                            var screen_seat_id = (from cs in context.cinema_seat where cs.cinema_id == MovieSchedule.CinemaKey && cs.object_type == 2 select cs.id).First();
                            if (screen_seat_id != 0)
                            {
                                movies_schedule_list_house_seat mslhs = new movies_schedule_list_house_seat()
                                {
                                    movies_schedule_list_id = MovieSchedule.Key,
                                    cinema_seat_id = screen_seat_id,
                                    reserved_date = ParadisoObjectManager.GetInstance().CurrentDate,
                                    session_id = strSessionId,
                                    movies_schedule_list_patron_id = _SelectedPatron.Key
                                };
                                context.movies_schedule_list_house_seat.AddObject(mslhs);
                                context.SaveChanges();

                                IsUpdated = true;
                            }
                        }
                    }
                }
            }

            if (IsUpdated)
                this.UpdateMovieSchedule();
            //timer.Start();
            this.setFocus();

        }

        private void BlankCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.setFocus();

        }

        private void BlankCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas seatCanvas = (Canvas)sender;
            Seat = null;
            SelectedPatron = null;

            if (blnIsUpdating)
                return;
            if (IsReadOnly)
                return;

            seatCanvas.ContextMenu.PlacementTarget = this;
            seatCanvas.ContextMenu.IsOpen = true;
            
            e.Handled = true;
            this.setFocus();

        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.confirmPatrons();
            }
        }

        private void PurchaseDetailsText_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            elementBeingDragged = PurchaseDetailsPanel;
            elementBeingDragged.ReleaseMouseCapture();

            this.isDragInProgress = false;
            this.origCursorLocation = e.GetPosition(this);

            double left = Canvas.GetLeft(elementBeingDragged);
            double right = Canvas.GetRight(elementBeingDragged);
            double top = Canvas.GetTop(elementBeingDragged);
            double bottom = Canvas.GetBottom(elementBeingDragged);

            this.origHorizOffset = this.ResolveOffset(left, right, out modifyLeftOffset);
            this.origVertOffset = this.ResolveOffset(top, bottom, out modifyTopOffset);

            e.Handled = true;

            this.isDragInProgress = true;
        }

        private void PurchaseDetailsText_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (elementBeingDragged == null || !this.isDragInProgress)
                return;

            Point cursorLocation = e.GetPosition(this);

            double newHorizontalOffset;
            double newVerticalOffset;

            if (this.modifyLeftOffset)
                newHorizontalOffset =
                  this.origHorizOffset + (cursorLocation.X - this.origCursorLocation.X);
            else
                newHorizontalOffset =
                  this.origHorizOffset - (cursorLocation.X - this.origCursorLocation.X);

            if (this.modifyTopOffset)
                newVerticalOffset = this.origVertOffset + (cursorLocation.Y - this.origCursorLocation.Y);
            else
                newVerticalOffset = this.origVertOffset - (cursorLocation.Y - this.origCursorLocation.Y);

            Rect elemRect = this.CalculateDragElementRect(newHorizontalOffset, newVerticalOffset);

            //
            // If the element is being dragged out of the viewable area, 
            // determine the ideal rect location, so that the element is 
            // within the edge(s) of the canvas.
            //
            bool leftAlign = elemRect.Left < 0;
            bool rightAlign = elemRect.Right > this.ActualWidth;

            if (leftAlign)
                newHorizontalOffset = modifyLeftOffset ? 0 : this.ActualWidth - elemRect.Width;
            else if (rightAlign)
                newHorizontalOffset = modifyLeftOffset ? this.ActualWidth - elemRect.Width : 0;

            bool topAlign = elemRect.Top < 0;
            bool bottomAlign = elemRect.Bottom > this.ActualHeight;

            if (topAlign)
                newVerticalOffset = modifyTopOffset ? 0 : this.ActualHeight - elemRect.Height;
            else if (bottomAlign)
                newVerticalOffset = modifyTopOffset ? this.ActualHeight - elemRect.Height : 0;

            if (this.modifyLeftOffset)
                Canvas.SetLeft(elementBeingDragged, newHorizontalOffset);
            else
                Canvas.SetRight(elementBeingDragged, newHorizontalOffset);

            if (this.modifyTopOffset)
                Canvas.SetTop(elementBeingDragged, newVerticalOffset);
            else
                Canvas.SetBottom(elementBeingDragged, newVerticalOffset);
        }
        

        private void PurchaseDetailsText_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            elementBeingDragged = null;
        }

        private Rect CalculateDragElementRect(double newHorizOffset, double newVertOffset)
        {
            if (this.elementBeingDragged == null)
                throw new InvalidOperationException("ElementBeingDragged is null.");

            Size elemSize = this.elementBeingDragged.RenderSize;

            double x, y;

            if (this.modifyLeftOffset)
                x = newHorizOffset;
            else
                x = this.ActualWidth - newHorizOffset - elemSize.Width;

            if (this.modifyTopOffset)
                y = newVertOffset;
            else
                y = this.ActualHeight - newVertOffset - elemSize.Height;

            Point elemLoc = new Point(x, y);

            return new Rect(elemLoc, elemSize);
        }

        public double ResolveOffset(double side1, double side2, out bool useSide1)
        {
            useSide1 = true;
            double result;
            if (Double.IsNaN(side1))
            {
                if (Double.IsNaN(side2))
                {
                    // Both sides have no value, so set the
                    // first side to a value of zero.
                    result = 0;
                }
                else
                {
                    result = side2;
                    useSide1 = false;
                }
            }
            else
            {
                result = side1;
            }
            return result;
        }

        private void UnReserveSeat_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            ReservedSeatListModel rsl = (ReservedSeatListModel) b.DataContext;
            if (rsl.ReservedSeats.Count > 0)
            {
                MessageYesNoWindow messageYesNoWindow = new MessageYesNoWindow();
                messageYesNoWindow.MessageText.Text = "Do you want to unreserved these seats?";
                messageYesNoWindow.ShowDialog();
                if (!messageYesNoWindow.IsYes)
                    return;

                string strException = string.Empty;
                bool success = false;

                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            foreach (ReservedSeatModel rs in rsl.ReservedSeats)
                            {
                                var _rs = (from mslhs in context.movies_schedule_list_house_seat
                                           where mslhs.id == rs.Key
                                           select mslhs).FirstOrDefault();
                                if (_rs != null)
                                {
                                    context.movies_schedule_list_house_seat.DeleteObject(_rs);
                                    context.SaveChanges();
                                }
                            }

                            context.AcceptAllChanges();
                            transaction.Complete();
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                strException = ex.InnerException.Message.ToString();
                            else
                                strException = ex.Message.ToString();
                        }
                    }
                }

                if (success)
                {
                    //load as selected
                    this.UpdateMovieSchedule();
                }
                else
                {
                    MessageWindow messageWindow = new MessageWindow();
                    messageWindow.MessageText.Text = "Seats cannot be unreserved.";
                    messageWindow.ShowDialog();
                }
            }
            //prompt password for administrator

            //MessageBox.Show(rsl.ReservedSeats.Count.ToString());

        }
    }
}
