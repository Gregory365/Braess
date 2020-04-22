﻿namespace Braess.Model
{
    public class Line
    {
        public Line(Point point1, Point point2)
        {
            if (point1.X < point2.X)
            {
                Point1 = point1;
                Point2 = point2;
            }
            else if (point1.X > point2.X)
            {
                Point1 = point2;
                Point2 = point1;
            }
            else if (point1.Y < point2.Y)
            {
                Point1 = point1;
                Point2 = point2;
            }
            else
            {
                Point1 = point2;
                Point2 = point1;
            }
        }

        public Point Point1 { get; }

        public Point Point2 { get; }

        public static bool operator ==(Line line1, Line line2)
        {
            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
        {
            return !(line1 == line2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Line otherLine)
            {
                return Point1 == otherLine.Point1 && Point2 == otherLine.Point2;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (2 * Point1.GetHashCode()) + (3 * Point2.GetHashCode());
        }
    }
}