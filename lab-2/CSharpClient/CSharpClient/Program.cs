using CSharpClient.Generated;
using CSharpClient.Generated.Dto;

namespace CSharpClient;

public static class Program
{
    public static void Main()
    {
        var textController = new TextController("http://localhost:8080/");

        var author = new Person("Vasya", "2");
        for (var i = 0; i < 100; i++)
        {
            textController.Add(new Text(Guid.NewGuid(), $"Funniest text #{i}", author, DateTime.Today, DateTime.Today));
        }
        
        Console.WriteLine(textController.Coil().deepNumber.First().First().First().First().First().First().First().First());
    }
}