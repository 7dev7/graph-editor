using Petzold.Media2D;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.Logic.Graphs
{
    public class Edge
    {
        public Vertex vertexFrom { get; set; }
        public Vertex vertexTo { get; set; }

        public GraphType Type { get; set; }

        public Line line = new Line();
        public ArrowLine arrowLine = new ArrowLine();

        public void Draw(Canvas canvas)
        {
            if (Type == GraphType.DIRECTED)
            {
                DrawArrow(canvas);
            }
            else
            {
                DrawLine(canvas);
            }
        }

        private void DrawArrow(Canvas canvas)
        {
            arrowLine.Stroke = Brushes.Black;
            arrowLine.StrokeThickness = 2;
            arrowLine.ArrowLength = 25;

            arrowLine.X1 = vertexFrom.VertexPoint.X + 15; arrowLine.Y1 = vertexFrom.VertexPoint.Y + 15;
            arrowLine.X2 = vertexTo.VertexPoint.X + 15; arrowLine.Y2 = vertexTo.VertexPoint.Y + 15;

            Binding x1 = new Binding(); x1.Path = new PropertyPath(Canvas.LeftProperty); x1.Converter = new EllipseBindingConverter(); x1.ConverterParameter = vertexFrom.Element;
            Binding y1 = new Binding(); y1.Path = new PropertyPath(Canvas.TopProperty); y1.Converter = new EllipseBindingConverter(); y1.ConverterParameter = vertexFrom.Element;

            Binding x2 = new Binding(); x2.Path = new PropertyPath(Canvas.LeftProperty); x2.Converter = new EllipseBindingConverter(); x2.ConverterParameter = vertexTo.Element;
            Binding y2 = new Binding(); y2.Path = new PropertyPath(Canvas.TopProperty); y2.Converter = new EllipseBindingConverter(); y2.ConverterParameter = vertexTo.Element;

            x1.Source = y1.Source = vertexFrom.Element;
            x2.Source = y2.Source = vertexTo.Element;

            arrowLine.SetBinding(ArrowLine.X1Property, x1);
            arrowLine.SetBinding(ArrowLine.Y1Property, y1);
            arrowLine.SetBinding(ArrowLine.X2Property, x2);
            arrowLine.SetBinding(ArrowLine.Y2Property, y2);

            arrowLine.MouseEnter += (s, e) => { ((ArrowLine)s).Stroke = Brushes.BlueViolet; ((ArrowLine)s).StrokeThickness += 2; };
            arrowLine.MouseLeave += (s, e) => { ((ArrowLine)s).Stroke = Brushes.Black; ((ArrowLine)s).StrokeThickness -= 2; };
            arrowLine.Cursor = System.Windows.Input.Cursors.Hand;

            canvas.Children.Add(arrowLine);
        }

        private void DrawLine(Canvas canvas)
        {
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 2;

            line.X1 = vertexFrom.VertexPoint.X + 15; line.Y1 = vertexFrom.VertexPoint.Y + 15;
            line.X2 = vertexTo.VertexPoint.X + 15; line.Y2 = vertexTo.VertexPoint.Y + 15;

            Binding x1 = new Binding(); x1.Path = new PropertyPath(Canvas.LeftProperty); x1.Converter = new EllipseBindingConverter(); x1.ConverterParameter = vertexFrom.Element;
            Binding y1 = new Binding(); y1.Path = new PropertyPath(Canvas.TopProperty); y1.Converter = new EllipseBindingConverter(); y1.ConverterParameter = vertexFrom.Element;

            Binding x2 = new Binding(); x2.Path = new PropertyPath(Canvas.LeftProperty); x2.Converter = new EllipseBindingConverter(); x2.ConverterParameter = vertexTo.Element;
            Binding y2 = new Binding(); y2.Path = new PropertyPath(Canvas.TopProperty); y2.Converter = new EllipseBindingConverter(); y2.ConverterParameter = vertexTo.Element;

            x1.Source = y1.Source = vertexFrom.Element;
            x2.Source = y2.Source = vertexTo.Element;

            line.SetBinding(Line.X1Property, x1);
            line.SetBinding(Line.Y1Property, y1);
            line.SetBinding(Line.X2Property, x2);
            line.SetBinding(Line.Y2Property, y2);

            line.MouseEnter += (s, e) => { ((Line)s).Stroke = Brushes.BlueViolet; ((Line)s).StrokeThickness += 2; };
            line.MouseLeave += (s, e) => { ((Line)s).Stroke = Brushes.Black; ((Line)s).StrokeThickness -= 2; };
            line.Cursor = System.Windows.Input.Cursors.Hand;

            canvas.Children.Add(line);
        }

        public override string ToString()
        {
            return "From" + vertexFrom + " To: " + vertexTo;
        }

        private class EllipseBindingConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Ellipse e = parameter as Ellipse;
                Double d = (Double)value;

                return d + (e.ActualWidth / 2);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Ellipse e = parameter as Ellipse;
                Double d = (Double)value;

                return d - (e.ActualWidth / 2);
            }
        }
    }
}
