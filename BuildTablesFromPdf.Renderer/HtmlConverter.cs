using System;
using BuildTablesFromPdf.Engine;
using BuildTablesFromPdf.Engine.Tables;

namespace BuildTablesFromPdf.Renderer
{
    public static class HtmlConverter
    {
        public static string Convert(PageCollection pages)
        {
            string content = string.Empty;
            foreach (Page page in pages)
            {
                if (!string.IsNullOrEmpty(content))
                    content += "<hr/>";
                foreach (IPageContent pageContent in page.Contents)
                {
                    if (pageContent is Paragraph)
                    {
                        content += string.Format("<p>{0}</p>", ((Paragraph)pageContent).Content);
                    }
                    else if (pageContent is Table)
                    {
                        Table table = (Table)pageContent;
                        content += "<table>";
                        for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                        {
                            content += "<tr>";
                            for (int columnIndex = 0; columnIndex < table.Columns.Count + 2; columnIndex++)
                            {
                                string borderStyle =
                                    columnIndex == 0 || columnIndex == table.Columns.Count + 1 ?
                                    "style=\"border: none;\"" :
                                    ""
                                    ;

                                if (rowIndex == 0)
                                {
                                    content += "<th " + borderStyle + ">";
                                    content += table[rowIndex, columnIndex];
                                    content += "</th>";
                                }
                                else
                                {
                                    content += "<td " + borderStyle + ">";
                                    content += table[rowIndex, columnIndex];
                                    content += "</td>";
                                }
                            }
                            content += "</tr>";
                        }
                        content += "</table>";
                    }
                    else
                    {
                        content += pageContent.ToString().Replace("\r\n", "<br/>");
                    }
                }
            }
            return Properties.Resources.HTML_Header + content + Properties.Resources.HTML_Footer;
        }
    }
}
