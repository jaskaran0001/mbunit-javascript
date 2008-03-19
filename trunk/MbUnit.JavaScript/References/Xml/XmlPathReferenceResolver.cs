using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlPathReferenceProvider : IXmlReferenceResolver {
        public IEnumerable<JavaScriptReference> GetReferences(IXPathNavigable referencesRoot, string scriptPath) {
            var pathNodes = referencesRoot.CreateNavigator().Select("reference/@path");
            foreach (XPathNavigator pathNode in pathNodes) {
                string path = this.GetFullPath(scriptPath, pathNode.Value);
                yield return JavaScriptReference.Files(path, "");
            }
        }

        private string GetFullPath(string scriptPath, string referencePath) {
            if (Path.IsPathRooted(referencePath))
                return referencePath;

            var scriptDirectory = Path.GetDirectoryName(scriptPath);

            return Path.Combine(scriptDirectory, referencePath);
        }
    }
}
