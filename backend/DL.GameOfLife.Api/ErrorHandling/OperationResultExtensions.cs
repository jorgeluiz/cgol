using System.Diagnostics.CodeAnalysis;
using DL.GameOfLife.Domain.Common;

namespace DL.GameOfLife.Api.ErrorHandling;
[ExcludeFromCodeCoverage]
public static class OperationResultExtensions
{
    public static ErrorResponse ErrorResponse<T>(this OperationResult<T> result)
    {
        if (result.Errors.Count > 0)
        {
            return new ErrorResponse
            {
                Errors = result.Errors.Select(e => new ErrorModel
                {
                    Code = e.Code,
                    Message = e.Message
                })
            };
        }
        return new ErrorResponse();
    }
}
