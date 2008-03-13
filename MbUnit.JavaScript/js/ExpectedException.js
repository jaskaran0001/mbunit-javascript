/// <reference path="Core\Invokers\ExpectedExceptionRunInvoker.js" />

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