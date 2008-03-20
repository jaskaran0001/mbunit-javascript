using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    public interface IXmlReferenceResolver {
        JavaScriptReference TryResolve(XPathNavigator referenceNode, JavaScriptReference original);
    }
}
