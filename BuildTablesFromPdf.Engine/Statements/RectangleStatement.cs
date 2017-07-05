using System.Collections.Generic;
using System.Globalization;

namespace BuildTablesFromPdf.Engine.Statements
{
    internal class RectangleStatement : SingleLineStatement
    {
        public RectangleStatement(string rawContent)
        {
            RawContent = rawContent;
            Corner = Point.Parse(rawContent);
            Width = float.Parse(rawContent.Split(' ')[2], NumberFormatInfo.InvariantInfo);
            Height = float.Parse(rawContent.Split(' ')[3], NumberFormatInfo.InvariantInfo);
        }

        public readonly Point Corner;
        public readonly float Width;
        public readonly float Height;

        public List<Line> GetLines()
        {
            var lines = new List<Line>();
            if (Corner != new Point(Corner.X + Width, Corner.Y))
                lines.Add(new Line(Corner, new Point(Corner.X + Width, Corner.Y)));
            if (new Point(Corner.X + Width, Corner.Y) != new Point(Corner.X + Width, Corner.Y + Height))
                lines.Add(new Line(new Point(Corner.X + Width, Corner.Y), new Point(Corner.X + Width, Corner.Y + Height)));
            if (new Point(Corner.X , Corner.Y + Height) != new Point(Corner.X + Width, Corner.Y + Height))
                lines.Add(new Line(new Point(Corner.X , Corner.Y + Height), new Point(Corner.X + Width, Corner.Y + Height)));
            if (Corner != new Point(Corner.X, Corner.Y + Height))
                lines.Add(new Line(Corner, new Point(Corner.X, Corner.Y + Height)));
            return lines;
        }
    }
}