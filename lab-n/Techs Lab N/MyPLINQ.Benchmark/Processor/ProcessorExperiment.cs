using MyPLINQ.Benchmark.Tools;

namespace MyPLINQ.Benchmark.Processor;

public abstract class ProcessorExperiment<TInput, TOutput>
{
    private TInput[] _inputCollection;

    protected ProcessorExperiment(TInput[] inputCollection)
    {
        _inputCollection = inputCollection;
    }

    protected abstract TOutput Process(TInput element);

    public void RunExperiment()
    {
        Console.WriteLine($"For loop:             {MeasureProcessing(ProcessWithoutLinq)} ms");
        Console.WriteLine($"Sequence LINQ:        {MeasureProcessing(ProcessAsSequence)} ms");
        Console.WriteLine($"AsParallel LINQ:      {MeasureProcessing(ProcessAsParallel)} ms");
        Console.WriteLine($"AsParallelCollection: {MeasureProcessing(ProcessAsParallelCollection)} ms");
    }

    private long MeasureProcessing(Func<TInput[], List<TOutput>> processFunction)
    {
        return Utils.Measure(() => processFunction(_inputCollection));
    }

    private List<TOutput> ProcessWithoutLinq(TInput[] collection)
    {
        var result = new List<TOutput>(collection.Length);
        foreach (var element in collection)
        {
            result.Add(Process(element));
        }

        return result;
    }

    private List<TOutput> ProcessAsSequence(TInput[] collection)
    {
        return collection
            .Select(Process)
            .ToList();
    }

    private List<TOutput> ProcessAsParallel(TInput[] collection)
    {
        return collection
            .AsParallel()
            .Select(Process)
            .ToList();
    }

    private List<TOutput> ProcessAsParallelCollection(TInput[] collection)
    {
        return collection
            .AsParallelCollection()
            .Select(Process)
            .ToList();
    }
}