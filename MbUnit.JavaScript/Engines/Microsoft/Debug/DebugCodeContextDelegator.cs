using System;
using System.Collections.Generic;

using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    internal class DebugCodeContextDelegator : IDebugCodeContext {
        private readonly IDebugCodeContext actual;

        public DebugCodeContextDelegator(IDebugCodeContext actual) {
            this.actual = actual;
        }
        
        public void GetDocumentContext(out IDebugDocumentContext ppsc) {
            DebugLog.Log("IDebugCodeContext.GetDocumentContext() -> ");
            DebugLog.BeginIndent();

            ppsc = null;
            try {
                this.actual.GetDocumentContext(out ppsc);
            }
            catch (Exception ex) {
                DebugLog.EndIndent();
                DebugLog.Log("!{0}", ex);
                return;
            }

            DebugLog.EndIndent();
            DebugLog.Log("[{0}]", ppsc);
        }

        public void SetBreakPoint(tagBREAKPOINT_STATE bps) {
            DebugLog.Log("IDebugCodeContext.SetBreakPoint({0}) -> ", bps);
            DebugLog.BeginIndent();

            try {
                this.actual.SetBreakPoint(bps);
            }
            catch (Exception ex) {
                DebugLog.EndIndent();
                DebugLog.Log("!{0}", ex);
                return;
            }

            DebugLog.EndIndent();
            DebugLog.Log("[]");
        }

        public override string ToString() {
            return this.GetType().Name;
        }
    }
}
