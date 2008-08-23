using System;
using System.Collections.Generic;
using System.Text;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using MbUnit.JavaScript.Engines.Microsoft.Debug;

namespace MbUnit.JavaScript.Engines.Microsoft.Debug {
    public class ComLogger : IInterceptor {
        private readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public void Intercept(IInvocation invocation) {
            var argumentStrings = Array.ConvertAll(invocation.Arguments, a => a != null ? a.ToString() : "null");
            var argumentString = string.Join(", ", argumentStrings);

            DebugLog.Log("{0}.{1}({2}) ->", invocation.TargetType.Name, invocation.Method.Name, argumentString);
            DebugLog.BeginIndent();

            try {
                invocation.Proceed();
            }
            catch (Exception ex) {
                DebugLog.EndIndent();
                DebugLog.Log("!{0}", ex);
                throw;
            }
            finally {
                DebugLog.EndIndent();
            }

            var parameters = invocation.Method.GetParameters();
            for (int i = 0; i < parameters.Length; i++) {
                if (!parameters[i].IsOut)
                    return;

                var type = parameters[i].ParameterType;
                if (!type.IsInterface && !type.IsImport)
                    continue;

                invocation.Arguments[i] = proxyGenerator.CreateInterfaceProxyWithTarget(type, invocation.Arguments[i], this);
            }

            DebugLog.Log("[{0}]", invocation.ReturnValue);
        }
    }
}