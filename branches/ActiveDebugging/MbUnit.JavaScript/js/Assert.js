/// <reference path="Core\Formatter.js" />

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
        /// <summary>
        /// Throws an AssertionException with the message that is
        /// passed in. This is used by the other Assert functions.
        /// </summary>
        /// <param name="format">
        /// The message to initialize the <see cref="AssertionException"/> with.
        /// </param>
        throw new AssertionFailureException(message);
    },    
    
    _combineMessage : function(message, expectedMessage) {
        return message ? (message + " " + expectedMessage) : expectedMessage;
    },
      
    areSame : function(expected, actual, message) {
        /// <summary>
        /// Verifies that two values are same (using === operator). If they are not same 
        /// an AssertionException is thrown.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to be displayed when two values are not same.</param>        
    
        if (expected !== actual)
            Assert._fail.comparison(expected, '!==', actual, message);
    },
    
    areNotSame : function(expected, actual, message) {
        /// <summary>
        /// Verifies that two objects are not same (using !== operator). If they are same
        /// an AssertionException is thrown.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to be displayed when the two values are same.</param>
    
        if (expected !== actual)
            Assert._fail.comparison(expected, '===', actual, message);
    },    
    
    areEqual : function(expected, actual, message) {
        /// <summary>
        /// Verifies that two values are equal (using == operator). If they are not equal 
        /// an AssertionException is thrown.
        /// </summary>        
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to be displayed when two values are same.</param>        
    
        if (expected != actual)
            Assert._fail.comparison(expected, '!=', actual, message);
    },
    
    areNotEqual : function(expected, actual, message) {
        /// <summary>
        /// Verifies that two objects are not equal (using != operator). If they are equal
        /// an AssertionException is thrown.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to be displayed when the two values are equal.</param>
    
        if (expected == actual)
            Assert._fail.comparison(expected, '==', actual, message);
    },
        
    isFalse : function(value) {
        if (value)
            Assert._fail.is(value, true);
    },

    isTrue : function(value) {
        if (!value)
            Assert._fail.is(value, false);
    },

    isDefined : function(value) {
        if (value === undefined)
            Assert._fail.is(value, undefined);
    },
    
    isNotNaN : function(value) {
        if (!isFinite(value))
            Assert._fail.is(value, NaN);
    } 
};

}
