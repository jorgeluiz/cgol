using System;

namespace DL.GameOfLife.Models;

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

    public static ErrorModel New(string? code, string? message)
    {
        return new ErrorModel { Code = code, Message = message };
    }
}
