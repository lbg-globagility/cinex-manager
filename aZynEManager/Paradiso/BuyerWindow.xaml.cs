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
            //validate first
            if (BuyerInfo.IsSCPWD) //name and idnum is required
            {
                if (BuyerInfo.Name.Trim() == string.Empty)
                {
                    System.Windows.MessageBox.Show("Name is required.", string.Empty);
                    return;
                }
                else if (BuyerInfo.IDNum.Trim() == string.Empty)
                {
                    System.Windows.MessageBox.Show("ID Number is required..", string.Empty);
                    return;
                }

            }
            BuyerInfo.IsCancelled = false;
            this.Close();
        }



        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (BuyerInfo.IsSCPWD)
            {
                IDNumLabel.Visibility = Visibility.Visible;
                IDNum.Visibility = Visibility.Visible;
                Cancel.Visibility = Visibility.Hidden;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BuyerInfo.IsSCPWD && BuyerInfo.IsCancelled)
            {
                if (BuyerInfo.Name.Trim() == string.Empty)
                {
                    e.Cancel = true;
                    System.Windows.MessageBox.Show("Name is required.", string.Empty);
                    return;
                }
                else if (BuyerInfo.IDNum.Trim() == string.Empty)
                {
                    e.Cancel = true;
                    System.Windows.MessageBox.Show("ID Number is required..", string.Empty);
                    return;
                }
                BuyerInfo.IsCancelled = false;
            }
        }
    }
}
