using System;
using System.Collections.Generic;

using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    internal class DebugDocumentContextDelegator : IDebugDocumentContext {
        private readonly IDebugDocumentContext context;
        private readonly uint contextCookie;

        public DebugDocumentContextDelegator(IDebugDocumentContext context, uint contextCookie) {
            this.context = context;
            this.contextCookie = contextCookie;
        }

        public void GetDocument(out IDebugDocument ppsd) {
            DebugLog.Log("IDebugDocumentContext[{0}].GetDocument()", this.contextCookie);
            this.context.GetDocument(out ppsd);
        }

        public void EnumCodeContexts(out IEnumDebugCodeContexts ppescc) {
            DebugLog.Log("IDebugDocumentContext[{0}].EnumCodeContexts()", this.contextCookie);
            this.context.EnumCodeContexts(out ppescc);
        }

        public override string ToString() {
            return this.GetType().Name + "[" + this.contextCookie + "]";
        }
    }
}