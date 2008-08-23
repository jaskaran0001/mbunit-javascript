using System;
using System.Collections.Generic;
using System.Text;
using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    internal class EnumDebugCodeContextsDelegator : IEnumDebugCodeContexts {
        private readonly IEnumDebugCodeContexts actual;

        public EnumDebugCodeContextsDelegator(IEnumDebugCodeContexts actual) {
            this.actual = actual;
        }

        #region IEnumDebugCodeContexts Members

        public void Clone(out IEnumDebugCodeContexts ppescc) {
            DebugLog.Log("IEnumDebugCodeContexts.Clone()");
            this.actual.Clone(out ppescc);
        }

        public void RemoteNext(uint celt, out IDebugCodeContext pscc, out uint pceltFetched) {
            this.actual.RemoteNext(celt, out pscc, out pceltFetched);
            pscc = new DebugCodeContextDelegator(pscc);

            DebugLog.Log("IEnumDebugCodeContexts.RemoteNext({0}) -> [{1}, {2}]", celt, pscc, pceltFetched);
        }

        public void Reset() {
            DebugLog.Log("IEnumDebugCodeContexts.Reset()");
            this.actual.Reset();
        }

        public void Skip(uint celt) {
            DebugLog.Log("IEnumDebugCodeContexts.Skip({0})", celt);
            this.actual.Skip(celt);
        }

        #endregion
    }
}
