/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.js.ExpectedException.js" />

function TestException() {}

var ExpectedExceptionTest = TestFixture({
    testExpectedException : Test(
        ExpectedException(TestException),
        function() {
            throw new TestException();
        }
    )
});