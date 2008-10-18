/// <reference path="external\jquery-1.2.6.js" />

// Extensions

var $add = function(name, options, parent) {
    options = options || {};
    parent = $(parent || document.body);

    var element = parent[0].ownerDocument.createElement(name);
    for (var key in options) {
        if (key === 'onload' && element.attachEvent) {
            var onload = options[key];
            element.attachEvent("onreadystatechange", function(e) { 
                var target = e.srcElement;
                if(target.readyState == "complete")
                    onload.call(target); 
            });
                
            continue;
        }
    
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
                    that._loadTestsFromLocation();
                });
            }
        });
    },
    
    _setupUIElements : function() {
        this._testInput = $('#inputTestPath');
        
        // Wonderful world of workarounds:
        // http://stackoverflow.com/questions/81180/how-to-get-the-file-path-from-html-input-form-in-firefox-3
        var getFileUrlMethods = {
            simple   : function(input) { return input.value; },            
            firefox3 : function(input) { return input.files[0].getAsDataURL(); }
        };
        
        var input = this._testInput[0];
        var getFileUrl = input.files ? getFileUrlMethods.firefox3 : getFileUrlMethods.simple;
        
        var that = this;
        $('#loadTests').click(function() {
             var path = getFileUrl(input);
             window.location.hash = 'test=' + path;
             that.loadTests(path);
        });
    },
  
    _loadFramework: function(loaded) {
        var that = this;
        this._loadScript("MbUnit.JavaScript.js", function() {
            that.runner = new that.sandbox.MbUnit.Core.Runner();                
            loaded();
        });
    },
    
    _loadTestsFromLocation : function() {
        var test = /test=(.+)$/.exec(window.location.hash);
        if (!test)
            return;
        
        test = test[1];
        this.loadTests(test);
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
        var li = $add('li', {}, root);        
        test.ui = new MbUnit.UI.Test(test, li);
        
        $add('a', { href: "#", innerHTML: test.name }, li)
            .click(function(event) { 
                test.ui.run();
                event.preventDefault();
            });
        
        return li;           
    }
};

// MbUnit.UI.Test

MbUnit.UI.Test = function(test, node) {
    this._test = test;
    this._node = node;
};

MbUnit.UI.Test.prototype = {
    run : function() {
        var that = this;
        var test = this._test;
        
        if (test.invokers)
            return this._runChildren();
    
        try {
            test.execute();
        }
        catch(ex) {
            this._fail(ex);
            return false;
        }
        
        this._pass();
        return true;
    },
    
    _runChildren : function() {
        var test = this._test;
        var passed = true;
        $.each(test.invokers, function(index, test) {
            passed = test.ui.run() && passed;                
        });
        passed ? this._pass() : this._fail();
        
        return passed;
    },
    
    _fail : function(ex) {
        this._node
            .removeClass("Passed")
            .addClass("Failed");
        
        if (ex)
            this._node.attr("title", ex.message || ex);
    },
    
    _pass : function(test) {
        this._node
            .removeClass("Failed")
            .addClass("Passed");
        
        this._node.attr("title", null);   
    }
};

$(function() { MbUnit.UI.load(); });