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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace MbUnit.JavaScript.References.Xml {
    public class XmlReferenceParser {
        public static XmlReferenceParser Default { get; private set; }
        private static readonly Regex CommentRegex = new Regex(@"^\s*///\s*(<reference.*)");

        static XmlReferenceParser() {
            Default = new XmlReferenceParser();
        } 

        public IXPathNavigable Parse(string script) {
            var xmlString = this.ExtractReferencesString(script);
            XPathDocument xml;
            using (var reader = new StringReader(xmlString)) {
                xml = new XPathDocument(reader);
            }

            return xml.CreateNavigator().SelectSingleNode("references");
        }

        private string ExtractReferencesString(string script) {
            var lines = script.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
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
