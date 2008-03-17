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
