using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

using MbUnit.Framework;

using MbUnit.JavaScript.References.Xml;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathToResourceReferenceResolver))]
    public class XmlPathToResourceReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row(@"Test.js",        "Dir.Original.js",       "Dir.Test.js")]
        [Row(@"..\DirX\My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"../DirX/My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"./DirX/My.js",   "Dir.Original.js",       "Dir.DirX.My.js")]
        public void TestResolve(string referencePath, string originalResourceName, string expectedResourceName) {
            var xml = this.GetPathReferencesXml(referencePath);
            var resolver = new XmlPathToResourceReferenceResolver();

            var original = new JavaScriptResourceReference(originalResourceName, this.GetType().Assembly);
            var references = resolver.GetReferences(xml, original);
            var reference = (JavaScriptResourceReference)this.GetFirst(references);

            Assert.AreSame(original.Assembly, reference.Assembly);
            Assert.AreEqual(expectedResourceName, reference.Pattern);
        }

        private XPathNavigator GetPathReferencesXml(params string[] paths) {
            var attributeStrings = Array.ConvertAll(paths, path => string.Format("path='{0}'", path));
            return this.GetReferencesXml(attributeStrings);
        }
    }
}
