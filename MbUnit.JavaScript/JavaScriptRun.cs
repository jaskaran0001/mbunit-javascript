using System;
using System.Collections.Generic;

using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;

using MbUnit.JavaScript.Engines;
using MbUnit.JavaScript.Internal;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript {
    [JavaScriptResourceReference("MbUnit.JavaScript.js.Core.Runner.js", typeof(JavaScriptRun))]
    internal class JavaScriptRun : Run {
        internal const string RunnerTypeName = "MbUnit.Core.Runner";
        internal const string CurrentRunnerName = RunnerTypeName + ".__current";

        // ashmind: Script Engine has to be preserved during the life of the run,
        // or the referenced scripts will be unloaded.
        // TODO: Fix it by referencing engine in each ScriptObject.
        private readonly IScriptEngine engine;

        private readonly JavaScriptDependencyResolver dependencyResolver;

        public JavaScriptRun(IJavaScriptReferenceExtractor referenceExtractor) : base("JavaScript", false) {
            this.engine = ScriptEngineFactory.Create();
            this.dependencyResolver = new JavaScriptDependencyResolver(referenceExtractor);
        }

        public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t) {
            foreach (var fixture in this.Reflect(t)) {
                //var vertex = (RunInvokerVertex)tree.Graph.AddVertex();
                //tree.Graph.AddEdge(parent, vertex);

                foreach (var invoker in fixture.Invokers) {
                    tree.AddChild(/*vertex*/parent, invoker);
                }
            }
        }

        internal IEnumerable<JavaScriptImportedFixture> Reflect(Type t) {
            var scripts = new List<string>();

            scripts.AddRange(this.ReflectAndLoadScripts(this.GetType()));
            scripts.AddRange(this.ReflectAndLoadScripts(t));
            
            scripts.ForEach(engine.Load);

            var fixturesData = this.LoadTests(engine);
            foreach (IScriptObject fixtureData in fixturesData) {
                var fixture = new JavaScriptImportedFixture {
                    Name     = (string)fixtureData["name"]
                };
                fixture.Invokers = this.GetInvokers(fixture, fixtureData);

                yield return fixture;
            }
        }

        private IEnumerable<JavaScriptRunInvoker> GetInvokers(JavaScriptImportedFixture fixture, IScriptObject fixtureData) {
            var invokersData = (IScriptArray)fixtureData["invokers"];
            foreach (IScriptObject invokerData in invokersData) {
                yield return new JavaScriptRunInvoker(this, fixture, invokerData);
            }
        }

        private IScriptArray LoadTests(IScriptEngine engine) {
            string loadFunction = string.Format(
                @"function() {{
                    {0} = new {1}();
                    return {0}.load();
                 }}", CurrentRunnerName, RunnerTypeName
            );

            return (IScriptArray)engine.Eval("(" + loadFunction + ")()");
        }

        private IEnumerable<string> ReflectAndLoadScripts(Type type) {
            var attributes = (JavaScriptReferenceAttribute[])type.GetCustomAttributes(
                typeof(JavaScriptReferenceAttribute), true
            );
            
            var references = Array.ConvertAll(attributes, a => a.Reference);
            return this.dependencyResolver.LoadScripts(references);
        }
    }
}
