using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Drawing;

namespace Views.Converters
{
    class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            Color color = (Color)value;
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
           
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)value;
            return System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        }
    }
}
