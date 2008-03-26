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
using System.Reflection;
using System.Runtime.InteropServices.Expando;

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
