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
using System.Xml.XPath;

using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.References {
    public class ScriptXmlReferenceExtractor : IScriptReferenceExtractor {
        private readonly XmlReferenceParser parser;
        private readonly IXmlReferenceResolver resolver;

        // ashmind: this makes me think of DI container, but it feels like an overkill.
        // Still, this is too hacky.
        public ScriptXmlReferenceExtractor() 
            : this(
                XmlReferenceParser.Default,
                new XmlAllReferencesResolver(
                    new XmlResourceReferenceResolver(),
                    new XmlPathReferenceResolver(),
                    new XmlPathToResourceReferenceResolver()
                ) 
            )
        {
        }

        public ScriptXmlReferenceExtractor(XmlReferenceParser parser, IXmlReferenceResolver resolver) {
            this.parser = parser;
            this.resolver = resolver;
        }

        public IEnumerable<IScriptReference> GetReferences(IScriptReference originalReference, string scriptContent) {
            var xml = this.parser.Parse(scriptContent);
            var referenceNodes = xml.CreateNavigator().Select("reference");

            foreach (XPathNavigator referenceNode in referenceNodes) {
                // ashmind: Should we throw here or should we throw only if reference was not found?
                // I think it should be made configurable in the future.

                var reference = resolver.TryResolve(referenceNode, originalReference);
                if (reference != null)
                    yield return reference;
            }
        }
    }
}
