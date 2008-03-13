using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Types {
    internal class ComScriptFunction : IScriptFunction, IComObjectWrapper {
        private ComScriptObject innerWrapper;

        public ComScriptFunction(ComScriptObject innerWrapper) {
            this.innerWrapper = innerWrapper;
        }

        public object Call(object @this, params object[] args) {
            var combinedArgs = new object[args.Length + 1];

            combinedArgs[0] = @this;
            Array.Copy(args, 0, combinedArgs, 1, args.Length);

            return this.innerWrapper.Invoke("call", combinedArgs);
        }

        internal IExpando InnerFunction {
            get { return this.innerWrapper.InnerObject; }
        }

        public ScriptFunctionDelegate ToDelegate() {
            return args => this.Call(null, args);
        }

        #region IComObjectWrapper Members

        object IComObjectWrapper.ComObject {
            get { return this.InnerFunction; }
        }

        #endregion

        public override string ToString() {
            return (string)this.innerWrapper.Invoke("toString");
        }
    }
}
