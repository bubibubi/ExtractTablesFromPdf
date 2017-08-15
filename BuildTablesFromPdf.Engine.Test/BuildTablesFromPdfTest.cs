using System;
using System.IO;
using CR.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildTablesFromPdf.Engine.Test
{
    [TestClass]
    public class BuildTablesFromPdfTest
    {

        private const string Path = @"TestFiles\";


        [TestMethod]
        public void BuildTablesFromPdfTestRun()
        {
            var pdfFileList = Directory.GetFiles(Path, "*.pdf");

            foreach (var pdfFilePath in pdfFileList)
                CheckFile(pdfFilePath);

        }


        private static void CheckFile(string pdfFilePath)
        {
            Console.WriteLine("Reading " + System.IO.Path.GetFileName(pdfFilePath));
            PageCollection pages = ContentExtractor.ReadPdfFileAndRefreshContent(pdfFilePath);
            string fileContent = string.Empty;
            foreach (Page page in pages)
            {
                fileContent += "======================================================\r\n";
                fileContent += page.ToString();
            }


            string txtFileName = pdfFilePath + ".txt";

            if (File.Exists(txtFileName))
            {
                Console.WriteLine("Testing " + System.IO.Path.GetFileName(pdfFilePath));
                string txtFileContent = File.ReadAllText(txtFileName);
                if (txtFileContent != fileContent)
                {
                    string[] txtFileLines = txtFileContent.Replace("\r\n", "\r").Split('\r');
                    string[] txtLines = fileContent.Replace("\r\n", "\r").Split('\r');
                    string diff = MHDiff.GetDiff(txtFileLines, txtLines);
                    Console.WriteLine("Files are different");
                    Console.WriteLine(diff);
                    throw new Exception("Wrong content in file " + pdfFilePath);
                }
            }
            else
            {
                Console.WriteLine(System.IO.Path.GetFileName(pdfFilePath) + " NOT TESTED!!!");
                Console.WriteLine("Creating txt file " + txtFileName);
                File.WriteAllText(txtFileName, fileContent);
            }
        }
    }
}
