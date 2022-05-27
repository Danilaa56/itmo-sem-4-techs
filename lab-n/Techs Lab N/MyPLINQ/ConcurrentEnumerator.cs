namespace MyPLINQ;

public class ConcurrentEnumerator<TSource>
{
    private readonly IEnumerator<TSource> _enumerator;

    public ConcurrentEnumerator(IEnumerator<TSource> enumerator)
    {
        _enumerator = enumerator;
    }

    public bool TryMoveNext(out TSource? dest)
    {
        lock (this)
        {
            if (!_enumerator.MoveNext())
            {
                dest = default;
                return false;
            }

            dest = _enumerator.Current;

            return true;
        }
    }
}