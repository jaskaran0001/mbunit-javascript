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

// This code emulates the browser, allowing frameworks such as JQuery to be loaded into test context
window = this;
(function() {
    var doNothing = function() {}; 
    var returnObject = function() { return {}; };
    var returnArray = function() { return []; };
       
    var element = {
        appendChild : doNothing,
        insertBefore : doNothing,
        removeChild : doNothing,
        
        getElementsByTagName : returnArray,
        
        style : { display : {} }
    };
    var returnElement = function() { return element; };
        
    // prototype.js sniffing
    window.attachEvent = doNothing;
    
    window.document = {
        write           : doNothing,
        getElementById  : returnElement,
        createElement   : returnElement,
        createComment   : returnObject,
        createTextNode  : returnElement,
        
        documentElement : element
    };
})();

window.navigator = { userAgent : 'MbUnit.JavaScript Emulation (IE engine)' };
window.location = {};

// Very cryptic, jQuery requires this for isFunction, for (fn + "") to work when fn is window.
window.toString = function() { return "[object]"; };