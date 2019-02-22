using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Threading.Tasks;

namespace FunctionsStarter.Infrastructure.IoC
{
    internal class InjectBinding : IBinding
    {
        private readonly Type _Type;
        private readonly ServiceProviderHolder _ServiceProviderHolder;

        internal InjectBinding(ServiceProviderHolder serviceProviderHolder, Type type)
        {
            this._Type = type ?? throw new ArgumentNullException(nameof(type));
            this._ServiceProviderHolder = serviceProviderHolder ?? throw new ArgumentNullException(nameof(serviceProviderHolder));
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) =>
            Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var value = this._ServiceProviderHolder.GetRequiredService(context.FunctionInstanceId, this._Type);
            return this.BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

        private class InjectValueProvider : IValueProvider
        {
            private readonly object _Value;

            public InjectValueProvider(object value) => this._Value = value ?? throw new ArgumentNullException(nameof(value));

            public Type Type => this._Value.GetType();

            public Task<object> GetValueAsync() => Task.FromResult(this._Value);

            public string ToInvokeString() => this._Value.ToString();
        }
    }
}