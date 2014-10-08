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
    public class MovieScheduleListVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;
            if (value is bool)
            {
                bool blnIsVisible = (bool)value;
                if (!blnIsVisible)
                    visibility = Visibility.Hidden;
            }
            else if (value is int)
            {
                int intValue = (int)value;
                if (intValue == 0)
                    visibility = Visibility.Hidden;
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
