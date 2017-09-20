using System;
using BuildTablesFromPdf.Engine.Statements;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class CMapToUnicode
    {
        public CMapToUnicode()
        {
            BFRanges = new BFRangeCollection();
        }

        public BFRangeCollection BFRanges { get; private set; }

        public int ConvertToUnicode(int cid)
        {
            var bfRange = BFRanges.Find(cid);
            if (bfRange == null)
                return cid;

            return cid - bfRange.BeginChar + bfRange.UnicodeChar.Value;
        }

        public char ConvertToUnicodeChar(int cid)
        {
            return Convert.ToChar(ConvertToUnicode(cid));
        }

        public char ConvertToUnicodeChar(char cid)
        {
            return Convert.ToChar(ConvertToUnicode(cid));
        }


        public string ConvertToString(string content)
        {
            string convert = string.Empty;
            foreach (char c in content)
                convert += ConvertToUnicodeChar(c);
            return convert;
        }

        public string ConvertToString(int[] content)
        {
            string convert = string.Empty;
            foreach (int c in content)
                convert += ConvertToUnicodeChar(c);
            return convert;
        }

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The CMapToUnicode or null if the characters map directly to unicode</returns>
        public static CMapToUnicode Parse(string s)
        {
            CMapToUnicode parse = new CMapToUnicode();

            InternalParseBFRange(s, parse);
            InternalParseBFChar(s, parse);

            return parse.BFRanges.Count == 0 ? null : parse;
        }

        private static void InternalParseBFRange(string s, CMapToUnicode parse)
        {
            string bfRange;
            int beginBfRangePosition = s.IndexOf("beginbfrange", StringComparison.CurrentCultureIgnoreCase);
            if (beginBfRangePosition == -1)
                return;
            beginBfRangePosition += 12;

            int endBfRangePosition = s.IndexOf("endbfrange", beginBfRangePosition, StringComparison.CurrentCultureIgnoreCase);
            bfRange = s.Substring(beginBfRangePosition, endBfRangePosition - beginBfRangePosition);

            int i = 0;
            Statement.SkipSpace(bfRange, ref i);
            while (i < bfRange.Length)
            {
                parse.BFRanges.Add(BFRange.Parse(bfRange, ref i));
                Statement.SkipSpace(bfRange, ref i);
            }
        }

        private static void InternalParseBFChar(string s, CMapToUnicode parse)
        {
            string bfChar;
            int beginBfCharPosition = s.IndexOf("beginbfchar", StringComparison.CurrentCultureIgnoreCase);
            if (beginBfCharPosition == -1)
                return;
            beginBfCharPosition += 12;

            int endBfCharPosition = s.IndexOf("endbfchar", beginBfCharPosition, StringComparison.CurrentCultureIgnoreCase);
            bfChar = s.Substring(beginBfCharPosition, endBfCharPosition - beginBfCharPosition);

            int i = 0;
            Statement.SkipSpace(bfChar, ref i);
            while (i < bfChar.Length)
            {
                parse.BFRanges.Add(BFChar.Parse(bfChar, ref i));
                Statement.SkipSpace(bfChar, ref i);
            }
        }


    }
}
