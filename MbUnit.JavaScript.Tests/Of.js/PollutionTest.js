/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

var __global = this;
var PollutionTest = TestFixture({
    testPollution: Test(function() {
        var objectNamesToTest = ['Array', 'Object', 'Function', 'Number'];
        
        for (var i = 0; i < objectNamesToTest.length; i++) {
            var name = objectNamesToTest[i];
            this.assertIsNotPolluted(__global[name], name);            
            this.assertIsNotPolluted(__global[name].prototype, name + ".prototype");
        }
    }),
    
    assertIsNotPolluted : function(object, name) {
        var enumerable = [];
        for (var key in object) {
            if (/^__MbUnit/.test(key)) // correctly namespaced
                continue;

            enumerable.push(key);
        }
        
        ArrayAssert.areEquivalent([], enumerable, name + " is polluted by MbUnit.JavaScript.");        
    },
    
    assertDoesNotDefineWindow : function(object, name) {
        Assert.isUndefined(window);
    }
});