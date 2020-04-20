namespace Braess.Model
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public class Line
    {
        public Line(Point point1, Point point2, double width, SolidColorBrush color)
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

            Width = width;
            Color = color;
        }

        public Point Point1 { get; }

        public Point Point2 { get; }

        public double X1 { get => Point1.X; }

        public double Y1 { get => Point1.Y; }

        public double X2 { get => Point2.X; }

        public double Y2 { get => Point2.Y; }

        public double Length { get => Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2)); }

        public double Width { get; }

        public SolidColorBrush Color { get; }

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
                return Point1 == otherLine.Point1 && Point2 == otherLine.Point2 && Width == otherLine.Width && Color == otherLine.Color;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (2 * Point1.GetHashCode()) + (3 * Point2.GetHashCode()) + (5 * Width.GetHashCode()) + (7 * Color.GetHashCode());
        }
    }
}
