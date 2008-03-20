using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlPathToResourceReferenceResolver : IXmlReferenceResolver {
        private static readonly Regex ResourceNameToPathRegex = new Regex(@"\.(?=.*\.)");

        public IJavaScriptReference TryResolve(XPathNavigator referenceNode, IJavaScriptReference original) {
            var path = referenceNode.GetAttribute("path", "");
            if (string.IsNullOrEmpty(path))
                return null;

            var originalAsResourceReference = original as JavaScriptResourceReference;
            if (originalAsResourceReference == null || Path.IsPathRooted(path))
                return null;

            string resourceName = this.GetResourceName(path, originalAsResourceReference.ResourceName);
            return new JavaScriptResourceReference(resourceName, originalAsResourceReference.Assembly);
        }

        private string GetResourceName(string referencePath, string originalResourceName) {
            // ashmind: I really dislike this code, however it was the only relatively simple solution I thought about.

            string pathPseudoRoot = Path.GetPathRoot(Directory.GetCurrentDirectory());

            string originalPseudoPath = ResourceNameToPathRegex.Replace(originalResourceName, Path.DirectorySeparatorChar.ToString());
            originalPseudoPath = Path.Combine(pathPseudoRoot, originalPseudoPath);

            string originalDirectory = Path.GetDirectoryName(originalPseudoPath);

            string finalPseudoPath = Path.GetFullPath(Path.Combine(originalDirectory, referencePath));
            finalPseudoPath = finalPseudoPath.Remove(0, pathPseudoRoot.Length);

            return finalPseudoPath.Replace(Path.DirectorySeparatorChar, '.');
        }
    }
}
