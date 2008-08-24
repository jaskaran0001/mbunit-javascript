/* 
  Copyright (c) 2008-2009, Andrey Shchekin
  All rights reserved.
 
  Redistribution and use in source and binary forms, with or without modification, are permitted provided
  that the following conditions are met:
    * Redistributions of source code must retain the above copyright notice, this list of conditions
      and the following disclaimer.
    
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions
      and the following disclaimer in the documentation and/or other materials provided with the
      distribution.
 
    * Neither the name of the Andrey Shchekin nor the names of his contributors may be used to endorse or 
      promote products derived from this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
*/

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
