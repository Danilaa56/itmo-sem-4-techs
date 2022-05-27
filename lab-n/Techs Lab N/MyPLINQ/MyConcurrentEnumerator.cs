using System.Collections;

namespace MyPLINQ;

public class MyConcurrentEnumerator<T> : IEnumerator<T>
{
    private IEnumerator<T> _enumerator;
    private Dictionary<int, T?> _current = new Dictionary<int, T?>();

    public MyConcurrentEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator;
    }

    public bool MoveNext()
    {
        var id = Environment.CurrentManagedThreadId;
        T current;
        lock (_enumerator)
        {
            if (!_enumerator.MoveNext())
                return false;
            current = _enumerator.Current;
        }

        _current[id] = current;
        return true;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public T Current
    {
        get
        {
            var id = Environment.CurrentManagedThreadId;
            if (!_current.TryGetValue(id, out var result))
                return default;

            _current.Remove(id);
            return result;
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        _enumerator.Dispose();
    }
    
}