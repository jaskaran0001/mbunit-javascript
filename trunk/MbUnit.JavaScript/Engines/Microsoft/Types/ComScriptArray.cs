using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using System.Text;

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
