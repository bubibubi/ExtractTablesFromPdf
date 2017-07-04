using System;
using BuildTablesFromPdf.Engine;

// https://pdftables.com/pdf-converter-for-business

namespace BuildTablesFromPdf
{
    class Program
    {
        static void Main(string[] args)
        {

            Matrix matrix = new Matrix(1,0,-1,0,50,60);
            Console.WriteLine(matrix * Matrix.Identity);
            Console.WriteLine(Matrix.Identity * matrix);


            //ExtractByLocation.Read(@"C:\Users\Utente\Desktop\CR2993\GLOBAL_SISTEMI.PDF");

            var pages = ExtractText.Read(@"C:\Users\Utente\Desktop\CR2993\GLOBAL_SISTEMI.PDF");

            pages[2].DetermineTableStructures();


            Console.ReadLine();
        }
    }
}
