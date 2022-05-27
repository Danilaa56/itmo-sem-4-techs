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
        return FindSolution<TGenome, TFitnessCalculator>(random, populationSize, new TFitnessCalculator());
        // var stats = new List<GenAlgStats>();
        //
        // var population = new Population<TGenome, TFitnessCalculator>(random, populationSize);
        // var maxPossibleFitness = population.MaxPossibleFitness;
        //
        // var generation = 0;
        // stats.Add(new GenAlgStats(generation, population.GetStats()));
        // while (
        //     population.GetStats().MaxFitness < maxPossibleFitness &&
        //     generation < _maxGenerations)
        // {
        //     generation++;
        //
        //     population.SelectionTournament();
        //     population.Crossover(_crossoverP);
        //     population.Mutate(_mutatationP);
        //
        //     var populationStats = population.GetStats();
        //
        //     var st = new GenAlgStats(generation, populationStats);
        //     Console.WriteLine(st);
        //     stats.Add(st);
        // }
        //
        // return stats;
    }

    public List<GenAlgStats> FindSolution<TGenome, TFitnessCalculator>(Random random, int populationSize,
        TFitnessCalculator fitnessCalculator)
        where TGenome : IGenome<TGenome>, new()
        where TFitnessCalculator : IFitnessCalculator<TGenome>, new()
    {
        var stats = new List<GenAlgStats>();

        var population = new Population<TGenome, TFitnessCalculator>(random, populationSize, fitnessCalculator);
        var maxPossibleFitness = population.MaxPossibleFitness;

        var generation = 0;
        stats.Add(new GenAlgStats(generation, population.GetStats()));
        while (
            population.GetStats().MaxFitness < maxPossibleFitness &&
            generation < _maxGenerations)
        {
            generation++;

            population.SelectionTournament();
            population.Crossover(_crossoverP);
            population.Mutate(_mutatationP);

            var populationStats = population.GetStats();

            var st = new GenAlgStats(generation, populationStats);
            Console.WriteLine(st);
            stats.Add(st);
        }

        return stats;
    }
}