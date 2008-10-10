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
using IO = System.IO;

namespace MbUnit.JavaScript.References {
    public class ScriptFileReference : IScriptReference {
        private static readonly WildcardSupport WildcardSupport = new WildcardSupport(
            IO.Path.DirectorySeparatorChar, IO.Path.AltDirectorySeparatorChar, '.'
        );

        public string Path                  { get; private set; }

        public ScriptFileReference(string path) {
            this.Path = path;
        }

        public Script[] LoadScripts() {
            if (!WildcardSupport.HasWildcards(this.Path))
                return new[] { this.LoadScript(this.Path) };

            var rootPath = WildcardSupport.GetFixedRoot(this.Path);
            var paths = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

            var scripts = new List<Script>();
            foreach (var path in WildcardSupport.GetMatches(this.Path, paths)) {
                scripts.Add(this.LoadScript(path));
            }

            return scripts.ToArray();
        }

        private Script LoadScript(string path) {
            var content = File.ReadAllText(path);
            return new Script(path, content, this);
        }

        public bool Equals(ScriptFileReference reference) {
            if (reference == null)
                return false;

            return this.Path == reference.Path;
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as ScriptFileReference);
        }

        public override int GetHashCode() {
            return this.Path.GetHashCode();
        }
    }
}