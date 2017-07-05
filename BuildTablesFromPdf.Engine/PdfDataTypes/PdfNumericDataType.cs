using System;

namespace BuildTablesFromPdf.Engine
{
    public static class PdfNumericDataType
    {
        public static bool IsStartChar(string content, int i)
        {
            return IsValidChar(content, i);
        }

        public static bool IsValidChar(string content, int i)
        {
            return content[i] >= '0' && content[i] <= '9' || content[i] == '.' || content[i] == '+' || content[i] == '-';
        }


        public static string GetRawData(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("Content is not a PdfNumericDataType", "content");

            string data = String.Empty;
            while (IsValidChar(content, i))
            {
                data += content[i];
                i++;
            }
            return data;
        }
    }
}