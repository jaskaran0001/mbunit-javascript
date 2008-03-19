using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    public interface IXmlReferenceResolver {
        bool CanGetReferences(JavaScriptReference original);
        IEnumerable<JavaScriptReference> GetReferences(IXPathNavigable referencesRoot, JavaScriptReference original);
    }
}
