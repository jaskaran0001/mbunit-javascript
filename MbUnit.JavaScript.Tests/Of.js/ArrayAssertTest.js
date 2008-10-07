/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var ArrayAssertTest = TestFixture({
    testAreEquivalent : RowTest(
        Row([1],             [1]),
        Row(['A', 'B'],      ['B', 'A']),
        Row([Test, RowTest], [RowTest, Test]),        
        function(first, second) {
            ArrayAssert.areEquivalent(first, second);
        }
    ),
    
    testAreEquivalentThrowsForInequivalentArrays : RowTest(
        Row(['A', 'B', 'C'], ['A', 'B']),
        Row(['A', 'B'],      ['A', 'B', 'C']),
        ExpectedException(AssertionFailureException),
        function(first, second) {
            ArrayAssert.areEquivalent(first, second);
        }
    )   
});