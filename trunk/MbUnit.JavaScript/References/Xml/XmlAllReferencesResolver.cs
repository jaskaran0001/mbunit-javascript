using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlAllReferencesResolver : IXmlReferenceResolver {
        private readonly IEnumerable<IXmlReferenceResolver> resolvers;

        public XmlAllReferencesResolver(IEnumerable<IXmlReferenceResolver> resolvers) {
            this.resolvers = resolvers;
        }

        public XmlAllReferencesResolver(params IXmlReferenceResolver[] resolvers) {
            this.resolvers = resolvers;
        }

        public IEnumerable<JavaScriptReference> GetReferences(IXPathNavigable referencesRoot, JavaScriptReference original) {
            foreach (var resolver in this.resolvers) {
                if (!resolver.CanGetReferences(original))
                    continue;

                foreach (var reference in resolver.GetReferences(referencesRoot, original)) {
                    yield return reference;
                }
            }
        }

        public bool CanGetReferences(JavaScriptReference original) {
            foreach (var resolver in this.resolvers) {
                if (resolver.CanGetReferences(original))
                    return true;
            }

            return false;
        }
    }
}
