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
        private ColorCalculator _colorCalculator;

        public MyGraphics(DirectBitmap directBitmap, Settings settings)
        {
            _directBitmap = directBitmap;
            _settings = settings;
            _colorCalculator = new ColorCalculator();
        }
        // ToDo: Zmienić na strukturę
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
                        _directBitmap.SetPixel(j, y - 1,_colorCalculator.CalculateColor(j, y - 1, triangle,_settings));
                    });
                 
                }
                // ToDo: Przerzucić wyżej
                foreach (var t in AET)
                {
                    t.X += t.iM;
                }
            }
        }

        private  void CheckNeighbour(List<Node> AET, int i, Vertex Pi, int iNext, Vertex PiNext)
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

        

      

    }
}
