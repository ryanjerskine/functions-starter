using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class ServiceProviderHolder
    {
        private readonly ConcurrentDictionary<Guid, IServiceScope> _Scopes = new ConcurrentDictionary<Guid, IServiceScope>();
        private readonly IServiceProvider _ServiceProvider;

        public ServiceProviderHolder(IServiceProvider serviceProvider) =>
            this._ServiceProvider = serviceProvider ?? throw new InvalidOperationException("No service provider provided!");

        public void RemoveScope(Guid functionInstanceId)
        {
            if (this._Scopes.TryRemove(functionInstanceId, out var scope))
            {
                scope.Dispose();
            }
        }

        public object GetRequiredService(Guid functionInstanceId, Type serviceType)
        {
            var scopeFactory = _ServiceProvider.GetService<IServiceScopeFactory>();
            if (scopeFactory == null)
            {
                throw new InvalidOperationException("The current service provider does not support scoping!");
            }

            var scope = this._Scopes.GetOrAdd(functionInstanceId, (_) => scopeFactory.CreateScope());
            return scope.ServiceProvider.GetRequiredService(serviceType);
        }
    }
}