using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using System.Text;

using MSScriptControl;

using MbUnit.JavaScript.Engines.Microsoft.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class MicrosoftScriptControlEngine : IScriptEngine {
        private readonly IScriptControl control = new ScriptControlClass {
            Language = "JScript",
            AllowUI = false,
            UseSafeSubset = false,
            Timeout = 500000
        };

        private readonly IThreadingRequirement threading = new MtaThreadOnly();
        private readonly ComScriptConverter converter;

        public MicrosoftScriptControlEngine() {
            this.converter = new ComScriptConverter(threading);
        }

//        private ScriptFunction CreateExceptionWrapper() {
//            var wrapperCode = @"
//                function(wrapped, wrappedThis, wrappedArguments) {
//                    var result;
//                    var error;
//                    try {
//                        result = wrapped.apply(wrappedThis, wrappedArguments);
//                    }
//                    catch (ex) {
//                        error = ex;
//                    }
//
//                    return { result : result, error : error };
//                }
//            ";

//            wrapperCode = wrapperCode.Replace("                ", "");

//            var converter = new FunctionConverter();
//            var wrapperRaw = control.Eval(wrapperCode);

//            var wrapper = converter.ConvertFromScript((IExpando)wrapperRaw);
//            return wrapper;
//        }

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
            var wrapperBuilder = new StringBuilder();
            wrapperBuilder.AppendLine("(function() {")
                          .AppendLine("    var result;")
                          .AppendLine("    var error;")
                          .AppendLine("    try {")
                              .Append("        result = ").Append(expression).AppendLine(";")
                          .AppendLine("    }")
                          .AppendLine("    catch (ex) {")
                          .AppendLine("        error = ex;")
                          .AppendLine("    }")
                          .AppendLine()
                          .AppendLine("    return { result : result, error : error };")
                          .AppendLine("})()");

            var rawResultOrError = (IExpando)control.Eval(wrapperBuilder.ToString());
            var resultOrError = (IScriptObject)this.converter.ConvertFromScript(rawResultOrError);

            this.EnsureNotError(resultOrError);
            return resultOrError["result"];
        }

        private void EnsureNotError(IScriptObject resultOrError) {
            var error = resultOrError["error"];
            if (error == null)
                return;

            throw new ScriptException("JavaScript evaluation failed.", error);
        }
    }
}
