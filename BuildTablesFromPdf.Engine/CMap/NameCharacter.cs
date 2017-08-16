using System;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class NameCharacter
    {
        public string Name { get; set; }
        public char Character { get; set; }

        public override string ToString()
        {
            return string.Format("{0} => {1}", Name, Character);
        }
    }
}
