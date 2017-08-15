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

        public double Width { get { return BottomRightPoint.X - TopLeftPoint.X; } }
        public double Heigth { get { return BottomRightPoint.Y - TopLeftPoint.Y; } }

        private string[,] _Content;

        public string this[int row, int column]
        {
            get { return _Content[row, column]; }
            set { _Content[row, column] = value; }
        }

        public string this[int row, string columnName]
        {
            get
            {
                return _Content[row, GetColumnIndex(columnName)];
            }
            set
            {
                _Content[row, GetColumnIndex(columnName)] = value;
            }
        }

        public bool ColumnExists(string columnName)
        {
            if (columnName == "<" || columnName == ">")
                return true;

            for (int i = 1; i < _Content.GetLength(1) - 1; i++)
            {
                if (String.Equals(_Content[0, i].Trim(), columnName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public string GetValueOrNull(int row, string columnName)
        {
            if (!ColumnExists(columnName))
                return null;
            return this[row, GetColumnIndex(columnName)];
        }

        private int GetColumnIndex(string columnName)
        {
            if (columnName == "<")
                return 0;

            if (columnName == ">")
                return _Content.GetLength(1) - 1;

            for (int i = 1; i < _Content.GetLength(1) - 1; i++)
            {
                if (String.Equals(_Content[0, i].Trim(), columnName, StringComparison.CurrentCultureIgnoreCase))
                    return i;
            }

            throw new ArgumentException(string.Format("Column '{0}' not found", columnName), "columnName");

        }


        public bool Contains(Line line)
        {
            return
                TopLeftPoint.Y - ContentExtractor.Tolerance <= line.StartPoint.Y &&
                line.EndPoint.Y <= BottomRightPoint.Y + ContentExtractor.Tolerance
                &&
                TopLeftPoint.X - ContentExtractor.Tolerance <= line.StartPoint.X &&
                line.EndPoint.X <= BottomRightPoint.X + ContentExtractor.Tolerance;
        }

        public bool Contains(double y)
        {
            return
                TopLeftPoint.Y - ContentExtractor.Tolerance <= y &&
                y <= BottomRightPoint.Y + ContentExtractor.Tolerance;
        }

        public bool Contains(Point point)
        {
            return
                TopLeftPoint.Y - ContentExtractor.Tolerance <= point.Y &&
                point.Y <= BottomRightPoint.Y - ContentExtractor.Tolerance
                &&
                TopLeftPoint.X - ContentExtractor.Tolerance <= point.X &&
                point.X <= BottomRightPoint.X - ContentExtractor.Tolerance;
        }

        internal void CreateContent()
        {
            _Content = new string[Rows.Count, Columns.Count + 2];
        }

        public void AddText(Point point, string content)
        {
            if (_Content == null)
                throw new InvalidOperationException("Content is not initialized. Please call CreateContent first");

            // The text can be also on the left or on the right of the table
            Row row = FindRow(point.Y);
            if (row == null)
                throw new InvalidOperationException("The point is outside the table");

            int columnIndex = FindColumnIndex(point.X);
            int rowIndex = Rows.Count - row.Index - 1;

            if (string.IsNullOrEmpty(_Content[rowIndex, columnIndex]))
                _Content[rowIndex, columnIndex] = content;
            else if (_Content[rowIndex, columnIndex].EndsWith(" "))
                _Content[rowIndex, columnIndex] += content;
            else
                _Content[rowIndex, columnIndex] += " " + content;
        }

        /// <summary>
        /// Finds the index of the column of the x coordinate.
        /// If x is on the left of the table, 0 is returned
        /// If x is on the right of the table, Count is returned
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <returns>The column</returns>
        private int FindColumnIndex(double x)
        {
            if (x < TopLeftPoint.X)
                return 0;

            if (BottomRightPoint.X < x)
                return Columns.Count + 1;

            Column column = Columns.SingleOrDefault(_ => _.BeginX <= x && x <= _.EndX);

            if (column == null)
                column = Columns.OrderBy(_ => _.Index).Last(_ => x <= _.EndX);

            return column.Index + 1;
        }

        /// <summary>
        /// Finds the row corresponding to the y coordinate.
        /// Null if y is outside the table.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>The row or null if y is outside the table</returns>
        private Row FindRow(double y)
        {
            Row row = Rows.FirstOrDefault(_ => _.BeginY <= y && y <= _.EndY);
            if (row == null)
                row = Rows.FirstOrDefault(_ => _.BeginY - ContentExtractor.Tolerance <= y && y <= _.EndY + ContentExtractor.Tolerance);
            return row;
        }

        double IPageContent.Y { get { return TopLeftPoint.Y; } }

        #region IFormattable

        private string DebuggerDisplay { get { return ToString("d"); } }

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
                    if (_Content == null)
                        return "";
                    string content = "";
                    for (int i = 0; i < _Content.GetLength(0); i++)
                    {
                        for (int j = 0; j < _Content.GetLength(1); j++)
                        {
                            if (j == 0)
                                content += _Content[i, j];
                            else
                                content += " | " + _Content[i, j];
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