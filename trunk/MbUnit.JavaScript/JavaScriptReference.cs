using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public abstract class JavaScriptReference {
        public abstract IEnumerable<string> LoadAll();

        public static JavaScriptReference Resources(string pattern) {
            return Resources(pattern, Assembly.GetCallingAssembly());
        }

        public static JavaScriptReference Resources(string pattern, Assembly assembly) {
            return new JavaScriptResourceReference(pattern, assembly);
        }
    }
}
