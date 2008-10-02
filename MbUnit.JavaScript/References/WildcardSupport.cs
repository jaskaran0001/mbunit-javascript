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
using System.Text.RegularExpressions;

namespace MbUnit.JavaScript.References {
    public class WildcardSupport {
        private readonly char[] separators;
        private readonly string escapedSeparators;
        private readonly Regex wildcardRootRegex;
        private readonly Regex substitutionRegex;

        public WildcardSupport(params char[] separators) {
            this.separators = separators;
            Array.Sort(this.separators);

            this.escapedSeparators = Regex.Escape(new string(separators));
            wildcardRootRegex = new Regex(this.GetRootPattern());
            substitutionRegex = new Regex(string.Format(@"\*{{1,2}}|[{0}]|[^*{0}]+", this.escapedSeparators));
        }

        private string GetRootPattern() {
            if (this.escapedSeparators.Length == 0) 
                return "^(?:[^*]+)";

            return string.Format(@"^((?:[^{0}]+[{0}]+)+)[^{0}]*\*?", this.escapedSeparators);
        }

        public bool HasWildcards(string pattern) {
            return pattern.Contains("*");
        }

        public string GetFixedRoot(string pattern) {
            return wildcardRootRegex.Match(pattern).Groups[1].Value;
        }

        public IEnumerable<string> GetMatches(string pattern, IEnumerable<string> list) {
            var regexString = substitutionRegex.Replace(pattern, match => SubstitutePattern(match.Value));
            var regex = new Regex("^" + regexString + "$");

            foreach(var item in list) {
                if (!regex.IsMatch(item))
                    continue;

                yield return item;
            }
        }

        private string SubstitutePattern(string pattern) {
            if (pattern == "**")
                return ".*";

            if (pattern == "*")
                return "[^" + this.escapedSeparators + "]*";

            if (pattern.Length == 1 && Array.BinarySearch(this.separators, pattern[0]) > -1)
                return "[" + this.escapedSeparators + "]";

            return Regex.Escape(pattern);
        }
    }
}
