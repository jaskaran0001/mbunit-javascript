﻿var $ = document.getElementById;
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
        //        var sandbox = $new("iframe", { src: 'MbUnit.JavaScript.UI.Empty.html' });
        //        sandbox.style.display = "none";
        //        
        //        this.sandbox = sandbox.contentDocument;

        this.sandbox = window;
        this.tree = document.getElementById('tree');

        var that = this;
        this._loadTests(function() { that._showTests(); });
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