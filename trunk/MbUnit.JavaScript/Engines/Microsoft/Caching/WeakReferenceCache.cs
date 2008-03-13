using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Caching {
    internal delegate T Creator<T>();

    internal class WeakReferenceCache<TKey, TValue> {
        private readonly IDictionary<WeakReference, WeakReference> innerCache = new Dictionary<WeakReference, WeakReference>(
            new WeakReferenceEqualityComparer()
        );

        public TValue Load(TKey key, Creator<TValue> creator) {
            var keyReference = new WeakReference(key);

            WeakReference valueReference;
            bool cached = this.innerCache.TryGetValue(keyReference, out valueReference);

            TValue value;
            if (!cached || !valueReference.IsAlive) {
                value = creator();
                this.innerCache[keyReference] = new WeakReference(value);
            }
            else {
                value = (TValue)valueReference.Target;
            }

            return value;
        }
    }
}
