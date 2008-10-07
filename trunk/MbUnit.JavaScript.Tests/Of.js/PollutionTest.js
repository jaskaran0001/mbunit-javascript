/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var PollutionTest = TestFixture({
    testPollution: Test(function() {
        var objectsToTest = [Array, Object, Function, Number];
        
        for (var i = 0; i < objectsToTest.length; i++) {
            var enumerable = [];
            for (var key in objectsToTest[i].prototype) {
                enumerable.push(key);
            }            
            ArrayAssert.areEquivalent([], enumerable);
        }
    })
});