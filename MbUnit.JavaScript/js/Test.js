﻿/// <reference path="Common.js" />

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

function Test(attributesThenMethod) {
    /// <param name="attributesThenMethod" mayBeNull="false" optional="false" parameterArray="true" elementType="Object" />
    
    Test.processArguments(arguments);        
    MbUnit.extend(arguments.method, Test.prototype);
    
    return arguments.method;
}

Test.processArguments = function(arguments) {
    /// <param name="arguments" mayBeNull="false" optional="false" type="Arguments" />

    var attributes = MbUnit.extend([]);
    for (var i = 0; i < arguments.length - 1; i++) {
        attributes.push(arguments[i]);
    }
    
    var method = arguments[arguments.length - 1];
    
    arguments.attributes = attributes;
    arguments.method = method;
    
    var decorators = arguments.attributes
                              .where(function(attribute) { return attribute.getRunInvoker; });
    
    arguments.method._decorators = decorators;
}

Test.prototype.applyDecorators = function(invoker) {
    this._decorators.forEach(function(decorator) {
        invoker = decorator.getRunInvoker(invoker);
    });
    return invoker;
},

Test.prototype.getRunInvokers = function(fixture, methodName) {
    var test = this;
    var invoker = {
        name: methodName,
        execute: function() { test.apply(fixture, arguments); }
    };
    invoker = this.applyDecorators(invoker);

    return MbUnit.extend([invoker]);
};