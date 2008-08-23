using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using MbUnit.JavaScript.References;
using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.Tests.Of.References {
    [TestFixture]
    public class XmlReferenceParserTest : MockingTestBase {
        [Test]
        public void TestParse() {
            const string Script = @"
                /// <reference path='C:\Test1.js' />
                /// <reference name='Testing.Test2.js' assembly='Testing' />
                function Test() {
                    return ""/// <reference name='ShouldNotBeFound.js' />"";
                }
            ";

            var parser = new XmlReferenceParser();
            var references = parser.Parse(Script);

            XPathAssert.NodesExist(references, @"reference[@path='C:\Test1.js']");
            XPathAssert.NodesExist(references, @"reference[@name='Testing.Test2.js' and @assembly='Testing']");
            XPathAssert.NoNodesExist(references, @"reference[@name='ShouldNotBeFound.js']");
        }
    }
}
