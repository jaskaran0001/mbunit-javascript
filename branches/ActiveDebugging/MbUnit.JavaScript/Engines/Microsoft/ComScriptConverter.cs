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
using System.Collections;
using System.Runtime.InteropServices.Expando;

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
