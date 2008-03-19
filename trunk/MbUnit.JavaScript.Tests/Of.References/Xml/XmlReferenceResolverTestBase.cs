using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    public abstract class XmlReferenceResolverTestBase {
        protected XPathNavigator GetReferencesXml(params string[] attributeStrings) {
            var builder = new StringBuilder("<references>").AppendLine();
            foreach (var attributeString in attributeStrings) {
                builder.AppendFormat("<reference {0} />", attributeString).AppendLine();
            }
            builder.Append("</references>");

            using (var reader = new StringReader(builder.ToString())) {
                return new XPathDocument(reader)
                                .CreateNavigator()
                                .SelectSingleNode("references");
            }
        }

        protected JavaScriptReference GetFirst(IEnumerable<JavaScriptReference> references) {
            // ashmind: this can obviously be upgraded if we move to LINQ
            foreach (var reference in references) {
                return reference;
            }
            return null;
        }
    }
}
