using System;
using System.Collections.Generic;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class EncodingDifference
    {
        public EncodingDifference(int beginChar)
        {
            BeginChar = beginChar;
            NameCharacters = new List<NameCharacter>();
        }

        public int BeginChar { get; private set; }

        public int EndChar
        {
            get { return BeginChar + NameCharacters.Count - 1; }
        }


        public List<NameCharacter> NameCharacters { get; private set; }
    }
}
