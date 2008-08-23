/// <reference path="../External/json.js" />

MbUnit.Core = MbUnit.Core || {};
MbUnit.Core.Formatter = {
    toString : function(o) {
        if (o === null)
            return "null";
    
        return Object.toJSONString(o);
    }
};