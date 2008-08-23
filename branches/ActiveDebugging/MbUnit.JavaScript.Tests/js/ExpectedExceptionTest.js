/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

function TestException() {}

var ExpectedExceptionTest = TestFixture({
    testExpectedException : Test(
        ExpectedException(TestException),
        function() {
            throw new TestException();
        }
    )
});