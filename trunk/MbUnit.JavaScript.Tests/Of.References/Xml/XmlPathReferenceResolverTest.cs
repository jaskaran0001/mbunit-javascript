using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

using MbUnit.Framework;

using MbUnit.JavaScript.References;
using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathReferenceResolver))]
    public class XmlPathReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row(@"C:\Test.js",   "",                     @"C:\Test.js"         )]
        [Row(@"C:\Test.js",   @"D:\",                 @"C:\Test.js"         )]
        [Row(@"T2\Test.js",   @"C:\T1\Original.js",   @"C:\T1\T2\Test.js"   )]
        [Row(@"..\Test.js",   @"C:\T1\Original.js",   @"C:\Test.js"         )]
        [Row(@"..\Test.js",   @"C:/T1/Original.js",   @"C:\Test.js"         )]
        public void TestLoadReferences(string referencePath, string originalPath, string expectedPath) {
            var resolver = new XmlPathReferenceResolver();
            var xmlRoot = this.GetPathReferenceXml(referencePath);

            var reference = resolver.TryResolve(xmlRoot, JavaScriptReference.Files(originalPath, "")) as JavaScriptFileReference;

            Assert.IsNotNull(reference);
            Assert.AreEqual(expectedPath, reference.Path);
        }

        private XPathNavigator GetPathReferenceXml(string path) {
            var attributeString = string.Format("path='{0}'", path);
            return this.GetReferenceXml(attributeString);
        }
    }
}
