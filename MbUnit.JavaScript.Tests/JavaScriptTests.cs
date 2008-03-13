using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Tests {
    public class JavaScriptTests : JavaScriptFixtureBase {
        public override IEnumerable<JavaScriptReference> GetScripts() {
            return new[] {
                JavaScriptReference.Resources("Test.js$") 
            };
        }
    }
}
