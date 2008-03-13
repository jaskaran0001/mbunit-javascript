using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using System.Text;

using MbUnit.JavaScript.Engines.Microsoft.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft.Types
{
    internal class ThreadAwareComScriptObject : ComScriptObject {
        private readonly IThreadingRequirement threading;

        public ThreadAwareComScriptObject(IExpando innerObject, ComScriptConverter converter, IThreadingRequirement threading)
            : base(innerObject, converter)
        {
            this.threading = threading;
        }

        public override void Add(string key, object value) {
            Synchronize(() => base.Add(key, value));
        }

        public override bool TryGetValue(string key, out object value) {
            var result = Synchronize(() => {
                object synchronizedValue = null;
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
