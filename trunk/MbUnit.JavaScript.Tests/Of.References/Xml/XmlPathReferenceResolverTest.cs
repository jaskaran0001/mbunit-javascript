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
    [TestsOn(typeof(XmlPathReferenceResolver))]
    public class XmlPathReferenceResolverTest : MockingTestBase {
        [RowTest]
        [Row(@"C:\Test.js",   "",                     @"C:\Test.js",          Description = "Absolute path with no orignal path.")]
        [Row(@"C:\Test.js",   @"D:\",                 @"C:\Test.js",          Description = "Absolute path with original path.")]
        [Row(@"T2\Test.js",   @"C:\T1\Original.js",   @"C:\T1\T2\Test.js",    Description = "Relative path.")]
        public void TestLoadReferences(string referencePath, string originalPath, string expectedPath) {
            var resolver = new XmlPathReferenceResolver();
            var xmlRoot = this.GetReferenceXml(referencePath);

            var references = resolver.GetReferences(xmlRoot, JavaScriptReference.Files(originalPath, ""));
            
            CollectionAssert.AreElementsEqual(references, new[] { JavaScriptReference.Files(expectedPath, "") });
        }

        private XPathNavigator GetReferenceXml(params string[] paths) {
            var builder = new StringBuilder("<references>").AppendLine();
            foreach (var path in paths) {
                builder.AppendFormat("<reference path='{0}' />", path).AppendLine();
            }
            builder.Append("</references>");

            using (var reader = new StringReader(builder.ToString())) {
                return new XPathDocument(reader)
                                .CreateNavigator()
                                .SelectSingleNode("references");
            }
        }
    }
}
