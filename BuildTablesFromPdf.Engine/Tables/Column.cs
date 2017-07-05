namespace BuildTablesFromPdf.Engine.Tables
{
    public class Column
    {
        public float BeginX { get; set; }
        public float EndX { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return string.Format("Index: {0}, {1}-{2}", Index, BeginX, EndX);
        }

    }
}