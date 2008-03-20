using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    public interface IXmlReferenceResolver {
        IJavaScriptReference TryResolve(XPathNavigator referenceNode, IJavaScriptReference original);
    }
}
