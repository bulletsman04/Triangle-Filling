using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Models
{
    public class Triangle
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
        public TriangleSettings TriangleSettings { get; set; }
        public Triangle()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }

        double sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        public bool PointInTriangle(Point pt)
        {
            double d1, d2, d3;
            bool has_neg, has_pos;
            Point v1 = new Point(Vertices[0].X,Vertices[0].Y);
            Point v2 = new Point(Vertices[1].X, Vertices[1].Y);
            Point v3 = new Point(Vertices[2].X, Vertices[2].Y);


            d1 = sign(pt, v1, v2);
            d2 = sign(pt, v2, v3);
            d3 = sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

    }
}
