using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using MbUnit.JavaScript.Engines;

namespace MbUnit.JavaScript.Tests.Of.Engines {
    partial class ScriptEngineTest {
        [Test]
        public void TestExceptionDuringEval() {
            ExceptionAssert.Throws<ScriptException>(
                () => this.Eval<object>("(function() { throw 'some error'; })()"),
                ex => Assert.AreEqual(ex.WrappedException, "some error")
            );
        }

        //[Test]
        //public void TestExceptionFromFunction() {
        //    var function = this.Eval<IScriptFunction>("function() { throw 'some error'; }").ToDelegate();

        //    ExceptionAssert.Throws<ScriptException>(
        //        () => function(),
        //        ex => Assert.AreEqual(ex.WrappedException, "some error")
        //    );
        //}

        [Test]
        public void TestSyntaxErrorDuringLoadCode() {
            ExceptionAssert.Throws<ScriptSyntaxException>(
                () => this.engine.Load("var a = 5; var b = "),
                ex => Assert.AreEqual(1, ex.Line)
                    // ashmind: I think it is impossible to require 
                    // consistent column number in all implementations 
            );
        }
    }
}
