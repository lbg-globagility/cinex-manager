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

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for CinemaSeatControl.xaml
    /// </summary>
    public partial class SeatControl : UserControl
    {
        public event EventHandler SeatControlClicked; 

        public CinemaSeat Seat { get; set; }

        public SeatControl(CinemaSeat seat)
        {
            InitializeComponent();
            Seat = new CinemaSeat(seat);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SeatNameLabel.Text = Seat.Name;
            if (SeatNameLabel.Text.Length == 3)
                Canvas.SetLeft(SeatNameLabel, 3);
            else if (SeatNameLabel.Text.Length == 2)
                Canvas.SetLeft(SeatNameLabel, 7);
            else if (SeatNameLabel.Text.Length == 1)
                Canvas.SetLeft(SeatNameLabel, 12);

            this.SetSeatIcon();
        }

        private void SetSeatIcon()
        {
            if (Seat.Action == CinemaSeat.ActionType.NoActionType)
            {
                SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seats-taken.png", UriKind.Relative));
            }
            else
            {
                if (Seat.Type == CinemaSeat.SeatType.NormalLockedSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seats-selected.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.NormalTakenSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seats-taken.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.NormalAvailableSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seats-available.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.NormalReservedSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seats-reserved.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedLockedSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/disabled-selected.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedTakenSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/disabled-taken.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedAvailableSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/disabled-available.png", UriKind.Relative));
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedReservedSeatType)
                    SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/disabled-reserved.png", UriKind.Relative));

            }
        }

        private void SeatCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //action type
            if (Seat.Action == CinemaSeat.ActionType.TakenActionType)
            {
                if (Seat.Type == CinemaSeat.SeatType.NormalTakenSeatType || Seat.Type == CinemaSeat.SeatType.HandicappedTakenSeatType)
                    return;
                if (Seat.Type == CinemaSeat.SeatType.NormalAvailableSeatType)
                    Seat.Type = CinemaSeat.SeatType.NormalLockedSeatType;
                else if (Seat.Type == CinemaSeat.SeatType.NormalLockedSeatType)
                    Seat.Type = CinemaSeat.SeatType.NormalAvailableSeatType;
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedAvailableSeatType)
                    Seat.Type = CinemaSeat.SeatType.HandicappedLockedSeatType;
                else if (Seat.Type == CinemaSeat.SeatType.HandicappedLockedSeatType)
                    Seat.Type = CinemaSeat.SeatType.HandicappedAvailableSeatType;

                this.SetSeatIcon();

                if (SeatControlClicked != null)
                {
                    SeatControlClicked(this, new CinemaSeatArgs(Seat.Key, Seat.Type));
                }
            }
        }


    }
}
