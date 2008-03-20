using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Reflection;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlResourceReferenceResolver : IXmlReferenceResolver {
        // ashmind: I am not sure that full name is the best solution considering
        // .Net 2.0 compatibility, but there is no good way to load it from GAC otherwise.
        private const string DefaultAssemblyName = "System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

        public IJavaScriptReference TryResolve(XPathNavigator referenceNode, IJavaScriptReference original) {
            var resourceName = referenceNode.GetAttribute("name", "");
            if (string.IsNullOrEmpty(resourceName))
                return null;

            var assemblyName = referenceNode.GetAttribute("assembly", "");
            if (string.IsNullOrEmpty(assemblyName))
                assemblyName = DefaultAssemblyName;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = Array.Find(assemblies, a => a.GetName().Name == assemblyName);
            if (assembly == null)
                assembly = Assembly.Load(assemblyName);

            return new JavaScriptResourceReference(resourceName, assembly);
        }
    }
}
