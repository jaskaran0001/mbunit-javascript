/// <reference path="Common.js" />

function Row(args) {
    /// <param name="args" optional="false" parameterArray="true">Row arguments.</param>   
    var rowValues = [];
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
    /// <param name="expectedRowLength" type="Number" optional="false">
    ///    Expected count of row values before positional arguments start.
    /// </param>
    
    var named = this.row.splice(expectedRowLength, this.row.length - expectedRowLength);
    if (named.length > 1)
        throw new InvalidOperationException();
    
    if (named.length === 0)
        return;
    
    named = named[0];
    
    this.expectedException = named.expectedException;
};