using System;
using System.Collections.Generic;
using System.Text;

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
            public ScriptStub(string content, params IJavaScriptReference[] references) {
                this.Content = content;
                this.References = new List<IJavaScriptReference>(references) ?? new List<IJavaScriptReference>();
            }

            public string Content { get; private set; }
            public List<IJavaScriptReference> References { get; private set; }

            public void AddReferences(params IJavaScriptReference[] references) {
                this.References.AddRange(references);
            }

            public string LoadContent() {
                return this.Content;
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
            var scriptContents = resolver.LoadScripts(new[] {script1});

            CollectionAssert.AreElementsEqual(
                scriptContents, new[] { "4", "2", "3", "1" }
            );
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
            var scriptContents = resolver.LoadScripts(new[] { script1 });

            CollectionAssert.AreElementsEqual(
                scriptContents, new[] { "3", "2", "1" }
            );
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
