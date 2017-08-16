using System;
using System.Globalization;

namespace BuildTablesFromPdf.Engine
{
    public struct Point
    {
        public static readonly Point Origin = new Point(0, 0);

        #region ==

        public bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < ContentExtractor.Tolerance && Math.Abs(Y - other.Y) < ContentExtractor.Tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region >, >=, <, <=

        public static bool operator >(Point left, Point right)
        {
            if (Math.Abs(left.X - right.X) < ContentExtractor.Tolerance)
            {
                if (Math.Abs(left.Y - right.Y) < ContentExtractor.Tolerance)
                    // Equal point
                    return false;
                else
                    return left.Y > right.Y;
            }
            else
            {
                return left.X > right.X;
            }
        }

        public static bool operator >=(Point left, Point right)
        {
            if (Math.Abs(left.X - right.X) < ContentExtractor.Tolerance)
            {
                if (Math.Abs(left.Y - right.Y) < ContentExtractor.Tolerance)
                    // Equal point
                    return true;
                else
                    return left.Y > right.Y;
            }
            else
            {
                return left.X > right.X;
            }
        }

        public static bool operator <=(Point left, Point right)
        {
            if (Math.Abs(left.X - right.X) < ContentExtractor.Tolerance)
            {
                if (Math.Abs(left.Y - right.Y) < ContentExtractor.Tolerance)
                    // Equal point
                    return true;
                else
                    return left.Y < right.Y;
            }
            else
            {
                return left.X < right.X;
            }
        }


        public static bool operator <(Point left, Point right)
        {
            if (Math.Abs(left.X - right.X) < ContentExtractor.Tolerance)
            {
                if (Math.Abs(left.Y - right.Y) < ContentExtractor.Tolerance)
                    // Equal point
                    return false;
                else
                    return left.Y < right.Y;
            }
            else
            {
                return left.X < right.X;
            }
        }

        #endregion

        public readonly double X;
        public readonly double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point Parse(string rawContent)
        {
            var splittedRawContent = rawContent.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            float x = float.Parse(splittedRawContent[0], NumberFormatInfo.InvariantInfo);
            float y = float.Parse(splittedRawContent[1], NumberFormatInfo.InvariantInfo);
            return new Point(x, y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        public float Distance(Point point)
        {
            return (float) Math.Sqrt((X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y));
        }

        public bool IsValid()
        {
            if (X < 0 || Y < 0)
                return false;

            if (X > 10000 || Y > 10000)
                return false;

            return true;
        }

        public Point Rotate(int pageRotation)
        {
            switch (pageRotation)
            {
                case 0:
                    return new Point(X, 800 - Y);
                case 90:
                    return new Point(Y, X);
                default:
                    return this;
            }
        }
    }
}
