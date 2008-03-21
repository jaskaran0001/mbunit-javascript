using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    public abstract class XmlReferenceResolverTestBase : MockingTestBase {
        protected XPathNavigator GetReferenceXml(string attributeString) {
            string xml = string.Format("<reference {0} />", attributeString);

            using (var reader = new StringReader(xml)) {
                return new XPathDocument(reader)
                                .CreateNavigator()
                                .SelectSingleNode("reference");
            }
        }
    }
}
