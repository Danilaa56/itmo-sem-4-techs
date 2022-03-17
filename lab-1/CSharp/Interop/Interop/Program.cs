using System.Runtime.InteropServices;

namespace Interop
{
    public class Program
    {
        [DllImport("..\\..\\..\\native\\cmake-build-debug\\libNative.dll")]
        public static extern int Sum(int[] array, int length);
        
        public static void Main(string[] args)
        {
            const int n = 100_000_000;
            var numbers = new int[n];

            for (var i = 0; i < n; numbers[i] = i++)
            {
                numbers[i] = i;
            }
            
            Console.WriteLine(Sum(numbers, n));
        }
    }
}