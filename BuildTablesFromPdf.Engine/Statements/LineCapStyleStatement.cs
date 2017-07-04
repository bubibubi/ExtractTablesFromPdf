using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    class LineCapStyleStatement : SingleLineStatement
    {
        public LineCapStyleStatement(string rawContent)
        {
            base.RawContent = rawContent;
        }
    }
}
