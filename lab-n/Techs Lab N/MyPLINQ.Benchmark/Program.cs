using MyPLINQ.Benchmark.Processor;
using static MyPLINQ.Benchmark.Tools.Utils;

const int stringCount = 10000;
const int minStringLength = 1000;
const int maxStringLength = 100_000;

Console.WriteLine(Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"));
Console.WriteLine("Logical processors count: " + Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));

var strings = RandomStringArray(56, stringCount, minStringLength, maxStringLength);

var stringSha512Processor = new StringSha512ProcessorExperiment(strings);
stringSha512Processor.RunExperiment();