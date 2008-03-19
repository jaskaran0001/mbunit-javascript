using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References {
    public interface IJavaScriptReferenceExtractor {
        IEnumerable<JavaScriptReference> GetReferences(string script, string scriptPath);
    }
}
