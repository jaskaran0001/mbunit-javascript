**Simple test:**
```
var SimpleTest = TestFixture({
    testEverythingWorks : Test(function() {
        // ...
    })
});
```

**Expected exceptions:**
```
var SimpleTest = TestFixture({    
    testSomethingThrows : Test(
        ExpectedException(MyException),
        function() {
            // ...
        }
    )
});
```

**Row test:**
```
var SimpleTest = TestFixture({    
    testToStringWorks : RowTest(
        Row(1,  "1"),
        Row({}, "{}"),
        function(value, expected) {
            // ...
        }
    )
});
```