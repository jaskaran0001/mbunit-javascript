/// <reference assembly="MbUnit.JavaScript" name="MbUnit.JavaScript.Framework.js" />

with (MbUnit.Core) {

var FormatterTest = TestFixture({
    testToString : RowTest(
        Row(1,                      "1"),
        Row("x",                    '"x"'),
        Row(true,                   "true"),
        Row(null,                   "null"),
        Row(undefined,              "undefined"),
        Row(NaN,                    'NaN'),
        Row(function() {},          'function() {}'),
        Row([],                     "[]"),        
        Row([1, 2],                 "[1, 2]"),
        Row({name: 'test'},         '{ name: "test" }'),
        Row({},                     "{}"),
        Row({ test: function() {}}, '{ test: function() {} }'),
        Row({ test: [1,2,{}]},       '{ test: [1, 2, {}] }'),
        function(value, expected) {
            var string = Formatter.toString(value);
            Assert.areEqual(expected, string);            
        }
    )
});

}