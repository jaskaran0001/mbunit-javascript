using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Threading {
    internal interface IThreadingRequirement {
        void InvokeAsRequired(Action action);
    }
}
