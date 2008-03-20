using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References {
    public interface IJavaScriptReferenceExtractor {
        IEnumerable<IJavaScriptReference> GetReferences(IJavaScriptReference script, string scriptContent);
    }
}
