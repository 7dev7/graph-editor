using System.Windows;
using System.Windows.Controls;
using GraphEditor.Logic.Graphs;
using System.Collections.Generic;
using GraphEditor.Logic;

namespace GraphEditor.Model
{
    public class Drawer
    {
        public Graph Graph { get; set; }
        private Canvas canvas;
        private const int OFFSET = 15;
        private const int VERTEX_SIZE = 30;
        private const int THICKNESS = 2;

        public Drawer(Canvas canvas)
        {
            this.canvas = canvas;
            this.Graph = new Graph();
        }

        public void AddVertex(Point point)
        {
            Vertex v = new Vertex(point);
            Graph.Vertices.Add(v);
            v.Draw(canvas);
        }

        public void AddEdge(UIElement elem1, UIElement elem2, GraphType type)
        {
            var edg = new Edge();
            var v1 = Graph.Vertices.Find(x => x.Element == elem1);
            var v2 = Graph.Vertices.Find(x => x.Element == elem2);
            edg.vertexFrom = v1;
            edg.vertexTo = v2;
            edg.Type = type;

            v1.IncidenceEdges.Add(edg);
            v2.IncidenceEdges.Add(edg);

            edg.Draw(canvas);

            if (Graph.IsColored)
            {
                Painter painter = new Painter(canvas);
                painter.Colorize(Graph);
            }
        }

        public void DeleteVertex(UIElement elem)
        {
            var v = Graph.Vertices.Find(x => x.Element == elem);

            foreach (var edge in v.IncidenceEdges)
            {
                canvas.Children.Remove(edge.line);
                canvas.Children.Remove(edge.arrowLine);
            }
            Graph.Vertices.RemoveAll(it => it.Element == elem);
            canvas.Children.Remove(elem);            

            if (Graph.IsColored)
            {
                Painter painter = new Painter(canvas);
                painter.Colorize(Graph);
            }
        }

        public void DeleteEdge(UIElement elem)
        {
            foreach (var v in Graph.Vertices)
            {
                v.IncidenceEdges.RemoveAll(it => it.line == elem);
                v.IncidenceEdges.RemoveAll(it => it.arrowLine == elem);
            }
            Graph.Edges.RemoveAll(it => it.line == elem);
            Graph.Edges.RemoveAll(it => it.arrowLine == elem);
            canvas.Children.Remove(elem);

            if (Graph.IsColored)
            {
                Painter painter = new Painter(canvas);
                painter.Colorize(Graph);
            }
        }

        public List<Edge> getIncidenceEdges(UIElement elem)
        {
            var v = Graph.Vertices.Find(x => x.Element == elem);
            return v.IncidenceEdges;
        }
    }
}
