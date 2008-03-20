using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public abstract class JavaScriptReference : IJavaScriptReference {
        public abstract IEnumerable<string> LoadAll();

        public static JavaScriptReference Resources(string pattern) {
            return Resources(pattern, Assembly.GetCallingAssembly());
        }

        public static JavaScriptReference Resources(string pattern, Assembly assembly) {
            return new JavaScriptResourceReference(pattern, assembly);
        }

        public static JavaScriptReference Files(string path, string pattern) {
            return new JavaScriptFileReference(path, pattern);
        }

        public static JavaScriptReference Files(string path, string pattern, SearchOption searchOption) {
            return new JavaScriptFileReference(path, pattern, searchOption);
        }
    }
}
