using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class SeatLabelForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string strValue = (string)value;
                int index = strValue.IndexOf("|");
                int intType = 1;
                string strName = string.Empty;
                if (index != -1)
                {
                    int.TryParse(strValue.Substring(0, index), out intType);
                    strName = strValue.Substring(index + 1);
                }

                if (intType == 3)
                {
                    if (strName.StartsWith("SECTION"))
                        return "White";
                    else
                        return "Yellow";
                }
            }

            return "Black";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
