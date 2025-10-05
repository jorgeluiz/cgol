using System;

namespace DL.GameOfLife.Domain.Common;

public class ResultError
{
    public string? Code { get; set; } = string.Empty;
    public string? Message { get; set; } = string.Empty;

    public ResultError() { }

    public static ResultError New(string? code, string? message)
    {
        return new ResultError { Code = code, Message = message };
    }
}
