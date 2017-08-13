using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTablesFromPdf.Engine.Statements;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    public class ExtractText
    {

        public static bool ShowParserInfo = false;
        public static bool IgnoreWhiteLines = true;

        public static PageCollection Read(string filePath)
        {

            var pdfReader = new PdfReader(filePath);
            var pages = new PageCollection();

            pages.Errors = new List<string>();
            pages.PdfReader = pdfReader;

            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {

                if (ShowParserInfo)
                    Console.WriteLine("Page {0} ========================================================", i + 1);

                var page = new Page();
                page.Index = pages.Count;
                MultiLineStatement currentMultilineStatement = null;

                GraphicState graphicState = new GraphicState();
                graphicState.TransformMatrix = Matrix.Identity;
                graphicState.Color = Color.White;
                Stack<GraphicState> graphicStateStack = new Stack<GraphicState>();

                Point currentPoint = new Point(0, 0);

                page.Rotation = pdfReader.GetPageRotation(i + 1);

                string rawPdfContent = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(i + 1)));
                int pointer = 0;
                
                string statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
                while (statement != null)
                {
                    if (statement == "BT")
                    {
                        currentMultilineStatement = new TextObjectStatement(pdfReader, i, graphicState.TransformMatrix);
                        page.Statements.Add(currentMultilineStatement);
                    }
                    else if (statement == "ET")
                    {
                        if (!(currentMultilineStatement is TextObjectStatement))
                            pages.Errors.Add("ET outside a text object");
                        else
                        {
                            ((TextObjectStatement)currentMultilineStatement).CloseMultiLineStatement();
                            currentMultilineStatement = null;
                        }
                    }
                    else if (currentMultilineStatement != null)
                    {
                        currentMultilineStatement.RawContent.Add(statement);
                    }
                    else if (statement == "q")
                    {
                        graphicStateStack.Push(graphicState.Clone());
                    }
                    else if (statement == "Q")
                    {
                        graphicState = graphicStateStack.Pop();
                    }
                    else if (statement.EndsWith(" cm"))
                    {
                        Matrix newTransformMatrix;
                        if (!Matrix.TryParse(statement, out newTransformMatrix))
                            newTransformMatrix = Matrix.Identity;
                        graphicState.TransformMatrix *= newTransformMatrix;
                    }
                    else if (statement.EndsWith(" J"))
                    {
                        page.Statements.Add(new LineCapStyleStatement(statement));
                    }
                    else if (statement.EndsWith(" j"))
                    {
                        page.Statements.Add(new LineJoinStyleStatement(statement));
                    }
                    else if (statement.EndsWith(" rg"))
                    {
                        graphicState.Color = new NonStrokingColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" RG"))
                    {
                        graphicState.Color = new StrokingColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" G"))
                    {
                        graphicState.Color = new GreyColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" g"))
                    {
                        graphicState.Color = new GreyColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" m"))
                    {
                        currentPoint = graphicState.TransformMatrix.TransformPoint(new SetPointStatement(statement).Point).Rotate(page.Rotation);
                    }
                    else if (statement.EndsWith(" l"))
                    {
                        var lineToStatement = new LineToStatement(statement);
                        var destinationPoint = graphicState.TransformMatrix.TransformPoint(lineToStatement.Point).Rotate(page.Rotation);
                        if (currentPoint != destinationPoint)
                        {
                            page.AllLines.Add(new Line(currentPoint, destinationPoint));
                            currentPoint = destinationPoint;
                        }
                    }
                    else if (statement.EndsWith(" c"))
                    {
                        var bezierCurveStatement = new BezierCurveStatement(statement);
                        currentPoint = graphicState.TransformMatrix.TransformPoint(bezierCurveStatement.ToPoint).Rotate(page.Rotation);
                    }
                    else if (statement.EndsWith(" d"))
                    {
                        page.Statements.Add(new SetLineDashPatternStatement(statement));
                    }
                    else if (statement.EndsWith(" w"))
                    {
                        page.Statements.Add(new SetLineWidthStatement(statement));
                    }
                    else if (statement.EndsWith(" re"))
                    {
                        var lines =
                            new RectangleStatement(statement)
                            .GetLines()
                            .Where(_ => graphicState.TransformMatrix.TransformPoint(_.StartPoint) != graphicState.TransformMatrix.TransformPoint(_.EndPoint))
                            .Select(_ => graphicState.TransformMatrix.TransformLine(_).Rotate(page.Rotation))
                            ;
                        if (!IgnoreWhiteLines || !graphicState.Color.IsWhite())
                            page.AllLines.AddRange(lines);
                        else
                            Console.WriteLine("Ignored rectangle");
                    }
                    else if (statement == "S")
                    {
                        page.Statements.Add(StrokePathStatement.Value);
                    }
                    else if (statement == "s")
                    {
                        page.Statements.Add(CloseStrokePathStatement.Value);
                    }
                    else if (statement == "f")
                    {
                        page.Statements.Add(FillPathStatement.Value);
                    }
                    else
                    {
                        if (ShowParserInfo)
                            Console.WriteLine(statement);
                    }

                    statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
                }

                page.DeleteWrongLines();

                pages.Add(page);
            }

            return pages;
        }
    }
}