using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

using NMock2;

using MbUnit.Framework;

using MbUnit.JavaScript.References;
using MbUnit.JavaScript.References.Xml;
using MbUnit.JavaScript.References.Xml.Resources;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathToResourceReferenceResolver))]
    public class XmlPathToResourceReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row(@"Test.js",        "Dir.Original.js",       "Dir.Test.js")]
        [Row(@"..\DirX\My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"../DirX/My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"./DirX/My.js",   "Dir.Original.js",       "Dir.DirX.My.js")]
        [Row(@".././../My.js",  "Dir1.Dir2.Original.js", "My.js")]
        public void TestResolve(string referencePath, string originalResourceName, string expectedResourceName) {
            var xml = this.GetPathReferenceXml(referencePath);
            var lookupFactory = this.MockLookupFactoryWithOneExistingResource(expectedResourceName);
            var resolver = new XmlPathToResourceReferenceResolver(lookupFactory);

            var original = new JavaScriptResourceReference(originalResourceName, this.GetType().Assembly);
            var reference = resolver.TryResolve(xml, original) as JavaScriptResourceReference;

            Assert.IsNotNull(reference);
            Assert.AreSame(original.Assembly, reference.Assembly);
            Assert.AreEqual(expectedResourceName, reference.ResourceName);
        }

        private IResourceLookupFactory MockLookupFactoryWithOneExistingResource(string resourceName) {
            var lookup = Mock<IResourceLookup>(
                mock => {
                    Expect.Once.On(mock)
                        .Method("ResourceExists")
                        .With(resourceName)
                        .Will(Return.Value(true));

                    Stub.On(mock).Method("ResourceExists").Will(Return.Value(false));
                }
            );

            return Mock<IResourceLookupFactory>(
                mock => Expect.Once.On(mock)
                              .Method("CreateLookup")
                              .Will(Return.Value(lookup))
            );            
        }

        private XPathNavigator GetPathReferenceXml(string path) {
            var attributeString = string.Format("path='{0}'", path);
            return this.GetReferenceXml(attributeString);
        }
    }
}
