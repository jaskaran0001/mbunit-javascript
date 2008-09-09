using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    public class Script {
        public string Name { get; private set; }
        public string Content { get; private set; }

        public Script(string content) : this(string.Empty, content) {
        }

        public Script(string name, string content) {
            this.Name = name;
            this.Content = content;
        }

        public static implicit operator Script(string content) {
            return new Script(content);
        }
    }
}