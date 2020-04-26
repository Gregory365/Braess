namespace Braess.Model
{
    using System;
    using System.Collections.Generic;

    public class Journey
    {
        public Journey(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public Point StartPoint { get; }

        public Point EndPoint { get; }

        public List<Line> Path { get; } = new List<Line>();
    }
}
