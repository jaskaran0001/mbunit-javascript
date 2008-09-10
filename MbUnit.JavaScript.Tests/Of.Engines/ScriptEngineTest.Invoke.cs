/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
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
            this.engine.Load(@"__TestObject = function() {};
                               __TestObject.prototype = " + CodeOfObjectWithSumFunction);

            Block clean = () => this.engine.Load("delete this['__TestObject']");

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
