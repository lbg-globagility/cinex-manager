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
            //checks if buyer info is empty
            if (BuyerInfo.IsEmpty)
            {
                BuyerInfo.Id = 1; //empty
            }
            else if (BuyerInfo.Id == 1)
            {
                //save new
                BuyerInfo.Id = 0; 
                //retrieve new id
                using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {
                    buyer _b = new buyer()
                    {
                        lastname = BuyerInfo.LastName,
                        firstname = BuyerInfo.FirstName,
                        middleinitial = BuyerInfo.MiddleInitial,
                        address = BuyerInfo.Address,
                        municipality = BuyerInfo.Municipality,
                        province = BuyerInfo.Province,
                        tin = BuyerInfo.TIN,
                        idnum = BuyerInfo.IDNum,
                        isscpwd = BuyerInfo.IsSCPWD
                    };

                    context.buyers.AddObject(_b);

                    context.SaveChanges();
                    BuyerInfo.Id = _b.id;
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            BuyerInfo.Id = 1;
            BuyerInfo.LastName = string.Empty;
            BuyerInfo.FirstName = string.Empty;
            BuyerInfo.MiddleInitial = string.Empty;
            BuyerInfo.Address = string.Empty;
            BuyerInfo.Municipality = string.Empty;
            BuyerInfo.Province = string.Empty;
            BuyerInfo.TIN = string.Empty;
            BuyerInfo.IDNum = string.Empty;
            EnableBuyerEditing(true);
        }

        private void EnableBuyerEditing(bool blnIsEnabled)
        {
            LastName.IsEnabled = blnIsEnabled;
            FirstName.IsEnabled = blnIsEnabled;
            MiddleInitial.IsEnabled = blnIsEnabled;
            Address.IsEnabled = blnIsEnabled;
            Municipality.IsEnabled = blnIsEnabled;
            Province.IsEnabled = blnIsEnabled;
            TIN.IsEnabled = blnIsEnabled;
            IDNum.IsEnabled = blnIsEnabled;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            using (BuyerSearchWindow searchWindow = new BuyerSearchWindow())
            {

                var window = Window.GetWindow(this);

                if (window != null)
                    searchWindow.Owner = window;
                searchWindow.ShowDialog();
                if (!searchWindow.IsCancelled)
                {
                    if (searchWindow.SelectedBuyer != null)
                    {
                        BuyerInfo.Id = searchWindow.SelectedBuyer.Id;
                        BuyerInfo.LastName = searchWindow.SelectedBuyer.LastName;
                        BuyerInfo.FirstName = searchWindow.SelectedBuyer.FirstName;
                        BuyerInfo.MiddleInitial = searchWindow.SelectedBuyer.MiddleInitial;
                        BuyerInfo.Address = searchWindow.SelectedBuyer.Address;
                        BuyerInfo.Municipality = searchWindow.SelectedBuyer.Municipality;
                        BuyerInfo.Province = searchWindow.SelectedBuyer.Province;
                        BuyerInfo.TIN = searchWindow.SelectedBuyer.TIN;
                        BuyerInfo.IDNum = searchWindow.SelectedBuyer.IDNum;
                        EnableBuyerEditing(false);
                    }
                }

                searchWindow.Close();
            }
        }
    }
}
