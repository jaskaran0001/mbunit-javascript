using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.JavaScript.Tests {
    internal class Disposer : IDisposable {
        private readonly Block dispose;

        public Disposer(Block dispose) {
            this.dispose = dispose;
        }

        public void Dispose() {
            this.dispose();
        }
    }
}
