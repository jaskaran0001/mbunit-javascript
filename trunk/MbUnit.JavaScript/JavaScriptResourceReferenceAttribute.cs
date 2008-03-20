using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    public class JavaScriptResourceReferenceAttribute : JavaScriptReferenceAttribute {
        private readonly IJavaScriptReference reference;

        public JavaScriptResourceReferenceAttribute(string resourceName, string assemblyName)
            : this(resourceName, Assembly.Load(assemblyName))
        {
        }

        public JavaScriptResourceReferenceAttribute(string resourceName, Type anyTypeFromRequiredAssembly) 
            : this(resourceName, anyTypeFromRequiredAssembly.Assembly)
        {
        }

        private JavaScriptResourceReferenceAttribute(string resourceName, Assembly assembly) {
            this.reference = new JavaScriptResourceReference(resourceName, assembly);
        }

        public override IJavaScriptReference Reference {
            get { return reference; }
        }
    }
}
