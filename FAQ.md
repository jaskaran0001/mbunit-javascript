## General ##
**What is MbUnit.JavaScript?**

It is a javascript unit-testing framework built in .NET.
The main advantage is being able to use MbUnit GUI and console runners to run the tests, so a build process can do it without starting a browser.

**How does it work?**

The project is using Microsoft ActiveScript engine (the same that IE uses internally).
Since the engine can be instructed to run specific code, and all exceptions from engine can be intercepted, it makes the test runing process completely controllable from .NET.

**How is it related to MbUnit?**

MbUnit.JavaScript relies on MbUnit tests runners to run the tests and visualize the results. Basically, it does not work without MbUnit.

**Why not NUnit/xUnit/...?**

I prefer MbUnit over NUnit for the good extensiblity features. As for xUnit.Net, it was too fresh when I started this project. However, in the future it may be possible to create a framework-independent solution.

## Technical ##
**Is it possible to use Firefox engine instead of IE one?**

Yes and no. The engine system is completely pluggable, you can use any engine as long as it correctly implements the IScriptEngine interface. However, the only out-of-the-box engine I have built is IE one.

## Javascript ##
**Is it possible to use the javascript parts of framework without the .NET code?**

Yes. However, right now it does include any kind of UI except for MbUnit one, so you will have to implement it. I am going to implement some kind of HTML UI in the future.

## Alternatives ##
**How does this compare to [QUnit](http://docs.jquery.com/QUnit)?**

For .NET developers, this project has several advantages over QUnit:
  1. JavaScript unit tests can fail the continuous integration build, you can't overlook issues.
  1. The APIs are MbUnit/NUnit APIs, so you have Assert.areEqual() instead of ok().
  1. The APIs themselves are richer, for example, ArrayAssert.areElementsEqual.

**Can I run QUnit tests from MbUnit.JavaScript?**

Not directly, but in javascript you can probably make a wrapper.
I think I will do one at some point.