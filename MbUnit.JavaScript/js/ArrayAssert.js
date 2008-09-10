/// <reference path="Assert.js" />

/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

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
