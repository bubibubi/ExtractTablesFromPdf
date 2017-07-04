using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuildTablesFromPdf.Engine.Tables
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Table : IPageContent, IFormattable
    {
        public Table()
        {
            Rows = new List<Row>();
            Columns = new List<Column>();
        }

        public Point TopLeftPoint { get; set; }
        public Point BottomRightPoint { get; set; }

        public List<Row> Rows { get; private set; }
        public List<Column> Columns { get; private set; }

        public float Width { get { return BottomRightPoint.X - TopLeftPoint.X; } }
        public float Heigth { get { return BottomRightPoint.Y - TopLeftPoint.Y; }}

        public string[,] Content { get; private set; }

        public bool Contains(Line line)
        {
            return
                TopLeftPoint.Y - Line.Tolerance <= line.StartPoint.Y &&
                line.EndPoint.Y <= BottomRightPoint.Y + Line.Tolerance
                &&
                TopLeftPoint.X - Line.Tolerance <= line.StartPoint.X &&
                line.EndPoint.X <= BottomRightPoint.X + Line.Tolerance;
        }

        public bool Contains(float y)
        {
            return
                TopLeftPoint.Y - Line.Tolerance <= y &&
                y <= BottomRightPoint.Y + Line.Tolerance;
        }

        public bool Contains(Point point)
        {
            return
                TopLeftPoint.Y - Line.Tolerance <= point.Y &&
                point.Y <= BottomRightPoint.Y - Line.Tolerance
                &&
                TopLeftPoint.X - Line.Tolerance <= point.X &&
                point.X <= BottomRightPoint.X - Line.Tolerance;
        }

        internal void CreateContent()
        {
            Content = new string[Rows.Count, Columns.Count + 2];
        }

        public void AddText(Point point, string content)
        {
            if (Content == null)
                throw new InvalidOperationException("Content is not initialized. Please call CreateContent first");

            // The text can be also on the left or on the right of the table
            Row row = FindRow(point.Y);
            if (row == null)
                throw new InvalidOperationException("The point is outside the table");

            int columnIndex = FindColumnIndex(point.X);
            int rowIndex = Rows.Count - row.Index - 1;

            if (string.IsNullOrEmpty(Content[row.Index, columnIndex]))
                Content[rowIndex, columnIndex] = content;
            else
                Content[rowIndex, columnIndex] += " " + content;
        }

        /// <summary>
        /// Finds the index of the column of the x coordinate.
        /// If x is on the left of the table, 0 is returned
        /// If x is on the right of the table, Count is returned
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <returns>The column</returns>
        private int FindColumnIndex(float x)
        {
            if (x < TopLeftPoint.X)
                return 0;

            if ( BottomRightPoint.X < x)
                return Columns.Count + 1;

            var column = Columns.Single(_ => _.BeginX <= x && x <= _.EndX);

            return column.Index + 1;
        }

        /// <summary>
        /// Finds the row corresponding to the y coordinate.
        /// Null if y is outside the table.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>The row or null if y is outside the table</returns>
        private Row FindRow(float y)
        {
            return Rows.FirstOrDefault(_ => _.BeginY <= y && y <= _.EndY);
        }

        float IPageContent.Y { get { return TopLeftPoint.Y; }}

        #region IFormattable

        private string DebuggerDisplay { get { return ToString("d"); }}

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
                    if (Content == null)
                        return "";
                    string content = "";
                    for (int i = 0; i < Content.GetLength(0); i++)
                    {
                        for (int j = 0; j < Content.GetLength(1); j++)
                        {
                            if (j == 0)
                                content += Content[i, j];
                            else
                                content += " | " + Content[i, j];
                        }
                        content += "\r\n";
                    }
                    return content;
                case "d":
                    return string.Format("{0} - {1}; Rows = {2}, Columns = {3}", TopLeftPoint, BottomRightPoint, Rows.Count, Columns.Count);
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