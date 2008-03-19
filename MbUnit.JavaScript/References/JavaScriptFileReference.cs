using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MbUnit.JavaScript.References {
    internal class JavaScriptFileReference : JavaScriptReference {
        public string Path                  { get; private set; }
        public string Pattern               { get; private set; }
        public SearchOption SearchOption    { get; private set; }

        public JavaScriptFileReference(string path, string pattern)
            : this (path, pattern, SearchOption.TopDirectoryOnly)
        {
        }

        public JavaScriptFileReference(string path, string pattern, SearchOption option) {
            this.Path = path;
            this.Pattern = pattern;
            this.SearchOption = option;
        }

        public override IEnumerable<string> LoadAll() {
            var files = Directory.GetFiles(this.Path, this.Pattern, this.SearchOption);
            foreach (var file in files) {
                yield return File.ReadAllText(file);
            }
        }

        public bool Equals(JavaScriptFileReference reference) {
            if (reference == null)
                return false;

            return this.Path == reference.Path
                || this.Pattern == reference.Pattern
                || this.SearchOption == reference.SearchOption;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as JavaScriptFileReference);
        }

        public override int GetHashCode() {
            return this.Path.GetHashCode() ^ this.Pattern.GetHashCode() ^ this.SearchOption.GetHashCode();
        }
    }
}