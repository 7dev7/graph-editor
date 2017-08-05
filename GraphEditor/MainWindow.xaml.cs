using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GraphEditor.Logic;
using GraphEditor.Model;
using GraphEditor.Logic.Graphs;
using Petzold.Media2D;

namespace GraphEditor
{
    public partial class MainWindow : Window
    {
        private State state;
        private Drawer drawer;
        private GraphType graphType = GraphType.NON_ORIENTED;

        UIElement firstSelectedElem;

        bool isDown;
        bool isDragging;
        bool selected;

        Point startPoint;
        private double originalLeft;
        private double originalTop;

        public MainWindow()
        {
            InitializeComponent();
            IniEvents();
            drawer = new Drawer(canvas);
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (state == State.CURSOR)
            {
                ClearSelected();
                return;
            }
            if (state == State.EDGE)
            {
                if (firstSelectedElem != null)
                {
                    var selectedElem = e.Source as UIElement;

                    if (selectedElem is Canvas || selectedElem is Line || selectedElem is ArrowLine ||
                        firstSelectedElem is Canvas || firstSelectedElem is Line || firstSelectedElem is ArrowLine)
                    {
                        firstSelectedElem = null;
                        return;
                    }
                    drawer.AddEdge(firstSelectedElem, selectedElem, graphType);
                    firstSelectedElem = null;
                    return;
                }
                firstSelectedElem = e.Source as UIElement;
                startPoint = e.GetPosition(canvas);
                return;
            }
            drawer.AddVertex(e.GetPosition(canvas));
        }

        private void canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (state == State.CURSOR)
            {
                ClearSelected();
                if (e.Source != canvas)
                {
                    isDown = true;
                    startPoint = e.GetPosition(canvas);

                    firstSelectedElem = e.Source as UIElement;

                    originalLeft = Canvas.GetLeft(firstSelectedElem);
                    originalTop = Canvas.GetTop(firstSelectedElem);
                    selected = true;
                    e.Handled = true;
                }
                return;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (state == State.CURSOR)
            {
                StopDragging();
                e.Handled = true;
            }
        }

        private void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            if (state == State.CURSOR)
            {
                StopDragging();
                e.Handled = true;
            }
        }

        private void StopDragging()
        {
            if (isDown)
            {
                isDown = false;
                isDragging = false;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (state == State.CURSOR)
            {
                if (isDown)
                {
                    if ((isDragging == false) &&
                        ((Math.Abs(e.GetPosition(canvas).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                        (Math.Abs(e.GetPosition(canvas).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                        isDragging = true;

                    if (isDragging)
                    {
                        Point position = Mouse.GetPosition(canvas);

                        Canvas.SetTop(firstSelectedElem, position.Y - (startPoint.Y - originalTop));
                        Canvas.SetLeft(firstSelectedElem, position.X - (startPoint.X - originalLeft));
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (firstSelectedElem != null)
                {
                    if (firstSelectedElem is Ellipse)
                    {
                        drawer.DeleteVertex(firstSelectedElem);
                    }
                    if (firstSelectedElem is Line || firstSelectedElem is ArrowLine)
                    {
                        drawer.DeleteEdge(firstSelectedElem);
                    }
                    firstSelectedElem = null;
                }
            }
        }

        private void ClearSelected()
        {
            if (selected)
            {
                selected = false;
                if (firstSelectedElem != null)
                {
                    firstSelectedElem = null;
                }
            }
        }

        private void IniEvents()
        {
            vertexBtn.Checked += (s, e) => { state = State.VERTEX; ClearSelected(); };
            edgeBtn.Checked += (s, e) => { state = State.EDGE; ClearSelected(); };
            cursorBtn.Checked += (s, e) => { state = State.CURSOR; ClearSelected(); };
            typeBtn.Click += (s, e) => { ChangeType(); ClearSelected(); };
            colorBtn.Click += (s, e) => { ColorizeGraph(); ClearSelected(); };
            resetColorBtn.Click += (s, e) => { ResetColor(); ClearSelected(); };

            this.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            canvas.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
        }

        private void ChangeType()
        {
            if (graphType == GraphType.NON_ORIENTED)
            {
                graphType = GraphType.ORIENTED;
                typeLabel.Content = "Directed graph";
            }
            else
            {
                graphType = GraphType.NON_ORIENTED;
                typeLabel.Content = "Undirected graph";
            }
        }

        private void ColorizeGraph()
        {
            Painter painter = new Painter(canvas);
            painter.Colorize(drawer.Graph);
        }

        private void ResetColor()
        {
            Painter painter = new Painter(canvas);
            painter.ResetColors(drawer.Graph);
        }
    }
}
