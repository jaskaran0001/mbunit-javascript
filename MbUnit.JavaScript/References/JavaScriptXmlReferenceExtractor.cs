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

        public IEnumerable<JavaScriptReference> GetReferences(string script, string scriptPath) {
            var xml = this.parser.Parse(script);
            return provider.GetReferences(xml, scriptPath);
        }
    }
}
