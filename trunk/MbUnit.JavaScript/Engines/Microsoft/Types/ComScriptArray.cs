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

using MbUnit.JavaScript.Engines.Base;

namespace MbUnit.JavaScript.Engines.Microsoft.Types {
    internal class ComScriptArray : ScriptArrayBase, IComObjectWrapper {
        private readonly ComScriptObject innerWrapper;

        public ComScriptArray(ComScriptObject innerWrapper) {
            this.innerWrapper = innerWrapper;
        }

        public override object this[int index] {
            get { return this.innerWrapper[index.ToString()]; }
            set { this.innerWrapper[index.ToString()] = value; }
        }

        public override void Insert(int index, object item) {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public override int Count {
            get { return (int)this.innerWrapper["length"]; }
        }

        internal IExpando InnerArray {
            get { return this.innerWrapper.InnerObject; }
        }

        #region IComObjectWrapper Members

        object IComObjectWrapper.ComObject {
            get { return this.InnerArray; }
        }

        #endregion
    }
}
