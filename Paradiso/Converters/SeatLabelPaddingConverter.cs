using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class SeatLabelPaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int intHeight = (int)value;
                if (intHeight == 24)
                {
                    return "0,3,0,0";
                }
            }

            return "0,7,0,0";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
