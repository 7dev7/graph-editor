using GraphEditor.Logic.Graphs;
using System.Collections.Generic;

namespace GraphEditor.Model
{
    public class Graph
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
        public GraphType Type { get; private set; }
        public bool IsColored { get; set; }

        public Graph()
        {
            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
        }
    }
}
