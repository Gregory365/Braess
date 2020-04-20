namespace Braess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Braess.Model.Tools;

    public class Canvas
    {
        private readonly IList<Point> points = new List<Point>
        {
            new Point(87, 122),
            new Point(125, 96),
            new Point(273, 93),
            new Point(346, 122),
            new Point(555, 127),
            new Point(953, 105),
            new Point(311, 196),
            new Point(434, 274),
            new Point(797, 227),
            new Point(460, 322),
            new Point(663, 321),
            new Point(986, 301),
            new Point(273, 401),
            new Point(524, 398),
            new Point(663, 390),
            new Point(875, 353),
            new Point(1077, 368),
            new Point(1179, 373),
            new Point(290, 441),
            new Point(743, 430),
            new Point(922, 431),
            new Point(170, 575),
            new Point(279, 546),
            new Point(317, 545),
            new Point(459, 504),
            new Point(420, 560),
            new Point(472, 576),
            new Point(722, 478),
            new Point(625, 529),
            new Point(883, 518),
            new Point(614, 691),
            new Point(882, 615),
            new Point(816, 381),
        };

        private Dictionary<Circle, List<Line>> linesLinkedToCircles = new Dictionary<Circle, List<Line>>();

        private ObservableCollection<Circle> circles = new ObservableCollection<Circle>();

        private ObservableCollection<Line> lines = new ObservableCollection<Line>();

        private double circleDiameter;

        private SolidColorBrush circleColor;

        private Circle selectedCircle;

        private double selectedCircleDiameter;

        private SolidColorBrush selectedCircleColor;

        private double lineWidth;

        private SolidColorBrush lineColor;

        public Canvas(double circleDiameter, SolidColorBrush circleColor, double selectedCircleDiameter, SolidColorBrush selectedCircleColor, double lineWidth, SolidColorBrush lineColor)
        {
            this.circleDiameter = circleDiameter;
            this.circleColor = circleColor;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
            this.selectedCircleDiameter = selectedCircleDiameter;
            this.selectedCircleColor = selectedCircleColor;

            foreach (Point point in points)
            {
                AddCircle(point);
            }
        }

        public event EventHandler SelectedCircleChanged = delegate { };

        public ReadOnlyObservableCollection<Circle> Circles => new ReadOnlyObservableCollection<Circle>(circles);

        public ReadOnlyObservableCollection<Line> Lines => new ReadOnlyObservableCollection<Line>(lines);

        public Circle SelectedCircle
        {
            get
            {
                return selectedCircle;
            }

            private set
            {
                selectedCircle = value;
                SelectedCircleChanged.Invoke(this, new EventArgs());
            }
        }

        public void RemoveCircle(Circle circle)
        {
            if (SelectedCircle?.Point == circle.Point)
            {
                SelectedCircle = null;
            }

            circles.Remove(circle);
            lines.RemoveAll(x => x.Point1 == circle.Point || x.Point2 == circle.Point);
        }

        public void AddCircle(Point point)
        {
            circles.Add(new Circle(point, circleDiameter, circleColor));
        }

        public void ToggleLine(Point point1, Point point2)
        {
            Line newLine = new Line(point1, point2, lineWidth, lineColor);

            if (lines.Contains(newLine))
            {
                lines.Remove(newLine);
            }
            else
            {
                lines.Add(newLine);
            }
        }

        public void Process(Circle closestCircle)
        {
            if (SelectedCircle is null)
            {
                SelectedCircle = new Circle(closestCircle.Point, selectedCircleDiameter, selectedCircleColor);
            }
            else
            {
                if (SelectedCircle.Point == closestCircle.Point)
                {
                    SelectedCircle = null;
                }
                else
                {
                    ToggleLine(SelectedCircle.Point, closestCircle.Point);
                    SelectedCircle = new Circle(closestCircle.Point, selectedCircleDiameter, selectedCircleColor);
                }
            }
        }

        public Circle GetClosestCircle(Point mousePoint)
        {
            if (circles.Count == 0)
            {
                return null;
            }

            List<double> distances = new List<double>();

            foreach (Circle circle in circles)
            {
                distances.Add(GetDistance(circle.Point, mousePoint));
            }

            return circles[distances.IndexOf(distances.Min())];
        }

        private double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
