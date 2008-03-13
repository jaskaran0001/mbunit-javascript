/// <reference path="Common.js" />

function Test(attributesThenMethod) {
    /// <param name="attributesThenMethod" mayBeNull="false" optional="false" parameterArray="true" elementType="Object" />
    
    Test.processArguments(arguments);
    
    var decorators = arguments.attributes
                              .where(function(attribute) { return attribute.getRunInvoker; });
        
    Object.extend(arguments.method, Test.prototype);
    arguments.method._decorators = decorators;
    
    return arguments.method;
}

Test.processArguments = function(arguments) {
    /// <param name="arguments" mayBeNull="false" optional="false" type="Arguments" />

    var attributes = [];
    for (var i = 0; i < arguments.length - 1; i++) {
        attributes.push(arguments[i]);
    }
    
    var method = arguments[arguments.length - 1];
    
    arguments.attributes = attributes;
    arguments.method = method;
}

Test.prototype.getRunInvokers = function(methodName) {
    var invoker = {
        name : methodName,
        execute : this
    };
    this._decorators.forEach(function(decorator) {
        invoker = decorator.getRunInvoker(invoker);
    });

    return [invoker];
};