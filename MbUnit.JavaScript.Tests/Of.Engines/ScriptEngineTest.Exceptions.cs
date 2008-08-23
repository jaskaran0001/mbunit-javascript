using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using MbUnit.JavaScript.Engines;

namespace MbUnit.JavaScript.Tests.Of.Engines {
    partial class ScriptEngineTest {
        private const string ThrowingFunction = "function() { throw 'some error'; }";

        [Test]
        public void TestExceptionMessageFromFunction() {
            var function = this.Eval<IScriptFunction>(
                "(function() { throw { message : 'x' }; })"
            ).ToDelegate();

            ExceptionAssert.Throws<ScriptException>(
                () => function(),
                ex => Assert.AreEqual(ex.Message, "x")
            );
        }

        [Test]
        public void TestExceptionDuringEval() {
            ExceptionAssert.Throws<ScriptException>(
                () => this.Eval<object>("(" + ThrowingFunction + ")()"),
                ex => Assert.AreEqual(ex.WrappedException, "some error")
            );
        }

        [Test]
        public void TestExceptionFromFunction() {
            var function = this.Eval<IScriptFunction>(ThrowingFunction).ToDelegate();

            ExceptionAssert.Throws<ScriptException>(
                () => function(),
                ex => Assert.AreEqual(ex.WrappedException, "some error")
            );
        }

        [Test]
        public void TestExceptionFromMethod() {
            var @object = this.Eval<IScriptObject>("{ test : " + ThrowingFunction + " }");

            ExceptionAssert.Throws<ScriptException>(
                () => @object.Invoke("test"),
                ex => Assert.AreEqual(ex.WrappedException, "some error")
            );
        }

        [Test]
        public void TestSyntaxErrorDuringLoadCode() {
            ExceptionAssert.Throws<ScriptSyntaxException>(
                () => this.Load("var a = 5; var b = "),
                ex => Assert.AreEqual(1, ex.Line)
                    // ashmind: I think it is impossible to require 
                    // consistent column number in all implementations 
            );
        }
    }
}
