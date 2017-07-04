namespace BuildTablesFromPdf.Engine.Statements
{
    internal class SetLineWidthStatement : SingleLineStatement
    {
        public SetLineWidthStatement(string rawContent)
        {
            RawContent = rawContent;
        }
    }
}