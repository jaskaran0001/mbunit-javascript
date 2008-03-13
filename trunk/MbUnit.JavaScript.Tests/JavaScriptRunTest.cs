using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Core.Invokers;

namespace MbUnit.JavaScript.Tests {
    [TestFixture]
    public class JavaScriptRunTest {
        [Test]
        public void TestReflect() {
            var run = new JavaScriptRun();
            var invokers = run.Reflect(typeof(JavaScriptTests));

            Assert.IsNotNull(invokers);
        }
    }
}
