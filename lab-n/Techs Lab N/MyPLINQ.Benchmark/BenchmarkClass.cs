using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace MyPLINQ.Benchmark;

[WarmupCount(3)]
[IterationCount(2)]
public class BenchmarkClass
{
    private List<string> _strings;
    private Func<string, string> _operation;

    [GlobalSetup]
    public void Setup()
    {
        _strings = new List<string>();
        var r = new Random(56);
        for (var i = 0; i < 1000; i++)
            _strings.Add(RandomString(r, 10000, 1000000));
        _operation = Sha256;
    }

    public static string RandomString(Random r, int minLength, int maxLength)
    {
        var length = r.Next(minLength, maxLength);
        var chars = new char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = (char)('a' + r.Next(0, 26));
        }

        return new string(chars);
    }
    
    // [Benchmark]
    // public void JustIterate()
    // {
    //     _nums
    //         .ToList();
    // }
    
    [Benchmark]
    public void JustIterate()
    {
        _strings
            .Select(_operation)
            .ToList();
    }
    
    // [Benchmark]
    // public void MultiplyIterate2()
    // {
    //     _nums
    //         .Select(_operation)
    //         .Select(_operation)
    //         .ToList();
    // }
    //
    // [Benchmark]
    // public void MultiplyIterate3()
    // {
    //     _nums
    //         .Select(_operation)
    //         .Select(_operation)
    //         .Select(_operation)
    //         .ToList();
    // }
    
    [Benchmark]
    public void MultiplyIterateParallel()
    {
        _strings
            .AsParallel()
            .Select(_operation)
            .ToList();
    }
    
    // [Benchmark]
    // public void MultiplyIterateParallel2()
    // {
    //     _nums
    //         .AsParallel()
    //         .Select(_operation)
    //         .Select(_operation)
    //         .ToList();
    // }
    //
    // [Benchmark]
    // public void MultiplyIterateParallel3()
    // {
    //     _nums
    //         .AsParallel()
    //         .Select(_operation)
    //         .Select(_operation)
    //         .Select(_operation)
    //         .ToList();
    // }
    
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

    public static string Sha256(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var hash = SHA512.Create();
        
        var hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        //
        // hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        // hashedInputBytes = hash.ComputeHash(bytes);
        
        var hashedInputStringBuilder = new StringBuilder(128);
        foreach (var b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));
        return hashedInputStringBuilder.ToString();
    }
}