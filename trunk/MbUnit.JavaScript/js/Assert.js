/// <reference path="Core\Formatter.js" />

with (MbUnit.Core) {

function AssertionFailureException(message) {
    this.message = message;
}

var Assert = {
    _fail : {
        is : function(value, unexpected) {
            Assert.fail(["Assertion failed: [[", Formatter.toString(value), "]] is ", Formatter.toString(unexpected), "."].join(''));
        },
        
        comparison : function(expected, isNotOperator, actual, message) {
            message = message ? message + " " : "";    
            Assert.fail([
                message,
                "Assertion failed: [[", Formatter.toString(expected), "]]", isNotOperator, "[[", Formatter.toString(actual), "]]"
            ].join(''));
        }  
    },

    fail : function(message) {    
        throw new AssertionFailureException(message);
    },    
    
    _combineMessage : function(message, expectedMessage) {
        return message ? (message + " " + expectedMessage) : expectedMessage;
    },
      
    areSame : function(expected, actual, message) {
        if (expected !== actual)
            Assert._fail.comparison(expected, '!==', actual, message);
    },
    
    areNotSame : function(expected, actual, message) {
        if (expected !== actual)
            Assert._fail.comparison(expected, '===', actual, message);
    },    
    
    areEqual : function(expected, actual, message) {
        if (expected != actual)
            Assert._fail.comparison(expected, '!=', actual, message);
    },
    
    areNotEqual : function(expected, actual) {
        if (expected == actual)
            Assert._fail.comparison(expected, '==', actual, message);
    },
        
    isFalse : function(value) {
        if (value)
            Assert._fail.is(value, "true");
    },

    isTrue : function(value) {
        if (!value)
            Assert._fail.is(value, "false");
    },

    isDefined : function(value) {
        if (value === undefined)
            Assert._fail.is(value, "undefined");
    }
};

}