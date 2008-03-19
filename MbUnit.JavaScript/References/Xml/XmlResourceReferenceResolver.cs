using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Reflection;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlResourceReferenceResolver : IXmlReferenceResolver {
        private const string DefaultAssemblyName = "System.Web.Extensions";

        public bool CanGetReferences(JavaScriptReference original) {
            return true;
        }

        public IEnumerable<JavaScriptReference> GetReferences(IXPathNavigable referencesRoot, JavaScriptReference original) {
            var resourceNodes = referencesRoot.CreateNavigator().Select("reference[@name]");
            foreach (XPathNavigator node in resourceNodes) {
                yield return this.GetReference(node);
            }
        }

        private JavaScriptResourceReference GetReference(XPathNavigator referenceNode) {
            var assemblyName = referenceNode.GetAttribute("assembly", "");
            if (string.IsNullOrEmpty(assemblyName))
                assemblyName = DefaultAssemblyName;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = Array.Find(assemblies, a => a.GetName().Name == assemblyName);
            if (assembly == null) {
                // TODO: Make it work when the System.Web.Extensions or other GAC assemblies are referenced
                // by their partial name but not loaded in the current AppDomain. I should check what 
                // Visual Studio does in these cases.
                // Right now I can't even test System.Web.Extensions special case without mocking Assembly.Load,
                // which is a bit of an overkill.

                assembly = Assembly.Load(assemblyName);
            }

            var resourceName = referenceNode.GetAttribute("name", "");

            return new JavaScriptResourceReference(resourceName, assembly);
        }
    }
}
