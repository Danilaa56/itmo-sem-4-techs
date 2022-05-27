using System.Collections.Immutable;
using GenAlg.Common;

namespace GenAlg.PushThePoint;

public class Genome : IGenome<Genome>
{
    public ImmutableArray<GenomeAction> ActionsSequence;
    const int GenesLength = 200;

    public Genome()
    {
        var array = new GenomeAction[GenesLength];
        for (var i = 0; i < GenesLength; i++)
        {
            array[i] = (GenomeAction) Program.Random.Next(5);
        }

        ActionsSequence = array.ToImmutableArray();
    }
    
    public Genome(ImmutableArray<GenomeAction> actions)
    {
        ActionsSequence = actions;
    }

    public Genome Clone()
    {
        return this;
    }

    public (Genome, Genome) Cross(Genome other)
    {
        var s = Program.Random.Next(2, GenesLength - 3);
        var ar1 = new GenomeAction[GenesLength];
        var ar2 = new GenomeAction[GenesLength];
        for (var i = 0; i < GenesLength; i++)
        {
            if (i < s)
            {
                ar1[i] = ActionsSequence[i];
                ar2[i] = other.ActionsSequence[i];
            }
            else
            {
                ar1[i] = other.ActionsSequence[i];
                ar2[i] = ActionsSequence[i];
            }
        }

        return (new Genome(ar1.ToImmutableArray()), new Genome(ar2.ToImmutableArray()));
    }

    public Genome Mutate()
    {
        var ar = ActionsSequence.ToArray();

        for (var i = 0; i < 5; i++)
        {
            var index = Program.Random.Next(0, GenesLength);
            ar[index] = (GenomeAction) Program.Random.Next(5);            
        }
        
        return new Genome(ar.ToImmutableArray());
    }

    public void Dispose()
    {
    }
}