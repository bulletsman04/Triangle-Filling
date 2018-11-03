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
        private bool isColor = true;
        private System.Windows.Media.Color _pickedColor = System.Windows.Media.Color.FromRgb(1,1,1);
        private Bitmap _pickedTriangleTexture = new Bitmap(@"C:\Users\mikew\source\repos\GKLab2\Views\Icons\maximize.png");
        public bool IsColor
        {
            get { return isColor; }
            set
            {
                isColor = value;
                RaisePropertyChanged("IsColor");
            }
        }

        public System.Windows.Media.Color PickedColor
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
