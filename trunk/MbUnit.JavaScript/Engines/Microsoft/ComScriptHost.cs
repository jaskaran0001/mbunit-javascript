using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MbUnit.JavaScript.Engines.Microsoft.Interop;
using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class ComScriptHost : IActiveScriptSite, IDisposable {
        private readonly JScript engine = new JScript();
        private readonly IActiveScriptParse activeScriptParse;
        private readonly IActiveScript activeScript;

        private ComScriptErrorInfo lastError;

        public ComScriptHost() {
            this.activeScriptParse = engine as IActiveScriptParse;
            this.activeScriptParse.InitNew();

            this.activeScript = engine as IActiveScript;
            this.activeScript.SetScriptSite(this);
            this.activeScript.SetScriptState(tagSCRIPTSTATE.SCRIPTSTATE_CONNECTED);
        }

        public void LoadScript(string script) {
            //const int SyntaxErrorCode = unchecked((int)0x800A03EA);
            const int ReportedErrorCode = unchecked((int)0x80020101);

            try {
                this.ParseScriptText(script, false);
            }
            catch (COMException ex) {
                if (ex.ErrorCode == ReportedErrorCode) {
                    throw new ScriptSyntaxException(
                        this.lastError.Description,
                        this.lastError.Line,
                        this.lastError.Column,
                        ex
                    );
                }
                else
                    throw;
            }
        }

        public object Eval(string expression) {
            return this.ParseScriptText(expression, true);
        }

        private object ParseScriptText(string script, bool isExpression) {
            var flags = isExpression ? tagSCRIPTTEXT.SCRIPTTEXT_ISEXPRESSION : 0;

            stdole.EXCEPINFO excepinfo;
            object result;
            this.activeScriptParse.ParseScriptText(
                script, null, IntPtr.Zero, null, 0, 0, flags, out result, out excepinfo
            );

            return result;
        }


        #region IActiveScriptSite Members

        void IActiveScriptSite.GetDocVersionString(out string pbstrVersion) {
            throw new NotImplementedException();
        }

        void IActiveScriptSite.GetItemInfo(string pstrName, uint dwReturnMask, out object ppiunkItem, out Type ppti) {
            throw new NotImplementedException();
        }

        void IActiveScriptSite.GetLCID(out uint plcid) {
            throw new NotImplementedException();
        }

        void IActiveScriptSite.OnEnterScript() {
        }

        void IActiveScriptSite.OnLeaveScript() {
        }

        void IActiveScriptSite.OnScriptError(IActiveScriptError pscripterror) {
            stdole.EXCEPINFO excepinfo;
            pscripterror.RemoteGetExceptionInfo(out excepinfo);

            uint sourceContext;
            uint lineNumber;
            int position;
            pscripterror.GetSourcePosition(out sourceContext, out lineNumber, out position);

            lastError = new ComScriptErrorInfo(excepinfo.bstrDescription, ((int)lineNumber + 1), position);
        }

        void IActiveScriptSite.OnScriptTerminate(ref object pVarResult, ref stdole.EXCEPINFO pexcepinfo) {
        }

        void IActiveScriptSite.OnStateChange(tagSCRIPTSTATE ssScriptState) {
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            this.activeScript.Close();
        }

        #endregion
    }
}
