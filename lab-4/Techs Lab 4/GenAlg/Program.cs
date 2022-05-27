using System.Buffers;
using System.Text.Json;
using System.Text.Json.Nodes;
using GenAlg.Common;
using GenAlg.Dto;
using GenAlg.PushThePoint;

namespace GenAlg;

public static class Program
{
    public static readonly Random Random = new(56);

    public const int GenomeLength = 100;
    
    private const int PopulationSize = 1_000;
    private const int MaxGenerations = 500;
    private const double CrossoverP = 0.9;
    private const double MutationP = 0.1;

    public static ArrayPool<GenomeAction> ProgramArrayPool = ArrayPool<GenomeAction>.Create(200, 2048);

    public static void Main(string[] args)
    {
        var configPath = args[0];
        
        var config = JsonNode.Parse(File.ReadAllText(configPath)).Deserialize<Config>()!;
        var fitnessCalculator = new FitnessCalculator(config);
        
        var genAlg = new GenAlgOptimizer(MaxGenerations, CrossoverP, MutationP);
        var result = genAlg.FindSolution<Genome, FitnessCalculator>(Random, PopulationSize, fitnessCalculator);
    }
}