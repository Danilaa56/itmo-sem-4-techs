using GenAlg.Common;
using GenAlg.OneMax;

namespace GenAlg;

public static class Program
{
    public static readonly Random Random = new(56);

    public const int GenomeLength = 1000;
    private const int PopulationSize = 200;
    private const int MaxGenerations = 50;
    private const double CrossoverP = 0.9;
    private const double MutationP = 0.1;

    public static void Main()
    {
        var genAlg = new GenAlgOptimizer(MaxGenerations, CrossoverP, MutationP);
        var result = genAlg.FindSolution<Genome, FitnessCalculator>(Random, PopulationSize);
        
        result.ForEach(Console.WriteLine);
    }
}