using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Models;
using PixelMapSharp;

namespace Views.Converters
{
    public class TextureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            PixelMap pixelMap = (PixelMap) value;
            return LibrariesConverters.BitmapToImageSource(pixelMap.GetBitmap());

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
