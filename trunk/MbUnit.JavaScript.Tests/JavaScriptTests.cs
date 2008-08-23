using System;
using System.Collections.Generic;

namespace MbUnit.JavaScript.Tests {
    [JavaScriptFixture]
    [JavaScriptResourceReference("MbUnit.JavaScript.Tests.Of.js.AssertTest.js", typeof(JavaScriptTests))]
    [JavaScriptResourceReference("MbUnit.JavaScript.Tests.Of.js.ExpectedExceptionTest.js", typeof(JavaScriptTests))]
    [JavaScriptResourceReference("MbUnit.JavaScript.Tests.Of.js.Core.FormatterTest.js", typeof(JavaScriptTests))]
    public class JavaScriptTests {
    }
}
