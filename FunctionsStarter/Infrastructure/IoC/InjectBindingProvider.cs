using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Threading.Tasks;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class InjectBindingProvider : IBindingProvider
    {
        private readonly ServiceProviderHolder _ServiceProviderHolder;

        public InjectBindingProvider(ServiceProviderHolder serviceProviderHolder) =>
            this._ServiceProviderHolder = serviceProviderHolder ?? throw new ArgumentNullException(nameof(serviceProviderHolder));

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new InjectBinding(this._ServiceProviderHolder, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }
    }
}