using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MbUnit.JavaScript.Tasks {
    public class CreateHtmlUILoadScript : Task {
        [Required]
        public ITaskItem[] Scripts { get; set; }

        [Required]
        [Output]
        public ITaskItem LoadScript { get; set; }

        public override bool Execute() {
            var count = this.Scripts.Length;
            var index = 0;

            using (var writer = new StreamWriter(this.LoadScript.ItemSpec)) {
                writer.WriteLine("parent.MbUnit.UI.loadTests(");
                foreach (var script in this.Scripts) {
                    index += 1;

                    writer.Write("    '{0}'", GetEscapedScriptFullPath(script));
                    if (index < count)
                        writer.WriteLine(",");
                    else
                        writer.WriteLine();
                }

                writer.WriteLine(");");
            }

            return true;
        }

        private string GetEscapedScriptFullPath(ITaskItem script) {
            var path = script.GetMetadata("FullPath");

            // to prevent javascript from interpreting \ as an escape character
            path = path.Replace("\\", "\\\\");

            return "file://" + path;
        }
    }
}
