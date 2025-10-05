using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Common;
using DL.GameOfLife.Domain.Enums;
using DL.GameOfLife.Domain.Extensions;

namespace DL.GameOfLife.Api.ErrorHandling;

[ExcludeFromCodeCoverage]
public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private Stream _originalContetBody = null;
    private MemoryStream _modifiedContentBody = null;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _originalContetBody = context.Response.Body;
            _modifiedContentBody = new MemoryStream();
            context.Response.Body = _modifiedContentBody;

            await next(context);
            await VerifyStatusCodes(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Internal server error | {ex.Message}");
            await HandleExceptionAsync(context);
        }
        finally
        {
            _modifiedContentBody.Position = 0;
            await _modifiedContentBody.CopyToAsync(_originalContetBody);
            context.Response.Body = _originalContetBody;
            _modifiedContentBody.Dispose();
        }
    }

    private async Task VerifyStatusCodes(HttpContext context)
    {
        switch (context.Response.StatusCode)
        {
            case >= 500 and <= 599:
                {
                    var errorResponse = new ErrorResponse(GenerateError(ErrorCodes.ERR_0000));
                    await HandleResponseAsync(context, HttpStatusCode.InternalServerError, errorResponse);
                }
                break;
                 case StatusCodes.Status400BadRequest:
                 case StatusCodes.Status415UnsupportedMediaType:
                {
                    _modifiedContentBody.SetLength(0);
                    var errorResponse = new ErrorResponse(GenerateError(ErrorCodes.ERR_0001));
                    await HandleResponseAsync(context, HttpStatusCode.BadRequest, errorResponse);
                }
                break;
        }
    }

    public static Task HandleExceptionAsync(HttpContext context)
    {
        var errorResponse = new ErrorResponse(GenerateError(ErrorCodes.ERR_0000));
        return HandleResponseAsync(context, HttpStatusCode.InternalServerError, errorResponse);
    }

    private static Task HandleResponseAsync(HttpContext context, HttpStatusCode httpStatusCode, ErrorResponse errorResponse)
    {
        var result = JsonSerializer.Serialize(errorResponse, _jsonSerializerOptions);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)httpStatusCode;

        return context.Response.WriteAsync(result);

    }

    private static ErrorModel GenerateError(ErrorCodes error)
    {
        return new ErrorModel(error.Code(), error.Description());
    }
}
