using System;
using System.Collections.Generic;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public class Script : IEquatable<Script> {
        public string Name { get; private set; }
        public string Content { get; private set; }
        public IScriptReference LoadedFrom { get; private set; }
        
        public Script(string content) : this(string.Empty, content, null) {
        }

        public Script(string name, string content, IScriptReference loadedFrom) {
            this.Name = name;
            this.Content = content;
            this.LoadedFrom = loadedFrom;
        }

        public static implicit operator Script(string content) {
            return new Script(content);
        }

        public override int GetHashCode() {
            return this.Content.GetHashCode();
        }

        public override bool Equals(object obj) {
            var script = obj as Script;
            if (script == null)
                return false;

            return this.Equals(script);
        }

        public bool Equals(Script other) {
            return this.Content == other.Content;
        }
    }
}