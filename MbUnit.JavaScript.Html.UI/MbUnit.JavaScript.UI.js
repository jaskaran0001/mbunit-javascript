﻿/// <reference path="external\jquery-1.2.6.js" />

// Extensions

var $add = function(name, options, parent) {
    options = options || {};
    parent = $(parent || document.body);

    var element = parent[0].ownerDocument.createElement(name);
    for (var key in options) {
        element[key] = options[key];
    }    
    parent[0].appendChild(element); // .append does not work
    return $(element);
};

// MbUnit.UI

var MbUnit = { UI : {} };

MbUnit.UI = {
    load: function() {
        //var storage = new Persist.Store('TestFiles');        
        var that = this;
        this._setupUIElements();
        
        this.sandboxFrame = $add("iframe", {
            className: 'Hidden',
            src: 'Sandbox.html',
            onload: function(e) {
                that.sandbox = that.sandboxFrame[0].contentWindow;
                that.tree = $('#tree');
                
                that._loadFramework(function() {
                    that._loadTestsFromQuery();
                });
            }
        });
    },
    
    _setupUIElements : function() {
        this._testInput = $('#inputTestPath');
        var loadMethods = {
            standard : function() {
                var path = that._testInput.val();
                that.loadTests(path);                
            },
            
            firefox3 : function() {
                var content = that._testInput[0].files[0].getAsText('utf-8');
                content += '\r\n' + 'window.parent.MbUnit.UI._showTests();';
                
                that._loadScriptFromString(content);                
            }
        };
        
        // Wonderful world of workarounds:
        // http://stackoverflow.com/questions/81180/how-to-get-the-file-path-from-html-input-form-in-firefox-3
        var loadTestsFromInput = this._testInput[0].files ? loadMethods.firefox3 : loadMethods.standard;
        
        var that = this;
        $('#loadTests').click(loadTestsFromInput);
    },
  
    _loadFramework: function(loaded) {
        var that = this;
        this._loadScript("MbUnit.JavaScript.js", function() {
            that.runner = new that.sandbox.MbUnit.Core.Runner();                
            loaded();
        });
    },
    
    _loadTestsFromQuery : function() {
        var query = /\?tests=(.+)$/.exec(window.location.href);
        if (!query)
            return;
        
        var tests = query[1].split(',');
        this.loadTests(tests);
    },
    
    loadTests : function() {
        var paths = [];
        $.each(arguments, function() {
            if (this.length && this.push)
                paths = paths.concat(this);
            else
                paths.push(this);
        });
    
        var yetToLoad = paths.length;
        var that = this;
        
        var loaded = function() {
            yetToLoad -= 1;
            if (yetToLoad === 0)
                that._showTests();                
        };
    
        $.each(paths, function() { that._loadScript(this, loaded); });
    },

    _loadScript: function(src, whenloaded) {
        $add("script", {
            src: src,
            type: 'text/javascript',
            onload: whenloaded
        }, this.sandbox.document.body);
    },
    
    // Firefox 3 workaround
    _loadScriptFromString: function(content, whenloaded) {
        $add("script", {
            type: 'text/javascript',
            textContent: content
        }, this.sandbox.document.body); 
    },

    _showTests: function() {
        var fixtures = this.runner.load();
        
        this.tree.empty();
        this._showTestHierarchy(fixtures, this.tree);
    },

    _showTestHierarchy: function(tests, root) {
        for (var i = 0; i < tests.length; i++) {
            var li = this._showTestNode(root, tests[i]);
            if (tests[i].invokers) {
                var ul = $add('ul', {}, li);
                this._showTestHierarchy(tests[i].invokers, ul);
            }
        }
    },
    
    _showTestNode : function(root, test) {
        var that = this;
    
        var li = $add('li', {}, root);
        test.node = li;
        
        $add('a', { href: "#", innerHTML: test.name }, li)
            .click(function(event) { 
                that._runTest(test);
                event.preventDefault();
            });
        
        return li;           
    },
    
    _runTest : function(test) {
        var that = this;
        
        if (test.invokers) {   
            var passed = true;
            $.each(test.invokers, function(index, test) {
                passed = that._runTest(test) && passed;                
            });
            passed ? this._passTest(test) : this._failTest(test);
                        
            return passed;
        }
    
        try {
            test.execute();
        }
        catch(ex) {
            this._failTest(test, ex);
            return false;
        }
        
        this._passTest(test);
        return true;
    },
    
    _failTest : function(test, ex) {
        test.node
            .removeClass("Passed")
            .addClass("Failed");
        
        if (ex)
            test.node.attr("title", ex.message || ex);
    },
    
    _passTest : function(test) {
        test.node
            .removeClass("Failed")
            .addClass("Passed");
        
        test.node.attr("title", null);   
    }
};

$(function() { MbUnit.UI.load(); });