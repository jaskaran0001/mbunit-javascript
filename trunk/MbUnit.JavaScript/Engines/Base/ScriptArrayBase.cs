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
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Base {
    public abstract class ScriptArrayBase : IScriptArray {
        protected const int IndexNotFound = -1;

        public abstract object this[int index] { get; set; }

        public abstract void Insert(int index, object item);
        public virtual void Add(object item) {
            this.Insert(this.Count, item);
        }

        public abstract void RemoveAt(int index);

        public virtual bool Remove(object item) {
            int index = this.IndexOf(item);
            if (index == IndexNotFound)
                return false;

            this.RemoveAt(index);
            return true;
        }
     
        public virtual void Clear() {
            for (int i = this.Count - 1; i >= 0; i--) {
                this.RemoveAt(i);
            }
        }

        public virtual int IndexOf(object item) {
            int index = 0;
            bool found = false;

            foreach (var testItem in this) {
                found = object.Equals(testItem, item);
                if (found)
                    break;

                index += 1;
            }

            return found ? index : IndexNotFound;
        }

        public virtual bool Contains(object item) {
            return this.IndexOf(item) != IndexNotFound;
        }

        public void CopyTo(object[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public abstract int Count { get; }

        bool ICollection<object>.IsReadOnly {
            get { return false; }
        }
        
        public IEnumerator<object> GetEnumerator() {
            int count = this.Count;
            for (int i = 0; i < count; i++) {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override string ToString() {
            var builder = new StringBuilder();

            builder.Append("[");
            foreach (var value in this) {
                builder.Append(value)
                       .Append(", ");
            }

            int lastCommaLength = ", ".Length;
            builder.Remove(builder.Length - lastCommaLength, lastCommaLength);

            builder.Append("]");

            return builder.ToString();
        }
    }
}