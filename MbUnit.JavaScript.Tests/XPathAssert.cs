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
using System.IO;
using System.Xml;
using System.Xml.XPath;

using MbUnit.Framework;

namespace MbUnit.JavaScript.Tests {
    public static class XPathAssert
    {
        public static void NodeValueIsEqual(IXPathNavigable document, string xpath, string expected) {
            var nodes = SelectNodesEnsuringCount(document, xpath, 1);
            nodes.MoveNext();
            var node = nodes.Current;

            Assert.AreEqual(expected, node.Value);
        }

        private static void NodeHasAttributes(XPathNavigator node, IEnumerable<string> attributeNames, IXPathNavigable documentForErrorMessage, string xpathForErrorMessage) {
            Assert.AreEqual(XmlNodeType.Element, node.NodeType);

            foreach (string attributeName in attributeNames) {
                var attribute = node.GetAttribute(attributeName, "");
                Assert.IsNotNull(
                    attribute, "Attribute {0} does not exist on {1} {2}",
                        attributeName, xpathForErrorMessage, ForDocument(documentForErrorMessage)
                );
            }
        }

        public static void NodesHaveAttributes(IXPathNavigable document, string xpath, params string[] attributeNames) {
            var nodes = document.CreateNavigator().Select(xpath);

            foreach (XPathNavigator node in nodes) {
                XPathAssert.NodeHasAttributes(node, attributeNames, document, xpath);
            }
        }

        public static void IsAttributeValueUnique(IXPathNavigable document, string xpath, string attributeName) {
            var values = new List<string>();
            var nodes = document.CreateNavigator().Select(xpath);

            foreach (XPathNavigator node in nodes) {
                Assert.AreEqual(XmlNodeType.Element, node.NodeType);
                var attribute = node.GetAttribute(attributeName, "");

                string value = attribute ?? string.Empty;

                Assert.NotIn(value, values);
                values.Add(value);
            }
        }

        public static void SingleNodeExists(IXPathNavigable document, string xpath) {
            SelectNodesEnsuringCount(document, xpath, 1);
        }

        public static void NodesExist(IXPathNavigable document, string xpath) {
            var nodes = document.CreateNavigator().Select(xpath);
            Assert.IsTrue(
                nodes.Count > 0,
                "No nodes found for query {0} {1}",
                    xpath, ForDocument(document)
            );
        }

        public static void NoNodesExist(IXPathNavigable document, string xpath) {
            SelectNodesEnsuringCount(document, xpath, 0);
        }

        private static XPathNodeIterator SelectNodesEnsuringCount(IXPathNavigable document, string xpath, int expectedCount) {
            var nodes = document.CreateNavigator().Select(xpath);
            Assert.AreEqual(
                nodes.Count, expectedCount,
                "Invalid node count for query {0} {1}",
                    xpath, ForDocument(document)
            );

            return nodes;
        }

        private static string ForDocument(IXPathNavigable document) {
            using (var writer = new StringWriter()) {
                var xmlWriter = XmlWriter.Create(
                    writer, new XmlWriterSettings { Indent = true }
                );

                document.CreateNavigator().WriteSubtree(xmlWriter);
                xmlWriter.Close();

                return string.Format(
                    "for document:\r\n{0}\r\n", writer
                );
            }
        }
    }
}