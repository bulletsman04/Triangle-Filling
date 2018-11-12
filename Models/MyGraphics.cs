using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PixelMapSharp;

namespace Models
{
    public class MyGraphics
    {
        private DirectBitmap _directBitmap;
        private Settings _settings;

        public MyGraphics(DirectBitmap directBitmap, Settings settings)
        {
            _directBitmap = directBitmap;
            _settings = settings;
        }
        private class Node
        {
            public int Start { get; }
            public int End { get; }
            public double iM { get; }
            public double X { get; set; }


            public Node(int start, int end, double M, int x)
            {
                Start = start;
                End = end;
                this.iM = M != 0 ? 1 / M : 0;
                X = x;
            }
        }
        public void FillPolygon(Triangle triangle)
        {
            List<Vertex> vertices = triangle.Vertices;
            int[] ind = Enumerable.Range(0, triangle.Vertices.Count).OrderBy(x => triangle.Vertices[x].Y).ToArray();
            int ymin = vertices[ind[0]].Y;
            int ymax = vertices[ind[vertices.Count - 1]].Y;
            int k = 0;
            List<Node> AET = new List<Node>();

            for (int y = ymin; y <= ymax; y++)
            {
                while (vertices[ind[k]].Y == y - 1)
                {
                    int i = ind[k];
                    int iPrev = (ind[k] - 1) % vertices.Count;
                    if (iPrev < 0)
                        iPrev += vertices.Count;

                    Vertex Pi = vertices[i];
                    Vertex PiPrev = vertices[iPrev];

                    CheckNeighbour(AET, iPrev, Pi, i, PiPrev);

                    int iNext = (ind[k] + 1) % vertices.Count;
                    if (iNext < 0)
                        iNext += vertices.Count;

                    Vertex PiNext = vertices[iNext];

                    CheckNeighbour(AET, i, Pi, iNext, PiNext);
                    k++;
                }

                // AET update
                AET = AET.OrderBy(node => node.X).ToList();
                for (int i = 0; i < AET.Count - 1; i += 2)
                {
                    Parallel.For((int) Math.Round(AET[i].X), (int) Math.Round(AET[i + 1].X), j =>
                    {
                        if(j < _directBitmap.Width && j >= 0 && (y-1) < _directBitmap.Height && (y-1) >=0 )
                        _directBitmap.SetPixel(j, y - 1,CalculateColor(j, y - 1, triangle));
                    });
                 
                }

                foreach (var t in AET)
                {
                    t.X += t.iM;
                }
            }
        }

        private static void CheckNeighbour(List<Node> AET, int i, Vertex Pi, int iNext, Vertex PiNext)
        {
            if (PiNext.Y > Pi.Y)
            {
                double M = PiNext.X != Pi.X ? (double)(Pi.Y - PiNext.Y) / (double)(Pi.X - PiNext.X) : 0;
                AET.Add(new Node(i, iNext, M, Pi.X));
            }
            else
            {
                for (int j = 0; j < AET.Count; j++)
                {
                    if (AET[j].Start != i || AET[j].End != iNext) continue;
                    AET.RemoveAt(j);
                    break;
                }
            }
        }

        public System.Drawing.Color CalculateColor(int x, int y, Triangle triangle)
        {
            Vector3 L;
            if (_settings.IsLightMouse || _settings.IsLightAnimation)
            {
               L = new Vector3(_settings.LightPoint.X - x,_settings.LightPoint.Y + y,
                    _settings.LightPoint.Z);
            }
            else 
            {
                L = new Vector3(0,0,1);
            }

            L = Vector3.Normalize(L);

            Vector3 IL = _settings.LightColor;

            Vector3 IO;
            if (triangle.TriangleSettings.IsColor)
            {
                IO = triangle.TriangleSettings.PickedColor;
            }
            else
            {
                // ToDo: tutaj trzeba rozpatrzeć że modulo wychodzi ujemne
                IO = triangle.TriangleSettings.PickedTriangleTexture[
                    (x - triangle.MoveVector.Coordinates.X) % triangle.TriangleSettings.PickedTriangleTexture.GetLength(0),
                    (y - triangle.MoveVector.Coordinates.Y) % triangle.TriangleSettings.PickedTriangleTexture.GetLength(1)];
            }

            Vector3 N = _settings.NMap[x, y];

            double cosVR = 0;

            if (_settings.IsPhong)
            {
                Vector3 V = new Vector3(0, 0, 1);
                Vector3 RV = Vector3.Normalize(2 * N - L);

                cosVR = Math.Pow(V.X * RV.X + V.Y * RV.Y + V.Z * RV.Z, _settings.MPhong);
                if (cosVR < 0) cosVR = 0;
            }

            double cosLN = N.X * L.X + N.Y * L.Y + N.Z * L.Z;
            double R = IL.X * (_settings.LambertRate * IO.X * cosLN + _settings.PhongRate * cosVR);
            double G = IL.Y * (_settings.LambertRate*IO.Y *  cosLN + _settings.PhongRate * cosVR);
            double B = IL.Z * (_settings.LambertRate*IO.Z *  cosLN + _settings.PhongRate * cosVR);

            if (R < 0) R = 0;
            if (R > 1) R = 1;
            if (G < 0) G = 0;
            if (G > 1) G = 1;
            if (B < 0) B = 0;
            if (B > 1) B = 1;
            return Color.FromArgb((int)Math.Round(255 * R), (int)Math.Round(255 * G), (int)Math.Round(255 * B));

        }

      

    }
}
