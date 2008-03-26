using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace MbUnit.JavaScript {
    [Serializable]
    public class ResourceNotFoundException : Exception {
        public ResourceNotFoundException(Assembly assembly, string resourceName) 
            : this(GetDefaultMessage(assembly, resourceName), assembly, resourceName)
        {
        }

        public ResourceNotFoundException(string message, Assembly assembly, string resourceName) 
            : base(message) 
        {
            this.Assembly = assembly;
            this.ResourceName = resourceName;
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        private static string GetDefaultMessage(Assembly assembly, string resourceName) {
            return string.Format(
                "Resource '{0}' couldn't be found in assembly '{1}'.",
                    resourceName, assembly.GetName().FullName
            );
        }

        public Assembly Assembly { get; private set; }
        public string ResourceName { get; private set; }
    }
}
