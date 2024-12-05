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

namespace Paradiso
{
    /// <summary>
    /// Interaction logic for MessageYesNoWindow.xaml
    /// </summary>
    public partial class MessageYesNoWindow : Window
    {
        public bool IsYes { get; set; }

        public MessageYesNoWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = true;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = false;
            this.Close();
        }
    }
}
