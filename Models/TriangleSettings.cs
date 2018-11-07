using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmFoundation.Wpf;

namespace Models
{
    public class TriangleSettings:ObservableObject
    {
        private bool isColor = false;
        private Color _pickedColor = Color.FromArgb(1,1,1);
        private Bitmap _pickedTriangleTexture = new Bitmap(@"C:\Users\mikew\source\repos\GKLab2\Models\Textures\normal_map.jpg");
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

        public Bitmap PickedTriangleTexture
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
