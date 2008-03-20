/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

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
    ),
    
    testAreEqualExceptionMessage : Test(
        function() {
            try {
                Assert.areEqual(1, 2);
            }
            catch (ex) {
                Assert.areEqual(ex.message, "Equal assertion failed: [[1]]!=[[2]]");
            }
        }
    )    
});