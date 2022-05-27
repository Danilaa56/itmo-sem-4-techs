namespace MyPLINQ.Benchmark;

public class ManualLaunch
{
    public static void Main()
    {
        var strings = new List<string>();
        var r = new Random(56);
        for (var i = 0; i < 10_000; i++)
            strings.Add(BenchmarkClass.RandomString(r, 1_000, 100_000));

        foreach (var str in strings)
        {
            string s;
            
            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);

            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);
            s = BenchmarkClass.Sha256(str);
            // Console.WriteLine(s);
        }

        // _operation = Sha256;
    }
}