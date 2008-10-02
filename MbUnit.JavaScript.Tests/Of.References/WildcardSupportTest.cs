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
