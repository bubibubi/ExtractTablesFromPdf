using System;
using BuildTablesFromPdf.Engine.CMap;

namespace BuildTablesFromPdf.Engine.Statements
{
    public class TextObjectStatementLine : ICloneable
    {
        public double FontHeight { get; set; }
        public Point Position { get; set; }
        public string Content { get; set; }
        public CMapToUnicode CMapToUnicode { get; set; }
        public EncodingDifferenceToUnicode EncodingDifferenceToUnicode { get; set; }

        #region ICloneable Members

        public TextObjectStatementLine Clone()
        {
            return new TextObjectStatementLine()
            {
                CMapToUnicode = CMapToUnicode,
                FontHeight = FontHeight,
                Position = Position,
                Content = Content,
                EncodingDifferenceToUnicode = EncodingDifferenceToUnicode
            };    
        }


        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}