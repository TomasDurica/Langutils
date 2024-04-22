using BenchmarkDotNet.Attributes;
using Langutils.Core.Options;
using Langutils.Core.Results;

namespace Langutils.Benchmarks;

public class TryBenchmarks
{
    [Benchmark]
    public void Try_Native_GenericException_Success()
    {
        try
        {
            Success();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [Benchmark]
    public void Try_Native_GenericException_Fail()
    {
        try
        {
            Failure();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [Benchmark]
    public void Try_Native_ConcreteException_Success()
    {
        try
        {
            Success();
        }
        catch (ConcreteException)
        {
            // ignored
        }
    }

    [Benchmark]
    public void Try_Native_ConcreteException_Fail()
    {
        try
        {
            Failure();
        }
        catch (ConcreteException)
        {
            // ignored
        }
    }

    [Benchmark]
    public void Try_Langutils_GenericException_Success()
    {
        Result.Try(() => Success());
    }

    [Benchmark]
    public void Try_Langutils_GenericException_Fail()
    {
        Result.Try(() => Failure());
    }

    [Benchmark]
    public void Try_Langutils_ConcreteException_Success()
    {
        Result.Try<int, ConcreteException>(() => Success());
    }

    [Benchmark]
    public void Try_Langutils_ConcreteException_Fail()
    {
        Result.Try<int, ConcreteException>(() => Failure());
    }

    [Benchmark]
    public async Task TryAsync_Native_GenericException_Success()
    {
        try
        {
            await SuccessAsync();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task TryAsync_Native_GenericException_Fail()
    {
        try
        {
            await FailureAsync();
        }
        catch (ConcreteException)
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task TryAsync_Native_ConcreteException_Success()
    {
        try
        {
            await SuccessAsync();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task TryAsync_Native_ConcreteException_Fail()
    {
        try
        {
            await FailureAsync();
        }
        catch (ConcreteException)
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task TryAsync_Langutils_GenericException_Success()
    {
        await Result.TryAsync(() => SuccessAsync());
    }

    [Benchmark]
    public async Task TryAsync_Langutils_GenericException_Fail()
    {
        await Result.TryAsync(() => FailureAsync());
    }

    [Benchmark]
    public async Task TryAsync_Langutils_ConcreteException_Success()
    {
        await Result.TryAsync<int, ConcreteException>(() => SuccessAsync());
    }

    [Benchmark]
    public async Task TryAsync_Langutils_ConcreteException_Fail()
    {
        await Result.TryAsync<int, ConcreteException>(() => FailureAsync());
    }

    private int Success()
    {
        return 0;
    }

    private Task<int> SuccessAsync()
    {
        return Task.Run(() => 0);
    }

    private int Failure()
    {
        throw new ConcreteException();
    }

    private Task<int> FailureAsync()
    {
        throw new ConcreteException();
    }

    private class ConcreteException : Exception;
}