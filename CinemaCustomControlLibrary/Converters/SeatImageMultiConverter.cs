using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace CinemaCustomControlLibrary.Converters
{
    public class SeatImageMultiConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int intType = (int)values[0];
            bool blnIsHandicapped = false;
            if (values[1] is bool)
                blnIsHandicapped = (bool)values[1];

            string strUri = string.Empty;
            if (intType == 2)
                strUri = @"/CinemaCustomControlLibrary;component/Images/screen.png";
            else if (intType == 1 && blnIsHandicapped)
                strUri = @"/CinemaCustomControlLibrary;component/Images/seat-disabled.png";
            else if (intType == 1)
                strUri = @"/CinemaCustomControlLibrary;component/Images/seat-white.png";

            return new BitmapImage(new Uri(strUri, UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
