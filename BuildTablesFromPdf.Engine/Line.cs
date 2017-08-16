using System;

namespace BuildTablesFromPdf.Engine
{
    /// <summary>
    /// Class that handles lines. Comparers are based on tolerance
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> struct.
        /// </summary>
        /// <param name="startX">The start x.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="startY">The start y.</param>
        /// <param name="endY">The end y.</param>
        public Line(float startX, float endX, float startY, float endY)
        {
            if (startX > endX)
            {
                EndPoint = new Point(startX, startY);
                StartPoint = new Point(endX, endY);
            }
            else if (startY > endY)
            {
                EndPoint = new Point(startX, startY);
                StartPoint = new Point(endX, endY);
            }
            else
            {
                StartPoint = new Point(startX, startY);
                EndPoint = new Point(endX, endY);
            }

            if (StartPoint == EndPoint)
                throw new InvalidOperationException("The line is a single point");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> struct.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        public Line(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
                throw new InvalidOperationException("The line is a single point");

            if (startPoint > endPoint)
            {
                EndPoint = startPoint;
                StartPoint = endPoint;
            }
            else
            {
                StartPoint = startPoint;
                EndPoint = endPoint;
            }
        }

        /// <summary>
        /// The start point
        /// </summary>
        public readonly Point StartPoint;

        /// <summary>
        /// The end point
        /// </summary>
        public readonly Point EndPoint;


        public Line Rotate(int pageRotation)
        {
            return new Line(StartPoint.Rotate(pageRotation), EndPoint.Rotate(pageRotation));
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", StartPoint, EndPoint);
        }

        /// <summary>
        /// Determines whether this line is horizontal.
        /// </summary>
        /// <returns>True if the line is horizontal; otherwise false</returns>
        public bool IsHorizontal()
        {
            return Math.Abs(StartPoint.Y - EndPoint.Y) < ContentExtractor.Tolerance;
        }


        /// <summary>
        /// Determines whether this line is vertical.
        /// </summary>
        /// <returns>True if the line is vertical; otherwise false</returns>
        public bool IsVertical()
        {
            return Math.Abs(StartPoint.X - EndPoint.X) < ContentExtractor.Tolerance;
        }


        /// <summary>
        /// Determines whether the specified line is coincident with this line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True if the lines are coincident; otherwise false</returns>
        public bool IsCoincident(Line line)
        {
            return this == line;
        }

        /// <summary>
        /// Determines whether the specified line is consecutive to this line.
        /// One line is considered consecutive of another line if the start point of one line is the same
        /// of the end point of the other line or vice versa. Also overlapped lines can be consecutive
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True if the line is consecutive to this line</returns>
        public bool IsConsecutive(Line line)
        {
            if (this.StartPoint == line.StartPoint)
                return true;
            if (this.StartPoint == line.EndPoint)
                return true;
            if (this.EndPoint == line.StartPoint)
                return true;
            if (this.EndPoint == line.EndPoint)
                return true;
            return false;
        }

        /// <summary>
        /// Determines whether the specified line is partially or totally overlapped with this line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True if this line is partially or totally overlapped with the specified line</returns>
        /// <exception cref="InvalidOperationException">IsOverlapped works only on horizontal and vertical lines</exception>
        public bool IsOverlapped(Line line)
        {
            if (
                !this.IsHorizontal() && !this.IsVertical() ||
                !line.IsHorizontal() && !line.IsVertical() ||
                this.IsHorizontal() != line.IsHorizontal())
                return false;

            if (!this.IsAlignedHorizontally(line) && !this.IsAlignedVertically(line))
                return false;

            if (IsConsecutive(line))
                return true;
            else if (IsCoincident(line))
                return true;
            else if (this.StartPoint <= line.StartPoint && line.StartPoint <= this.EndPoint)
                return true;
            else if (this.StartPoint <= line.EndPoint && line.EndPoint <= this.EndPoint)
                return true;
            else if (line.StartPoint <= this.StartPoint && this.EndPoint <= line.EndPoint)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Joins the line with the specified line and returns the joined line that could be
        /// the current line if the specified line is totally overlapped with this line,
        /// the specified line if the current line is totally overlapped with the specified line,
        /// a new line if the lines are partially overlapped
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>The joined line</returns>
        /// <exception cref="InvalidOperationException">
        /// The lines are not overlapped
        /// or
        /// The lines are not aligned
        /// </exception>
        public Line Join(Line line)
        {
            if (!this.IsOverlapped(line))
                throw new InvalidOperationException("The lines are not overlapped");

            if (!this.IsAlignedHorizontally(line) && !this.IsAlignedVertically(line))
                throw new InvalidOperationException("The lines are not aligned");

            if (this.IsCoincident(line))
                return this;

            if (this.StartPoint <= line.StartPoint && line.EndPoint <= this.EndPoint)
                return this;
            else if (this.StartPoint <= line.StartPoint && line.StartPoint <= this.EndPoint && this.EndPoint <= line.EndPoint && this.StartPoint != line.EndPoint)
                return new Line(this.StartPoint, line.EndPoint);
            else if (line.StartPoint <= this.StartPoint && this.StartPoint <= line.EndPoint && line.EndPoint <= this.EndPoint && line.StartPoint != this.EndPoint)
                return new Line(line.StartPoint, this.EndPoint);
            else if (line.StartPoint <= this.StartPoint && this.EndPoint <= line.EndPoint)
                return line;
            return this;
        }

        /// <summary>
        /// Determines whether this line and the specified line are aligned vertically.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True if the lines are aligned; otherwise false</returns>
        public bool IsAlignedVertically(Line line)
        {
            if (!this.IsVertical() || !line.IsVertical())
                return false;
            else if (Math.Abs(this.StartPoint.X - line.StartPoint.X) < ContentExtractor.Tolerance)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether this line and the specified line are aligned horizontally.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True if the lines are aligned; otherwise false</returns>
        public bool IsAlignedHorizontally(Line line)
        {
            if (!this.IsHorizontal() || !line.IsHorizontal())
                return false;
            else if (Math.Abs(this.StartPoint.Y - line.StartPoint.Y) < ContentExtractor.Tolerance)
                return true;
            else
                return false;
        }

        #region ==

        /// <summary>
        /// Determines whether the specified line, is equal to this line.
        /// </summary>
        /// <param name="other">The Line to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified Line is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Line other)
        {
            return 
                StartPoint == other.StartPoint && EndPoint == other.EndPoint ||
                StartPoint == other.EndPoint && EndPoint == other.StartPoint;

        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Line && Equals((Line)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (StartPoint.GetHashCode() * 397) ^ EndPoint.GetHashCode();
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Line left, Line right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Line left, Line right)
        {
            return !left.Equals(right);
        }

        #endregion


    }
}
