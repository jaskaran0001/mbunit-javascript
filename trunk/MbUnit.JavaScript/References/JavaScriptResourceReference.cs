using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MbUnit.JavaScript {
    internal class JavaScriptResourceReference : JavaScriptReference {
        public string Pattern { get; private set; }
        public Assembly Assembly { get; private set; }

        public JavaScriptResourceReference(string pattern, Assembly asssembly) {
            this.Pattern = pattern;
            this.Assembly = asssembly;
        }

        public override IEnumerable<string> LoadAll() {
            var names = this.FindScriptNames();
            return this.LoadAllScripts(names);
        }

        private IList<string> FindScriptNames() {
            var resourceNames = new List<string>(this.Assembly.GetManifestResourceNames());
            var pattern = new Regex(this.Pattern);

            return resourceNames.FindAll(name => pattern.IsMatch(name));
        }

        private IEnumerable<string> LoadAllScripts(IEnumerable<string> names) {
            foreach (var name in names) {
                using (var stream = this.Assembly.GetManifestResourceStream(name))
                using (var reader = new StreamReader(stream)) {
                    yield return reader.ReadToEnd();
                }
            }
        }
    }
}
