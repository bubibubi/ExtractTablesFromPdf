# State of the library
The library works with few pdfs for two main reasons:
1. The transformation matrix and the graphic state is not handled
2. The fonts/encodings are not correctly handled

# ExtractTablesFromPdf
Extract tables (and paragraphs outside tables) from pdf


## License limitations
**(please read before use)**

This software is released under MIT license but uses iTextSharp v.4.1.6 that is released under MPL LGPL license. Before using this software you should also agree with the iTextSharp v.4.1.6 license.
Also, take care if you upgrade iTextSharp because newer versions are released under AGPL.

## What's this
PDF is a file format used to define device independent page output.
This project intend to retrieve text and tables from a pdf.

The main part is the **Engine**.

The **Renderer** is a debug window to understand what's happening.

## Usage

Call

	var pages = ExtractText.Read(fileName);

to read all the pages.

Then, for every page, call

	Page.DetermineTableStructures();
	Page.DetermineParagraphs();
	Page.FillContent();

To check if you already called the method above, use

	Page.IsRefreshed

After that you'll be able to access to

	Page.Contents

Contents is a collection of IPageContent ordered from top of page to bottom.  
A IPageContent can be a  
- Paragraph that contains text (Content)
- Table that contains a matrix of text (Content[,])


