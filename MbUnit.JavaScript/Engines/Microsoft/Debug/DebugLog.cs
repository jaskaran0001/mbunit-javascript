using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    internal static class DebugLog {
        private static int indent;

        public static void Log(string format, params object[] args) {
            format = "<t" + Thread.CurrentThread.ManagedThreadId + "> " + format;
            format = new string(' ', indent) + format;
            File.AppendAllText("C:\\comscript.log", string.Format(format, args) + "\r\n");
        }

        internal static void BeginIndent() {
            indent += 4;
        }

        internal static void EndIndent() {
            indent -= 4;
        }
    }
}