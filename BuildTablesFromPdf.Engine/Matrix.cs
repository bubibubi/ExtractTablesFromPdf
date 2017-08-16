using System;
using System.Globalization;

namespace BuildTablesFromPdf.Engine
{
    public class Matrix
    {

        public static readonly Matrix Identity = new Matrix();

        private Matrix()
        {}

        public Matrix(double a, double b, double c, double d, double e, double f)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _e = e;
            _f = f;
        }

        private double _a = 1;
        private double _b = 0;
        private double _c = 0;
        private double _d = 1;
        private double _e = 0;
        private double _f = 0;

        // ReSharper disable InconsistentNaming
        public double a
        {
            get { return _a; }
        }

        public double b
        {
            get { return _b; }
        }

        public double c
        {
            get { return _c; }
        }

        public double d
        {
            get { return _d; }
        }

        public double e
        {
            get { return _e; }
        }

        public double f
        {
            get { return _f; }
        }
        // ReSharper restore InconsistentNaming


        public Matrix GetRotationMatrix()
        {
            return new Matrix(a, b, c, d, 0, 0);
        }

        public Matrix GetTranslationMatrix()
        {
            return new Matrix(0, 0, 0, 0, e, f);
        }



        public static Matrix Parse(string s)
        {
            string[] parts = s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 6)
                throw new FormatException("s is not a transformation matrix");

            var transformMatrix = new Matrix()
            {
                _a = double.Parse(parts[0], NumberFormatInfo.InvariantInfo),
                _b = double.Parse(parts[1], NumberFormatInfo.InvariantInfo),
                _c = double.Parse(parts[2], NumberFormatInfo.InvariantInfo),
                _d = double.Parse(parts[3], NumberFormatInfo.InvariantInfo),
                _e = double.Parse(parts[4], NumberFormatInfo.InvariantInfo),
                _f = double.Parse(parts[5], NumberFormatInfo.InvariantInfo)
            };

            return transformMatrix;
        }

        public static bool TryParse(string s, out Matrix trasformationMatrix)
        {
            try
            {
                trasformationMatrix = Parse(s);
                return true;
            }
            catch
            {
                trasformationMatrix = new Matrix();
                return false;
            }
        }

        public static Matrix operator *(Matrix l, Matrix r)
        {
            if (l == null)
                return null;
            if (r == null)
                return null;

            Matrix z = new Matrix();
            z._a = l.a * r.a + l.b * r.c;
            z._b = l.a * r.b + l.b * r.d;
            z._c = l.c * r.a + l.d * r.c;
            z._d = l.c * r.b + l.d * r.d;
            z._e = l.e * r.a + l.f * r.c + 1 * r._e;
            z._f = l.e * r.b + l.f * r.d + 1 * r.f;

            return z;
        }


        public static Point operator *(Point l, Matrix r)
        {
            if (r == null)
                return l;

            double x = r.TransformX(l.X, l.Y);
            double y = r.TransformY(l.X, l.Y);

            return new Point(x, y);
        }

        public double TransformX(double x, double y)
        {
            return a * x + c * y + e;

        }

        public double TransformY(double x, double y)
        {
            return b * x + d * y + f;

        }

        public Point TransformPoint(Point point)
        {
            return new Point(TransformX(point.X, point.Y), TransformY(point.X, point.Y));
        }

        public Line TransformLine(Line line)
        {
            return new Line(TransformPoint(line.StartPoint), TransformPoint(line.EndPoint));
        }

        #region ==

        protected bool Equals(Matrix other)
        {
            return _a.Equals(other._a) && _b.Equals(other._b) && _c.Equals(other._c) && _d.Equals(other._d) && _e.Equals(other._e) && _f.Equals(other._f);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _a.GetHashCode();
                hashCode = (hashCode * 397) ^ _b.GetHashCode();
                hashCode = (hashCode * 397) ^ _c.GetHashCode();
                hashCode = (hashCode * 397) ^ _d.GetHashCode();
                hashCode = (hashCode * 397) ^ _e.GetHashCode();
                hashCode = (hashCode * 397) ^ _f.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("|{0}|{1}|0 - {2}|{3}|0 - {4}|{5}|1", a, b, c, d, e, f);
        }
    }
}
