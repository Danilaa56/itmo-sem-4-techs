using System.Collections;

namespace MyPLINQ;

public class MyParallelQuery<TSource> : IEnumerable<TSource>
{
    private IEnumerator<TSource> _enumerator;

    public MyParallelQuery(IEnumerable<TSource> source)
    {
        _enumerator = new MyConcurrentEnumerator<TSource>(source.GetEnumerator());
    }

    private MyParallelQuery(IEnumerator<TSource> enumerator)
    {
        _enumerator = enumerator;
    }

    public MyParallelQuery<TResult> Select<TResult>(Func<TSource, TResult> f)
    {
        return new MyParallelQuery<TResult>(Enumerable.Select(this, f).GetEnumerator());
    }
    
    public MyParallelQuery<TSource> Where(Func<TSource, bool> f)
    {
        return new MyParallelQuery<TSource>(Enumerable.Where(this, f).GetEnumerator());
    }
    //
    // public MyParallelQuery<TResult> Select<TResult>(Func<TSource, TResult> f)
    // {
    //     return new MyParallelQuery<TResult>(Enumerable.Select(this, f).GetEnumerator());
    // }
    //
    // public MyParallelQuery<TResult> Select<TResult>(Func<TSource, TResult> f)
    // {
    //     return new MyParallelQuery<TResult>(Enumerable.Select(this, f).GetEnumerator());
    // }

    // private MyParallelQuery(IReadOnlyCollection<TSource> data)
    // {
    //     _data = data;
    // }

    public List<TSource> ToList()
    {
        var list = new List<TSource>();
    
        // var enumerator = GetEnumerator();
    
        DoParallel(Environment.ProcessorCount, () =>
        {
            while (_enumerator.MoveNext())
            {
                var obj = _enumerator.Current;
                
                lock (list)
                {
                    list.Add(obj);
                }
            }
        });
        
        return list;
    }

    // public MyParallelQuery<TResult> Select<TResult>(Func<TSource, TResult> f)
    // {
    // var list = new List<TResult>();
    //
    // var enumerator = _data.GetEnumerator();
    //
    // DoParallel(Environment.ProcessorCount, () =>
    // {
    //     while (true)
    //     {
    //         TSource src;
    //         lock (enumerator)
    //         {
    //             if (!enumerator.MoveNext())
    //                 break;
    //             src = enumerator.Current;
    //         }
    //
    //         var res = f(src);
    //         lock (list)
    //         {
    //             list.Add(res);
    //         }
    //     }
    // });
    //
    // return new MyParallelQuery<TResult>(list);
    // }

    // private interface IConcurrentEnumerator<T>
    // {
    //     bool TryMoveNext(out T? dest);
    // }
    //
    // public class ConcurrentEnumerator<T> : IConcurrentEnumerator<T>
    // {
    //     private IEnumerator<> _enumerator
    //     
    //     public ConcurrentEnumerator()
    //     {
    //
    //     }
    //     
    //     public bool TryMoveNext(out T? dest)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
    //
    // IConcurrentEnumerator<T>
    // {
    //     public 
    //     {
    //         lock (_enumerator)
    //         {
    //             if (!_enumerator.MoveNext())
    //             {
    //                 dest = default;
    //                 return false;
    //             }
    //
    //             dest = _enumerator.Current;
    //             return true;
    //         }
    //     }
    // }
    //
    // private class ConcurrentEnumerator<T>
    // {
    //     private readonly IEnumerator<T> _enumerator;
    //
    //     public ConcurrentEnumerator(IEnumerator<T> enumerator)
    //     {
    //         _enumerator = enumerator;
    //     }
    //
    //     public bool TryMoveNext(out T? dest)
    //     {
    //         lock (_enumerator)
    //         {
    //             if (!_enumerator.MoveNext())
    //             {
    //                 dest = default;
    //                 return false;
    //             }
    //
    //             dest = _enumerator.Current;
    //             return true;
    //         }
    //     }
    // }
    
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

    public IEnumerator<TSource> GetEnumerator()
    {
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}