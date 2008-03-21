using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MbUnit.JavaScript.References.Xml.Resources {
    internal class ResourceLookup : IResourceLookup {
        // ashmind: Should be changed to a HashSet when moving to .Net 3.5
        private readonly IDictionary<string, bool> resourceNames = new Dictionary<string, bool>();

        public ResourceLookup(Assembly assembly) {
            foreach(var name in assembly.GetManifestResourceNames()) {
                resourceNames.Add(name, true);
            }
        }

        public bool ResourceExists(string resourceName) {
            return resourceNames.ContainsKey(resourceName);
        }
    }
}