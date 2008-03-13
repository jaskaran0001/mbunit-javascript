using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript {
    // can be removed if moving to 3.5

    internal delegate void Action();

    internal delegate TResult Func<TResult>();
    internal delegate TResult Func<T, TResult>(T arg);
}
