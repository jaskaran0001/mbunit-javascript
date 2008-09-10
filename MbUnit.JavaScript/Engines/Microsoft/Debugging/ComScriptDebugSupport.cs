using System;
using System.Collections.Generic;

using ProcessDebugManagerLib;
using MbUnit.JavaScript.Engines.Microsoft.Debug;

namespace MbUnit.JavaScript.Engines.Microsoft.Debugging {
    internal class ComScriptDebugSupport : IDisposable {
        private readonly ProcessDebugManagerClass debugManager = new ProcessDebugManagerClass();
        private readonly IDebugApplication application;

        private readonly IDictionary<uint, IDebugDocumentHelper> documents = new Dictionary<uint, IDebugDocumentHelper>();
        
        public ComScriptDebugSupport() {
            var name = this.GetType().Assembly.GetName().Name;

            this.application = this.CreateApplication(name);
        }

        public IDebugApplication DebugApplication {
            get { return application; }
        }

        public uint RegisterScript(IActiveScript activeScript, string scriptName, string scriptCode) {
            var documentHelper = CreateDocumentHelper(scriptName);
            
            documentHelper.AddUnicodeText(scriptCode);
            uint context;
            documentHelper.DefineScriptBlock(0, (uint)scriptCode.Length, activeScript, 0, out context);

            this.documents[context] = documentHelper;

            return context;
        }

        public IDebugDocumentHelper GetDebugDocument(uint context) {
            return this.documents[context];
        }

        private IDebugDocumentHelper CreateDocumentHelper(string name) {
            CDebugDocumentHelper documentHelper;
            this.debugManager.CreateDebugDocumentHelper(null, out documentHelper);
            documentHelper.Init(this.application, name, name, 0);
            documentHelper.Attach(null);

            return documentHelper;
        }

        private IDebugApplication CreateApplication(string name) {
            IDebugApplication application;
            this.debugManager.CreateApplication(out application);
            application.SetName(name);

            //application = new DebugApplicationDelegator(application);

            uint appCode;
            debugManager.AddApplication(application, out appCode);

            return application;//new Debug.DebugApplicationDelegator(application);
        }

        public void Dispose() {          
            this.application.Close();
        }
    }
}