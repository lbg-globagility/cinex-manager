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
using MahApps.Metro.Controls;

namespace Cinemapps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MoviesTile_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ReportsTile_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Owner = this;
            reportWindow.Show();
        }

        private void SetupTile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdministrationTile_Click(object sender, RoutedEventArgs e)
        {
            SeatWindow seatWindow = new SeatWindow();
            seatWindow.LoadCinema(2);
            seatWindow.Owner = this;
            seatWindow.Show();
        }
    }
}
