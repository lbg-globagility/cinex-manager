using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace CinemaCustomControlLibrary.Converters
{
    public class IntegerToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int ivalue = (int)value;
                if (ivalue == 0)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
