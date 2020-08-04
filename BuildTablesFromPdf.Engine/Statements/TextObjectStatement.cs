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
            Lines = new List<TextObjectStatementLine>();
        }

        public List<TextObjectStatementLine> Lines { get; private set; }

        public override void CloseMultiLineStatement()
        {

        }

        // ReSharper disable once InconsistentNaming
        public static string GetTJContent(string rawContent, CMapToUnicode cMapToUnicode, EncodingDifferenceToUnicode encodingDifferenceToUnicode)
        {
            string content;
            string rawArray = rawContent.Remove(rawContent.Length - 2).Trim();
            if (string.IsNullOrWhiteSpace(rawArray))
                return null;
            PdfArrayDataType pdfArrayDataType = PdfArrayDataType.Parse(rawArray);
            content = string.Empty;
            foreach (string item in pdfArrayDataType.Elements.Where(_ => _ is string))
            {
                string escapedContent;
                escapedContent = item.Trim();
                content +=
                    PdfHexStringDataType.IsStartChar(escapedContent) ? 
                    PdfFontHelper.ToUnicode(PdfHexStringDataType.GetHexContent(escapedContent), cMapToUnicode, encodingDifferenceToUnicode).ToString() : 
                    PdfFontHelper.ToUnicode(PdfStringDataType.GetContentFromEscapedContent(escapedContent), cMapToUnicode, encodingDifferenceToUnicode);
            }
            return content;
        }
    }
}
