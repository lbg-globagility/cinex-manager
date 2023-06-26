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
using System.Windows.Controls.Primitives;
using CinemaCustomControlLibrary.ViewModel;
using CinemaCustomControlLibrary.Model;
using CinemaCustomControlLibrary.Converters;

namespace CinemaCustomControlLibrary
{
    /// <summary>
    /// Interaction logic for SeatWindow.xaml
    /// </summary>
    public partial class SeatWindow : MetroWindow
    {
        private int intSeatDimension = 36;
        private int intScreenDimension = 900;
        private SeatsViewModel seatsViewModel;
        private Canvas previousCanvas;

        Nullable<Point> dragStart = null;

        public SeatWindow()
        {
            InitializeComponent();

            seatsViewModel = new SeatsViewModel();

            this.DataContext = seatsViewModel;
        }

        private void SeatMode_Click(object sender, RoutedEventArgs e)
        {

            //add new seat
            seatsViewModel.Seats.Add(new SeatModel()
            {
                Key = 0,
                X = 0,
                Y = 0,
                ColumnName = seatsViewModel.MaxColumnName,
                RowName = seatsViewModel.NextRowName,
                Width = intSeatDimension,
                Height = intSeatDimension,
                Type = 1,
                IsHandicapped = false,
            });
        }

        private void ScreenButton_Click(object sender, RoutedEventArgs e)
        {
            seatsViewModel.Seats.Add(new SeatModel()
            {
                Key = 0,
                X = 0,
                Y = 0,
                ColumnName = "",
                RowName = "",
                Width = intScreenDimension,
                Height = intSeatDimension,
                Type = 2,
                IsHandicapped = false,
            });
        }


        private void GridSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //this.UpdateSeatCanvas();
        }


        public void ValidateCinemaSeats(int intCapacity)
        {
            if (seatsViewModel.ScreenCount == 0)
            {
                throw new Exception("No screen set.");
            }
            else if (seatsViewModel.SeatCount == 0)
            {
                throw new Exception("No seats set.");
            }
            else if (seatsViewModel.SeatCount > intCapacity)
            {
                throw new Exception("Number of seats is beyond capacity.");
            }
        }

