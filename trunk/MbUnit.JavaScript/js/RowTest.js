/// <reference path="Test.js" />
/// <reference path="Row.js" />
/// <reference path="Core\Formatter.js" />
/// <reference path="Core\Invokers\ExpectedExceptionRunInvoker.js" />

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

function RowTest() {
    Test.processArguments(arguments);  
    
    var method = arguments.method;
    var attributes = arguments.attributes;
    
    var rowAttributes = attributes
                          .where(function(attribute) { return attribute.constructor === Row; });
                          
    rowAttributes.forEach(function(attribute) { attribute.updateNamedArguments(method.length); });
        
    method._rows = rowAttributes.select("row");

    Object.extend(method, RowTest.prototype);
    return method;
}

RowTest.prototype = {
    applyDecorators : Test.prototype.applyDecorators,

    getRunInvokers : function(fixture, methodName) {
        return this._rows
                   .select(function(row) { 
                        return this._getRunInvoker(fixture, methodName, row); 
                    }, this)
                   .select(this.applyDecorators, this);
    },

    _getRunInvoker : function(fixture, methodName, row) {
        var name = methodName + "(";       
        row.forEach(function(value, index) {
            name += Formatter.toString(value);
            if (index < row.length - 1)
                name += ", ";
        });
        name += ")";
    
        var test = this;
        var invoker = {
            name : name,
            execute : function() { test.apply(fixture, row); }
        };
        
        if (row.expectedException)
            invoker = new MbUnit.Core.Invokers.ExpectedExceptionRunInvoker(invoker, row.expectedException);
        
        return invoker;
    }
}

}