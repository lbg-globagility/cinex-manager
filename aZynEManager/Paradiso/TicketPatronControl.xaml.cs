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
using Xceed.Wpf.Toolkit;

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for TicketPatronControl.xaml
    /// </summary>
    public partial class TicketPatronControl : UserControl
    {
        public event EventHandler TicketPatronClicked; 
        public CinemaTicket Ticket { get; set; }
        public int m_intPrevPatronKey;

        public TicketPatronControl(CinemaTicket cinemaTicket)
        {
            InitializeComponent();

            Ticket = new CinemaTicket(cinemaTicket);
            this.DataContext = this;
        }

        private void PatronComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatronComboBox.SelectedIndex == -1)
            {
                Ticket.PatronKey = 0;
                Ticket.MinQuantity = 1;
                QtyIntegerUpDown.IsEnabled = false;
            }
            else
            {
                QtyIntegerUpDown.IsEnabled = true;
                //QtyIntegerUpDown.Minimum = 0;
                //prevent selected value as minimum
                if (QtyIntegerUpDown.Value == null || QtyIntegerUpDown.Value == 0)
                {
                    //QtyIntegerUpDown.Value = 1;
                    Ticket.Quantity = 1;
                    //update price
                }
                
                CinemaPatron cinemaPatron = (CinemaPatron)PatronComboBox.SelectedItem;
                if (cinemaPatron != null)
                {
                    Ticket.PatronKey = cinemaPatron.Key;
                    Ticket.Price = cinemaPatron.Price * Ticket.Quantity;
                }
                
            }

            if (TicketPatronClicked != null)
                TicketPatronClicked(this, new TicketPatronArgs(Ticket, m_intPrevPatronKey));

            m_intPrevPatronKey = Ticket.PatronKey;
        }

        private void QtyIntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Ticket.Price = 0.0M;
            if (PatronComboBox.SelectedItem != null)
            {
                CinemaPatron cinemaPatron = (CinemaPatron)PatronComboBox.SelectedItem;
                Ticket.Price = cinemaPatron.Price * Ticket.Quantity;
            }
            if (TicketPatronClicked != null)
                TicketPatronClicked(this, new TicketPatronArgs(Ticket, -1));
            
        }
    }
}
