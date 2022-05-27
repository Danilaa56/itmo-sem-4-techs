using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MyPLINQ.Benchmark.Tools;

namespace MyPLINQ.Benchmark;

[WarmupCount(3)]
[IterationCount(2)]
public class BenchmarkClass
{
    private List<string> _strings;
    private Func<string, string> _operation;

    public static void Run()
    {
        BenchmarkRunner.Run<BenchmarkClass>();
    }

    [GlobalSetup]
    public void Setup()
    {
        _strings = new List<string>();
        var r = new Random(56);
        for (var i = 0; i < 5000; i++)
            _strings.Add(Utils.RandomString(r, 1_000, 100_000));
        _operation = Sha512_8times;
    }

    [Benchmark]
    public void JustIterate()
    {
        _strings
            .Select(_operation)
            .ToList();
    }

    [Benchmark]
    public void MultiplyIterateParallel()
    {
        _strings
            .AsParallel()
            .Select(_operation)
            .ToList();
    }

    [Benchmark]
    public void MultiplyIterateMyParallel()
    {
        _strings
            .AsParallelCollection()
            .Select(_operation)
            .ToList();
    }

    private int JustReturn(int n)
    {
        return n;
    }

    private int SleepReturn(int n)
    {
        Thread.Sleep(1);
        return n;
    }

    private string Sha512_8times(string str)
    {
        for (var i = 0; i < 8; i++)
            Utils.Sha512(str);

        return str;
    }
}