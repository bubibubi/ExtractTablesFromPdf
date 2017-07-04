namespace BuildTablesFromPdf.Engine.Statements
{
    internal class GreyColorStatement : SingleLineStatement
    {
        public GreyColorStatement(string rawContent)
        {
            RawContent = rawContent;
        }
    }
}