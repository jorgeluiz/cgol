using DL.GameOfLife.Domain.Common;
using DL.GameOfLife.Domain.Enums;

namespace DL.GameOfLife.Domain.Extensions;

public static class ErrorCodeExtensions
{
    public static string? Code(this ErrorCodes errorCode)
    {
        return errorCode.ToString();
    }

    public static string? Description(this ErrorCodes errorCode)
    {
        return errorCode.GetDescription();
    }

    public static ResultError NewResultError(this ErrorCodes errorCode)
    {
        return ResultError.New(errorCode.Code(), errorCode.Description());
    }
}
