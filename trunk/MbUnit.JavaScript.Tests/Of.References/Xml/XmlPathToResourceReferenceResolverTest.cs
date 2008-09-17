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
using System.Reflection;
using System.Xml.XPath;

using Moq;

using MbUnit.Framework;

using MbUnit.JavaScript.References;
using MbUnit.JavaScript.References.Xml;
using MbUnit.JavaScript.References.Xml.Resources;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlPathToResourceReferenceResolver))]
    public class XmlPathToResourceReferenceResolverTest : XmlReferenceResolverTestBase {
        [RowTest]
        [Row(@"Test.js",        "Dir.Original.js",       "Dir.Test.js")]
        [Row(@"..\DirX\My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"../DirX/My.js",  "Dir1.Dir2.Original.js", "Dir1.DirX.My.js")]
        [Row(@"./DirX/My.js",   "Dir.Original.js",       "Dir.DirX.My.js")]
        [Row(@".././../My.js",  "Dir1.Dir2.Original.js", "My.js")]
        public void TestResolve(string referencePath, string originalResourceName, string expectedResourceName) {
            var xml = this.GetPathReferenceXml(referencePath);
            var lookupFactory = this.MockLookupFactoryWithOneExistingResource(expectedResourceName);
            var resolver = new XmlPathToResourceReferenceResolver(lookupFactory);

            var original = new ScriptResourceReference(originalResourceName, this.GetType().Assembly);
            var reference = (ScriptResourceReference)resolver.TryResolve(xml, original);

            Assert.IsNotNull(reference);
            Assert.AreSame(original.Assembly, reference.Assembly);
            Assert.AreEqual(expectedResourceName, reference.ResourceName);
        }

        private IResourceLookupFactory MockLookupFactoryWithOneExistingResource(string resourceName) {
            var lookup = Mock<IResourceLookup>(
                mock => mock.Expect(x => x.ResourceExists(resourceName)).Returns(true)
            );

            return Mock<IResourceLookupFactory>(
                mock => mock.Expect(x => x.CreateLookup(It.IsAny<Assembly>())).Returns(lookup)
            );            
        }

        private XPathNavigator GetPathReferenceXml(string path) {
            var attributeString = string.Format("path='{0}'", path);
            return this.GetReferenceXml(attributeString);
        }
    }
}
