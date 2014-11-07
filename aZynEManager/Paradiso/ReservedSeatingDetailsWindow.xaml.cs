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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for ReservedSeatingDetailsWindow.xaml
    /// </summary>
    public partial class ReservedSeatingDetailsWindow : MetroWindow
    {
        public ReservedSeatingDetails SeatingDetails { get; set; }

        public ReservedSeatingDetailsWindow()
        {
            InitializeComponent();

            SeatingDetails = new ReservedSeatingDetails();

            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SeatingDetails.IsCancelled = false;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SeatingDetails.IsCancelled = true;
            this.Close();
        }
    }
}
