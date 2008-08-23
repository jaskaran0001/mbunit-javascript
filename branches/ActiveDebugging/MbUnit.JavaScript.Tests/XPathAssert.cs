using System;
using System.Collections;
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