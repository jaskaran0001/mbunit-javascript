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
using System.Linq;

using MbUnit.Framework;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tests.Of.References {
    [TestFixture]
    public class WildcardSupportTest {
        [RowTest]
        [Row("Test", false)]
        [Row("Test*", true)]
        [Row("*Test", true)]
        public void TestHasWildcards(string pattern, bool expected) {
            Assert.AreEqual(expected, new WildcardSupport().HasWildcards(pattern));
        }

        [RowTest]
        [Row(@"D:\Development\*Test", @"D:\Development\")]
        [Row(@"D:\Development\Test*", @"D:\Development\")]
        [Row(@"D:\Development*Test",  @"D:\")]
        [Row(@"D:/Development/*Test", @"D:/Development/")]
        public void TestGetFixedRoot(string pattern, string expectedResult) {
            Assert.AreEqual(
                expectedResult, new WildcardSupport('/', '\\').GetFixedRoot(pattern)
            );
        }

        [RowTest]
        [Row(@"D:\**")]
        [Row(@"D:\Development\**")]
        [Row(@"D:\*\Project\Test.js")]
        [Row(@"D:\**\Test.js")]
        [Row(@"D:\*\Project\*.js")]
        [Row(@"D:\*\Project\*.*")]
        [Row(@"D:\Development\*\*.*")]
        [Row(@"D:\Development/*/*.*")]
        public void TestGetMatchesMatchesExpectedInput(string pattern) {
            const string ExpectedMatch = @"D:\Development\Project\Test.js";
            var support = PathBasedWildcardSupport();

            Assert.AreEqual(
                ExpectedMatch,
                support.GetMatches(pattern, Enumerable.Repeat(ExpectedMatch, 1)).SingleOrDefault()
            );
        }

        [RowTest]
        [Row(@"D:\*\Test.js")]
        [Row(@"D:\Development\Project\*")]
        public void TestGetMatchesDoesNotMatchOverSeparators(string pattern) {
            const string UnexpectedMatch = @"D:\Development\Project\Test.js";
            var support = PathBasedWildcardSupport();

            Assert.IsNull(support.GetMatches(pattern, Enumerable.Repeat(UnexpectedMatch, 1)).SingleOrDefault());
        }

        private WildcardSupport PathBasedWildcardSupport() {
            return new WildcardSupport('/', '\\', '.');
        }
    }
}