        public void SaveCinemaSeats(int intCinemaKey)
        {
            using (var context = new cinemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("CinemaModel")))
            {
                //removed deleted seats if any
                List<int> oldseatids = new List<int>();
                foreach (var seat in seatsViewModel.Seats)
                {
                    if (seat.Key != 0)
                        oldseatids.Add(seat.Key);
                }

                if (oldseatids.Count > 0)
                {
                    var oldseats = (from cs in context.cinema_seat where cs.cinema_id == intCinemaKey select cs).ToList();
                    if (oldseats.Count > 0)
                    {
                        foreach (var oldseat in oldseats)
                        {
                            if (oldseatids.IndexOf(oldseat.id) == -1)
                            {
                                context.cinema_seat.DeleteObject(oldseat);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                //insert or update cinema seats
                foreach (var seat in seatsViewModel.Seats)
                {
                    //cinemaseat
                    if (seat.Key == 0)
                    {
                        cinema_seat cseat = new cinema_seat()
                        {
                            id = seat.Key,
                            cinema_id = intCinemaKey,
                            x1 = seat.X,
                            y1 = seat.Y,
                            x2 = seat.X + seat.Width,
                            y2 = seat.Y + seat.Height,
                            row_name = seat.RowName,
                            col_name = seat.ColumnName,
                            object_type = seat.Type,
                            is_handicapped = (sbyte) ( (seat.Type == 1 && seat.IsHandicapped) ? 1 : 0),
                        };
                        context.cinema_seat.AddObject(cseat);
                    }
                    else
                    {
                        var oldseat = (from cs in context.cinema_seat where cs.id == seat.Key select cs).SingleOrDefault();
                        if (oldseat != null)
                        {
                            oldseat.x1 = seat.X;
                            oldseat.y1 = seat.Y;
                            oldseat.x2 = seat.X + seat.Width;
                            oldseat.y2 = seat.Y + seat.Height;
                            oldseat.row_name = seat.RowName;
                            oldseat.col_name = seat.ColumnName;
                            oldseat.object_type = seat.Type;
                            oldseat.is_handicapped = (sbyte)((seat.Type == 1 && seat.IsHandicapped) ? 1 : 0);
                        }
                    }
                    context.SaveChanges();
                }
            }
        }

        public void LoadCinema(int intKey, string strCinemaName, int intCapacity)
        {
            seatsViewModel.CinemaKey = intKey;
            seatsViewModel.CinemaName = strCinemaName;
            seatsViewModel.Capacity = intCapacity;
            
            seatsViewModel.Seats.Clear();

            using (var context = new cinemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("CinemaModel")))
            {
                var seats = from cs in context.cinema_seat where cs.cinema_id == seatsViewModel.CinemaKey select cs;
                foreach (var seat in seats)
                {
                    seatsViewModel.Seats.Add(new SeatModel()
                    {
                        Key = seat.id,
                        CinemaKey = seat.cinema_id,
                        X = (int)seat.x1,
                        Y = (int)seat.y1,
                        Width = (int)(seat.x2 - seat.x1),
                        Height = (int)(seat.y2 - seat.y1),
                        ColumnName = seat.col_name,
                        RowName = seat.row_name,
                        Type = (int)seat.object_type,
                        IsHandicapped = ( (byte) seat.is_handicapped == 1) ? true : false
                    });
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.GridSlider.Value = 50;
            //this.UpdateSeatCanvas();

        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.UpdateSeatCanvas();
        }

        private void SelectItem_Checked(object sender, RoutedEventArgs e)
        {
            //this.UpdateSeatCanvas();
        }

        private void SelectItem_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.UpdateSeatCanvas();
        }

        private void SeatCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //setup adorner
            if (previousCanvas != null)
            {
                Adorner[] toRemoveArray = AdornerLayer.GetAdornerLayer(previousCanvas).GetAdorners(previousCanvas);
                if (toRemoveArray != null)
                {
                    Adorner toRemove = toRemoveArray[0];
                    AdornerLayer.GetAdornerLayer(previousCanvas).Remove(toRemove);
                }
            }

            Canvas canvas = (Canvas)sender;
            CinemaCustomControlLibrary.Adorners.SimpleCircleAdorner adorner = new CinemaCustomControlLibrary.Adorners.SimpleCircleAdorner(canvas);
            AdornerLayer.GetAdornerLayer(canvas).Add(adorner);
            previousCanvas = canvas;

            //update binding
            seatsViewModel.SelectedSeat = (SeatModel)(canvas.DataContext);

            //move
            var element = (UIElement)sender;
            dragStart = e.GetPosition(element);
            element.CaptureMouse();
        }


        private void SeatCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var element = (UIElement)sender;
                var dragEnd = e.GetPosition(element);

                var seat = (SeatModel)( ((Canvas) sender).DataContext);
                seat.X += (int) ( dragEnd.X - dragStart.Value.X);
                seat.Y += (int) (dragEnd.Y - dragStart.Value.Y);
            }
        }

        private void SeatCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            dragStart = null;
            element.ReleaseMouseCapture();
        }

        private void SeatItemsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SeatItemsControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (seatsViewModel.SelectedSeat != null)
            {
                if (e.Key == Key.Delete)
                {
                    seatsViewModel.Seats.Remove(seatsViewModel.SelectedSeat);
                    seatsViewModel.SelectedSeat = null;
                    previousCanvas = null;
                }
                else if (e.Key == Key.Left)
                {
                    seatsViewModel.SelectedSeat.X--;
                }
                else if (e.Key == Key.Right)
                {
                    seatsViewModel.SelectedSeat.X++;
                }
                else if (e.Key == Key.Up)
                {
                    seatsViewModel.SelectedSeat.Y--;
                }
                else if (e.Key == Key.Down)
                {
                    seatsViewModel.SelectedSeat.Y++;
                }
            }
        
        }

    }
}
