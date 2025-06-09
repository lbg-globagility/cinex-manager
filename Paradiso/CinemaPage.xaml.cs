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
using System.Collections.ObjectModel;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for CinemaPage.xaml
    /// </summary>
    public partial class CinemaPage : UserControl, IDisposable
    {
        public ObservableCollection<CinemaModel> Cinemas { get; set; }
        public CinemaModel Cinema { get; set; }
        public ObservableCollection<SeatModel> Seats { get; set; }

        private Dictionary<int, bool> SeatKeys = new Dictionary<int, bool>();
        private Dictionary<int, bool> Changes = new Dictionary<int, bool>();

        public CinemaPage()
        {
            InitializeComponent();

            Cinemas = new ObservableCollection<CinemaModel>();
            Seats = new ObservableCollection<SeatModel>();
            this.GetCinemas();

            this.DataContext = this;
        }

        public void Dispose()
        {
            CinemaComboBox.SelectionChanged -= CinemaComboBox_SelectionChanged;
            Cinemas.Clear();
            Seats.Clear();
        }

        private void GetCinemas()
        {
            Cinemas.Clear();
            try
            {
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var c = context.cinemas.OrderBy(x => x.in_order).ToList();
                    if (c.Count > 0)
                    {
                        foreach (var _c in c)
                            Cinemas.Add(new CinemaModel() { Id = _c.id, Name = _c.name });
                    }
                }
            }
            catch { }
        }

        private void CinemaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Seats.Clear();
            SeatKeys.Clear();
            Changes.Clear();
            Save.IsEnabled = Cinema != null && Changes.Count > 0;

            CinemaComboBox.IsEnabled = false;
            Save.IsEnabled = false;
            Cancel.IsEnabled = false;

            if (Cinema != null)
            {
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    var seats = (from s in context.cinema_seat
                                 where s.cinema_id == Cinema.Id
                                 select s).ToList();
                    if (seats.Count == 0)
                        return;

                    int padding = 50;
                    var max_x = seats.Max(s => s.x2);
                    var max_y = seats.Max(s => s.y2);

                    SeatItemsControl.Width = (int)(max_x) + padding;
                    SeatItemsControl.Height = (int)(max_y) + padding;


                    foreach (var seat in seats)
                    {
                        SeatModel seatModel = new SeatModel()
                        {
                            Key = seat.id,
                            Name = string.Format("{0}{1}", seat.col_name, seat.row_name),
                            X = (int)seat.x1,
                            Y = (int)seat.y1,
                            Width = (int)seat.x2 - (int)seat.x1,
                            Height = (int)seat.y2 - (int)seat.y1,
                            Type = (int)seat.object_type,
                            SeatType = 1,
                            IsHandicapped = (seat.is_handicapped == 1) ? true : false,
                            IsDisabled = (seat.is_disabled == 1) ? true : false
                        };

                        SeatKeys.Add(seatModel.Key, seatModel.IsDisabled);
                        //get seat type
                        if (seatModel.IsDisabled && seatModel.SeatColor == 0)
                            seatModel.SeatColor = 8421504;

                        Seats.Add(seatModel);
                    }
                }
            }

            CinemaComboBox.IsEnabled = true;
            Save.IsEnabled = Cinema != null && Changes.Count > 0;
            Cancel.IsEnabled = true;

            //MessageBox.Show(Cinema.Id.ToString());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //this.Dispose();
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.SwitchContent("MovieCalendarPage.xaml");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Changes.Count > 0)
            {
                int count = 0;
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    foreach (var change in Changes)
                    {
                        try
                        {
                            var cinemaseat = context.cinema_seat.Where(x => x.id == change.Key).FirstOrDefault();
                            if (cinemaseat != null)
                            {
                                cinemaseat.is_disabled = Convert.ToSByte(change.Value);
                                context.SaveChanges();
                                SeatKeys[change.Key] = change.Value;
                                count++;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                if (count == 0)
                    MessageBox.Show("No seat has been saved.");
                else
                    MessageBox.Show(string.Format(count == 1 ? "{0} seat has been saved." : "{0} seats have been saved.", count));

                Changes.Clear();
                Save.IsEnabled = Cinema != null && Changes.Count > 0;
            }
        }

        private void SeatIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas seatCanvas = (Canvas)sender;
            try
            {
                SeatModel seatModel = (SeatModel)(seatCanvas).DataContext;
                int seatKey = seatModel.Key;
                int _seatsCount = Seats.Count;
                if (_seatsCount > 0)
                {
                    for (int _i = 0; _i < _seatsCount; _i++)
                    {
                        if (Seats[_i].Key == seatKey)
                        {
                            Seats[_i].IsDisabled = !Seats[_i].IsDisabled;

                            if (Seats[_i].IsDisabled == SeatKeys[seatKey])
                            {
                                //remove if exists
                                if (Changes.ContainsKey(seatKey))
                                    Changes.Remove(seatKey);
                            }
                            else
                            {
                                if (Changes.ContainsKey(seatKey))
                                    Changes[seatKey] = Seats[_i].IsDisabled;
                                else
                                    Changes.Add(seatKey, Seats[_i].IsDisabled);
                            }
                        }
                    }
                }

                Save.IsEnabled = Cinema != null && Changes.Count > 0;
                e.Handled = true;
            }
            catch (Exception ex)
            {
            }

        }
    }
}
