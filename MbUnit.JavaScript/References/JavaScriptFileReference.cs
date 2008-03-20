using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MbUnit.JavaScript.References {
    internal class JavaScriptFileReference : IJavaScriptReference {
        public string Path                  { get; private set; }

        public JavaScriptFileReference(string path) {
            this.Path = path;
        }

        public string LoadContent() {
            return File.ReadAllText(this.Path);
        }

        public bool Equals(JavaScriptFileReference reference) {
            if (reference == null)
                return false;

            return this.Path == reference.Path;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as JavaScriptFileReference);
        }

        public override int GetHashCode() {
            return this.Path.GetHashCode();
        }
    }
}