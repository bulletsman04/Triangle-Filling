using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
