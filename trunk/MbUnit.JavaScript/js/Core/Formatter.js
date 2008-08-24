MbUnit = MbUnit || {};
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