using BayersHealthcare.Common.ResponseInterceptor;
using FluentValidation;
using MediatR;
using System.Net;

namespace BayersHealthcare.Application
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Count != 0)
                {
                    var responseType = typeof(TResponse);

                    if (responseType.IsGenericType)
                    {
                        var resultType = responseType.GetGenericArguments();
                        var invalidResponseType = typeof(ValidatableResponse<>).MakeGenericType(resultType);

                        var invalidResponse = Activator.CreateInstance(invalidResponseType, "Please fix following errors", failures.Select(e => e.ErrorMessage).ToList(), null, HttpStatusCode.BadRequest);

                        return (TResponse)invalidResponse;
                    }
                }

            }
            return await next();
        }
    }
}