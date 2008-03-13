using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Exceptions;

namespace MbUnit.JavaScript.Tests {
    public delegate void Block();

    public static class ExceptionAssert {
        public static void Throws<TException>(Block block, Action<TException> asserter)
            where TException : Exception
        {
            try {
                block();
            }
            catch (TException ex) {
                asserter(ex);
                return;
            }

            throw new ExceptionNotThrownException(
                typeof(TException), "Expected exception was not thrown."
            );
        }
    }
}
