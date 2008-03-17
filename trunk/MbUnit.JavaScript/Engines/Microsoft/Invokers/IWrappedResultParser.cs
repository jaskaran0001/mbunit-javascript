using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Engines.Microsoft.Invokers {
    internal interface IWrappedResultParser {
        object GetResult(object wrapperFunctionResult);
    }
}