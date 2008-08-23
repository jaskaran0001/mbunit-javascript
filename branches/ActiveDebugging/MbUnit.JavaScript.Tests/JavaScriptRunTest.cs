using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Core.Invokers;

using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tests {
    [TestFixture]
    public class JavaScriptRunTest : MockingTestBase {
        [Test]
        public void TestReflect() {
            var run = new JavaScriptRun(Mock<IJavaScriptReferenceExtractor>());
            var invokers = run.Reflect(typeof(JavaScriptTests));

            Assert.IsNotNull(invokers);
        }
    }
}
