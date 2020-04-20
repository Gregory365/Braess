namespace Braess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Braess.Model.Tools;
    using MoreLinq;

    public class Canvas
    {
        private readonly ObservableCollection<Circle> circles = new ObservableCollection<Circle>();

        private readonly ObservableCollection<Line> lines = new ObservableCollection<Line>();

        private readonly double circleDiameter;

        private readonly SolidColorBrush circleColor;

        private readonly double selectedCircleDiameter;

        private readonly SolidColorBrush selectedCircleColor;

        private readonly double lineWidth;

        private readonly SolidColorBrush lineColor;

        private Circle selectedCircle;

        public Canvas(double circleDiameter, SolidColorBrush circleColor, double selectedCircleDiameter, SolidColorBrush selectedCircleColor, double lineWidth, SolidColorBrush lineColor)
        {
            this.circleDiameter = circleDiameter;
            this.circleColor = circleColor;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
            this.selectedCircleDiameter = selectedCircleDiameter;
            this.selectedCircleColor = selectedCircleColor;
        }

        public event EventHandler SelectedCircleChanged = delegate { };

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

        public ReadOnlyObservableCollection<Circle> Circles => new ReadOnlyObservableCollection<Circle>(circles);

        public ReadOnlyObservableCollection<Line> Lines => new ReadOnlyObservableCollection<Line>(lines);

        public void AddCircle(Point point)
        {
            circles.Add(new Circle(point, circleDiameter, circleColor));
        }

        public void RemoveClosestCircle(Point mousePoint)
        {
            Circle closestCircle = GetClosestCircle(mousePoint);

            if (SelectedCircle?.Point == closestCircle.Point)
            {
                SelectedCircle = null;
            }

            circles.Remove(closestCircle);
            lines.RemoveAll(x => x.Point1 == closestCircle.Point || x.Point2 == closestCircle.Point);
        }

        public void Process(Point mousePoint)
        {
            Circle closestCircle = GetClosestCircle(mousePoint);

            if (closestCircle is null)
            {
                return;
            }

            // if circle is selected
            if (!(SelectedCircle is null))
            {
                // if the closest circle is the same as the currently selected circle.
                if (closestCircle.Point == SelectedCircle.Point)
                {
                    // deselect the cicle.
                    SelectedCircle = null;
                }

                // if the closest circle is different to the currently selected circle.
                else
                {
                    ToggleLine(SelectedCircle.Point, closestCircle.Point);

                    // change the closest circle to be the new selected circle
                    SelectedCircle = new Circle(closestCircle.Point, selectedCircleDiameter, selectedCircleColor);
                }
            }
            else
            {
                SelectedCircle = new Circle(closestCircle.Point, selectedCircleDiameter, selectedCircleColor);
            }
        }

        private static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        private Circle GetClosestCircle(Point point)
        {
            return circles.Count == 0 ? null : circles.MinBy(x => GetDistance(point, x.Point)).First();
        }

        private void ToggleLine(Point point1, Point point2)
        {
            Line newLine = new Line(point1, point2, lineWidth, lineColor);

            // Todo: Line could be split into two classes one for the two points and the other containing the width and color so it is more efficient.
            if (lines.Contains(newLine))
            {
                lines.Remove(newLine);
            }
            else
            {
                lines.Add(newLine);
            }
        }
    }
}
