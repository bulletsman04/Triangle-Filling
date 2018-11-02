using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Edge
    {
        public Vertex Left { get; set; }
        public Vertex Right { get; set; }

        public Edge(Vertex left, Vertex right)
        {
            Left = left;
            Right = right;
        }
    }
}
