function AssertionFailureException(message) {
    this.message = message;
}

var Assert = {
    fail : function(message) {
        throw new AssertionFailureException(message);
    },
    
    areEqual : function(expected, actual) {
        var equal = (expected == actual);
        if (!equal)
            Assert.fail();
    },
    
    isDefined : function(value) {
        if (value === undefined)
            Assert.fail("Value is undefined.");
    }
};