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
                this.References = references;
            }

            public string Content { get; private set; }
            public IEnumerable<IJavaScriptReference> References { get; private set; }

            public IEnumerable<string> LoadAll() {
                yield return this.Content;
            }
        }

        #endregion

        [Test]
        public void TestLoadScripts() {
            var script4 = new ScriptStub("4");
            var script3 = new ScriptStub("3", script4);
            var script2 = new ScriptStub("2");
            var script1 = new ScriptStub("1", script2, script3, script4);

            var extractor = Mock<IJavaScriptReferenceExtractor>(mock => {
                Expect.AtLeastOnce.On(mock)
                      .Method("GetReferences")
                      .Will(Be.A(
                           (IJavaScriptReference script) => ((ScriptStub)script).References
                      ));
            });

            var resolver = new JavaScriptDependencyResolver(extractor);
            var scriptContents = resolver.LoadScripts(new[] {script1});

            CollectionAssert.AreElementsEqual(
                scriptContents, new[] { "2", "4", "3", "1" }
            );
        }
    }
}
