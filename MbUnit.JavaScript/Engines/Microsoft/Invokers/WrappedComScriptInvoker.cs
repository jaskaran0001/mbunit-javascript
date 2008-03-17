using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.Expando;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Invokers {
    internal class WrappedComScriptInvoker : IComScriptInvoker {
        private readonly IExpando wrappingFunction;
        private readonly IComScriptInvoker wrappingFunctionInvoker;
        private readonly IWrappedResultParser resultParser;
        private readonly IComArrayConstructor arrayConstructor;

        public WrappedComScriptInvoker(IExpando wrappingFunction, IComScriptInvoker wrappingFunctionInvoker, IWrappedResultParser resultParser, IComArrayConstructor arrayConstructor) {
            this.wrappingFunction = wrappingFunction;
            this.wrappingFunctionInvoker = wrappingFunctionInvoker;
            this.resultParser = resultParser;
            this.arrayConstructor = arrayConstructor;
        }

        public object Invoke(IExpando owner, string name, params object[] args) {
            var originalFunction = owner.GetProperty(name, BindingFlags.Instance).GetValue(owner, null);
            var actualArgs = new[] { owner, originalFunction, owner, this.arrayConstructor.ToScriptArray(args) };

            var result = this.wrappingFunctionInvoker.Invoke(this.wrappingFunction, "call", actualArgs);
            return resultParser.GetResult(result);
        }
    }
}
