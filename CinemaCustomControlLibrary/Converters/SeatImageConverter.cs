using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace CinemaCustomControlLibrary.Converters
{
    public class SeatImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            string strUri = string.Empty;
            if (value is int)
            {
                int intType = (int)value;

                if (intType == 2)
                    strUri = @"/CinemaCustomControlLibrary;component/Images/screen.png";
                else if (intType == 1)
                    strUri = @"/CinemaCustomControlLibrary;component/Images/seat-red-r.png";
            }
            
            return new BitmapImage(new Uri(strUri, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
