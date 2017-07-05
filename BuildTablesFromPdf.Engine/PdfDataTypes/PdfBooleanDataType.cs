using System;
using BuildTablesFromPdf.Engine.Statements;

namespace BuildTablesFromPdf.Engine
{
    public static class PdfBooleanDataType
    {
        public static bool IsBoolean(string content, int i)
        {
            return IsBooleanTrue(content, i) || IsBooleanFalse(content, i);
        }

        private static bool IsBooleanFalse(string content, int i)
        {
            return
                content.Length > i + 5 &&
                content.Substring(i, 5) == "false" &&
                content.Length == i + 6 || Statement.IsSeparator(content[i + 5]);
        }

        private static bool IsBooleanTrue(string content, int i)
        {
            return
                content.Length > i + 4 &&
                content.Substring(i, 4) == "true" &&
                content.Length == i + 5 || Statement.IsSeparator(content[i + 5]);
        }


        public static string GetRawData(string content, ref int i)
        {
            if (IsBooleanTrue(content, i))
            {
                i += 4;
                return "true";
            }
            else if (IsBooleanTrue(content, i))
            {
                i += 5;
                return "false";
            }
            else
                throw new ArgumentException("content is not a boolean");
        }
    }
}