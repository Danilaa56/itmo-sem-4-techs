using GenAlg.OneMax;

namespace GenAlg.Common;

public class GenAlgOptimizer
{
    private int _maxGenerations;
    private double _crossoverP;
    private double _mutatationP;

    public GenAlgOptimizer(int maxGenerations, double crossoverP, double mutatationP)
    {
        _maxGenerations = maxGenerations;
        _crossoverP = crossoverP;
        _mutatationP = mutatationP;
    }
    
    public List<GenAlgStats> FindSolution<TGenome, TFitnessCalculator>(Random random, int populationSize)
        where TGenome : IGenome<TGenome>, new()
        where TFitnessCalculator : IFitnessCalculator<TGenome>, new()
    {
        var stats = new List<GenAlgStats>();
        
        var population = new Population<TGenome, TFitnessCalculator>(random, populationSize);
        var maxPossibleFitness = population.MaxPossibleFitness;

        var generation = 0;
        while (
            population.GetStats().MaxFitness < maxPossibleFitness &&
            generation < _maxGenerations)
        {
            generation++;

            population.SelectionTournament();
            population.Crossover(_crossoverP);
            population.Mutate(_mutatationP);

            var populationStats = population.GetStats();

            stats.Add(new GenAlgStats(generation, populationStats));
        }

        return stats;
    }
}