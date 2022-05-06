using System;

namespace ConsoleApplication1
{
    class TestClass
    {
        public static void PrintNotNull(string str)
        {
            if (str == null)
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