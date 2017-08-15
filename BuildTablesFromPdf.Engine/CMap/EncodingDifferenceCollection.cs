using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildTablesFromPdf.Engine.CMap
{
    public class EncodingDifferenceCollection : List<EncodingDifference>
    {
        public EncodingDifference Find(int id)
        {
            return this.SingleOrDefault(_ => _.BeginChar <= id && _.EndChar >= id);
        }
    }
}
