using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    // can be removed if moving to 3.5

    internal delegate void Action();
    internal delegate void Action<T1, T2>(T1 arg1, T2 arg2);

    internal delegate TResult Func<TResult>();
    internal delegate TResult Func<T, TResult>(T arg);
}
