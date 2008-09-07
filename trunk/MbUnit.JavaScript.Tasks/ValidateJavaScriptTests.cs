using System;
using System.Collections.Generic;
using System.Reflection;

using MbUnit.Core;
using MbUnit.Core.Filters;
using MbUnit.Core.Invokers;
using MbUnit.Core.Remoting;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MbUnit.JavaScript.Tasks {
    public class ValidateJavaScriptTests : AppDomainIsolatedTask {
        private static readonly IRunPipeFilter NoFilter = new AnyRunPipeFilter();

        public ITaskItem AssemblyPath { get; set; }

        public override bool Execute() {
            try {
                return this.ExecuteSafe();
            }
            catch (Exception ex) {
                Log.LogErrorFromException(ex, true);
                return false;
            }
        }

        private bool ExecuteSafe() {
            var assembly = Assembly.LoadFrom(this.AssemblyPath.ItemSpec);
            var explorer = new FixtureExplorer(assembly);
            var exceptionsDetected = false;

            explorer.Explore();
            foreach (Fixture fixture in explorer.FixtureGraph.Fixtures) {
                fixture.Load(NoFilter);
                exceptionsDetected = this.DetectExceptions(fixture) || exceptionsDetected;
            }

            return !exceptionsDetected;
        }

        private bool DetectExceptions(Fixture fixture) {
            foreach (RunPipeStarter starter in fixture.Starters)
            foreach (RunInvokerVertex invokerVertex in starter.Pipe.Invokers) {
                var failed = invokerVertex.Invoker as FailedLoadingRunInvoker;
                if (failed != null) {
                    Log.LogErrorFromException(failed.Exception, true);
                    return true;
                }
            }

            return false;
        }
    }
}
