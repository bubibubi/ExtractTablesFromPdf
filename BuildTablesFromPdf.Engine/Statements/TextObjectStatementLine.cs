using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    public class TextObjectStatementLine : ICloneable
    {
        public Point Position { get; set; }
        public string Content { get; set; }
        public FontInfo Font { get; set; }

        public double FontHeight { get; set; }

        #region ICloneable Members

        public TextObjectStatementLine Clone()
        {
            return new TextObjectStatementLine()
            {
                Position = Position,
                Content = Content,
                Font = Font,
                FontHeight = FontHeight
            };    
        }


        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}