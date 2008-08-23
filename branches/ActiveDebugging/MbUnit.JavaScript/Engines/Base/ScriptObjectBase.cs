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
    public abstract class ScriptObjectBase : IScriptObject {
        public abstract void Add(string key, object value);
        public abstract bool ContainsKey(string key);

        public abstract bool Remove(string key);
        public abstract bool TryGetValue(string key, out object value);

        public abstract ICollection<string> Keys    { get; }
        public abstract ICollection<object> Values  { get; }

        public abstract object this[string key] { get; set; }
        
        public abstract void Clear();

        public abstract int Count { get; }

        public abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        public abstract object Invoke(string key, params object[] args);

        #region ICollection<KeyValuePair<string, object>> members

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
            this.Add(item.Key, item.Value);    
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) {
            object value;
            bool has = this.TryGetValue(item.Key, out value);

            return has && object.Equals(value, item.Value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly {
            get { return false; }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) {
            var cast = this as ICollection<KeyValuePair<string, object>>;
            if (!cast.Contains(item))
                return false;

            return this.Remove(item.Key);
        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.Append("{");
            foreach (var pair in this) {
                builder.Append(pair.Key)
                       .Append(" : ")
                       .Append(pair.Value)
                       .Append(", ");
            }

            if (this.Count > 0) {
                int lastCommaLength = ", ".Length;
                builder.Remove(builder.Length - lastCommaLength, lastCommaLength);
            }

            builder.Append("}");

            return builder.ToString();
        }
    }
}
