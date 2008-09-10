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
using System.Reflection;
using System.Runtime.InteropServices.Expando;
using System.Text;

using MbUnit.JavaScript.Engines.Microsoft.Debugging;
using MbUnit.JavaScript.Engines.Microsoft.Invokers;
using MbUnit.JavaScript.Engines.Microsoft.Threading;
using MbUnit.JavaScript.Properties;

namespace MbUnit.JavaScript.Engines.Microsoft {
    public class ComActiveScriptEngine : IScriptEngine, IWrappedResultParser {
        private readonly IThreadingRequirement threading = new SingleThreadOnly();
        private ComScriptHost host;
        private ComScriptDebugSupport debugSupport;
        private readonly ComScriptConverter converter;

        public ComActiveScriptEngine() {
            IComArrayConstructor arrayConstructor = null;

            this.threading.InvokeAsRequired(() => {
                this.debugSupport = new ComScriptDebugSupport();
                this.host = new ComScriptHost(this.debugSupport);

                arrayConstructor = new ComArrayConstructor(host.Eval);
            });

            var exceptionWrapper = this.CreateExceptionWrapper();
            var scriptInvoker = new WrappedComScriptInvoker(exceptionWrapper, DirectComScriptInvoker.Default, this, arrayConstructor);

            this.converter = new ComScriptConverter(scriptInvoker, threading, arrayConstructor);
        }

        private IExpando CreateExceptionWrapper() {
            IExpando wrapper = null;
            threading.InvokeAsRequired(
                () => wrapper = this.CreateExceptionWrapperNoThreading()
            );
            return wrapper;
        }

        private IExpando CreateExceptionWrapperNoThreading() {
            var wrapperCode = new StringBuilder()
                .AppendLine("({ wrap :")
                .AppendFormat(
                    Resources.ScriptExceptionWrapper, 
                        "wrapped, wrappedThis, wrappedArguments",
                        "wrapped.apply(wrappedThis || this, wrappedArguments)"
                )
                .AppendLine()
                .Append("})")
                .ToString();

            var wrapperRaw = (IExpando)this.host.Eval(wrapperCode);
            var wrapperFunction = wrapperRaw.GetProperty("wrap", BindingFlags.Instance).GetValue(wrapperRaw, null);

            return (IExpando)wrapperFunction;
        }

        public void Load(Script script) {
            this.threading.InvokeAsRequired(
                () => this.host.LoadScript(script)
            );
        }

        public object Eval(string expression) {
            object result = null;
            this.threading.InvokeAsRequired(() => result = this.EvalNoThreading(expression));
            return result;
        }

        private object EvalNoThreading(string expression) {
            var wrapped = string.Format(Resources.ScriptExceptionWrapper, "", expression);
            var rawResultOrError = (IExpando)this.host.Eval("(" + wrapped + ")()");

            return this.ProcessRawResult(rawResultOrError);
        }

        private object ProcessRawResult(object rawResultOrError) {
            var resultOrError = (IScriptObject)this.converter.ConvertFromScript(rawResultOrError);
            this.EnsureNotError(resultOrError);

            return resultOrError["result"];            
        }

        private void EnsureNotError(IScriptObject resultOrError) {
            var error = resultOrError["error"];
            if (error == null)
                return;

            string message = this.GetErrorMessage(error);
            throw new ScriptException(message, error);
        }

        private string GetErrorMessage(object scriptError) {
            var errorObject = scriptError as IScriptObject;
            if (errorObject == null || !errorObject.ContainsKey("message"))
                return "JavaScript evaluation failed: " + scriptError + ".";

            return (string)errorObject["message"];
        }

        #region IWrappedResultParser Members

        object IWrappedResultParser.GetResult(object wrapperFunctionResult) {
            return this.ProcessRawResult(wrapperFunctionResult);
        }

        #endregion

        public void Dispose() {
            this.host.Dispose();
        }
    }
}
