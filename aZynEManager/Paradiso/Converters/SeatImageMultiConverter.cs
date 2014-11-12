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
            bool blnIsHandicapped = false;
            if (values.Count() >= 4 && values[3] is bool)
                blnIsHandicapped = (bool)values[3];

            string strUri = string.Empty;

            if (intType == 2)
            {
                strUri = @"/Paradiso;component/Images/screen.png";
            }
            else if (intSeatType == 1)  //free
            {
                if (blnIsHandicapped)
                    strUri = @"/Paradiso;component/Images/seat-disabled.png";
                else
                    strUri = @"/Paradiso;component/Images/seat-white.png";
            }
            else  //if (intSeatType == 2) //selected
            {
                byte[] bytes = BitConverter.GetBytes(intColor);

                string strImageUri = string.Empty;
                
                int r = 128;
                int g = 128;
                int b = 128;

                if (blnIsHandicapped)
                    strImageUri = @"Images/seat-gray-disabled.png";
                else
                    strImageUri = @"Images/seat-gray.png";

                /*
                if (intSeatType == 2)
                {
                    if (blnIsHandicapped)
                        strImageUri = @"Images/seat-gray-disabled.png";
                    else
                        strImageUri = @"Images/seat-gray.png";
                }
                else
                {
                    if (blnIsHandicapped)
                        strImageUri = @"Images/seat-gray-disabled-x.png";
                    else
                        strImageUri = @"Images/seat-gray-x.png";
                    
                }
                */

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(System.Windows.Application.GetResourceStream(
                    new Uri(strImageUri, UriKind.Relative)).Stream);

                strUri = @"/Paradiso;component/Images/seat-red-r.png";

                if (bmp != null)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            //int argb = bmp.GetPixel(x, y).ToArgb();
                            if ((bmp.GetPixel(x, y).R == r && bmp.GetPixel(x, y).G == g && bmp.GetPixel(x, y).B == b))
                            {
                                bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(bytes[0], bytes[1], bytes[2]));
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
            /*
            else //taken
            {
                strUri = @"/Paradiso;component/Images/seat-red-r.png";
            }
            */

            return new BitmapImage(new Uri(strUri, UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
