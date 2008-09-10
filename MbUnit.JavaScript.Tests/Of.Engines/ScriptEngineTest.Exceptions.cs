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
                () => this.engine.Load("var a = 5; var b = "),
                ex => Assert.AreEqual(1, ex.Line)
                    // ashmind: I think it is impossible to require 
                    // consistent column number in all implementations 
            );
        }
    }
}
