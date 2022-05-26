namespace GenAlg.Common;

public class Population<TGenome, TFitnessCalculator>
    where TGenome : IGenome<TGenome>, new()
    where TFitnessCalculator : IFitnessCalculator<TGenome>, new()
{
    private TFitnessCalculator _fitnessCalculator = new();
    private List<TGenome> _genomes;
    private readonly Random _random;

    private PopulationStats? _stats;
    private TGenome? _best;

    public Population(Random random, int size)
    {
        _random = random;
        _genomes = new List<TGenome>(size);
        for (var i = 0; i < size; i++)
            _genomes.Add(new TGenome());
    }

    public void SelectionTournament()
    {
        SelectionTournament(_genomes.Count);
    }
    
    private void SelectionTournament(int size)
    {
        _stats = null;
        var oldGen = _genomes;
        _genomes = new List<TGenome>(size);
        for (var i = 0; i < size; i++)
        {
            int i1 = 0, i2 = 0, i3 = 0;
            while (i1 == i2 || i1 == i3 || i2 == i3)
            {
                i1 = _random.Next(oldGen.Count);
                i2 = _random.Next(oldGen.Count);
                i3 = _random.Next(oldGen.Count);
            }

            var triple = new[]
            {
                oldGen[i1],
                oldGen[i2],
                oldGen[i3]
            };

            _genomes.Add(triple
                .MaxBy(genome => _fitnessCalculator.Calculate(genome))!
                .Clone());
        }
    }
    
    public void Crossover(double crossoverChance)
    {
        _stats = null;

        var oldGen = _genomes;
        _genomes = new List<TGenome>(_genomes.Count);

        for (var i = 1; i < oldGen.Count; i += 2)
        {
            if (_random.NextDouble() < crossoverChance)
            {
                var children = oldGen[i - 1].Cross(oldGen[i]);
                _genomes.Add(children.Item1);
                _genomes.Add(children.Item2);
            }
            else
            {
                _genomes.Add(oldGen[i - 1]);
                _genomes.Add(oldGen[i]);
            }
        }

        if (oldGen.Count % 2 == 1)
        {
            _genomes.Add(oldGen.Last());
        }
    }

    public void Mutate(double mutationChance)
    {
        _stats = null;

        var oldGen = _genomes;
        _genomes = new List<TGenome>(_genomes.Count);

        foreach (var oldGenome in oldGen)
        {
            _genomes.Add(_random.NextDouble() < mutationChance ? oldGenome.Mutate() : oldGenome);
        }
    }

    public PopulationStats GetStats()
    {
        if (_stats is not null)
            return _stats;
        var maxFitness = double.MinValue;
        var minFitness = double.MaxValue;
        var fitnessSum = 0d;
        foreach (var genome in _genomes)
        {
            var fitness = _fitnessCalculator.Calculate(genome);
            if (maxFitness < fitness)
            {
                maxFitness = fitness;
                _best = genome;
            }

            if (fitness < minFitness)
            {
                minFitness = fitness;
            }

            fitnessSum += fitness;
        }

        _stats = new PopulationStats(maxFitness, fitnessSum / _genomes.Count, minFitness, _genomes.Count);
        return _stats;
    }

    public TGenome GetBest()
    {
        if (_stats is null)
        {
            GetStats();
        }
        return _best!.Clone();
    }

    public double MaxPossibleFitness => _fitnessCalculator.MaxPossibleFitness();
}