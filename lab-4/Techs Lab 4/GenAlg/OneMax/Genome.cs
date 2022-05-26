using System.Collections.Immutable;
using GenAlg.Common;

namespace GenAlg.OneMax;

public class Genome : IGenome<Genome>
{
    public const int GenesLength = Program.GenomeLength;
    public ImmutableArray<int> Genes { get; }

    public Genome()
    {
        var array = new int[GenesLength];
        for (var i = 0; i < GenesLength; i++)
        {
            array[i] = Program.Random.Next(2);
        }

        Genes = array.ToImmutableArray();
    }

    private Genome(IEnumerable<int> genes)
        : this(genes.ToImmutableArray())
    {
    }

    private Genome(ImmutableArray<int> genes)
    {
        Genes = genes;
    }

    public Genome Mutate()
    {
        var index = Program.Random.Next(0, GenesLength);
        var ar = Genes.ToArray();
        ar[index] = 1 - ar[index];
        return new Genome(ar);
    }

    public Genome Clone()
    {
        return this;
    }

    public (Genome, Genome) Cross(Genome other)
    {
        var s = Program.Random.Next(2, GenesLength - 3);
        var ar1 = new int[GenesLength];
        var ar2 = new int[GenesLength];
        for (var i = 0; i < GenesLength; i++)
        {
            if (i < s)
            {
                ar1[i] = Genes[i];
                ar2[i] = other.Genes[i];
            }
            else
            {
                ar1[i] = other.Genes[i];
                ar2[i] = Genes[i];
            }
        }

        return (new Genome(ar1.ToImmutableArray()), new Genome(ar2.ToImmutableArray()));
    }
}