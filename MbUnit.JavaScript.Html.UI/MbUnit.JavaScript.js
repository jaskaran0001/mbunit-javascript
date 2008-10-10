// MbUnit.JavaScript.js.Common.js
if (!this.window) window = this;
MbUnit = this.MbUnit || {};

MbUnit.extend = function(target, source) {
    if (!source) {  
        if (target instanceof Array) {
            this.extend(target, MbUnit.Extensions.Array);
        }
        return target;
    }
    
    for (var key in source) {
        if (target[key])
            continue;

        target[key] = source[key];
    }
    return target;
};

MbUnit.Extensions = {};
MbUnit.Extensions.Array = {
    forEach : function(iterator, thisObject) {
        thisObject = thisObject || window;
        for (var i = 0; i < this.length; i++) {
            iterator.call(thisObject, this[i], i);
        }
        
        return thisObject;
    },

    map : function(mapper, thisObject) {
        thisObject = thisObject || window;
        
        var result = [];
        for (var i = 0; i < this.length; i++) {
            result[i] = mapper.call(thisObject, this[i], i);
        }
        
        return result;
    },

    filter : function(filter, thisObject) {
        thisObject = thisObject || window;
        
        var result = [];
        for (var i = 0; i < this.length; i++) {
            var matches = filter.call(thisObject, this[i], i);
            if (matches)
                result.push(this[i]);
        }
        
        return result;
    },

    select : function(selector, thisObject) {
        if (typeof selector === 'string') {
            var field = selector;
            selector = function(value) { return value[field]; };
        }

        return MbUnit.extend(this.map(selector, thisObject));
    },

    indexOf : function(value) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] === value)
                return i;
        }
        
        return -1;
    },

    remove : function(value) {
        var index = this.indexOf(value);
        if (index < 0)
            return;
        
        this.splice(index, 1);
    },
    
    where : function(condition, thisObject) {
        var result = this.filter(condition, thisObject);
        return MbUnit.extend(result);
    }
};

InvalidOperationException = window.InvalidOperationException || function() {};
// MbUnit.JavaScript.js.Core.Formatter.js
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
            result.pop();             result.push(" ");
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
        result.pop();     
    result.push(']');
    
    return result.join('');
};

