using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    public class PageCollection : List<Page>
    {
        public List<string> Errors { get; set; }
        public PdfReader PdfReader { get; set; }
    }
}
