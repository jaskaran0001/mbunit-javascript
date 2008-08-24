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
                this.References = new List<IJavaScriptReference>(references);
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
