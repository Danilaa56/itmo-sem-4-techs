namespace GenAlg.Common;

public class PopulationStats
{
    public double MaxFitness { get; }
    public double MeanFitness { get; }
    public double MinFitness { get; }
    public int Size { get; }

    public PopulationStats(double maxFitness, double meanFitness, double minFitness, int size)
    {
        MaxFitness = maxFitness;
        MeanFitness = meanFitness;
        MinFitness = minFitness;
        Size = size;
    }

    public override string ToString()
    {
        return $"Fitness: max - {MaxFitness}, mean - {MeanFitness}, min - {MinFitness}; size: {Size}";
    }
}