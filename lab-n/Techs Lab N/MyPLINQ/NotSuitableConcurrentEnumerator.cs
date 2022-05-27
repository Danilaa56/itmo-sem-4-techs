namespace MyPLINQ;

public class NotSuitableConcurrentEnumerator<TSource>
{
    private IEnumerator<TSource> _enumerator;

    public NotSuitableConcurrentEnumerator(IEnumerator<TSource> enumerator)
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