using System;
using System.Globalization;
using BuildTablesFromPdf.Engine.Statements;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class BFRange
    {
        public int BeginChar { get; set; }
        public int EndChar { get; set; }
        public int? UnicodeChar { get; set; }

        public static BFRange Parse(string s, ref int startPosition)
        {
            string sBeginChar;
            string sEndChar;
            string sUnicodeChar;

            Statement.SkipSpace(s, ref startPosition);
            sBeginChar = PdfHexStringDataType.GetRawData(s, ref startPosition);
            Statement.SkipSpace(s, ref startPosition);
            sEndChar = PdfHexStringDataType.GetRawData(s, ref startPosition);
            Statement.SkipSpace(s, ref startPosition);
            sUnicodeChar = PdfHexStringDataType.GetRawData(s, ref startPosition);

            int beginChar = int.Parse(sBeginChar.Substring(1, sBeginChar.Length - 2), NumberStyles.HexNumber);
            int endChar = int.Parse(sEndChar.Substring(1, sEndChar.Length - 2), NumberStyles.HexNumber);
            int unicodeChar = int.Parse(sUnicodeChar.Substring(1, sUnicodeChar.Length - 2), NumberStyles.HexNumber);

            return new BFRange()
            {
                BeginChar = beginChar,
                EndChar = endChar,
                UnicodeChar = unicodeChar
            };
        }

        public override string ToString()
        {
            return string.Format("{0}-{1} {2}({3})", BeginChar, EndChar, UnicodeChar, (char)UnicodeChar.GetValueOrDefault('?'));
        }
    }
}
