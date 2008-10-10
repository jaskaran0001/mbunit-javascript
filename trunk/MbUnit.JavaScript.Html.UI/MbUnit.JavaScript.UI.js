var $ = document.getElementById;
var $new = function(name, options, parent) {
    options = options || {};
    parent = parent || document.body;

    var element = parent.ownerDocument.createElement(name);
    for (var key in options) {
        element[key] = options[key];
    }    
    parent.appendChild(element);
    return element;
};

var MbUnit = { UI : {} };

MbUnit.UI = {
    load : function() {
//        var sandbox = $new("iframe", { src: 'MbUnit.JavaScript.UI.Empty.html' });
//        sandbox.style.display = "none";
//        
//        this.sandbox = sandbox.contentDocument;
        
        this.sandbox = window;
        this.tree = document.getElementById('tree');
        
        var that = this;
        this._loadTests(function() { that._findTests(); });
    },
    
    _loadTests : function(finished) {
        var that = this;
        this._loadScript("MbUnit.JavaScript.js", function() {
            that.runner = new that.sandbox.MbUnit.Core.Runner();
        
            var query = /\?scripts=(.+)$/.exec(window.location.href);
            if (query)        
                that._loadScript(query[1], finished);
            else
                finished();
        });
    },
    
    _loadScript : function(src, whenloaded) {
        var script = $new("script", { 
            src:    src,
            type:  'text/javascript',
            onload: whenloaded
        }, this.sandbox.document.body);
    },
    
    _findTests : function() {
        var fixtures = this.runner.load();
        for (var i = 0; i < fixtures.length; i++) {
            var li = $new('li', {}, this.tree);
            li.innerHtml = fixtures[i].name;
        }
    }
};

window.onload = function() { MbUnit.UI.load(); }