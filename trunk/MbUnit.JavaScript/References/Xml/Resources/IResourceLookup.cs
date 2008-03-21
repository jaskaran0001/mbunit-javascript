using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.References.Xml.Resources {
    internal interface IResourceLookup {
        bool ResourceExists(string resourceName);
    }
}