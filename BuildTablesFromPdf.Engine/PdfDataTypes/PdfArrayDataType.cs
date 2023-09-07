using System;
using System.Collections.Generic;
using System.Globalization;

namespace BuildTablesFromPdf.Engine
{
    public class PdfArrayDataType
    {
        public PdfArrayDataType()
        {
            StringElements = new List<string>();
            Elements = new List<object>();
        }

        public List<String> StringElements { get; private set; }
        public List<object> Elements { get; private set; }


        public static PdfArrayDataType Parse(string s)
        {
            s = s.Trim();
            if (!s.StartsWith("[") || !s.EndsWith("]"))
                throw new InvalidOperationException("{0} is not a valid array");

            PdfArrayDataType pdfArrayDataType = new PdfArrayDataType();
            int i = 0;
            InternalParse(pdfArrayDataType, s, ref i);
            return pdfArrayDataType;
        }

        private static void InternalParse(PdfArrayDataType pdfArrayDataType, string s, ref int i)
        {
            i++;
            while (i < s.Length)
            {
                if (PdfStringDataType.IsStartChar(s, i))
                {
                    string item = PdfStringDataType.GetRawData(s, ref i);
                    pdfArrayDataType.StringElements.Add(item);
                    pdfArrayDataType.Elements.Add(item);
                }
                else if (PdfHexStringDataType.IsStartChar(s, i))
                {
                    string item = PdfHexStringDataType.GetRawData(s, ref i);
                    pdfArrayDataType.StringElements.Add(item);
                    pdfArrayDataType.Elements.Add(item);
                }
                else if (PdfNumericDataType.IsStartChar(s, i))
                {
                    string item = PdfNumericDataType.GetRawData(s, ref i);
                    pdfArrayDataType.StringElements.Add(item);
                    pdfArrayDataType.Elements.Add(float.Parse(item, NumberFormatInfo.InvariantInfo));
                }
                else if (PdfArrayDataType.IsStartChar(s, i))
                {
                    string item = PdfArrayDataType.GetRawData(s, ref i);
                    pdfArrayDataType.StringElements.Add(item);

                    PdfArrayDataType innerPdfArrayDataType = new PdfArrayDataType();
                    InternalParse(innerPdfArrayDataType, s, ref i);
                    pdfArrayDataType.Elements.Add(innerPdfArrayDataType);
                }
                else if (s[i] == ']')
                    return;
                else if (s[i] == ' ')
                    i++;
                else if (s[i] == '\n')
                    i++;
                else
                    throw new ArgumentException(string.Format("{0} is not an array", s));
            }
        }


        public static bool IsStartChar(string content, int i)
        {
            return content[i] == '[';
        }

        public static string GetRawData_(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("The content is not a PdfArrayDataType");

            int stackElements = 1;
            string data = "[";
            i++;
            while (stackElements != 0)
            {
                data += content[i];
                if (content[i] == '[') stackElements++;
                if (content[i] == ']') stackElements--;
                i++;
            }
            return data;
        }

        public static string GetRawData(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("The content is not a PdfArrayDataType");

            string data = "[";

            i++;
            while (i < content.Length)
            {
                if (PdfStringDataType.IsStartChar(content, i))
                {
                    string item = PdfStringDataType.GetRawData(content, ref i);
                    data += item;
                }
                else if (PdfHexStringDataType.IsStartChar(content, i))
                {
                    string item = PdfHexStringDataType.GetRawData(content, ref i);
                    data += item;
                }
                else if (PdfNumericDataType.IsStartChar(content, i))
                {
                    string item = PdfNumericDataType.GetRawData(content, ref i);
                    data += item;
                }
                else if (PdfArrayDataType.IsStartChar(content, i))
                {
                    string item = PdfArrayDataType.GetRawData(content, ref i);
                    data += item;
                }
                else if (content[i] == ']')
                {
                    data += content[i];
                    return data;
                }
                else if (content[i] == ' ')
                    i++;
                else if (content[i] == '\n')
                    i++;
                else
                    throw new ArgumentException(string.Format("{0} is not an array", content));
            }

            return data;
        }


    }
}
