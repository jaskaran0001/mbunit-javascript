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

namespace MbUnit.JavaScript.Engines.Microsoft.Types {
    internal class ComScriptFunction : IScriptFunction, IComObjectWrapper {
        private readonly ComScriptObject innerWrapper;

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
