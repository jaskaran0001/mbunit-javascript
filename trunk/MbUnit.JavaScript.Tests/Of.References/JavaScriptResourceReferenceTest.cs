using System;

using MbUnit.Framework;
using MbUnit.JavaScript.References;

namespace MbUnit.JavaScript.Tests.Of.References {
    [TestFixture]
    public class JavaScriptResourceReferenceTest {
        [Test]
        public void TestLoadContentThrowsMeaningfulExceptionWhenResourceWasNotFound() {
            const string ResourceName = "No_Such_Resource";

            var assembly = this.GetType().Assembly;
            var reference = new JavaScriptResourceReference(ResourceName, assembly);

            ExceptionAssert.Throws<ResourceNotFoundException>(
                () => reference.LoadContent(),
                ex => {
                    Assert.AreEqual(assembly, ex.Assembly);
                    Assert.AreEqual(ResourceName, ex.ResourceName);
                }
            );
        }
    }
}
