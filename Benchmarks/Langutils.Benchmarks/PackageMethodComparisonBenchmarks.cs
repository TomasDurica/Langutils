using BenchmarkDotNet.Attributes;
using CSharpFunctionalExtensions;
using Functional;
using Langutils.Core.Results;

namespace Langutils.Benchmarks;

[MemoryDiagnoser(false)]
public class PackageMethodComparisonBenchmarks
{
    private static readonly object Value = new();
    private static readonly object OtherValue = new();
    private static readonly object Error = new();

    [Benchmark]
    public void Langutils_Result_Success_Map()
    {
        var result = Core.Results.Result.Success<object, object>(Value)
            .Map(_ => OtherValue);

        if (!result.IsSuccess)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Langutils_Result_Error_Map()
    {
        var result = Core.Results.Result.Error<object, object>(Error)
            .Map(_ => OtherValue);

        if (!result.IsError)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Langutils_Result_Success_AndThen_Success()
    {
        var result = Core.Results.Result.Success<object, object>(Value)
            .AndThen(_ => Core.Results.Result.Success<object, object>(Error));

        if (!result.IsSuccess)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Langutils_Result_Success_AndThen_Error()
    {
        var result = Core.Results.Result.Success<object, object>(Value)
            .AndThen(_ => Core.Results.Result.Error<object, object>(Error));

        if (!result.IsError)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Langutils_Result_Error_AndThen()
    {
        var result = Core.Results.Result.Error<object, object>(Error)
            .AndThen(_ => Core.Results.Result.Success<object, object>(Error));

        if (!result.IsError)
        {
            throw new Exception();
        }
    }


    [Benchmark]
    public void LanguageExt_Result_Success_Map()
    {
        var result = LanguageExt.Prelude.Right<object, object>(Value)
            .Map(_ => OtherValue);

        if (!result.IsRight)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void LanguageExt_Result_Error_Map()
    {
        var result = LanguageExt.Prelude.Left<object, object>(Error)
            .Map(_ => OtherValue);

        if (!result.IsLeft)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void LanguageExt_Result_Success_AndThen_Success()
    {
        var result = LanguageExt.Prelude.Right<object, object>(Value)
            .Bind(_ => LanguageExt.Prelude.Right<object, object>(Value));

        if (!result.IsRight)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void LanguageExt_Result_Success_AndThen_Error()
    {
        var result = LanguageExt.Prelude.Right<object, object>(Value)
            .Bind(_ => LanguageExt.Prelude.Left<object, object>(Error));

        if (!result.IsLeft)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void LanguageExt_Result_Error_AndThen()
    {
        var result = LanguageExt.Prelude.Left<object, object>(Value)
            .Bind(_ => LanguageExt.Prelude.Right<object, object>(Value));

        if (!result.IsLeft)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void FunctionalExt_Result_Success_Map()
    {
        var result = CSharpFunctionalExtensions.Result.Success<object, object>(Value)
            .Map(_ => OtherValue);

        if (!result.IsSuccess)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void FunctionalExt_Result_Error_Map()
    {
        var result = CSharpFunctionalExtensions.Result.Failure<object, object>(Error)
            .Map(_ => OtherValue);

        if (!result.IsFailure)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void FunctionalExt_Result_Success_AndThen_Success()
    {
        var result = CSharpFunctionalExtensions.Result.Success<object, object>(Value)
            .Bind(_ => CSharpFunctionalExtensions.Result.Success<object, object>(Error));

        if (!result.IsSuccess)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void FunctionalExt_Result_Success_AndThen_Error()
    {
        var result = CSharpFunctionalExtensions.Result.Success<object, object>(Value)
            .Bind(_ => CSharpFunctionalExtensions.Result.Failure<object, object>(Error));

        if (!result.IsFailure)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void FunctionalExt_Result_Error_AndThen()
    {
        var result = CSharpFunctionalExtensions.Result.Failure<object, object>(Error)
            .Bind(_ => CSharpFunctionalExtensions.Result.Success<object, object>(Error));

        if (!result.IsFailure)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Oxide_Result_Success_Map()
    {
        var result = Oxide.Results.Ok<object, object>(Value)
            .Map(_ => OtherValue);

        if (!result.IsOk)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Oxide_Result_Error_Map()
    {
        var result = Oxide.Results.Err<object, object>(Error)
            .Map(_ => OtherValue);

        if (!result.IsError)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Oxide_Result_Success_AndThen_Success()
    {
        var result = Oxide.Results.Ok<object, object>(Value)
            .AndThen(_ => Oxide.Results.Ok<object, object>(Error));

        if (!result.IsOk)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Oxide_Result_Success_AndThen_Error()
    {
        var result = Oxide.Results.Ok<object, object>(Value)
            .AndThen(_ => Oxide.Results.Err<object, object>(Error));

        if (!result.IsError)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Oxide_Result_Error_AndThen()
    {
        var result = Oxide.Results.Err<object, object>(Error)
            .AndThen(_ => Oxide.Results.Ok<object, object>(Error));

        if (!result.IsError)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Functional_Result_Success_Map()
    {
        var result = Functional.Result.Success<object, object>(Value)
            .Map(_ => OtherValue);

        if (!result.IsSuccess())
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Functional_Result_Error_Map()
    {
        var result = Functional.Result.Failure<object, object>(Error)
            .Map(_ => OtherValue);

        if (result.IsSuccess())
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Functional_Result_Success_AndThen_Success()
    {
        var result = Functional.Result.Success<object, object>(Value)
            .Bind(_ => Functional.Result.Success<object, object>(Error));

        if (!result.IsSuccess())
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Functional_Result_Success_AndThen_Error()
    {
        var result = Functional.Result.Success<object, object>(Value)
            .Bind(_ => Functional.Result.Failure<object, object>(Error));

        if (result.IsSuccess())
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Functional_Result_Error_AndThen()
    {
        var result = Functional.Result.Failure<object, object>(Error)
            .Bind(_ => Functional.Result.Success<object, object>(Error));

        if (result.IsSuccess())
        {
            throw new Exception();
        }
    }
}