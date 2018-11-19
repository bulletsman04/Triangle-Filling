using System;
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

        public Settings Settings { get; set; }

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
                Settings.LightPoint = new Point3D((int)mousePoint.X, (int)-mousePoint.Y, 100);
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
