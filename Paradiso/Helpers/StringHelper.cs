using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paradiso.Helpers
{
    public class StringHelper
    {
        public static string CenterString(int intWidth, string _strData)
        {
            string data = _strData;
            if (data.Length > intWidth)
                data = data.Substring(0, intWidth);
            else
                data = string.Format("{0}{1}", "".PadLeft((intWidth - data.Length) / 2, ' '), data);
            return data;
        }
    }
}
