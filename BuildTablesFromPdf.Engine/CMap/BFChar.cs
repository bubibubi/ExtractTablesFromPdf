using System.Globalization;
using BuildTablesFromPdf.Engine.Statements;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class BFChar : BFRange
    {
        public static BFRange Parse(string s, ref int startPosition)
        {
            string sBeginChar;
            string sUnicodeChar;

            Statement.SkipSpace(s, ref startPosition);
            sBeginChar = PdfHexStringDataType.GetRawData(s, ref startPosition);
            Statement.SkipSpace(s, ref startPosition);
            sUnicodeChar = PdfHexStringDataType.GetRawData(s, ref startPosition);

            int beginChar = int.Parse(sBeginChar.Substring(1, sBeginChar.Length - 2), NumberStyles.HexNumber);
            int unicodeChar = int.Parse(sUnicodeChar.Substring(1, sUnicodeChar.Length - 2), NumberStyles.HexNumber);

            return new BFRange()
            {
                BeginChar = beginChar,
                EndChar = beginChar,
                UnicodeChar = unicodeChar
            };
        }

    }
}