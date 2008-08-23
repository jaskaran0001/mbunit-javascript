using System;
using System.Collections.Generic;
using System.Text;
using ProcessDebugManagerLib;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    public class DebugApplicationDelegator : IDebugApplication, IRemoteDebugApplication {
        private readonly IDebugApplication actual;

        public DebugApplicationDelegator(IDebugApplication actual) {
            this.actual = actual;
        }

        #region IDebugApplication Members

        public void AddGlobalExpressionContextProvider(IProvideExpressionContexts pdsfs, out uint pdwCookie) {
            DebugLog.Log("IDebugApplication.AddGlobalExpressionContextProvider");
            this.actual.AddGlobalExpressionContextProvider(pdsfs, out pdwCookie);
        }

        public void AddStackFrameSniffer(IDebugStackFrameSniffer pdsfs, out uint pdwCookie) {
            DebugLog.Log("IDebugApplication.AddStackFrameSniffer");
            this.actual.AddStackFrameSniffer(pdsfs, out pdwCookie);
        }

        public void CauseBreak() {
            DebugLog.Log("IDebugApplication.CauseBreak");
            this.actual.CauseBreak();
        }

        public void Close() {
            DebugLog.Log("IDebugApplication.Close");
            this.actual.Close();
        }

        public void ConnectDebugger(IApplicationDebugger pad) {
            DebugLog.Log("IDebugApplication.ConnectDebugger");
            this.actual.ConnectDebugger(pad);
        }

        public void CreateApplicationNode(out IDebugApplicationNode ppdanNew) {
            DebugLog.Log("IDebugApplication.CreateApplicationNode");
            this.actual.CreateApplicationNode(out ppdanNew);
        }

        public void CreateAsyncDebugOperation(IDebugSyncOperation psdo, out IDebugAsyncOperation ppado) {
            DebugLog.Log("IDebugApplication.CreateAsyncDebugOperation");
            this.actual.CreateAsyncDebugOperation(psdo, out ppado);
        }

        public void CreateInstanceAtApplication(ref Guid rclsid, object pUnkOuter, uint dwClsContext, ref Guid riid, out object ppvObject) {
            DebugLog.Log("IDebugApplication.CreateInstanceAtApplication");
            this.actual.CreateInstanceAtApplication(ref rclsid, pUnkOuter, dwClsContext, ref riid, out ppvObject);
        }

        public void DebugOutput(string pstr) {
            DebugLog.Log("IDebugApplication.DebugOutput");
            this.actual.DebugOutput(pstr);
        }

        public void DisconnectDebugger() {
            DebugLog.Log("IDebugApplication.DisconnectDebugger");
            this.actual.DisconnectDebugger();
        }

        public void EnumGlobalExpressionContexts(out IEnumDebugExpressionContexts ppedec) {
            DebugLog.Log("IDebugApplication.EnumGlobalExpressionContexts");
            this.actual.EnumGlobalExpressionContexts(out ppedec);
        }

        public void EnumThreads(out IEnumRemoteDebugApplicationThreads pperdat) {
            DebugLog.Log("IDebugApplication.EnumThreads");
            this.actual.EnumThreads(out pperdat);
        }

        public int FCanJitDebug() {
            DebugLog.Log("IDebugApplication.FCanJitDebug");
            return this.actual.FCanJitDebug();
        }

        public int FIsAutoJitDebugEnabled() {
            DebugLog.Log("IDebugApplication.FIsAutoJitDebugEnabled");
            return this.actual.FIsAutoJitDebugEnabled();
        }

        public void FireDebuggerEvent(ref Guid riid, object punk) {
            DebugLog.Log("IDebugApplication.FireDebuggerEvent");
            this.actual.FireDebuggerEvent(ref riid, punk);
        }

        public void GetBreakFlags(out uint pabf, out IRemoteDebugApplicationThread pprdatSteppingThread) {
            DebugLog.Log("IDebugApplication.GetBreakFlags");
            this.actual.GetBreakFlags(out pabf, out pprdatSteppingThread);
        }

        public void GetCurrentThread(out IDebugApplicationThread pat) {
            DebugLog.Log("IDebugApplication.GetCurrentThread");
            this.actual.GetCurrentThread(out pat);
        }

        public void GetDebugger(out IApplicationDebugger pad) {
            DebugLog.Log("IDebugApplication.GetDebugger");
            this.actual.GetDebugger(out pad);
        }

        public void GetName(out string pbstrName) {
            DebugLog.Log("IDebugApplication.GetName");
            this.actual.GetName(out pbstrName);
        }

        public void GetRootNode(out IDebugApplicationNode ppdanRoot) {
            DebugLog.Log("IDebugApplication.GetRootNode");
            this.actual.GetRootNode(out ppdanRoot);
        }

        public void HandleBreakPoint(tagBREAKREASON br, out tagBREAKRESUME_ACTION pbra) {
            DebugLog.Log("IDebugApplication.HandleBreakPoint");
            this.actual.HandleBreakPoint(br, out pbra);
        }

        public void HandleRuntimeError(IActiveScriptErrorDebug pErrorDebug, IActiveScriptSite pScriptSite, out tagBREAKRESUME_ACTION pbra, out tagERRORRESUMEACTION perra, out int pfCallOnScriptError) {
            DebugLog.Log("IDebugApplication.HandleRuntimeError");
            this.actual.HandleRuntimeError(pErrorDebug, pScriptSite, out pbra, out perra, out pfCallOnScriptError);
        }

        public void QueryAlive() {
            DebugLog.Log("IDebugApplication.QueryAlive");
            this.actual.QueryAlive();
        }

        public void QueryCurrentThreadIsDebuggerThread() {
            DebugLog.Log("IDebugApplication.QueryCurrentThreadIsDebuggerThread");
            this.actual.QueryCurrentThreadIsDebuggerThread();
        }

        public void RemoveGlobalExpressionContextProvider(uint dwCookie) {
            DebugLog.Log("IDebugApplication.RemoveGlobalExpressionContextProvider");
            this.actual.RemoveGlobalExpressionContextProvider(dwCookie);
        }

        public void RemoveStackFrameSniffer(uint dwCookie) {
            DebugLog.Log("IDebugApplication.RemoveStackFrameSniffer");
            this.actual.RemoveStackFrameSniffer(dwCookie);
        }

        public void ResumeFromBreakPoint(IRemoteDebugApplicationThread prptFocus, tagBREAKRESUME_ACTION bra, tagERRORRESUMEACTION era) {
            DebugLog.Log("IDebugApplication.ResumeFromBreakPoint");
            this.actual.ResumeFromBreakPoint(prptFocus, bra, era);
        }

        public void SetName(string pstrName) {
            DebugLog.Log("IDebugApplication.SetName");
            this.actual.SetName(pstrName);
        }

        public void StartDebugSession() {
            DebugLog.Log("IDebugApplication.StartDebugSession");
            this.actual.StartDebugSession();
        }

        public void StepOutComplete() {
            DebugLog.Log("IDebugApplication.StepOutComplete");
            this.actual.StepOutComplete();
        }

        public void SynchronousCallInDebuggerThread(IDebugThreadCall pptc, uint dwParam1, uint dwParam2, uint dwParam3) {
            DebugLog.Log("IDebugApplication.SynchronousCallInDebuggerThread");
            this.actual.SynchronousCallInDebuggerThread(pptc, dwParam1, dwParam2, dwParam3);
        }

        #endregion
    }
}
