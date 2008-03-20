using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    public interface IJavaScriptReference {
        IEnumerable<string> LoadAll();
    }
}
