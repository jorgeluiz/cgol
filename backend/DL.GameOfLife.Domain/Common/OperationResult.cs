using DL.GameOfLife.Domain.Enums;

namespace DL.GameOfLife.Domain.Common;

public class OperationResult<T>
{
    public T? Model { get; private set; }
    public bool IsSuccess { get; private set; }
    public List<ResultError> Errors { get; private set; } = new List<ResultError>();
    public ResultTypes ResultType { get; private set; }

    public static OperationResult<T> Ok(T model)
    {
        return new OperationResult<T>
        {
            Model = model,
            IsSuccess = true,
            ResultType = ResultTypes.Success
        };
    }

    public static OperationResult<T> Error(params ResultError[] errors)
    {
        var output = new OperationResult<T>
        {
            Model = default,
            IsSuccess = false,
            ResultType = ResultTypes.Error
        };
        output.Errors.AddRange(errors);
        return output;
    }

    public static OperationResult<T> NotFound(params ResultError[] errors)
    {
        var output = new OperationResult<T>
        {
            Model = default,
            IsSuccess = false,
            ResultType = ResultTypes.NotFound
        };
        output.Errors.AddRange(errors);
        return output;
    }
}
