using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

using MbUnit.Framework;

using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathReferenceResolver))]
    public class XmlPathReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row(@"C:\Test.js",   "",                     @"C:\Test.js",          Description = "Absolute path with no orignal path.")]
        [Row(@"C:\Test.js",   @"D:\",                 @"C:\Test.js",          Description = "Absolute path with original path.")]
        [Row(@"T2\Test.js",   @"C:\T1\Original.js",   @"C:\T1\T2\Test.js",    Description = "Relative path.")]
        public void TestLoadReferences(string referencePath, string originalPath, string expectedPath) {
            var resolver = new XmlPathReferenceResolver();
            var xmlRoot = this.GetPathReferencesXml(referencePath);

            var references = resolver.GetReferences(xmlRoot, JavaScriptReference.Files(originalPath, ""));
            
            CollectionAssert.AreElementsEqual(references, new[] { JavaScriptReference.Files(expectedPath, "") });
        }

        private XPathNavigator GetPathReferencesXml(params string[] paths) {
            var attributeStrings = Array.ConvertAll(paths, path => string.Format("path='{0}'", path));
            return this.GetReferencesXml(attributeStrings);
        }
    }
}
