using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Vertex
    {
        public Vertex Left { get; set; }
        public Vertex Right { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Rectangle Rectangle => new Rectangle(X-5,Y-5,10,10);

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
