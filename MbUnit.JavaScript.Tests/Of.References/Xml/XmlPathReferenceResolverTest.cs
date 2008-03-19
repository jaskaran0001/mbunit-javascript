using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

using NMock2;

using MbUnit.Framework;

using MbUnit.JavaScript.References.Xml;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathReferenceProvider))]
    public class XmlPathReferenceResolverTest : MockingTestBase {
        [Test]
        public void TestLoadReferencesWithAbsolutePathWorksCorrectly() {
            const string Path = @"C:\Test.js";

            var loader = new XmlPathReferenceProvider();
            var xmlRoot = this.CreateReferencesDocument(Path).CreateNavigator().SelectSingleNode("references");

            var references = loader.GetReferences(xmlRoot, "");

            CollectionAssert.AreElementsEqual(references, new[] { JavaScriptReference.Files(Path, "") });
        }

        private XPathDocument CreateReferencesDocument(params string[] paths) {
            var builder = new StringBuilder("<references>").AppendLine();
            foreach (var path in paths) {
                builder.AppendFormat("<reference path='{0}' />", path).AppendLine();
            }
            builder.Append("</references>");

            using (var reader = new StringReader(builder.ToString())) {
                return new XPathDocument(reader);
            }
        }
    }
}
