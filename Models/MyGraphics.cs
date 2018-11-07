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
        private PixelMap _pixelMap;
        private Settings _settings;

        public MyGraphics(PixelMap pixelMap, Settings settings)
        {
            _pixelMap = pixelMap;
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
                    for (int j = (int)Math.Round(AET[i].X); j < (int)Math.Round(AET[i + 1].X); j++)
                    {

                        _pixelMap[j, y - 1] = new Pixel(CalculateColor(j, y - 1, triangle));
                        //_bitmap.SetPixel(j, y - 1, triangle.TriangleSettings.PickedTriangleTexture.GetPixel(j% triangle.TriangleSettings.PickedTriangleTexture.Width,(y-1)% triangle.TriangleSettings.PickedTriangleTexture.Height));
                    }
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    AET[i].X += AET[i].iM;
                }
            }
        }

        public System.Drawing.Color CalculateColor(int x, int y, Triangle triangle)
        {
            Vector3 L = new Vector3((float)(_settings.LightPoint.X - x), (float)(_settings.LightPoint.Y + y), (float)_settings.LightPoint.Z);
            //Vector3 L = new Vector3(0,-10,1);
            L = Vector3.Normalize(L);
            Vector3 N;

            Vector3 IL = ColorToVector(_settings.LightColor);

            Vector3 IO;
            if (triangle.TriangleSettings.IsColor)
            {
                IO = ColorToVector(triangle.TriangleSettings.PickedColor);
            }
            else
            {
                // ToDo: tutaj trzeba rozpatrzeć że modulo wychodzi ujemne
                IO = ColorToVector(triangle.TriangleSettings.PickedTriangleTexture.GetPixel(
                    (x - triangle.MoveVector.Coordinates.X) % triangle.TriangleSettings.PickedTriangleTexture.Width,
                    (y - triangle.MoveVector.Coordinates.Y) % triangle.TriangleSettings.PickedTriangleTexture.Height));
            }

            if (_settings.IsNormalConst)
            {
                N = _settings.NormalVector;
            }
            else
            {
                N = ColorToNormalVector(_settings.NormalMap.GetPixel(x % _settings.NormalMap.Width, y % _settings.NormalMap.Height));
            }

            N = Vector3.Normalize(N);

            if (!_settings.IsHeightConst)
            {

                Vector3 TV = new Vector3(1, 0, -N.X / N.Z);
                Vector3 BV = new Vector3(0, 1, -N.Y / N.Z);
                float dX = _settings.HeightMap.GetPixel((x + 1) % _settings.HeightMap.Width, y % _settings.HeightMap.Height).R - _settings.HeightMap.GetPixel(x % _settings.HeightMap.Width, y % _settings.HeightMap.Height).R;
                float dY = _settings.HeightMap.GetPixel(x % _settings.HeightMap.Width, (y + 1) % _settings.HeightMap.Height).R - _settings.HeightMap.GetPixel(x % _settings.HeightMap.Width, y % _settings.HeightMap.Height).R;
                Vector3 D = TV * dX + BV * dY;

                N += D / (float)30;
                N = Vector3.Normalize(N);
            }

            double cosVR = 0;

            if (_settings.IsPhong)
            {
                Vector3 V = new Vector3(0, 0, 1);
                Vector3 RV = Vector3.Normalize(2 * N - L);

                cosVR = Math.Pow(V.X * RV.X + V.Y * RV.Y + V.Z * RV.Z, _settings.MPhong);
                if (cosVR < 0) cosVR = 0;
            }

            double cosLN = N.X * L.X + N.Y * L.Y + N.Z * L.Z;
            double R = IO.X * IL.X * cosLN + 0.5 * IL.X * cosVR;
            double G = IO.Y * IL.Y * cosLN + 0.5 * IL.Y * cosVR;
            double B = IO.Z * IL.Z * cosLN + 0.5 * IL.Z * cosVR;

            if (R < 0) R = 0;
            if (R > 1) R = 1;
            if (G < 0) G = 0;
            if (G > 1) G = 1;
            if (B < 0) B = 0;
            if (B > 1) B = 1;

            return Color.FromArgb((int)(255 * R), (int)(255 * G), (int)(255 * B));

        }

        public static Vector3 ColorToVector(Color color)
        {
            return new Vector3((float)color.R / 255, (float)color.G / 255, (float)color.B / 255);
        }

        public static Vector3 ColorToNormalVector(Color color)
        {
            return new Vector3(((float)color.R * 2 / 255 - 1), ((float)color.G * 2 / 255 - 1), (float)color.B / 255);
        }

    }
}
