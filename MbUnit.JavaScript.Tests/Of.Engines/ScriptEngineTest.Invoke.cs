using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using MbUnit.Framework;
using MbUnit.JavaScript.Engines;

namespace MbUnit.JavaScript.Tests.Of.Engines {
    partial class ScriptEngineTest {
        private const string CodeOfObjectWithSumFunction = "{ sum : function(a, b) { return a + b; } }";

        private void TestEvalAndInvokeSum(string code) {
            var @object = this.EvalObject(code);
            this.TestInvokeSum(@object);
        }

        private void TestInvokeSum(IScriptObject @object) {
            var sum = (int)@object.Invoke("sum", 3, 6);
            Assert.AreEqual(3 + 6, sum);
        }

        [Test]
        public void TestEvalObjectAndInvoke() {
            TestEvalAndInvokeSum(CodeOfObjectWithSumFunction);
        }

        [Test]
        public void TestEvalObjectAndInvokeFromPrototype() {
            this.Load(@"__TestObject = function() {};
                        __TestObject.prototype = " + CodeOfObjectWithSumFunction);

            Block clean = () => this.Load("delete this['__TestObject']");

            using (new Disposer(clean)) {
                TestEvalAndInvokeSum("new __TestObject()");
            }
        }

        [Test]
        public void TestInvokeWorksFromOtherMTAThread() {
            var @object = this.EvalObject(CodeOfObjectWithSumFunction);
            Exception exception = null;

            var thread = new Thread(
                () => { 
                    try {
                        this.TestInvokeSum(@object);
                    }
                    catch (Exception ex) {
                        exception = ex;
                    }
                }
            );

            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            if (exception != null)
                throw exception;
        }
    }
}
