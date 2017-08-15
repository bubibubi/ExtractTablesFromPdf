using System;
using System.Collections.Generic;
using System.Linq;
using BuildTablesFromPdf.Engine.Statements;
using BuildTablesFromPdf.Engine.Tables;

namespace BuildTablesFromPdf.Engine
{
    /// <summary>
    /// A Pdf page
    /// </summary>
    public class Page
    {
        public Page()
        {
            Statements = new List<Statement>();
            Tables = new List<Table>();
            AllLines = new List<Line>();
        }

        public int Index { get; set; }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <value>
        /// The statements.
        /// </value>
        public List<Statement> Statements { get; private set; }

        public List<Line> AllLines { get; private set; }

        public List<Line> JoinedHorizontalLines { get; set; }

        public List<Line> JoinedVerticalLines { get; set; }

        public List<Line> JoinedLines { get; set; }

        public int Rotation { get; set; }

        /// <summary>
        /// Gets or sets the table structures.
        /// </summary>
        /// <value>
        /// The table structures.
        /// </value>
        public List<Table> Tables { get; set; }

        public List<Paragraph> Paragraphs { get; set; }

        public List<IPageContent> Contents { get; set; }
        
        public bool IsRefreshed { get { return JoinedLines != null; } }


        public void DeleteWrongLines()
        {
            // ReSharper disable ImpureMethodCallOnReadonlyValueField
            AllLines = AllLines.Where(_ => _.StartPoint.IsValid() && _.EndPoint.IsValid()).ToList();
            // ReSharper restore ImpureMethodCallOnReadonlyValueField
        }


        /// <summary>
        /// Determines the table structures.
        /// </summary>
        public void DetermineTableStructures()
        {
            JoinedLines = JoinLines(AllLines);

            // Find table borders
            foreach (Line horizontalLine in JoinedHorizontalLines.OrderBy(_ => _.StartPoint.Y))
            {
                // We consider that this line is a top line of a table if
                // 1. There is not a table with this line inside
                // 2. There is a vertical line starting from this line

                if (Tables.Any(_ => _.Contains(horizontalLine.StartPoint.Y)))
                    continue;

                Line? tableLine = JoinedVerticalLines
                    .Where(_ => _.StartPoint == horizontalLine.StartPoint || _.StartPoint == horizontalLine.EndPoint)
                    .OrderByDescending(_ => _.EndPoint.Y - _.StartPoint.Y)
                    .Cast<Line?>()
                    .FirstOrDefault();

                if (tableLine == null)
                    continue;

                Table tableStructure = new Table()
                {
                    TopLeftPoint = horizontalLine.StartPoint,
                    BottomRightPoint = new Point(horizontalLine.EndPoint.X, tableLine.Value.EndPoint.Y)
                };

                Tables.Add(tableStructure);
            }

            // Add the first row and the first column to all tables
            foreach (Table tableStructure in Tables)
            {
                tableStructure.Rows.Add(new Row(){BeginY = tableStructure.TopLeftPoint.Y});
                tableStructure.Columns.Add(new Column(){BeginX = tableStructure.TopLeftPoint.X});
            }

            // Find rows
            foreach (Line horizontalLine in JoinedHorizontalLines.OrderBy(_ => _.StartPoint.Y))
            {
                var tableStructure = Tables.FirstOrDefault(_ => _.Contains(horizontalLine));
                // No table contains this line
                if (tableStructure == null)
                    continue;

                // Check if the row already belongs to the table
                if (tableStructure.Rows.Any(_ => Math.Abs(_.BeginY - horizontalLine.StartPoint.Y) < ContentExtractor.Tolerance))
                    continue;

                // Check if the row is the bottom edge of the table
                if (tableStructure.BottomRightPoint.Y - horizontalLine.StartPoint.Y < ContentExtractor.Tolerance)
                    continue;

                tableStructure.Rows.Add(new Row() {BeginY = horizontalLine.StartPoint.Y});
            }

            // Find columns
            foreach (Line verticalLine in JoinedVerticalLines.OrderBy(_ => _.StartPoint.X))
            {
                var tableStructure = Tables.FirstOrDefault(_ => _.Contains(verticalLine));
                // No table contains this line
                if (tableStructure == null)
                    continue;

                // The row already belongs to the table
                if (tableStructure.Columns.Any(_ => Math.Abs(_.BeginX - verticalLine.StartPoint.X) < ContentExtractor.Tolerance))
                    continue;

                // Check if the row is the bottom edge of the table
                if (tableStructure.BottomRightPoint.X - verticalLine.StartPoint.X < ContentExtractor.Tolerance)
                    continue;


                tableStructure.Columns.Add(new Column() { BeginX = verticalLine.StartPoint.X });
            }


            // Fix EndX and EndY and indexes
            foreach (Table tableStructure in Tables)
            {
                // Fix EndYs
                for (int i = 0; i < tableStructure.Rows.Count - 1; i++)
                    tableStructure.Rows[i].EndY = tableStructure.Rows[i + 1].BeginY - ContentExtractor.Tolerance * 0.1f;

                tableStructure.Rows[tableStructure.Rows.Count - 1].EndY = tableStructure.BottomRightPoint.Y;


                // Fix EndXs
                for (int i = 0; i < tableStructure.Columns.Count - 1; i++)
                    tableStructure.Columns[i].EndX = tableStructure.Columns[i + 1].BeginX - ContentExtractor.Tolerance * 0.1f;

                tableStructure.Columns[tableStructure.Columns.Count - 1].EndX = tableStructure.BottomRightPoint.X;

                int index;

                index = 0;
                foreach (var column in tableStructure.Columns.OrderBy(_ => _.BeginX))
                {
                    column.Index = index;
                    index++;
                }

                index = 0;
                foreach (var row in tableStructure.Rows.OrderByDescending(_ => _.BeginY))
                {
                    row.Index = index;
                    index++;
                }

                tableStructure.CreateContent();

            }

        }


