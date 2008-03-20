using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public abstract class JavaScriptReferenceAttribute : Attribute {
        public abstract IJavaScriptReference Reference { get; }
    }
}
