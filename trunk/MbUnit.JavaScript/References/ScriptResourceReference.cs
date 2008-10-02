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
using System.IO;
using System.Reflection;

namespace MbUnit.JavaScript.References {
    internal class ScriptResourceReference : IScriptReference {
        private static readonly WildcardSupport WildcardSupport = new WildcardSupport('.');

        public string ResourceName { get; private set; }
        public Assembly Assembly { get; private set; }

        public ScriptResourceReference(string resourceName, Assembly assembly) {
            this.ResourceName = resourceName;
            this.Assembly = assembly;
        }

        public Script[] LoadScripts() {
            if (!WildcardSupport.HasWildcards(this.ResourceName))
                return new[] { this.LoadScript(this.ResourceName) };

            var resourceNames = this.Assembly.GetManifestResourceNames();
            var scripts = new List<Script>();
            foreach (var path in WildcardSupport.GetMatches(this.ResourceName, resourceNames)) {
                scripts.Add(this.LoadScript(path));
            }

            return scripts.ToArray();
        }

        private Script LoadScript(string resourceName) {
            return new Script(resourceName, this.LoadContent(resourceName), this);
        }

        private string LoadContent(string resourceName) {
            using (var stream = this.Assembly.GetManifestResourceStream(resourceName)) {
                if (stream == null)
                    throw new ResourceNotFoundException(this.Assembly, resourceName);

                using (var reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }

        public bool Equals(ScriptResourceReference reference) {
            if (reference == null)
                return false;

            return this.Assembly == reference.Assembly
                && this.ResourceName == reference.ResourceName;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as ScriptResourceReference);
        }

        public override int GetHashCode() {
            return this.Assembly.GetHashCode() ^ this.ResourceName.GetHashCode();
        }
    }
}