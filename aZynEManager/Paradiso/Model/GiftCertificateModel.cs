using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class GiftCertificateModel : INotifyPropertyChanged
    {
        private int intId;
        private string strName;
        private decimal decAmount;
        private bool blnIsExpirable;
        private DateTime dtExpireDate;

        public GiftCertificateModel()
        {
        }

        public GiftCertificateModel(int id, string name, decimal amount, bool isExpirable, DateTime expireDate)
        {
            Id = id;
            Name = name;
            Amount = amount;
            HasExpiration = isExpirable;
            ExpirationDate = expireDate;
        }

        public int Id
        {
            get { return intId; }
            set
            {
                if (value != intId)
                {
                    intId = value;
                    this.NotifyPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return strName; }
            set
            {
                if (value != strName)
                {
                    strName = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public decimal Amount
        {
            get { return decAmount; }
            set
            {
                if (value != decAmount)
                {
                    decAmount = value;
                    this.NotifyPropertyChanged("Amount");
                }
            }
        }


        public bool HasExpiration
        {
            get { return blnIsExpirable; }
            set
            {
                if (value != blnIsExpirable)
                {
                    blnIsExpirable = value;
                    this.NotifyPropertyChanged("HasExpiration");
                }
            }
        }

        public DateTime ExpirationDate
        {
            get { return dtExpireDate; }
            set
            {
                if (value != dtExpireDate)
                {
                    dtExpireDate = value;
                    this.NotifyPropertyChanged("ExpirationDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
