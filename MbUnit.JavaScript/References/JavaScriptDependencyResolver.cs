using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References {
    public class JavaScriptDependencyResolver {
        private readonly IJavaScriptReferenceExtractor referenceExtractor;

        public JavaScriptDependencyResolver(IJavaScriptReferenceExtractor referenceExtractor) {
            this.referenceExtractor = referenceExtractor;
        }

        public IEnumerable<string> LoadScripts(IEnumerable<IJavaScriptReference> entryScripts) {
            var scriptContents = new List<string>();
            
            foreach (var entry in entryScripts) {
                this.CollectReferencesRecursive(entry, scriptContents, new Dictionary<IJavaScriptReference, bool>());
            }

            return scriptContents;
        }

        private void CollectReferencesRecursive(IJavaScriptReference script, ICollection<string> allScriptContents, IDictionary<IJavaScriptReference, bool> alreadyCollected) {
            if (alreadyCollected.ContainsKey(script))
                return;

            string scriptContent = script.LoadContent();
            var references = referenceExtractor.GetReferences(script, scriptContent);
            foreach (var reference in references) {
                this.CollectReferencesRecursive(reference, allScriptContents, alreadyCollected);
            }

            alreadyCollected.Add(script, true);
            allScriptContents.Add(scriptContent);
        }
    }
}
