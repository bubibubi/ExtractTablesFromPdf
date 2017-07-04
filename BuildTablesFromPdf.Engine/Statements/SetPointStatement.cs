using System;

namespace BuildTablesFromPdf.Engine.Statements
{

    /// <summary>
    /// Set the new begin point for the graphic operations
    /// </summary>
    /// <seealso cref="PointStatement" />
    class SetPointStatement : PointStatement
    {
        private Point _point;

        public SetPointStatement(string rawContent)
        {
            RawContent = rawContent;
            _point = Point.Parse(rawContent);
        }

        public Point Point
        {
            get { return _point; }
            set { _point = value; }
        }
    }
}
