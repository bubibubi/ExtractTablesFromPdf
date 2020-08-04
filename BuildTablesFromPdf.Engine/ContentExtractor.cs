using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BuildTablesFromPdf.Engine.CMap;
using BuildTablesFromPdf.Engine.Statements;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    public class ContentExtractor
    {

        public static bool ShowParserInfo = false;
        public static bool IgnoreWhiteLines = true;

        /// <summary>
        /// The tolerance. This parameter is used to determine same line/points
        /// Decrease this value if you need to discover more table cells/paragraphs
        /// Increase this value if you need to discover less table cells/paragraphs
        /// Often the right parameter is determined by the line boldness. Bold lines, 
        /// in pdf files, are box filled
        /// </summary>
        public static float Tolerance = 2f;


        public static PageCollection ReadPdfFileAndRefreshContent(string fileName)
        {
            PageCollection pages = ContentExtractor.Read(fileName);
            foreach (Page page in pages)
            {
                page.DetermineTableStructures();
                page.DetermineParagraphs();

                page.FillContent();
            }
            return pages;
        }


        public static PageCollection Read(string filePath)
        {

            var pdfReader = new PdfReader(filePath);
            var pages = new PageCollection();

            pages.Errors = new List<string>();
            pages.PdfReader = pdfReader;

            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {

                if (ShowParserInfo)
                    Console.WriteLine("Page {0} === ({1}, {2}, {3}, {4}) rotated of {5} ======================================================", i + 1, pdfReader.GetPageSize(i + 1).Top, pdfReader.GetPageSize(i + 1).Left, pdfReader.GetPageSize(i + 1).Bottom, pdfReader.GetPageSize(i + 1).Right, pdfReader.GetPageRotation(i + 1));

                var page = new Page();
                page.Index = pages.Count;
                MultiLineStatement currentMultilineStatement = null;
                Point textPosition = new Point();
                TextObjectStatementLine actualLineSettings = null;

                GraphicState graphicState = new GraphicState();
                graphicState.TransformMatrix = Matrix.Identity;
                graphicState.Color = Color.White;
                Stack<GraphicState> graphicStateStack = new Stack<GraphicState>();

                Matrix textTransformMatrix = Matrix.Identity;
                Matrix baseTextTransformMatrix = Matrix.Identity;

                FontInfo actualFont = new FontInfo();

                float leadingParameter = 0;

                Point currentPoint = new Point(0, 0);

                page.Rotation = pdfReader.GetPageRotation(i + 1);

                string rawPdfContent = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(i + 1)));
                int pointer = 0;

                string previousStatement = null;
                string statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
                while (statement != null)
                {
                    

                    // Embedded image
                    if (statement.EndsWith("BI"))
                    {
                        pointer = rawPdfContent.IndexOf("\nEI", pointer, StringComparison.Ordinal);
                    }
                    else if (statement.Trim() == "BT")
                    {
                        baseTextTransformMatrix = graphicState.TransformMatrix;
                        currentMultilineStatement = new TextObjectStatement(pdfReader, i + 1, graphicState.TransformMatrix);
                        page.Statements.Add(currentMultilineStatement);

                        actualLineSettings = new TextObjectStatementLine();
                        textTransformMatrix = Matrix.Identity;
                        textPosition = new Point();
                        leadingParameter = 0;


                    }

                    else if (statement.EndsWith("Tm"))
                    {
                        Matrix matrix;
                        if (Matrix.TryParse(statement, out matrix))
                            textTransformMatrix = matrix;
                    }
                    else if (statement.EndsWith("Tf"))
                    {
                        string[] fontParameters = statement.Split(' ');
                        if (fontParameters.Length < 3)
                        {
                            // Try to retrieve from previous line. This is global a parsing issue
                            if (string.IsNullOrWhiteSpace(previousStatement))
                                continue;

                            fontParameters = (previousStatement.Trim() + " " + statement.Trim()).Split(' ');
                            if (fontParameters.Length < 3)
                                continue;
                        }
                        float fontSize;
                        actualFont = new FontInfo();
                        if (float.TryParse(fontParameters[fontParameters.Length - 2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out fontSize))
                            actualFont.FontHeight = fontSize;
                        string fontKey = fontParameters[fontParameters.Length - 3];
                        if (!string.IsNullOrWhiteSpace(fontKey))
                        {
                            actualFont.CMapToUnicode = PdfFontHelper.GetFontCMapToUnicode(pdfReader, i + 1, fontKey);
                            actualFont.EncodingDifferenceToUnicode = EncodingDifferenceToUnicode.Parse(PdfFontHelper.GetFont(pdfReader, i + 1, fontKey));
                        }
                    }
                    else if (statement.EndsWith("Td"))
                    {
                        float tx;
                        float ty;
                        string[] parameters = statement.Split(' ');
                        if (
                            float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tx) &&
                            float.TryParse(parameters[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ty))
                            textTransformMatrix = new Matrix(1, 0, 0, 1, tx, ty) * textTransformMatrix;
                    }
                    else if (statement.EndsWith("TD"))
                    {
                        float tx;
                        float ty;
                        string[] parameters = statement.Split(' ');
                        if (
                            float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tx) &&
                            float.TryParse(parameters[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ty))
                        {
                            textTransformMatrix = new Matrix(1, 0, 0, 1, tx, ty) * textTransformMatrix;
                            leadingParameter = -ty;
                        }
                    }
                    else if (statement.EndsWith("TL"))
                    {
                        float tl;
                        string[] parameters = statement.Split(' ');
                        if (
                            float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tl))
                            leadingParameter = tl;
                    }
                    else if (statement.EndsWith("T*"))
                    {
                        textTransformMatrix = new Matrix(1, 0, 0, 1, 0, -leadingParameter) * textTransformMatrix;
                    }
                    else if (statement.EndsWith("TJ"))
                    {
                        actualLineSettings.Font = actualFont;
                        
                        string content = TextObjectStatement.GetTJContent(statement, actualLineSettings.Font.CMapToUnicode, actualLineSettings.Font.EncodingDifferenceToUnicode);
                        if (string.IsNullOrEmpty(content))
                            continue;
                        var line = actualLineSettings.Clone();
                        line.FontHeight =
                            line.Font.FontHeight * textTransformMatrix.a * (page.Rotation == 90 || page.Rotation == 270 ? baseTextTransformMatrix.b : baseTextTransformMatrix.a);
                        line.Position = baseTextTransformMatrix.TransformPoint(new Point(textTransformMatrix.TransformX(textPosition.X, textPosition.Y + line.Font.FontHeight), textTransformMatrix.TransformY(textPosition.X, textPosition.Y + line.Font.FontHeight))).Rotate(page.Rotation);
                        line.Content = content;
                        ((TextObjectStatement)currentMultilineStatement).Lines.Add(line);
                    }
                    else if (statement.Trim().EndsWith("Tj"))
                    {
                        actualLineSettings.Font = actualFont;

                        string escapedContent;
                        escapedContent = statement.Trim();
                        escapedContent = escapedContent.Remove(escapedContent.Length - 2);
                        string content = PdfHexStringDataType.IsStartChar(escapedContent) ? PdfHexStringDataType.GetContent(escapedContent) : PdfStringDataType.GetContentFromEscapedContent(escapedContent);

                        var line = actualLineSettings.Clone();
                        line.FontHeight =
                            line.Font.FontHeight * textTransformMatrix.a * (page.Rotation == 90 || page.Rotation == 270 ? baseTextTransformMatrix.b : baseTextTransformMatrix.a);
                        line.Position = baseTextTransformMatrix.TransformPoint(new Point(textTransformMatrix.TransformX(textPosition.X, textPosition.Y + line.Font.FontHeight), textTransformMatrix.TransformY(textPosition.X, textPosition.Y + line.Font.FontHeight))).Rotate(page.Rotation);
                        line.Content = PdfFontHelper.ToUnicode(content, line.Font.CMapToUnicode, line.Font.EncodingDifferenceToUnicode);
                        ((TextObjectStatement)currentMultilineStatement).Lines.Add(line);
                    }
                    else if (statement.Trim() == "ET")
                    {
                        if (!(currentMultilineStatement is TextObjectStatement))
                            pages.Errors.Add("ET outside a text object");
                        else
                        {
                            ((TextObjectStatement)currentMultilineStatement).CloseMultiLineStatement();
                            currentMultilineStatement = null;
                        }
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
                    else if (statement.EndsWith(" SCN"))
                    {
                        graphicState.Color = new StrokingColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" scn"))
                    {
                        graphicState.Color = new NonStrokingColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" SC"))
                    {
                        graphicState.Color = new StrokingColorStatement(statement).Color;
                    }
                    else if (statement.EndsWith(" sc"))
                    {
                        graphicState.Color = new NonStrokingColorStatement(statement).Color;
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
                            if (!IgnoreWhiteLines || !graphicState.Color.IsWhite())
                                page.AllLines.Add(new Line(currentPoint, destinationPoint));
                            else
                            {
                                if (ShowParserInfo)
                                    Console.WriteLine("Ignored rectangle");
                            }
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
                        // ReSharper disable AccessToModifiedClosure
                        var lines =
                            new RectangleStatement(statement)
                            .GetLines()
                            .Where(_ => graphicState.TransformMatrix.TransformPoint(_.StartPoint) != graphicState.TransformMatrix.TransformPoint(_.EndPoint))
                            .Select(_ => graphicState.TransformMatrix.TransformLine(_).Rotate(page.Rotation))
                            ;
                        // ReSharper restore AccessToModifiedClosure
                        if (!IgnoreWhiteLines || !graphicState.Color.IsWhite())
                            page.AllLines.AddRange(lines);
                        else
                        {
                            if (ShowParserInfo)
                                Console.WriteLine("Ignored rectangle");
                        }
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
                    else if (currentMultilineStatement != null)
                    {
                        currentMultilineStatement.RawContent.Add(statement);
                    }
                    else
                    {
                        if (ShowParserInfo)
                            Console.WriteLine(statement);
                    }

                    previousStatement = statement;
                    statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
                }

                page.DeleteWrongLines();

                pages.Add(page);
            }

            return pages;
        }

    }
}