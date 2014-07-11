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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CinemaCustomControlLibrary
{
    /// <summary>
    /// Interaction logic for SeatWindow.xaml
    /// </summary>
    public partial class SeatWindow : MetroWindow
    {
        public enum CinemaButtonMode
        {
            IdleButtonMode,
            SeatButtonMode,
            ScreenButtonMode
        }

        public CinemaButtonMode ButtonMode { get; set; }

        public int SeatCapacity { get; set; }
        public List<CinemaSeat> CinemaSeats { get; set; }
        public List<CinemaScreen> CinemaScreens { get; set; }

        double dblHeight = 24.0;
        double dblWidth = 24.0;

        double dblScreenHeight = 35.0;
        double dblScreenWidth = 900.0;

        public SeatWindow()
        {
            InitializeComponent();

            ButtonMode = CinemaButtonMode.IdleButtonMode;
            CinemaScreens = new List<CinemaScreen>();
            CinemaSeats = new List<CinemaSeat>();
        }

        private void SeatMode_Click(object sender, RoutedEventArgs e)
        {
            ButtonMode = CinemaButtonMode.SeatButtonMode;


            this.XName.Text = string.Empty;
            this.YName.Text = string.Empty;
            this.AngleName.Text = string.Empty;
            this.ColumnName.Text = string.Empty;
            this.RowName.Text = string.Empty;
            this.Handicapped.IsChecked = false;
        }

        private void ScreenButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonMode = CinemaButtonMode.ScreenButtonMode;
        }

        private void UpdateSeatCanvas()
        {
            bool blnIsHitTestVisible = false;
            if (SelectItem.IsChecked == true)
                blnIsHitTestVisible = true;

            SeatCanvas.Children.Clear();

            //grid lines

            double dblMaxWidth = SeatCanvas.ActualWidth;
            double dblMaxHeight = SeatCanvas.ActualHeight;
            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(2);
            dashes.Add(2);

            if (GridSlider.Value > 0)
            {
                for (int i = 0; i < dblMaxWidth; i += (int)GridSlider.Value)
                {
                    if (i == 0)
                        continue;
                    Line myLine = new Line();
                    myLine.Stroke = System.Windows.Media.Brushes.LightGray;
                    myLine.StrokeDashArray = dashes;
                    myLine.X1 = i;
                    myLine.Y1 = 0;
                    myLine.X2 = i;
                    myLine.Y2 = dblMaxHeight;
                    myLine.SnapsToDevicePixels = true;
                    myLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                    SeatCanvas.Children.Add(myLine);
                }
                for (int i = 0; i < dblMaxHeight; i += (int)GridSlider.Value)
                {
                    if (i == 0)
                        continue;
                    Line myLine = new Line();
                    myLine.Stroke = System.Windows.Media.Brushes.LightGray;
                    myLine.StrokeDashArray = dashes;
                    myLine.X1 = 0;
                    myLine.Y1 = i;
                    myLine.X2 = dblMaxWidth;
                    myLine.Y2 = i;
                    myLine.SnapsToDevicePixels = true;
                    myLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                    SeatCanvas.Children.Add(myLine);
                }
            }

            //screen
            if (CinemaScreens.Count > 0)
            {
                ContentControl contentControl = new ContentControl()
                {
                    Width = CinemaScreens[0].Width,
                    Height = CinemaScreens[0].Height,
                };

                Style style = this.FindResource("DesignerItemStyle") as Style;
                contentControl.Style = style;

                Image image = new Image()
                {
                    Width = CinemaScreens[0].Width,
                    Height = CinemaScreens[0].Height,
                    Source = new BitmapImage(new Uri(@"/CinemaCustomControlLibrary;component/Images/screen.png", UriKind.Relative)),
                    Stretch = Stretch.Fill,
                    IsHitTestVisible = blnIsHitTestVisible,
                };

                contentControl.Content = image;

                SeatCanvas.Children.Add(contentControl);
                Canvas.SetLeft(contentControl, CinemaScreens[0].X1);
                Canvas.SetTop(contentControl, CinemaScreens[0].Y1);
            }
            
            ScreenButton.IsEnabled = (CinemaScreens.Count == 0);

            //seats
            if (CinemaSeats.Count > 0)
            {


                foreach (CinemaSeat cinemaSeat in CinemaSeats)
                {
                    ContentControl contentControl = new ContentControl()
                    {
                        Width = dblWidth,
                        Height = dblHeight,
                    };

                    Style style = this.FindResource("DesignerItemStyle") as Style;
                    contentControl.Style = style;

                    SeatControl seatControl = new SeatControl(cinemaSeat);
                    seatControl.IsHitTestVisible = blnIsHitTestVisible;

                    contentControl.Content = seatControl;

                    SeatCanvas.Children.Add(contentControl);
                    Canvas.SetLeft(contentControl, cinemaSeat.X1);
                    Canvas.SetTop(contentControl, cinemaSeat.Y1);
                }
            }

            SeatLeft.Text = string.Format("{0:#;0;0}", SeatCapacity - CinemaSeats.Count);

            SeatMode.IsEnabled = (SeatCapacity - CinemaSeats.Count) > 0;
        }

        private void GridSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.UpdateSeatCanvas();
        }

        private void SeatCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point x = //e.MouseDevice.GetPosition(this);
            e.GetPosition(SeatCanvas);
            //checks if selection intertwines with existing select seat
            //create seat

            CinemaSeat cinemaSeat = new CinemaSeat();

            if (this.ButtonMode == CinemaButtonMode.SeatButtonMode)
            {
                cinemaSeat.CX = x.X;
                cinemaSeat.CY = x.Y;

                System.Drawing.Point point = new
                    System.Drawing.Point((int)cinemaSeat.CX, (int)cinemaSeat.CY);
                System.Drawing.Point point1 = new
                    System.Drawing.Point((int)(cinemaSeat.CX - (dblWidth / 2)), (int)(cinemaSeat.CY - (dblHeight / 2)));
                System.Drawing.Point point2 = new
                    System.Drawing.Point((int)(cinemaSeat.CX - (dblWidth / 2)), (int)(cinemaSeat.CY + (dblHeight / 2)));
                System.Drawing.Point point3 = new
                    System.Drawing.Point((int)(cinemaSeat.CX + (dblWidth / 2)), (int)(cinemaSeat.CY - (dblHeight / 2)));
                System.Drawing.Point point4 = new
                    System.Drawing.Point((int)(cinemaSeat.CX + (dblWidth / 2)), (int)(cinemaSeat.CY + (dblHeight / 2)));

                if (CinemaSeats.Count > 0)
                {
                    foreach (CinemaSeat _cinemaSeat in CinemaSeats)
                    {
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)_cinemaSeat.X1,
                            (int)_cinemaSeat.Y1, (int)_cinemaSeat.X2 - (int)_cinemaSeat.X1,
                            (int)_cinemaSeat.Y2 - (int)_cinemaSeat.Y1);

                        if (rect.Contains(point) || rect.Contains(point1) || rect.Contains(point2)
                            || rect.Contains(point3) || rect.Contains(point4))
                        {
                            return;
                        }
                    }
                }

                cinemaSeat.A = 0.0; //test


                //double dblX1 = Math.Cos(cinemaSeat.A) * (dblWidth/2);
                /*
                Image image = new Image
                {
                    Width = dblWidth,
                    Height = dblHeight,
                    Source = new BitmapImage(new Uri(@"/CinemaCustomControlLibrary;component/Images/seat.png", UriKind.Relative))
                };
                */


                if (cinemaSeat.A != 0.0)
                {
                    /*
                    RotateTransform rotateTransform = new RotateTransform(cinemaSeat.A);
                    rotateTransform.CenterX = x.X - (dblHeight / 2);
                    rotateTransform.CenterY = x.Y - (dblHeight / 2);
                    image.RenderTransform = rotateTransform;
                    */
                }

                cinemaSeat.X1 = cinemaSeat.CX - (dblWidth / 2);
                cinemaSeat.Y1 = (int) ( cinemaSeat.CY - (dblHeight / 2));
                cinemaSeat.X2 = cinemaSeat.X1 + dblWidth;
                cinemaSeat.Y2 = cinemaSeat.Y1 + dblHeight;

                SeatControl seatControl = new SeatControl(cinemaSeat);

                ContentControl contentControl = new ContentControl()
                {
                    Width = dblWidth,
                    Height = dblHeight,
                };

                Style style = this.FindResource("DesignerItemStyle") as Style;
                contentControl.Style = style;

                contentControl.Content = seatControl;
                SeatCanvas.Children.Add(contentControl);
                Canvas.SetLeft(contentControl, cinemaSeat.X1);
                Canvas.SetTop(contentControl, cinemaSeat.Y1);

                //add to list
                CinemaSeats.Add(new CinemaSeat(cinemaSeat));

                this.UpdateSeatCanvas();

            }
            else if (this.ButtonMode == CinemaButtonMode.ScreenButtonMode)
            {
                if (CinemaScreens.Count == 0)
                {
                    CinemaScreen cinemaScreen = new CinemaScreen();

                    cinemaScreen.CX = x.X;
                    cinemaScreen.CY = x.Y;



                    System.Drawing.Point point = new
                        System.Drawing.Point((int)cinemaScreen.CX, (int)cinemaScreen.CY);
                    System.Drawing.Point point1 = new
                        System.Drawing.Point((int)(cinemaScreen.CX - (dblScreenWidth / 2)), (int)(cinemaScreen.CY - (dblScreenWidth / 2)));
                    System.Drawing.Point point2 = new
                        System.Drawing.Point((int)(cinemaScreen.CX - (dblScreenWidth / 2)), (int)(cinemaScreen.CY + (dblScreenHeight / 2)));
                    System.Drawing.Point point3 = new
                        System.Drawing.Point((int)(cinemaScreen.CX + (dblScreenWidth / 2)), (int)(cinemaScreen.CY - (dblScreenHeight / 2)));
                    System.Drawing.Point point4 = new
                        System.Drawing.Point((int)(cinemaScreen.CX + (dblScreenWidth / 2)), (int)(cinemaScreen.CY + (dblScreenHeight / 2)));

                    if (CinemaSeats.Count > 0)
                    {
                        foreach (CinemaSeat _cinemaSeat in CinemaSeats)
                        {
                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)_cinemaSeat.X1,
                                (int)_cinemaSeat.Y1, (int)_cinemaSeat.X2 - (int)_cinemaSeat.X1,
                                (int)_cinemaSeat.Y2 - (int)_cinemaSeat.Y1);

                            if (rect.Contains(point) || rect.Contains(point1) || rect.Contains(point2)
                                || rect.Contains(point3) || rect.Contains(point4))
                            {
                                return;
                            }
                        }
                    }

                    cinemaScreen.A = 0.0; //test
                    cinemaScreen.X1 = cinemaScreen.CX - (dblScreenWidth / 2);
                    cinemaScreen.Y1 = (int)(cinemaScreen.CY - (dblScreenHeight / 2));
                    cinemaScreen.X2 = cinemaScreen.X1 + dblScreenWidth;
                    cinemaScreen.Y2 = cinemaScreen.Y1 + dblScreenHeight;
                    cinemaScreen.IsResizable = true;
                    
                    CinemaScreens.Add(new CinemaScreen(cinemaScreen));

                    this.UpdateSeatCanvas();
                }
            }
            else
            {
                /*
                foreach (UIElement child in SeatCanvas.Children)
                {
                    if (child is Control)
                        Selector.SetIsSelected((Control)child, true);
                }
                */

                cinemaSeat.CX = x.X;
                cinemaSeat.CY = x.Y;

                System.Drawing.Point point = new
                    System.Drawing.Point((int)cinemaSeat.CX, (int)cinemaSeat.CY);
                System.Drawing.Point point1 = new
                    System.Drawing.Point((int)(cinemaSeat.CX - (dblWidth / 2)), (int)(cinemaSeat.CY - (dblHeight / 2)));
                System.Drawing.Point point2 = new
                    System.Drawing.Point((int)(cinemaSeat.CX - (dblWidth / 2)), (int)(cinemaSeat.CY + (dblHeight / 2)));
                System.Drawing.Point point3 = new
                    System.Drawing.Point((int)(cinemaSeat.CX + (dblWidth / 2)), (int)(cinemaSeat.CY - (dblHeight / 2)));
                System.Drawing.Point point4 = new
                    System.Drawing.Point((int)(cinemaSeat.CX + (dblWidth / 2)), (int)(cinemaSeat.CY + (dblHeight / 2)));

                if (CinemaSeats.Count > 0)
                {
                    foreach (CinemaSeat _cinemaSeat in CinemaSeats)
                    {
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)_cinemaSeat.X1,
                            (int)_cinemaSeat.Y1, (int)_cinemaSeat.X2 - (int)_cinemaSeat.X1,
                            (int)_cinemaSeat.Y2 - (int)_cinemaSeat.Y1);

                        if (rect.Contains(point) || rect.Contains(point1) || rect.Contains(point2)
                            || rect.Contains(point3) || rect.Contains(point4))
                        {
                            foreach (UIElement child in SeatCanvas.Children)
                            {
                                if (child is Control)
                                {
                                    ContentControl contentControl = (ContentControl)child;
                                    if (contentControl.Content is SeatControl)
                                    {
                                        SeatControl seatControl = (SeatControl)contentControl.Content;
                                        if (seatControl.Seat.IsEqual(_cinemaSeat))
                                        {
                                            
                                            Selector.SetIsSelected((Control)child, true);

                                            this.XName.Text = string.Format("{0}", _cinemaSeat.CX);
                                            this.YName.Text = string.Format("{0}", _cinemaSeat.CY);
                                            this.AngleName.Text = string.Format("{0}", _cinemaSeat.A);
                                            this.ColumnName.Text = _cinemaSeat.ColumnName;
                                            this.RowName.Text = _cinemaSeat.RowName;
                                            this.Handicapped.IsChecked = (cinemaSeat.Type == CinemaSeat.SeatType.HandicappedTakenSeatType);
                                        }
                                        else
                                        {
                                            Selector.SetIsSelected((Control)child, false);
                                        }
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
            }



            //MessageBox.Show(string.Format("{0},{1}", x.X, x.Y));

            ButtonMode = CinemaButtonMode.IdleButtonMode;
            SeatCanvas.Cursor = Cursors.Arrow;

        }

        private void SeatCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.ButtonMode == CinemaButtonMode.ScreenButtonMode || this.ButtonMode == CinemaButtonMode.SeatButtonMode)
                SeatCanvas.Cursor = Cursors.Cross;
            else
                SeatCanvas.Cursor = Cursors.Arrow;
        }

        private void SeatCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            SeatCanvas.Cursor = Cursors.Arrow;
        }

        /*
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton selectionCheckBox = sender as ToggleButton;
            if (selectionCheckBox != null && selectionCheckBox.IsChecked == true)
            {
                foreach (UIElement child in SeatCanvas.Children)
                {
                    if (child is Control)
                        Selector.SetIsSelected((Control)child, true);
                }
            }
            else
            {
                foreach (UIElement child in SeatCanvas.Children)
                {
                    if (child is Control)
                        Selector.SetIsSelected((Control)child, false);
                }
            }

        }
        */

        private void SelectMode_Click(object sender, RoutedEventArgs e)
        {

        }


        public void LoadCinema(int intKey, string strCinemaName, int intCapacity)
        {
            CinemaName.Text = strCinemaName;
            SeatCapacity = intCapacity;
            CinemaCapacity.Text = string.Empty;
            CinemaScreens.Clear();
            CinemaSeats.Clear();

            double dblCX = 0.0;
            double dblCY = 0.0;

            using (var context = new cinemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("CinemaModel")))
            {
                var cinemas = from c in context.cinemas where c.id == intKey select c;

                foreach (var cinema in cinemas)
                {
                    if (strCinemaName == string.Empty)
                        CinemaName.Text = cinema.name;
                    
                    SeatCapacity = (int) cinema.capacity;
                    CinemaCapacity.Text = string.Format("{0:#,##0}", SeatCapacity);
                }

                var seats = from s in context.cinema_seat
                             where s.cinema_id == intKey && s.object_type == 2
                             select s;
                foreach (var seat in seats)
                {
                    CinemaScreens.Add(new CinemaScreen()
                    {
                        Key = seat.id,
                        X1 = (int) seat.x1,
                        Y1 = (int) seat.y1,
                        X2 = (int) seat.x2,
                        Y2 = (int) seat.y2
                    });

                }


                var screens = from s in context.cinema_seat
                             where s.cinema_id == intKey && s.object_type == 1
                             select s;
                foreach (var screen in screens)
                {
                    dblCX = (int) screen.x1 + (( (int) screen.x2 - (int) screen.x1) / 2);
                    dblCY = (int) screen.y1 + (( (int) screen.y2 - (int) screen.y1) / 2);

                    CinemaSeats.Add(new CinemaSeat()
                    {
                        Key = screen.id,
                        Name = string.Format("{0}{1}", screen.row_name, screen.col_name),
                        CX = dblCX,
                        CY = dblCY,
                        X1 = dblCX - (dblWidth / 2),
                        Y1 = dblCY - (dblHeight / 2),
                        X2 = dblCX + (dblWidth / 2),
                        Y2 = dblCY + (dblHeight / 2),
                    });
                }
            }

            if (SeatCapacity > intCapacity)
                SeatCapacity = intCapacity;
            CinemaCapacity.Text = string.Format("{0:#,##0}", SeatCapacity);

            this.UpdateSeatCanvas();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.GridSlider.Value = 50;
            this.UpdateSeatCanvas();
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateSeatCanvas();
            /*
            if (this.SelectItem.IsChecked == true)
            {
            }
            */
        }

        private void SelectItem_Checked(object sender, RoutedEventArgs e)
        {
            /*
            bool blnIsHitTestVisible = false;
            ToggleButton selectionCheckBox = sender as ToggleButton;
            if (selectionCheckBox != null && selectionCheckBox.IsChecked == true)
                blnIsHitTestVisible = true;
            */


            this.UpdateSeatCanvas();
        }

        private void SelectItem_Unchecked(object sender, RoutedEventArgs e)
        {
            this.UpdateSeatCanvas();
        }
    }
}
