using Microsoft.Extensions.DependencyInjection;
using System;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly Action<IServiceCollection> _ConfigureServices;

        public ServiceProviderBuilder(Action<IServiceCollection> configureServices) =>
            this._ConfigureServices = configureServices;

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();
            this._ConfigureServices(services);
            return services.BuildServiceProvider();
        }
    }
}