using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Internal {
    internal class JavaScriptImportedFixture {
        public string Name { get; set; }
        public IEnumerable<JavaScriptRunInvoker> Invokers { get; set; }
    }
}
