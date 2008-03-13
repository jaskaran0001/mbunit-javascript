using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.Expando;
using System.Text;

using NMock2;

using MbUnit.Framework;
using MbUnit.JavaScript.Engines.Microsoft;
using MbUnit.JavaScript.Engines.Microsoft.Threading;

namespace MbUnit.JavaScript.Tests.Of.Engines.Microsoft {
    [TestFixture]
    public class ComScriptConverterTest {
        [Test]
        public void TestConvertFromScriptHasIdentityMap() {
            var mockery = new Mockery();
            var converter = CreateConverter(mockery);
            var value = mockery.NewMock<IExpando>();

            Stub.On(value).Method("GetProperty");

            var first = converter.ConvertFromScript(value);
            var second = converter.ConvertFromScript(value);

            Assert.AreSame(first, second);
        }

        private ComScriptConverter CreateConverter(Mockery mockery) {
            var threading = mockery.NewMock<IThreadingRequirement>();
            return new ComScriptConverter(threading);
        }
    }
}
