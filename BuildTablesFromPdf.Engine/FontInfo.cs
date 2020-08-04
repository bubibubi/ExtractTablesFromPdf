using BuildTablesFromPdf.Engine.CMap;

namespace BuildTablesFromPdf.Engine
{
    public class FontInfo
    {
        public double FontHeight { get; set; }
        public CMapToUnicode CMapToUnicode { get; set; }
        public EncodingDifferenceToUnicode EncodingDifferenceToUnicode { get; set; }
    }
}