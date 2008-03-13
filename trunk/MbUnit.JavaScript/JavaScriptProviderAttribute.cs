using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public sealed class JavaScriptProviderAttribute : Attribute {
    }
}
