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

using MbUnit.Framework;
using MbUnit.JavaScript.Engines;

namespace MbUnit.JavaScript.Tests.Of.Engines {
    // ashmind:
    // This should be TypeFixture, but MbUnit 2 does not support
    // row tests with type fixtres. So while there is a single engine,
    // it would work (and still be generic enough to allow future
    // extensions).

    [TestFixture]
    public partial class ScriptEngineTest {
        private IScriptEngine engine;

        [SetUp]
        public void SetUp() {
            engine = ScriptEngineFactory.Create();
        }

        [RowTest]
        [Row("'test'",     "test")]
        [Row("1",           1)]
        [Row("true",        true)]
        [Row("null",        null)]
        [Row("undefined",   null)]
        public void TestEval(string expression, object expected) {
            var result = this.Eval<object>(expression);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestEvalArray() {
            var result = this.Eval<IScriptArray>("['test', 2]");

            CollectionAssert.AreElementsEqual(
                new object[] { "test", 2 }, result
            );
        }

        [Test]
        public void TestEvalArrayWithPrototypeExtensions() {
            this.engine.Load("Array.prototype.x = 'This Should Be Ignored';");
            this.TestEvalArray();
        }

        [Test]
        public void TestEvalObject() {
            var result = this.EvalObject("{ answer: 42 }");

            Assert.IsTrue(result.ContainsKey("answer"));
            Assert.AreEqual(42, result["answer"]);
        }

        [Test]
        public void TestEvalFunction() {
            var function = this.Eval<IScriptFunction>("function(a, b) { return a + b; }").ToDelegate();
            var sum = (int)function(3, 6);

            Assert.AreEqual(3 + 6, sum);
        }

        private IScriptObject EvalObject(string expression) {
            return this.Eval<IScriptObject>(expression);
        }

        private T Eval<T>(string expression) {
            var result = this.engine.Eval("(" + expression + ")");

            if (result != null)
                Assert.IsInstanceOfType(typeof(T), result);

            return (T)result;
        }
    }
}
