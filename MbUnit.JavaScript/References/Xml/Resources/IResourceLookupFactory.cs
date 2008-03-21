using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MbUnit.JavaScript.References.Xml.Resources {
    internal interface IResourceLookupFactory {
        IResourceLookup CreateLookup(Assembly assembly);
    }
}