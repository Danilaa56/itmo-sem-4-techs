namespace MyPLINQ;

public class ParallelCollection<TSource>
{
    private readonly List<TSource> _storage;

    public ParallelCollection(IEnumerable<TSource> enumerable)
    {
        _storage = enumerable.ToList();
    }

    public ParallelCollection<TResult> Select<TResult>(Func<TSource, TResult> mapper)
    {
        var list = new List<TResult>(_storage.Capacity);
        ProcessParallel(src =>
        {
            var res = mapper(src!);
            lock (list)
            {
                list.Add(res);
            }
        });
        return new ParallelCollection<TResult>(list);
    }

    public ParallelCollection<TSource> Where(Func<TSource, bool> predicate)
    {
        var list = new List<TSource>();
        ProcessParallel(src =>
        {
            if (predicate(src!))
                lock (list)
                {
                    list.Add(src);
                }
        });
        return new ParallelCollection<TSource>(list);
    }

    public ParallelCollection<TResult> SelectMany<TResult>(Func<TSource, IEnumerable<TResult>> mapper)
    {
        var list = new List<TResult>();
        ProcessParallel(src =>
        {
            var res = mapper(src!);
            lock (list)
            {
                list.AddRange(res);
            }
        });
        return new ParallelCollection<TResult>(list);
    }

    public bool Any(Func<TSource, bool> predicate)
    {
        var continueResearch = true;
        var enumerator = new ConcurrentEnumerator<TSource>(_storage.GetEnumerator());
        DoParallel(Environment.ProcessorCount, () =>
        {
            while (enumerator.TryMoveNext(out var src) && continueResearch)
            {
                if (predicate(src!))
                    continueResearch = false;
            }
        });
        return !continueResearch;
    }

    public bool All(Func<TSource, bool> predicate)
    {
        var continueResearch = true;
        var enumerator = new ConcurrentEnumerator<TSource>(_storage.GetEnumerator());
        DoParallel(Environment.ProcessorCount, () =>
        {
            while (enumerator.TryMoveNext(out var src) && continueResearch)
            {
                if (!predicate(src!))
                    continueResearch = false;
            }
        });
        return continueResearch;
    }

    public List<TSource> ToList()
    {
        return _storage;
    }

    private void ProcessParallel(Action<TSource> action)
    {
        var enumerator = new ConcurrentEnumerator<TSource>(_storage.GetEnumerator());
        DoParallel(Environment.ProcessorCount, () =>
        {
            while (enumerator.TryMoveNext(out var dest))
            {
                action(dest!);
            }
        });
    }

    private static void DoParallel(int threadsCount, ThreadStart task)
    {
        var threads = new Thread[threadsCount];
        for (var i = 0; i < threadsCount; i++)
        {
            threads[i] = new Thread(task);
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}