/// <reference path="../Common.js" />

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

MbUnit.Core = MbUnit.Core || {};

MbUnit.Core.Formatter = {
    toString : function(object) {
        if (object === null)
            return "null";
            
        if (object === undefined)
            return "undefined";
            
        if (typeof object.__MbUnit_Formatter_toString === 'function')
            return object.__MbUnit_Formatter_toString(this);
            
        if (typeof object === 'object')
            return this._objectToString(object);
    
        return object.toString();
    },
    
    _objectToString : function(object) {
        var result = ["{"];       
        for (var key in object) {
            result.push(" ", key, ": ", this.toString(object[key]), ",");
        }
       
        if (result.length > 1) {
            result.pop(); // removing last ','
            result.push(" ");
        }
            
        result.push("}");

        return result.join('');
    }
};

Array.prototype.__MbUnit_Formatter_toString = function(formatter) {
    var result = ['['];
    for (var i = 0; i < this.length; i++) {
        result.push(formatter.toString(this[i]), ", ");
    }
    
    if (this.length > 0)
        result.pop(); // removing last ','
    
    result.push(']');
    
    return result.join('');
};

String.prototype.__MbUnit_Formatter_toString = function() {
    return '"' + this + '"';
};