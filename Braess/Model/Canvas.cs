namespace Braess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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

        public double TotalNumberOfCars { get; set; } = 10;

        public double TotalTime => lines.Sum(x => x.Time * x.NumberOfCars);

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

            if (SelectedLine?.Point1 == closestPoint || SelectedLine?.Point2 == closestPoint)
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
                SelectedPoint = closestPoint;
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
                    SelectedPoint = closestPoint;
                }
            }
        }

        public void FindAllRoutes()
        {
            foreach (var line in lines)
            {
                line.NumberOfCars = 0;
            }

            List<Journey> journeys = new List<Journey>();

            double totalPopulation = points.Sum(x => x.Population);
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    for (int k = 0; k < Math.Round(points[i].Population * points[j].Population / totalPopulation); k++)
                    {
                        journeys.Add(new Journey(points[i], points[j]));
                        journeys.Add(new Journey(points[j], points[i]));
                    }
                }
            }

            foreach (var journey in journeys)
            {
                FindRoute(journey);
            }

            foreach (var journey in journeys)
            {
                UndoJourney(journey);
                FindRoute(journey);
            }
        }

        public void FindBestRoutes()
        {
            foreach (var line in lines)
            {
                line.NumberOfCars = 0;
            }

            List<Journey> journeys = new List<Journey>();

            double totalPopulation = points.Sum(x => x.Population);
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    for (int k = 0; k < Math.Round(points[i].Population * points[j].Population / totalPopulation); k++)
                    {
                        journeys.Add(new Journey(points[i], points[j]));
                        journeys.Add(new Journey(points[j], points[i]));
                    }
                }
            }

            foreach (var journey in journeys)
            {
                FindBestRoute(journey);
            }

            foreach (var journey in journeys)
            {
                UndoJourney(journey);
                FindBestRoute(journey);
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
                SelectedLine = closestLine;
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
                    SelectedLine = closestLine;
                }
            }
        }

        private static Point GetOtherPoint(Line line, Point point)
        {
            return line.Point1 == point ? line.Point2 : line.Point1;
        }

        private static double GetPointsDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        private static double GetLinesDistance(Line line, Point point)
        {
            return Math.Sqrt(Math.Pow(line.CentrePoint.X - point.X, 2) + Math.Pow(line.CentrePoint.Y - point.Y, 2));
        }

        private void FindRoute(Journey journey)
        {
            Dictionary<Point, DijkstrasRow> dijkstrasTable = points.ToDictionary(x => x, x => new DijkstrasRow());
            dijkstrasTable[journey.StartPoint].Distance = 0;

            Step(journey.StartPoint, journey.EndPoint, dijkstrasTable);
            BackFill(journey, journey.EndPoint, dijkstrasTable);
        }

        private void FindBestRoute(Journey journey)
        {
            Dictionary<Point, DijkstrasRow> dijkstrasTable = points.ToDictionary(x => x, x => new DijkstrasRow());
            dijkstrasTable[journey.StartPoint].Distance = 0;

            BestStep(journey.StartPoint, journey.EndPoint, dijkstrasTable);
            BackFill(journey, journey.EndPoint, dijkstrasTable);
        }

        private void Step(Point startPoint, Point endPoint, Dictionary<Point, DijkstrasRow> dijkstrasTable)
        {
            if (startPoint == endPoint)
            {
                return;
            }

            IEnumerable<Line> linesFromPoint = lines.Where(x => x.Point1 == startPoint || x.Point2 == startPoint);

            foreach (var line in linesFromPoint)
            {
                Point otherPoint = GetOtherPoint(line, startPoint);
                if (!dijkstrasTable[otherPoint].Visited)
                {
                    line.NumberOfCars += 1;
                    if (dijkstrasTable[otherPoint].Distance > (line.Time + dijkstrasTable[startPoint].Distance))
                    {
                        dijkstrasTable[otherPoint].Distance = line.Time + dijkstrasTable[startPoint].Distance;
                        dijkstrasTable[otherPoint].PreviousPoint = startPoint;
                        dijkstrasTable[otherPoint].Line = line;
                    }

                    line.NumberOfCars -= 1;
                }
            }

            dijkstrasTable[startPoint].Visited = true;
            Step(dijkstrasTable.Where(x => !x.Value.Visited).MinBy(x => x.Value.Distance).First().Key, endPoint, dijkstrasTable);
        }

        private void BestStep(Point startPoint, Point endPoint, Dictionary<Point, DijkstrasRow> dijkstrasTable)
        {
            if (startPoint == endPoint)
            {
                return;
            }

            IEnumerable<Line> linesFromPoint = lines.Where(x => x.Point1 == startPoint || x.Point2 == startPoint);

            foreach (var line in linesFromPoint)
            {
                Point otherPoint = GetOtherPoint(line, startPoint);
                if (!dijkstrasTable[otherPoint].Visited)
                {
                    double previousTotalTime = TotalTime;
                    line.NumberOfCars += 1;
                    if (dijkstrasTable[otherPoint].Distance > (TotalTime - previousTotalTime + dijkstrasTable[startPoint].Distance))
                    {
                        dijkstrasTable[otherPoint].Distance = TotalTime - previousTotalTime + dijkstrasTable[startPoint].Distance;
                        dijkstrasTable[otherPoint].PreviousPoint = startPoint;
                        dijkstrasTable[otherPoint].Line = line;
                    }

                    line.NumberOfCars -= 1;
                }
            }

            dijkstrasTable[startPoint].Visited = true;
            BestStep(dijkstrasTable.Where(x => !x.Value.Visited).MinBy(x => x.Value.Distance).First().Key, endPoint, dijkstrasTable);
        }

        private void BackFill(Journey journey, Point endPoint, Dictionary<Point, DijkstrasRow> dijkstrasTable)
        {
            if (journey.StartPoint == endPoint)
            {
                return;
            }

            dijkstrasTable[endPoint].Line.NumberOfCars += 1;
            journey.Path.Add(dijkstrasTable[endPoint].Line);
            BackFill(journey, dijkstrasTable[endPoint].PreviousPoint, dijkstrasTable);
        }

        private void UndoJourney(Journey journey)
        {
            foreach (var line in journey.Path)
            {
                line.NumberOfCars -= 1;
            }

            journey.Path.Clear();
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
