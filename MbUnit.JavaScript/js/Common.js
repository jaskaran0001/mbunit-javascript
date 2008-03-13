this.window = this.window || this;

Array.prototype.forEach = Array.prototype.forEach || function(iterator, thisObject) {
    thisObject = thisObject || window;
    for (var i = 0; i < this.length; i++) {
        iterator.call(thisObject, this[i], i);
    }
};

Array.prototype.map = Array.prototype.map || function(mapper, thisObject) {
    thisObject = thisObject || window;
    
    var result = [];
    for (var i = 0; i < this.length; i++) {
        result[i] = mapper.call(thisObject, this[i], i);
    }
    
    return result;
};

Array.prototype.filter = Array.prototype.filter || function(filter, thisObject) {
    thisObject = thisObject || window;
    
    var result = [];
    for (var i = 0; i < this.length; i++) {
        var matches = filter.call(thisObject, this[i], i);
        if (matches)
            result.push(this[i]);
    }
    
    return result;
};

Array.prototype.select = Array.prototype.select || function(selector, thisObject) {
    if (typeof selector === 'string') {
        var field = selector;
        selector = function(value) { return value[field]; };
    }

    return this.map(selector, thisObject);
};

Array.prototype.where = Array.prototype.where || Array.prototype.filter;

Object.extend = Object.extend || function(target, source) {
    for (var name in source) {
        target[name] = source[name];
    }
};

InvalidOperationException = window.InvalidOperationException || function() {};

MbUnit = this.MbUnit || {};