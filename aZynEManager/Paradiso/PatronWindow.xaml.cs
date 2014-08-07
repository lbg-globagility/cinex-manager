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
    public partial class PatronWindow : MetroWindow
    {
        public SeatModel Seat { get; set; }
        public ObservableCollection<PatronModel> Patrons { get; set; }
        public PatronModel SelectedPatron { get; set; }

        public PatronWindow(SeatModel _seatModel, ObservableCollection<PatronModel> _patrons, PatronModel _selectedPatron)
        {
            InitializeComponent();

            Seat = _seatModel;
            Patrons = _patrons;
            SelectedPatron = _selectedPatron;

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
                        //TODO checks if seat is reserved 
                        
                        //var seat = (from s in context.cinem

                        //TODO save, delete, update reserved seat
                    }
                }

                this.Close();
            }
        }


    }
}
