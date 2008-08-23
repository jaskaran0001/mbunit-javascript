using System;
using System.Collections.Generic;

namespace MbUnit.JavaScript {
    public class ScriptInfo {
        public string Name { get; private set; }
        public string Content { get; private set; }

        public ScriptInfo(string name, string content) {
            this.Name = name;
            this.Content = content;
        }
    }
}
