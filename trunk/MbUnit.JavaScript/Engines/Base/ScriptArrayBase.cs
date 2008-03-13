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