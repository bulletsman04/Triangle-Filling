using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Models;
using PixelMapSharp;

namespace Views.Converters
{
    public class TriangleTextureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            Vector3[,] vectorMap = (Vector3[,])value;
            DirectBitmap bitmap = new DirectBitmap(vectorMap.GetLength(0),vectorMap.GetLength(1));
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Vector3 color = vectorMap[i, j];
                    bitmap.SetPixel(i,j,Color.FromArgb((byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255)));
                }
            }
            return LibrariesConverters.BitmapToImageSource(bitmap.Bitmap);

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
