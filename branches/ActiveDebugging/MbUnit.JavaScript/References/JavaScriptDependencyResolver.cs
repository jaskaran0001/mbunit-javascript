/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;

namespace MbUnit.JavaScript.References {
    public class JavaScriptDependencyResolver {
        private readonly IJavaScriptReferenceExtractor referenceExtractor;

        public JavaScriptDependencyResolver(IJavaScriptReferenceExtractor referenceExtractor) {
            this.referenceExtractor = referenceExtractor;
        }

        public IEnumerable<ScriptInfo> LoadScripts(IEnumerable<IJavaScriptReference> entryScriptReferences) {
            var scripts = new List<ScriptInfo>();

            foreach (var reference in entryScriptReferences) {
                this.CollectReferencesRecursive(
                    reference, scripts,
                    new Dictionary<IJavaScriptReference, bool>(),
                    new Stack<IJavaScriptReference>()
                );
            }

            return scripts;
        }

        private void CollectReferencesRecursive(IJavaScriptReference scriptReference, ICollection<ScriptInfo> allScripts, IDictionary<IJavaScriptReference, bool> alreadyCollected, Stack<IJavaScriptReference> processing) {
            if (alreadyCollected.ContainsKey(scriptReference) || processing.Contains(scriptReference))
                return;

            processing.Push(scriptReference);

            var script = scriptReference.LoadScript();
            var references = referenceExtractor.GetReferences(scriptReference, script.Content);
            foreach (var reference in references) {
                this.CollectReferencesRecursive(reference, allScripts, alreadyCollected, processing);
            }

            processing.Pop();
            alreadyCollected.Add(scriptReference, true);
            allScripts.Add(script);
        }
    }
}
