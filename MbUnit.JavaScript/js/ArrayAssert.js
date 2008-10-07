/// <reference path="Common.js" />
/// <reference path="Core\Formatter.js" />
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

with (MbUnit.Core) {

var ArrayAssert = {
    _resolveMessageOrEquals : function(messageOrEquals, otherMessageOrEquals) {        
        var firstIsFunction = (typeof messageOrEquals == 'function');
        
        var options = {
            equals : firstIsFunction ? messageOrEquals : otherMessageOrEquals,
            message : !firstIsFunction ? messageOrEquals : otherMessageOrEquals
        };
        
        return options;
    },

    areEquivalent : function(expected, actual, messageOrEquals, otherMessageOrEquals) {
        var options = this._resolveMessageOrEquals(messageOrEquals, otherMessageOrEquals);
    
        var notFoundInExpected = actual.concat([]);
        var notFoundInActual = expected.concat([]);
                
        expected.forEach(function(expectedValue) {
            var found = false;        
            actual.forEach(function(actualValue) {
                if (expectedValue === actualValue) {
                    notFoundInActual.remove(actualValue);
                    found = true;
                    return;
                }
            });
            
            if (found)
                notFoundInExpected.remove(expectedValue);
        });

        if (notFoundInExpected.length == 0 && notFoundInActual.length == 0)
            return;
               
        var message = [
            "Arrays are not equivalent.\r\n",
            "Expected: ", Formatter.toString(expected), "\r\n",
            "Actual: ", Formatter.toString(actual), "\r\n"
        ];
        
        if (options.message) {
            message.unshift(options.message, "\r\n");
        }

        if (notFoundInActual.length > 0) {
            message.push("Missing elements: ", Formatter.toString(notFoundInActual), "\r\n");
        }

        if (notFoundInExpected.length > 0) {
            message.push("Unexpected elements: ", Formatter.toString(notFoundInExpected), "\r\n");
        }

        Assert.fail(message.join(''));
    },
    
    _comparers : {
        equality : function(first, second) { return first == second; },
        sameness : function(first, second) { return first === second; }
    },   
    
    areElementsEqual : function(expected, actual, equals) {
        if (!equals)
            equals = ArrayAssert._comparers.equality;

        var failedIndex;
        for (var i = 0; i < expected.length; i++) {
            if (!equals(actual[i], expected[i])) {
                failedIndex = i;
                break;
            }            
        }

        if (failedIndex === undefined) {
            Assert.areEqual(expected.length, actual.length, "Arrays do not have the same number of elements.");
            return;
        }

        var message = [
            "Arrays are not equal.\r\n",
            "Expected: ", Formatter.toString(expected), "\r\n",
            "Actual: ", Formatter.toString(actual), "\r\n",
            "At index ", failedIndex, 
            " actual [", Formatter.toString(actual[failedIndex]), "] != expected [", Formatter.toString(expected[failedIndex]) ,"].\r\n"
        ];

        Assert.fail(message.join(''));        
    },
    
    areElementsSame : function(expected, actual) {
        ArrayAssert.areElementsEqual(expected, actual, ArrayAssert._comparers.sameness);
    }
};

}