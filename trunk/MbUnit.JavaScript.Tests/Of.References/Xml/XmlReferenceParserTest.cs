﻿/* 
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
using MbUnit.JavaScript.References.Xml;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
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

        [Test]
        public void TestParseDoesNotThrowOnNonXmlComments() {
            const string Script = @"/// A comment: < />";

            ExceptionAssert.DoesNotThrow(
                () => new XmlReferenceParser().Parse(Script)
            );
        }
    }
}