        /// <summary>
        /// Joins the horizontal and vertical lines.
        /// </summary>
        /// <param name="allLines">All the lines.</param>
        /// <returns>The orizontal and the vertical lines (eventually joined)</returns>
        private List<Line> JoinLines(List<Line> allLines)
        {
            JoinedVerticalLines =  JoinVerticalLines(allLines);
            JoinedHorizontalLines = JoinHorizontalLines(allLines);

            return JoinedHorizontalLines.Union(JoinedVerticalLines).ToList();
        }

        /// <summary>
        /// Joins the vertical lines.
        /// </summary>
        /// <param name="allLines">All lines.</param>
        /// <returns>The vertical lines (eventually joined)</returns>
        private static List<Line> JoinVerticalLines(List<Line> allLines)
        {
            var lines = new List<Line>();

            var verticalLines = allLines.Where(_ => _.IsVertical()).OrderBy(_ => _.StartPoint.X).ThenBy(_ => _.StartPoint.Y).ToList();

            foreach (Line verticalLine in verticalLines)
            {
                if (lines.Count == 0)
                    lines.Add(verticalLine);
                else if (verticalLine.IsCoincident(lines[lines.Count - 1]))
                    continue;
                else if (verticalLine.IsOverlapped(lines[lines.Count - 1]))
                {
                    var joinedLine = lines[lines.Count - 1].Join(verticalLine);
                    lines.RemoveAt(lines.Count - 1);
                    lines.Add(joinedLine);
                }
                else
                    lines.Add(verticalLine);
            }

            return lines;
        }

        private static List<Line> JoinHorizontalLines(List<Line> allLines)
        {
            var lines = new List<Line>();

            var horizontalLines = allLines.Where(_ => _.IsHorizontal()).OrderBy(_ => _.StartPoint.Y).ThenBy(_ => _.StartPoint.X).ToList();

            foreach (Line horizontalLine in horizontalLines)
            {
                if (lines.Count == 0)
                    lines.Add(horizontalLine);
                else if (horizontalLine.IsCoincident(lines[lines.Count - 1]))
                    continue;
                else if (horizontalLine.IsOverlapped(lines[lines.Count - 1]))
                {
                    var joinedLine = horizontalLine.Join(lines[lines.Count - 1]);
                    lines.RemoveAt(lines.Count - 1);
                    lines.Add(joinedLine);
                }
                else
                    lines.Add(horizontalLine);
            }

            return lines;
        }


        public void DetermineParagraphs()
        {
            Paragraphs = new List<Paragraph>();

            var textObjectStatementLines = Statements.Where(_ => _ is TextObjectStatement).Cast<TextObjectStatement>().SelectMany(_ => _.Lines)
                .Where(_ => !string.IsNullOrWhiteSpace(_.Content))
                .Where(_ => !Tables.Any(t => t.Contains(_.Position.Y)))
                .OrderBy(_ => _.Position.Y);

            foreach (var line in textObjectStatementLines)
            {
                if (!Paragraphs.Any(t => t.Contains(line.Position)))
                    Paragraphs.Add(new Paragraph(line.Position.Y));
            }
        }


        public void FillContent()
        {
            Contents = new List<IPageContent>();
            Contents.AddRange(Paragraphs.Cast<IPageContent>().Union(Tables).OrderBy(_ => _.Y));

            var textObjectStatementsLines = Statements.Where(_ => _ is TextObjectStatement).Cast<TextObjectStatement>().SelectMany(_ => _.Lines)
                .Where(_ => !string.IsNullOrWhiteSpace(_.Content))
                .OrderBy(_ => _.Position.Y).ThenBy(_ => _.Position.X);

            foreach (var line in textObjectStatementsLines.Where(_ => _.Position.IsValid()))
            {
                IPageContent targetPageContent = Contents.First(_ => _.Contains(line.Position.Y));
                targetPageContent.AddText(line.Position, line.Content);
            }
        }

        public override string ToString()
        {
            string pageContent = string.Empty;
            if (Contents != null)
            {
                foreach (IPageContent content in Contents)
                    pageContent += string.Format("{0}\r\n", content);
            }
            return pageContent;
        }

    }
}