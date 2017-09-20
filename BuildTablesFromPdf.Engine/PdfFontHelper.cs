using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BuildTablesFromPdf.Engine.CMap;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine
{
    static class PdfFontHelper
    {
        public static PdfDictionary GetFont(PdfReader pdfReader, int pageNumber, string fontKey)
        {
            PdfDictionary resources = pdfReader.GetPageN(pageNumber).GetAsDict(PdfName.RESOURCES);
            return FindFontDictionary(resources, fontKey);
        }

        public static void ExtractFontNameOfPdf(string sourceFileName)
        {
            using (Stream pdfStream = new FileStream(sourceFileName, FileMode.Open))
            {
                ExtractFontNameOfPdf(new PdfReader(pdfStream));
            }
        }

        public static void ExtractFontNameOfPdf(PdfReader pdfReader)
        {
            List<BaseFont> set = new List<BaseFont>();

            for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
            {
                PdfDictionary resources = pdfReader.GetPageN(pageNumber).GetAsDict(PdfName.RESOURCES);
                ProcessResources(set, resources);
            }

            foreach (BaseFont item in set)
                Console.WriteLine(item.PostscriptFontName + " " + item.FontType.ToString());
        }

        public static void ExtractFontNameOfPdf(PdfReader pdfReader, int pageNumber)
        {
            List<BaseFont> set = new List<BaseFont>();
            PdfDictionary resources;


            // GetPageN parameter is 1 based
            resources = pdfReader.GetPageN(pageNumber + 1).GetAsDict(PdfName.RESOURCES);
            ProcessResources(set, resources);


            foreach (BaseFont item in set)
                Console.WriteLine(item.PostscriptFontName + " " + item.FontType.ToString());
        }


        private static void ProcessResources(List<BaseFont> baseFonts, PdfDictionary resources)
        {
            if (resources == null)
                return;
            PdfDictionary xObjects = resources.GetAsDict(PdfName.XOBJECT);
            if (xObjects != null)
            {
                foreach (PdfName key in xObjects.Keys)
                {
                    ProcessResources(baseFonts, xObjects.GetAsDict(key));
                }
            }

            PdfDictionary fonts = resources.GetAsDict(PdfName.FONT);

            if (fonts == null)
                return;
            foreach (PdfName key in fonts.Keys)
            {
                PRIndirectReference iRef = (PRIndirectReference)fonts.Get(key);
                if (iRef != null)
                    baseFonts.Add(BaseFont.CreateFont(iRef));
            }
        }


        public static CMapToUnicode GetFontCMapToUnicode(PdfReader pdfReader, int pageNumber, string fontKey)
        {
            PdfDictionary resources = pdfReader.GetPageN(pageNumber).GetAsDict(PdfName.RESOURCES);
            var fontDict = FindFontDictionary(resources, fontKey);
            if (fontDict == null)
                return null;
            PRStream toUnicodeIndirectReference = (PRStream)PdfReader.GetPdfObject(fontDict.Get(PdfName.TOUNICODE));
            if (toUnicodeIndirectReference == null)
                return null;
            string toUnicode = Encoding.UTF8.GetString(PdfReader.GetStreamBytes(toUnicodeIndirectReference));

            return CMapToUnicode.Parse(toUnicode);
        }


        private static PdfDictionary FindFontDictionary(PdfDictionary resources, string fontKey)
        {
            if (resources == null)
                return null;
            PdfDictionary xObjects = resources.GetAsDict(PdfName.XOBJECT);
            PdfDictionary fontDictionary;
            if (xObjects != null)
            {
                foreach (PdfName key in xObjects.Keys)
                {
                    fontDictionary = FindFontDictionary(xObjects.GetAsDict(key), fontKey);
                    if (fontDictionary != null)
                        return fontDictionary;
                }
            }

            PdfDictionary fonts = resources.GetAsDict(PdfName.FONT);

            if (fonts == null)
                return null;

            PdfName pdfName = fonts.Keys.Cast<PdfName>().FirstOrDefault(_ => _.ToString() == fontKey);
            if (pdfName == null)
                return null;

            fontDictionary = (PdfDictionary)PdfReader.GetPdfObject(fonts.Get(pdfName));

            return fontDictionary;

        }


        public static string ToUnicode(string content, CMapToUnicode cMapToUnicode, EncodingDifferenceToUnicode encodingDifferenceToUnicode)
        {
            if (cMapToUnicode != null)
                return cMapToUnicode.ConvertToString(content);
            else if (encodingDifferenceToUnicode != null)
                return encodingDifferenceToUnicode.ConvertToString(content);
            else
                return content;
        }

        public static string ToUnicode(int[] content, CMapToUnicode cMapToUnicode, EncodingDifferenceToUnicode encodingDifferenceToUnicode)
        {
            if (cMapToUnicode != null)
                return cMapToUnicode.ConvertToString(content);
            else if (encodingDifferenceToUnicode != null)
                return encodingDifferenceToUnicode.ConvertToString(content);
            else
            {
                byte[] byteContent = new byte[content.Length * sizeof(int)];
                Buffer.BlockCopy(content, 0, byteContent, 0, byteContent.Length);
                string stringContent = System.Text.Encoding.Unicode.GetString(byteContent);
                return stringContent;
            }
        }

        public static char ToUnicode(int character, CMapToUnicode cMapToUnicode, EncodingDifferenceToUnicode encodingDifferenceToUnicode)
        {
            if (cMapToUnicode != null)
                return cMapToUnicode.ConvertToUnicodeChar(character);
            else if (encodingDifferenceToUnicode != null)
                return encodingDifferenceToUnicode.ConvertToUnicodeChar(character);
            else
                return Convert.ToChar(character);
        }
    }
}
