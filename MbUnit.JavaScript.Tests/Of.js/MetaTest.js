/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var MetaTest = TestFixture({
    _iAmMetaTest : true,

    testThisIsCorrectInTest: Test(function() {
        Assert.isDefined(this._iAmMetaTest);
    }),

    testThisIsCorrectInRowTest: RowTest(
        Row(),
        function() {
            Assert.isDefined(this._iAmMetaTest);
        }
    )
});