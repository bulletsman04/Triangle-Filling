using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MyGraphics
    {
        private Bitmap _bitmap;

        public MyGraphics(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public void FillPolygon(Triangle triangle)
        {
            List<Vertex> vertices = triangle.Vertices;
            int[] ind = Enumerable.Range(0,triangle.Vertices.Count).OrderBy(x => triangle.Vertices[x].Y).ToArray();
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
                        AET.Add(new Node(iPrev,i,M,Pi.X));
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
                for (int i = 0; i < AET.Count - 1; i+= 2)
                {
                    for (int j = (int)Math.Round(AET[i].X); j < (int)Math.Round(AET[i+1].X); j++)
                    {
                        // _bitmap.SetPixel(j,y-1,LibrariesConverters.ColorConverter(triangle.TriangleSettings.PickedColor));
                        _bitmap.SetPixel(j, y - 1, triangle.TriangleSettings.PickedTriangleTexture.GetPixel(j% triangle.TriangleSettings.PickedTriangleTexture.Width,(y-1)% triangle.TriangleSettings.PickedTriangleTexture.Height));
                    }
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    AET[i].X += AET[i].iM;
                }
            }
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
    }
}
