using DL.GameOfLife.Domain.Enums;
using DL.GameOfLife.Domain.Extensions;
using DL.GameOfLife.Models;

namespace DL.GameOfLife.Api.ErrorHandling;

public class ErrorResponse
{
    public IEnumerable<ErrorModel> Errors { get; set; } = new List<ErrorModel>();

    public ErrorResponse()
    {
        Errors = new List<ErrorModel>();
    }
    public ErrorResponse(params ErrorModel[] errors)
    {
        Errors = errors;
    }

    public ErrorResponse(ErrorCodes errorCode) : this(errorCode.NewErrorModel())
    {

    }
}
