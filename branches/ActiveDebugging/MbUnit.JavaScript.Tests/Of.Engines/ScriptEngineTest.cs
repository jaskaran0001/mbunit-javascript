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
