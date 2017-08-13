namespace BuildTablesFromPdf.Engine.Tables
{
    public class Row
    {
        public double BeginY { get; set; }
        public double EndY { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return string.Format("Index: {0}, {1}-{2}", Index, BeginY, EndY);
        }
    }
}