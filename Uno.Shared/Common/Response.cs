using System.Net;
using System.Text.Json.Serialization;
using Uno.Shared.Resources;

namespace Uno.Shared.Common;

public class Response
{
    public bool IsSuccess { get; set; } = true;

    [JsonIgnore]
    public bool IsFailure
        => !IsSuccess;

    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public Response(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        if (StatusCode != HttpStatusCode.OK)
            IsSuccess = false;
    }

    public Response(string message)
        => Message = message;

    public static Response Success(string message = ResponseConstants.DefaultSuccessMessage)
        => new(message);

    public static Response Error(
        string errorMessage = ResponseConstants.DefaultErrorMessage,
        HttpStatusCode resultCode = HttpStatusCode.InternalServerError)
        => new(errorMessage, resultCode);
}

public class Response<T> : Response
{
    public T Result { get; set; }

    public Response(T result, string message, HttpStatusCode statusCode)
        : base(message, statusCode)
        => Result = result;

    public Response(T result, string message)
        : base(message)
        => Result = result;

    public static Response<T> Success(T result, string message = ResponseConstants.DefaultSuccessMessage)
        => new(result, message);

    public static Response<T> Error(T result,
        string errorMessage = ResponseConstants.DefaultErrorMessage,
        HttpStatusCode resultCode = HttpStatusCode.InternalServerError)
        => new(result, errorMessage, resultCode);

    public new static Response<T> Error(string errorMessage = ResponseConstants.DefaultErrorMessage,
        HttpStatusCode resultCode = HttpStatusCode.InternalServerError)
        => new(default!, errorMessage, resultCode);

    public static Response<T> Error(string errorMessage = ResponseConstants.DefaultErrorMessage)
        => new(default!, errorMessage);
}