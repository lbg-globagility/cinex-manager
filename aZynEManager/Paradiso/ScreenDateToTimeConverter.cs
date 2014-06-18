using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class ScreenDateToTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime screenDate = (DateTime)value;
                if (screenDate == DateTime.MinValue)
                    return string.Empty;
                else
                    return string.Format("{0:HH:mm}", screenDate);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
