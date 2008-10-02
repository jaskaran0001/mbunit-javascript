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
using System.Text.RegularExpressions;
using System.Xml.XPath;

using MbUnit.JavaScript.References.Xml.Resources;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlPathToResourceReferenceResolver : IXmlReferenceResolver {
        private static readonly Regex PathSplitRegex = new Regex(
            @"[" + Regex.Escape(Path.DirectorySeparatorChar.ToString() + Path.AltDirectorySeparatorChar) + "]"
        );

        private readonly IResourceLookupFactory lookupFactory;

        public XmlPathToResourceReferenceResolver(IResourceLookupFactory lookupFactory) {
            this.lookupFactory = lookupFactory;
        }

        public XmlPathToResourceReferenceResolver()
            : this(new ResourceLookupFactory())
        {
        }

        public IScriptReference TryResolve(XPathNavigator referenceNode, Script original) {
            var path = referenceNode.GetAttribute("path", "");
            if (string.IsNullOrEmpty(path))
                return null;

            var originalReferenceAsResource = original.LoadedFrom as ScriptResourceReference;
            if (originalReferenceAsResource == null || Path.IsPathRooted(path))
                return null;

            var assembly = originalReferenceAsResource.Assembly;
            var lookup = this.lookupFactory.CreateLookup(assembly);

            string resourceName = this.GuessResourceName(path, original, lookup);
            return new ScriptResourceReference(resourceName, originalReferenceAsResource.Assembly);
        }

        private string GuessResourceName(string referencePath, Script original, IResourceLookup lookup) {
            // ashmind: This is some ugly, but tested code. I just haven't found the better way.
            
            var pathParts = PathSplitRegex.Split(referencePath);
            var referencePathSuffixStack = new Stack<string>();

            int skip = 0;
            for (int i = pathParts.Length - 1; i >= 0; i--) {
                var path = pathParts[i];
                if (path == "..") {
                    skip += 1;
                    continue;
                }

                if (path == ".")
                    continue;

                if (skip > 0) {
                    skip -= 1;
                    continue;
                }

                referencePathSuffixStack.Push(path);
            }
            string referencePathSuffix = string.Join(".", referencePathSuffixStack.ToArray());

            var suggestedNames = new List<string>();

            var resourceNameParts = original.Name.Split('.');
            for (int i = resourceNameParts.Length - 1; i - skip >= 0; i--) {
                var suggestedName = string.Join(".", resourceNameParts, 0, i - skip);
                if (suggestedName.Length > 0)
                    suggestedName += ".";

                suggestedName += referencePathSuffix;

                bool exists = lookup.ResourceExists(suggestedName);
                if (exists)
                    return suggestedName;

                suggestedNames.Add(suggestedName);
            }

            throw this.GetNotFoundException(referencePath, original, suggestedNames);
        }

        private Exception GetNotFoundException(string referencePath, Script original, List<string> suggestedNames) {
            var namesString = string.Join(", ", suggestedNames.ToArray());
            var assembly = ((ScriptResourceReference)original.LoadedFrom).Assembly;

            return new ResourceNotFoundException(string.Format(
                "Resource '{0}' referenced by {1} was not found in {2}. Following names were tried: {3}.",
                    referencePath, original.Name, assembly, namesString
            ), assembly, namesString);
        }
    }
}
