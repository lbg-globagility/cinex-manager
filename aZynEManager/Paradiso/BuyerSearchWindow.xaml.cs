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
    /// Interaction logic for BuyerSearchWindow.xaml
    /// </summary>
    public partial class BuyerSearchWindow : MetroWindow, IDisposable
    {
        private Buyer buyer;
        private bool blnIsCancelled = true;

        public ObservableCollection<Buyer> Buyers { get; set; }

        public BuyerSearchWindow()
        {
            InitializeComponent();

            buyer = new Buyer() { Id = 1 };
            Buyers = new ObservableCollection<Buyer>();

            DataContext = this;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //checks if there are entries 
            if (FilterName.Text.Trim() != string.Empty || FilterAddress.Text.Trim() != string.Empty ||
                FilterTIN.Text.Trim() != string.Empty || FilterIDNum.Text.Trim() != string.Empty)
            {

                Buyers.Clear();
                using (var context = new paradisoEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                {

                    var results = (from b in context.buyers
                                   where 
                                       (FilterName.Text.Trim() != string.Empty && (
                                       b.lastname.Contains(FilterName.Text.Trim()) || 
                                       b.firstname.Contains(FilterName.Text.Trim()) ||
                                       b.middleinitial.Contains(FilterName.Text.Trim())))
                                       || 
                                       (FilterAddress.Text.Trim() != string.Empty && (
                                       b.address.Contains(FilterAddress.Text.Trim()) || b.municipality.Contains(FilterAddress.Text.Trim()) ||
                                       b.province.Contains(FilterAddress.Text.Trim())))
                                       || 
                                       (FilterTIN.Text.Trim() != string.Empty && b.tin.Contains(FilterTIN.Text.Trim())) || 
                                       (FilterIDNum.Text.Trim() != string.Empty && b.idnum.Contains(FilterIDNum.Text.Trim()))
                                   select b).ToList();
                    if (results != null && results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            Buyers.Add(new Buyer()
                            {
                                Id = result.id,
                                LastName = result.lastname,
                                FirstName = result.firstname,
                                MiddleInitial = result.middleinitial,
                                Address = result.address,
                                Municipality = result.municipality,
                                Province = result.province,
                                TIN = result.tin,
                                IDNum = result.idnum
                            });
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Search filter is required.");
            }
        }

        public Buyer SelectedBuyer
        {
            get { return buyer; }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            blnIsCancelled = true;
            this.Close();
        }

        public bool IsCancelled
        {
            get { return blnIsCancelled; }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                buyer = (Buyer)item;
                blnIsCancelled = false;
                this.Close();
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (Buyers.Count > 0)
            Buyers.Clear();
        }

        #endregion
    }
}
