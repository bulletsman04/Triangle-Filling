using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Models.Properties;
using MvvmFoundation.Wpf;
using PixelMapSharp;

namespace Models
{
    public class TriangleSettings: ObservableObject
    {
        private bool _isColor = false;
        private Vector3 _pickedColor = new Vector3(0,0,0);
        private Vector3[,] _pickedTriangleTexture = LibrariesConverters.BitmapToVectors(Resources.LavaCrackedHeavy001_sphere);
        public bool IsColor
        {
            get => _isColor;
            set
            {
                _isColor = value;
                RaisePropertyChanged("IsColor");
            }
        }

        public Vector3 PickedColor
        {
            get => _pickedColor;
            set
            {
                _pickedColor = value;
                IsColor = true;
                RaisePropertyChanged("PickedColor");
            }
        }

        public Vector3[,] PickedTriangleTexture
        {
            get => _pickedTriangleTexture;
            set
            {
                _pickedTriangleTexture = value;
                IsColor = false;
                RaisePropertyChanged("PickedTriangleTexture");
            }
        }
    }
}
