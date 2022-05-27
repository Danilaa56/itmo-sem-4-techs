using System.Numerics;
using GenAlg.Common;
using GenAlg.Dto;

namespace GenAlg.PushThePoint;

public class FitnessCalculator : IFitnessCalculator<Genome>
{
    private Config _config;

    public FitnessCalculator()
    {
    }

    public FitnessCalculator(Config config)
    {
        _config = config;
    }

    public double Calculate(Genome genome)
    {
        var pos = new Vector2(0, 0);
        var v = new Vector2(0, 0);

        var m = 10;
        var dv = _config.Fmax * _config.dt / m;

        var sum = 0;

        var length = genome.ActionsSequence.Length;
        for (var t = 0; t < length; t++)
        {
            var action = genome.ActionsSequence[t];
            switch (action)
            {
                case GenomeAction.DontPush:
                    break;
                case GenomeAction.PushUp:
                    v.Y += dv;
                    break;
                case GenomeAction.PushDown:
                    v.Y -= dv;
                    break;
                case GenomeAction.PushLeft:
                    v.X -= dv;
                    break;
                case GenomeAction.PushRight:
                    v.X += dv;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            pos += v * _config.dt;

            if (pos.X < 0 || 1 < pos.X || pos.Y < 0 || 1 < pos.Y)
            {
                return -1000;
            }

            foreach (var circle in _config.circles)
            {
                var dx = pos.X - circle.X;
                var dy = pos.Y - circle.Y;
                if (Math.Sqrt(dx * dx + dy * dy) < circle.R)
                    return -500;
            }

            if ((pos - Vector2.One).Length() < 1E-6)
            {
                return length - t;
            }
        }

        return -(pos - Vector2.One).Length();
    }

    public double MaxPossibleFitness()
    {
        return 1_000_000;
    }
}