using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Caching {
    public class WeakReferenceEqualityComparer : IEqualityComparer<WeakReference> {
        private static readonly object DeadReferenceTarget = new object();

        public bool Equals(WeakReference x, WeakReference y) {
            return x.IsAlive 
                && y.IsAlive
                && x.Target.Equals(y.Target);
        }

        public int GetHashCode(WeakReference obj) {
            return (obj.Target ?? DeadReferenceTarget).GetHashCode();
        }
    }
}
