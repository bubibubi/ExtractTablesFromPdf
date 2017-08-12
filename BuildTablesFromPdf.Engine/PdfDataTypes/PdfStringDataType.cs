using System;

namespace BuildTablesFromPdf.Engine
{
    public static class PdfStringDataType
    {
        public static string GetContentFromEscapedContent(string escapedContent)
        {
            if (escapedContent == null) throw new ArgumentNullException("escapedContent");

            if (!escapedContent.StartsWith("(") || !escapedContent.EndsWith(")"))
                throw new ArgumentException(String.Format("Error retrieving content from escaped content '{0}'", escapedContent), "escapedContent");

            return System.Text.RegularExpressions.Regex.Unescape(escapedContent.Substring(1, escapedContent.Length - 2));
        }

        public static bool IsStartChar(string content, int i)
        {
            return content[i] == '(';
        }

        public static string GetRawData(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("The content is not a PdfStringDataType");

            string data = String.Empty;
            while (content[i] != ')')
            {
                if (content[i] == '\\')
                {
                    data += content[i];
                    i++;
                }
                data += content[i];
                i++;
            }
            data += content[i];
            i++;
            return data;
        }
    }
}