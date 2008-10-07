/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var __global = this;
var PollutionTest = TestFixture({
    testPollution: Test(function() {
        var objectNamesToTest = ['Array', 'Object', 'Function', 'Number'];
        
        for (var i = 0; i < objectNamesToTest.length; i++) {
            var name = objectNamesToTest[i];
            
            var enumerable = [];
            for (var key in __global[name].prototype) {
                enumerable.push(key);
            }
            
            ArrayAssert.areEquivalent([], enumerable, name + ".prototype is polluted by MbUnit.JavaScript.");
        }
    })
});