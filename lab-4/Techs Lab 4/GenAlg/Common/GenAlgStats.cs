namespace GenAlg.Common;

public class GenAlgStats
{
    public int Generation { get; }
    public PopulationStats PopulationStats { get; }

    public GenAlgStats(int generation, PopulationStats populationStats)
    {
        Generation = generation;
        PopulationStats = populationStats;
    }

    public override string ToString()
    {
        return $"Generation {Generation}. Population: {PopulationStats}";
    }
}