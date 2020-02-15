using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuildTablesFromPdf.Engine
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Paragraph : IPageContent, IFormattable
    {
        private List<ParagraphContent> _Contents = new List<ParagraphContent>();

        public Paragraph(double y)
        {

            Y = y;
        }

        public double Y { get; private set; }

        public string Content
        {
            get
            {
                string result = null;
                foreach (ParagraphContent content in _Contents.OrderBy(_ => _.Point.X))
                {
                    if (result == null)
                        result = content.Content;
                    else
                        result = result + " " + content.Content;
                }

                return result;
            }

        }

        public void AddText(Point point, string content)
        {
            if (!Contains(point))
                throw new InvalidOperationException("The point is not on the paragraph");

            _Contents.Add(new ParagraphContent(point, content));
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


        private class ParagraphContent
        {
            public ParagraphContent(Point point, string content)
            {
                Point = point;
                Content = content;
            }

            public Point Point { get; private set; }
            public string Content { get; private set; }
        }

    }

}
