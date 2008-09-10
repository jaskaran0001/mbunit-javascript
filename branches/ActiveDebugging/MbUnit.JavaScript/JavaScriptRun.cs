/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;

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

        public JavaScriptRun(IScriptEngine engine, IJavaScriptReferenceExtractor referenceExtractor)
            : base("JavaScript", false) 
        {
            this.engine = engine;
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
            var scripts = new List<Script>();

            scripts.AddRange(this.ReflectAndLoadScripts(this.GetType()));
            scripts.AddRange(this.ReflectAndLoadScripts(t));
            
            scripts.ForEach(engine.Load);

            var fixturesData = this.LoadTests();
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

        private IScriptArray LoadTests() {
            string loadFunction = string.Format(
                @"function() {{
                    {0} = new {1}();
                    return {0}.load();
                 }}", CurrentRunnerName, RunnerTypeName
            );

            return (IScriptArray)this.engine.Eval("(" + loadFunction + ")()");
        }

        private IEnumerable<Script> ReflectAndLoadScripts(Type type) {
            var attributes = (JavaScriptReferenceAttribute[])type.GetCustomAttributes(
                typeof(JavaScriptReferenceAttribute), true
            );
            
            var references = Array.ConvertAll(attributes, a => a.Reference);
            return this.dependencyResolver.LoadScripts(references);
        }
    }
}
