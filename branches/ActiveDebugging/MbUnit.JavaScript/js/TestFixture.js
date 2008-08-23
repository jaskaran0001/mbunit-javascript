/// <reference path="Common.js" />

function TestFixture(body) {
    /// <param name="body" mayBeNull="false" optional="false" type="Object">Test fixture body.</param>        
    Object.extend(body, TestFixture.prototype);
    
    return body;
}

TestFixture.prototype = {
    getRunInvokers : function() {
        var tests = [];
    
        for (var name in this) {
            var value = this[name];
            if (value && value.getRunInvokers) {
                tests = tests.concat(value.getRunInvokers(name));
            }
        }
        
        return tests;
    }
}