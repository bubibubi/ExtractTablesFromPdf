namespace BuildTablesFromPdf.Engine.Statements
{
    internal class SetLineDashPatternStatement : SingleLineStatement
    {
        public SetLineDashPatternStatement(string rawContent)
        {
            RawContent = rawContent;
        }
    }
}