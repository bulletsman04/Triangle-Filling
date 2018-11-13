using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MvvmFoundation.Wpf;
using PixelMapSharp;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Windows.Point;

namespace Models
{
    public class WorkingArea: ObservableObject
    {
        private int _width;
        private int _height;
        private ImageSource _imageSource;
        public Triangle Triangle1 { get; set; }
        public Triangle Triangle2 { get; set; }
        public Settings Settings { get; set; }
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value; 
                RaisePropertyChanged("ImageSource");
            } }

        public WorkingArea(int width, int height, Settings settings)
        {
            _width = width;
            _height = height;
            Settings = settings;
            Settings.Width = width;
            Settings.Height = height;
            Settings.NMap = new Vector3[width,height];
            Settings.CalculateNMap();
            InitializeTriangles();
            RepaintBitmap();
        }

        private void InitializeTriangles()
        {
            Triangle1 = new Triangle();
            Triangle2 = new Triangle();
            TriangleSettings settings1 = new TriangleSettings();
            Triangle1.TriangleSettings = settings1;
            Settings.TriangleSettingsList.Add(settings1);
            int x1, y1;
            x1 = _width / 20 ;
            y1 = 20;
            Vertex v1 = new Vertex(x1,y1);
            Vertex v2 = new Vertex(_width - x1, y1);
            Vertex v3 = new Vertex(x1, 20*y1);
            Edge e1 = new Edge(v1,v2);
            Edge e2 = new Edge(v2,v3);
            Edge e3 = new Edge(v3, v1);
            Triangle1.Vertices.AddRange(new []{v1,v2,v3});
            Triangle1.Edges.AddRange(new []{e1,e2,e3});
            Triangle1.V1StartPoint = new Point(x1,y1);
            Triangle1.MoveVector = new Vector2D(Triangle1.V1StartPoint, Triangle1.V1StartPoint);

            TriangleSettings settings2 = new TriangleSettings();
            settings2.PickedColor = LibrariesConverters.ColorToVector(Color.Firebrick);
            Triangle2.TriangleSettings = settings2;
            Settings.TriangleSettingsList.Add(settings2);
           

            Vertex v4 = new Vertex(_width - x1, 20 * y1 + 15);
            Vertex v5 = new Vertex(_width - x1, y1 + 15);
            Vertex v6 = new Vertex(x1 ,20*y1 + 15);
            Edge e4 = new Edge(v4, v5);
            Edge e5 = new Edge(v5, v6);
            Edge e6 = new Edge(v6, v4);
            Triangle2.Vertices.AddRange(new[] { v4, v5, v6 });
            Triangle2.Edges.AddRange(new[] { e4, e5, e6 });
            Triangle2.V1StartPoint = new Point(_width - x1, _height - y1);
            Triangle2.MoveVector = new Vector2D(Triangle2.V1StartPoint, Triangle2.V1StartPoint);

            Settings.RaiseEventListAdd();
        }

        

        public void RepaintBitmap()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DirectBitmap db = new DirectBitmap(_width, _height);
            Bitmap bitmap = db.Bitmap;

            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.Clear(Color.White);

                MyGraphics mg = new MyGraphics(db, Settings);
                mg.FillPolygon(Triangle1);
                mg.FillPolygon(Triangle2);

                DrawPolygon(Triangle1, gr);
                DrawPolygon(Triangle2, gr);


                
                ImageSource = LibrariesConverters.BitmapToImageSource(bitmap);

                db.Dispose();
            }
            sw.Stop();
            int frames = (int) (1000 / sw.ElapsedMilliseconds);
            Settings.Fps = frames;

        }

        private void DrawPolygon(Triangle triangle, Graphics gr)
        {


            Pen pen = new Pen(Color.LightGray, 2);
            Brush brush = new SolidBrush(Color.FromArgb(89, 89, 89));
            
            foreach (var edge in triangle.Edges)
            {
                gr.DrawLine(pen, edge.Left.X, edge.Left.Y, edge.Right.X, edge.Right.Y);
            }

            foreach (Vertex v in triangle.Vertices)
            {
                gr.FillEllipse(brush, v.Rectangle);
            }

            brush.Dispose();
            pen.Dispose();

        }

        public Vertex CheckForClickedVertex(Point mousePoint)
        {
            foreach (var vertex in Triangle1.Vertices.Union(Triangle2.Vertices))
            {
                if (vertex.Rectangle.Contains((int)mousePoint.X,(int)mousePoint.Y))
                    return vertex;
            }
            return null;
        }

        public Triangle CheckForClickedPolygon(Point mousePoint)
        {
            foreach (var triangle in new[]{Triangle1,Triangle2})
            {
                if (triangle.PointInTriangle(new Point((int) mousePoint.X, (int) mousePoint.Y)))
                    return triangle;
            }
            return null;
        }

        public void MoveTriangle(Triangle triangle, Vector2D vector)
        {
            foreach (var vertex in triangle.Vertices)
            {
                vertex.X += vector.Coordinates.X;
                vertex.Y += vector.Coordinates.Y;
            }

            triangle.MoveVector = triangle.MoveVector + vector;
        }
    }
}
