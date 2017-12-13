using System;

namespace BuildTablesFromPdf.Engine
{
    public static class PdfHexStringDataType
    {

        public static bool IsStartChar(string content, int i)
        {
            return content[i] == '<';
        }

        public static bool IsStartChar(string content)
        {
            return content[0] == '<';
        }


        public static string GetRawData(string content, ref int i)
        {
            if (!IsStartChar(content, i))
                throw new ArgumentException("The content is not a PdfStringDataType");

            string data = String.Empty;
            while (content[i] != '>')
            {
                data += content[i];
                i++;
            }
            data += content[i];
            i++;
            return data;
        }

        public static string GetContent(string escapedContent)
        {
            if (escapedContent == null) throw new ArgumentNullException("escapedContent");
            escapedContent = escapedContent.Trim();
            if (!escapedContent.StartsWith("<") || !escapedContent.EndsWith(">"))
                throw new ArgumentException(String.Format("Error retrieving content from escaped content '{0}'", escapedContent), "escapedContent");

            string content = string.Empty;
            for (int i = 1; i < escapedContent.Length - 1; i += 2)
            {
                char c = (char)int.Parse(escapedContent.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                if (c >= 32)
                    content += c;
            }

            return content;
        }

        public static int[] GetHexContent(string escapedContent)
        {
            if (escapedContent == null) throw new ArgumentNullException("escapedContent");

            if (!escapedContent.StartsWith("<") || !escapedContent.EndsWith(">"))
                throw new ArgumentException(String.Format("Error retrieving content from escaped content '{0}'", escapedContent), "escapedContent");

            string hexContentString = escapedContent.Substring(1, escapedContent.Length - 2);
            if (hexContentString.Length < 5)
                return new[] { int.Parse(hexContentString, System.Globalization.NumberStyles.HexNumber) };

            if ((hexContentString.Length & 0x01) != 0)
                throw new ArgumentException("Odd number of hex characters");

            int[] content;
            if ((hexContentString.Length & 0x03) != 0)
            {
                content = new int[hexContentString.Length >> 1];
                // Bytes
                for (int i = 0; i < hexContentString.Length >> 1; i++)
                    content[i] = int.Parse(hexContentString.Substring(i << 1, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                content = new int[hexContentString.Length >> 2];
                // Words
                for (int i = 0; i < hexContentString.Length >> 2; i ++)
                    content[i] = int.Parse(hexContentString.Substring(i << 2, 4), System.Globalization.NumberStyles.HexNumber);
            }

            return content;
        }

    }
}