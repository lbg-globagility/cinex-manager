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
        public enum SeatType
        {
            NormalSeat,
            HandicappedSeat
        }

        public SeatControl()
        {
            InitializeComponent();
        }

        public string SeatName
        {
            set
            {
                SeatNameLabel.Text = value;
                if (SeatNameLabel.Text.Length == 3)
                    Canvas.SetLeft(SeatNameLabel, 3);
                else if (SeatNameLabel.Text.Length == 2)
                    Canvas.SetLeft(SeatNameLabel, 7);
                else if (SeatNameLabel.Text.Length == 1)
                    Canvas.SetLeft(SeatNameLabel, 12);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SeatIcon.Source = new BitmapImage(new Uri(@"/Cinemapps;component/Images/seat.png", UriKind.Relative));
        }
    }
}
