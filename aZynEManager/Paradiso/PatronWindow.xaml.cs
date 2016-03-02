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
using MahApps.Metro.Controls;
using Paradiso.Model;
using System.Collections.ObjectModel;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for PatronWindow.xaml
    /// </summary>
    public partial class PatronWindow : MetroWindow, IDisposable
    {
        public MovieScheduleListModel MovieSchedule { get; set; }
        public SeatModel Seat { get; set; }
        public ObservableCollection<PatronModel> Patrons { get; set; }
        public PatronModel SelectedPatron { get; set; }
        public bool IsUpdated { get; set; }

        public PatronWindow(MovieScheduleListModel _movieSchedule, SeatModel _seatModel, ObservableCollection<PatronModel> _patrons, PatronModel _selectedPatron)
        {
            InitializeComponent();
            
            MovieSchedule = _movieSchedule;
            Seat = _seatModel;
            Patrons = _patrons;
            SelectedPatron = _selectedPatron;
            IsUpdated = false;

            this.DataContext = this;
        }

        private void PatronsListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(PatronsListView, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                SelectedPatron = (PatronModel) item.Content;
                {
                    using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                    {
                        //TODO checks if seat is already taken
                        //TODO checks if seat is already reserved

                        //selected seats 
                        string strSessionId = ParadisoObjectManager.GetInstance().SessionId;

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
                            if (Seat.SeatType == 1 && SelectedPatron != null) //add
                            {
                                movies_schedule_list_house_seat mslhs = new movies_schedule_list_house_seat()
                                {
                                    movies_schedule_list_id = MovieSchedule.Key,
                                    cinema_seat_id = Seat.Key,
                                    reserved_date = ParadisoObjectManager.GetInstance().CurrentDate,
                                    session_id = strSessionId,
                                    movies_schedule_list_patron_id = SelectedPatron.Key
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
                                    if (SelectedPatron == null)
                                    {
                                        context.movies_schedule_list_house_seat.DeleteObject(r);
                                    }
                                    else //update reservation
                                    {
                                        r.movies_schedule_list_patron_id = SelectedPatron.Key;
                                    }

                                    context.SaveChanges();
                                    IsUpdated = true;
                                }
                            }

                        }
                    }
                }

                this.Close();
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            Patrons.Clear();
        }

        #endregion
    }
}
