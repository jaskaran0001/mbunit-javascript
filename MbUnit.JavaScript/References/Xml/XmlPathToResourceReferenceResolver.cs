using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using MbUnit.JavaScript.References.Xml.Resources;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlPathToResourceReferenceResolver : IXmlReferenceResolver {
        private static readonly Regex PathSplitRegex = new Regex(
            @"[" + Regex.Escape(Path.DirectorySeparatorChar.ToString() + Path.AltDirectorySeparatorChar) + "]"
        );

        private readonly IResourceLookupFactory lookupFactory;

        public XmlPathToResourceReferenceResolver(IResourceLookupFactory lookupFactory) {
            this.lookupFactory = lookupFactory;
        }

        public XmlPathToResourceReferenceResolver()
            : this(new ResourceLookupFactory())
        {
        }

        public IJavaScriptReference TryResolve(XPathNavigator referenceNode, IJavaScriptReference original) {
            var path = referenceNode.GetAttribute("path", "");
            if (string.IsNullOrEmpty(path))
                return null;

            var originalAsResourceReference = original as JavaScriptResourceReference;
            if (originalAsResourceReference == null || Path.IsPathRooted(path))
                return null;

            var assembly = originalAsResourceReference.Assembly;
            var lookup = this.lookupFactory.CreateLookup(assembly);

            string resourceName = this.GuessResourceName(path, originalAsResourceReference, lookup);
            return new JavaScriptResourceReference(resourceName, originalAsResourceReference.Assembly);
        }

        private string GuessResourceName(string referencePath, JavaScriptResourceReference original, IResourceLookup lookup) {
            // ashmind: This is some ugly, but tested code. I just haven't found the better way.
            
            var pathParts = PathSplitRegex.Split(referencePath);
            var referencePathSuffixStack = new Stack<string>();

            int skip = 0;
            for (int i = pathParts.Length - 1; i >= 0; i--) {
                var path = pathParts[i];
                if (path == "..") {
                    skip += 1;
                    continue;
                }

                if (path == ".")
                    continue;

                if (skip > 0) {
                    skip -= 1;
                    continue;
                }

                referencePathSuffixStack.Push(path);
            }
            string referencePathSuffix = string.Join(".", referencePathSuffixStack.ToArray());

            var suggestedNames = new List<string>();

            var resourceNameParts = original.ResourceName.Split('.');
            for (int i = resourceNameParts.Length - 1; i - skip >= 0; i--) {
                var suggestedName = string.Join(".", resourceNameParts, 0, i - skip);
                if (suggestedName.Length > 0)
                    suggestedName += ".";

                suggestedName += referencePathSuffix;

                bool exists = lookup.ResourceExists(suggestedName);
                if (exists)
                    return suggestedName;

                suggestedNames.Add(suggestedName);
            }

            throw this.GetNotFoundException(referencePath, original, suggestedNames);
        }

        private Exception GetNotFoundException(string referencePath, JavaScriptResourceReference original, List<string> suggestedNames) {
            var namesString = string.Join(", ", suggestedNames.ToArray());
            return new FileNotFoundException(string.Format(
                "Resource '{0}' referenced by {1} was not found in {2}. Following names were tried: {3}.",
                    referencePath, original.ResourceName, original.Assembly,namesString
            ));
        }
    }
}
