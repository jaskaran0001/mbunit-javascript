using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.JavaScript.Engines.Microsoft;

namespace MbUnit.JavaScript.Engines {
    internal static class ScriptEngineFactory {
        public static IScriptEngine Create() {
            return new MicrosoftScriptControlEngine();
        }
    }
}
