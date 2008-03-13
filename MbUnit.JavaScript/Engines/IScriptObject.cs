using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines {
    public interface IScriptObject : IDictionary<string, object> {
        object Invoke(string key, params object[] args);
    }
}