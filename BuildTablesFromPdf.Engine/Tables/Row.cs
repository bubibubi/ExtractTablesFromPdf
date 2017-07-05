namespace BuildTablesFromPdf.Engine.Tables
{
    public class Row
    {
        public float BeginY { get; set; }
        public float EndY { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return string.Format("Index: {0}, {1}-{2}", Index, BeginY, EndY);
        }
    }
}