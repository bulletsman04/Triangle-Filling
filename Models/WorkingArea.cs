using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MvvmFoundation.Wpf;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Windows.Point;

namespace Models
{
    public class WorkingArea: ObservableObject
    {
        private ImageSource _imageSource;
        public Triangle Triangle1 { get; set; }
        public Triangle Triangle2 { get; set; }
        public Bitmap Bitmap { get; set; }
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
            Bitmap = new Bitmap(width,height);
            Settings = settings;
           // Settings.ResizeBitmaps(width,height);
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
            Settings.RaiseEventListAdd();
            int x1, y1;
            x1 = Bitmap.Width / 10;
            y1 = Bitmap.Height / 8;
            Vertex v1 = new Vertex(x1,y1);
            Vertex v2 = new Vertex(3*x1, 3*y1);
            Vertex v3 = new Vertex(5*x1, y1);
            Edge e1 = new Edge(v1,v2);
            Edge e2 = new Edge(v2,v3);
            Edge e3 = new Edge(v3, v1);
            Triangle1.Vertices.AddRange(new []{v1,v2,v3});
            Triangle1.Edges.AddRange(new []{e1,e2,e3});
            Triangle1.V1StartPoint = new Point(x1,y1);
            Triangle1.MoveVector = new Vector2D(Triangle1.V1StartPoint, Triangle1.V1StartPoint);

            Vertex v4 = new Vertex(Bitmap.Width - x1, Bitmap.Height- y1);
            Vertex v5 = new Vertex(Bitmap.Width - 3 * x1, Bitmap.Height - 3 * y1);
            Vertex v6 = new Vertex(Bitmap.Width - 5 * x1, Bitmap.Height - y1);
            Edge e4 = new Edge(v4, v5);
            Edge e5 = new Edge(v5, v6);
            Edge e6 = new Edge(v6, v4);
            Triangle2.Vertices.AddRange(new[] { v4, v5, v6 });
            Triangle2.Edges.AddRange(new[] { e4, e5, e6 });

        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                
                return bitmapimage;
            }
        }
        public void RepaintBitmap()
        {
            // Tu własne wypełnianie
            
            using (Graphics gr = Graphics.FromImage(Bitmap))
            {
                gr.Clear(Color.White);
                MyGraphics mg = new MyGraphics(Bitmap, Settings);
                mg.FillPolygon(Triangle1);
                DrawPolygon(Triangle1,gr);
                DrawPolygon(Triangle2,gr);
            }

            
            ImageSource = BitmapToImageSource(Bitmap);


        }

        private void DrawPolygon(Triangle triangle, Graphics gr)
        {
           
                
                Pen pen = new Pen(Color.Orange, 2);
                Brush brush = new SolidBrush(Color.Blue);




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
