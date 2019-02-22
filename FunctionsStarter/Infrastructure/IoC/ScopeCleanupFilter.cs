using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class ScopeCleanupFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly ServiceProviderHolder _ScopeHolder;

        public ScopeCleanupFilter(ServiceProviderHolder scopeHolder) =>
            this._ScopeHolder = scopeHolder ?? throw new ArgumentNullException(nameof(scopeHolder));

        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            this._ScopeHolder.RemoveScope(exceptionContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            this._ScopeHolder.RemoveScope(executedContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}