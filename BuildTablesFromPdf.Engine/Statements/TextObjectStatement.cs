using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BuildTablesFromPdf.Engine.CMap;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine.Statements
{
    // From BT to ET
    public class TextObjectStatement:MultiLineStatement
    {

        public TextObjectStatement(PdfReader pdfReader, int pageNumber, Matrix baseTransformMatrix)
            : base(pdfReader, pageNumber, baseTransformMatrix)
        {
        }

        public List<TextObjectStatementLine> Lines { get; private set; }

        public override void CloseMultiLineStatement()
        {
            Lines = new List<TextObjectStatementLine>();

            TextObjectStatementLine actualLineSettings = new TextObjectStatementLine();
            Matrix transformMatrix = Matrix.Identity;
            Point position = new Point();
            float leadingParameter = 0;

            int pageRotation = PdfReader.GetPageRotation(PageNumber + 1);

            foreach (string rawContent in RawContent)
            {
                if (rawContent.EndsWith("Tm"))
                {
                    Matrix matrix;
                    if (Matrix.TryParse(rawContent, out matrix))
                        transformMatrix = matrix;
                }
                else if (rawContent.EndsWith("Tf"))
                {
                    string[] fontParameters = rawContent.Split(' ');
                    float fontSize;
                    if (float.TryParse(fontParameters[fontParameters.Length - 2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out fontSize))
                        actualLineSettings.FontHeight = fontSize;
                    actualLineSettings.CMapToUnicode = PdfFontHelper.GetFontCMapToUnicode(PdfReader, PageNumber, fontParameters[fontParameters.Length - 3]);
                }
                else if (rawContent.EndsWith("Td"))
                {
                    float tx;
                    float ty;
                    string[] parameters = rawContent.Split(' ');
                    if (
                        float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tx) && 
                        float.TryParse(parameters[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ty))
                    transformMatrix = new Matrix(1, 0, 0, 1, tx, ty);
                }
                else if (rawContent.EndsWith("TD"))
                {
                    float tx;
                    float ty;
                    string[] parameters = rawContent.Split(' ');
                    if (
                        float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tx) &&
                        float.TryParse(parameters[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ty))
                    {
                        transformMatrix = new Matrix(1, 0, 0, 1, tx, ty) * transformMatrix;
                        leadingParameter = -ty;
                    }
                }
                else if (rawContent.EndsWith("TL"))
                {
                    float tl;
                    string[] parameters = rawContent.Split(' ');
                    if (
                        float.TryParse(parameters[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out tl))
                        leadingParameter = tl;
                }
                else if (rawContent.EndsWith("T*"))
                {
                    transformMatrix = new Matrix(1, 0, 0, 1, 0, -leadingParameter) * transformMatrix;
                }
                else if (rawContent.EndsWith("TJ"))
                {
                    string rawArray = rawContent.Remove(rawContent.Length - 2).Trim();
                    if (string.IsNullOrWhiteSpace(rawArray))
                        continue;
                    PdfArrayDataType pdfArrayDataType = PdfArrayDataType.Parse(rawArray);
                    string content = string.Empty;
                    foreach (string item in pdfArrayDataType.Elements.Where(_ => _ is string))
                    {
                        string escapedContent;
                        escapedContent = item.Trim();
                        content += PdfHexStringDataType.IsStartChar(escapedContent) ? PdfHexStringDataType.GetContent(escapedContent) : PdfStringDataType.GetContentFromEscapedContent(escapedContent);
                    }
                    var line = actualLineSettings.Clone();
                    line.FontHeight = line.FontHeight * transformMatrix.a;
                    line.Position = BaseTransformMatrix.TransformPoint(new Point(transformMatrix.TransformX(position.X, position.Y), transformMatrix.TransformY(position.X, position.Y) + line.FontHeight)).Rotate(pageRotation);
                    line.Content = ToUnicode(content, line.CMapToUnicode);
                    Lines.Add(line);
                }
                else if (rawContent.Trim().EndsWith("Tj"))
                {
                    string escapedContent;
                    escapedContent = rawContent.Trim();
                    escapedContent = escapedContent.Remove(escapedContent.Length - 2);
                    string content = PdfHexStringDataType.IsStartChar(escapedContent) ? PdfHexStringDataType.GetContent(escapedContent) : PdfStringDataType.GetContentFromEscapedContent(escapedContent);
                    
                    var line = actualLineSettings.Clone();
                    line.FontHeight = line.FontHeight * transformMatrix.a;
                    line.Position = BaseTransformMatrix.TransformPoint(new Point(transformMatrix.TransformX(position.X, position.Y), transformMatrix.TransformY(position.X, position.Y) + line.FontHeight)).Rotate(pageRotation);
                    line.Content = ToUnicode(content, line.CMapToUnicode);
                    Lines.Add(line);
                }


            }

        }

        private string ToUnicode(string content, CMapToUnicode cMapToUnicode)
        {
            if (cMapToUnicode == null)
                return content;
            return cMapToUnicode.ConvertToString(content);
        }
    }
}
