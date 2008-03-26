/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Xml.XPath;
using System.Reflection;

namespace MbUnit.JavaScript.References.Xml {
    internal class XmlResourceReferenceResolver : IXmlReferenceResolver {
        // ashmind: I am not sure that full name is the best solution considering
        // .Net 2.0 compatibility, but there is no good way to load it from GAC otherwise.
        private const string DefaultAssemblyName = "System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

        public IJavaScriptReference TryResolve(XPathNavigator referenceNode, IJavaScriptReference original) {
            var resourceName = referenceNode.GetAttribute("name", "");
            if (string.IsNullOrEmpty(resourceName))
                return null;

            var assemblyName = referenceNode.GetAttribute("assembly", "");
            if (string.IsNullOrEmpty(assemblyName))
                assemblyName = DefaultAssemblyName;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = Array.Find(assemblies, a => a.GetName().Name == assemblyName);
            if (assembly == null)
                assembly = Assembly.Load(assemblyName);

            return new JavaScriptResourceReference(resourceName, assembly);
        }
    }
}
