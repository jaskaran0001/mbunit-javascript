using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft.Threading {
    internal class MtaThreadOnly : IThreadingRequirement {
        public void InvokeAsRequired(Action action) {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA) {
                action();
                return;
            }

            var result = action.BeginInvoke(ar => { }, null);
            result.AsyncWaitHandle.WaitOne();
            action.EndInvoke(result);
        }
    }
}
