using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.Expando;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Invokers {
    internal class DirectComScriptInvoker : IComScriptInvoker {
        public static DirectComScriptInvoker Default { get; private set; }

        static DirectComScriptInvoker() {
            Default = new DirectComScriptInvoker();
        }

        public object Invoke(IExpando owner, string methodName, params object[] args) {
            return owner.InvokeMember(
                methodName, BindingFlags.InvokeMethod, null, owner, args, null, null, null
            );
        }
    }
}