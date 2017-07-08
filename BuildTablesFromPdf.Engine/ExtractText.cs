using System;
using System.Collections.Generic;
using System.Text;
using BuildTablesFromPdf.Engine.Statements;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    public class ExtractText
    {

        public static bool ShowParserInfo = false;

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

                Matrix transformMatrix = Matrix.Identity;

                string rawPdfContent = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(i + 1)));
                int pointer = 0;
                
                string statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
                while (statement != null)
                {
                    if (statement == "BT")
                    {
                        currentMultilineStatement = new TextObjectStatement();
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
                        page.Statements.Add(PushGraphicStateStatement.Value);
                    }
                    else if (statement == "Q")
                    {
                        page.Statements.Add(PopGraphicStateStatement.Value);
                    }
                    else if (statement.EndsWith(" cm"))
                    {
                        page.Statements.Add(new ModifyMatrixStatement(statement));
                        Matrix newTransformMatrix;
                        if (!Matrix.TryParse(statement, out newTransformMatrix))
                            newTransformMatrix = Matrix.Identity;
                        transformMatrix *= newTransformMatrix;
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
                        page.Statements.Add(new NonStrokingColorStatement(statement));
                    }
                    else if (statement.EndsWith(" RG"))
                    {
                        page.Statements.Add(new StrokingColorStatement(statement));
                    }
                    else if (statement.EndsWith(" G"))
                    {
                        page.Statements.Add(new GreyColorStatement(statement));
                    }
                    else if (statement.EndsWith(" m"))
                    {
                        page.Statements.Add(new SetPointStatement(statement));
                    }
                    else if (statement.EndsWith(" l"))
                    {
                        page.Statements.Add(new LineToStatement(statement));
                    }
                    else if (statement.EndsWith(" c"))
                    {
                        page.Statements.Add(new BezierCurveStatement(statement));
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
                        page.Statements.Add(new RectangleStatement(statement));
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

                pages.Add(page);
            }

            return pages;
        }
    }
}