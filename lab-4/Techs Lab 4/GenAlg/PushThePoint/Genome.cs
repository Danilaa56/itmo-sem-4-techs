using GenAlg.Common;

namespace GenAlg.PushThePoint;

public class Genome : IGenome<Genome>
{
    public GenomeAction[] ActionsSequence;
    const int GenesLength = 200;

    public Genome()
    {
        ActionsSequence = new GenomeAction[GenesLength];
        for (var i = 0; i < GenesLength; i++)
        {
            ActionsSequence[i] = (GenomeAction)Program.Random.Next(5);
        }
    }

    public Genome(GenomeAction[] actions)
    {
        ActionsSequence = actions.ToArray();
    }

    public Genome Clone()
    {
        return new Genome(ActionsSequence);
    }

    public (Genome, Genome) Cross(Genome other)
    {
        var s = Program.Random.Next(2, GenesLength - 3);
        if (s < GenesLength - s)
            for (var i = 0; i < s; i++)
                (ActionsSequence[i], other.ActionsSequence[i]) = (other.ActionsSequence[i], ActionsSequence[i]);
        else
            for (var i = s; i < GenesLength; i++)
                (ActionsSequence[i], other.ActionsSequence[i]) = (other.ActionsSequence[i], ActionsSequence[i]);

        return (this, other);
    }

    public Genome Mutate()
    {
        for (var i = 0; i < 5; i++)
        {
            var index = Program.Random.Next(0, GenesLength);
            ActionsSequence[index] = (GenomeAction)Program.Random.Next(5);
        }

        return this;
    }

    public void Dispose()
    {
        ActionsSequence = null;
    }
}