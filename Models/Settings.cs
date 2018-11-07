using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmFoundation.Wpf;
using System.Numerics;
using System.Windows.Media.Media3D;

namespace Models
{
    public class Settings: ObservableObject
    {
        private Color _lightColor = Color.FromArgb(255,255,255);
        private Point3D _lightPoint = new Point3D(300,-200,300);
        private Bitmap _normalMap = new Bitmap(@"C:\Users\mikew\source\repos\GKLab2\Models\Textures\brick_normalmap.png");
        private Bitmap _heightMap = new Bitmap(@"C:\Users\mikew\source\repos\GKLab2\Models\Textures\brick_heightmap.png");
        private Vector3 _normalVector = new Vector3(0, 0, 1);
        private Vector3 _heightVector = new Vector3(0, 0, 0);
        private int _mPhong = 50;
        private bool _isLightConst = true;
        private bool _isNormalConst = false;
        private bool _isHeightConst = true;
        private bool _isPhong = true;

        private int width;
        private int height;


        public Settings()
        {
            TriangleSettingsList = new ObservableCollection<TriangleSettings>();
            
        }

        public void ResizeBitmaps(int width, int height)
        {
            NormalMap = new Bitmap(NormalMap,width,height);
            HeightMap = new Bitmap(HeightMap,width,height);
            this.width = width;
            this.height = height;
        }


        public bool IsLightConst
        {
            get => _isLightConst;
            set
            {
                _isLightConst = value;
                RaisePropertyChanged("IsLightConst");
            }
        }

       
        public bool IsNormalConst
        {
            get => _isNormalConst;
            set
            {
                _isNormalConst = value;
                RaisePropertyChanged("IsNormalConst");
            }
        }

        public bool IsHeightConst
        {
            get => _isHeightConst;
            set
            {
                _isHeightConst = value;
                RaisePropertyChanged("IsHeightConst");
            }
        }

        public ObservableCollection<TriangleSettings> TriangleSettingsList { get; set; }

        public bool IsPhong
        {
            get { return _isPhong; }
            set
            {
                _isPhong = value;
                RaisePropertyChanged("IsPhong");
            }
        }

        public Vector3 NormalVector
        {
            get { return _normalVector; }
            set
            {
                _normalVector = value;
                RaisePropertyChanged("NormalVector");
            }
        }

        public Vector3 HeightVector
        {
            get { return _heightVector; }
            set
            {
                _heightVector = value;
                RaisePropertyChanged("HeightVector");

            }
        }

     

        public Color LightColor
        {
            get { return _lightColor; }
            set
            {
                _lightColor = value;
                RaisePropertyChanged("LightColor");

            }
        }

        public Point3D LightPoint
        {
            get { return _lightPoint; }
            set
            {
                _lightPoint = value;
                RaisePropertyChanged("LightPoint");
            }
        }

        public Bitmap NormalMap
        {
            get { return _normalMap; }
            set
            {
                _normalMap = value;
                RaisePropertyChanged("NormalMap");
            }
        }

        public Bitmap HeightMap
        {
            get { return _heightMap; }
            set
            {
                _heightMap = value;
                RaisePropertyChanged("HeightMap");
            }
        }

        public int MPhong
        {
            get { return _mPhong; }
            set
            {
                _mPhong = value;
                RaisePropertyChanged("MPhong");
            }
        }


        public void RaiseEventListAdd()
        {
          RaisePropertyChanged("TriangleSettingsList");
        }
    }
}
