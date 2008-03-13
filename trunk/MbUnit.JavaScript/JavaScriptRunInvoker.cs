using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Invokers;

using MbUnit.JavaScript.Engines;

namespace MbUnit.JavaScript {
    internal class JavaScriptRunInvoker : RunInvoker {
        private readonly string name;
        private readonly IScriptObject invoker;

        public JavaScriptRunInvoker(JavaScriptRun run, IScriptObject invoker) 
            : base(run) {

            this.name = (string)invoker["name"];
            this.invoker = invoker;
        }

        public override object Execute(object o, IList args) {
            if (args.Count > 0)
                throw new NotImplementedException("Arguments are not implemented for JavaScript runs.");

            return this.invoker.Invoke("execute");
        }

        public override string Name {
            get { return this.name; }
        }
    }
}
