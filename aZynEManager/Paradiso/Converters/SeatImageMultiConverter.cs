using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Paradiso
{
    public class SeatImageMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int intType = (int)values[0];
            int intSeatType = (int)values[1];
            string strUri = string.Empty;

            if (intType == 2)
                strUri = @"/Paradiso;component/Images/screen.png";
            else if (intSeatType == 1)
                strUri = @"/Paradiso;component/Images/seat-green-r.png";
            else if (intSeatType == 2)
                strUri = string.Empty; //@"/Paradiso;component/Images/seat-yellow-r.png";
            else 
                strUri = @"/Paradiso;component/Images/seat-red-r.png";

            return new BitmapImage(new Uri(strUri, UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
