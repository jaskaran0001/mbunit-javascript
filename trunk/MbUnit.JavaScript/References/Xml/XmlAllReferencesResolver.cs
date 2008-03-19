using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlAllReferencesResolver : IXmlReferenceResolver {
        private readonly IEnumerable<IXmlReferenceResolver> loaders;

        public XmlAllReferencesResolver(IEnumerable<IXmlReferenceResolver> loaders) {
            this.loaders = loaders;
        }

        public IEnumerable<JavaScriptReference> GetReferences(IXPathNavigable referencesRoot, string scriptPath) {
            foreach (var loader in this.loaders) {
                foreach (var reference in loader.GetReferences(referencesRoot, scriptPath)) {
                    yield return reference;
                }
            }
        }
    }
}
