using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal interface IComScriptInvoker {
        object Invoke(IExpando owner, string name, params object[] args);
    }
}
