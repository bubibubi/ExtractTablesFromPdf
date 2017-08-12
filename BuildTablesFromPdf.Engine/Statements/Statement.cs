using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    public class Statement
    {
        public static string GetNextStatement(string content, ref int i)
        {
            string statement = "";
            bool readingStatement = false;

            if (i >= content.Length)
                return null;

            while (i < content.Length)
            {
                if (content[i] == ' ')
                {
                    if (readingStatement)
                    {
                        i++;
                        return statement;
                    }

                    statement += " ";
                    i++;
                }
                else if (content[i] == '\n')
                {
                    i++;
                    return statement;
                }
                else if (readingStatement && IsSeparator(content[i]))
                {
                    return statement;
                }
                else if (PdfNumericDataType.IsStartChar(content, i))
                {
                    // string parameter
                    statement += PdfNumericDataType.GetRawData(content, ref i);
                }
                else if (PdfStringDataType.IsStartChar(content, i))
                {
                    // string parameter
                    statement += PdfStringDataType.GetRawData(content, ref i);
                }
                else if (PdfArrayDataType.IsStartChar(content, i))
                {
                    // array parameter
                    statement += PdfArrayDataType.GetRawData(content, ref i);
                }
                else if (PdfHexStringDataType.IsStartChar(content, i))
                {
                    // hex string parameter
                    statement += PdfHexStringDataType.GetRawData(content, ref i);
                }
                else if (content[i] == 't' && i + 5 < content.Length && content.Substring(i, 5) == "true ")
                {
                    // boolean true parameter
                    statement += "true ";
                    i += 5;
                }
                else if (content[i] == 'f' && i + 6 < content.Length && content.Substring(i, 6) == "false ")
                {
                    // boolean false parameter
                    statement += "false ";
                    i += 6;
                }
                else if (PdfObjectDataType.IsStartChar(content, i))
                {
                    // hex string parameter
                    statement += PdfObjectDataType.GetRawData(content, ref i);
                }
                else
                {
                    statement += content[i];
                    readingStatement = true;
                    i++;
                }
            }

            return statement;
        }

        public static bool IsSeparator(char c)
        {
            switch (c)
            {
                case' ':
                case '\n':
                case '[':
                case '(':
                case '<':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSpace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\n':
                    return true;
                default:
                    return false;
            }
        }

        public static void SkipSpace(string s, ref int i)
        {
            while (i < s.Length && IsSpace(s[i]))
                i++;
        }
    }
}
