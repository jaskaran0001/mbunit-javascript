using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class ComScriptErrorInfo {
        public ComScriptErrorInfo(string description, int line, int column) {
            this.Description = description;
            this.Line = line;
            this.Column = column;
        }

        public string Description { get; private set; }
        public int Line           { get; private set; }
        public int Column         { get; private set; }
    }
}
