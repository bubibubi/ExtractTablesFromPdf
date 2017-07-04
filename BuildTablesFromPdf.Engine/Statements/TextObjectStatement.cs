using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BuildTablesFromPdf.Engine.Statements
{
    // From BT to ET
    public class TextObjectStatement:MultiLineStatement
    {
        public List<TextObjectStatementLine> Lines { get; private set; }

        static TextObjectStatement()
        {
            TextFinderRegex = new Regex(@"\((?<content>.*)\)\s*Tj");
        }

        public static readonly Regex TextFinderRegex;

        public override void CloseMultiLineStatement()
        {
            Lines = new List<TextObjectStatementLine>();

            TextObjectStatementLine actualLineSettings = new TextObjectStatementLine();
            Matrix transformMatrix = Matrix.Identity;
            Point position = new Point();

            foreach (string rawContent in RawContent)
            {
                if (rawContent.EndsWith("Tm"))
                {
                    Matrix matrix;
                    if (Matrix.TryParse(rawContent, out matrix))
                        transformMatrix = matrix;
                }
                if (rawContent.EndsWith("Tf"))
                {
                    string[] fontParameters = rawContent.Split(' ');
                    float fontSize;
                    if (float.TryParse(fontParameters[fontParameters.Length - 2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out fontSize))
                        actualLineSettings.FontHeight = fontSize;
                }
                else if (rawContent.Trim().EndsWith("Tj"))
                {
                    Match match = TextFinderRegex.Match(rawContent);
                    string content = string.Empty;
                    string escapedContent = match.Groups["content"].Value;
                    for (int i = 0; i < escapedContent.Length; i++)
                    {
                        char c = escapedContent[i];
                        if (c == '\\')
                            i++;
                        content += escapedContent[i];
                    }
                    var line = actualLineSettings.Clone();
                    line.Position = new Point(transformMatrix.TransformX(position.X, position.Y), transformMatrix.TransformY(position.X, position.Y) + line.FontHeight);
                    line.Content = content;
                    Lines.Add(line);
                }


            }

        }
    }
}
