/// <reference path="Assert.js" />

var ArrayAssert = {
    _notMarked : function(value) {
        return !value.__foundByArrayAssert;
    },
    
    _mark : function(value) {
        value.__foundByArrayAssert = true;
    },
    
    _unmark : function(value) {
        delete value.__foundByArrayAssert;
    },

    areEquivalent : function(expected, actual) {
        expected.forEach(function(expectedValue) {
            var found = false;        
            actual.forEach(function(actualValue) {
                if (expectedValue === actualValue) {
                    ArrayAssert._mark(actualValue);
                    found = true;
                    return;
                }
            });
            
            if (found)
                ArrayAssert._mark(expectedValue);
        });
        
        var notFoundInExpected = actual.where(ArrayAssert._notMarked);
        var notFoundInActual = expected.where(ArrayAssert._notMarked);
        
        actual.forEach(ArrayAssert._unmark);
        expected.forEach(ArrayAssert._unmark);        
        
        if (notFoundInExpected.length == 0 && notFoundInActual.length == 0)
            return;
        
        var message = [
            "Arrays are not equivalent.\r\n",
            "Expected: ", Assert.toString(expected), "\r\n",
            "Actual: ", Assert.toString(actual), "\r\n"
        ];

        if (notFoundInActual.length > 0) {
            message.push("Missing elements: ", Assert.toString(notFoundInActual), "\r\n");
        }

        if (notFoundInExpected.length > 0) {
            message.push("Unexpected elements: ", Assert.toString(notFoundInExpected), "\r\n");
        }

        Assert.fail(message.join(''));
    },
    
    _comparers : {
        equality : function(first, second) { return first == second; },
        sameness : function(first, second) { return first === second; }
    },   
    
    areElementsEqual : function(expected, actual, equals) {
        Assert.areEqual(expected.length, actual.length, "Arrays do not have the same number of elements.");
    
        if (!equals)
            equals = ArrayAssert._comparers.equality;
    
        var failedIndex;
        for (var i = 0; i < expected.length; i++) {
            if (!equals(actual[i], expected[i])) {
                failedIndex = i;
                break;
            }            
        }
        
        if (failedIndex === undefined)
            return;

        var message = [
            "Arrays are not equal.\r\n",
            "Expected: ", Assert.toString(expected), "\r\n",
            "Actual: ", Assert.toString(actual), "\r\n",
            "At index ", failedIndex, 
            " actual [", Assert.toString(actual[failedIndex]), "] != expected [", Assert.toString(expected[failedIndex]) ,"].\r\n"
        ];
        
        Assert.fail(message.join(''));
    },
    
    areElementsSame : function(expected, actual) {
        ArrayAssert.areElementsEqual(expected, actual, ArrayAssert._comparers.sameness);
    }
};