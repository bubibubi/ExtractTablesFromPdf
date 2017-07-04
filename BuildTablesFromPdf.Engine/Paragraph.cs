using System;
using System.Diagnostics;

namespace BuildTablesFromPdf.Engine
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Paragraph : IPageContent, IFormattable
    {
        public Paragraph(float y)
        {
            Y = y;
        }

        public float Y { get; private set; }

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
            return Y - Line.Tolerance < point.Y && point.Y < Y + Line.Tolerance * 3;
        }

        public bool Contains(float y)
        {
            return Y - Line.Tolerance < y && y < Y + Line.Tolerance * 3;
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
