using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

using MbUnit.JavaScript.References.Xml;
using NMock2;

namespace MbUnit.JavaScript.Tests.Of.References.Xml {
    [TestFixture]
    [TestsOn(typeof(XmlAllReferencesResolver))]
    public class XmlAllReferencesResolverTest : MockingTestBase {
        [RowTest]
        [Row(true,  false, true)]
        [Row(true,  true,  true)]
        [Row(false, false, false)]
        public void TestCanResolve(bool firstResolverCan, bool secondResolverCan, bool expectAllCan) {
            Action<IXmlReferenceResolver, bool> setupCanGetReferences = (mock, result) => {
                Stub.On(mock)
                    .Method("CanGetReferences")
                    .Will(Return.Value(result));
            };

            var firstResolver = Mock<IXmlReferenceResolver>(mock => setupCanGetReferences(mock, firstResolverCan));
            var secondResolver = Mock<IXmlReferenceResolver>(mock => setupCanGetReferences(mock, secondResolverCan));

            var allResolver = new XmlAllReferencesResolver(firstResolver, secondResolver);

            bool can = allResolver.CanGetReferences(null);

            Assert.AreEqual(expectAllCan, can);
        }
    }
}
