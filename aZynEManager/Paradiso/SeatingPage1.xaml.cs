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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for SeatingPage1.xaml
    /// </summary>
    public partial class SeatingPage1 : Page
    {
        public MovieScheduleListItem MovieSchedule { get; set; }
        public List<int> SelectedCinemaSeats { get; set; }

        public SeatingPage1(MovieScheduleListItem _movieScheduleListItem, List<int> selectedCinemaSeats)
        {
            InitializeComponent();

            MovieSchedule = _movieScheduleListItem;

            SelectedCinemaSeats = new List<int>();
            if (selectedCinemaSeats != null)
            {
                for (int i = 0; i < selectedCinemaSeats.Count; i++)
                    SelectedCinemaSeats.Add(selectedCinemaSeats[i]);
            }

            this.DataContext = this;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
