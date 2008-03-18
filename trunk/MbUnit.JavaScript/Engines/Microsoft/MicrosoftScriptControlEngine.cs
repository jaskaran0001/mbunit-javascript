using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using System.Text;

using MSScriptControl;

using MbUnit.JavaScript.Engines.Microsoft.Invokers;
using MbUnit.JavaScript.Engines.Microsoft.Threading;
using MbUnit.JavaScript.Properties;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class MicrosoftScriptControlEngine : IScriptEngine, IWrappedResultParser {
        private readonly IScriptControl control = new ScriptControlClass {
            Language = "JScript",
            AllowUI = false,
            UseSafeSubset = false,
            Timeout = 500000
        };

        private readonly IThreadingRequirement threading = new MtaThreadOnly();
        private readonly ComScriptConverter converter;

        public MicrosoftScriptControlEngine() {
            IComArrayConstructor arrayConstructor = null;
            this.threading.InvokeAsRequired(
                () => arrayConstructor = new ComArrayConstructor(this.control)
            );

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

            var wrapperRaw = (IExpando)this.control.Eval(wrapperCode);
            var wrapperFunction = wrapperRaw.GetProperty("wrap", BindingFlags.Instance).GetValue(wrapperRaw, null);

            return (IExpando)wrapperFunction;
        }

        public void Load(string script) {
            const int SyntaxErrorCode = -2146827286;

            try {
                control.AddCode(script);
            }
            catch (COMException ex) {
                if (ex.ErrorCode == SyntaxErrorCode) {
                    throw new ScriptSyntaxException(
                        control.Error.Text,
                        control.Error.Line,
                        control.Error.Column
                    );
                }
                else
                    throw;
            }
        }

        public object Eval(string expression) {
            object result = null;
            this.threading.InvokeAsRequired(() => result = this.EvalNoThreading(expression));
            return result;
        }

        private object EvalNoThreading(string expression) {
            var wrapped = string.Format(Resources.ScriptExceptionWrapper, "", expression);
            var rawResultOrError = (IExpando)control.Eval("(" + wrapped + ")()");

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
                return "JavaScript evaluation failed: " + scriptError.ToString() + ".";

            return (string)errorObject["message"];
        }

        #region IWrappedResultParser Members

        object IWrappedResultParser.GetResult(object wrapperFunctionResult) {
            return this.ProcessRawResult(wrapperFunctionResult);
        }

        #endregion
    }
}
