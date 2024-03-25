using AuthService.Domain.Models.UserAggregate.ValueObjects;

namespace AuthService.Domain.Shared;

public static class Start
{
    public static Result<T> From<T>()
    {
        return new Result<T>(default, true, Error.None);
    }

    public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result)
    {
        if (result.IsFailure)
            return Result.Failure<TOut>(result.Error);
        
        return new Result<TOut>(default, true, Error.None);
    }

    public static Result<T2> GetResult<T1, T2>(
        this Result<T1> result,
        T2 readyElement)
    {
        if (result.IsFailure)
            return Result.Failure<T2>(result.Error);
        
        return Result.Create<T2>(readyElement);
    }
    
    public static Result<T1> Check<T1, T2>(
        this Result<T1> result,
        Func<T2, Result<T1>> func,
        T2 field,
        out T1 completedField)
    {
        if (result.IsFailure)
        {
            completedField = default!;
            return result;
        }

        var nextResult = func(field);

        if (nextResult.IsSuccess)
        {
            completedField = nextResult.Value;
            return nextResult;
        }
        
        completedField = default!;
        return nextResult;
    }
}