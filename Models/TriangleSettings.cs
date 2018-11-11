using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Properties;
using MvvmFoundation.Wpf;
using PixelMapSharp;

namespace Models
{
    public class TriangleSettings:ObservableObject
    {
        private bool isColor = false;
        private Color _pickedColor = Color.FromArgb(1,1,1);
        private PixelMap _pickedTriangleTexture = PixelMap.SlowLoad(new Bitmap(Resources.normal_map));
        public bool IsColor
        {
            get { return isColor; }
            set
            {
                isColor = value;
                RaisePropertyChanged("IsColor");
            }
        }

        public Color PickedColor
        {
            get => _pickedColor;
            set
            {
                _pickedColor = value;
                RaisePropertyChanged("PickedColor");
            }
        }

        public PixelMap PickedTriangleTexture
        {
            get => _pickedTriangleTexture;
            set
            {
                _pickedTriangleTexture = value;
                RaisePropertyChanged("PickedTriangleTexture");
            }
        }
    }
}
