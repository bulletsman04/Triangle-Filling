﻿using System;
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
        private int _lightAnimRadius = 50;
        private Vector3 _lightColor = LibrariesConverters.ColorToVector(Color.SandyBrown);
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
        private bool _isNormalTexture = false;
        private bool _isNormalFunction = false;
        private bool _isHeightConst = false;
        private bool _isPhong = false;
        private  float _lambertRate = 1f;
        private float _phongRate = 0.2f;
        private float _heightRate = 0.02f;
        private int width;
        private int height;
        private int _fps;
        public Vector3[,] NMap { get; set; }

        public ObservableCollection<TriangleSettings> TriangleSettingsList { get; set; }


        public Settings()
        {
          
            _normalMap = PixelMap.SlowLoad(Resources.brick_normalmap1);
            _heightMap = PixelMap.SlowLoad(Resources.LavaCrackedHeavy001_DISP_1K_2x);
           
            TriangleSettingsList = new ObservableCollection<TriangleSettings>();
            
        }
        
        public void CalculateNMap()
        {
            Vector3 N;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    N = IsNormalConst ? NormalVector : IsNormalFunction ? CalculateNormalFromFunction(i,j) : LibrariesConverters.ColorToNormalVector(NormalMap[i % NormalMap.Width, j % NormalMap.Height].Color);

                    N = Vector3.Normalize(N);

                    if (!IsHeightConst)
                    {

                        Vector3 TV = new Vector3(1, 0, -N.X / N.Z);
                        Vector3 BV = new Vector3(0, 1, -N.Y / N.Z);
                        float dX = HeightMap[(i + 1) % HeightMap.Width, j % HeightMap.Height].Color.R - HeightMap[i % HeightMap.Width, j % HeightMap.Height].Color.R;
                        float dY = HeightMap[i % HeightMap.Width, (j + 1) % HeightMap.Height].Color.R - HeightMap[i % HeightMap.Width, j % HeightMap.Height].Color.R;
                        Vector3 D = TV * dX + BV * dY;

                        N += D * HeightRate;
                        N = Vector3.Normalize(N);
                    }
                    if(float.IsNaN(N.X) || float.IsNaN(N.Y) || float.IsNaN(N.Z) )
                        N = new Vector3(0,0,1);
                    NMap[i, j] = N;
                }
            }
        }

        private Vector3 CalculateNormalFromFunction(int x, int y)
        {
            Vector3 zx = new Vector3(1,0,(float)((2f / 3)*Math.Cos(x/60f)*Math.Cos(y / 40f)));
            Vector3 zy = new Vector3(0, 1, (float)(-(1f / 1) * Math.Sin(x / 60f) * Math.Sin(y / 40f)));
            Vector3 Nxy = Vector3.Cross(zx,zy);
            Nxy = new Vector3(Nxy.X,Nxy.Y,Math.Abs(Nxy.Z));
            return Nxy;
        }

        private Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            float x, y, z;
            x = v1.Y * v2.Z - v2.Y * v1.Z;
            y = (v1.X * v2.Z - v2.X * v1.Z) * -1;
            z = v1.X * v2.Y - v2.X * v1.Y;

            var rtnvector = new Vector3(x, y, z);
            return rtnvector;
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

     

        public Vector3 LightColor
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
                IsNormalConst = false;
                RaisePropertyChanged("NormalMap");
            }
        }

        public PixelMap HeightMap
        {
            get { return _heightMap; }
            set
            {
                _heightMap = value;
                IsHeightConst = false;
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

        public float LambertRate
        {
            get { return _lambertRate; }
            set
            {
                _lambertRate = value;
                RaisePropertyChanged("LambertRate");
            }
        }

        public float PhongRate
        {
            get { return _phongRate; }
            set
            {
                _phongRate = value;
                RaisePropertyChanged("PhongRate");
            }
        }

        public float HeightRate
        {
            get { return _heightRate; }
            set
            {
                _heightRate = value;
                RaisePropertyChanged("HeightRate");
            }
        }

        public int Fps
        {
            get { return _fps; }
            set
            {
                _fps = value;
                RaisePropertyChanged("Fps");
            }
        }

        public bool IsNormalFunction
        {
            get { return _isNormalFunction; }
            set
            {
                _isNormalFunction = value;
                RaisePropertyChanged("IsNormalFunction");
            }
        }

        public bool IsNormalTexture
        {
            get { return _isNormalTexture; }
            set
            {
                _isNormalTexture = value;
                RaisePropertyChanged("IsNormalTexture");
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
