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
            bool blnIsEnabled = false;
            int intKey = 0;
            int intCurrentKey = 0;
            try
            {
                blnIsEnabled = (bool)values[0];
            }
            catch { }
            try
            {
                intKey = (int)values[1];
            }
            catch { }
            try
            {
                int.TryParse(values[2].ToString(), out intCurrentKey);
            }
            catch { }
            //checks if current selection, binding not working properly

            
            if (!blnIsEnabled) //disabled
                return new SolidColorBrush(Colors.LightGray);
            else if (intKey == intCurrentKey) //selected
                return new SolidColorBrush(Colors.Green);
            else //enabled
                return new SolidColorBrush(Colors.DarkGray);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
