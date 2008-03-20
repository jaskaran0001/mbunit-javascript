using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    public class XmlReferenceParser {
        public static XmlReferenceParser Default { get; private set; }
        private static readonly Regex CommentRegex = new Regex(@"^\s*///\s*(.*)");

        static XmlReferenceParser() {
            Default = new XmlReferenceParser();
        } 

        public IXPathNavigable Parse(string script) {
            string xmlString = this.ExtractReferencesString(script);
            XPathDocument xml;
            using (var reader = new StringReader(xmlString)) {
                xml = new XPathDocument(reader);
            }

            return xml.CreateNavigator().SelectSingleNode("references");
        }

        private string ExtractReferencesString(string script) {
            string[] lines = script.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var references = new StringBuilder("<references>");

            foreach (var line in lines) {
                var match = CommentRegex.Match(line);
                if (!match.Success)
                    break;

                references.AppendLine(match.Groups[1].Value);
            }

            references.Append("</references>");
            return references.ToString();
        }
    }
}
