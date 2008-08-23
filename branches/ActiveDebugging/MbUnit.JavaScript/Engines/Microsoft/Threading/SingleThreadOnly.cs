using System;
using System.Collections.Generic;
using System.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft.Threading {
    internal class SingleThreadOnly : IThreadingRequirement, IDisposable {
        private readonly object syncRoot = new object();

        private readonly Thread thread;
        private Action nextAction;
        private Exception lastException;
        private bool disposing = false;

        private readonly AutoResetEvent wakeDispatch = new AutoResetEvent(false);
        private readonly AutoResetEvent actionCompleted = new AutoResetEvent(false);

        public SingleThreadOnly() {
            this.thread = new Thread(this.Dispatch);
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        private void Dispatch() {
            this.wakeDispatch.WaitOne();
            while (!disposing) {
                try {
                    this.nextAction();
                }
                catch (Exception ex) {
                    this.lastException = ex;
                }
                this.actionCompleted.Set();
                this.wakeDispatch.WaitOne();
            }
        }

        public void InvokeAsRequired(Action action) {
            if (Thread.CurrentThread == this.thread) {
                action();
                return;
            }

            lock (this.syncRoot) {
                this.nextAction = action;
                this.wakeDispatch.Set();
                this.actionCompleted.WaitOne();

                this.ThrowLastExceptionIfAny();
            }
        }

        private void ThrowLastExceptionIfAny()
        {
            if (this.lastException == null)
                return;

            var exception = this.lastException;
            this.lastException = null;
            throw exception;
        }

        #region IDisposable Members

        public void Dispose() {
            this.disposing = true;
            this.wakeDispatch.Set();
            this.thread.Join();
        }

        #endregion
    }
}
