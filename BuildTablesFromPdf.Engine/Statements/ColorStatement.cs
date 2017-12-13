using System;
using System.Globalization;

namespace BuildTablesFromPdf.Engine.Statements
{
    class ColorStatement : SingleLineStatement
    {
        public Color Color
        {
            get
            {
                string[] parts = RawContent.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 4)
                    return new Color(float.Parse(parts[0], NumberFormatInfo.InvariantInfo), float.Parse(parts[1], NumberFormatInfo.InvariantInfo), float.Parse(parts[2], NumberFormatInfo.InvariantInfo));
                if (parts.Length == 2)
                    return new Color(float.Parse(parts[0], NumberFormatInfo.InvariantInfo), float.Parse(parts[0], NumberFormatInfo.InvariantInfo), float.Parse(parts[0], NumberFormatInfo.InvariantInfo));
                else
                    return new Color(float.Parse(parts[0], NumberFormatInfo.InvariantInfo), float.Parse(parts[0], NumberFormatInfo.InvariantInfo), float.Parse(parts[0], NumberFormatInfo.InvariantInfo));
            }
        }
    }
}
