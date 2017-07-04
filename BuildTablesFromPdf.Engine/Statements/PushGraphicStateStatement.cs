using System;

namespace BuildTablesFromPdf.Engine.Statements
{
    class PushGraphicStateStatement : SingleLineStatement
    {
        public static readonly PushGraphicStateStatement Value = new PushGraphicStateStatement();
    }
}
