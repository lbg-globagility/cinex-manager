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
using Paradiso.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Paradiso
{
    /// <summary>
    /// Interaction logic for GiftCertificateWindow.xaml
    /// </summary>
    public partial class GiftCertificateWindow : Window, IDisposable
    {
        public GiftCertificateModel GiftCertificate { get; set;}
        public ObservableCollection<GiftCertificateModel> GiftCertificates { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CashAmount { get; set; }

        public GiftCertificateWindow()
        {
            InitializeComponent();
            GiftCertificates = new ObservableCollection<GiftCertificateModel>();
        }

        private void AddGiftCertificate_Click(object sender, RoutedEventArgs e)
        {
            string strError = string.Empty;
            
            decimal decTotalGiftCertificateAmount = GiftCertificates.Sum(x => x.Amount);
            if (CashAmount + decTotalGiftCertificateAmount >= TotalAmount)
            {
                strError = "Total Amount exceeds Total Amount Due.";
            }
            else
            {

                string strGiftCertificateName = GiftCertificateName.Text.Trim();
                if (strGiftCertificateName == string.Empty)
                {
                    strError = "Missing Gift Certificate Name.";
                }
                else
                {
                    DateTime dtCurrentDate = ParadisoObjectManager.GetInstance().CurrentDate;
                    using (var context = new azynemaEntities(CommonLibrary.CommonUtility.EntityConnectionString("ParadisoModel")))
                    {
                        var gc = (from _gc in context.gift_certificate where _gc.name == strGiftCertificateName select _gc).FirstOrDefault();
                        if (gc == null)
                        {
                            strError = "Invalid Gift Certificate Name.";
                        }
                        else
                        {
                            if (GiftCertificates.Count > 0)
                            {
                                foreach (GiftCertificateModel __gc in GiftCertificates)
                                {
                                    if (__gc.Name == strGiftCertificateName)
                                    {
                                        strError = "Gift Certificate is already on the list.";
                                    }
                                }
                            }

                            if (strError == string.Empty)
                            {

                                if (gc.isexpired == 1)
                                {
                                    strError = "Gift Certificate is already used.";
                                }
                                else if (gc.expiration_date <= dtCurrentDate.Date)
                                {
                                    strError = "Gift Certificate is already expired.";
                                }
                                else
                                {
                                    DateTime dtExpirationDate = dtCurrentDate.AddYears(10);
                                    if (gc.expiration_date != null)
                                        dtExpirationDate = (DateTime)gc.expiration_date;
                                    decimal decAmount = 0;
                                    if (gc.amount != null)
                                        decAmount = (decimal)gc.amount;
                                    bool isexpired = false;
                                    if (gc.isexpired == 1)
                                        isexpired = true;

                                    GiftCertificate = new GiftCertificateModel(gc.id, gc.name, decAmount, isexpired, dtExpirationDate);
                                    this.Close();
                                }
                            }
                        }

                    }
                }
            }
            if (strError == string.Empty)
            {
                this.Close();
            }
            else
            {
                MessageWindow message = new MessageWindow();
                message.MessageText.Text = strError;
                message.ShowDialog();
            }
        }

        private void CancelGiftCertifcate_Click(object sender, RoutedEventArgs e)
        {
            GiftCertificate = null;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GiftCertificateName.Focus();
        }

        #region IDisposable Members

        public void Dispose()
        {
            GiftCertificates.Clear();
        }

        #endregion
    }
}
