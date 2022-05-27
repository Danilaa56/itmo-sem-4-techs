using MyPLINQ.Benchmark.Tools;

namespace MyPLINQ.Benchmark.Processor;

public class StringSha512ProcessorExperiment : ProcessorExperiment<string, byte[]>
{
    protected override byte[] Process(string element)
    {
        var s = Utils.Sha512(element);
        s = Utils.Sha512(element);
        s = Utils.Sha512(element);
        s = Utils.Sha512(element);

        return s;
    }

    public StringSha512ProcessorExperiment(string[] inputCollection) : base(inputCollection)
    {
    }
}