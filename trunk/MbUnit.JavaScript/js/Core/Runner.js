/// <reference path="../Common.js" />

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

        //debugger;
        return fixtures;
    },
    
    _isFixture : function(value) {
        return (value && value.getRunInvokers);
    },
    
    _loadFixture : function(name, type) {        
        return {
            name     : name,
            invokers : type.getRunInvokers()
        };
    }
}