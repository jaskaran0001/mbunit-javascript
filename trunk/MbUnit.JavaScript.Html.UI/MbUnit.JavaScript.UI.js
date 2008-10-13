/// <reference path="external\persist.js" />
/// <reference path="external\jquery-1.2.6.js" />
/// <reference path="external\jquery.elastic.js" />

var $add = function(name, options, parent) {
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
    load: function() {
        //var storage = new Persist.Store('TestFiles');
        $('textarea').elastic();
        
        var that = this;
        
        this.sandboxFrame = $add("iframe", {
            className: 'Hidden',
            src: 'Sandbox.html',
            onload: function(e) {
                that.sandbox = that.sandboxFrame.contentWindow;
                that.tree = $('#tree')[0];
                
                that._loadTests(function() { that._showTests(); });
            }
        });
    },

    _loadTests: function(finished) {
        var that = this;
        this._loadScript("MbUnit.JavaScript.js", function() {
            that.runner = new that.sandbox.MbUnit.Core.Runner();

            var query = /\?tests=(.+)$/.exec(window.location.href);
            if (query)
                that._loadScript(query[1], finished);
            else
                finished();
        });
    },

    _loadScript: function(src, whenloaded) {
        var script = $add("script", {
            src: src,
            type: 'text/javascript',
            onload: whenloaded
        }, this.sandbox.document.body);
    },

    _showTests: function() {
        var fixtures = this.runner.load();
        this._showTestHierarchy(fixtures, this.tree);
    },

    _showTestHierarchy: function(tests, root) {
        for (var i = 0; i < tests.length; i++) {
            var li = $add('li', { className: "Succeeded" }, root);
            var span = $add('a', { href: "#", innerHTML: tests[i].name }, li);

            if (tests[i].invokers) {
                var ul = $add('ul', {}, li);
                this._showTestHierarchy(tests[i].invokers, ul);
            }
        }
    }
};

window.onload = function() { MbUnit.UI.load(); }