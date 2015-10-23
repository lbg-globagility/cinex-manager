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
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>
    public partial class BuyerWindow : MetroWindow
    {
        public Buyer BuyerInfo { get; set; }

        public BuyerWindow()
        {
            InitializeComponent();

            BuyerInfo = new Buyer();

            DataContext = this;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            BuyerInfo.IsCancelled = true;
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            BuyerInfo.IsCancelled = false;
            //validate first
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (BuyerInfo.IsSCPWD)
            {
                IDNumLabel.Visibility = Visibility.Visible;
                IDNum.Visibility = Visibility.Visible;
            }
        }
    }
}
