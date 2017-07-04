using System;
using System.Collections.Generic;

namespace BuildTablesFromPdf.Engine.Statements
{
    public abstract class MultiLineStatement : Statement
    {
        public MultiLineStatement()
        {
            RawContent = new List<string>();
        }

        /// <summary>
        /// Closes the multi line statement.
        /// </summary>
        public abstract void CloseMultiLineStatement();

        public List<string> RawContent { get; private set; }
    }
}
