using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class InjectConfiguration : IExtensionConfigProvider
    {
        public readonly InjectBindingProvider _InjectBindingProvider;

        public InjectConfiguration(InjectBindingProvider injectBindingProvider) =>
            this._InjectBindingProvider = injectBindingProvider ?? throw new ArgumentNullException(nameof(injectBindingProvider));

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<InjectAttribute>()
                .Bind(this._InjectBindingProvider);
    }
}