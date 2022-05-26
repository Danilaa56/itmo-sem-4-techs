using GenAlg.Common;

namespace GenAlg.OneMax;

public class FitnessCalculator : IFitnessCalculator<Genome>
{
    public double Calculate(Genome genome)
    {
        return genome.Genes.Sum();
    }

    public double MaxPossibleFitness()
    {
        return Genome.GenesLength;
    }
}