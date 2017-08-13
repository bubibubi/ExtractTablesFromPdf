namespace BuildTablesFromPdf.Engine.Tables
{
    public class Column
    {
        public double BeginX { get; set; }
        public double EndX { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return string.Format("Index: {0}, {1}-{2}", Index, BeginX, EndX);
        }

    }
}