using System;

namespace ConsoleApplication1
{
    class TESTCLASS
    {
        public static void PrintNotNull(string str)
        {
            if (str == null)
            {
                Console.WriteLine("Str is null");
            }
            if (str is null)
            {
                Console.WriteLine("Str is null");
            }
            else
            {
                Console.WriteLine(str);
            }
        }
    }
}