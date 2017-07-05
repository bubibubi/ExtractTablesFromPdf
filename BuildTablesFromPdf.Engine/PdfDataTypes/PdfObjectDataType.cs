using System;
using BuildTablesFromPdf.Engine.Statements;

namespace BuildTablesFromPdf.Engine
{
    public static class PdfObjectDataType
    {
        public static bool IsStartChar(string content, int i)
        {
            return content[i] == '/';
        }

        public static string GetRawData(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("Content is not a PdfNumericDataType", "content");

            string data = String.Empty;
            while (!Statement.IsSeparator(content[i]))
            {
                data += content[i];
                i++;
            }
            return data;
        }
    }
}