using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References {
    public class JavaScriptNoReferenceExtractor : IJavaScriptReferenceExtractor {
        public IEnumerable<IJavaScriptReference> GetReferences(IJavaScriptReference originalReference, string scriptContent) {
            yield break;
        }
    }
}
