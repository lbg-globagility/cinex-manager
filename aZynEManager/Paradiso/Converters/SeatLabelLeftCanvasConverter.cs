using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class SeatLabelLeftCanvasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string strName = (string)value;

                if (strName.Length == 3)
                    return 3;
                else if (strName.Length == 2)
                    return 7;
                else if (strName.Length == 1)
                    return 12;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
