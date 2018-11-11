﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Models;
using MvvmFoundation.Wpf;
using Point = System.Windows.Point;
using Point3D = Models.Point3D;

//==using Point3D = Models.Point3D;

namespace ViewModels
{
    public class CanvasViewModel: ObservableObject
    {
        private WorkingArea _workingArea;
        private Settings _settings;
        private Point _mousePoint;
        private Triangle _movedTriangle;
        public  Image image { get; set; }
        public WorkingArea WorkingArea
        {
            get => _workingArea;
            set
            {
                _workingArea = value;
                RaisePropertyChanged("WorkingArea");
            } }
        public Vertex MovedVertex { get; set; }

        public Settings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public CanvasViewModel(Settings settings)
        {
            Settings = settings;
        }

        public void HandleMouseDown(Point mousePoint)
        {
            MovedVertex = WorkingArea.CheckForClickedVertex(mousePoint);
            if (MovedVertex == null)
            {
                _movedTriangle = WorkingArea.CheckForClickedPolygon(mousePoint);
                _mousePoint = mousePoint;
            }
            // zapamiętać punkt myszy i w mousemove przesuwać o wektor punkty a punkt myszy aktualizować
        }
        public void HandleMouseMove(Point mousePoint)
        {
            
            if (MovedVertex != null)
            {
                MovedVertex.X = (int)mousePoint.X;
                MovedVertex.Y = (int) mousePoint.Y;
                WorkingArea.RepaintBitmap();

            }
            else if (_movedTriangle != null)
            {
                Vector2D vector = new Vector2D(_mousePoint,mousePoint);
                WorkingArea.MoveTriangle(_movedTriangle,vector);
                _mousePoint = mousePoint;
                WorkingArea.RepaintBitmap();
            }
            else if(Settings.IsLightMouse == true)
            {
                _settings.LightPoint = new Point3D((int)mousePoint.X, (int)-mousePoint.Y, 300);
                WorkingArea.RepaintBitmap();

            }

        }

        public void HandleMouseUp()
        {
            MovedVertex = null;
            _movedTriangle = null;
        }


    }
}
