using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

using NMock2;

namespace MbUnit.JavaScript.Tests {
    public abstract class MockingTestBase {
        private Mockery mockery;

        public T Mock<T>() {
            return mockery.NewMock<T>();
        }

        public T Mock<T>(Action<T> expect) {
            var mock = Mock<T>();
            expect(mock);
            return mock;
        }

        [SetUp]
        public virtual void SetUp() {
            mockery = new Mockery();
        }

        [TearDown]
        public virtual void TearDown() {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
