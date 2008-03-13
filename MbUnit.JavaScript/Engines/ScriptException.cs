using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MbUnit.JavaScript.Engines {
    [Serializable]
    public class ScriptException : Exception {
        public ScriptException() : base("Exception happened in script code.") { }
        public ScriptException(string message, object wrappedException)
            : base(message) 
        {
            this.WrappedException = wrappedException;       
        }

        public ScriptException(string message, object wrappedException, Exception innerException)
            : base(message, innerException) 
        {
            this.WrappedException = wrappedException;
        }

        protected ScriptException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public object WrappedException { get; private set; }
    }
}
