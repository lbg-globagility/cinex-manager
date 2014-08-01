using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;


namespace Paradiso
{
    public class ScreenTimeColorMultiConverter: IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool blnIsEnabled = (bool)values[0];
            int intKey = (int)values[1];
            int intCurrentKey = 0;
            int.TryParse(values[2].ToString(), out intCurrentKey);
            //check if current selection, binding not working properly
            
            if (!blnIsEnabled)
                return new SolidColorBrush(Colors.LightGray);
            else if (intKey == intCurrentKey)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.DarkGray);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
