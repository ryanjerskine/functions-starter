using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace FunctionsStarter.Infrastructure.Pipelines
{

    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidatorFactory _ValidationFactory;

        public ValidationPipeline(IValidatorFactory validationFactory)
        {
            this._ValidationFactory = validationFactory;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validator = this._ValidationFactory.GetValidator(request.GetType());
            var result = validator?.Validate(request);

            if (result != null && !result.IsValid)
                throw new ValidationException(result.Errors);

            return await next();
        }
    }
}