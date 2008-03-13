using System;
using System.Collections.Generic;

using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Runs;
using MbUnit.JavaScript.Engines;
using MbUnit.JavaScript.Internal;

namespace MbUnit.JavaScript {
    internal class JavaScriptRun : Run {
        internal const string RunnerTypeName = "MbUnit.Core.Runner";
        internal const string CurrentRunnerName = RunnerTypeName + ".__current";

        // ashmind: Script Engine has to be preserved during the life of the run,
        // or the referenced scripts will be unloaded.
        private IScriptEngine engine;

        public JavaScriptRun() : base("JavaScript", false) {
            this.engine = ScriptEngineFactory.Create();
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
                yield return new JavaScriptImportedFixture {
                    Name     = (string)fixtureData["name"],
                    Invokers = this.GetInvokers(fixtureData)
                };
            }
        }

        private IEnumerable<JavaScriptRunInvoker> GetInvokers(IDictionary<string, object> fixtureData) {
            var invokersData = (IScriptArray)fixtureData["invokers"];
            foreach (IScriptObject invokerData in invokersData) {
                yield return new JavaScriptRunInvoker(this, invokerData);
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
            var method = TypeHelper.GetAttributedMethod(type, typeof(JavaScriptProviderAttribute));
            object instance = null;            
            if (!method.IsStatic)
                instance = TypeHelper.CreateInstance(type);
            
            var references = (IEnumerable<JavaScriptReference>)method.Invoke(instance, null);
            var scripts = new List<string>();
            foreach (var reference in references) {
                scripts.AddRange(reference.LoadAll());
            }

            return scripts;
        }

        [JavaScriptProvider]
        public IEnumerable<JavaScriptReference> GetRequiredScripts() {
            yield return JavaScriptReference.Resources(@"Common\.js$");
            yield return JavaScriptReference.Resources(@".js$");
        }
    }
}
