using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.Expando;

namespace MbUnit.JavaScript.Engines.Microsoft {
    internal interface IComArrayConstructor {
        IExpando ToScriptArray(object[] original);
    }
}
