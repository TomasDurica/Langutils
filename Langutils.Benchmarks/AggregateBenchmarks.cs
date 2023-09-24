using BenchmarkDotNet.Attributes;
using Langutils.Core.Options;

[MemoryDiagnoser]
public class AggregateBenchmarks
{
    [Benchmark]
    public void Sum_Some()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4) };
        var result = options.Sum();
        if (!result.IsSome || result.Value != 10)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Sum_None()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4) };
        var result = options.Sum();
        if (result.IsSome)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_Some()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4) };
        var result = options.Aggregate(0, (a, b) => a + b);
        if (!result.IsSome || result.Value != 10)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_None()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4) };
        var result = options.Aggregate(0, (a, b) => a + b);
        if (result.IsSome)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_Some_Static()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4) };
        var result = options.Aggregate(0, static (a, b) => a + b);
        if (!result.IsSome || result.Value != 10)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_None_Static()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4) };
        var result = options.Aggregate(0, static (a, b) => a + b);
        if (result.IsSome)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_Some_Pre()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4) };
        var result = options.Aggregate(0, Add);
        if (!result.IsSome || result.Value != 10)
        {
            throw new Exception();
        }
    }

    [Benchmark]
    public void Agg_None_Pre()
    {
        var options = new[] { Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4) };
        var result = options.Aggregate(0, Add);
        if (result.IsSome)
        {
            throw new Exception();
        }
    }

    private static int Add(int a, int b) => a + b;
}