using System;
using System.Collections.Generic;

using MbUnit.Framework;
using MbUnit.JavaScript.References;
using MbUnit.JavaScript.Tests.Mocking;
using NMock2;

namespace MbUnit.JavaScript.Tests.Of.References {
    [TestFixture]
    [TestsOn(typeof(JavaScriptDependencyResolver))]
    public class JavaScriptDependencyResolverTest : MockingTestBase {
        #region ScriptStub Class

        private class ScriptStub : IJavaScriptReference {
            public ScriptStub(string name, params IJavaScriptReference[] references) {
                this.Name = name;
                this.References = new List<IJavaScriptReference>(references);
            }

            public string Name { get; private set; }
            public List<IJavaScriptReference> References { get; private set; }

            public ScriptInfo LoadScript() {
                return new ScriptInfo(this.Name, string.Empty);
            }

            public void AddReferences(params IJavaScriptReference[] references) {
                this.References.AddRange(references);
            }
        }

        #endregion

        [Test]
        public void TestLoadScripts() {
            var script4 = new ScriptStub("4");
            var script3 = new ScriptStub("3", script4);
            var script2 = new ScriptStub("2", script4);
            var script1 = new ScriptStub("1", script2, script3, script4);

            var extractor = this.MockExtractor();
            var resolver = new JavaScriptDependencyResolver(extractor);
            var scripts = resolver.LoadScripts(new[] {script1});

            AssertScriptsHaveNames(scripts, "4", "2", "3", "1");
        }

        [Test]
        public void TestAvoidRecursiveReferences() {
            var script1 = new ScriptStub("1");
            var script2 = new ScriptStub("2");
            var script3 = new ScriptStub("3");

            script1.AddReferences(script2);
            script2.AddReferences(script3);
            script3.AddReferences(script1);

            var extractor = this.MockExtractor();
            var resolver = new JavaScriptDependencyResolver(extractor);
            var scripts = resolver.LoadScripts(new[] { script1 });

            AssertScriptsHaveNames(scripts, "3", "2", "1");
        }

        private void AssertScriptsHaveNames(IEnumerable<ScriptInfo> scripts, params string[] expectedNames) {
            var names = new List<string>();
            foreach (var script in scripts) {
                names.Add(script.Name);
            }

            CollectionAssert.AreElementsEqual(expectedNames, names);
        }

        private IJavaScriptReferenceExtractor MockExtractor() {
            return this.Mock<IJavaScriptReferenceExtractor>(mock => {
                Expect.AtLeastOnce.On(mock)
                      .Method("GetReferences")
                      .Will(Be.A(
                           (IJavaScriptReference script) => ((ScriptStub)script).References
                      ));
            });
        }
    }
}
