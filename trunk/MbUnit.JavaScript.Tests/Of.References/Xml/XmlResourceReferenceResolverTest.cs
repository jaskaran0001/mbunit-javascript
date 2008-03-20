using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.XPath;

using MbUnit.Framework;
using MbUnit.JavaScript.References;
using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlResourceReferenceResolver))]
    public class XmlResourceReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row("Test.js", "System.Xml")]
        [Row("Test.js", "AssemblyX", ExpectedException = typeof(FileNotFoundException))]
        public void TestGetReferences(string name, string assemblyName) {
            var reference = GetReference(name, assemblyName);

            Assert.AreEqual(name, reference.Pattern);
            Assert.AreEqual(assemblyName, reference.Assembly.GetName().Name);
        }

        [Test]
        [Ignore("Right now not possible on machine that does not have ASP.NET AJAX Extensions 2.0 installed.")]
        public void TestGetReferencesWithEmptyAssemblyLoadsAspNetAjax() {
            var reference = GetReference("Irrelevant.js", null);

            Assert.AreEqual("System.Web.Extensions", reference.Assembly.GetName().Name);
        }

        private JavaScriptResourceReference GetReference(string resourceName, string assemblyName) {
            var xml = this.GetResourceReferenceXml(resourceName, assemblyName);
            var reference = new XmlResourceReferenceResolver().TryResolve(xml, null);

            return (JavaScriptResourceReference)reference;            
        }

        private XPathNavigator GetResourceReferenceXml(string name, string assembly) {
            var attributeString = string.Format("name='{0}'", name);
            if (assembly != null)
                attributeString += string.Format(" assembly='{0}'", assembly);

            return this.GetReferenceXml(attributeString);
        }
    }
}
