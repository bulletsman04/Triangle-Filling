using System;
using System.Globalization;
using System.Windows.Data;

namespace Views.Converters
{
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value != null && !(bool) value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value != null && !(bool) value;
        }
    }
}
