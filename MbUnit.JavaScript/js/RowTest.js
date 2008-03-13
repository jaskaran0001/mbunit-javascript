/// <reference path="Test.js" />
/// <reference path="Core\Invokers\ExpectedExceptionRunInvoker.js" />

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
    getRunInvokers : function(methodName) {
        return this._rows
                   .select(function(row) { 
                        return this._getRunInvoker(methodName, row); 
                    }, this);
    },

    _getRunInvoker : function(methodName, row) {
        var name = methodName + "(";       
        row.forEach(function(value, index) {
            name += value.toString();
            if (index < row.length - 1)
                name += ", ";
        });
        name += ")";
    
        var test = this;
        var invoker = {
            name : name,
            execute : function() { test.apply(this, row); }
        };
        
        if (row.expectedException)
            invoker = new MbUnit.Core.Invokers.ExpectedExceptionRunInvoker(invoker, row.expectedException);
        
        return invoker;
    }
}