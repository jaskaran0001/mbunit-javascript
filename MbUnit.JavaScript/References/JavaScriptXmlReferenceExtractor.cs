using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.References {
    public class JavaScriptXmlReferenceExtractor : IJavaScriptReferenceExtractor {
        private readonly XmlReferenceParser parser;
        private readonly IXmlReferenceResolver resolver;

        public JavaScriptXmlReferenceExtractor(XmlReferenceParser parser, IXmlReferenceResolver resolver) {
            this.parser = parser;
            this.resolver = resolver;
        }

        public IEnumerable<IJavaScriptReference> GetReferences(IJavaScriptReference script, string scriptContent) {
            var xml = this.parser.Parse(scriptContent);
            var referenceNodes = xml.CreateNavigator().Select("reference");

            foreach (XPathNavigator referenceNode in referenceNodes) {
                // ashmind: Should we throw here or should we throw only if reference was not found?
                // I think it should be made configurable in the future.

                var reference = resolver.TryResolve(referenceNode, script);
                if (reference != null)
                    yield return reference;
            }
        }
    }
}
