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
        private readonly IXmlReferenceResolver provider;

        public JavaScriptXmlReferenceExtractor(XmlReferenceParser parser, IXmlReferenceResolver provider) {
            this.parser = parser;
            this.provider = provider;
        }

        public IEnumerable<JavaScriptReference> GetReferences(JavaScriptReference script, string scriptContent) {
            var xml = this.parser.Parse(scriptContent);
            return provider.GetReferences(xml, script);
        }
    }
}
