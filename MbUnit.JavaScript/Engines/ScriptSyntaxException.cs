using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MbUnit.JavaScript.Engines {
    [Serializable]
    public class ScriptSyntaxException : Exception {
        public ScriptSyntaxException() : base() { }
        public ScriptSyntaxException(string message, int line, int column)
            : base(message) 
        {
            this.Line = line;
            this.Column = column;
        }

        public ScriptSyntaxException(string message, int line, int column, Exception innerException)
            : base(message, innerException) 
        {
            this.Line = line;
            this.Column = column;
        }

        protected ScriptSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public int Line { get; private set; }
        public int Column { get; private set; }
    }
}
