using System;
using iTextSharp.text.pdf;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class EncodingDifferenceToUnicode
    {
        public EncodingDifferenceToUnicode()
        {
            EncodingDifferences = new EncodingDifferenceCollection();
        }

        public EncodingDifferenceCollection EncodingDifferences { get; private set; }


        public int ConvertToUnicode(int cid)
        {
            var encodingDifference = EncodingDifferences.Find(cid);
            if (encodingDifference == null)
                return cid;

            return encodingDifference.NameCharacters[cid - encodingDifference.BeginChar].Character;
        }

        public char ConvertToUnicodeChar(int cid)
        {
            return (char)ConvertToUnicode(cid);
        }

        public char ConvertToUnicodeChar(char cid)
        {
            return (char)ConvertToUnicode(cid);
        }


        public string ConvertToString(string content)
        {
            string convert = string.Empty;
            foreach (char c in content)
                convert += ConvertToUnicodeChar(c);
            return convert;
        }

        public string ConvertToString(int[] content)
        {
            string convert = string.Empty;
            foreach (int c in content)
                convert += ConvertToUnicodeChar(c);
            return convert;
        }


        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="fontDictionary">The font dictionary.</param>
        /// <returns>
        /// The EncodingDifferenceToUnicode or null if the characters map directly to unicode
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Name found before Number
        /// or
        /// In /Differences only Numbers and Names are allowed
        /// </exception>
        public static EncodingDifferenceToUnicode Parse(PdfDictionary fontDictionary)
        {
            EncodingDifferenceToUnicode parse = new EncodingDifferenceToUnicode();

            var encodingDictionaryReference = fontDictionary.GetAsDict(PdfName.ENCODING);
            if (encodingDictionaryReference == null)
                return null;


            PdfArray differencesArray = encodingDictionaryReference.GetAsArray(PdfName.DIFFERENCES);

            EncodingDifference encodingDifference = null;

            foreach (var item in differencesArray.ArrayList)
            {
                if (item is PdfNumber)
                {
                    encodingDifference = new EncodingDifference(((PdfNumber) item).IntValue);
                    parse.EncodingDifferences.Add(encodingDifference);
                }
                else if (item is PdfName)
                {
                    if (encodingDifference == null)
                        throw new InvalidOperationException("Name found before Number");
                    string name = ((PdfName) item).ToString().Substring(1);
                    var nameCharacter = NameCharacterCollection.Instance.Find(name);
                    encodingDifference.NameCharacters.Add(nameCharacter);
                }
                else
                {
                    throw new InvalidOperationException("In /Differences only Numbers and Names are allowed");
                }
            }

            return parse;
        }

    }
}
