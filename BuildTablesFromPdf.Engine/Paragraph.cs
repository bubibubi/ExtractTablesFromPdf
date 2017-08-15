using System;
using System.Diagnostics;

namespace BuildTablesFromPdf.Engine
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Paragraph : IPageContent, IFormattable
    {
        public Paragraph(double y)
        {
            Y = y;
        }

        public double Y { get; private set; }

        public string Content { get; set; }

        public void AddText(Point point, string content)
        {
            if (!Contains(point))
                throw new InvalidOperationException("The point is not on the paragraph");

            if (string.IsNullOrEmpty(Content))
                Content = content;
            else
                Content += " " + content;
        }

        public bool Contains(Point point)
        {
            return Y - ContentExtractor.Tolerance < point.Y && point.Y < Y + ContentExtractor.Tolerance * 3;
        }

        public bool Contains(double y)
        {
            return Y - ContentExtractor.Tolerance < y && y < Y + ContentExtractor.Tolerance * 3;
        }

        #region IFormattable

        // ReSharper disable once UnusedMember.Local
        private string DebuggerDisplay
        {
            get { return ToString("d"); }
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case "s":
                case "":
                case null:
                    return Content;
                case "d":
                    return string.Format("{0} {1}", Y, Content);
                default:
                    throw new FormatException();
            }
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }

        #endregion

    }
}
