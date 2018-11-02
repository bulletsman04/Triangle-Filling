using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Models
{
    public class Vector2D
    {
        private Point _startPoint;
        private Point _endPoint;
        public (int X,int Y) Coordinates { get; }

        public Vector2D(Point startPoint, Point endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            Coordinates = ((int)(endPoint.X - startPoint.X), (int)(endPoint.Y - startPoint.Y));
        }
    }
}
