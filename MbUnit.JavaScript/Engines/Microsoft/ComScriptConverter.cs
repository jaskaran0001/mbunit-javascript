using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.Expando;
using System.Text;

using MbUnit.JavaScript.Engines.Microsoft.Caching;
using MbUnit.JavaScript.Engines.Microsoft.Threading;
using MbUnit.JavaScript.Engines.Microsoft.Types;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class ComScriptConverter {
        private readonly WeakReferenceCache<IExpando, IComObjectWrapper> cache = new WeakReferenceCache<IExpando, IComObjectWrapper>();
        private readonly IComScriptInvoker invoker;       
        private readonly IThreadingRequirement threading;
        private readonly IComArrayConstructor arrayConstructor;

        public ComScriptConverter(IComScriptInvoker invoker, IThreadingRequirement threading, IComArrayConstructor arrayConstructor) {
            this.invoker = invoker;  
            this.threading = threading;
            this.arrayConstructor = arrayConstructor;
        }

        public object ConvertToScript(object value) {
            var wrapper = value as IComObjectWrapper;
            if (wrapper != null)
                return wrapper.ComObject;

            var array = value as object[];
            if (array != null) {
                return this.arrayConstructor.ToScriptArray(array);
            }

            return value;
        }

        public object ConvertFromScript(object value) {
            // ashmind: Is it possible to do it better without introducing a factory
            // for each wrapper type? I fear it is not possible.
            
            var expando = value as IExpando;
            if (expando == null) {
                // ashmind: cryptic special case
                if (value is DBNull)
                    return null;

                return value;
            }

            return this.WrapFromScriptWithCaching(expando);
        }

        private IComObjectWrapper WrapFromScriptWithCaching(IExpando comObject) {
            return this.cache.Load(
                comObject, () => this.WrapFromScript(comObject)
            );
        }

        private IComObjectWrapper WrapFromScript(IExpando comObject) {
            return this.WrapFromScript(comObject, this.invoker);
        }

        private IComObjectWrapper WrapFromScript(IExpando comObject, IComScriptInvoker invokeWrapper) {
            var wrapper = new ThreadAwareComScriptObject(comObject, this, invokeWrapper, this.threading);

            if (IsFunction(wrapper))
                return new ComScriptFunction(wrapper);

            if (IsArray(wrapper))
                return new ComScriptArray(wrapper);

            return wrapper;
        }

        private bool IsFunction(ComScriptObject wrapper) {
            return wrapper.ContainsKey("call") && wrapper.ContainsKey("apply");
        }

        private bool IsArray(ComScriptObject wrapper) {
            return wrapper.InnerObject is IEnumerable;
        }
    }
}
