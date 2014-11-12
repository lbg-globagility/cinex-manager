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
            //00d3d7 becomes d7d300
            return new SolidColorBrush(Color.FromRgb(bytes[0], bytes[1], bytes[2]));
            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
