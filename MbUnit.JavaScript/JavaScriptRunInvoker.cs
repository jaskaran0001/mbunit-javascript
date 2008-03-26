using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Invokers;

using MbUnit.JavaScript.Engines;
using MbUnit.JavaScript.Internal;

namespace MbUnit.JavaScript {
    internal class JavaScriptRunInvoker : RunInvoker {
        private readonly string name;
        private readonly IScriptObject invoker;
        private readonly JavaScriptImportedFixture fixture;

        public JavaScriptRunInvoker(JavaScriptRun run, JavaScriptImportedFixture fixture, IScriptObject invoker) 
            : base(run) {

            this.name = (string)invoker["name"];
            this.fixture = fixture;
            this.invoker = invoker;
        }

        public override object Execute(object o, IList args) {
            if (args.Count > 0)
                throw new NotImplementedException("Arguments are not implemented for JavaScript runs.");

            return this.invoker.Invoke("execute");
        }

        public override string Name {
            get { return this.fixture.Name + "." + this.name; }
        }
    }
}
