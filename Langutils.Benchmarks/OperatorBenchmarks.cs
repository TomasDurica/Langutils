using BenchmarkDotNet.Attributes;
using Langutils.Core.Results;

namespace Langutils.Benchmarks;

[MemoryDiagnoser]
public class OperatorBenchmarks
{
    [Benchmark]
    public Result<int, string> ImplicitOperatorSuccess() => 42;

    [Benchmark]
    public Result<int, string> ImplicitOperatorError() => "Error";

    [Benchmark]
    public Result<int, string> ExplicitSuccess() => Result.Success<int, string>(42);

    [Benchmark]
    public Result<int, string> ExplicitError() => Result.Error<int, string>("Error");
}