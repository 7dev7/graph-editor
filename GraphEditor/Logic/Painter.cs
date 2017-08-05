using GraphEditor.Logic.Graphs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using GraphEditor.Model;

namespace GraphEditor.Logic
{
    public class Painter
    {
        private Canvas canvas;
        private SolidColorBrush[] colors = new SolidColorBrush[] {
            Brushes.Blue, Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Aqua,
            Brushes.Pink, Brushes.Orange, Brushes.Olive, Brushes.Lime, Brushes.Indigo };

        private SolidColorBrush DEFAULT_COLOR = Brushes.LightGray;

        public Painter(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Colorize(Graph graph)
        {
            if (graph.Vertices.Count == 0)
            {
                return;
            }
            Vertex[] vertices = graph.Vertices.OrderByDescending(it => it.IncidenceEdges.Count).ToArray();
            vertices[0].SetColor(colors[0], canvas, 0);

            for (int i = 1; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                List<int> colorIndexes = new List<int>();
                foreach (var edge in vertex.IncidenceEdges)
                {
                    var adjVertex = (vertex == edge.vertexFrom) ? edge.vertexTo : edge.vertexFrom;

                    if (adjVertex.ColorIndex != null)
                    {
                        colorIndexes.Add(adjVertex.ColorIndex.Value);
                    }
                }
                int minColorIndex = GetLowerNumber(colorIndexes);
                vertex.SetColor(colors[minColorIndex], canvas, minColorIndex);
            }
            graph.IsColored = true;
        }

        public void ResetColors(Graph graph)
        {
            foreach (var vertex in graph.Vertices)
            {
                vertex.SetColor(DEFAULT_COLOR, canvas, null);
            }
            graph.IsColored = false;
        }

        private int GetLowerNumber(List<int> adjIndex)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!adjIndex.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
