/// <reference path="External\json.js" />

function AssertionFailureException(message) {
    this.message = message;
}

var Assert = {
    fail : function(message) {    
        throw new AssertionFailureException(message);
    },
    
    _failIs : function(value, unexpected) {
        Assert.fail(["Assertion failed: [[", Assert.toString(value), "]] is ", Assert.toString(unexpected), "."].join(''));
    },
    
    _failComparison : function(expected, isNotOperator, actual, message) {
        message = message ? message + " " : "";    
        Assert.fail([
            message,
            "Assertion failed: [[", Assert.toString(expected), "]]", isNotOperator, "[[", Assert.toString(actual), "]]"
        ].join(''));
    },    
    
    _combineMessage : function(message, expectedMessage) {
        return message ? (message + " " + expectedMessage) : expectedMessage;
    },
      
    areSame : function(expected, actual, message) {
        if (expected !== actual)
            Assert._failComparison(expected, '!==', actual, message);
    },
    
    areNotSame : function(expected, actual, message) {
        if (expected !== actual)
            Assert._failComparison(expected, '===', actual, message);
    },    
    
    areEqual : function(expected, actual, message) {
        if (expected != actual)
            Assert._failComparison(expected, '!=', actual, message);
    },
    
    areNotEqual : function(expected, actual) {
        if (expected == actual)
            Assert._failComparison(expected, '==', actual, message);
    },
        
    isFalse : function(value) {
        if (value)
            Assert._failIs(value, "true");
    },

    isTrue : function(value) {
        if (!value)
            Assert._failIs(value, "false");
    },

    isDefined : function(value) {
        if (value === undefined)
            Assert._failIs(value, "undefined");
    },
    
    toString : function(o) {
        return Object.toJSONString(o);
    }
};