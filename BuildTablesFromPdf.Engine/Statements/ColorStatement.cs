using System;

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
                    return new Color(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
                if (parts.Length == 2)
                    return new Color(float.Parse(parts[0]), float.Parse(parts[0]), float.Parse(parts[0]));
                else
                    return new Color(float.Parse(parts[0]), float.Parse(parts[0]), float.Parse(parts[0]));
            }
        }
    }
}
