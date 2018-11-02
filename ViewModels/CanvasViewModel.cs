using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Models;
using MvvmFoundation.Wpf;

namespace ViewModels
{
    public class CanvasViewModel: ObservableObject
    {
        private WorkingArea _workingArea;
        private Point _mousePoint;
        private Triangle _movedTriangle;
        public WorkingArea WorkingArea
        {
            get => _workingArea;
            set
            {
                _workingArea = value;
                RaisePropertyChanged("WorkingArea");
            } }
        public Vertex MovedVertex { get; set; }

        public CanvasViewModel()
        {
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
        }

        public void HandleMouseUp()
        {
            MovedVertex = null;
            _movedTriangle = null;
        }


    }
}
