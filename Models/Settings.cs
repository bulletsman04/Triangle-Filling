using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmFoundation.Wpf;
using System.Numerics;
using System.Resources;
using Models.Properties;
using PixelMapSharp;

namespace Models
{
    public class Settings: ObservableObject
    {
        private int _lightAnimRadius = 30;
        private Color _lightColor = Color.FromArgb(255,255,255);
        private Point3D _lightPoint = new Point3D(300,-200,100);
        private PixelMap _normalMap;
        private PixelMap _heightMap;
        private Vector3 _normalVector = new Vector3(0, 0, 1);
        private Vector3 _heightVector = new Vector3(0, 0, 0);
        private int _mPhong = 50;
        private bool _isLightConst = true;
        private bool _isLightMouse = false;
        private bool _isLightAnimation = false;
        private bool _isNormalConst = false;
        private bool _isHeightConst = false;
        private bool _isPhong = false;
        private  double _lambertRate = 1;
        private double _phongRate = 0.2;

        private int width;
        private int height;
        public Vector3[,] NMap { get; set; } 


        public Settings()
        {
          
            _normalMap = PixelMap.SlowLoad(new Bitmap(Resources.normal_map));
            _heightMap = PixelMap.SlowLoad(new Bitmap(Resources.heightmap));
           
            TriangleSettingsList = new ObservableCollection<TriangleSettings>();
            
        }


        public void CalculateNMap()
        {
            Vector3 N;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (IsNormalConst)
                    {
                        N = NormalVector;
                    }
                    else
                    {
                        N = LibrariesConverters.ColorToNormalVector(NormalMap[i % NormalMap.Width, j % NormalMap.Height].Color);
                    }

                    N = Vector3.Normalize(N);

                    if (!IsHeightConst)
                    {

                        Vector3 TV = new Vector3(1, 0, -N.X / N.Z);
                        Vector3 BV = new Vector3(0, 1, -N.Y / N.Z);
                        float dX = HeightMap[(i + 1) % HeightMap.Width, j % HeightMap.Height].Color.R - HeightMap[i % HeightMap.Width, j % HeightMap.Height].Color.R;
                        float dY = HeightMap[i % HeightMap.Width, (j + 1) % HeightMap.Height].Color.R - HeightMap[i % HeightMap.Width, j % HeightMap.Height].Color.R;
                        Vector3 D = TV * dX + BV * dY;

                        N += D / (float)180;
                        N = Vector3.Normalize(N);
                    }

                    NMap[i, j] = N;
                }
            }
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

        public PixelMap NormalMap
        {
            get { return _normalMap; }
            set
            {
                _normalMap = value;
                RaisePropertyChanged("NormalMap");
            }
        }

        public PixelMap HeightMap
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

        public bool IsLightMouse
        {
            get { return _isLightMouse; }
            set
            {
                _isLightMouse = value;
                RaisePropertyChanged("IsLightMouse");
            }
        }

        public bool IsLightAnimation
        {
            get { return _isLightAnimation; }
            set
            {
                _isLightAnimation = value;
                RaisePropertyChanged("IsLightAnimation");
            }
        }

        public int LightAnimRadius
        {
            get { return _lightAnimRadius; }
            set
            {
                _lightAnimRadius = value;
                RaisePropertyChanged("LightAnimRadius");
            }
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public double LambertRate
        {
            get { return _lambertRate; }
            set
            {
                _lambertRate = value;
                RaisePropertyChanged("LambertRate");
            }
        }

        public double PhongRate
        {
            get { return _phongRate; }
            set
            {
                _phongRate = value;
                RaisePropertyChanged("PhongRate");
            }
        }

        public void RaiseEventListAdd()
        {
          RaisePropertyChanged("TriangleSettingsList");
        }

        public void RaiseLightPointChanged()
        {
            RaisePropertyChanged("LightPoint");
        }
    }
}
