using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    class LineJoinStyleStatement : SingleLineStatement
    {
        public LineJoinStyleStatement(string rawContent)
        {
            base.RawContent = rawContent;
        }
    }
}
