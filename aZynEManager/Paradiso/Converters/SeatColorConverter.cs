using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace Paradiso
{
    public class SeatColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            int intColor = 0;
            if (value is int)
            {
                intColor = (int) value;
            }

            byte[] bytes = BitConverter.GetBytes(intColor);
            return new SolidColorBrush(Color.FromRgb(bytes[2], bytes[1], bytes[0]));
            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
