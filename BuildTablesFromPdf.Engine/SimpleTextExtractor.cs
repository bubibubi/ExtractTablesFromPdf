using System;
using System.Globalization;
using System.Linq;
using System.Text;
using BuildTablesFromPdf.Engine.CMap;
using BuildTablesFromPdf.Engine.Statements;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    /// <summary>
    /// The output of this class should be very similar to SimpleTextExtractor missing in LGPL version of iTextSharp
    /// </summary>
    public class SimpleTextExtractor
    {
        public static string ReadPdfFile(string filename)
        {
            PdfReader pdfReader = new PdfReader(filename);
            string strText = string.Empty;

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                string s = GetTextFromPage(pdfReader, page);

                /*
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                s = s.Replace("\n", "\r\n");
                s = s.Replace("\0", " ");
                */

                strText += s;
            }

            pdfReader.Close();

            return strText;


        }

        public static string ReadPdfFilePage(string filename, int page)
        {
            PdfReader pdfReader = new PdfReader(filename);
            string strText = string.Empty;

            string s = GetTextFromPage(pdfReader, page);

            /*
            s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
            s = s.Replace("\n", "\r\n");
            s = s.Replace("\0", " ");
            */

            strText += s;

            pdfReader.Close();

            return strText;


        }

        private static string GetTextFromPage(PdfReader pdfReader, int pageNumber)
        {
            StringBuilder sb = new StringBuilder();

            Matrix transformMatrix = Matrix.Identity;
            float leadingParameter = 0;
            Point position;
            CMapToUnicode cMapToUnicode = null;
            EncodingDifferenceToUnicode encodingDifferenceToUnicode = null;

            double oldY = 0;
            string lineContent = null;

            string rawPdfContent = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(pageNumber)));
            int pointer = 0;

            string statement = Statement.GetNextStatement(rawPdfContent, ref pointer);
            while (statement != null)
            {

                // Embedded image
                if (statement.EndsWith("BI"))
                {
                    pointer = rawPdfContent.IndexOf("\nEI", pointer, StringComparison.Ordinal);
                }
                else if (statement.EndsWith("Tm"))
                {
                    Matrix matrix;
                    if (Matrix.TryParse(statement, out matrix))
                        transformMatrix = matrix;
                }
                else if (statement.EndsWith("Tf"))
                {
                    string[] fontParameters = statement.Split(' ');
                    cMapToUnicode = PdfFontHelper.GetFontCMapToUnicode(pdfReader, pageNumber, fontParameters[fontParameters.Length - 3]);
                    encodingDifferenceToUnicode = EncodingDifferenceToUnicode.Parse(PdfFontHelper.GetFont(pdfReader, pageNumber, fontParameters[fontParameters.Length - 3]));
                }
                else if (statement.EndsWith("Td"))
                {
                    float tx;
                    float ty;
                    string[] parameters = statement.Split(' ');
                    if (
                        float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tx) &&
                        float.TryParse(parameters[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ty))
                        transformMatrix = new Matrix(1, 0, 0, 1, tx, ty);
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
                        transformMatrix = new Matrix(1, 0, 0, 1, tx, ty) * transformMatrix;
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
                    transformMatrix = new Matrix(1, 0, 0, 1, 0, -leadingParameter) * transformMatrix;
                }
                else if (statement.EndsWith("TJ"))
                {
                    string content = TextObjectStatement.GetTJContent(statement, cMapToUnicode, encodingDifferenceToUnicode);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        content = content.Trim();

                        //line.Position = BaseTransformMatrix.TransformPoint(new Point(transformMatrix.TransformX(position.X, position.Y), transformMatrix.TransformY(position.X, position.Y) + line.FontHeight)).Rotate(pageRotation);
                        position = new Point(transformMatrix.TransformX(Point.Origin.X, Point.Origin.Y), transformMatrix.TransformY(Point.Origin.X, Point.Origin.Y));
                        if (oldY == position.Y)
                        {
                            if (!string.IsNullOrWhiteSpace(lineContent))
                                lineContent += " " + content;
                            else
                                lineContent = content;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(lineContent))
                                sb.AppendLine(lineContent);
                            lineContent = content;
                            oldY = position.Y;
                        }
                    }
                }
                else if (statement.Trim().EndsWith("Tj"))
                {
                    string escapedContent;
                    escapedContent = statement.Trim();
                    escapedContent = escapedContent.Remove(escapedContent.Length - 2);
                    string content = PdfHexStringDataType.IsStartChar(escapedContent) ? PdfHexStringDataType.GetContent(escapedContent) : PdfStringDataType.GetContentFromEscapedContent(escapedContent);
                    content = content.Trim();
                    content = PdfFontHelper.ToUnicode(content, cMapToUnicode, encodingDifferenceToUnicode);
                    //line.Position = BaseTransformMatrix.TransformPoint(new Point(transformMatrix.TransformX(position.X, position.Y), transformMatrix.TransformY(position.X, position.Y) + line.FontHeight)).Rotate(pageRotation);
                    position = new Point(transformMatrix.TransformX(Point.Origin.X, Point.Origin.Y), transformMatrix.TransformY(Point.Origin.X, Point.Origin.Y));
                    if (Math.Abs(oldY - position.Y) < 1)
                    {
                        if (!string.IsNullOrWhiteSpace(lineContent))
                            lineContent += " " + content;
                        else
                            lineContent = content;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(lineContent))
                            sb.AppendLine(lineContent);
                        lineContent = content;
                        oldY = position.Y;
                    }
                }


                statement = Statement.GetNextStatement(rawPdfContent, ref pointer);

            }

            if (!string.IsNullOrWhiteSpace(lineContent))
                sb.Append(lineContent);
            string textFromPage = sb.ToString();

            return textFromPage;
        }
    }
}
