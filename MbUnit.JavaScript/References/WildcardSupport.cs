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
