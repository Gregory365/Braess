﻿namespace Braess.Model
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
        private readonly ObservableCollection<Point> points = new ObservableCollection<Point>();

        private readonly ObservableCollection<Line> lines = new ObservableCollection<Line>();

        private Point selectedPoint;

        private Line selectedLine;

        public event EventHandler SelectedPointChanged = delegate { };

        public event EventHandler SelectedLineChanged = delegate { };

        public Point SelectedPoint
        {
            get
            {
                return selectedPoint;
            }

            private set
            {
                selectedPoint = value;
                SelectedPointChanged.Invoke(this, new EventArgs());
            }
        }

        public Line SelectedLine
        {
            get
            {
                return selectedLine;
            }

            private set
            {
                selectedLine = value;
                SelectedLineChanged.Invoke(this, new EventArgs());
            }
        }

        public ReadOnlyObservableCollection<Point> Points => new ReadOnlyObservableCollection<Point>(points);

        public ReadOnlyObservableCollection<Line> Lines => new ReadOnlyObservableCollection<Line>(lines);

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public void RemoveClosestPoint(Point mousePoint)
        {
            Point closestPoint = GetClosestPoint(mousePoint);

            if (SelectedPoint == closestPoint)
            {
                SelectedPoint = null;
            }

            if (SelectedLine.Point1 == closestPoint || SelectedLine.Point2 == closestPoint)
            {
                SelectedLine = null;
            }

            points.Remove(closestPoint);
            lines.RemoveAll(x => x.Point1 == closestPoint || x.Point2 == closestPoint);
        }

        public void ProcessPoint(Point mousePoint)
        {
            Point closestPoint = GetClosestPoint(mousePoint);

            if (closestPoint is null)
            {
                return;
            }

            // if no point is selected.
            if (SelectedPoint is null)
            {
                // change the selected point to be the closest point.
                SelectedPoint = closestPoint.Clone();
            }
            else
            {
                // if the currently selected point is the closest point.
                if (SelectedPoint == closestPoint)
                {
                    // deselect it.
                    SelectedPoint = null;
                }

                // if the currently selected point is different to the closest point.
                else
                {
                    ToggleLine(SelectedPoint, closestPoint);

                    // change the selected point to be the closest point.
                    SelectedPoint = closestPoint.Clone();
                }
            }
        }

        public void ProcessLine(Point mousePoint)
        {
            Line closestLine = GetClosestLine(mousePoint);

            if (closestLine is null)
            {
                return;
            }

            // if no line is selected.
            if (SelectedLine is null)
            {
                // change the selected line to be the closest line.
                SelectedLine = closestLine.Clone();
            }
            else
            {
                // if the currently selected line is the closest line.
                if (SelectedLine == closestLine)
                {
                    // deselect it.
                    SelectedLine = null;
                }

                // if the currently selected line is different to the closest line.
                else
                {
                    // change the selected line to be the closest line.
                    SelectedLine = closestLine.Clone();
                }
            }
        }

        private static double GetPointsDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        private static double GetLinesDistance(Line line, Point point)
        {
            return Math.Sqrt(Math.Pow(line.CentrePoint.X - point.X, 2) + Math.Pow(line.CentrePoint.Y - point.Y, 2));
        }

        private Point GetClosestPoint(Point point)
        {
            return points.Count == 0 ? null : points.MinBy(x => GetPointsDistance(x, point)).First();
        }

        private Line GetClosestLine(Point point)
        {
            return lines.Count == 0 ? null : lines.MinBy(x => GetLinesDistance(x, point)).First();
        }

        private void ToggleLine(Point point1, Point point2)
        {
            Line newLine = new Line(point1, point2);

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
