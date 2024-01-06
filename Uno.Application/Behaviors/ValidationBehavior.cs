using System.Net;

namespace Uno.Application.Behaviors;

/// <summary>
/// Pipie line for validation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    private static TResult GenerateResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
       => (TResult)Activator.CreateInstance(typeof(Response<object>), false, message, statusCode);

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken ct = default)
    {
        if (!_validators.Any())
            return await next();

        ValidationContext<TRequest> validationContext = new(request);

        var validationResults = await Task.WhenAll
        (
            _validators.Select(v => v.ValidateAsync(validationContext, ct))
        );

        var errorList = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(x => x.Errors)
            .ToList();


        if (errorList.Count > 0)
        {
            string message = errorList
                .Select(e => e.ErrorMessage)
                .Aggregate((prev, next) => $"{prev} | {next}");

            if (Enum.TryParse(errorList.First().ErrorCode, out HttpStatusCode statusCode))
                return GenerateResponse(message, statusCode);

            return GenerateResponse(message);
        }

        return await next();
    }
}