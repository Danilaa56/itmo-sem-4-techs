namespace GenAlg.Common;

public interface IFitnessCalculator<in T>
{
    double Calculate(T genome);
    double MaxPossibleFitness();
}