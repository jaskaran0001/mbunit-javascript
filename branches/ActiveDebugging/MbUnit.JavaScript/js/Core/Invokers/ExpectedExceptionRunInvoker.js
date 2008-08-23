/// <reference path="../../Common.js" />
/// <reference path="../../Assert.js" />

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