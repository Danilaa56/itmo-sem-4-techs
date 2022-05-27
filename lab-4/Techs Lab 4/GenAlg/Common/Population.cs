namespace GenAlg.Common;

public class Population<TGenome, TFitnessCalculator>
    where TGenome : IGenome<TGenome>, new()
    where TFitnessCalculator : IFitnessCalculator<TGenome>, new()
{
    private TFitnessCalculator _fitnessCalculator;
    private TGenome[] _genomes;
    private readonly Random _random;

    private PopulationStats? _stats;
    private TGenome? _best;
    private readonly int _size;

    public Population(Random random, int size)
        : this(random, size, new TFitnessCalculator())
    {
    }

    public Population(Random random, int size, TFitnessCalculator fitnessCalculator)
    {
        _fitnessCalculator = fitnessCalculator;
        _random = random;
        _genomes = new TGenome[size];
        _size = size;
        for (var i = 0; i < size; i++)
            // _genomes.Add(new TGenome());
            _genomes[i] = new TGenome();
    }

    public void SelectionTournament()
    {
        SelectionTournament(_size);
    }

    private void SelectionTournament(int size)
    {
        _stats = null;
        var oldGen = _genomes;
        // _genomes = new List<TGenome>(size);
        _genomes = new TGenome[_size];
        for (var i = 0; i < size; i++)
        {
            int i1 = 0, i2 = 0, i3 = 0;
            while (i1 == i2 || i1 == i3 || i2 == i3)
            {
                i1 = _random.Next(_size);
                i2 = _random.Next(_size);
                i3 = _random.Next(_size);
            }

            var triple = new[]
            {
                oldGen[i1],
                oldGen[i2],
                oldGen[i3]
            };

            _genomes[i] = triple
                .MaxBy(genome => _fitnessCalculator.Calculate(genome))!
                .Clone();
        }
    }

    public void Crossover(double crossoverChance)
    {
        _stats = null;

        // var oldGen = _genomes;
        // _genomes = new List<TGenome>(_genomes.Count);

        for (var i = 1; i < _size; i += 2)
        {
            if (_random.NextDouble() < crossoverChance)
            {
                var children = _genomes[i - 1].Cross( _genomes[i]);
                // _genomes.Add(children.Item1);
                // _genomes.Add(children.Item2);
                _genomes[i - 1] = children.Item1;
                _genomes[i] = children.Item2;
            }
            // else
            // {
            //     _genomes.Add(oldGen[i - 1]);
            //     _genomes.Add(oldGen[i]);
            // }
        }

        // if (oldGen.Count % 2 == 1)
        // {
        //     _genomes.Add(oldGen.Last());
        // }
    }

    public void Mutate(double mutationChance)
    {
        _stats = null;

        // var oldGen = _genomes;
        // _genomes = new List<TGenome>(_genomes.Count);

        for (var i = 0; i < _size; i++)
        {
            // _genomes.Add( ? oldGenome.Mutate() : oldGenome);
            if (_random.NextDouble() < mutationChance)
                _genomes[i] = _genomes[i].Mutate();
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

        _stats = new PopulationStats(maxFitness, fitnessSum / _size, minFitness, _size);
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