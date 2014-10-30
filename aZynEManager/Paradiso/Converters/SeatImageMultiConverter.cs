using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.IO;

namespace Paradiso
{
    public class SeatImageMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int intType = (int)values[0];
            int intSeatType = (int)values[1];
            int intColor = 0;
            if (values.Count() >= 3 && values[2] is int)
                intColor = (int)values[2];

            string strUri = string.Empty;

            if (intType == 2)
                strUri = @"/Paradiso;component/Images/screen.png";
            else if (intSeatType == 1)
                strUri = @"/Paradiso;component/Images/seat-green-r.png";
            else if (intSeatType == 2)
            {
                byte[] bytes = BitConverter.GetBytes(intColor);

                strUri = @"/Paradiso;component/Images/seat-red-r.png";

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(System.Windows.Application.GetResourceStream(
                    new Uri("Images/seat-red-r.png", UriKind.Relative)).Stream);

                if (bmp != null)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            //int argb = bmp.GetPixel(x, y).ToArgb();
                            if ((bmp.GetPixel(x, y).R == 192 && bmp.GetPixel(x, y).G == 25 && bmp.GetPixel(x, y).B == 0))
                            {
                                
                                bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(bytes[2], bytes[1], bytes[0]));
                            }
                        }
                    }

                    BitmapSource i = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                   bmp.GetHbitmap(),
                                   IntPtr.Zero,
                                   System.Windows.Int32Rect.Empty,
                                   BitmapSizeOptions.FromEmptyOptions());

                    return i;
                }
            }
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
