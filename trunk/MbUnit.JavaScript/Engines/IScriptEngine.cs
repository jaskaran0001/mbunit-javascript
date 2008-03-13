﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines {
    public interface IScriptEngine {
        void Load(string script);
        object Eval(string expression);
    }
}
