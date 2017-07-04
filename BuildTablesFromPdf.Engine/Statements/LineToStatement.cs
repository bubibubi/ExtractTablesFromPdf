using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    /// <summary>
    /// Draw a line from the current point to the specified point
    /// </summary>
    /// <seealso cref="PointStatement" />
    class LineToStatement : PointStatement
    {
        public LineToStatement(string rawContent)
        {
            RawContent = rawContent;
            _point = Point.Parse(rawContent);
        }

        private Point _point;

        public Point Point
        {
            get { return _point; }
            set { _point = value; }
        }
    }
}