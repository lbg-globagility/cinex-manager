using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using Paradiso.Model;

namespace Paradiso
{
    public class TotalPatronSeatConverter:IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, CultureInfo culture)
        {
            decimal decTotal = 0;
            if (value is ObservableCollection<PatronSeatModel>)
            {
                ObservableCollection<PatronSeatModel> patronSeats = (ObservableCollection<PatronSeatModel>)value;
                if (patronSeats != null && patronSeats.Count > 0)
                {
                    foreach (PatronSeatModel psm in patronSeats)
                    {
                        decTotal += psm.Price;
                    }
                }
            }

            return string.Format("PHP {0:#,##0.00}", decTotal);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
