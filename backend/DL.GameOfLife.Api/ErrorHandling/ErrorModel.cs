using System;

namespace DL.GameOfLife.Api.ErrorHandling;

public class ErrorModel
{
    public string? Code { get; set; } = string.Empty;
    public string? Message { get; set; } = string.Empty;

    public ErrorModel() { }
    public ErrorModel(string? code, string? message)
    {
        Code = code;
        Message = message;
    }
}
