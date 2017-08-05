using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.Logic.Graphs
{
    public class Vertex
    {
        public Point VertexPoint { get; set; }
        public List<Edge> IncidenceEdges { get; set; }

        public Ellipse Element = new Ellipse();

        public int? ColorIndex = null;

        public Vertex(Point point)
        {
            VertexPoint = point;
            IncidenceEdges = new List<Edge>();
        }

        public void Draw(Canvas canvas)
        {
            Element.Stroke = Brushes.Black;
            Element.StrokeThickness = 2;
            Element.Fill = Brushes.LightGray;

            Element.SetValue(Canvas.LeftProperty, VertexPoint.X);
            Element.SetValue(Canvas.TopProperty, VertexPoint.Y);

            Element.Width = 30;
            Element.Height = 30;

            canvas.Children.Add(Element);
        }

        public void SetColor(SolidColorBrush brush, Canvas canvas, int? ColorIndex)
        {
            Element.Fill = brush;
            canvas.Children.Remove(Element);
            canvas.Children.Add(Element);
            this.ColorIndex = ColorIndex;
        }

        public override string ToString()
        {
            return "Vertex: " + VertexPoint.ToString();
        }
    }
}
