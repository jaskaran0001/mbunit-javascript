/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var AssertTest = TestFixture({
    testAreEqual : RowTest(
        Row(1,    1),
        Row("x", "x"),
        Row(true, true),
        Row(2,   "2"),
        Row(true, 1),
        function(first, second) {
            Assert.areEqual(first, second);
        }
    ),
    
    testAreSame : RowTest(
        Row(1,    1),
        Row("x", "x"),
        Row(true, true),
        Row(2,   "2",  { expectedException : AssertionFailureException }),
        Row(true, 1,   { expectedException : AssertionFailureException }),
        function(first, second) {
            Assert.areSame(first, second);
        }
    ),    

    testIsDefined: RowTest(
        Row("x"),
        Row(0),
        Row(""),
        Row(false),        
        Row(undefined,  { expectedException : AssertionFailureException }),
        function(value) {
            Assert.isDefined(value);
        }
    ),
    
    testIsUndefined: Test(function() {
        Assert.isUndefined(undefined);
    }),    
    
    testAreEqualExceptionMessage : Test(
        function() {
            try {
                Assert.areEqual(1, 2);
            }
            catch (ex) {
                Assert.areEqual(ex.message, "Assertion failed: [[1]]!=[[2]]");
            }
        }
    )
});