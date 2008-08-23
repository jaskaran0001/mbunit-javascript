using System;
using System.Collections.Generic;

using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    internal class ActiveScriptDebugDelegator : IActiveScript, IActiveScriptDebug {
        private readonly IActiveScript engine;
        private readonly IActiveScriptDebug debug;

        public ActiveScriptDebugDelegator(IActiveScript engine) {
            this.engine = engine;
            this.debug = engine as IActiveScriptDebug;
        }
        
        #region IActiveScript Members

        public void AddNamedItem(string pstrName, uint dwFlags) {
            this.engine.AddNamedItem(pstrName, dwFlags);
        }

        public void AddTypeLib(ref Guid rguidTypeLib, uint dwMajor, uint dwMinor, uint dwFlags) {
            this.engine.AddTypeLib(ref rguidTypeLib, dwMajor, dwMinor, dwFlags);
        }

        public void Clone(out IActiveScript ppscript) {
            this.engine.Clone(out ppscript);
            ppscript = new ActiveScriptDebugDelegator(ppscript);
        }

        public void Close() {
            this.engine.Close();
        }

        public void GetCurrentScriptThreadID(out uint pstidThread) {
            this.engine.GetCurrentScriptThreadID(out pstidThread);
        }

        public void GetScriptDispatch(string pstrItemName, out object ppdisp) {
            this.engine.GetScriptDispatch(pstrItemName, out ppdisp);
        }

        public void GetScriptSite(ref Guid riid, out IntPtr ppvObject) {
            this.engine.GetScriptSite(ref riid, out ppvObject);
        }

        public void GetScriptState(out tagSCRIPTSTATE pssState) {
            this.engine.GetScriptState(out pssState);
        }

        public void GetScriptThreadID(uint dwWin32ThreadId, out uint pstidThread) {
            this.engine.GetScriptThreadID(dwWin32ThreadId, out pstidThread);
        }

        public void GetScriptThreadState(uint stidThread, out tagSCRIPTTHREADSTATE pstsState) {
            this.engine.GetScriptThreadState(stidThread, out pstsState);
        }

        public void InterruptScriptThread(uint stidThread, ref stdole.EXCEPINFO pexcepinfo, uint dwFlags) {
            this.engine.InterruptScriptThread(stidThread, ref pexcepinfo, dwFlags);
        }

        public void SetScriptSite(IActiveScriptSite pass) {
            this.engine.SetScriptSite(pass);
        }

        public void SetScriptState(tagSCRIPTSTATE ss) {
            this.engine.SetScriptState(ss);
        }

        #endregion

        #region IActiveScriptDebug Members

        public void EnumCodeContextsOfPosition(uint dwSourceContext, uint uCharacterOffset, uint uNumChars, out IEnumDebugCodeContexts ppescc) {
            DebugLog.Log("IActiveScriptDebug.EnumCodeContextsOfPosition({0}, {1}, {2}) ->", dwSourceContext, uCharacterOffset, uNumChars);
            
            this.debug.EnumCodeContextsOfPosition(dwSourceContext, uCharacterOffset, uNumChars, out ppescc);
            ppescc = new EnumDebugCodeContextsDelegator(ppescc);

            DebugLog.Log("[{0}]", ppescc);
        }

        public void GetScriptTextAttributes(string pstrCode, uint uNumCodeChars, string pstrDelimiter, uint dwFlags, ref ushort pattr) {
            DebugLog.Log("IActiveScriptDebug.GetScriptTextAttributes({0}, {1}, {2})", uNumCodeChars, pstrDelimiter, dwFlags);
            this.debug.GetScriptTextAttributes(pstrCode, uNumCodeChars, pstrDelimiter, dwFlags, ref pattr);

            DebugLog.Log("[{0}]", pattr);
        }

        public void GetScriptletTextAttributes(string pstrCode, uint uNumCodeChars, string pstrDelimiter, uint dwFlags, ref ushort pattr) {
            DebugLog.Log("IActiveScriptDebug.GetScriptletTextAttributes({0}, {1}, {2})", uNumCodeChars, pstrDelimiter, dwFlags);
            this.debug.GetScriptTextAttributes(pstrCode, uNumCodeChars, pstrDelimiter, dwFlags, ref pattr);

            DebugLog.Log("[{0}]", pattr);
        }

        #endregion
    }
}