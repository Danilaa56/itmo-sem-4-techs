namespace CSharpClientGenerator;

public static class Program
{
    public static void Main(string[] args)
    {
        var javaSrc = args[0];
        var destination = args[1];
        var package = args[2];
        
        var builder = new ClientBuilder();
        builder.GenerateFromJavaSource(javaSrc, destination, package);
    }
}