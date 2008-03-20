using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlPathReferenceResolver : IXmlReferenceResolver {
        public JavaScriptReference TryResolve(XPathNavigator referenceNode, JavaScriptReference original) {
            var path = referenceNode.GetAttribute("path", "");
            if (string.IsNullOrEmpty(path))
                return null;

            var originalAsFileReference = original as JavaScriptFileReference;
            if (originalAsFileReference == null && !Path.IsPathRooted(path))
                return null;

            var fullPath = this.GetFullPath(path, originalAsFileReference);

            return new JavaScriptFileReference(fullPath, "");
        }

        private string GetFullPath(string referencePath, JavaScriptFileReference original) {
            if (Path.IsPathRooted(referencePath))
                return referencePath;

            var scriptDirectory = Path.GetDirectoryName(original.Path);

            return Path.GetFullPath(Path.Combine(scriptDirectory, referencePath));
        }
    }
}
