using BenchmarkDotNet.Attributes;
using Langutils.Core.Options;

namespace Langutils.Benchmarks;

public class PatternBenchmarks
{
    [Benchmark]
    public int Some_PatternMatch()
        => Option.Some(42) is { IsSome: true, Value: var value } ? value : -1;

    [Benchmark]
    public int Some_UnwrapOr()
        => Option.Some(42).UnwrapOr(-1);

    [Benchmark]
    public int Some_TryUnwrap()
        => Option.Some(42).TryUnwrap(out var value) ? value : -1;

    [Benchmark]
    public int None_PatternMatch()
        => Option.None<int>() is { IsSome: true, Value: var value } ? value : -1;

    [Benchmark]
    public int None_UnwrapOr()
        => Option.None<int>().UnwrapOr(-1);

    [Benchmark]
    public int None_TryUnwrap()
        => Option.None<int>().TryUnwrap(out var value) ? value : -1;
}