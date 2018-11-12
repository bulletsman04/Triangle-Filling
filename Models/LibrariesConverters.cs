using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models
{
    public static class LibrariesConverters
    {
        public static System.Drawing.Color ColorMediaConverter(System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.R,color.G,color.B);
        }

        public static System.Windows.Media.Color ColorDrawingConverter(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static Vector3 ColorToVector(Color color)
        {
            return new Vector3((float)color.R / 255, (float)color.G / 255, (float)color.B / 255);
        }

        public static Vector3 ColorToNormalVector(Color color)
        {
            return new Vector3(((float)color.R * 2 / 255 - 1), ((float)color.G * 2 / 255 - 1), (float)color.B / 255);
        }

        public static Vector3[,] BitmapToVectors(Bitmap bitmap)
        {
            Vector3[,] textureVectors = new Vector3[bitmap.Width,bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    textureVectors[i, j] = LibrariesConverters.ColorToVector(bitmap.GetPixel(i, j));
                }
            }

            return textureVectors;
        }
    }
}
