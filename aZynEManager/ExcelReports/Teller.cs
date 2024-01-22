using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelReports
{
    public class Teller
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }

        public Teller(int intUserId, string strUserCode, string strFullName, bool blnIsActive)
        {
            UserId = intUserId;
            UserCode = strUserCode;
            Fullname = strFullName;
            IsActive = blnIsActive;
        }

        public override string ToString()
        {
            if (IsActive)
                return UserCode;
            else
                return string.Format("{0} (Inactive)", UserCode);
        }
    }
}
