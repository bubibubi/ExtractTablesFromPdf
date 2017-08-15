using System;
using BuildTablesFromPdf.Engine;

// https://pdftables.com/pdf-converter-for-business

namespace BuildTablesFromPdf
{
    class Program
    {
        static void Main(string[] args)
        {

            var pages = ContentExtractor.Read(@"Example.PDF");
            var page = pages[0];

            page.DetermineTableStructures();
            page.DetermineParagraphs();
            page.FillContent();

            Console.WriteLine(page);

            Console.ReadLine();
        }
    }
}
