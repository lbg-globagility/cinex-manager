using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace Paradiso
{
    public class SeatColorMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int intType = (int)values[0];
            int intColor = 0;
            if (values[1] is int)
                intColor = (int) values[1];

            if (intType == 2)
                return Colors.Transparent;

            byte[] bytes = BitConverter.GetBytes(intColor);
            return new SolidColorBrush(Color.FromRgb(bytes[2], bytes[1], bytes[0]));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
