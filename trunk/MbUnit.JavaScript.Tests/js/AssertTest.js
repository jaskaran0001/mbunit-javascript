/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.js.Assert.js" />
/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.js.TestFixture.js" />

var AssertTest = TestFixture({
    testAreEqual : RowTest(
        Row(1,    1),
        Row("x", "x"),
        Row(true, true),        
        Row(2,   "2",  { expectedException : AssertionFailureException }),
        Row(true, 1,   { expectedException : AssertionFailureException }),
        function(first, second) {
            Assert.areEqual(first, second);
        }
    )
});