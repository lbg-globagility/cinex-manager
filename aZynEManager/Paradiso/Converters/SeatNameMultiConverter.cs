using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class SeatNameMultiConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string strName = (string) values[0];
            int intType = 0;
            if (values[1] is int)
                intType = (int) values[1];
            bool blnIsHandicapped = false;
            if (values[2] is bool)
                blnIsHandicapped = (bool)values[2];
            bool blnIsDisabled = false;
            if (values[3] is bool)
                blnIsDisabled = (bool)values[3];
            if (intType == 3 || blnIsHandicapped || blnIsDisabled)
                return string.Empty;
            else
                return strName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
