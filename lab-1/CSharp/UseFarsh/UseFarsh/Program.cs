using Farsh;

namespace UseFarsh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Types.OptionalField integerField = Types.OptionalField.NewOptionalInteger(1);
            Types.OptionalField stringField = Types.OptionalField.NewOptionalString("Hello");
            
            Console.WriteLine(integerField);
            Console.WriteLine(stringField);
        }
    }
}