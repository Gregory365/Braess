namespace Braess.Model
{
    using System.Windows;
    using System.Windows.Media;

    public class Circle
    {
        public Circle(Point point, double diameter, SolidColorBrush color)
        {
            Point = point;
            Diameter = diameter;
            Color = color;
        }

        public Point Point { get; }

        public double X { get => Point.X; }

        public double Y { get => Point.Y; }

        public double CenterX { get => X - (Diameter / 2); }

        public double CenterY { get => Y - (Diameter / 2); }

        public double Diameter { get; }

        public SolidColorBrush Color { get; }

        public static bool operator ==(Circle circle1, Circle circle2)
        {
            return circle1.Equals(circle2);
        }

        public static bool operator !=(Circle circle1, Circle circle2)
        {
            return !(circle1 == circle2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Circle otherCircle)
            {
                return Point == otherCircle.Point && Diameter == otherCircle.Diameter && Color == otherCircle.Color;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (2 * Point.GetHashCode()) + (3 * Diameter.GetHashCode()) + (5 * Color.GetHashCode());
        }
    }
}
