/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

with (MbUnit.Core) {

var FormatterTest = TestFixture({
    testToString : RowTest(
        Row(1,                        "1"),
        Row("x",                      '"x"'),
        Row(true,                     "true"),
        Row(null,                     "null"),
        Row(undefined,                "undefined"),
        Row([1, 2],                   "[1,2]"),
        Row({name: 'test'},           '{"name":"test"}'),
        //todo:Row(function() {},            'function() {}'), 
        //todo:Row({ test: function() {}},   '{"test":function() {}}'),
        function(value, expected) {
            var string = Formatter.toString(value);
            Assert.areEqual(expected, string);            
        }
    )
});

}