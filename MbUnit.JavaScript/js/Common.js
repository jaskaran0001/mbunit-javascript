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

this.window = this.window || this;

Array.prototype.forEach = Array.prototype.forEach || function(iterator, thisObject) {
    thisObject = thisObject || window;
    for (var i = 0; i < this.length; i++) {
        iterator.call(thisObject, this[i], i);
    }
};

Array.prototype.map = Array.prototype.map || function(mapper, thisObject) {
    thisObject = thisObject || window;
    
    var result = [];
    for (var i = 0; i < this.length; i++) {
        result[i] = mapper.call(thisObject, this[i], i);
    }
    
    return result;
};

Array.prototype.filter = Array.prototype.filter || function(filter, thisObject) {
    thisObject = thisObject || window;
    
    var result = [];
    for (var i = 0; i < this.length; i++) {
        var matches = filter.call(thisObject, this[i], i);
        if (matches)
            result.push(this[i]);
    }
    
    return result;
};

Array.prototype.select = Array.prototype.select || function(selector, thisObject) {
    if (typeof selector === 'string') {
        var field = selector;
        selector = function(value) { return value[field]; };
    }

    return this.map(selector, thisObject);
};

Array.prototype.where = Array.prototype.where || Array.prototype.filter;

Object.extend = Object.extend || function(target, source) {
    for (var name in source) {
        target[name] = source[name];
    }
};

InvalidOperationException = window.InvalidOperationException || function() {};

MbUnit = this.MbUnit || {};
