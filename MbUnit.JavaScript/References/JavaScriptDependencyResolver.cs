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
            
            string scriptContent = this.GetFirst(script.LoadAll());
            var references = referenceExtractor.GetReferences(script, scriptContent);
            foreach (var reference in references) {
                this.CollectReferencesRecursive(reference, allScriptContents, alreadyCollected);
            }

            alreadyCollected.Add(script, true);
            allScriptContents.Add(scriptContent);
        }

        // TODO: This is a temporary hack to support obsolete API
        private string GetFirst(IEnumerable<string> scripts) {
            foreach (var script in scripts) {
                return script;
            }
            return null;
        }
    }
}
