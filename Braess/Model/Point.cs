namespace Braess.Model
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point otherPoint)
            {
                return X == otherPoint.X && Y == otherPoint.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (2 * X.GetHashCode()) + (3 * Y.GetHashCode());
        }

        public Point Clone()
        {
            return new Point(X, Y);
        }
    }
}
