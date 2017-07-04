using System;
using System.Collections.Generic;
using System.Text;
using BuildTablesFromPdf.Engine.Statements;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    public class ExtractText
    {
        public static PageCollection Read(string filePath)
        {

            var pdfReader = new PdfReader(filePath);
            var pages = new PageCollection();

            pages.Errors = new List<string>();
            pages.PdfReader = pdfReader;

            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {

                Console.WriteLine("Page {0} ========================================================", i + 1);

                var page = new Page();
                page.Index = pages.Count;
                MultiLineStatement currentMultilineStatement = null;

                Matrix transformMatrix = Matrix.Identity;

                string textFromPage = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(i + 1)));
                var lines = textFromPage.Split(new[] {"\n"}, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (line == "BT")
                    {
                        currentMultilineStatement = new TextObjectStatement();
                        page.Statements.Add(currentMultilineStatement);
                    }
                    else if (line == "ET")
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
                        currentMultilineStatement.RawContent.Add(line);
                    }
                    else if (line == "q")
                    {
                        page.Statements.Add(PushGraphicStateStatement.Value);
                    }
                    else if (line == "Q")
                    {
                        page.Statements.Add(PopGraphicStateStatement.Value);
                    }
                    else if (line.EndsWith(" cm"))
                    {
                        page.Statements.Add(new ModifyMatrixStatement(line));
                        Matrix newTransformMatrix;
                        if (!Matrix.TryParse(line, out newTransformMatrix))
                            newTransformMatrix = Matrix.Identity;
                        transformMatrix *= newTransformMatrix;
                    }
                    else if (line.EndsWith(" J"))
                    {
                        page.Statements.Add(new LineCapStyleStatement(line));
                    }
                    else if (line.EndsWith(" j"))
                    {
                        page.Statements.Add(new LineJoinStyleStatement(line));
                    }
                    else if (line.EndsWith(" rg"))
                    {
                        page.Statements.Add(new NonStrokingColorStatement(line));
                    }
                    else if (line.EndsWith(" RG"))
                    {
                        page.Statements.Add(new StrokingColorStatement(line));
                    }
                    else if (line.EndsWith(" G"))
                    {
                        page.Statements.Add(new GreyColorStatement(line));
                    }
                    else if (line.EndsWith(" m"))
                    {
                        page.Statements.Add(new SetPointStatement(line));
                    }
                    else if (line.EndsWith(" l"))
                    {
                        page.Statements.Add(new LineToStatement(line));
                    }
                    else if (line.EndsWith(" c"))
                    {
                        page.Statements.Add(new BezierCurveStatement(line));
                    }
                    else if (line.EndsWith(" d"))
                    {
                        page.Statements.Add(new SetLineDashPatternStatement(line));
                    }
                    else if (line.EndsWith(" w"))
                    {
                        page.Statements.Add(new SetLineWidthStatement(line));
                    }
                    else if (line.EndsWith(" re"))
                    {
                        page.Statements.Add(new RectangleStatement(line));
                    }
                    else if (line == "S")
                    {
                        page.Statements.Add(StrokePathStatement.Value);
                    }
                    else if (line == "s")
                    {
                        page.Statements.Add(CloseStrokePathStatement.Value);
                    }
                    else if (line == "f")
                    {
                        page.Statements.Add(FillPathStatement.Value);
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                }

                pages.Add(page);
            }

            return pages;
        }


    }
}