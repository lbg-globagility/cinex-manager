using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Paradiso
{
    public class FooterLineConverter: IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                double width = (double)value;
                width -= 170; //fixed width - WPFConverters not working properly compile problems
                if (width < 0)
                    width = 0;
                return width;
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
