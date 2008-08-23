using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NMock2;
using NMock2.Monitoring;

namespace MbUnit.JavaScript.Tests.Mocking {
    internal class Be : IAction {
        private Delegate action;

        private Be(Delegate action) {
            this.action = action;
        }

        public static Be A(Delegate action) {
            return new Be(action);
        }

        public static Be A(Action action) {
            return new Be(action);
        }

        public static Be A<T, TResult>(Func<T, TResult> func) {
            return new Be(func);
        }

        #region IInvokable Members

        public void Invoke(Invocation invocation) {
            var count = this.action.Method.GetParameters().Length;
            var parameters = new object[count];

            for (int i = 0; i < count; i++) {
                parameters[i] = invocation.Parameters[i];
            }

            object result = this.action.DynamicInvoke(parameters);
            if (invocation.Method.ReturnType != typeof(void)) {
                invocation.Result = result;
            }
        }

        #endregion

        #region ISelfDescribing Members

        public void DescribeTo(TextWriter writer) {
            writer.Write("call ");
            writer.Write(this.action.Method.Name);
        }

        #endregion
    }
}