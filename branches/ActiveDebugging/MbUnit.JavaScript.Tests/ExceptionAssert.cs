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
            catch (Exception ex) {
                throw new ExceptionTypeMistmachException(typeof(TException), "Exception type mismatch.", ex);
            }

            throw new ExceptionNotThrownException(
                typeof(TException), "Expected exception was not thrown."
            );
        }
    }
}
