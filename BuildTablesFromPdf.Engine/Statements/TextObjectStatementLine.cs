using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    public class TextObjectStatementLine : ICloneable
    {
        public float FontHeight { get; set; }
        public Point Position { get; set; }
        public string Content { get; set; }


    
        #region ICloneable Members

        public TextObjectStatementLine Clone()
        {
            return new TextObjectStatementLine()
            {
                Content = Content,
                FontHeight = FontHeight,
                Position = Position
            };    
        }


        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}