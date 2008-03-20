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

        public JavaScriptReference TryResolve(XPathNavigator referenceNode, JavaScriptReference original) {
            JavaScriptReference reference = null;
            foreach (var resolver in this.resolvers) {
                reference = resolver.TryResolve(referenceNode, original);
                if (reference != null)
                    break;
            }

            return reference;
        }
    }
}
