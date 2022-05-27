using System.Security.Cryptography;
using System.Text;

namespace MyPLINQ.Benchmark.Tools;

public static class Utils
{
    public static byte[] Sha512(byte[] data)
    {
        using var hash = SHA512.Create();
        return hash.ComputeHash(data);
    }
    
    public static byte[] Sha512(string str)
    {
        return Sha512(Encoding.UTF8.GetBytes(str));
    }
    
    public static long Measure<T>(Func<T> task)
    {
        var start = DateTime.Now.Ticks ;
        task.Invoke();
        var finish = DateTime.Now.Ticks;
        return (finish - start) / TimeSpan.TicksPerMillisecond;
    }
    
    public static string RandomString(Random r, int minLength, int maxLength)
    {
        var length = r.Next(minLength, maxLength);
        var chars = new char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = (char)('a' + r.Next(26));
        }

        return new string(chars);
    }

    public static string[] RandomStringArray(int randomSeed, int stringsCount, int minStringLength, int maxStringLength)
    {
        var r = new Random(randomSeed);
        var strings = new string[stringsCount];
        for (var i = 0; i < stringsCount; i++)
        {
            strings[i] = RandomString(r, minStringLength, maxStringLength);
        }

        return strings;
    }
}