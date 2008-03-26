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
using System.Runtime.InteropServices.Expando;

using MbUnit.JavaScript.Engines.Microsoft.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft.Types
{
    internal class ThreadAwareComScriptObject : ComScriptObject {
        private readonly IThreadingRequirement threading;

        public ThreadAwareComScriptObject(IExpando innerObject, ComScriptConverter converter, IComScriptInvoker invokeWrapper, IThreadingRequirement threading)
            : base(innerObject, converter, invokeWrapper)
        {
            this.threading = threading;
        }

        public override void Add(string key, object value) {
            Synchronize(() => base.Add(key, value));
        }

        public override bool TryGetValue(string key, out object value) {
            var result = Synchronize(() => {
                object synchronizedValue;
                var found = base.TryGetValue(key, out synchronizedValue);
                
                return new { Found = found, Value = synchronizedValue };
            });

            value = result.Value;
            return result.Found;
        }

        public override object this[string key] {
            get { return Synchronize(() => base[key]); }
            set { Synchronize(() => base[key] = value); }
        }

        public override object Invoke(string key, params object[] args) {
            return Synchronize(() => base.Invoke(key, args));
        }

        private void Synchronize(Action action) {
            this.threading.InvokeAsRequired(action);
        }

        private TResult Synchronize<TResult>(Func<TResult> function) {
            TResult result = default(TResult);
            this.threading.InvokeAsRequired(() => result = function());

            return result;
        }
    }
}
