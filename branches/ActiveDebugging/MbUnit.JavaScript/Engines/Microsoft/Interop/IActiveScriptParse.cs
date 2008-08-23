using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MbUnit.JavaScript.Engines.Microsoft.Interop {
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BB1A2AE2-A4F9-11CF-8F20-00805F2CD064")]
    interface IActiveScriptParse {
        void InitNew();
        void AddScriptlet(string defaultName,
                          string code,
                          string itemName,
                          string subItemName,
                          string eventName,
                          string delimiter,
                          uint sourceContextCookie,
                          uint startingLineNumber,
                          uint flags,
                          out string name,
                          out stdole.EXCEPINFO info);

        void ParseScriptText(string code,
                             string itemName,
                             IntPtr context,
                             string delimiter,
                             uint sourceContextCookie,
                             uint startingLineNumber,
                             tagSCRIPTTEXT flags,
                             out object result,
                             out stdole.EXCEPINFO info);
    }
}
