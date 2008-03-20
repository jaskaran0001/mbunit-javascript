using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Tests {
    [JavaScriptFixture]
    [JavaScriptResourceReference("MbUnit.JavaScript.Tests.js.AssertTest.js", typeof(JavaScriptTests))]
    [JavaScriptResourceReference("MbUnit.JavaScript.Tests.js.ExpectedExceptionTest.js", typeof(JavaScriptTests))]
    public class JavaScriptTests {
    }
}