String.prototype.__MbUnit_Formatter_toString = function() {
    return '"' + this + '"';
};
// MbUnit.JavaScript.js.Assert.js
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
                                                                throw new AssertionFailureException(message);
    },    
    
    _combineMessage : function(message, expectedMessage) {
        return message ? (message + " " + expectedMessage) : expectedMessage;
    },
      
    areSame : function(expected, actual, message) {
                                                            
        if (expected !== actual)
            Assert._fail.comparison(expected, '!==', actual, message);
    },
    
    areNotSame : function(expected, actual, message) {
                                                            
        if (expected !== actual)
            Assert._fail.comparison(expected, '===', actual, message);
    },    
    
    areEqual : function(expected, actual, message) {
                                                            
        if (expected != actual)
            Assert._fail.comparison(expected, '!=', actual, message);
    },
    
    areNotEqual : function(expected, actual, message) {
                                                            
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
// MbUnit.JavaScript.js.ArrayAssert.js
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
    
        var notFoundInExpected = MbUnit.extend(actual.concat([]));
        var notFoundInActual   = MbUnit.extend(expected.concat([]));

        for (var i = 0; i < expected.length; i++) {
            var expectedValue = expected[i];
            var found = false;

            for (var j = 0; j < actual.length; j++) {
                var actualValue = actual[j];
                if (expectedValue === actualValue) {
                    notFoundInActual.remove(actualValue);
                    found = true;
                    break;
                }
            }

            if (found)
                notFoundInExpected.remove(expectedValue);
        }

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
// MbUnit.JavaScript.js.Test.js
function Test(attributesThenMethod) {
        
    Test.processArguments(arguments);        
    MbUnit.extend(arguments.method, Test.prototype);
    
    return arguments.method;
}

Test.processArguments = function(arguments) {
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
// MbUnit.JavaScript.js.Row.js
function Row(args) {
        var rowValues = MbUnit.extend([]);
    for (var i = 0; i < arguments.length; i++) {
        rowValues[i] = arguments[i];
    }
    
    var that = this;
    if (that === window)
        that = new Row();

    that.row = rowValues;    
    return that;
}

Row.prototype.updateNamedArguments = function(expectedRowLength) {
                
    var named = this.row.splice(expectedRowLength, this.row.length - expectedRowLength);
    if (named.length > 1)
        throw new InvalidOperationException();
    
    if (named.length === 0)
        return;
    
    named = named[0];
    
    this.expectedException = named.expectedException;
};
// MbUnit.JavaScript.js.Core.Invokers.ExpectedExceptionRunInvoker.js
MbUnit.Core = MbUnit.Core || {};
MbUnit.Core.Invokers = MbUnit.Core.Invokers || {};

function ExceptionNotThrownException() {}
function ExceptionTypeMismatchException() {}

MbUnit.Core.Invokers.ExpectedExceptionRunInvoker = function(invoker, exceptionType) {
    this._invoker = invoker;
    this.name = invoker.name;
    
    Assert.isDefined(exceptionType);
    this._exceptionType = exceptionType;
}

MbUnit.Core.Invokers.ExpectedExceptionRunInvoker.prototype = {
    execute : function() {
        var result;
        var caught;
        
        try {
            result = this._invoker.execute();
        }
        catch (ex) {
            caught = ex;
        }
        
        if (!caught)
            throw new ExceptionNotThrownException();
        
        if (caught.constructor !== this._exceptionType)
            throw new ExceptionTypeMismatchException();
        
        return result;
    }
}
// MbUnit.JavaScript.js.RowTest.js
with (MbUnit.Core) {

function RowTest() {
    Test.processArguments(arguments);  
    
    var method = arguments.method;
    var attributes = arguments.attributes;
    
    var rowAttributes = attributes
                          .where(function(attribute) { return attribute.constructor === Row; });
                          
    rowAttributes.forEach(function(attribute) { attribute.updateNamedArguments(method.length); });
        
    method._rows = rowAttributes.select("row");

    MbUnit.extend(method, RowTest.prototype);
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
// MbUnit.JavaScript.js.TestFixture.js
function TestFixture(body) {
        MbUnit.extend(body, TestFixture.prototype);
    
    return body;
}

TestFixture.prototype = {
    getRunInvokers : function() {
        var tests = [];
    
        for (var name in this) {
            var value = this[name];
            if (value && value.getRunInvokers) {
                tests = tests.concat(value.getRunInvokers(this, name));
            }
        }
        
        return tests;
    }
}
// MbUnit.JavaScript.js.ExpectedException.js
function ExpectedException(exceptionType) {
    if (this === window)
        return new ExpectedException(exceptionType);

    this._exceptionType = exceptionType;
}

ExpectedException.prototype = {
    getRunInvoker : function(invoker) {
        return new MbUnit.Core.Invokers.ExpectedExceptionRunInvoker(invoker, this._exceptionType);
    }
}
// MbUnit.JavaScript.Framework.js

// MbUnit.JavaScript.js.Common.js
if (!this.window) window = this;
MbUnit = this.MbUnit || {};

MbUnit.extend = function(target, source) {
    if (!source) {  
        if (target instanceof Array) {
            this.extend(target, MbUnit.Extensions.Array);
        }
        return target;
    }
    
    for (var key in source) {
        if (target[key])
            continue;

        target[key] = source[key];
    }
    return target;
};

MbUnit.Extensions = {};
MbUnit.Extensions.Array = {
    forEach : function(iterator, thisObject) {
        thisObject = thisObject || window;
        for (var i = 0; i < this.length; i++) {
            iterator.call(thisObject, this[i], i);
        }
        
        return thisObject;
    },

    map : function(mapper, thisObject) {
        thisObject = thisObject || window;
        
        var result = [];
        for (var i = 0; i < this.length; i++) {
            result[i] = mapper.call(thisObject, this[i], i);
        }
        
        return result;
    },

    filter : function(filter, thisObject) {
        thisObject = thisObject || window;
        
        var result = [];
        for (var i = 0; i < this.length; i++) {
            var matches = filter.call(thisObject, this[i], i);
            if (matches)
                result.push(this[i]);
        }
        
        return result;
    },

    select : function(selector, thisObject) {
        if (typeof selector === 'string') {
            var field = selector;
            selector = function(value) { return value[field]; };
        }

        return MbUnit.extend(this.map(selector, thisObject));
    },

    indexOf : function(value) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] === value)
                return i;
        }
        
        return -1;
    },

    remove : function(value) {
        var index = this.indexOf(value);
        if (index < 0)
            return;
        
        this.splice(index, 1);
    },
    
    where : function(condition, thisObject) {
        var result = this.filter(condition, thisObject);
        return MbUnit.extend(result);
    }
};

InvalidOperationException = window.InvalidOperationException || function() {};
// MbUnit.JavaScript.js.Core.Runner.js
MbUnit.Core = MbUnit.Core || {};

MbUnit._global = this;

MbUnit.Core.Runner = function () {
}

MbUnit.Core.Runner.prototype = {
    load : function() {
        var fixtures = [];
    
        for (var name in window) {
            var value = window[name];
            if (!this._isFixture(value))
                continue;

            var fixture = this._loadFixture(name, value);
            fixtures.push(fixture);
        }

                return fixtures;
    },
    
    _isFixture : function(value) {
        try {
            return (value && value.getRunInvokers);
        }
        catch(ex) {
            return false;
        }
    },
    
    _loadFixture : function(name, type) {        
        return {
            name     : name,
            invokers : type.getRunInvokers()
        };
    }
}
