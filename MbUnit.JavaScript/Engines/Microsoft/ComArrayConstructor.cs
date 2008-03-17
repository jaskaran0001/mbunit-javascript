using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices.Expando;

using MSScriptControl;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class ComArrayConstructor : IComArrayConstructor {
        private readonly IExpando arrayConstructor;

        public ComArrayConstructor(IScriptControl control) {
            arrayConstructor = (IExpando)control.Eval(@"({ create : function() {
                var array = [];
                for (var i = 0; i < arguments.length; i++) {
                    array[i] = arguments[i];
                }
                return array;
            }})");
        }

        public IExpando ToScriptArray(object[] original) {
            return (IExpando)arrayConstructor.InvokeMember(
                "create", BindingFlags.InvokeMethod, null, arrayConstructor, original, null, null, null
            );
        }
    }
}
