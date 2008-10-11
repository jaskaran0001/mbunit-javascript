/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

function RowTestTestException() {}

var RowTestTest = TestFixture({
    testExpectedExceptionOnAllRows : RowTest(
        Row(),
        Row(),
        ExpectedException(RowTestTestException),
        function() {
            throw new RowTestTestException();
        }
    ),
    
    testExpectedExceptionOnSingleRow : RowTest(
        Row(false),
        Row(true, { expectedException : RowTestTestException }),
        function(shouldThrow) {
            if (!shouldThrow)
                return;
                        
            throw new RowTestTestException();
        }
    )
});