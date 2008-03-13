using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines {
    public interface IScriptFunction {
        object Call(object @this, params object[] args);
        ScriptFunctionDelegate ToDelegate();
    }
}
