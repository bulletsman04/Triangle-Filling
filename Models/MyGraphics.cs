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

                    if (PiPrev.Y > Pi.Y)
                    {
                        double M = PiPrev.X != Pi.X ? (double)(Pi.Y - PiPrev.Y) / (double)(Pi.X - PiPrev.X) : 0;
                        AET.Add(new Node(iPrev, i, M, Pi.X));
                    }
                    else
                    {
                        for (int j = 0; j < AET.Count; j++)
                        {
                            if (AET[j].Start == iPrev && AET[j].End == i)
                            {
                                AET.RemoveAt(j);
                                break;
                            }
                        }
                    }

                    int iNext = (ind[k] + 1) % vertices.Count;
                    if (iNext < 0)
                        iNext += vertices.Count;
                    Vertex PiNext = vertices[iNext];

                    if (PiNext.Y > Pi.Y)
                    {
                        double M = PiNext.X != Pi.X ? (double)(Pi.Y - PiNext.Y) / (double)(Pi.X - PiNext.X) : 0;
                        AET.Add(new Node(i, iNext, M, Pi.X));
                    }
                    else
                    {
                        for (int j = 0; j < AET.Count; j++)
                        {
                            if (AET[j].Start == i && AET[j].End == iNext)
                            {
                                AET.RemoveAt(j);
                                break;
                            }
                        }
                    }
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

                for (int i = 0; i < AET.Count; i++)
                {
                    AET[i].X += AET[i].iM;
                }
            }
        }

        public System.Drawing.Color CalculateColor(int x, int y, Triangle triangle)
        {
            Vector3 L;
            if (_settings.IsLightMouse || _settings.IsLightAnimation)
            {
               L = new Vector3((float) (_settings.LightPoint.X - x), (float) (_settings.LightPoint.Y + y),
                    (float) _settings.LightPoint.Z);
            }
            else 
            {
                L = new Vector3(0,0,1);
            }

            L = Vector3.Normalize(L);
            

            Vector3 IL = LibrariesConverters.ColorToVector(_settings.LightColor);

            Vector3 IO;
            if (triangle.TriangleSettings.IsColor)
            {
                IO = LibrariesConverters.ColorToVector(triangle.TriangleSettings.PickedColor);
            }
            else
            {
                // ToDo: tutaj trzeba rozpatrzeć że modulo wychodzi ujemne
                IO = LibrariesConverters.ColorToVector(triangle.TriangleSettings.PickedTriangleTexture[
                    (x - triangle.MoveVector.Coordinates.X) % triangle.TriangleSettings.PickedTriangleTexture.Width,
                    (y - triangle.MoveVector.Coordinates.Y) % triangle.TriangleSettings.PickedTriangleTexture.Height].Color);
            }

            Vector3 N = _settings.NMap[x, y];

            double cosVR = 0;

            if (_settings.IsPhong)
            {
                Vector3 V = new Vector3(0, 0, 1);
                Vector3 RV = Vector3.Normalize((float)2 * N - L);

                cosVR = Math.Pow(V.X * RV.X + V.Y * RV.Y + V.Z * RV.Z, _settings.MPhong);
                if (cosVR < 0) cosVR = 0;
            }

            double cosLN = N.X * L.X + N.Y * L.Y + N.Z * L.Z;
            double R = _settings.LambertRate * IO.X * IL.X * cosLN +  _settings.PhongRate * IL.X * cosVR;
            double G = _settings.LambertRate*IO.Y * IL.Y * cosLN + _settings.PhongRate * IL.Y * cosVR;
            double B = _settings.LambertRate*IO.Z * IL.Z * cosLN + _settings.PhongRate * IL.Z * cosVR;

            if (R < 0) R = 0;
            if (R > 1) R = 1;
            if (G < 0) G = 0;
            if (G > 1) G = 1;
            if (B < 0) B = 0;
            if (B > 1) B = 1;
            // ToDo: Przepełnienie inta przy wczytaniu nie normla mapy jako normal mapy
            return Color.FromArgb((int)Math.Round(255 * R), (int)Math.Round(255 * G), (int)Math.Round(255 * B));

        }

      

    }
}
