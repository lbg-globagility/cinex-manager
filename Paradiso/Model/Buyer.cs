using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Paradiso.Model
{
    public class Buyer : INotifyPropertyChanged
    {
        private int intId = 1;
        private string strLastName = string.Empty;
        private string strFirstName = string.Empty;
        private string strMiddleInitial = string.Empty;
        private string strAddress = string.Empty;
        private string strMunicipality = string.Empty;
        private string strProvince = string.Empty;

        private string strTIN = string.Empty;
        private string strIDNum = string.Empty;

        private bool blnIsSCPWD = false;

        private bool blnIsCancelled = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public Buyer()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleInitial = string.Empty;
            Address = string.Empty;
            Municipality = string.Empty;
            Province = string.Empty;
            TIN = string.Empty;
            IDNum = string.Empty;
        }

        public Buyer(Buyer buyer)
        {
            Id = buyer.Id;
            LastName = buyer.LastName;
            FirstName = buyer.FirstName;
            MiddleInitial = buyer.MiddleInitial;
            Address = buyer.Address;
            Municipality = buyer.Municipality;
            Province = buyer.Province;
            TIN = buyer.TIN;
            IDNum = buyer.IDNum;
            IsSCPWD = buyer.IsSCPWD;
            IsCancelled = buyer.IsCancelled;
        }

        public int Id
        {
            get { return intId; }
            set
            {
                if (value != intId)
                {
                    intId = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        public string LastName
        {
            get { return strLastName; }
            set
            {
                if (value != strLastName)
                {
                    strLastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        public string FirstName
        {
            get { return strFirstName; }
            set
            {
                if (value != strFirstName)
                {
                    strFirstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string MiddleInitial
        {
            get { return strMiddleInitial; }
            set
            {
                if (value != strMiddleInitial)
                {
                    strMiddleInitial = value;
                    NotifyPropertyChanged("MiddleInitial");
                }
            }
        }

        public string Name
        {
            get 
            {
                StringBuilder strName = new StringBuilder();
                if (!String.IsNullOrEmpty(FirstName))
                    strName.Append(strFirstName);
                if (!String.IsNullOrEmpty(MiddleInitial))
                {
                    if (strName.Length > 0)
                        strName.Append(" ");
                    strName.Append(MiddleInitial);
                    if (!MiddleInitial.EndsWith("."))
                        strName.Append(".");
                }
                if (!String.IsNullOrEmpty(LastName))
                {
                    if (strName.Length > 0)
                        strName.Append(" ");
                    strName.Append(LastName);
                }

                return strName.ToString();;
            }
        }

        public string Address
        {
            get { return strAddress; }
            set
            {
                if (value != strAddress)
                {
                    strAddress = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }

        public string Municipality
        {
            get { return strMunicipality; }
            set
            {
                if (value != strMunicipality)
                {
                    strMunicipality = value;
                    NotifyPropertyChanged("Municipality");
                }
            }
        }

        public string Province
        {
            get { return strProvince; }
            set
            {
                if (value != strProvince)
                {
                    strProvince = value;
                    NotifyPropertyChanged("Province");
                }
            }
        }

        public string FullAddress
        {
            get
            {
                StringBuilder strFullAddress = new StringBuilder();
                if (!String.IsNullOrEmpty(Address))
                    strFullAddress.Append(Address);
                if (!String.IsNullOrEmpty(Municipality))
                {
                    if (strFullAddress.Length > 0)
                        strFullAddress.Append(" ");
                    strFullAddress.Append(Municipality);
                }
                if (!String.IsNullOrEmpty(Province))
                {
                    if (strFullAddress.Length > 0)
                        strFullAddress.Append(" ");
                    strFullAddress.Append(Province);
                }
                return strFullAddress.ToString();
            }
        }


        public string TIN
        {
            get { return strTIN; }
            set
            {
                if (value != strTIN)
                {
                    strTIN = value;
                    NotifyPropertyChanged("TIN");
                }
            }
        }

        public string IDNum
        {
            get { return strIDNum; }
            set
            {
                if (value != strIDNum)
                {
                    strIDNum = value;
                    NotifyPropertyChanged("IDNum");
                }
            }
        }

        public bool IsSCPWD
        {
            get { return blnIsSCPWD; }
            set
            {
                if (value != blnIsSCPWD)
                {
                    blnIsSCPWD = value;
                    NotifyPropertyChanged("IsSCPWD");
                }
            }
        }

        public bool IsCancelled
        {
            get { return blnIsCancelled; }
            set
            {
                if (value != blnIsCancelled)
                {
                    blnIsCancelled = value;
                    NotifyPropertyChanged("IsCancelled");
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(LastName) && String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(MiddleInitial) &&
                    String.IsNullOrEmpty(Address) && String.IsNullOrEmpty(Municipality) && String.IsNullOrEmpty(Province);
            }
        }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
