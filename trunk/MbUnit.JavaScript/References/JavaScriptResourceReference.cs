using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MbUnit.JavaScript.References {
    internal class JavaScriptResourceReference : IJavaScriptReference {
        public string ResourceName { get; private set; }
        public Assembly Assembly { get; private set; }

        public JavaScriptResourceReference(string resourceName, Assembly assembly) {
            this.ResourceName = resourceName;
            this.Assembly = assembly;
        }

        public string LoadContent() {
            using (var stream = this.Assembly.GetManifestResourceStream(this.ResourceName))
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        public bool Equals(JavaScriptResourceReference reference) {
            if (reference == null)
                return false;

            return this.Assembly == reference.Assembly
                && this.ResourceName == reference.ResourceName;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as JavaScriptResourceReference);
        }

        public override int GetHashCode() {
            return this.Assembly.GetHashCode() ^ this.ResourceName.GetHashCode();
        }
    }
}