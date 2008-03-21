using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using MbUnit.JavaScript.References.Xml.Resources;

namespace MbUnit.JavaScript.References.Xml {
    internal class ResourceLookupFactory : IResourceLookupFactory {
        public IResourceLookup CreateLookup(Assembly assembly) {
            return new ResourceLookup(assembly);
        }
    }
}
