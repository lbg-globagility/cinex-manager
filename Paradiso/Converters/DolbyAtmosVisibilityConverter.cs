using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows;

namespace Paradiso
{
    public class DolbyAtmosVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;
            if (value is int)
            {
                int _value = (int)value;
                if (_value != 1 && _value != 2)
                    visibility = Visibility.Collapsed;
            }
            else
            {
                visibility = Visibility.Collapsed;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
