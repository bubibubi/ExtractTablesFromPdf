using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    class ModifyMatrixStatement : SingleLineStatement
    {
        public ModifyMatrixStatement(string rawContent)
        {
            base.RawContent = rawContent;
        }
    }
}
