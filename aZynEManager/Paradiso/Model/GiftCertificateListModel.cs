using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Paradiso.Model
{
    public class GiftCertificateListModel : INotifyPropertyChanged
    {
        public ObservableCollection<GiftCertificateModel> GiftCertificates { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public GiftCertificateListModel()
        {
            GiftCertificates = new ObservableCollection<GiftCertificateModel>();
            GiftCertificates.CollectionChanged += (sender, e) => NotifyPropertyChanged("Total");
        }

        public void AddGiftCertificate(GiftCertificateModel giftCertificate)
        {

            //checks if exists
            
            //checks if already used or expired

            //checks expiration date
           
            //checks if already in list

            GiftCertificates.Add(giftCertificate);

        }

        public void RemoveGiftCertificate(string strGiftCertificateName)
        {
            if (GiftCertificates.Count > 0)
            {
                for (int j = GiftCertificates.Count - 1; j >= 0; j--)
                {
                    if (GiftCertificates[j].Name == strGiftCertificateName)
                        GiftCertificates.RemoveAt(j);
                }
            }
        }

        public void Dispose()
        {
            GiftCertificates.Clear();
            try
            {
                GiftCertificates.CollectionChanged -= (sender, e) => NotifyPropertyChanged("Total");
            }
            catch { }
        }

        public decimal Total
        {
            get
            {
                return GiftCertificates.Sum(x => x.Amount);
            }
        }


        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
