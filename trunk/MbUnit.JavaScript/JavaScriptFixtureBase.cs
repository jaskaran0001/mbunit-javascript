using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    [JavaScriptFixture]
    public abstract class JavaScriptFixtureBase {
        protected JavaScriptFixtureBase() {
        }

        [JavaScriptProvider]
        public abstract IEnumerable<JavaScriptReference> GetScripts();
    }
}
