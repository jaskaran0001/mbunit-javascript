/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ProcessDebugManagerLib;

using MbUnit.JavaScript.Engines.Microsoft.Debug;
using MbUnit.JavaScript.Engines.Microsoft.Debugging;
using MbUnit.JavaScript.Engines.Microsoft.Interop;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal class ComScriptHost : IActiveScriptSite, IActiveScriptSiteDebug, IDisposable {
        private readonly ComScriptDebugSupport debug;
        private readonly IActiveScriptParse activeScriptParse;
        private readonly IActiveScript activeScript;

        private readonly IActiveScript debugLogged;

        private ComScriptErrorInfo lastError;

        public ComScriptHost(ComScriptDebugSupport debug) {
            this.debug = debug;
            var engine = new JScript();

            this.activeScriptParse = engine as IActiveScriptParse;
            this.activeScriptParse.InitNew();

            this.activeScript = engine as IActiveScript;
            this.activeScript.SetScriptSite(this);
            this.activeScript.SetScriptState(tagSCRIPTSTATE.SCRIPTSTATE_CONNECTED);

            this.debugLogged = new ActiveScriptDebugDelegator(this.activeScript);
        }

        public void LoadScript(Script script) {
            //const int SyntaxErrorCode = unchecked((int)0x800A03EA);
            const int ReportedErrorCode = unchecked((int)0x80020101);
            
            try {
                this.ParseAndRegisterScript(script.Content, script.Name, false);
            }
            catch (COMException ex) {
                if (ex.ErrorCode == ReportedErrorCode) {
                    throw new ScriptSyntaxException(
                        this.lastError.Description,
                        script, this.lastError.Line, this.lastError.Column,
                        ex
                    );
                }

                throw;
            }
        }

        public object Eval(string expression) {
            return this.ParseAndRegisterScript(expression, "Eval code", true);
        }

        private object ParseAndRegisterScript(string script, string scriptName, bool isExpression) {
            script = script.Replace("\r", "");
            
            var context = this.debug.RegisterScript(this.debugLogged, scriptName, script);
            DebugLog.Log("Registered script '{0}' as {1}.", scriptName, context);

            var flags = isExpression ? tagSCRIPTTEXT.SCRIPTTEXT_ISEXPRESSION : 0;
            flags |= tagSCRIPTTEXT.SCRIPTTEXT_HOSTMANAGESSOURCE;

            stdole.EXCEPINFO excepinfo;
            object result;
            this.activeScriptParse.ParseScriptText(
                script, null, IntPtr.Zero, null, context, 0, flags, out result, out excepinfo
            );

            return result;
        }

        #region IActiveScriptSite Members

        void IActiveScriptSite.GetDocVersionString(out string pbstrVersion) {
            DebugLog.Log("IActiveScriptSite.GetDocVersionString() -> !NotImplementedException");
            throw new NotImplementedException();
        }

        void IActiveScriptSite.GetItemInfo(string pstrName, uint dwReturnMask, out object ppiunkItem, out Type ppti) {
            DebugLog.Log("IActiveScriptSite.GetItemInfo('{0}', {1}) -> !NotImplementedException", pstrName, dwReturnMask);
            throw new NotImplementedException();
        }

        void IActiveScriptSite.GetLCID(out uint plcid) {
            DebugLog.Log("IActiveScriptSite.GetLCID() -> !NotImplementedException");
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

        #region IActiveScriptSiteDebug Members

        void IActiveScriptSiteDebug.GetApplication(out IDebugApplication ppda) {
            DebugLog.Log("IActiveScriptSiteDebug.GetApplication() ->");
            ppda = this.debug.DebugApplication;
            DebugLog.Log("[{0}]", ppda);
        }

        void IActiveScriptSiteDebug.GetDocumentContextFromPosition(uint dwSourceContext, uint uCharacterOffset, uint uNumChars, out IDebugDocumentContext ppsc) {
            DebugLog.Log("IActiveScriptSiteDebug.GetDocumentContextFromPosition({0}, {1}, {2}) ->", dwSourceContext, uCharacterOffset, uNumChars);
            DebugLog.BeginIndent();

            try {
                var document = this.debug.GetDebugDocument(dwSourceContext);

                IActiveScript @as;
                uint startPosition;
                uint length;
                DebugLog.Log("document.GetScriptBlockInfo({0}) ->", dwSourceContext);
                document.GetScriptBlockInfo(dwSourceContext, out @as, out startPosition, out length);
                DebugLog.Log("[{0}, {1}, {2}]", @as, startPosition, length);

                DebugLog.Log("document.CreateDebugDocumentContext({0}, {1}) ->", startPosition + uCharacterOffset, uNumChars);
                document.CreateDebugDocumentContext(startPosition+uCharacterOffset, uNumChars, out ppsc);
                DebugLog.Log("[{0}]", ppsc);
            }
            catch (Exception ex) {
                DebugLog.Log("{0}", ex);
                DebugLog.EndIndent();
                throw;
            }

            ppsc = new DebugDocumentContextDelegator(ppsc, dwSourceContext);
            DebugLog.Log("[{0}]", ppsc);
            DebugLog.EndIndent();
        }

        void IActiveScriptSiteDebug.GetRootApplicationNode(out IDebugApplicationNode ppdanRoot) {
            DebugLog.Log("IActiveScriptSiteDebug.GetRootApplicationNode() ->");
            this.debug.DebugApplication.GetRootNode(out ppdanRoot);
            DebugLog.Log("[{0}]", ppdanRoot);
        }

        void IActiveScriptSiteDebug.OnScriptErrorDebug(IActiveScriptErrorDebug pErrorDebug, out int pfEnterDebugger, out int pfCallOnScriptErrorWhenContinuing) {
            DebugLog.Log("IActiveScriptSiteDebug.OnScriptErrorDebug({0}) -> !NotImplementedException", pErrorDebug);
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            this.activeScript.Close();
            this.debug.Dispose();
        }

        #endregion
    }
}
