using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Drawing;
using System.Numerics;
using Models;

namespace Views.Converters
{
    class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            Vector3 color = (Vector3)value;
            return System.Windows.Media.Color.FromRgb((byte)(color.X*255), (byte)(color.Y * 255), (byte)(color.Z * 255));
           
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)value;
            return LibrariesConverters.ColorToVector(Color.FromArgb(color.R,color.G,color.B));
        }
    }
}
