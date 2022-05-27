namespace GenAlg.Common;

public interface IGenome<T> : IDisposable where T : IGenome<T>
{
    T Clone();
    (T, T) Cross(T other);
    T Mutate();
}