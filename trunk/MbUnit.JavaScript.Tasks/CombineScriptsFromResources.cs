using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tasks {
    public class CombineScriptsFromResources : AppDomainIsolatedTask {
        [Required]
        public ITaskItem OriginalAssemblyPath { get; set; }

        [Required]
        public ITaskItem[] OriginalResources { get; set; }

        [Required] [Output]
        public ITaskItem CombinedFile { get; set; }

        public bool StripComments { get; set; }

        public override bool Execute() {
            var references = this.GetOriginalReferences();

            var resolver = new ScriptDependencyResolver(new ScriptXmlReferenceExtractor());
            var scripts = resolver.LoadScripts(references);
            using (var writer = new StreamWriter(this.CombinedFile.ItemSpec)) {
                foreach (var script in scripts) {
                    writer.WriteLine("// " + script.Name);

                    var content = script.Content;
                    if (this.StripComments)
                        content = this.RemoveComments(content);
                    writer.WriteLine(content);
                }
            }

            return true;
        }

        private string RemoveComments(string text) {
            text = Regex.Replace(text, "//.+[\r\n]*", "");
            text = Regex.Replace(text, @"/\*.*?\*/[ \t]*[\r\n]*",  "", RegexOptions.Singleline);

            return text;
        } 

        private IEnumerable<IScriptReference> GetOriginalReferences() {
            var assembly = Assembly.LoadFrom(this.OriginalAssemblyPath.ItemSpec);

            var references = new List<IScriptReference>();
            foreach (var resource in this.OriginalResources) {
                references.Add(new ScriptResourceReference(resource.ItemSpec, assembly));
            }
            return references;
        }
    }
}
